using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tella_CMS.Efs.Context;
using Tella_CMS.Efs.Entities;

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
            return View(await _context.Oder_view.ToListAsync());
        }

        // GET: Order_Pro/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order_Pro = await _context.Order_Pro
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Create([Bind("Id,Code,Fk_Customer_Id,Fk_Product_Id,Quanlity,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,RowVersion,RowStatus")] Order_Pro order_Pro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order_Pro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order_Pro);
        }

        // GET: Order_Pro/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order_Pro = await _context.Order_Pro.FindAsync(id);
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
        public async Task<IActionResult> Edit(string id, [Bind("Id,Code,Fk_Customer_Id,Fk_Product_Id,Quanlity,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,RowVersion,RowStatus")] Order_Pro order_Pro)
        {
            if (id != order_Pro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order_Pro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Order_ProExists(order_Pro.Id))
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
            return View(order_Pro);
        }

        // GET: Order_Pro/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order_Pro = await _context.Order_Pro
                .FirstOrDefaultAsync(m => m.Id == id);
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


}
