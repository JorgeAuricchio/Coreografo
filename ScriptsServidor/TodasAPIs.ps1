$appAccountingService = Start-Process -NoNewWindow powershell '-File AccountingService.ps1' -PassThru
$appConsumerService = Start-Process -NoNewWindow powershell '-File ConsumerService.ps1' -PassThru
$appKitchenService = Start-Process -NoNewWindow powershell '-File KitchenService.ps1' -PassThru
$appOrderService = Start-Process -NoNewWindow powershell '-File OrderService.ps1' -PassThru
$appTicket = Start-Process -NoNewWindow powershell '-File Ticket.ps1' -PassThru