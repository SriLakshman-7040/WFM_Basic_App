using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM_Core.Abstraction;
using WFM_Domain.Models;
using WFM_Domain.ViewModel;

namespace WFM_Service.Services
{
    public class MemberService : IMembersStuff
    {
        private readonly WfmDbContext _dbContext;
        public MemberService(WfmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CandidateLockReqProcess(Softlock LockReqProcess)
        {
            if (LockReqProcess.Status == "Approve")
            {
                var softLockCandidateDetail = _dbContext.Softlocks.FirstOrDefault(e => e.EmployeeId == LockReqProcess.EmployeeId);//.FindAsync(LockReqProcess.EmployeeId);
                if (softLockCandidateDetail != null)
                {
                    softLockCandidateDetail.Status = LockReqProcess.Status;
                    softLockCandidateDetail.EmployeeId = LockReqProcess.EmployeeId;
                    softLockCandidateDetail.RequestMessage = LockReqProcess.RequestMessage;
                    softLockCandidateDetail.Manager = LockReqProcess.Manager;
                    softLockCandidateDetail.LastUpdated = DateTime.UtcNow;
                    softLockCandidateDetail.ManagerStatus = "Locked";
                    int result = await _dbContext.SaveChangesAsync();
                    if (result > 0)
                    {
                        var empDetail = await _dbContext.Employees.FindAsync(LockReqProcess.EmployeeId);
                        if (empDetail != null)
                        {
                            empDetail.LockStatus = "Locked";
                            await _dbContext.SaveChangesAsync();
                            return "Request Approved Successfully..! ";
                        }
                        return "Oops..! Employee Table doesn't contain given candidate details";
                    }
                }
                return "Oops..! SoftLock Table doesn't contain given candidate details";
            }
            else
            {
                var softLockCandidateDetail = _dbContext.Softlocks.FirstOrDefault(e => e.EmployeeId == LockReqProcess.EmployeeId);
                if (softLockCandidateDetail != null)
                {
                    softLockCandidateDetail.Status = LockReqProcess.Status;
                    softLockCandidateDetail.EmployeeId = LockReqProcess.EmployeeId;
                    softLockCandidateDetail.RequestMessage = LockReqProcess.RequestMessage;
                    softLockCandidateDetail.LastUpdated = DateTime.UtcNow;
                    softLockCandidateDetail.Manager = LockReqProcess.Manager;

                    int result = await _dbContext.SaveChangesAsync();
                    if (result > 0)
                    {
                        var empDetail = await _dbContext.Employees.FindAsync(LockReqProcess.EmployeeId);
                        if (empDetail != null)
                        {
                            empDetail.LockStatus = "Not_Requested";
                            await _dbContext.SaveChangesAsync();
                            return "Request Rejected";
                        }
                        return "Oops..! Employee Table doesn't contain given candidate details";
                    }
                }
                return "Oops..! SoftLock Table doesn't contain given candidate details";
            }
        }

        public async Task<List<Softlock>> GetReqWaitingCandidates()
        {            
           var reqWaitingCandidates = await _dbContext.Softlocks.Where(s => s.ManagerStatus == "Awaiting_Confirmation").ToListAsync();
            if(!reqWaitingCandidates.Any())
                return null;
            return reqWaitingCandidates.ToList();
        }
    }
}
