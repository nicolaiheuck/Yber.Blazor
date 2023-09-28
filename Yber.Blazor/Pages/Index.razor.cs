using System.Net;
using System.Text.Json;
using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Radzen.Blazor;
using Toolbelt.Blazor.HotKeys2;
using Yber.Blazor.Shared.Modal;
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
    [Inject] public IModalService Modal { get; set; } = default!;

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
    
    private List<StudentDTO> _liftStudents { get; set; }
    private List<StudentDTO> _driverStudents { get; set; }
    private StudentDTO _actualStudent { get; set; }
	private RadzenDataGrid<StudentDTO>? _LiftTakerGrid;
	private RadzenDataGrid<StudentDTO>? _LiftGiverGrid;
	private StudentDTO _selectedStudent { get; set; }

	protected override async Task OnInitializedAsync()
    {
        await GetUserInfoFromAPI();
        _languageTable = await I18nText.GetTextTableAsync<I18nText.LanguageTable>(this);
    }

    // ReSharper disable once InconsistentNaming
    private async Task GetUserInfoFromAPI()
    {
        _authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        _httpClient = MyHttpClient;

        if (_authenticationState.User.Identity == null) return;
        _UserName = _authenticationState.User.Identity.Name;

             
        _liftStudents = await _httpClient.GetFromJsonAsync<List<StudentDTO>>(
            $"{AppSettings["YberAPIBaseURI"]}GetStudentLift");
        _driverStudents = await _httpClient.GetFromJsonAsync<List<StudentDTO>>(
            $"{AppSettings["YberAPIBaseURI"]}GetStudentDriver");
        _actualStudent = _liftStudents.FirstOrDefault(s => s.Username == _UserName) ?? _driverStudents.FirstOrDefault(s => s.Username == _UserName);

        _FirstName = _actualStudent == null ? "" : _actualStudent.First_Name;

    }

    // ReSharper disable once InconsistentNaming
    private async Task<bool> GetInfoFromAPIAsync()
    {
        if (!_FirstName.IsNullOrEmpty())
        {
            _CalculatedRoute = (await (await _httpClient.PostAsync
                    ($"{AppSettings["YberAPIBaseURI"]}GetStudentRoute?studentName={_UserName}", null))
                .Content.ReadFromJsonAsync<CalculatedRouteDTO>())!;
        }
        else
        {
            return false;
        };
        
        var studentsCoordinates = new List<StudentCoordinateDTO>();
        if (_actualStudent.Lift_Give == true)
        {
            foreach (var student in _liftStudents)
            {
                studentsCoordinates.Add(new StudentCoordinateDTO
                {
                    lat = student.LatLng[0],
                    lng = student.LatLng[1]
                });
            }
        }
        if (_actualStudent.Lift_Take == true)
        {
            foreach (var student in _driverStudents)
            {
                studentsCoordinates.Add(new StudentCoordinateDTO
                {
                    lat = student.LatLng[0],
                    lng = student.LatLng[1]
                });
            }
        }

        var studentCoord = new StudentCoordinateDTO
        {
            lat = _actualStudent.LatLng[0],
            lng = _actualStudent.LatLng[1]
        };
        
        _StudentLocationJson = JsonSerializer.Serialize(studentsCoordinates);
        _ActualStucentLocationJson = JsonSerializer.Serialize(studentCoord);

        return true;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            _UserName = _authenticationState.User.Identity.Name;
            if (_UserName == null)
            {
                var parametersNotInDb = new ModalParameters().Add(nameof(InfoMessage.Message), $"Hello");
                var optionsNotInDb = new ModalOptions { UseCustomLayout = true };
                var messageFormNotInDb = Modal.Show<InfoMessage>("Info", parametersNotInDb, optionsNotInDb);
                var resulNotInDbt = await messageFormNotInDb.Result;
            }


            var parameters = new ModalParameters().Add(nameof(LiftGiverTaker.Message), $"Hello {_UserName}");
            var options = new ModalOptions { UseCustomLayout = true };
            var messageForm = Modal.Show<LiftGiverTaker>("LiftGiverTaker", parameters, options);
            var result = await messageForm.Result;

            if (result.Confirmed)
            {
                if (result.Data.ToString() == "LiftGiver")
                {

                }
                if (result.Data.ToString() == "LiftTaker")
                {

                }
            }


            _jsRuntime = JsRuntime;
            if (await GetInfoFromAPIAsync() == false) return;
            await _jsRuntime.InvokeVoidAsync("initMap", _StudentLocationJson, _ActualStucentLocationJson, _CalculatedRoute.EncodedPolyline);
        }
    }

	async Task Accept()
	{

	}

	async Task Reject()
    {

    }


	public void Dispose()
    {
        _hotKeysContext?.Dispose();
    }
}