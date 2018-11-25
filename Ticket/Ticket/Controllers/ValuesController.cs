using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ticket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        const string enderecoFila = "amqp://guest:guest@127.0.0.1:32772";
        const string topico = "TXN";

        // POST api/values
        [HttpPost]
        public string Post([FromBody] object entrada)
        {
            Random random = new System.Random();
            int valueSleep = random.Next(1, 10) * 1000;

            Thread.Sleep(valueSleep);
            AMQ.AMQ mensagem = new AMQ.AMQ();
            string stringData = JsonConvert.SerializeObject(entrada);
            dynamic results = JsonConvert.DeserializeObject<dynamic>(stringData);
            Guid guid = Guid.NewGuid();
            string codigo = guid.ToString();

            results.codigoTicket = codigo;
            results.fila = enderecoFila;
            results.dataExecucao = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            stringData = JsonConvert.SerializeObject(results);
            string fila = results.fila;
            mensagem.executa(fila, topico, stringData, codigo);
            return stringData;
        }
       
    }
}
