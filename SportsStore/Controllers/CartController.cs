﻿using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.Domain;
using SportsStore.Filters;

namespace SportsStore.Controllers
{
    [ServiceFilter(typeof(CartSessionFilter))]
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;

        public CartController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index(Cart cart)
        {
            if (cart.IsEmpty)
                return View("EmptyCart");
            ViewData["Total"] = cart.TotalValue;
            return View(cart.CartLines);
        }

        [HttpPost]
        public IActionResult Add(int id, int quantity, Cart cart)
        {
            Product product = _productRepository.GetById(id);
            if (product != null)
            {
                cart.AddLine(product, quantity);
            }
            return RedirectToAction(nameof(Index), "Store");
        }

        [HttpPost]
        public IActionResult Remove(int id, Cart cart)
        {
            Product product = _productRepository.GetById(id);
            cart.RemoveLine(product);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Plus(int id, Cart cart)
        {
            cart.IncreaseQuantity(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Min(int id, Cart cart)
        {
            cart.DecreaseQuantity(id);
            return RedirectToAction(nameof(Index));
        }
    }
}