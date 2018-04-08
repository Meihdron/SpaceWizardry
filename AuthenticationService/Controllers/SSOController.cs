using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace AuthenticationService.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/SSO")]
    public class SSOController : Controller
    {
        
        // GET api/SSO
        [HttpGet]
        public IActionResult Redirect()
        {
            //  return new string[]{ "<html><body>test</body></html>"};
            //return HtmlEncoder.Default.Encode($"Hello {AuthenticationService.Models.Constants.CLIENT_ID}, NumTimes is: {AuthenticationService.Models.Constants.EVE_AUTH_URL}");
            return View(new Models.SSO_Redirect());
        }

        [HttpGet("callback")]
        public string callback(string code)
        {
            try
            {
                DataObjects.ESI_Token token = new DataObjects.ESI_Token(code);
                return "all done";
            }
            catch (Exception e)
            {
                return "Token could not be recieved: " + e.Message;
            }
        }
        
       
        //// POST api/SSO
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/SSO/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/SSO/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

    }
}
