using Microsoft.AspNetCore.Mvc;
using VsiriusSystem._Services.Interfaces;
using VsiriusSystem.Dtos;
using VsiriusSystem.Helpers.Params;
using VsiriusSystem.Helpers.Utilities;

namespace VsiriusSystem.Controllers
{
    [Route("api/system/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private ILogService _logService;
        private IApiKeysService _apiKeysService;
        public LogController(ILogService logService, IApiKeysService apiKeysService)
        {
            _apiKeysService = apiKeysService;
            _logService = logService;
        }
        /// <summary>
        /// </summary>
        /// <remarks>
        ///"ProductApi": "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJkYXRhIjoiNTczNDkxYjktNjU4Mi00NzBhLThhZWUtYTE4MjIwY2ZhZjM1LVByb2R1Y3RBcGkiLCJuYmYiOjE2ODc1MDczNTksImV4cCI6MTY4NzUxMDk1OSwiaWF0IjoxNjg3NTA3MzU5fQ.", <br/>
        ///"OrderApi": "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJkYXRhIjoiN2MwMTgxOTMtN2I4Yi00ODNmLTllZDAtNTUxZjk5NTQ1YzI2LU9yZGVyQXBpIiwibmJmIjoxNjg3NTA3NDAzLCJleHAiOjE2ODc1MTEwMDMsImlhdCI6MTY4NzUwNzQwM30.", <br/>
        ///"CSRApi": "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJkYXRhIjoiNjE1NDYwZWEtNTE4OS00NmI1LTgyMWMtMGJkMjZmZTZiMGI0LUNTUkFwaSIsIm5iZiI6MTY4NzUwNzQyNywiZXhwIjoxNjg3NTExMDI3LCJpYXQiOjE2ODc1MDc0Mjd9.", <br/>
        ///"IdentityApi": "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJkYXRhIjoiODJmMmQ3NjAtOTlmYS00MjZkLThkZDMtMDNhYzBkYTUzODU1LUlkZW50aXR5QXBpIiwibmJmIjoxNjg3NTA3NDQ3LCJleHAiOjE2ODc1MTEwNDcsImlhdCI6MTY4NzUwNzQ0N30.", <br/>
        ///"CartApi": "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJkYXRhIjoiOTUyMzVhNTAtNjUzMy00MzdmLThhYzAtNzQ1MjgwY2RhYjg2LUNhcnRBcGkiLCJuYmYiOjE2ODc1MDc0NjcsImV4cCI6MTY4NzUxMTA2NywiaWF0IjoxNjg3NTA3NDY3fQ.", <br/>
        ///"FrontEnd": "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJkYXRhIjoiZjQyNTUxNzktMmNiYi00OTI5LThjMTgtOGJhMzc2NTUzOTAyLUZyb250RW5kIiwibmJmIjoxNjg3NTA3NDg2LCJleHAiOjE2ODc1MTEwODYsImlhdCI6MTY4NzUwNzQ4Nn0.", <br/>
        ///"BackOffice": "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJkYXRhIjoiNTI1NmZlNmQtNGYzMy00NjcxLTk2ZjQtYTIwZjRhOTFlZjg5LUJhY2tPZmZpY2UiLCJuYmYiOjE2ODc1MDc1MDIsImV4cCI6MTY4NzUxMTEwMiwiaWF0IjoxNjg3NTA3NTAyfQ." <br/>
        /// </remarks>
        /// <param name="pagination"></param>
        /// <param name="logParams"></param>
        /// <param name="keyword"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        [HttpGet("GetListLog")]
        public async Task<IActionResult> GetListLog([FromQuery] PaginationParam pagination, [FromQuery] LogParams logParams,string? keyword
                                                    , [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_apiKeysService.ValidateCredentials(clientId, clientSecret))
            {
                var res = await _logService.GetListLog(pagination, logParams, keyword);
                return Ok(res);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("GetLog")]
        public async Task<IActionResult> GetLog(Guid logId, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_apiKeysService.ValidateCredentials(clientId, clientSecret))
            {
                var res = await _logService.GetLog(logId);
                return Ok(res);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("GetLevelLog")]
        public async Task<IActionResult> GetLevelLog([FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_apiKeysService.ValidateCredentials(clientId, clientSecret))
            {
                var res = _logService.GetLevelLog();
                return Ok(res);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("FilterProjectName")]
        public async Task<IActionResult> FilterProjectName([FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_apiKeysService.ValidateCredentials(clientId, clientSecret))
            {
                var res = await _logService.FilterProjectName();
                return Ok(res);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("FilterURL")]
        public async Task<IActionResult> FilterURL(string projectName, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_apiKeysService.ValidateCredentials(clientId, clientSecret))
            {
                var res = await _logService.FilterURL(projectName);
                return Ok(res);
            }
            else
            {
                return Unauthorized();
            }
        }



        [HttpPost("WriteLogs")]
        public async Task<IActionResult> CreateLog(LogDto logViewModel, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_apiKeysService.ValidateCredentials(clientId, clientSecret))
            {
                var res = await _logService.CreateLog(logViewModel);
                return Ok(res);
            }
            else
            {
                return Unauthorized();
            }  
        }
    }
}
