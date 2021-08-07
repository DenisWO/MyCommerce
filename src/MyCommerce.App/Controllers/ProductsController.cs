using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCommerce.App.Extensions;
using MyCommerce.App.ViewModels;
using MyCommerce.Business.Interfaces;
using MyCommerce.Business.Models;
using MyCommerce.Business.Notifications;

namespace MyCommerce.App.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, IProductService productService, 
            IProviderRepository providerRepository, IMapper mapper, INotificator notificator) : base(notificator)
        {
            _productRepository = productRepository;
            _providerRepository = providerRepository;
            _mapper = mapper;
            _productService = productService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsWithProviders()));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            var productViewModel = await GetProduct(id);
            if (productViewModel == null)
                return NotFound();

            return View(productViewModel);
        }

        [ClaimsAuthorize("Product", "Add")]
        public async Task<IActionResult> Create()
        {
            var productViewModel = await PopulateProviders(new ProductViewModel());
            return View(productViewModel);
        }

        [HttpPost]
        [ClaimsAuthorize("Product", "Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            productViewModel = await PopulateProviders(productViewModel);

            if (!ModelState.IsValid) 
                return View(productViewModel);

            string imagePrefix = Guid.NewGuid() + "_";
            if (!await UploadFile(productViewModel.ImageUpload, imagePrefix))
                return View(productViewModel);

            productViewModel.Image = imagePrefix + productViewModel.ImageUpload.FileName;
            
            await _productService.Add(_mapper.Map<Product>(productViewModel));
            if (!ValidOperation())
                return View(productViewModel);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Product", "Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var productViewModel = await GetProduct(id);
            if (productViewModel == null)
                return NotFound();

            return View(productViewModel);
        }

        [ClaimsAuthorize("Product", "Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
                return NotFound();

            var productUpdated = await GetProduct(id);
            productViewModel.Provider = productUpdated.Provider;
            productViewModel.Image = productUpdated.Image;

            if (!ModelState.IsValid)
                return View(productViewModel);

            if (productViewModel.ImageUpload != null)
            {
                var imagePrefix = Guid.NewGuid() + "_";
                if (!await UploadFile(productViewModel.ImageUpload, imagePrefix))
                    return View(productViewModel);

                productUpdated.Image = imagePrefix + productViewModel.ImageUpload.FileName;
            }

            await _productService.Update(_mapper.Map<Product>(productViewModel));
            if (!ValidOperation())
                return View(productViewModel);
            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Product", "Remove")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await GetProduct(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [ClaimsAuthorize("Product", "Remove")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await GetProduct(id);
            if (product == null)
                return NotFound();

            await _productService.Remove(id);
            if (!ValidOperation())
                return View(product);

            TempData["Sucess"] = "Produto excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<ProductViewModel> GetProduct(Guid productId)
        {
            var product = _mapper.Map<ProductViewModel>(await _productRepository.GetProductWithProvider(productId));
            product.Providers = _mapper.Map<IEnumerable<ProviderViewModel>>(await _providerRepository.GetAll());

            return product;
        }

        private async Task<ProductViewModel> PopulateProviders(ProductViewModel product)
        {
            product.Providers = _mapper.Map<IEnumerable<ProviderViewModel>>(await _providerRepository.GetAll());

            return product;
        }

        private async Task<bool> UploadFile(IFormFile file, string imagePrefix)
        {
            if (file.Length <= 0)
                return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", imagePrefix + file.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
                await file.CopyToAsync(stream);
            
            return true;
        }
    }
}
