using AutoMapper;
using AutoMapper.QueryableExtensions;
using Korzh.EasyQuery.Linq;
using LinqKit;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Data.Entity;
using VsiriusSystem._Repositories.Interfaces;
using VsiriusSystem._Services.Interfaces;
using VsiriusSystem.Dtos;
using VsiriusSystem.Helpers.Params;
using VsiriusSystem.Helpers.Utilities;

namespace VsiriusSystem._Services.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logsRepository;
        private readonly IMapper _mapper;
        private ILogger<LogService> _logger;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IConfiguration _configuration;
        public LogService(ILogRepository logsRepository, IMapper mapper, ILogger<LogService> logger, MapperConfiguration mapperConfiguration, IConfiguration configuration)
        {
            _logsRepository = logsRepository;
            _mapper = mapper;
            _logger = logger;
            _mapperConfiguration = mapperConfiguration;
            _configuration = configuration;
        }

        public async Task<PaginationUtility<LogDto>> GetListLog(PaginationParam pagination, LogParams logParam, string? keyword, bool isPaging = true)
        {
            var predicate = GetPredicate(logParam);
            var data = _logsRepository.FindAll(predicate)
                                      .FullTextSearchQuery(keyword)
                                      .ProjectTo<LogDto>(_mapperConfiguration).AsNoTracking().OrderByDescending(x => x.TimeStamp);
            return await PaginationUtility<LogDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize, isPaging);
        }

        public async Task<LogDto> GetLog(Guid logId)
        {
            var data = await _logsRepository.FindSingle(x => x.LogId == logId);

            return _mapper.Map<LogDto>(data);
        }

        public async Task<List<string>> FilterProjectName()
        {
            var data = _logsRepository.FindAll().Select(x => x.ProjectName).Distinct().ToList(); 
            return data;
        }

        public async Task<List<string>> FilterURL(string projectName)
        {
            var data = _logsRepository.FindAll(x => x.ProjectName == projectName).Select(x => x.URL).Distinct().ToList();
            return data;
        }
        public Dictionary<int, string> GetLevelLog()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            result.Add((int)LogLevel.Trace, LogLevel.Trace.ToString());
            result.Add((int)LogLevel.Debug, LogLevel.Debug.ToString());
            result.Add((int)LogLevel.Information, LogLevel.Information.ToString());
            result.Add((int)LogLevel.Warning, LogLevel.Warning.ToString());
            result.Add((int)LogLevel.Error, LogLevel.Error.ToString());
            result.Add((int)LogLevel.Critical, LogLevel.Critical.ToString());
            result.Add((int)LogLevel.None, LogLevel.None.ToString());
            return result;
        }

        public async Task<OperationResult> CreateLog(LogDto logViewModel)
        {
            try
            {
                _logger.BeginScope("{LogId}", Guid.NewGuid());
                _logger.BeginScope("{Message}", logViewModel.Message);
                _logger.BeginScope("{TimeStamp}", DateTime.Now);
                _logger.BeginScope("{URL}", logViewModel.URL);
                _logger.BeginScope("{ProjectName}", logViewModel.ProjectName);
                _logger.BeginScope("{Level}", logViewModel.Level);
                _logger.BeginScope("{Exception}", logViewModel.Exception);
                _logger.BeginScope("{CorrelationId}", logViewModel.CorrelationId);
                _logger.LogError(logViewModel.Message);

                var sinkOpts = new MSSqlServerSinkOptions();
                sinkOpts.TableName = "Logs";

                Serilog.Log.Logger = new LoggerConfiguration()
                 .WriteTo.File(LocationLog(logViewModel.ProjectName, logViewModel.URL),
                  outputTemplate: "[TimeStamp: " + DateTime.Now + "]" + "\n\n" + "Message: " + logViewModel.Message + "\n\n" + "ControllerName: " + logViewModel.URL + "\n\n" +
                                    "ProjectName: " + logViewModel.ProjectName + "\n\n" + "Level: " + logViewModel.Level + "\n\n" + "Exception: " + logViewModel.Exception + "\n\n" + "CorrelationId: " + logViewModel.CorrelationId + "\n\n")
                    .WriteTo.MSSqlServer(
                        connectionString: _configuration.GetSection("ConnectionStrings")["DefaultConnection"],
                        sinkOptions: sinkOpts,
                        columnOptions: new ColumnOptions()
                    )
                    .CreateLogger();


                return new OperationResult { Success = true, Message = "Save Log Successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex.Message);
                return new OperationResult { };
            }
        }

        private ExpressionStarter<Models.Log> GetPredicate(LogParams logParams)
        {
            var predicate = PredicateBuilder.New<Models.Log>(true);

            if (!string.IsNullOrWhiteSpace(logParams.URL) || logParams.URL != null)
                predicate.And(x => x.URL.Contains(logParams.URL));

            if (logParams.Level != null)
                predicate.And(x => x.Level == logParams.Level.ToString());

            if (!string.IsNullOrWhiteSpace(logParams.ProjectName))
                predicate.And(x => x.ProjectName.Contains(logParams.ProjectName));

            if (!string.IsNullOrWhiteSpace(logParams.CorrelationId))
            {
                Guid guidOutput;
                Guid.TryParse(logParams.CorrelationId, out guidOutput);
                predicate.And(x => x.CorrelationId == guidOutput);
            }

            if (logParams.TimeStampFrom != null && logParams.TimeStampTo != null)
            {
                var startTime = Convert.ToDateTime(logParams.TimeStampFrom + " 00:00:00.000");
                var endTime = Convert.ToDateTime(logParams.TimeStampTo + " 23:59:59.997");

                if (startTime != null && endTime != null)
                {
                    predicate.And(x => x.TimeStamp >= startTime
                                       && x.TimeStamp <= endTime);
                }
                else if (startTime != null)
                {
                    predicate.And(x => x.TimeStamp >= startTime);
                }
                else if (endTime != null)
                {
                    predicate.And(x => x.TimeStamp <= endTime);
                }
            }

            return predicate;
        }

        private string LocationLog(string projectName, string URL)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            return currentDirectory + "\\Storage\\Logs\\" + projectName + "\\" + URL + "_" + DateTime.Now.ToString("dd-MM-yyyy-HHmmss") + ".txt";
        }
    }
}
