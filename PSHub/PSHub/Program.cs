using Amqp;
using Amqp.Framing;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PSHub
{
    class Program
    {
        static Dictionary<string, ParametrosRotina> topicos = new Dictionary<string, ParametrosRotina>();

        static Dictionary<string, bool> execucoes = new Dictionary<string, bool>();

        static void Main(string[] args)
        {
            PSHub principal = new PSHub();
            Settings configs = new Settings();
            ConfiguracoesValores escutas = configs.ResgataParametro();

            foreach (Escutas escuta in escutas.Parametros.Escuta)
            {
                AdicionaParametroRotina(escuta.EnderecoFila, escuta.Topico, escuta.API);
            }
            foreach (ParametrosRotina topico in topicos.Values)
            {
                Console.WriteLine("iniciando escuta: {0}", topico.topico);
                principal.executar(topico);
            }
        }

        static void AdicionaParametroRotina(string enderecoAMQ, string topico, string enderecoRota)
        {
            if (topicos.ContainsKey(topico))
            {
                topicos[topico].rota.Add(new Rota(enderecoRota, ""));
            }
            else
            {
                topicos.Add(topico, new ParametrosRotina(enderecoAMQ, topico, enderecoRota, ""));
            }
        }
    }
}
