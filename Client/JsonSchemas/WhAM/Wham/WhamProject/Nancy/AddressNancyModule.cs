using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using Nancy;
using Newtonsoft.Json;
using WhamBase;
using Wham.Base.Nancy;
using Wham.Base.Model;

namespace Wham.Base.Nancy
{
    // Generated by NancyModule.dlq for Base.Address
    public partial class AddressModule 
    	: NancyModuleBase<Address>
    { 
        public AddressModule(){
            Hookup("/api/Address"); 
        }

        public AddressModule(IRepository<Address> repo) : this()
        {
            Repo = repo; 
        } 
    }
}