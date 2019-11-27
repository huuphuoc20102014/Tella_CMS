using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tella_CMS.Models;

namespace Tella_CMS.Controllers
{
    public class CustomersController : Controller
    {
        private readonly Tella_CMSContext _context;

        public CustomersController(Tella_CMSContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public ActionResult Data_Read([DataSourceRequest] DataSourceRequest request)
        {

            // Convert to view model to avoid JSON serialization problems due to circular references.
            IQueryable<CustomerViewModel> orders = _context.Customer
                .OrderBy(p => p.CreatedDate)
                .Select(p => new CustomerViewModel
            {
                Id = p.Id,
                Code = p.Code,
                FullName = p.FullName,
                Address1 = p.Address1,
                Address2 = p.Address2,
                Age  = p.Age,
                Telephone = p.Telephone,
            });
            
            //orders = orders.ApplyOrdersFiltering(request.Filters);

            var total = orders.Count();

            //orders = orders.ApplyOrdersSorting(request.Groups, request.Sorts);

            if (request.Sorts != null && !request.Sorts.Any())
            {
                // Entity Framework doesn't support paging on unsorted data.
                orders = orders.OrderBy(o => o.CreatedDate);
            }

            orders = orders.ApplyOrdersPaging(request.Page, request.PageSize);
            var index = 0;
            foreach (var item in orders)
            {
                index++;
                item.Stt = index;
            }
            IEnumerable data = orders.ApplyOrdersGrouping(request.Groups);

            var result = new DataSourceResult()
            {
                Data = data,
                Total = total
            };

            return Json(result);
        }
        // GET: Customers/Details/5
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

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,FullName,Age,Address1,Address2,Telephone,Email,CreatedDate,Active,FkCustomerId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
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

        // POST: Customers/Edit/5
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

        // GET: Customers/Delete/5
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

        // POST: Customers/Delete/5
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
    public static class AjaxCustomBindingExtensions
    {
        public static IEnumerable ApplyOrdersGrouping(this IQueryable<CustomerViewModel> data, IList<GroupDescriptor>
            groupDescriptors)
        {
            if (groupDescriptors != null && groupDescriptors.Any())
            {
                Func<IEnumerable<CustomerViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;
                foreach (var group in groupDescriptors.Reverse())
                {
                    if (selector == null)
                    {
                        if (group.Member == "Id")
                        {
                            selector = Orders => BuildInnerGroup(Orders, o => o.Id);
                        }
                        else if (group.Member == "CreatedDate")
                        {
                            selector = Orders => BuildInnerGroup(Orders, o => o.CreatedDate);
                        }
                        else if (group.Member == "SoTien")
                        {
                            selector = Orders => BuildInnerGroup(Orders, o => o.Telephone);
                        }
                        else if (group.Member == "Code")
                        {
                            selector = Orders => BuildInnerGroup(Orders, o => o.Code);
                        }
                        else if (group.Member == "FullName")
                        {
                            selector = Orders => BuildInnerGroup(Orders, o => o.FullName);
                        }
                    }
                    else
                    {
                        if (group.Member == "Id")
                        {
                            selector = BuildGroup(o => o.Id, selector);
                        }
                        else if (group.Member == "CreatedDate")
                        {
                            selector = BuildGroup(o => o.CreatedDate, selector);
                        }
                        else if (group.Member == "Code")
                        {
                            selector = BuildGroup(o => o.Code, selector);
                        }
                        else if (group.Member == "FullName")
                        {
                            selector = BuildGroup(o => o.FullName, selector);
                        }
                    }
                }

                return selector.Invoke(data).ToList();
            }

            return data.ToList();
        }
        private static Func<IEnumerable<CustomerViewModel>, IEnumerable<AggregateFunctionsGroup>>
            BuildGroup<T>(Expression<Func<CustomerViewModel, T>> groupSelector, Func<IEnumerable<CustomerViewModel>,
            IEnumerable<AggregateFunctionsGroup>> selectorBuilder)
        {
            var tempSelector = selectorBuilder;
            return g => g.GroupBy(groupSelector.Compile())
                         .Select(c => new AggregateFunctionsGroup
                         {
                             Key = c.Key,
                             HasSubgroups = true,
                             Member = groupSelector.MemberWithoutInstance(),
                             Items = tempSelector.Invoke(c).ToList()
                         });
        }
        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<CustomerViewModel>
            group, Expression<Func<CustomerViewModel, T>> groupSelector)
        {
            return group.GroupBy(groupSelector.Compile())
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = groupSelector.MemberWithoutInstance(),
                        Items = i.ToList()
                    });
        }
        public static IQueryable<CustomerViewModel> ApplyOrdersSorting(this IQueryable<CustomerViewModel> data,
                    IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
        {
            if (groupDescriptors != null && groupDescriptors.Any())
            {
                foreach (var groupDescriptor in groupDescriptors.Reverse())
                {
                    data = AddSortExpression(data, groupDescriptor.SortDirection, groupDescriptor.Member);
                }
            }

            if (sortDescriptors != null && sortDescriptors.Any())
            {
                foreach (SortDescriptor sortDescriptor in sortDescriptors)
                {
                    data = AddSortExpression(data, sortDescriptor.SortDirection, sortDescriptor.Member);
                }
            }

            return data;
        }
        private static IQueryable<CustomerViewModel> AddSortExpression(IQueryable<CustomerViewModel> data, ListSortDirection
                    sortDirection, string memberName)
        {
            if (sortDirection == ListSortDirection.Ascending)
            {
                switch (memberName)
                {
                    case "Id":
                        data = data.OrderBy(order => order.Id);
                        break;
                    case "CreatedDate":
                        data = data.OrderBy(order => order.CreatedDate);
                        break;
                    case "Code":
                        data = data.OrderBy(order => order.Code);
                        break;
                    case "FullName":
                        data = data.OrderBy(order => order.FullName);
                        break;
                }
            }
            else
            {
                switch (memberName)
                {
                    case "Id":
                        data = data.OrderByDescending(order => order.Id);
                        break;
                    case "CreatedDate":
                        data = data.OrderByDescending(order => order.CreatedDate);
                        break;
                    case "Code":
                        data = data.OrderByDescending(order => order.Code);
                        break;
                    case "FullName":
                        data = data.OrderByDescending(order => order.FullName);
                        break;
                }
            }
            return data;
        }
        public static IQueryable<CustomerViewModel> ApplyOrdersPaging(this IQueryable<CustomerViewModel> data, int page, int pageSize)
        {
            if (pageSize > 0 && page > 0)
            {
                data = data.Skip((page - 1) * pageSize);
            }

            data = data.Take(pageSize);

            return data;
        }
        public static IQueryable<CustomerViewModel> ApplyOrdersFiltering(this IQueryable<CustomerViewModel> data,
       IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors != null && filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<CustomerViewModel>(filterDescriptors, false));
            }
            return data;
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
        public string FkCustomerId { get; set; }
    }
}
