using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Football_Fun_Facts.Data;
using Football_Fun_Facts.Models;
using Microsoft.AspNetCore.Authorization;

namespace Football_Fun_Facts.Controllers
{
    public class FactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Facts
        public async Task<IActionResult> Index()
        {
              return _context.Facts != null ? 
                          View(await _context.Facts.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Facts'  is null.");
        }

        // GET: SearchForm
        public async Task<IActionResult> Search()
        {
            return View();
                       
        }

        // POST: Facts/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.Facts.Where(j => j.FactQuestion.Contains(SearchPhrase)).ToListAsync()); 
           // await _context.Facts.ToListAsync() ➡️ This would show all
           // of the results.
           // With Index infront we will only get the result based on
           // what we typed.

        }

        // GET: Facts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Facts == null)
            {
                return NotFound();
            }

            var facts = await _context.Facts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facts == null)
            {
                return NotFound();
            }

            return View(facts);
        }

        // GET: Facts/Create

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Facts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FactQuestion,FactAnswer")] Facts facts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(facts);
        }

        // GET: Facts/Edit/5

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Facts == null)
            {
                return NotFound();
            }

            var facts = await _context.Facts.FindAsync(id);
            if (facts == null)
            {
                return NotFound();
            }
            return View(facts);
        }

        // POST: Facts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FactQuestion,FactAnswer")] Facts facts)
        {
            if (id != facts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FactsExists(facts.Id))
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
            return View(facts);
        }

        // GET: Facts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Facts == null)
            {
                return NotFound();
            }

            var facts = await _context.Facts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facts == null)
            {
                return NotFound();
            }

            return View(facts);
        }

        // POST: Facts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Facts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Facts'  is null.");
            }
            var facts = await _context.Facts.FindAsync(id);
            if (facts != null)
            {
                _context.Facts.Remove(facts);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FactsExists(int id)
        {
          return (_context.Facts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
