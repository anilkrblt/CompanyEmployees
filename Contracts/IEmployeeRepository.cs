using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetEmployees(
            Guid companyId,
            EmployeeParameters employeeParameters,
            bool trackChanges
        );
        Employee GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
        void CreateEmployeeForCompany(Guid companyId, Employee employee);
        void DeleteEmployee(Employee employee);
    }
}
