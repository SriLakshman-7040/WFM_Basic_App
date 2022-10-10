using WFM_Domain.Models;
using WFM_Domain.ViewModel;

namespace WFM_Core.Abstraction
{
    public interface IMembersStuff
    {
        Task<List<Softlock>> GetReqWaitingCandidates();
        Task<string> CandidateLockReqProcess(Softlock LockReqProcess);
    }
}
