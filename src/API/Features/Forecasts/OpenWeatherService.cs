namespace API.Features.Forecasts;

public interface IOpenWeatherService
{
    public Task GetOpenWeatherData(decimal latitude, decimal longitude);
}

public class OpenWeatherService : IOpenWeatherService
{
    public async Task GetOpenWeatherData(decimal latitude, decimal longitude)
    {
        // OpenWeatherMap API endpoint
        string apiUrl = "https://api.openweathermap.org/data/3.0/onecall?lat={lat}&lon={lon}&appid={API key}";

        // Replace {lat}, {lon}, and {API key} with your actual latitude, longitude, and API key respectively
        string lat = latitude.ToString();
        string lon = longitude.ToString();
        string apiKey = "...";

        // Replace placeholders in the API URL with actual values
        apiUrl = apiUrl.Replace("{lat}", lat).Replace("{lon}", lon).Replace("{API key}", apiKey);

        // Create an instance of HttpClient
        using (HttpClient client = new HttpClient())
        {
            try
            {
                // Make a GET request to the API
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the content of the response as JSON string
                    string jsonResult = await response.Content.ReadAsStringAsync();

                    // You can now parse the JSON string as needed
                    Console.WriteLine(jsonResult);
                }
                else
                {
                    // If the request was not successful, print the status code
                    Console.WriteLine("Failed to fetch data. Status code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, print the error message
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
