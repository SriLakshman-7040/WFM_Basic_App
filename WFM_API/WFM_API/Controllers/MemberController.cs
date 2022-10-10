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
    public class MemberController : ControllerBase
    {
        private readonly IMembersStuff _membersStuff;
        public MemberController(IMembersStuff membersStuff)
        {
            _membersStuff = membersStuff;
        }
        [Route("ReqWaitingCandidates")]
        [HttpGet]
        public async Task<ActionResult<SoftLockDto>> GetWaitingReqCandidates()
        {
            try
            {
                var reqWaitingCandidates = await _membersStuff.GetReqWaitingCandidates();
                if (reqWaitingCandidates == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { responseMessage = "There is no candidates in Awaiting_Confirmation status" });
                return StatusCode(StatusCodes.Status200OK, reqWaitingCandidates.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Route(template: "CandidateReqConfirmProcess")]
        [HttpPost]
        public async Task<ActionResult> CandidateReqProcess([FromBody] Softlock softLockEmployee)
        {
            try
            {
                var candidateLockedorNot = await _membersStuff.CandidateLockReqProcess(softLockEmployee);
                if (candidateLockedorNot is null)
                    return StatusCode(StatusCodes.Status400BadRequest, new { responseMessage = "Given Employee Details are not valid" });
                return StatusCode(StatusCodes.Status201Created, candidateLockedorNot);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
