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
    // Generated by NancyModule.dlq for Base.ShippingAddress
    public partial class ShippingAddressModule 
    	: NancyModuleBase<ShippingAddress>
    { 
        public ShippingAddressModule(){
            Hookup("/api/ShippingAddress"); 
        }

        public ShippingAddressModule(IRepository<ShippingAddress> repo) : this()
        {
            Repo = repo; 
        } 
    }
}
