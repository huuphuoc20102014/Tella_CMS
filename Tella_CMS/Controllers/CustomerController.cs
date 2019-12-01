using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tella_CMS.Efs.Context;
using Tella_CMS.Efs.Entities;
using Tella_CMS.Models;

namespace Tella_CMS.Controllers
{
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
                             FkCustomerId = Int32.Parse(p.Fk_Customer_Id)
                         })
                        .ToListAsync();

            var index = 0;
            foreach (var item in listCustomer)
            {
                index++;
                item.Stt = index;
            }
            return View(listCustomer);
        }
        //KH VIP
        public async Task<IActionResult> KHVIP()
        {
            var listCustomer = await _context.Customer
                .Where(d => Int32.Parse(d.Fk_Customer_Id) == (int)RowStatusEnum.VIP)
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
                             CreatedDate = p.CreatedDate.ToString("dd/MM/yyyy"),                         })
                        .ToListAsync();

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
        public async Task<IActionResult> Create([Bind("Id,Code,FullName,Age,Address1,Address2,Telephone,Email,CreatedDate,Active,Fk_Customer_Id")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
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
        public async Task<IActionResult> Edit(string id, [Bind("Id,Code,FullName,Age,Address1,Address2,Telephone,Email,CreatedDate,Active,Fk_Customer_Id")] Customer customer)
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
    public class CustomerViewModel
    {
        public int Stt { get; set; }
        public string Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string Age { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string CreatedDate { get; set; }
        public int FkCustomerId { get; set; }
    }
}
