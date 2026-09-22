try
{
    //Both the console app and the test api must be alive - started
    using HttpClient client = new HttpClient();
    HttpResponseMessage response = client.GetAsync("https://localhost:7066/api/Test/testUser").Result;
    string bodyContent = response.Content.ReadAsStringAsync().Result;

    Console.WriteLine(bodyContent);

}catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}
