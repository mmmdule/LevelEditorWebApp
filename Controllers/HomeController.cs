using LevelEditorWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using LevelEditorWebApp.Services;

namespace LevelEditorWebApp.Controllers {
    public class HomeController : Controller {
        //private readonly ILogger<HomeController> _logger;
        private readonly IPostsService _postsService;
        private readonly IVoteService _voteService;
        private readonly IDownloadStatsService _downloadService;

        //public HomeController(ILogger<HomeController> logger) {
        //    _logger = logger;
        //}

        public HomeController(IPostsService postsService, IVoteService voteService, IDownloadStatsService downloadService) {
            _postsService = postsService;
            _voteService = voteService;
            _downloadService = downloadService;
        }

        public IActionResult Index() {
            //get 5 most recently updated posts and pass them to the view
            var posts = _postsService.GetLatestFiveUpdatedPosts();
            var bestPostsAllTime = _voteService.GetBestRatedPostsAllTime(5);
            var mostDownloadedPostsAllTime = _downloadService.GetMostDownloadedPostsAllTime(5);
            ViewData["FiveNewestPosts"] = posts;
            ViewData["FiveBestAllTimePosts"] = bestPostsAllTime;
            ViewData["FiveMostDownloadedPosts"] = mostDownloadedPostsAllTime;
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