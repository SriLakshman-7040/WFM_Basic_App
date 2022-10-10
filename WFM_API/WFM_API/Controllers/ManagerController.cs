using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WFM_Core.Abstraction;
using WFM_Domain.Models;
using WFM_Domain.ViewModel;

namespace WFM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerStuff _managerStuff;
        public ManagerController(IManagerStuff managerStuff)
        {
            _managerStuff = managerStuff;
        }

        [Route("CandidateList")]
        [HttpGet]
        public async Task<ActionResult<EmployeeWithSkills>> GetNotRequestedList()
        {
            try
            {
                var notReqCandidateList = await _managerStuff.GetAllEmployees();
                if (notReqCandidateList == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { responseMessage = "Not-Available for Not_Requested Status Candidate List" });
                return StatusCode(StatusCodes.Status200OK, notReqCandidateList.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [Route(template:"LockCandidateAddSoftLock")]
        [HttpPost]
        public async Task<ActionResult> AddEmployeeSoftLockTbl([FromBody] Softlock softLockEmployee)
        {
            try
            {
                var candidateLockedorNot = await _managerStuff.AddEmployeeSoftLockTbl(softLockEmployee);
                if (candidateLockedorNot is null)
                    return StatusCode(StatusCodes.Status400BadRequest, new { responseMessage = "Given Employee Details are not valid" });
                return StatusCode(StatusCodes.Status201Created , candidateLockedorNot);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
