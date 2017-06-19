using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace Marigold
{
    public class ReservationsController : Controller
    {
        private IUnitOfWork unitOfWork;

        private IReservationsBll _bll;

        public ReservationsController(IReservationsBll bll, IUnitOfWork uow)
        {
            _bll = bll;
            unitOfWork = uow;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var pageIndex = !page.HasValue || page <= 0 ? 1 : page.Value;

            var list = _bll.Reservations(pageIndex - 1, 10);

            ViewBag.RoomUnitDescription = await _bll.RoomUnitDescription();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            return View(await _bll.ReservationInput());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationInputDto input)
        {
            if (!ModelState.IsValid)
                return View(input);

            await _bll.Create(input);

            return Redirect("Index");
        }

        public async Task<IActionResult> Checkout(string id)
        {
            var input = await _bll.CheckoutView(id);

            if (input == null)
                return new NotFoundResult();

            ViewBag.RoomUnitDescription = await _bll.RoomUnitDescription();
            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(ReservationInputDto input)
        {
            var res = await _bll.Checkout(input);

            if (!res)
                return new NotFoundResult();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Bill(string id)
        {
            var output = await _bll.Bill(id);

            if(output == null)
                return new NotFoundResult();

            ViewBag.Currency = "grams of gold";
            return View(output);
        }
    }
}