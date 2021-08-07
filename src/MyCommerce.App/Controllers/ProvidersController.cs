using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCommerce.App.Extensions;
using MyCommerce.App.ViewModels;
using MyCommerce.Business.Interfaces;
using MyCommerce.Business.Models;
using MyCommerce.Business.Notifications;

namespace MyCommerce.App.Controllers
{
    [Authorize]
    public class ProvidersController : BaseController
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IProviderService _providerService;
        private readonly IMapper _mapper;

        public ProvidersController(IProviderRepository providerRepository, IProviderService providerService, 
            IMapper mapper, INotificator notificator) : base(notificator)
        {
            _providerRepository = providerRepository;
            _providerService = providerService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProviderViewModel>>(await _providerRepository.GetAll()));
        }

        [AllowAnonymous]
        [Route("details/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var providerViewModel = await GetProviderWithAddress(id);
            
            if (providerViewModel == null)
            {
                return NotFound();
            }

            return View(providerViewModel);
        }

        [ClaimsAuthorize("Provider", "Add")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize("Provider", "Add")]
        public async Task<IActionResult> Create(ProviderViewModel providerViewModel)
        {
            if (!ModelState.IsValid) 
                return View(providerViewModel);

            var provider = _mapper.Map<Provider>(providerViewModel);
            await _providerService.Add(provider);
            
            if (!ValidOperation())
                return View(providerViewModel);

            return RedirectToAction(nameof(Index));            
        }

        [ClaimsAuthorize("Provider", "Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var providerViewModel = await GetProviderWithProductsAndAddress(id);
            if (providerViewModel == null)
            {
                return NotFound();
            }
            return View(providerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize("Provider", "Edit")]
        public async Task<IActionResult> Edit(Guid id, ProviderViewModel providerViewModel)
        {
            if (id != providerViewModel.Id)
                return NotFound();

            if (!ModelState.IsValid) 
                return View(providerViewModel);

            await _providerService.Update(_mapper.Map<Provider>(providerViewModel));
            if (!ValidOperation())
                return View(providerViewModel);

            return RedirectToAction(nameof(Index));

        }

        [ClaimsAuthorize("Provider", "Remove")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var providerViewModel = await GetProviderWithAddress(id);
            
            if (providerViewModel == null)
            {
                return NotFound();
            }

            return View(providerViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize("Provider", "Remove")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var providerViewModel = await GetProviderWithAddress(id);
            if (providerViewModel == null)
                return NotFound();

            await _providerService.Remove(id);
            if (!ValidOperation())
                return View(providerViewModel);

            TempData["Sucess"] = "Fornecedor excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetAddress(Guid id)
        {
            var provider = await GetProviderWithAddress(id);
            if (provider == null)
                return NotFound();

            return PartialView("_AddressDetails", provider);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAddress(Guid id)
        {
            var providerViewModel = await GetProviderWithAddress(id);
            if (providerViewModel == null)
                return NotFound();

            return PartialView("_UpdateAddress", new ProviderViewModel { Address = providerViewModel.Address });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAddress(ProviderViewModel providerViewModel)
        {
            ModelState.Remove("Name");
            ModelState.Remove("Document");

            if (!ModelState.IsValid)
                return PartialView("_UpdateAdress", providerViewModel);

            await _providerService.UpdateAddress(_mapper.Map<Address>(providerViewModel.Address));
            if (!ValidOperation())
                return View(providerViewModel);

            var url = Url.Action("GetAddress", "Providers", new { id = providerViewModel.Address.ProviderId });
            return Json(new { success = true, url });
        }

        private async Task<ProviderViewModel> GetProviderWithAddress(Guid providerId)
        {
            return _mapper.Map<ProviderViewModel>(await _providerRepository.GetProviderWithAddress(providerId));
        }

        private async Task<ProviderViewModel> GetProviderWithProductsAndAddress(Guid providerId)
        {
            return _mapper.Map<ProviderViewModel>(await _providerRepository.GetProviderWithProductsAndAddress(providerId));
        }
    }
}
