using IndigoSite.DAL;
using IndigoSite.Models;
using IndigoSite.ViewModel;
using Microsoft.AspNetCore.Mvc;
using IndigoSite.Utilies.Extension;
using Microsoft.AspNetCore.Authorization;

namespace IndigoSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]

    public class PostController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;

        public PostController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.MaxPageCount =(int) Math.Ceiling((decimal)_context.Posts.Count() / 5);
            var posts = _context.Posts.Skip((page - 1) * 5).Take(5).ToList();
            return View(posts);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreatePostVM postVM)
        {
            if (postVM.Image.CheckValidate("image/", 300).Length > 0)
            {
                ModelState.AddModelError("", postVM.Image.CheckValidate("image/", 300));
                return View();
            }
            if (!ModelState.IsValid) { return View(); }
            Post post = new Post()
            {
                Title = postVM.Title,
                Description = postVM.Description,
                ButtonText = postVM.ButtonText,
                ImgUrl = postVM.Image.SaveFile(Path.Combine(_env.WebRootPath, "assets", "images"))
            };
            _context.Posts.Add(post);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();
            Post post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null) return NotFound();
            post.ImgUrl.DeleteFile(_env.WebRootPath, "assets/images");
            _context.Posts.Remove(post);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Update(int? id, UpdatePostVM postVM)
        {
          if(postVM.Image!= null)
            {
                if (postVM.Image.CheckValidate("image/", 300).Length > 0)
                {
                    ModelState.AddModelError("", postVM.Image.CheckValidate("image/", 300));
                    return View();
                }
            }
         
            if (!ModelState.IsValid) { return View(); }
            if (id is null) return BadRequest();
            Post post= _context.Posts.FirstOrDefault(p=>p.Id == id);
            string img = post.ImgUrl;
            if (post == null) return NotFound();
            post.Title = postVM.Title;
            post.Description = postVM.Description;
            post.ButtonText = postVM.ButtonText;
            if(postVM.Image != null)
            {
                post.ImgUrl = postVM.Image.SaveFile(Path.Combine(_env.WebRootPath, "assets", "images"));
                img.DeleteFile(_env.WebRootPath, "assets/images");
            }
         

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
