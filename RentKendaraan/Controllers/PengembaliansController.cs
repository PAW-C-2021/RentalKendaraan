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
    public class PengembaliansController : Controller
    {
        private readonly RentKendaraanContext _context;

        public PengembaliansController(RentKendaraanContext context)
        {
            _context = context;
        }

        // GET: Pengembalians
        public async Task<IActionResult> Index(string ktsd, string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            /*var rentKendaraanContext = _context.Pengembalians.Include(p => p.IdKondisiNavigation).Include(p => p.IdPeminjamanNavigation);
            return View(await rentKendaraanContext.ToListAsync());*/

            var ktsdList = new List<string>();
            var ktsdQuery = from d in _context.Pengembalians orderby d.IdKondisiNavigation.NamaKondisi select d.IdKondisiNavigation.NamaKondisi;
            ktsdList.AddRange(ktsdQuery.Distinct());
            ViewBag.ktsd = new SelectList(ktsdList);
            var menu = from m in _context.Pengembalians.Include(k => k.IdPeminjamanNavigation).Include(k => k.IdKondisiNavigation) select m;
            if (!string.IsNullOrEmpty(ktsd))
            {
                menu = menu.Where(x => x.IdKondisiNavigation.NamaKondisi == ktsd);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                menu = menu.Where(s => s.TglPengembalian.ToString().Contains(searchString) || s.Denda.ToString().Contains(searchString));
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
                    menu = menu.OrderByDescending(s => s.Denda);
                    break;
                case "Date":
                    menu = menu.OrderBy(s => s.TglPengembalian);
                    break;
                case "date_desc":
                    menu = menu.OrderByDescending(s => s.TglPengembalian);
                    break;
                default: //name ascending
                    menu = menu.OrderBy(s => s.Denda);
                    break;
            }

            return View(await PaginatedList<Pengembalian>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
            //return View(await menu.ToListAsync());
        }

        // GET: Pengembalians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pengembalian = await _context.Pengembalians
                .Include(p => p.IdKondisiNavigation)
                .Include(p => p.IdPeminjamanNavigation)
                .FirstOrDefaultAsync(m => m.IdPengembalian == id);
            if (pengembalian == null)
            {
                return NotFound();
            }

            return View(pengembalian);
        }

        // GET: Pengembalians/Create
        public IActionResult Create()
        {
            ViewData["IdKondisi"] = new SelectList(_context.KondisiKendaraans, "IdKondisi", "IdKondisi");
            ViewData["IdPeminjaman"] = new SelectList(_context.Peminjamen, "IdPeminjaman", "IdPeminjaman");
            return View();
        }

        // POST: Pengembalians/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPengembalian,TglPengembalian,IdPeminjaman,IdKondisi,Denda")] Pengembalian pengembalian)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pengembalian);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdKondisi"] = new SelectList(_context.KondisiKendaraans, "IdKondisi", "IdKondisi", pengembalian.IdKondisi);
            ViewData["IdPeminjaman"] = new SelectList(_context.Peminjamen, "IdPeminjaman", "IdPeminjaman", pengembalian.IdPeminjaman);
            return View(pengembalian);
        }

        // GET: Pengembalians/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pengembalian = await _context.Pengembalians.FindAsync(id);
            if (pengembalian == null)
            {
                return NotFound();
            }
            ViewData["IdKondisi"] = new SelectList(_context.KondisiKendaraans, "IdKondisi", "IdKondisi", pengembalian.IdKondisi);
            ViewData["IdPeminjaman"] = new SelectList(_context.Peminjamen, "IdPeminjaman", "IdPeminjaman", pengembalian.IdPeminjaman);
            return View(pengembalian);
        }

        // POST: Pengembalians/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPengembalian,TglPengembalian,IdPeminjaman,IdKondisi,Denda")] Pengembalian pengembalian)
        {
            if (id != pengembalian.IdPengembalian)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pengembalian);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PengembalianExists(pengembalian.IdPengembalian))
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
            ViewData["IdKondisi"] = new SelectList(_context.KondisiKendaraans, "IdKondisi", "IdKondisi", pengembalian.IdKondisi);
            ViewData["IdPeminjaman"] = new SelectList(_context.Peminjamen, "IdPeminjaman", "IdPeminjaman", pengembalian.IdPeminjaman);
            return View(pengembalian);
        }

        // GET: Pengembalians/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pengembalian = await _context.Pengembalians
                .Include(p => p.IdKondisiNavigation)
                .Include(p => p.IdPeminjamanNavigation)
                .FirstOrDefaultAsync(m => m.IdPengembalian == id);
            if (pengembalian == null)
            {
                return NotFound();
            }

            return View(pengembalian);
        }

        // POST: Pengembalians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pengembalian = await _context.Pengembalians.FindAsync(id);
            _context.Pengembalians.Remove(pengembalian);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PengembalianExists(int id)
        {
            return _context.Pengembalians.Any(e => e.IdPengembalian == id);
        }
    }
}
