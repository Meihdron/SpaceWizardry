using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Route("api/EsiToken")]
    public class EsiTokenController : Controller
        
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id}")]
        public bool loadToken(int id)
        {
            DataObjects.ESI_Token token = new DataObjects.ESI_Token(id);
            token.refreshToken();
            return token.validateToken();
        }
    }
}