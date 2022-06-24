using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Siimple_Back_End.DAL;
using Siimple_Back_End.Models;

namespace Siimple_Back_End.Areas.SiimpleAdmin.Controllers
{
    [Area("SiimpleAdmin")]
    public class CardsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CardsController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cards.ToListAsync());
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

       
        public IActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Card card)
        {
            if (ModelState.IsValid)
            {
                if(card.Photo != null)
                {
                    if (!card.Photo.ContentType.Contains("image"))
                    {
                        ModelState.AddModelError("Photo", "Please choose image file");
                        return View();
                    }
                    if(card.Photo.Length > 1024 * 1024)
                    {
                        ModelState.AddModelError("Photo", "Size of image mustn't ,more than 1Mb");
                        return View();
                    }

                    string fileName = card.Photo.FileName;
                    string filePath = Path.Combine(_env.WebRootPath, "assets", "Image", "Service");
                    string fullPath = Path.Combine(filePath, fileName);

                    using(FileStream stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await card.Photo.CopyToAsync(stream);
                    }

                    card.Inage = fileName;

                    _context.Cards.Add(card);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Photo", "Please choose file!");
                    return View();
                }
               
            }
            else
            {
                return NotFound();
            }
            
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            return View(card);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Card card)
        {

            if (id != card.Id)
            {
                return NotFound();
            }
            Card existedCars = await _context.Cards.FindAsync(id);
            if (ModelState.IsValid)
            {
                try
                {
                    if (card.Photo != null)
                    {
                        if (!card.Photo.ContentType.Contains("image"))
                        {
                            ModelState.AddModelError("Photo", "Please choose image file");
                            return View();
                        }
                        if (card.Photo.Length > 1024 * 1024)
                        {
                            ModelState.AddModelError("Photo", "Size of image mustn't ,more than 1Mb");
                            return View();
                        }

                        string fileName = card.Photo.FileName;
                        string filePath = Path.Combine(_env.WebRootPath, "assets", "Image", "Service");
                        string fullPath = Path.Combine(filePath, fileName);

                        using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await card.Photo.CopyToAsync(stream);
                        }

                        string path = _env.WebRootPath + @"assets\Image\Service" + existedCars.Inage;

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        existedCars.Inage = fileName;
                    }
                    else
                    {
                        ModelState.AddModelError("Photo", "Please choose file!");
                        return View();
                    }
                    existedCars.Title = card.Title;
                    existedCars.SubTitle = card.SubTitle;
                    existedCars.Icon = card.Icon;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(card.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(card);
        }

     
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.Id == id);
        }
    }
}
