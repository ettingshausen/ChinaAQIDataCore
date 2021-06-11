using System;
using System.Text.Json;

namespace ChinaAQIDataCore.Utils
{
    public class LowerCaseNamingPolicy : JsonNamingPolicy
    {

        public override string ConvertName(string name)
        {
            return name.ToLower();
        }
    }
}
