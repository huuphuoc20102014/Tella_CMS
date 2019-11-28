using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tella_CMS.Models;

namespace Tella_CMS.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private readonly Tella_CMSContext _context;

        public CustomerController(Tella_CMSContext context)
        {
            _context = context;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            var listCustomer = await _context.Customer
                .OrderBy(p => p.CreatedDate)
                        .Select(p => new CustomerViewModel
                        {
                            Id = p.Id,
                            Code = p.Code,
                            FullName = p.FullName,
                            Address1 = p.Address1,
                            Address2 = p.Address2,
                            Age = p.Age,
                            Telephone = p.Telephone,
                            CreatedDate = p.CreatedDate.ToString("dd/MM/yyyy"),
                            FkCustomerId = "Normal"
                        }).ToListAsync();

            var index = 0;
            foreach (var item in listCustomer)
            {
                index++;
                item.Stt = index;
            }
            return View(listCustomer);
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel vm)
        {
            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            // Check code is existed
            if (await _context.Customer.AnyAsync(h => h.Code == vm.Code))
            {
                return View(vm);
            }

            // Create save db item


            var dbItem = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Code = vm.Code,
                FullName = vm.FullName,
                Age = vm.Age,
                Address1 = vm.Address1,
                Telephone = vm.Telephone,
                Email = vm.Email,
                CreatedDate = DateTime.Now
            };
            _context.Add(dbItem);
            // Set time stamp for table to handle concurrency conflict
            //tableVersion.Action = 0;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CustomerController.Details), new { id = dbItem.Id });
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Code,FullName,Age,Address1,Address2,Telephone,Email,CreatedDate,Active,FkCustomerId")] Customer customer)
        {
            if (id != customer.Id)
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
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var customer = await _context.Customer.FindAsync(id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(string id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
}
