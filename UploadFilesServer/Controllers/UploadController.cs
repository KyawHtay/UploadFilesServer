using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UploadFilesServer.Models;
using UploadFilesServer.Helper;
using UploadFilesServer.Context;

namespace UploadFilesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : Controller
    {

        private readonly UserContext  _context;

        public UploadController(UserContext context)
        {
            _context = context;
        }

        private void SaveToDB(string fullFilename)
        {
            List<Person> persons = FileHelper.ReadAllLines(fullFilename)
                .Skip(1)
                .Where(s => s != "")
                .Select(s => s.Split(new[] { ',' }))
                .Select(a => new Person
                {
                    FirstName = a[0],
                    LastName = a[1],
                    Company = a[2],
                    Address = a[3],
                    City = a[4],
                    County = a[5],
                    Postal = a[6],
                    Phone1 = a[7],
                    Phone2 = a[8],
                    Email = a[9],
                    Web = a[10]
            
                 })
                .ToList();

            foreach (var person in persons)
            {
                _context.Add(person);
                _context.SaveChanges();
            }

        }
        [HttpPost,DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources","files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath,FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    SaveToDB(Path.Combine(fullPath));
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500,$"Internal server error: {ex}");
            }
         
        }
    }
}
