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


        [HttpGet("{cityNamePinyin}")]
        public async Task<ActionResult<IEnumerable<AQIDTO>>> GetCityAQI(string cityNamePinyin)
        {
            var latestOne =  _context.Transcation
                .Where(c => c.CityPinyin.StartsWith(cityNamePinyin.ToUpper()))
                .FirstOrDefault();
            if (latestOne == null) {

                return NotFound();
            }
            return await _context.Transcation
                .Where(c => c.CityPinyin == latestOne.CityPinyin && c.TimePoint == latestOne.TimePoint)
                .ToListAsync();
        }

    }
}
