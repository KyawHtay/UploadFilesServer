using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UploadFilesServer.Context;

namespace UploadFilesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {

        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _context.person.ToList();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
