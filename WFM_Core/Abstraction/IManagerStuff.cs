using WFM_Domain.Models;
using WFM_Domain.ViewModel;

namespace WFM_Core.Abstraction
{
    public interface IManagerStuff
    {
        Task<IEnumerable<EmployeeWithSkills>> GetAllEmployees();
        Task<Employee> GetEmployeesById(int candidateId);
        Task <string> AddEmployeeSoftLockTbl(Softlock lockCandidateDetail);
    }
}
