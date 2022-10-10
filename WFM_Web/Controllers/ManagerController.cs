using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WFM_Domain.Models;
using WFM_Domain.ViewModel;
using WFM_Web.Helpers;

namespace WFM_Web.Controllers
{
    public class ManagerController : Controller
    {
        readonly ApiBaseUrl baseUrl = new();

        private readonly string apiBaseUrl = "https://localhost:7276/api/";
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult<List<EmployeeWithSkills>>> ManagerLandingPage()
        {
            List<EmployeeWithSkills> employeeWithSkills = new();
            var accessToken = HttpContext.Session.GetString("JWToken");            
            HttpClient clientReq = new HttpClient();
            clientReq.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",accessToken);
            string employeeResult = await clientReq.GetStringAsync($"{apiBaseUrl}Manager/CandidateList");
            employeeWithSkills = JsonConvert.DeserializeObject<List<EmployeeWithSkills>>(employeeResult).ToList();
            return View(employeeWithSkills?.ToList());

            /*List<EmployeeWithSkills> employeeWithSkills = new();
            HttpClient client = baseUrl.InitialClientMethod();
            HttpResponseMessage response = await client.GetAsync("api/Manager/CandidateList");
            if (response.IsSuccessStatusCode)
            {
                var candidateList = response.Content.ReadAsStringAsync().Result;
                employeeWithSkills = JsonConvert.DeserializeObject<List<EmployeeWithSkills>>(candidateList);
                return View(employeeWithSkills?.ToList());
            }
            else if(response.StatusCode.ToString().Equals("Unauthorized"))
            {
                return View("Unauthorized");
            }
            else
            {
                return Unauthorized();
            }*/

        }
        [HttpPost]
        public async Task<ActionResult> LockCandidateAddSoftLock([FromBody] Softlock candidateLock)
        {
            if (candidateLock.EmployeeId != null)
            {
                var accessToken = HttpContext.Session.GetString("JWToken");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var stringContent = new StringContent(JsonConvert.SerializeObject(candidateLock),Encoding.UTF8,"application/json");
                var postReq = await client.PostAsync($"{apiBaseUrl}Manager/LockCandidateAddSoftLock", stringContent);                
                var code = postReq.StatusCode.ToString();
                if (code == "Created")
                {
                    return Ok(code); 
                }
                return RedirectToAction("ManagerLandingPage");

                /*HttpClient client = baseUrl.InitialClientMethod();
                var postReq = client.PostAsJsonAsync<Softlock>("api/Manager/LockCandidateAddSoftLock", candidateLock);
                postReq.Wait();
                var userDetail = postReq.Result;
                var code = userDetail.StatusCode.ToString();
                if (code == "Created")
                {
                    return Ok(code); //RedirectToAction("ManagerLandingPage");
                }
                return RedirectToAction("ManagerLandingPage");
                */
            }
            else
            {
                return null;
            }
        }
    }
}
