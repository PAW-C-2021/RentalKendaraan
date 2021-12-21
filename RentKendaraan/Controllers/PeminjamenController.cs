using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentKendaraan.Models;

namespace RentKendaraan.Controllers
{
    public class PeminjamenController : Controller
    {
        private readonly RentKendaraanContext _context;

        public PeminjamenController(RentKendaraanContext context)
        {
            _context = context;
        }

        // GET: Peminjamans
        public async Task<IActionResult> Index(string ktsd, string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            /* var rentKendaraanContext = _context.Peminjamen.Include(p => p.IdCustomerNavigation).Include(p => p.IdJaminanNavigation).Include(p => p.IdKendaraanNavigation);
             return View(await rentKendaraanContext.ToListAsync());*/


            var ktsdList = new List<string>();
            var ktsdQuery = from d in _context.Peminjamen orderby d.IdKendaraan select d.IdKendaraan.ToString();
            ktsdList.AddRange(ktsdQuery.Distinct());
            ViewBag.ktsd = new SelectList(ktsdList);
            var menu = from m in _context.Peminjamen.Include(k => k.IdCostumerNavigation).Include(k => k.IdJaminanNavigation).Include(k => k.IdKendaraanNavigation) select m;
            if (!string.IsNullOrEmpty(ktsd))
            {
                menu = menu.Where(x => x.IdKendaraan.ToString() == ktsd);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                menu = menu.Where(s => s.TglPeminjaman.ToString().Contains(searchString) || s.Biaya.ToString().Contains(searchString)
                || s.IdJaminanNavigation.NamaJaminan.Contains(searchString) || s.IdKendaraanNavigation.NamaKendaraan.Contains(searchString));
            }


            //membuat paged list
            ViewData["CurrentSort"] = sortOrder;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;


            //definisi jumlah data pada halaman
            int pageSize = 5;


            //untuk sorting
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            switch (sortOrder)
            {
                case "name_desc":
                    menu = menu.OrderByDescending(s => s.IdCostumerNavigation.NamaCustomer);
                    break;
                case "Date":
                    menu = menu.OrderBy(s => s.TglPeminjaman);
                    break;
                case "date_desc":
                    menu = menu.OrderByDescending(s => s.TglPeminjaman);
                    break;
                default: //name ascending
                    menu = menu.OrderBy(s => s.IdCostumerNavigation.NamaCustomer);
                    break;
            }

            return View(await PaginatedList<Peminjaman>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));

            //return View(await menu.ToListAsync());
        }


        // GET: Peminjamen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peminjaman = await _context.Peminjamen
                .Include(p => p.IdCostumerNavigation)
                .Include(p => p.IdJaminanNavigation)
                .Include(p => p.IdKendaraanNavigation)
                .FirstOrDefaultAsync(m => m.IdPeminjaman == id);
            if (peminjaman == null)
            {
                return NotFound();
            }

            return View(peminjaman);
        }

        // GET: Peminjamen/Create
        public IActionResult Create()
        {
            ViewData["IdCostumer"] = new SelectList(_context.Customers, "IdCustomer", "IdCustomer");
            ViewData["IdJaminan"] = new SelectList(_context.Jaminans, "IdJaminan", "IdJaminan");
            ViewData["IdKendaraan"] = new SelectList(_context.Kendaraans, "IdKendaraan", "IdKendaraan");
            return View();
        }

        // POST: Peminjamen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPeminjaman,TglPeminjaman,IdKendaraan,IdCostumer,IdJaminan,Biaya")] Peminjaman peminjaman)
        {
            if (ModelState.IsValid)
            {
                _context.Add(peminjaman);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCostumer"] = new SelectList(_context.Customers, "IdCustomer", "IdCustomer", peminjaman.IdCostumer);
            ViewData["IdJaminan"] = new SelectList(_context.Jaminans, "IdJaminan", "IdJaminan", peminjaman.IdJaminan);
            ViewData["IdKendaraan"] = new SelectList(_context.Kendaraans, "IdKendaraan", "IdKendaraan", peminjaman.IdKendaraan);
            return View(peminjaman);
        }

        // GET: Peminjamen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peminjaman = await _context.Peminjamen.FindAsync(id);
            if (peminjaman == null)
            {
                return NotFound();
            }
            ViewData["IdCostumer"] = new SelectList(_context.Customers, "IdCustomer", "IdCustomer", peminjaman.IdCostumer);
            ViewData["IdJaminan"] = new SelectList(_context.Jaminans, "IdJaminan", "IdJaminan", peminjaman.IdJaminan);
            ViewData["IdKendaraan"] = new SelectList(_context.Kendaraans, "IdKendaraan", "IdKendaraan", peminjaman.IdKendaraan);
            return View(peminjaman);
        }

        // POST: Peminjamen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPeminjaman,TglPeminjaman,IdKendaraan,IdCostumer,IdJaminan,Biaya")] Peminjaman peminjaman)
        {
            if (id != peminjaman.IdPeminjaman)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(peminjaman);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeminjamanExists(peminjaman.IdPeminjaman))
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
            ViewData["IdCostumer"] = new SelectList(_context.Customers, "IdCustomer", "IdCustomer", peminjaman.IdCostumer);
            ViewData["IdJaminan"] = new SelectList(_context.Jaminans, "IdJaminan", "IdJaminan", peminjaman.IdJaminan);
            ViewData["IdKendaraan"] = new SelectList(_context.Kendaraans, "IdKendaraan", "IdKendaraan", peminjaman.IdKendaraan);
            return View(peminjaman);
        }

        // GET: Peminjamen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peminjaman = await _context.Peminjamen
                .Include(p => p.IdCostumerNavigation)
                .Include(p => p.IdJaminanNavigation)
                .Include(p => p.IdKendaraanNavigation)
                .FirstOrDefaultAsync(m => m.IdPeminjaman == id);
            if (peminjaman == null)
            {
                return NotFound();
            }

            return View(peminjaman);
        }

        // POST: Peminjamen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var peminjaman = await _context.Peminjamen.FindAsync(id);
            _context.Peminjamen.Remove(peminjaman);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeminjamanExists(int id)
        {
            return _context.Peminjamen.Any(e => e.IdPeminjaman == id);
        }
    }
}
