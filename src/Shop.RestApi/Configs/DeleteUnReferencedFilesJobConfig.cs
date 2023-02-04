using Microsoft.Extensions.Options;
using Shop.AdminServices.DeleteUnreferencedMedias;
using Shop.Infrastructures;
using Shop.Persistence.EF;

namespace Shop.RestApi.Configs
{
    public class DeleteUnReferencedFilesJobConfig : Configuration
    {
        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            container.Configure<DeleteUnReferencedFilesJobOptions>(_ =>
            {
                _.JobStepDelay = TimeSpan.FromHours(1);
                _.ReserveFileDuration = TimeSpan.FromHours(1);
            });

            container.AddHostedService<DeleteUnReferencedFileJobService>();
        }
    }

    public class DeleteUnReferencedFilesJobOptions
    {
        public DeleteUnReferencedFilesJobOptions()
        {
            SetDefaultValues();
        }

        public TimeSpan JobStepDelay { get; set; }
        public TimeSpan ReserveFileDuration { get; set; }

        private void SetDefaultValues()
        {
            JobStepDelay = TimeSpan.FromHours(1);
            ReserveFileDuration = TimeSpan.FromHours(1);
        }
    }

    public class DeleteUnReferencedFileJobService : BackgroundService
    {
        private readonly ILogger<DeleteUnreferencedMediaAppService> _logger;
        private readonly DeleteUnReferencedFilesJobOptions _options;
        private readonly IServiceProvider _serviceProvider;

        public DeleteUnReferencedFileJobService(
            IOptions<DeleteUnReferencedFilesJobOptions> options,
            IServiceProvider serviceProvider,
            ILogger<DeleteUnreferencedMediaAppService> logger)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider
                        .GetRequiredService<EFDataContext>();
                    var timeService = scope.ServiceProvider
                        .GetRequiredService<DateTimeService>();

                    var deleteUnReferencedFilesService =
                        new DeleteUnreferencedMediaAppService(
                            dbContext, timeService);
                    var result = await deleteUnReferencedFilesService
                        .Execute(stoppingToken);

                    result.MediasDeleted.ForEach(
                    mediaDeletedId => _logger.LogInformation(
                    $"media with id :" +
                    $" {mediaDeletedId} successfully deleted"));

                    result.MediasNotDeleted.ForEach(
                    mediaNotDeleted => _logger.LogError(
                    $"Delete media with id :" +
                    $"{mediaNotDeleted.MediaNotDeletedId} failed " +
                    $"with error : {mediaNotDeleted.ExceptionMessage}"));
                }

                await Task.Delay(_options.JobStepDelay, stoppingToken);
            }
        }
    }
}
