using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;

namespace WebApi.Services.SMS
{
    public class SendSMSService
    {
        public SendSMSService()
        {

        }
        public async Task<bool> SendAsync()
        {
            string token = "EAAITuneJoxUBOxaDD96NLTQyOYTZBrMp8ZAaeQ85bPyy6CYhpZC3ZCET5JBXkQ7WYFBjHaQMWjjWZAZBgGSotVjZCVyfeNMJQQ8YTwHmicNEnqTt0bsHvTRFvYxB6X1NxLk53yzvwK0MUJDnsoUY06oINJG0aktFUNq14h1hEoY68uiamhOoQVetnaOsCQJBufaW5309FHnrEgxggj4iQZDZD";
            string idTelefono = "133301529875779";
            string telefono = "573157690579";
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://graph.facebook.com/v15.0/" + idTelefono + "/messages");
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Content = new StringContent("{ \"messaging_product\": \"whatsapp\", \"to\": \"" + telefono + "\", \"type\": \"template\", \"template\": { \"name\": \"hello_world\", \"language\": { \"code\": \"en_US\" } } }");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return !responseBody.IsNullOrEmpty();
        }

    }

  
}
