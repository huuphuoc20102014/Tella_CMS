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
    public class ProductsController : Controller
    {
        private readonly Tella_CMSContext _context;

        public ProductsController(Tella_CMSContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var listProducts = await _context.Product
                .OrderBy(p => p.CreatedDate)
                .Where(p => p.RowStatus == (int)RowStatusEnum.Normal)
                .Select(p => new ProductsViewModel
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                    Price = p.Price,
                    PriceNew = p.PriceNew,
                    FkCategoryId = p.FkCategoryId,
                          
                }).ToListAsync();

            var index = 0;
            foreach (var item in listProducts)
            {
                index++;
                item.STT = index;
            }
            return View(listProducts);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,Slug_Name,AutoSlug,FkCategoryId,ShortDescription_Html,LongDescription_Html,Color,Size,Material,Style,Price,PriceNew,CCY,Country,Producer,Status,ImageSlug,Rating,CountView,IsService,Tags,KeyWord,MetaData,Note,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,RowVersion,RowStatus")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Code,Name,Slug_Name,AutoSlug,FkCategoryId,ShortDescription_Html,LongDescription_Html,Color,Size,Material,Style,Price,PriceNew,CCY,Country,Producer,Status,ImageSlug,Rating,CountView,IsService,Tags,KeyWord,MetaData,Note,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,RowVersion,RowStatus")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(string id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
    public class ProductsViewModel
    {
        public int STT { get; set; }
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Slug_Name { get; set; }
        public bool AutoSlug { get; set; }
        public string FkCategoryId { get; set; }
        public string ShortDescription_Html { get; set; }
        public string LongDescription_Html { get; set; }
        public string Style { get; set; }
        public double Price { get; set; }
        public double? PriceNew { get; set; }
        public string ImageSlug { get; set; } 
        public string Tags { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int RowStatus { get; set; }
    }
}
