using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DocsHub.Data;
using DocsHub.Models;

namespace DocsHub.Controllers
{
    public class docsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public docsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: docs
        public async Task<IActionResult> Index()
        {
            return View(await _context.docs.ToListAsync());
        }

        // GET: docs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docs = await _context.docs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docs == null)
            {
                return NotFound();
            }

            return View(docs);
        }

        // GET: docs/Create
        public IActionResult Create()
        {
            return View();
        }

        //public class ContractController : Controller
        //{
        //    private readonly ContractService _contractService;

        //    public ContractController(ContractService contractService)
        //    {
        //        _contractService = contractService;
        //    }

        //    public IActionResult CalculateTotalPrice()
        //    {
        //        var docsList = _context.docs.ToList();

        //        _contractService.CalculateTotalPrice();

        //        return View();
        //    }
        //}

        // POST: docs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Date,Title,Price")] docs docs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(docs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(docs);
        }

        // GET: docs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docs = await _context.docs.FindAsync(id);
            if (docs == null)
            {
                return NotFound();
            }
            return View(docs);
        }

        // POST: docs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Date,Title,Price")] docs docs)
        {
            if (id != docs.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!docsExists(docs.Id))
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
            return View(docs);
        }

        // GET: docs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docs = await _context.docs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docs == null)
            {
                return NotFound();
            }

            return View(docs);
        }

        // POST: docs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var docs = await _context.docs.FindAsync(id);
            if (docs != null)
            {
                _context.docs.Remove(docs);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool docsExists(int id)
        {
            return _context.docs.Any(e => e.Id == id);
        }
    }
}
