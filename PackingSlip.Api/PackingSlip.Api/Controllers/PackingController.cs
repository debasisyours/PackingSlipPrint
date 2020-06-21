using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PackingSlip.Domain.Entities;
using PackingSlip.Service;

namespace PackingSlip.Api.Controllers
{
    public class PackingController : Controller
    {
        private readonly IPackingSlipSaveService _packingSlipSaveService = null;
        private readonly IPackingSlipPrintService _packingSlipPrintService = null;

        public PackingController(IPackingSlipSaveService packingSlipSaveService,
            IPackingSlipPrintService packingSlipPrintService)
        {
            _packingSlipSaveService = packingSlipSaveService;
            _packingSlipPrintService = packingSlipPrintService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SavePackingSlip([FromBody] PackingSlipHeader packingSlip)
        {
            var response = await _packingSlipSaveService.SavePackingSlip(packingSlip);
            if (response.IsSuccess)
            {
                packingSlip.PackingSlipNumber = response.Detail;
                _packingSlipPrintService.PrintPackingSlip(packingSlip);
            }

            return Ok(response);
        }
    }
}
