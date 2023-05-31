using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductOrder;
using ProductOrder.Models;

namespace ProductOrder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderItemsController : Controller
    {
        //private readonly ApplicationContext db;

        //public OrderItemsController(ApplicationContext context)
        //{
        //    db = context;
        //}

        ApplicationContext db = new ApplicationContext();

        // GET: OrderItems
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orderItems = await db.OrderItems.ToListAsync();
            if (!orderItems.Any())
            {
                return NotFound();
            }

            return Ok(orderItems);
        }

        //// GET: OrderItems/Details/5
        //[HttpGet]
        //public async Task<IActionResult> Details(Guid? id)
        //{
        //    if (id == null || _context.OrderItems == null)
        //    {
        //        return NotFound();
        //    }

        //    var orderItem = await _context.OrderItems
        //        .Include(o => o.Order)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (orderItem == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(orderItem);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,OrderId,ProductId,Quantity,Price")] OrderItem orderItem)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        orderItem.Id = Guid.NewGuid();
        //        _context.Add(orderItem);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderItem.OrderId);
        //    return View(orderItem);
        //}

        //// GET: OrderItems/Edit/5
        //public async Task<IActionResult> Edit(Guid? id)
        //{
        //    if (id == null || _context.OrderItems == null)
        //    {
        //        return NotFound();
        //    }

        //    var orderItem = await _context.OrderItems.FindAsync(id);
        //    if (orderItem == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderItem.OrderId);
        //    return View(orderItem);
        //}

        //// POST: OrderItems/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Guid id, [Bind("Id,OrderId,ProductId,Quantity,Price")] OrderItem orderItem)
        //{
        //    if (id != orderItem.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(orderItem);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OrderItemExists(orderItem.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderItem.OrderId);
        //    return View(orderItem);
        //}

        //// GET: OrderItems/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null || _context.OrderItems == null)
        //    {
        //        return NotFound();
        //    }

        //    var orderItem = await _context.OrderItems
        //        .Include(o => o.Order)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (orderItem == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(orderItem);
        //}

        //// POST: OrderItems/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    if (_context.OrderItems == null)
        //    {
        //        return Problem("Entity set 'ApplicationContext.OrderItems'  is null.");
        //    }
        //    var orderItem = await _context.OrderItems.FindAsync(id);
        //    if (orderItem != null)
        //    {
        //        _context.OrderItems.Remove(orderItem);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool OrderItemExists(Guid id)
        //{
        //    return (_context.OrderItems?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
