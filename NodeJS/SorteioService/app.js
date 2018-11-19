var express = require('express');
var bodyParser = require('body-parser');
var app = express();
var AMQPClient = require('amqp10').Client;
var client = new AMQPClient(); // Uses PolicyBase default policy
var enderecoAMQ = 'amqp://guest:guest@127.0.0.1:32772';
var topico = 'SORTEIO_REGISTRADO';

app.use(bodyParser.json());

app.get('/Sorteio', function(req, res) {
  var dados = [
    {
      lat: -25.470991, 
      lon: -49.271036
    },
    {
      lat: -0.935586,
      lon: -49.635540
    },
    {
      lat: -2.485874, 
      lon: -43.128493
    }
  ];

  res.send(JSON.stringify(dados));
});

app.post('/Sorteio', function(req, res) {
  var dado = req.body;
  var dateTime = require('node-datetime');
  var dt = dateTime.create();
  var formatted = dt.format('Y/m/d H:M:S');

  dado.dataExecucao = formatted;
  dado.passo = "InscricaoSorteio";

  console.log(req.body);
  var dados = [
    {
      ret: 1
    }
  ];

client.connect(enderecoAMQ)
  .then(function() {
    return Promise.all([
      //client.createReceiver('amq.topic'),
      client.createSender(topico)
    ]);
  })
  .spread(function(sender) {
   /* receiver.on('errorReceived', function(err) { // check for errors });
    receiver.on('message', function(message) {
      console.log('Rx message: ', message.body);
    });

    return sender.send({ key: "Value" });
  })
  .error(function(err) {
    console.log("error: ", err);
  });*/  
  //sender.send({ key: JSON.stringify(dado), correlationId: "Aaa" });
  //sender.send( { header: { durable: true }, body: JSON.stringify(dado), correlation: "Aaa" } );
  sender.send( { body: JSON.stringify(dado) } );
});
  res.send(JSON.stringify(dado));
});

app.listen(8000, function() {
  console.log('Servidor rodando na porta 8000.');
});
