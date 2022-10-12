using Microsoft.AspNetCore.Mvc;
using SqlInjectionDemo.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SqlInjectionDemo.Data;

namespace SqlInjectionDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DemoContext _dbContext;

        public HomeController(ILogger<HomeController> logger, DemoContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Board");
        }

        [Route("[controller]/Board/{boardName?}")]
        public IActionResult Board(string boardName = "Demo Board")
        {
            //var model = _dbContext.Boards
            //    .Include(b => b.Posts)
            //    .FirstOrDefault(b => b.Name == boardName) ?? new Board { Name = boardName };

            var sql = $"select * from Posts where BoardId in (select Id from Boards where Name = '{boardName}')";
            var posts = _dbContext.Posts
                //.FromSqlRaw($"select b.*, p.Title, p.Text, p.CreatedTimestamp from Boards b left join Posts p on b.Id = p.BoardId where b.Name = '{boardName}'")
                .FromSqlRaw(sql)
                .ToList();
                

            return View(new Board { Name = boardName, Posts = posts});
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(string boardName, Post post)
        {
            var board = _dbContext.Boards.FirstOrDefault(b => b.Name == boardName) ?? new Board { Name = boardName };
            board.Posts.Add(post);
            _dbContext.Update(board);
            await _dbContext.SaveChangesAsync();
            
            return RedirectToAction("Board", new { boardName = board.Name });
        }
    }
}