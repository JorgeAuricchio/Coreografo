using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PSHub
{
    public class Settings
    {
        public ConfiguracoesValores ResgataParametro()
        {
            IConfiguration config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", true, true)
                 .Build();

            //var settingsSection = config.Get<ConfiguracoesValores>();
            ConfiguracoesValores settings = config.Get<ConfiguracoesValores>();

            return settings;
        }
    }
    public class ConfiguracoesValores
    {
        public Parametros Parametros { get; set; }
    }
    public class Parametros
    {
        public EndpointElasticSearch EndpointElasticSearch { get; set; }
        public Escutas[] Escuta { get; set; }
    }
    public class EndpointElasticSearch
    {
        public string DoctoOK { get; set; }
        public string DoctoERRO { get; set; }
    }

    public class Escutas
    {
        public string EnderecoFila { get; set; }
        public string Topico { get; set; }
        public string API { get; set; }
    }
}
