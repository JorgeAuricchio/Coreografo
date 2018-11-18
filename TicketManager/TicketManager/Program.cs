using Amqp;
using Newtonsoft.Json;
using System;
using System.Threading;

namespace TicketManager
{
    class Program
    {
        static void Main(string[] args)
        {
            inicio ini = new inicio();
            termino fim = new termino();
            erro err = new erro();

            //ROBO QUANDO PRECISAMOS DE ORQUESTRACAO
            Thread newThreadINI = new Thread(ini.monitorar);

            newThreadINI.Start();
            Thread newThreadFIM = new Thread(fim.monitorar);

            newThreadFIM.Start();

            Settings configs = new Settings();
            ConfiguracoesValores erros = configs.ResgataParametro();

            foreach (ErroParametro erro in erros.Parametros.Erro)
            {
                Thread newThreadERRO = new Thread(err.Monitorar);

                ErroParametros parametros = new ErroParametros();

                parametros.Topico = erro.Topico;
                parametros.EnderecoFila = erros.Parametros.EnderecoFila;
                parametros.Descricao = erro.Descricao;
                parametros.EndpointElasticSearchOK = erros.Parametros.EndpointElasticSearch.DoctoOK;
                parametros.PassoRetorno = erro.PassoRetorno;
                newThreadERRO.Start(parametros); //FALTA MAPEAR
            }
        }
    }
}
