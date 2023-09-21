using System.Text.Json;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Microsoft.JSInterop;
using Toolbelt.Blazor.HotKeys2;
using Yber.Services.DTO;
using Yber.Services.Interfaces;
using static Microsoft.JSInterop.IJSRuntime;

namespace Yber.Blazor.Pages;

public partial class Index : IDisposable
{
    [Inject] public IToastService? ToastService { get; set; }
    [Inject] public HotKeys HotKeys { get; set; }
    [Inject] public Toolbelt.Blazor.I18nText.I18nText I18nText { get; set; }
    [Inject] public IYberService YberService { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }
    private HotKeysContext? _hotKeysContext;
    private I18nText.LanguageTable _languageTable = new ();
    private IJSRuntime? _jsRuntime;
    private IYberService _yberService;
    public string StudentLocationJson { get; set; }
    public string ActualStucentLocationJson { get; set; }
    public CalculatedRouteDTO CalculatedRoute { get; set; }
    private string UserName { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _languageTable = await I18nText.GetTextTableAsync<I18nText.LanguageTable>(this);
        _hotKeysContext = HotKeys.CreateContext()
            .Add(Code.F8, Toaster);
    }

    private async Task GetInfoFromAPIAsync()
    {
        
        // TODO USE API DIPSHIT
        UserName = "elvn4";
        var students = await _yberService.GetStudentRequesterLocationInfoAsync();
        var actualStudent = students.Find(s => s.Username == UserName);
        CalculatedRoute = await _yberService.GetEncodedRouteLineAsync(UserName); // TODO FIX USERNAME FROM AZURE
        var studentsCoordinates = new List<StudentCoordinateDTO>();
        foreach (var student in students)
        {
            studentsCoordinates.Add(new StudentCoordinateDTO()
            {
                lat = student.LatLng[0],
                lng = student.LatLng[1]
            });
        }

        var studentCoord = new StudentCoordinateDTO()
        {
            lat = actualStudent.LatLng[0],
            lng = actualStudent.LatLng[1]
        };
        
        StudentLocationJson = JsonSerializer.Serialize(studentsCoordinates);
        ActualStucentLocationJson = JsonSerializer.Serialize(studentCoord);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            
            _jsRuntime = JsRuntime;
            _yberService = YberService;
            await GetInfoFromAPIAsync();
            await _jsRuntime.InvokeVoidAsync("initMap", StudentLocationJson, ActualStucentLocationJson, CalculatedRoute.EncodedPolyline);
    
        }
    }
    void Toaster() => ToastService.ShowInfo("Congratulations, you just pressed hotkey: F8");

    public void Dispose()
    {
        _hotKeysContext?.Dispose();
    }
}