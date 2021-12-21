using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentKendaraan.Models;

namespace RentalKendaraan.Controllers
{
    public class CustomersController : Controller
    {
        private readonly RentKendaraanContext _context;

        public CustomersController(RentKendaraanContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string ktsd, string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            /*  var rentKendaraanContext = _context.Customers.Include(c => c.IdGenderNavigation);
              return View(await rentKendaraanContext.ToListAsync());*/

            var ktsdList = new List<string>();
            var ktsdQuery = from d in _context.Customers orderby d.IdGenderNavigation.NamaGender select d.IdGenderNavigation.NamaGender;
            ktsdList.AddRange(ktsdQuery.Distinct());
            ViewBag.ktsd = new SelectList(ktsdList);
            var menu = from m in _context.Customers.Include(k => k.IdGenderNavigation) select m;
            if (!string.IsNullOrEmpty(ktsd))
            {
                menu = menu.Where(x => x.IdGenderNavigation.NamaGender == ktsd);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                menu = menu.Where(s => s.NamaCustomer.Contains(searchString) || s.Nik.Contains(searchString)
                || s.Alamat.Contains(searchString) || s.NoHp.Contains(searchString));
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
            ViewData["NIKSortParm"] = sortOrder == "NIK" ? "NIK_desc" : "NIK";

            switch (sortOrder)
            {
                case "name_desc":
                    menu = menu.OrderByDescending(s => s.NamaCustomer);
                    break;
                case "NIK":
                    menu = menu.OrderBy(s => s.Nik);
                    break;
                case "NIK_desc":
                    menu = menu.OrderByDescending(s => s.Nik);
                    break;
                default: //name ascending
                    menu = menu.OrderBy(s => s.NamaCustomer);
                    break;
            }

            return View(await PaginatedList<Customer>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));

            //return View(await menu.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.IdGenderNavigation)
                .FirstOrDefaultAsync(m => m.IdCostumer == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["IdGender"] = new SelectList(_context.Genders, "IdGender", "IdGender");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCustomer,NamaCustomer,Nik,Alamat,NoHp,IdGender")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdGender"] = new SelectList(_context.Genders, "IdGender", "IdGender", customer.IdGender);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["IdGender"] = new SelectList(_context.Genders, "IdGender", "IdGender", customer.IdGender);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCustomer,NamaCustomer,Nik,Alamat,NoHp,IdGender")] Customer customer)
        {
            if (id != customer.IdCostumer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.IdCostumer))
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
            ViewData["IdGender"] = new SelectList(_context.Genders, "IdGender", "IdGender", customer.IdGender);
            return View(customer);
        }
        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.IdGenderNavigation)
                .FirstOrDefaultAsync(m => m.IdCostumer == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.IdCostumer == id);
        }
    }
}