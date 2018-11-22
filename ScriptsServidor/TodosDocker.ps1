$appActiveMQ = Start-Process -NoNewWindow powershell '-File ActiveMQ.ps1' -PassThru
$appElasticSearch = Start-Process -NoNewWindow powershell '-File ElasticSearch.ps1' -PassThru