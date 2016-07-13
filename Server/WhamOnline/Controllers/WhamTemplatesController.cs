using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace WhamOnline.Controllers
{
    public class WhamTemplatesController : ApiController
    {
        // GET: api/WhamGenerator
        public IEnumerable<string> Get()
        {
           return Global.InitWhamTemplateResolver(); 
        } 
    }
}
