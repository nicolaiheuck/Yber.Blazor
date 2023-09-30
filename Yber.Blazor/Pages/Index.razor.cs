using System.Text.Json;
using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Radzen.Blazor;
using Yber.Blazor.Shared.Modal;
using Yber.Services.DTO;

namespace Yber.Blazor.Pages;

public partial class Index : IDisposable
{
    [Inject] public IToastService? ToastService { get; set; }
    [Inject] public Toolbelt.Blazor.I18nText.I18nText I18nText { get; set; }
    private I18nText.LanguageTable _languageTable = new ();
    [Inject] public IJSRuntime JsRuntime { get; set; }
    private IJSRuntime? _jsRuntime;
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    private AuthenticationStateProvider _authentication;
    private AuthenticationState _authenticationState;
    [Inject] public HttpClient MyHttpClient { get; set; }
    private HttpClient _httpClient;
    [Inject] public IConfiguration AppSettings { get; set; }
    [Inject] public IModalService Modal { get; set; } = default!;
    private string _StudentLocationJson { get; set; }
    private string _ActualStucentLocationJson { get; set; }
    private CalculatedRouteDTO _CalculatedRoute { get; set; }
    private string _UserName { get; set; }
    private string _FirstName { get; set; }
    private List<StudentDTO> _liftStudents { get; set; }
    private List<StudentDTO> _driverStudents { get; set; }
    private List<StudentDTO> _studentDataGrid { get; set; } = new();
    private StudentDTO _actualStudent { get; set; }
	private RadzenDataGrid<StudentDTO>? _LiftGiverGrid;

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

             
        _liftStudents = (await _httpClient.GetFromJsonAsync<List<StudentDTO>>($"{AppSettings["YberAPIBaseURI"]}GetStudentLift"))!;
        _driverStudents = (await _httpClient.GetFromJsonAsync<List<StudentDTO>>($"{AppSettings["YberAPIBaseURI"]}GetStudentDriver"))!;
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
        else { return false; } // RETURN
        
        var studentsCoordinates = new List<StudentCoordinateDTO>();
        if (_actualStudent.Lift_Give == true)
        {
            foreach (var student in _liftStudents)
            {
                studentsCoordinates.Add(new StudentCoordinateDTO
                {
                    name = student.First_Name,
                    latlng = new latlng
                    {
                        lat = student.LatLng[0],
                        lng = student.LatLng[1]
                    }
                });
            }
        }
        else
        {
            foreach (var student in _driverStudents)
            {
                studentsCoordinates.Add(new StudentCoordinateDTO
                {
                    name = student.First_Name,
                    latlng = new latlng
                    {
                        lat = student.LatLng[0],
                        lng = student.LatLng[1]
                    }
                });
            }
        }

        var studentCoord = new StudentCoordinateDTO
        {
            name = _actualStudent.First_Name,
            latlng = new latlng
            {
                lat = _actualStudent.LatLng[0],
                lng = _actualStudent.LatLng[1]
            }
        };
        
        _StudentLocationJson = JsonSerializer.Serialize(studentsCoordinates);
        _ActualStucentLocationJson = JsonSerializer.Serialize(studentCoord);
        return true;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await GetUserInfoFromAPI();
            _authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            _UserName = _authenticationState.User.Identity.Name;
            if (_UserName == null)
            {
                var parametersNotInDb = new ModalParameters().Add(nameof(InfoMessage.Message), $"Hello");
                var optionsNotInDb = new ModalOptions { UseCustomLayout = true };
                var messageFormNotInDb = Modal.Show<InfoMessage>("Info", parametersNotInDb, optionsNotInDb);
                var resulNotInDbt = await messageFormNotInDb.Result;
            }
            var currUser = await _httpClient.GetFromJsonAsync<StudentDTO>(
                $"{AppSettings["YberAPIBaseURI"]}GetStudentFromName?studentName={_UserName}");
            await UpdateUIInformationAsync((bool)currUser.Lift_Give);
        }
    }

	async Task Accept(int studentId)
    {
        var requestUri = $"{AppSettings["YberAPIBaseURI"]}GetStudentsFromID?studentID={studentId}"; // Weirdest exception ever o_O
        var student = await _httpClient.PostAsync(requestUri, null);
        var me = _actualStudent;
    }

	async Task Reject(int studentId)
    {
        var requestUri = $"{AppSettings["YberAPIBaseURI"]}GetStudentsFromID?studentID={studentId}"; // Weirdest exception ever o_O
        var student = await _httpClient.PostAsync(requestUri, null);
        var me = _actualStudent;
    }

    async Task ChangeDriveLift(bool drive)
    {
        await UpdateUIInformationAsync(drive);
    }

    // ReSharper disable once InconsistentNaming
    async Task UpdateUIInformationAsync(bool drive = false) 
    {
        var currentUser = await _httpClient.GetFromJsonAsync<StudentDTO>(
            $"{AppSettings["YberAPIBaseURI"]}GetStudentFromName?studentName={_UserName}");
        if (currentUser == null) return;
        _actualStudent = currentUser;
        switch (drive)
        {
            case true:
                _actualStudent.Lift_Give = true;
                _actualStudent.Lift_Take = false;
                _liftStudents.Remove(_liftStudents.FirstOrDefault(u => u.Username == _UserName));
                _studentDataGrid = _liftStudents;
                break;
            case false:
                _actualStudent.Lift_Give = false;
                _actualStudent.Lift_Take = true;
                _driverStudents.Remove(_driverStudents.FirstOrDefault(u => u.Username == _UserName));
                _studentDataGrid = _driverStudents;
                break;
        }
        
        await _httpClient.PostAsync(
            $"{AppSettings["YberAPIBaseURI"]}WantLift?studentID={currentUser.Id}&accept={_actualStudent.Lift_Take}", null);
        await _httpClient.PostAsync(
            $"{AppSettings["YberAPIBaseURI"]}OfferDrive?studentID={currentUser.Id}&accept={_actualStudent.Lift_Give}", null);
        
        
        _LiftGiverGrid.Data = _studentDataGrid;
        await _LiftGiverGrid.Reload();
        _jsRuntime = JsRuntime;
        if (await GetInfoFromAPIAsync() == false) return;
        await _jsRuntime.InvokeVoidAsync("initMap", _StudentLocationJson, _ActualStucentLocationJson, _CalculatedRoute.EncodedPolyline);
    }
    
	public void Dispose() { }
}