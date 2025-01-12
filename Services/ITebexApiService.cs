namespace ZombieLynxPortal.Services
{
    public interface ITebexApiService
    {
        Task<string> GetAllPackagesAsync();
    }
}
