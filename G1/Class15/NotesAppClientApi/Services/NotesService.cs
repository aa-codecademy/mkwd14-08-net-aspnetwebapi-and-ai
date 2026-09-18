using Microsoft.AspNetCore.Http.HttpResults;
using NotesAppClientApi.Models;
using System.Net.Http.Headers;

namespace NotesAppClientApi.Services;

/// <summary>
/// Everything this API knows about the Notes API lives here - the URLs, the JSON
/// and the token. It never creates an HttpClient: one arrives through the
/// constructor, and WHERE it comes from is what the three ways are about.
/// </summary>
public class NotesService
{
    private readonly HttpClient _httpClient;

    // GOOD WAY !!!
    // Typed client. Registered by AddHttpClient<NotesService>(...), so the factory
    // builds the HttpClient with BaseAddress and Timeout already applied and hands
    // it in here. The service asks for what it needs and nothing else - no factory,
    // no names, no configuration.
    public NotesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // OK WAY - the named-client variant.
    // Use it when ONE class needs to talk to several different APIs, so a single injected HttpClient wouldn't be enough;
    // each is registered with AddHttpClient("SomeName", ...) and pulled out by name.
    //public NotesService(IHttpClientFactory httpClientFactory)
    //{
    //    _httpClient = httpClientFactory.CreateClient("NotesServiceClient");
    //}

    /// <summary>Logs in and keeps the token for every later call.</summary>
    public async Task LoginAsync(string username, string password)
    {
        LoginRequest credentials = new LoginRequest
        {
            Username = username,
            Password = password
        };

        // BAD WAY !!!
        // https://localhost:7144/api/auth/login
        // Each `new HttpClient()` opens its own connection pool. Disposed per call ->
        // sockets pile up in TIME_WAIT and the port range runs out under load; kept in
        // a static field instead -> the connection never notices a DNS change. On top
        // of that the URL is hard-coded, so it can't differ between dev and prod.
        //HttpClient httpClient = new HttpClient();
        //httpClient.BaseAddress = new Uri("https://localhost:7144/");

        // Relative path, because BaseAddress was set at registration. PostAsJsonAsync
        // serializes the object and sets Content-Type: application/json for us.
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/auth/login", credentials);

        // Throws HttpRequestException on any non-2xx - including a 401 for wrong credentials
        response.EnsureSuccessStatusCode();

        LoginResponse? login = await response.Content.ReadFromJsonAsync<LoginResponse>();

        // Sets the header once on THIS HttpClient instance, so later calls don't have
        // to repeat it. The instance is created per NotesService, and NotesService is
        // scoped -> the token lives exactly as long as the current request. 
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.Token);
    }

    public async Task<List<NoteDto>> GetNotesAsync()
    {
        //HttpClient httpClient = new HttpClient();
        //httpClient.BaseAddress = new Uri("https://localhost:7144/");
        List<NoteDto> notes = await _httpClient.GetFromJsonAsync<List<NoteDto>>("api/notes");

        return notes ?? new List<NoteDto>();
    }
}
