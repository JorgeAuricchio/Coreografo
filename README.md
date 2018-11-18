# Coreografo
Exemplo de Coreografia de Serviços via Broker

### Desenvolvido em .NetCore 2.1

### Para rodar precisamos:
Ter um equipamento com Windows ou Mac<br/>
Visual Studio Community<br/>
Docker

### Diagrama

<a href="https://drive.google.com/open?id=1IDElG1HQziMcFWMFyWtwli-xDbb37c5H" target="_blank">formato PNG</a><br/>
<a href="https://drive.google.com/open?id=1Q8mO7v46o1by8Q-ceGIPIA1nSZiMtkQM" target="_blank">formato do Draw.IO</a><br/>
<a href="https://www.draw.io" target="_blank">Site do Draw.IO</a>
 

## Componentes<br/>
### ActiveMQ<br/>
Responsavel pela fila utilizada no Hub<br/>
para subir:<br/>
```
docker pull webcenter/activemq
docker run --name='activemq' -it --rm -e 'ACTIVEMQ_CONFIG_MINMEMORY=512' -e 'ACTIVEMQ_CONFIG_MAXMEMORY=2048' -P webcenter/activemq:latest
Url: http://localhost:32771/admin/
Usuário: admin
Senha: admin
```
### ElasticSearch<br/>
Responsavel por armazenar logs das execucoes dos servicos/hub/ticketmanager<br/>
para subir:<br/>
```
docker pull docker.elastic.co/elasticsearch/elasticsearch:6.4.3
docker run -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" -e "http.cors.enabled=true" -e "http.cors.allow-origin=*" docker.elastic.co/elasticsearch/elasticsearch:6.4.3
```

### Robô PSHub<br/>
Responsavel por rotear os topicos assinados e acionar os respectivos servicos (APIs)<br/>
para subir:<br/>
```
cd "PSHub\PSHub\bin\Debug\netcoreapp2.1"
dotnet .\PSHub.dll
```

### Robô TicketManager<br/>
Responsavel por orquestrar caso necessario algum servico<br/>
para subir:<br/>
```
cd "TicketManager\TicketManager\bin\Debug\netcoreapp2.1"
dotnet .\TicketManager.dll
```

### API de Ticket<br/>
API responsavel por criar ticket<br/>
Porta 9000<br/>
para subir:<br/>
```
cd "Ticket\Ticket\bin\Debug\netcoreapp2.1"
dotnet .\Ticket.dll
```

### API de OrderService<br/>
API/Servico para demonstracao de coreografia, ainda nao tem regras de negocio<br/>
Porta 9001<br/>
para subir:<br/>
```
cd "OrderService\OrderService\bin\Debug\netcoreapp2.1"
dotnet .\OrderService.dll
```

### API de ConsumerService<br/>
API/Servico para demonstracao de coreografia, ainda nao tem regras de negocio<br/>
Porta 9002<br/>
para subir:<br/>
```
cd "ConsumerService\ConsumerService\bin\Debug\netcoreapp2.1"
dotnet .\ConsumerService.dll
```

### API de KitchenService<br/>
API/Servico para demonstracao de coreografia, ainda nao tem regras de negocio<br/>
Porta 9003<br/>
para subir:<br/>
```
cd "KitchenService\KitchenService\bin\Debug\netcoreapp2.1"
dotnet .\KitchenService.dll
```

### API AccountingService<br/>
API/Servico para demonstracao de coreografia, ainda nao tem regras de negocio<br/>
Foi adicionada uma regra para autorizar ou negar uma transação<br/>
Porta 9004<br/>
para subir:<br/>
```
cd "AccountingService\AccountingService\bin\Debug\netcoreapp2.1"
dotnet .\AccountingService.dll
```

## Robô Canal
Responsável por simular a criação de tickets<br/>
para executar:<br/>
```
cd "Canal\Canal\bin\Debug\netcoreapp2.1"
dotnet .\Canal.dll
```

## Monitor (JQuery)
Responsável exibir em tempo real a evolução dos tickets criados<br/>
para executar:<br/>
```
cd "Web"
executar no browser .\monitor.html
```

### Para ver os logs dos serviços basta acessar o ElasticSearch<br/>
   http://localhost:9200/coreografado/doc/_search<br/>
```
Post:
{
  "query": {
    "bool": {
      "must": [
        { "match": { "codigoTicket": "UID DO TICKET" }}
      ]
    }
  }
}
```

Para visualizar use: https://www.getpostman.com/apps <br/>

Em todas as APIS foi colocado um Thread.Sleep para simular tempos diferentes de retorno.

### Próximos passos<br/>
   criar classes dos serviços e regras de negócio (incluir casos de erro)<br/>
   criar monitor grafico para ver os servicos atuando [em construção]<br/>
   criar conteineres de todos os servicos e consoles   <br/>
   criar testes unitarios
