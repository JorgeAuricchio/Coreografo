using Amqp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TicketManager
{
    public class erro
    {
        const string NOME_FILA_CONSUMIDOR_MQ = "consumidor";
        const string MIME_TYPE_JSON = "application/json";

        public void Monitorar(object parametros)
        {
            ErroParametros param = ((ErroParametros)parametros);
            string topico = param.Topico;
            string endrecoFila = param.EnderecoFila;
            string descricao = param.Descricao;
            string EndpointElasticSearchOK = param.EndpointElasticSearchOK;
            string passoRetorno = param.PassoRetorno;

            Console.WriteLine("ERRO Monitorado: " + topico);
            Console.WriteLine("Descricao: " + descricao);            
            Connection connection = null;
            try
            {                
                Address address = new Address(endrecoFila);
                connection = new Connection(address);
                Session session = new Session(connection);
                ReceiverLink receiver = new ReceiverLink(session, NOME_FILA_CONSUMIDOR_MQ, topico);
                ElasticSearch.ElasticSearch ES = new ElasticSearch.ElasticSearch();

                while (true)
                {
                    Message request = receiver.Receive(new TimeSpan(0, 30, 0));
                    string replyTo = request.Properties.ReplyTo;
                    string correlationID = request.Properties.CorrelationId;

                    if (null != request)
                    {
                        //                        Console.WriteLine(request.Body);
                        string stringData = request.Body.ToString();
                        dynamic results = JsonConvert.DeserializeObject<dynamic>(stringData);

                        //Console.WriteLine("FIM de operacao: {0}", correlationID);
                        //Console.WriteLine(stringData);

                        results.passo = passoRetorno;
                        results.dataExecucao = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                        stringData = JsonConvert.SerializeObject(results);

                        var contentDataES = new StringContent(stringData, System.Text.Encoding.UTF8, MIME_TYPE_JSON);

                        //Console.WriteLine("Gravando no ElasticSearch: {0}", EndpointElasticSearchOK);
                        ES.executa(EndpointElasticSearchOK, contentDataES);

                        receiver.Accept(request);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Exception {0}.", e);
                if (null != connection)
                {
                    connection.Close();
                }
            }
        }
    }
    public class ErroParametros
    {
        public string Topico;
        public string EnderecoFila;
        public string Descricao;
        public string EndpointElasticSearchOK;
        public string PassoRetorno;
    }
}