using Microsoft.EntityFrameworkCore;
using WFM_Core.Abstraction;
using WFM_Domain.Models;
using WFM_Domain.ViewModel;

namespace WFM_Service.Services
{
    public class ManagerService : IManagerStuff
    {
        private readonly WfmDbContext _dbContext;
        public ManagerService(WfmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> AddEmployeeSoftLockTbl(Softlock lockCandidateDetail)
        {
            var candidateDetail = new Softlock()
            {
               EmployeeId = lockCandidateDetail.EmployeeId,
               Manager = lockCandidateDetail.Manager,
               ReqDate = DateTime.UtcNow,   
               Status = lockCandidateDetail.Status,
               LastUpdated = lockCandidateDetail.LastUpdated,              
               RequestMessage = lockCandidateDetail.RequestMessage,
               WfmRemark = lockCandidateDetail.WfmRemark,
               ManagerStatus = lockCandidateDetail.ManagerStatus,
               ManagerStatusComment = lockCandidateDetail.ManagerStatusComment,
               MgrLastUpdate = lockCandidateDetail.MgrLastUpdate
            };
            await _dbContext.Softlocks.AddAsync(candidateDetail);
            int result = await _dbContext.SaveChangesAsync();
            if (result > 0)
            {
                var empDetail = await _dbContext.Employees.FindAsync(lockCandidateDetail.EmployeeId);
                if (empDetail != null)
                {
                    empDetail.LockStatus = "Request_Waiting";
                    await _dbContext.SaveChangesAsync();
                    return "Manager Request Created Successfully..! ";
                }
            }
            return "Oops..! Something is wrong";
        }

        public async Task<IEnumerable<EmployeeWithSkills>> GetAllEmployees()
        {
            var employeeDetails = await _dbContext.Employees.Include(e => e.SkillMaps).ThenInclude(s => s.Skill).Select(es => new EmployeeWithSkills
            {
                EmployeeId = es.EmployeeId,
                Name = es.Name,
                Status = es.Status,
                Manager = es.Manager,
                Wfm_Manager = es.WfmManager,
                Email = es.Email,
                LockStatus = es.LockStatus,
                Experience = es.Experience,
                Skills = es.SkillMaps.Select(s => s.Skill.Name).ToList()
            }).Where(l => l.LockStatus == "Not_Requested").ToListAsync();

            return employeeDetails;
        }

        public async Task<Employee> GetEmployeesById(int candidateId)
        {
            var employeeDetail = await _dbContext.Employees.SingleOrDefaultAsync(e => e.EmployeeId == candidateId);
            if(employeeDetail == null)
                return null;    
            return employeeDetail;
        }


    }
}
