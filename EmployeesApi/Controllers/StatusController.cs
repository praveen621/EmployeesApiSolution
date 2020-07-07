using EmployeesApi.Models;
using EmployeesApi.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace EmployeesApi.Controllers
{
    public class StatusController : ControllerBase
    {
        ISystemTime Time;

        public StatusController(ISystemTime time)
        {
            Time = time ?? throw new ArgumentNullException(nameof(time));
        }

        //GET/status
        [HttpGet("status")]
        public ActionResult GetStatus()
        {
            var resp = new StatusResponse
            {
                Status = "Good",
                CheckedBy = "PSN",
                LastChecked = Time.GetCurrent()
            };
            return Ok(resp);
        }

        [HttpGet("books/{bookId:int}")]
        [SwaggerResponse(200, "OK response-200")]
        [SwaggerResponse(404,"Cannot find error-404",typeof(ErrorResponse))]
        public ActionResult GetABook(int bookId)
        {
            if(bookId % 2 == 0)
            return Ok($"Getting book info for id {bookId}");
            return NotFound();
        }

        [HttpGet("blogs/{year:int}/{month:int}/{day:int}")]
        public ActionResult GetBlogPost(int year, int month, int day)
        {
            return Ok($"Getting blog info for {year}/{month}/{day}");
        }

        [HttpPost("games")]
        public ActionResult AddGame([FromBody] PostGameRequest game)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok($"Added Game {game.Title} under {game.Platform} platform");
        }
    }

    public class PostGameRequest : IValidatableObject
    {
        [Required]
        [StringLength(50,ErrorMessage ="Title is too long, max 50 chars")]
        public string Title { get; set; }

        [Required]
        public string Platform { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Platform.ToLower().Equals("ps4"))
            yield return  new ValidationResult("PS4 not supported");
        }
    }

    public class StatusResponse
    {
        public string Status { get; set; }
        public string CheckedBy { get; set; }
        public DateTime LastChecked { get; set; }
    }
}
