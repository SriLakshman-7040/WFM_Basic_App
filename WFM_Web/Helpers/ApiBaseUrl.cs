namespace WFM_Web.Helpers
{
    public class ApiBaseUrl
    {
        public HttpClient InitialClientMethod()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7276/");
            return client;
        }
    }
}
