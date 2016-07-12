using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using Nancy;
using Newtonsoft.Json;
using WhamBase;
using Wham.Base.NancyDoc;

namespace Wham.Base.NancyDoc
{
    // Generated by NancyModuleDocBase.dlq for Base.Address
    public partial class AddressModuleDoc : NancyModuleDocBase
    { 
        public override string ViewResource
        {
            get
            {
                return "Address.doc.html";
            } 
        }

        public AddressModuleDoc(){
            Hookup("/docs/Address"); 
        } 
    }
}