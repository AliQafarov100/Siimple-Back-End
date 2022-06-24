using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siimple_Back_End.DAL;
using Siimple_Back_End.Models;
using Siimple_Back_End.ViewModels;

namespace Siimple_Back_End.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Card> cards = await _context.Cards.ToListAsync();

            HomeVM model = new HomeVM
            {
                Cards = cards
            };
            return View(model);
        }
    }
}
