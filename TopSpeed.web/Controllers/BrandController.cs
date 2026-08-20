using Microsoft.AspNetCore.Mvc;
using TopSpeed.web.Data;
using TopSpeed.web.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TopSpeed.web.Controllers
{
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BrandController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Brand> brands = _dbContext.Brand.ToList();
            return View(brands);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            string webrootpath = _webHostEnvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;
            if(file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();
                string upload = Path.Combine(webrootpath,@"Images\Brand");
                string extension = Path.GetExtension(file[0].FileName);
                using (var filestream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(filestream);
                }

                brand.BrandLogo = @"\Images\Brand\" + newFileName + extension;

            }

            if(ModelState.IsValid)
            {
                _dbContext.Brand.Add(brand);
                _dbContext.SaveChanges();
                TempData["success"] = "Records Created Sucessfully";
                return RedirectToAction(nameof(Index));

            }
            return View();
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id);
            if(brand != null)
            {
                return View(brand);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id);
            if (brand != null)
            {
                return View(brand);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            string webrootpath = _webHostEnvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;
            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();
                string upload = Path.Combine(webrootpath, @"Images\Brand");
                string extension = Path.GetExtension(file[0].FileName);
                var dbObjct = _dbContext.Brand.FirstOrDefault(x => x.Id == brand.Id);
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(dbObjct.BrandLogo))
                {
                    string oldImagePath = Path.Combine(webrootpath, dbObjct.BrandLogo.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var filestream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(filestream);
                }

                brand.BrandLogo = @"\Images\Brand\" + newFileName + extension;

            }
            if (ModelState.IsValid)
            {
                var dbObj = _dbContext.Brand.FirstOrDefault(x => x.Id == brand.Id);
                dbObj.Name = brand.Name;
                dbObj.EstablishedYear = brand.EstablishedYear;
                if (brand.BrandLogo != null)
                {
                    dbObj.BrandLogo = brand.BrandLogo;
                }
                _dbContext.Brand.Update(dbObj);
                _dbContext.SaveChanges();
                TempData["warning"] = "Records Updated Sucessfully";
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id);
            if (brand != null)
            {
                return View(brand);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Delete(Brand brand)
        {
            var dbObj = _dbContext.Brand.FirstOrDefault(x => x.Id == brand.Id);
            if(dbObj != null)
            {
                //delte old image
                if(!string.IsNullOrEmpty(dbObj.BrandLogo))
                {
                    string webrootpath = _webHostEnvironment.WebRootPath;
                    string oldImagePath = Path.Combine(webrootpath,dbObj.BrandLogo.Trim('\\'));
                    if(System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _dbContext.Brand.Remove(dbObj);
                _dbContext.SaveChanges();
                TempData["error"] = "Records Deleted Sucessfully";
                return RedirectToAction(nameof(Index)); 
            }
            return NotFound();  

        }
    }
}
