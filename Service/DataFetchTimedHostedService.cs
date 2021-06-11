using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ChinaAQIDataCore.Models;
using ChinaAQIDataCore.Utils;

namespace ChinaAQIDataCore.Service
{
    internal class DataFetchTimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;
        static readonly HttpClient client = new HttpClient();

        public DataFetchTimedHostedService(ILogger<DataFetchTimedHostedService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation("Fetch Service is working.");
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
                    if (p.Name == "CityPinyin")
                    {
                        p.SetValue(aqi, PinyinUtil.GetPinyin((GetNodeValue(node, "Area"))), null);
                        continue;
                    }
                    p.SetValue(aqi, (GetNodeValue(node, p.Name)), null);
                }
                set.Add(aqi);
            }

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AQIContext>();
            context.Transcation.AddRange(set);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("insert failed");
            }


        }

        private string GetNodeValue(XmlElement node, string propertie)
        {

            return node.GetElementsByTagName(string.Format("b:{0}", propertie)).Item(0).InnerText;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}