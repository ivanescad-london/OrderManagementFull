using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Data;
using OrderSystem.DTOs;
using OrderSystem.Models;
using System.Diagnostics;

namespace OrderSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("GetAll")]
    public async Task<IEnumerable<OrderReadDto>> GetOrders()
    {
        Debug.WriteLine($".\nOrder Get All ");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        return await _context.Orders
            .Select(c => new OrderReadDto
            {
                Id = c.Id,
                OrderDate = c.OrderDate,
                Client = c.Client.Name,
                Supplier = c.Supplier.Name,
                Goods = c.Goods.Name,
                Quantity = c.Quantity,
            })
            .ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    [HttpGet("GetBySupplier/{id}")]
    public async Task<IEnumerable<OrderReadDto>> GetOrdersBySupplier(int id)
    {
        Debug.WriteLine($"Get Orders Supplier={id}  All_Orders={_context.Orders.Count()}");

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var list = _context.Orders
            .Where(p => p.SupplierId == id)
            .Select(c => new OrderReadDto
            {
                Id = c.Id,
                OrderDate = c.OrderDate,
                Client = c.Client.Name,
                Supplier = c.Supplier.Name,
                Goods = c.Goods.Name,
                Quantity = c.Quantity,
            })
            .ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        Debug.WriteLine($"Get Orders : Supplier Orders.Count={list.Count()}");

        return list;
    }

    [HttpGet("GetByClient/{id}")]
    public async Task<IEnumerable<OrderReadDto>> GetOrdersByClient(int id)
    {
        Debug.WriteLine($".\nGet Orders Client={id}  All_Orders={_context.Orders.Count()}");

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        List<OrderReadDto> list = _context.Orders
            .Where(p => p.ClientId == id)
            .Select(c => new OrderReadDto
            {
                Id = c.Id,
                OrderDate = c.OrderDate,
                Client = c.Client.Name,
                Supplier = c.Supplier.Name,
                Goods = c.Goods.Name,
                Quantity = c.Quantity,
            })
            .ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        Debug.WriteLine($"Get Orders : Client Orders.Count={list.Count()}");

        return list;
    }

    [HttpPost("Create")]
    public async Task<ActionResult<OrderReadDto>> Create(OrderCreateDto dto)
    {
        Debug.WriteLine($".\nOrder Create");
        var order = new Order
        {
            OrderDate = dto.OrderDate,
            ClientId = dto.ClientId,
            SupplierId = dto.SupplierId,
            GoodsId = dto.GoodsId,
            Quantity = dto.Quantity,
        };

        _context.Orders.Add(order);
        Debug.WriteLine($"Create order Quantity={order.Quantity} Orders={_context.Orders.Count()}");
        await _context.SaveChangesAsync();

        // Reload with relationships
        var createdOrder = await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.Supplier)
            .Include(o => o.Goods)
            .FirstAsync(o => o.Id == order.Id);

        return new OrderReadDto
        {
            Id = order.Id,
            OrderDate = createdOrder.OrderDate,
            Client = createdOrder.Client!.Name,
            Supplier = createdOrder.Supplier!.Name,
            Goods = createdOrder.Goods!.Name,
            Quantity = createdOrder.Quantity,
        };
    }

    [HttpGet("Read/{id}")]
    public async Task<ActionResult<OrderReadDto>> GetOrder(int id)
    {
        Debug.WriteLine($".\nOrder Read {id}");
        //var order = await _context.Orders.FindAsync(id);
        var order = await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.Supplier)
            .Include(o => o.Goods)
            .FirstAsync(o => o.Id == id);

        Debug.WriteLine($"order == null  : {order == null}");
#pragma warning disable CS8604 // Possible null reference argument.
        return order == null ? NotFound() : new OrderReadDto
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Client = order.Client!.Name,
            Supplier = order.Supplier!.Name,
            Goods= order.Goods!.Name,
            Quantity = order.Quantity,
        };
#pragma warning restore CS8604 // Possible null reference argument.
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(int id, Order order)
    {
        Debug.WriteLine($".\nOrder Update id={id} order.Id={order.Id}");
        if (id != order.Id) return BadRequest();

        _context.Entry(order).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Debug.WriteLine($".\nOrder Delete {id}");
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            Debug.WriteLine($"Order R: Delete id={id} Orders={_context.Orders.Count()} return false");
            return BadRequest();
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        Debug.WriteLine($"Order R: Delete id={id} Orders={_context.Orders.Count()} return true");
        return NoContent();
    }
}
