using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ChinaAQIDataCore.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml;
using ChinaAQIDataCore.Utils;
using System.Linq;

namespace ChinaAQIDataCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AQIController : ControllerBase
    {
        private readonly AQIContext _context;
        static readonly HttpClient client = new HttpClient();

        public AQIController(AQIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AQIDTO>>> GetAQIList()
        {
            return await _context.Transcation.ToArrayAsync();
        }

        [HttpGet("/fetch")]
        public async Task<IActionResult> Fetch()
        {
            //var endpoint = new EndpointAddress(new Uri("http://106.37.208.233:20035/emcpublish/ClientBin/Env-CnemcPublish-RiaServices-EnvCnemcPublishDomainService.svc/binary/GetAQIDataPublishLives"));
            // http://192.168.26.180:81/GetAQIDataPublishLives
            HttpResponseMessage response = await client.GetAsync("http://106.37.208.233:20035/emcpublish/ClientBin/Env-CnemcPublish-RiaServices-EnvCnemcPublishDomainService.svc/binary/GetAQIDataPublishLives");
            response.EnsureSuccessStatusCode();
            var output = await response.Content.ReadAsByteArrayAsync();
            var xml = WcfBinaryConverter.ConvertWcfBinaryMessageToXml(output);
            XmlNodeList list = xml.GetElementsByTagName("b:AQIDataPublishLive");

            var set = new HashSet<AQIDTO>(new AQIEqualityComparer());
            
            foreach (XmlElement node in list)
            {
                var aqi = new AQIDTO();
                foreach (var p in aqi.GetType().GetProperties())
                {
                    if (p.Name == "CityPinyin") {
                        p.SetValue(aqi, PinyinUtil.GetPinyin((GetNodeValue(node, "Area"))), null);
                        continue;
                    }

                    p.SetValue(aqi, (GetNodeValue(node, p.Name)), null);
                }
                set.Add(aqi);
            }
      
            _context.Transcation.AddRange(set);
            try {
            await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return NoContent();
        }

        private string GetNodeValue(XmlElement node, string propertie)
        {

            return node.GetElementsByTagName(string.Format("b:{0}", propertie)).Item(0).InnerText;
        }

    }
}
