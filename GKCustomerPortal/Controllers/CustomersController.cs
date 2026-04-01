using GKCustomerPortal.Model;
using GKCustomerPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GKCustomerPortal.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? firstName, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var (items, total) = await _customerService.GetAllAsync(firstName, page, pageSize);

        // Adding metadata to headers is a "Senior" move for paging
        Response.Headers.Append("X-Total-Count", total.ToString());

        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        return customer == null ? NotFound() : Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CustomerModel customer)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var created = await _customerService.CreateAsync(customer);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {            
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CustomerModel customer)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updated = await _customerService.UpdateAsync(id, customer);
            return updated == null ? NotFound() : Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _customerService.DeleteAsync(id);
        if (!success) return NotFound();

        return NoContent(); // Returns standard 204 No Content
    }
}