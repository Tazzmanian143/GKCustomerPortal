using GKCustomerPortal.Model;

namespace GKCustomerPortal.Services;

public interface ICustomerService
{
    Task<CustomerModel?> GetByIdAsync(int id);
    Task<(IEnumerable<CustomerModel> Items, int TotalCount)> GetAllAsync(string? firstNameFilter, int page, int pageSize);
    Task<CustomerModel> CreateAsync(CustomerModel customer);
    Task<CustomerModel?> UpdateAsync(int id, CustomerModel customer);
    Task<bool> DeleteAsync(int id);
}