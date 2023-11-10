using LevelEditorWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using LevelEditorWebApp.Services;

namespace LevelEditorWebApp.Controllers {
    public class HomeController : Controller {
        //private readonly ILogger<HomeController> _logger;
        private readonly IPostsService _postsService;

        //public HomeController(ILogger<HomeController> logger) {
        //    _logger = logger;
        //}

        public HomeController(IPostsService postsService) {
            _postsService = postsService;
        }

        public IActionResult Index() {
            //get 5 most recently updated posts and pass them to the view
            var posts = _postsService.GetLatestFiveUpdatedPosts();
            ViewData["FiveNewestPosts"] = posts;
            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}