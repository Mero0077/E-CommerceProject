using E_CommerceFIdentityScaff.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using E_CommerceFIdentityScaff.Models;
using Microsoft.AspNetCore.Authorization;
using E_CommerceFIdentityScaff.Models.ViewModels;

namespace E_CommerceFIdentityScaff.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="SuperAdmin,Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll(null, includes: [p => p.Category]);
            return View(products.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int ProductId)
        {
            var product = await _unitOfWork.Product.GetOne(c => c.Id == ProductId, null, false);
            if (product == null) return NotFound();

            var vm = BuildProductViewModel(product);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductVM vm, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                vm.CategoryList = GetCategoryList();
                return View(vm);
            }

            return await UpdateProduct(vm, imageFile);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = BuildProductViewModel(new Product());
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductVM vm, IFormFile imageFile)
        {
            if (await _unitOfWork.Product.Exists(c => c.Title == vm.Product.Title))
            {
                ModelState.AddModelError("Product.Title", "Product with this title already exists.");
            }

            if (!ModelState.IsValid)
            {
                vm.CategoryList = GetCategoryList();
                return View(vm);
            }

            await HandleImageUpload(vm, imageFile);
            await _unitOfWork.Product.Add(vm.Product);
            await _unitOfWork.Commit();

            TempData["success"] = "Product created successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var product = await _unitOfWork.Product.GetOne(p => p.Id == Id);
            if (product == null) return NotFound();

            _unitOfWork.Product.Delete(product);
            await _unitOfWork.Commit();

            TempData["success"] = "Product Deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        // ======================
        //  Helper Functions
        // ======================

        private IEnumerable<SelectListItem> GetCategoryList()
        {
            return _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
        }

        private ProductVM BuildProductViewModel(Product product)
        {
            return new ProductVM
            {
                Product = product,
                CategoryList = GetCategoryList()
            };
        }

        private async Task HandleImageUpload(ProductVM vm, IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) return;

            var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(imagesFolder, fileName);

            // Delete old image
            if (vm.Product.Id != 0)
            {
                var oldProduct = await _unitOfWork.Product.GetOne(p => p.Id == vm.Product.Id,null,false);
                var oldPath = Path.Combine(imagesFolder, oldProduct?.ImagePath ?? "");
                if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
            }

            using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            vm.Product.ImagePath = fileName;
        }

        private async Task<IActionResult> UpdateProduct(ProductVM vm, IFormFile? imageFile)
        {
            var productInDb = await _unitOfWork.Product.GetOne(e => e.Id == vm.Product.Id,null,false);
            if (productInDb == null) return NotFound();

            await HandleImageUpload(vm, imageFile);
            _unitOfWork.Product.Edit(vm.Product);
            await _unitOfWork.Commit();

            TempData["success"] = "Product updated successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
