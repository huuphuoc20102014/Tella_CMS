using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tella_CMS.Efs.Context;
using Tella_CMS.Models;

namespace Tella_CMS.Controllers
{
    public class VipCardController : Controller
    {
        private readonly Tella_CMSContext _context;

        public VipCardController(Tella_CMSContext context)
        {
            _context = context;
        }

        // GET: VipCard
        public async Task<IActionResult> Index()
        {
            var listCard = await _context.VIP_Card
                .Where(p=>p.RowStatus == (int)RowStatusEnum.None)
                .OrderBy(p => p.CreatedDate)
                .Select(p => new VipCardViewModel
                {
                    Id = p.Id,
                    Code = p.Code,
                    Seri = p.Seri,
                    CreatedBy = "Admin",
                    CreatedDate = p.CreatedDate.ToString("dd/MM/yyyy"),
                    RowStatus = 0,
                }).ToListAsync();
            var index = 0;
            foreach (var item in listCard)
            {
                index++;
                item.Stt = index;
            }
            return View(listCard);
        }

        // GET: VipCard/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vipCard = await _context.VIP_Card
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vipCard == null)
            {
                return NotFound();
            }

            return View(vipCard);
        }

        // GET: VipCard/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VipCard/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Seri,FkCustomerId,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,RowVersion,RowStatus")] VipCard vipCard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vipCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vipCard);
        }

        // GET: VipCard/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vipCard = await _context.VIP_Card.FindAsync(id);
            if (vipCard == null)
            {
                return NotFound();
            }
            return View(vipCard);
        }

        // POST: VipCard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Code,Seri,FkCustomerId,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,RowVersion,RowStatus")] VipCard vipCard)
        {
            if (id != vipCard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vipCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VipCardExists(vipCard.Id))
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
            return View(vipCard);
        }

        // GET: VipCard/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vipCard = await _context.VIP_Card
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vipCard == null)
            {
                return NotFound();
            }

            return View(vipCard);
        }

        // POST: VipCard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var vipCard = await _context.VIP_Card.FindAsync(id);
            _context.VIP_Card.Remove(vipCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VipCardExists(string id)
        {
            return _context.VIP_Card.Any(e => e.Id == id);
        }
        public async Task<IActionResult> CapThe(string id)
        {
            var item = await _context.VIP_Card
                .Where(p=>p.RowStatus == (int)RowStatusEnum.None)
                .FirstOrDefaultAsync(p=>p.Id == id);
            ViewBag.Seri = item.Seri;
            ViewBag.Id = item.Id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CapThe(VipCardViewModel vm)
        {
            var itemKH = await _context.Customer.FirstOrDefaultAsync(p => p.Id == vm.FkCustomerId);
            var item = await _context.VIP_Card.FirstOrDefaultAsync(p => p.Id == vm.Id);
            item.Fk_Customer_Id = vm.FkCustomerId;
            item.RowStatus = (int)RowStatusEnum.Normal;
            itemKH.Fk_Customer_Id = "VIP01";
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
    public class VipCardViewModel
    {
        public int Stt { get; set; }
        public string Id { get; set; }
        public string Code { get; set; }
        public int Seri { get; set; }
        public string FkCustomerId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int RowStatus { get; set; }
    }
}
