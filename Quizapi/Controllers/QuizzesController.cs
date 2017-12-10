using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quizapi.Models;

namespace Quizapi.Controllers
{
    [Produces("application/json")]
    [Route("api/Quizzes")]
    public class QuizzesController : Controller
    {
        private readonly QuizDbContext _context;

        public QuizzesController(QuizDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var getQuizz = _context.Quizzes.ToList();
            return Ok(getQuizz);
        }

        [HttpGet("{Id}", Name = "GetQuiz")]
        public IActionResult Get(int Id)
        {
            var getQuizz = _context.Quizzes.FirstOrDefault(x => x.ID == Id);
            return Ok(getQuizz);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostQuiz([FromBody] Quiz quiz)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var OwnerId = HttpContext.User.Claims.First().Value;
            quiz.OwnerId = OwnerId;
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetQuiz", new { id = quiz.ID }, quiz);
        }

    }
}