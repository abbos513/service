using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WAD_CW2_00003905.DAL.Repositories;

namespace WAD_CW2_00003905.Controllers
{
    public class EmailApiController : ApiController
    {
        [HttpGet]
        public bool EmailExsists(string email)
        {
            return new UserRepository().GetAll().Any(u => u.Email == email);
        }

        [HttpGet]
        public bool UsernameExsists(string username)
        {
            return new UserRepository().GetAll().Any(u => u.Username == username);
        }


    }
}
