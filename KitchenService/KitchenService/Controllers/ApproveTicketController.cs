﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApproveTicketController : ControllerBase
    {
        // POST api/values
        [HttpPost]
        public string Post([FromBody] object entrada)
        {
            AMQ.AMQ mensagem = new AMQ.AMQ();
            string stringData = JsonConvert.SerializeObject(entrada);
            dynamic results = JsonConvert.DeserializeObject<dynamic>(stringData);

            results.passo = "ApproveTicket";
            stringData = JsonConvert.SerializeObject(results);
            string fila = results.fila;

            mensagem.executa(fila, "TXN_5", stringData,"", "");

            return stringData;
        }
    }
}