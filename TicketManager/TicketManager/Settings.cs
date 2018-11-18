using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TicketManager
{
    public class Settings
    {
        public ConfiguracoesValores ResgataParametro()
        {
            IConfiguration config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", true, true)
                 .Build();
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
        public string EnderecoFila { get; set; }
        public EndpointElasticSearch EndpointElasticSearch { get; set; }
        public Inicio Inicio { get; set; }
        public Termino Termino { get; set; }
        public ErroParametro[] Erro { get; set; }
    }
    public class EndpointElasticSearch
    {
        public string DoctoOK { get; set; }
        public string DoctoERRO { get; set; }
    }

    public class Inicio
    {
        public string TopicoEntrada { get; set; }
        public string TopicoSaida { get; set; }
    }
    public class Termino
    {
        public string TopicoEntrada { get; set; }
        public string TopicoSaida { get; set; }
    }
    public class ErroParametro
    {
        public string Topico { get; set; }
        public string Descricao { get; set; }
        public string PassoRetorno { get; set; }
    }
}