public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddSingleton(x => new BlobServiceClient(Configuration.GetConnectionString("AzureStorage:ConnectionString")));
}
