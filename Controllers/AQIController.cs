using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ChinaAQIDataCore.Models;
using System.Threading.Tasks;
using System.Linq;

namespace ChinaAQIDataCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AQIController : ControllerBase
    {
        private readonly AQIContext _context;

        public AQIController(AQIContext context)
        {
            _context = context;
        }


        [HttpGet("{cityNamePinyin}")]
        public async Task<ActionResult<IEnumerable<AQIDTO>>> GetCityAQI(string cityNamePinyin)
        {
            var latestOne =  _context.AQIItems
                .Where(c => c.CityPinyin.StartsWith(cityNamePinyin.ToUpper()))
                .OrderByDescending(c => c.TimePoint)
                .FirstOrDefault();
            if (latestOne == null) {

                return NotFound();
            }
            return await _context.AQIItems
                .Where(c => c.CityPinyin == latestOne.CityPinyin && c.TimePoint == latestOne.TimePoint)
                .ToListAsync();
        }

    }
}
