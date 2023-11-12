using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LevelEditorWebApp.Data;
using LevelEditorWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO.Compression;
using LevelEditorWebApp.Classes;

namespace LevelEditorWebApp.Controllers {
    public class PostsController : Controller {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index() {
            return _context.Post != null ?
                        View(await _context.Post.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Post'  is null.");
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null || _context.Post == null) {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null) {
                return NotFound();
            }
            ViewData["Comments"] = _context.Comment.Where(c => c.PostId == id).ToList();
            return View(post);
        }

        // GET: Posts/Create
        [Authorize]
        public IActionResult Create() {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,Body,CreatedAt,ZipFile,Author,ZipFileName")] Post post) {
            post.CreatedAt = DateTime.Now;
            post.Author = User.Identity.Name;

            //get ZipFile from form and check if it is valid file
            var file = Request.Form.Files["ZipFile"];

            post.ZipFileName = file.FileName;

            post.ZipFile = new byte[file.Length];
            using (var stream = new System.IO.MemoryStream()) {
                await file.CopyToAsync(stream);
                post.ZipFile = stream.ToArray();
            }

            if (!IsValidZipFile(file)) {
                return Problem("Invalid zip file. Please upload a zip file containing exactly one .lep file and a folder named \"maps\" containing only .lem files.");
            }

            if (ModelState.IsValid) {
                _context.Add(post);
                await _context.SaveChangesAsync();
                //create zip file in wwwroot/uploads
                System.IO.File.WriteAllBytes("wwwroot/uploads/" + post.PostId + post.ZipFileName, post.ZipFile);

                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id) {
            if (!IsAuthorOfPost(id)) {
                return Unauthorized();
            }

            if (id == null || _context.Post == null) {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null) {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Body,CreatedAt,ZipFile,Author,ZipFileName")] Post post) {
            if (id != post.PostId) 
                return NotFound();
            if (!IsAuthorOfPost(id))
                return Unauthorized();

            post.CreatedAt = DateTime.Now;
            post.Author = User.Identity.Name;

            post.ZipFileName = _context.Post.Find(id).ZipFileName;
            post.ZipFile = _context.Post.Find(id).ZipFile;

            if (Request.Form.Files.Count != 0) {
                //get ZipFile from form and check if it is valid file
                var file = Request.Form.Files["ZipFile"];

                post.ZipFileName = file.FileName;

                post.ZipFile = new byte[file.Length];
                using (var stream = new System.IO.MemoryStream()) {
                    await file.CopyToAsync(stream);
                    post.ZipFile = stream.ToArray();
                }

                if (!IsValidZipFile(file))
                    return Problem("Invalid zip file. Please upload a zip file containing exactly one .lep file and a folder named \"maps\" containing only .lem files.");
            }


            if (ModelState.IsValid) {
                try {
                    _context.ChangeTracker.Clear();

                    _context.Update(post);
                    await _context.SaveChangesAsync();

                    if (Request.Form.Files.Count != 0) {
                        //delete old zip file in wwwroot/uploads
                        System.IO.File.Delete("wwwroot/uploads/" + post.PostId + post.ZipFileName);
                        //create zip file in wwwroot/uploads
                        System.IO.File.WriteAllBytes("wwwroot/uploads/" + post.PostId + post.ZipFileName, post.ZipFile);
                    }
                }
                catch (DbUpdateConcurrencyException) {
                    if (!PostExists(post.PostId)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id) {
            if (id == null || _context.Post == null) {
                return NotFound();
            }


            if (!IsAuthorOfPost(id)) {
                return Unauthorized();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null) {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            if (_context.Post == null) {
                return Problem("Entity set 'ApplicationDbContext.Post'  is null.");
            }
            var post = await _context.Post.FindAsync(id);

            //delete zip file in wwwroot/uploads
            System.IO.File.Delete("wwwroot/uploads/" + post.PostId + post.ZipFileName);

            if (post != null) {
                _context.Post.Remove(post);
            }


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id) {
            return (_context.Post?.Any(e => e.PostId == id)).GetValueOrDefault();
        }

        //pass in a file and check if it is a valid zip file
        //add file as parameter
        private bool IsValidZipFile(IFormFile file) {
            //check if file is a zip file
            //if (file.ContentType != "application/zip") {
            //    return false;
            //}

            //check if file is a zip file
            if (file.FileName.Split('.').Last() != "zip") {
                return false;
            }

            //check if zip file contains exactly one .lep file and a folder named "maps" containing at only .lem files
            //return false if any of these conditions are not met
            if (!ZipFileContainsOneLepFile(file) || !ZipFileContainsMapsFolder(file) || MapsContainOnlyLemFiles(file)) {
                return false;
            }

            return true;
        }

        //check if zip file contains exactly one .lep file in the root directory
        private bool ZipFileContainsOneLepFile(IFormFile file) {
            //open zip file
            using (var zip = new ZipArchive(file.OpenReadStream())) {
                //get all entries in zip file
                var entries = zip.Entries;

                //count number of .lep files in zip file
                int lepFileCount = 0;
                foreach (var entry in entries) {
                    if (entry.FullName.Split('.').Last() == "lep") {
                        lepFileCount++;
                    }
                }

                //return true if zip file contains exactly one .lep file
                return lepFileCount == 1;
            }
        }

        //check if zip file contains a folder named "maps"
        private bool ZipFileContainsMapsFolder(IFormFile file) {
            //open zip file
            using (var zip = new ZipArchive(file.OpenReadStream())) {
                //get all entries in zip file
                var entries = zip.Entries;

                //check if zip file contains a folder named "maps"
                foreach (var entry in entries) {
                    if (entry.FullName.ToLower() == "maps/") {
                        return true;
                    }
                }

                return false;
            }
        }

        //check if zip file contains only .lem files in the "maps" folder
        private bool MapsContainOnlyLemFiles(IFormFile file) {
            //open zip file
            using (var zip = new ZipArchive(file.OpenReadStream())) {
                //get all entries in zip file
                var entries = zip.Entries;

                //check if zip file contains only .lem files in the "maps" folder
                foreach (var entry in entries) {
                    if (entry.FullName.Split('.').Last() != "lem" && entry.FullName.Split('/').First().ToLower() == "maps") {
                        return false;
                    }
                }

                return true;
            }
        }

        // GET: Posts/Download/5
        public async Task<IActionResult> Download(int? id) {
            if (id == null || _context.Post == null) {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null) {
                return NotFound();
            }

            //create zip file in wwwroot/uploads
            System.IO.File.WriteAllBytes("wwwroot/uploads/" + post.PostId + post.ZipFileName, post.ZipFile);

            return File(post.ZipFile, "application/zip", post.ZipFileName);
        }

        //check if user is author of post
        private bool IsAuthorOfPost(int? id) {
            var post = _context.Post.Find(id);
            return post.Author == User.Identity.Name;
        }

        //add comment to post
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddComment([Bind("PostId,Content")] Comment comment) {
            comment.CreatedAt = DateTime.Now;
            comment.Username = User.Identity.Name;
            comment.Content = Request.Form["Content"].ToString().Trim();
            comment.PostId = int.Parse(Request.Form["PostId"]);
            comment.UserId = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id;


            if (CommentIsValid(comment)) {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = comment.PostId });
            }
            return RedirectToAction(nameof(Details), new { id = comment.PostId });
        }

        private bool CommentIsValid(Comment comment) {
            //check if all comment fields are valid
            return comment.Content is not null && comment.Content is not "" && 
                   comment.Username is not "" && comment.Username is not null &&
                   comment.UserId is not "" && comment.UserId is not null;
        }
    }
}
