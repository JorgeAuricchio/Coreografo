using Amqp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TicketManager
{
    public class termino
    {
        const string NOME_FILA_CONSUMIDOR_MQ = "consumidor";
        const string MIME_TYPE_JSON = "application/json";

        public void monitorar()
        {
            Console.WriteLine("TERMINO DA OPERACAO");
            Connection connection = null;
            try
            {
                Settings configs = new Settings();
                ConfiguracoesValores config = configs.ResgataParametro();
                string EndpointElasticSearchOK = config.Parametros.EndpointElasticSearch.DoctoOK;
                string enderecoFila = config.Parametros.EnderecoFila;

                Address address = new Address(enderecoFila);
                connection = new Connection(address);
                Session session = new Session(connection);
                string topicoEntrada = config.Parametros.Termino.TopicoEntrada;
                ReceiverLink receiver = new ReceiverLink(session, NOME_FILA_CONSUMIDOR_MQ, topicoEntrada);
                ElasticSearch.ElasticSearch ES = new ElasticSearch.ElasticSearch();

                while (true)
                {
                    Message request = receiver.Receive(new TimeSpan(0, 30, 0));
                    string replyTo = request.Properties.ReplyTo;
                    string correlationID = request.Properties.CorrelationId;

                    if (null != request)
                    {
                        Console.WriteLine(request.Body);
                        string stringData = request.Body.ToString();
                        dynamic results = JsonConvert.DeserializeObject<dynamic>(stringData);

                        results.passo = "TerminoProcesso";
                        results.dataExecucao = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        string codigoTicket = correlationID;

                        stringData = JsonConvert.SerializeObject(results);
                        using (var client = new HttpClient())
                        {
                            var contentDataES = new StringContent(stringData, System.Text.Encoding.UTF8, MIME_TYPE_JSON);

                            Console.WriteLine("Gravando no ElasticSearch: {0}", EndpointElasticSearchOK + codigoTicket);
                            ES.executa(EndpointElasticSearchOK, contentDataES);
                        }

                        Console.WriteLine("FIM de operacao");
                        Console.WriteLine(stringData);
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
}
