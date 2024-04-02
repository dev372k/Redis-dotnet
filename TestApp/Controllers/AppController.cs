using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using TestApp.DTOs;
using TestApp.Helpers;
using TestApp.Services;

namespace TestApp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AppController : ControllerBase
    {

        private readonly ILogger<AppController> _logger;
        private readonly ICacheService _cache;
        private readonly IUserRepo _userRepo;

        public AppController(ILogger<AppController> logger, ICacheService cache, IUserRepo userRepo)
        {
            _logger = logger;
            _cache = cache;
            _userRepo = userRepo;
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(AddUserDTO dto)
        {
            try
            {
                await _userRepo.AddUser(dto);
                bool isRemoved = _cache.Remove("users");
                return Ok(new JSONResponse { Status = true, Message = "User added successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex.InnerException.ToString() });
            }
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO dto)
        {
            try
            {
                await _userRepo.UpdateUser(dto);
                bool isRemoved = _cache.Remove("users");
                return Ok(new JSONResponse { Status = true, Message = "User updated successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex.InnerException.ToString() });
            }
        }

        [HttpGet("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            try
            {
                await _userRepo.DeleteUser(Id);
                bool isRemoved = _cache.Remove("users");
                return Ok(new JSONResponse { Status = true, Message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex.InnerException.ToString() });
            }
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(int Id)
        {
            try
            {
                var user = _userRepo.GetUser(Id);
                return Ok(new JSONResponse { Status = true, Data = user });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex.InnerException.ToString() });
            }
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                IEnumerable<GetUserDTO>? users = null;
                var cachedData = _cache.Get<IEnumerable<GetUserDTO>>("users");
                if (cachedData == null || cachedData.Count() == 0)
                {           
                    users = _userRepo.GetUsers();                 
                    _cache.Set("users", users);
                }
                else
                {
                    users = cachedData;
                }
                return Ok(new JSONResponse { Status = true, Data = users });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex.InnerException.ToString() });
            }
        }
    }

    public class JSONResponse
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDescription { get; set; }
        public object Data { get; set; }
    }
}