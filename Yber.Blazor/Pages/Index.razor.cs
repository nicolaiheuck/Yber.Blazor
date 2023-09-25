using System.Net;
using System.Text.Json;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Toolbelt.Blazor.HotKeys2;
using Yber.Services.DTO;

namespace Yber.Blazor.Pages;

public partial class Index : IDisposable
{
    [Inject] public IToastService? ToastService { get; set; }
    [Inject] public HotKeys HotKeys { get; set; }
    [Inject] public Toolbelt.Blazor.I18nText.I18nText I18nText { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] public IConfiguration AppSettings { get; set; }

    [Inject] public HttpClient MyHttpClient { get; set; }
    
    private HotKeysContext? _hotKeysContext;
    private I18nText.LanguageTable _languageTable = new ();
    private IJSRuntime? _jsRuntime;
    private HttpClient _httpClient;
    private AuthenticationStateProvider _authentication;
    private AuthenticationState _authenticationState;
    private string _StudentLocationJson { get; set; }
    private string _ActualStucentLocationJson { get; set; }
    private CalculatedRouteDTO _CalculatedRoute { get; set; }
    private string _UserName { get; set; }
    private string _FirstName { get; set; }
    
    private List<StudentDTO> _students { get; set; }
    private StudentDTO _actualStudent { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetUserInfoFromAPI();
        _languageTable = await I18nText.GetTextTableAsync<I18nText.LanguageTable>(this);
    }

    // ReSharper disable once InconsistentNaming
    private async Task GetUserInfoFromAPI()
    {
        _authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        
        // TODO SSL ERRORS
        _httpClient = MyHttpClient;
        
        if (_authenticationState.User.Identity == null) return;
        _UserName = _authenticationState.User.Identity.Name;
        
        _students = await _httpClient.GetFromJsonAsync<List<StudentDTO>>(
            $"{AppSettings["YberAPIBaseURI"]}GetStudentLift");
        _actualStudent = _students.Find(s => s.Username == _UserName);

        _FirstName = _actualStudent == null ? "Null" : _actualStudent.First_Name;
    }

    // ReSharper disable once InconsistentNaming
    private async Task GetInfoFromAPIAsync()
    {
        _CalculatedRoute = (await (await _httpClient.PostAsync
        ($"{AppSettings["YberAPIBaseURI"]}GetStudentRoute?studentName={_UserName}",null))
            .Content.ReadFromJsonAsync<CalculatedRouteDTO>())!;
        
        var studentsCoordinates = new List<StudentCoordinateDTO>();
        
        foreach (var student in _students)
        {
            studentsCoordinates.Add(new StudentCoordinateDTO
            {
                lat = student.LatLng[0],
                lng = student.LatLng[1]
            });
        }

        var studentCoord = new StudentCoordinateDTO
        {
            lat = _actualStudent.LatLng[0],
            lng = _actualStudent.LatLng[1]
        };
        
        _StudentLocationJson = JsonSerializer.Serialize(studentsCoordinates);
        _ActualStucentLocationJson = JsonSerializer.Serialize(studentCoord);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsRuntime = JsRuntime;
            await GetInfoFromAPIAsync();
            await _jsRuntime.InvokeVoidAsync("initMap", _StudentLocationJson, _ActualStucentLocationJson, _CalculatedRoute.EncodedPolyline);
        }
    }

    public void Dispose()
    {
        _hotKeysContext?.Dispose();
    }
}