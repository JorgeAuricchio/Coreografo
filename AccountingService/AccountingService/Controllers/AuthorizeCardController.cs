using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeCardController : ControllerBase
    {
        // POST api/values
        [HttpPost]
        public string Post([FromBody] object entrada)
        {
            Random random = new System.Random();
            int valueSleep = random.Next(1, 10); //returns integer of 0-100

            Thread.Sleep(valueSleep * 1000);
            //            Console.WriteLine("Sleep: {0}", valueSleep);
            AMQ.AMQ mensagem = new AMQ.AMQ();
            string stringData = JsonConvert.SerializeObject(entrada);
            dynamic results = JsonConvert.DeserializeObject<dynamic>(stringData);

            string passo = "AuthorizeCard";
            string topico = "TXN_4";

            if (valueSleep % 2 == 0)
            {
                passo = "RejectCard";
                topico = "TXN_40";
            }
            results.passo = passo;
            results.dataExecucao = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            stringData = JsonConvert.SerializeObject(results);
            string fila = results.fila;
            string codigoTicket = results.codigoTicket;

            mensagem.executa(fila, topico, stringData, codigoTicket);

            return stringData;
        }
    }
}
