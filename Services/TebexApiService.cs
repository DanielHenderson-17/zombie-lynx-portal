namespace ZombieLynxPortal.Services
{
    public class TebexApiService : ITebexApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TebexApiService> _logger;
        private readonly string _apiKey;
        private readonly string _projectId;

        public TebexApiService(HttpClient httpClient, IConfiguration configuration, ILogger<TebexApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiKey = configuration["Tebex:ApiKey"];
            _projectId = configuration["Tebex:ProjectId"];
        }

        public async Task<string> GetAllPackagesAsync()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://headless.tebex.io/packages")
            };

            request.Headers.Add("Authorization", $"Bearer {_apiKey}");
            request.Headers.Add("X-Tebex-Project", _projectId);

            _logger.LogInformation($"Requesting URL: {request.RequestUri}");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Error: {response.StatusCode} - {errorContent}");
                throw new HttpRequestException($"Error: {response.StatusCode} - {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Response: {content}");

            return content;
        }
    }
}
