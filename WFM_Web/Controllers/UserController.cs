using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WFM_Web.Helpers;
using WFM_Web.Models;

namespace WFM_Web.Controllers
{
    public class UserController : Controller
    {
        readonly ApiBaseUrl baseUrl = new();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LogInPage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogInPage(UserLoginModel userLogin)
        {
            //HttpClient client = baseUrl.InitialClientMethod();
            using (var httpClient = new HttpClient())
            {
                var code = string.Empty;
                StringContent content = new StringContent(JsonConvert.SerializeObject(userLogin),Encoding.UTF8,"application/json");
                using (var responseResult = await httpClient.PostAsync("https://localhost:7276/api/Users/Authenticate", content))
                {
                    code = responseResult.StatusCode.ToString();
                    string accessToken = await responseResult.Content.ReadAsStringAsync();
                    HttpContext.Session.SetString("JWToken", accessToken);
                }
                 
                if (code == "OK")
                {                    
                    return RedirectToAction("ManagerLandingPage", "Manager");
                }
                else if (code == "Accepted")
                {
                    return RedirectToAction("MemberLandingPage", "MemberMvc");
                }
                else
                {
                    return Unauthorized();
                }
            }
            /*HttpClient client = baseUrl.InitialClientMethod();
            var postReq = client.PostAsJsonAsync<UserLoginModel>("api/Users/Authenticate", userLogin);
            postReq.Wait();
            var userDetail = postReq.Result;
            var code = userDetail.StatusCode.ToString();
            if (code == "OK")
            {
                //return View("ManagerLandingPage");
                return RedirectToAction("ManagerLandingPage", "Manager");
            }
            else if (code == "Accepted")
            {
                return RedirectToAction("MemberLandingPage", "MemberMvc");
            }
            else
            {
                return Unauthorized();
            }*/
        }

        public IActionResult UserRegistration()
        {
            return View();
        }
    }
}
