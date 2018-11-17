using Amqp;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;

namespace PSHub
{
    public class PSHub
    {
        public PSHub()
        {

        }
        public void executar(object parametro)
        {
            Thread newThread = new Thread(NovaRotina);
            newThread.Start(parametro);
        }

        const int TENTATIVAS_RETRY = 3;
        const string SIM = "Sim";
        const string SUFIXO_MQ_ERRO = "_ERRO";
        const string NOME_FILA_CONSUMIDOR_MQ = "consumidor";
        const string MSG_TIMEOUT = "Timeout waiting for request. Keep waiting...";
        const string MIME_TYPE_JSON = "application/json";

        private void retryMSG(string enderecoAMQ, string topico, string stringData, string correlationID, string replyTo, string erro)
        {
            AMQ.AMQ gravaMensagem = new AMQ.AMQ();
            dynamic results = JsonConvert.DeserializeObject<dynamic>(stringData);

            if (results.Retries == null)
            {
                results.Retries = 0;
            }
            results.Retries = results.Retries + 1;
            results.erroChamada = erro;
            if (results.Retries <= TENTATIVAS_RETRY)
            {
                stringData = JsonConvert.SerializeObject(results);
                gravaMensagem.executa(enderecoAMQ, topico, stringData, correlationID);
            }
            else
            {
                results.numeroTentativasExcedidas = SIM;
                stringData = JsonConvert.SerializeObject(results);
                gravaMensagem.executa(enderecoAMQ, topico+ SUFIXO_MQ_ERRO, stringData, correlationID);
            }
        }

        private void NovaRotina(object parametro)
        {
            ParametrosRotina topico = (ParametrosRotina)parametro;
            Connection connection = null;

            try
            {
                Address address = new Address(topico.enderecoAMQ);
                connection = new Connection(address);
                Session session = new Session(connection);
                Settings configs = new Settings();
                ConfiguracoesValores config = configs.ResgataParametro();
                string EndpointElasticSearchOK = config.Parametros.EndpointElasticSearch.DoctoOK;
                string EndpointElasticSearchERRO = config.Parametros.EndpointElasticSearch.DoctoERRO;

                ReceiverLink receiver = new ReceiverLink(session, NOME_FILA_CONSUMIDOR_MQ, topico.topico);                
                while (true)
                {
                    Message request = receiver.Receive();
                    if (null != request)
                    {
                        Console.WriteLine("Topico acionado: {0}", topico.topico);
                        Console.WriteLine(request.Body);
                        string stringData = request.Body.ToString();
                        string correlationID = request.Properties.CorrelationId;
                        string replyTo = request.Properties.ReplyTo;

                        Console.WriteLine("CRID: {0}", correlationID);
                        Console.WriteLine("prp: {0}", request.Properties);

                        using (var client = new HttpClient())
                        {
                            foreach (Rota rota in topico.rota)
                            {
                                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, MIME_TYPE_JSON);
                                try
                                {
                                    HttpResponseMessage response1 = client.PostAsync(rota.endereco, contentData).Result;

                                    if (response1.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        string retorno = response1.Content.ReadAsStringAsync().Result;
                                        //posta elasticsearch
                                        var contentDataES = new StringContent(retorno, System.Text.Encoding.UTF8, MIME_TYPE_JSON);

                                        Console.WriteLine("Gravando no ElasticSearch: {0}", EndpointElasticSearchOK + correlationID);
                                        HttpResponseMessage responseES = client.PostAsync(EndpointElasticSearchOK, contentDataES).Result;
                                    }
                                    else
                                    {
                                        retryMSG(topico.enderecoAMQ, topico.topico, stringData, correlationID, replyTo, response1.StatusCode.ToString());

                                        //posta elasticsearch
                                        var contentDataES = new StringContent(stringData, System.Text.Encoding.UTF8, MIME_TYPE_JSON);

                                        Console.WriteLine("Gravando no ElasticSearch: {0}", EndpointElasticSearchERRO + correlationID);
                                        HttpResponseMessage responseES = client.PostAsync(EndpointElasticSearchERRO, contentDataES).Result;
                                    }
                                }
                                catch (Exception erro)
                                {
                                    retryMSG(topico.enderecoAMQ, topico.topico, stringData, correlationID, replyTo, erro.Message.ToString());
                                    //posta elasticsearch
                                    var contentDataES = new StringContent(stringData, System.Text.Encoding.UTF8, MIME_TYPE_JSON);

                                    Console.WriteLine("Gravando no ElasticSearch: {0}", EndpointElasticSearchERRO + correlationID);
                                    HttpResponseMessage responseES = client.PostAsync(EndpointElasticSearchERRO, contentDataES).Result;
                                }
                            }
                            receiver.Accept(request);
                        }
                    }
                    else
                    {
                        Console.WriteLine(MSG_TIMEOUT);
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
