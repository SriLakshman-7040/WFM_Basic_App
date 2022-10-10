using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WFM_Domain.Models;
using WFM_Domain.ViewModel;
using WFM_Web.Helpers;

namespace WFM_Web.Controllers
{
    public class MemberMvcController : Controller
    {
        readonly ApiBaseUrl baseUrl = new();
        private readonly string apiBaseUrl = "https://localhost:7276/api/";
        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult<List<SoftLockDto>>> MemberLandingPage()
        {
            List<SoftLockDto> watingReqCandidate = new List<SoftLockDto>();
            var accessToken = HttpContext.Session.GetString("JWToken");
            HttpClient clientReq = new HttpClient();
            clientReq.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string employeeResult = await clientReq.GetStringAsync($"{apiBaseUrl}Member/ReqWaitingCandidates");
            watingReqCandidate = JsonConvert.DeserializeObject<List<SoftLockDto>>(employeeResult).ToList();
            return View(watingReqCandidate?.ToList());
            /*
            HttpClient client = baseUrl.InitialClientMethod();
            HttpResponseMessage response = await client.GetAsync("api/Member/ReqWaitingCandidates");
            if(response.IsSuccessStatusCode)
            {
                var reqWaitingList = response.Content.ReadAsStringAsync().Result;
                watingReqCandidate = JsonConvert.DeserializeObject<List<SoftLockDto>>(reqWaitingList);
            }
            return View(watingReqCandidate?.ToList());
            */
        }

        [HttpPost]
        public async Task<ActionResult> CandidateReqProcess([FromBody] Softlock candidateLock)
        {
            if (candidateLock.EmployeeId != null)
            {
                var accessToken = HttpContext.Session.GetString("JWToken");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var stringContent = new StringContent(JsonConvert.SerializeObject(candidateLock), Encoding.UTF8, "application/json");
                var postReq = await client.PostAsync($"{apiBaseUrl}Member/CandidateReqConfirmProcess", stringContent);
                var code = postReq.StatusCode.ToString();
                if (code == "Created")
                {
                    return Ok(code); 
                }
                return RedirectToAction("MemberLandingPage");

                /*HttpClient client = baseUrl.InitialClientMethod();
                var postReq = client.PostAsJsonAsync<Softlock>("api/Member/CandidateReqConfirmProcess", candidateLock);
                postReq.Wait();
                var userDetail = postReq.Result;
                var code = userDetail.StatusCode.ToString();
                if (code == "Created")
                {
                    return Ok(code); //RedirectToAction("MemberLandingPage");
                }
                return RedirectToAction("MemberLandingPage");*/
            }
            else
            {
                return null;
            }
        }
    }
}
