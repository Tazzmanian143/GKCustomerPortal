using GKCustomerPortal.Data;
using GKCustomerPortal.Model;
using Microsoft.EntityFrameworkCore;

namespace GKCustomerPortal.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<CustomerModel> Items, int TotalCount)> GetAllAsync(string? firstName, int page, int pageSize)
    {
        // ASSESMENT NOTE: Because PII is encrypted in the database, SQL cannot perform a 'LIKE' (.Contains) operation.
        // To maintain partial search functionality for this assessment, records are pulled into memory to decrypt.
        // In a high-scale production environment, a hashed searchable column or deterministic exact-match encryption would be used.

        var allCustomers = await _context.Customers.ToListAsync();

        // Filtering (In-Memory)
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            allCustomers = allCustomers
                .Where(c => c.FirstName != null && c.FirstName.Contains(firstName, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        int totalCount = allCustomers.Count;

        // Paging (In-Memory)
        var items = allCustomers
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        return (items, totalCount);
    }

    public async Task<CustomerModel> CreateAsync(CustomerModel customer)
    {
        if (customer.Age < 0) throw new ArgumentException("Age cannot be negative.");

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<CustomerModel?> GetByIdAsync(int id) =>
        await _context.Customers.FindAsync(id);

    public async Task<CustomerModel?> UpdateAsync(int id, CustomerModel customer)
    {
        var existing = await _context.Customers.FindAsync(id);
        if (existing == null) return null;

        existing.FirstName = customer.FirstName;
        existing.LastName = customer.LastName;
        existing.Email = customer.Email;
        existing.Age = customer.Age;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return false;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return true;
    }
}