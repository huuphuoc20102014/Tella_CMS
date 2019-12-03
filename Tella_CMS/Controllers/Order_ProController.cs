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
    public class Order_ProController : Controller
    {
        private readonly Tella_CMSContext _context;

        public Order_ProController(Tella_CMSContext context)
        {
            _context = context;
        }

        // GET: Order_Pro
        public async Task<IActionResult> Index()
        {
            return View(await _context.Oder_view.OrderBy(p=>p.NgayMua).ToListAsync());
        }

        // GET: Order_Pro/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order_Pro = await _context.Oder_view
                .FirstOrDefaultAsync(m => m.Code == id);
            if (order_Pro == null)
            {
                return NotFound();
            }

            return View(order_Pro);
        }

        // GET: Order_Pro/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Order_Pro/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( OrderViewModel vm)
        {
            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            // Check code is existed
            if (await _context.Order_Pro.AnyAsync(h => h.Code == vm.Code))
            {
                return View(vm);
            }

            // Create save db item


            var dbItem = new Order_Pro
            {
                Id = Guid.NewGuid().ToString(),
                Code = vm.Code,
                Fk_Customer_Id = vm.Fk_Customer_Id,
                Fk_Product_Id = vm.Fk_Product_Id,
                CreatedBy = "System",
                Quanlity = vm.Quanlity,
                CreatedDate = DateTime.UtcNow,
                RowStatus = (int)RowStatusEnum.Normal
            };
            _context.Add(dbItem);
            // Set time stamp for table to handle concurrency conflict
            //tableVersion.Action = 0;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Order_ProController.Details), new { id = dbItem.Id });
        }

        // GET: Order_Pro/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order_Pro = _context.Order_Pro.Where(p => p.Code == id).FirstOrDefault();
            if (order_Pro == null)
            {
                return NotFound();
            }
            return View(order_Pro);
        }

        // POST: Order_Pro/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrderViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var dbItem = await _context.Order_Pro
                .Where(h => h.Code == vm.Code)
                .FirstOrDefaultAsync();
            // Update db item    
            dbItem.Code = vm.Code;
            dbItem.Fk_Customer_Id = vm.Fk_Customer_Id;
            dbItem.Fk_Product_Id = vm.Fk_Product_Id;
            dbItem.Quanlity = vm.Quanlity;

            //_dbContext.Entry(dbItem).Property(nameof(Customer.RowVersion)).OriginalValue = vmItem.RowVersion;
            // Set time stamp for table to handle concurrency conflict
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Order_ProController.Details), new { id = dbItem.Code });
        }

        // GET: Order_Pro/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order_Pro = await _context.Oder_view
                .FirstOrDefaultAsync(m => m.Code == id);
            if (order_Pro == null)
            {
                return NotFound();
            }

            return View(order_Pro);
        }

        // POST: Order_Pro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var order_Pro = await _context.Order_Pro.FindAsync(id);
            _context.Order_Pro.Remove(order_Pro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Order_ProExists(string id)
        {
            return _context.Order_Pro.Any(e => e.Id == id);
        }
    }
    public class OrderViewModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Fk_Customer_Id { get; set; }
        public string Fk_Product_Id { get; set; }
        public int? Quanlity { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int RowStatus { get; set; }
    }
}


