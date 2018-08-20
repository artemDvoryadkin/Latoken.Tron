# Итоги по заданию
## Как разворачивать
В корне солюшена лежит файл docker-compose.yml нужно запустить докер компосе поднимится два контейнара. 
fullnode - узел трона. Узел полность заливался за два часа.
v049_latoken.currencyprovider.protocol.tron.jsonrpcapp - серверная часть.
Все компоненты настронены.
049_latoken.currencyprovider.protocol.tron.jsonrpcapp
тестировать можно по адресу http://10.0.75.1:32777/api/trx

## Примеры запросов для ntcnbhjdfybz
```
Only HTTP POST method is supported.

----------
Served from CXuesong.JsonRpc.AspNetCore
```
Это супеер гуд. Ставим в браузер хром приложение куыедуе Restlet Client REST API Testing chrome-extension://aejoelaoggembcahagimdiliamlcdmfm/restlet_client.html
```
METHOD POST
http://localhost:18890/api/trx 
HEADERS Content-Type application/json
BODY {"jsonrpc": "2.0", "method": "txListHashByAddress", "params":["TRQGijAaik4DfXSQM7cBFxn376YPycTm2W"], "id" :1}
```
*Резултат*
```
{
"id": 1,
"result":[
"f8300406ea77607d7d5f4e640d2fbc25e66a2d1b38abacba0187a9500275b49b",
"a8ed1f77548d6e958e8b6317bb248cb4535cc68598ee94d2c60b11e5687302bc",
......
"30613698dff35d57ad35b71ac535c8a52290389e0cd70661283d844d0ccb4121"
],
"jsonrpc": "2.0"
}
```

## запросы для Json RPC
```
JSONPRC

ValidateAddress
{"jsonrpc": "2.0", "method": "balanceByAddress", "params":["THG48yHsR6inxrCJk2hhxZPsFLq1ehP88V"], "id" :1}

validateAddress
{"jsonrpc": "2.0", "method": "validateAddress", "params":["THG48yHsR6inxrCJk2hhxZPsFLq1ehP88V"], "id" :1} // правильный адрес
{"jsonrpc": "2.0", "method": "validateAddress", "params":["THG48yHsR6inxrCJk2hhxZPsFaq1ehP88V"], "id" :1} // не верный адресс

txOfflineExternalIdAvailable
{"jsonrpc": "2.0", "method": "txOfflineExternalIdAvailable", "id" :1} // правильный запрос

toNativeNumber
{"jsonrpc": "2.0", "method": "toNativeNumber", "params":["234,3234"], "id" :1}

balanceMetaByAddress
{"jsonrpc": "2.0", "method": "balanceMetaByAddress", "params":["THG48yHsR6inxrCJk2hhxZPsFLq1ehP88V"], "id" :1}

balanceByAddressDB
{"jsonrpc": "2.0", "method": "balanceByAddressDB", "params":["THG48yHsR6inxrCJk2hhxZPsFLq1ehP88V"], "id" :1}

validateTransactionOptions
{"jsonrpc": "2.0", "method": "validateTransactionOptions", "params":[{
	"agent" :
	{
		"address" : "TLWY31TNNkqENXNcSctb2mqH1qvRdwbjFL",
		"privateKey": "750EB0343C49C9958387737108FB41EB53B6FD1FC074700F30082A7F9D4CC823"
	},
	"credentials" :
	{
		"address" : "TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSu"
	},
	"value": 108
}], "id" :1}




{
	"agent" :
	{
		"address" : "TLWY31TNNkqENXNcSctb2mqH1qvRdwbjFL",
		"privateKey": "750EB0343C49C9958387737108FB41EB53B6FD1FC074700F30082A7F9D4CC823"
	},
	"credentials" :
	{
		"address" : "TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSu"
	},
	"value": 108
}

txListByAddress
{"jsonrpc": "2.0", "method": "txListByAddress", "params":["TRQGijAaik4DfXSQM7cBFxn376YPycTm2W"], "id" :1}


txListHashByAddress
{"jsonrpc": "2.0", "method": "txListHashByAddress", "params":["TRQGijAaik4DfXSQM7cBFxn376YPycTm2W"], "id" :1}

balanceByAddressDB
{"jsonrpc": "2.0", "method": "txGetSendingResult", "params":["41c712036e6be669c4586005c573af516cdd9223e308b4b6f3d1314c3f0f88e2"], "id" :1}
```


# Старое описание

## Условия
Тестовая задача звучала так
Задание минимум:
- поднять ноду TRX https://coinmarketcap.com/currencies/tron/
- понять как можно получать Tx по адресу
- реализовать функционал под требуемый интерйфейс (ниже)
- провести тестовые транзакции

Задание максимум
- завернуть в docker
- тесты
## Что сделано
### Поднять ноду TRX https://coinmarketcap.com/currencies/tron/
Нода была поднята на Ubuntu как mainnet так и testnet использовалась для отлова ошибок формирования grpc запросов через логи узлов. Поднять узел Solidity не получилось, при подъеме в джаве при поднятии конфигурации возникла ошибка, самостоятельные поиски и поиск в интернете ошибки не привел к решению. В итоге  Soliditi не был поднят.

Что нужно что бы поставить нод.
* Поставить на виртуалку образ Ubuntu
* Установить java 1.8
```
sudo add-apt-repository ppa:webupd8team/java 
sudo apt-get update 
sudo apt-get install oracle-java8-installer 
java -version
```
Дальше ставим нод 
```
cd ~
wget https://raw.githubusercontent.com/tronprotocol/TronDeployment/master/deploy_tron.sh -O deploy_tron.sh 
Bash deploy_tron.sh 
```
более подробно тут https://github.com/tronprotocol/TronDeployment


### Понять как можно получать Tx по адресу
Получать транзакции по адресу можно тремя способами
1. Предложенный Святом и как оказалось самый простой. Это постоянно зачитывать новые блоки и записывать соотношение транзакций и счетов в базу.
2. Воспользоваться HTTP API методом "/transfer?address={address} выдает транзакции по адресу, но ограничение в 100.
3. Реализовать подключение по grpc к апи SolidityNode и вызвать метод GetTransactionsFromThis2 для этого необходимо клиенту подключаться к узлу типа SolidityNode.

В ходе работы был сделан 2 способ, 3 способ не получилось протостировать (код есть) так как не получилось поднять узел SolidityNode и за ошибок в java на стороне сервера.

### Что было сделано по методам
+ validateAddress(string address)
- validateReceipt(object any) //был вопросы что делать.
+ balanceByAddress(string address)
+ long? balanceByAddressDB(string address)
+ long? balanceMetaByAddress(string address)
+ txOfflineExternalIdAvailable()
+ string txGetOfflineExternalID(string txHex)
- TransactionData txCreate(TransactionOptions options)
- TransactionSendResult txTransferRaw(string txHex, string txHash)
- TransactionSendResult txTransfer(TransactionOptions options)
+ ITransactionSendResult txGetSendingResult(string txid)
- txGetReceipt(string txid)
+ txListByAddress(string address)
+ txListHashByAddress(string address)
+ toNativeNumber(string @base)
+ fromNativeNumber(string @base)
+ getProviderAPIHost()

### Провести тестовые транзакции
Провести транзакцию так и не получилось, сервер выдавал ошибку не корректной подписи. Решение было передавать приватный ключ через HTTP API, но по условиям задачи это не корректное решение. 
Много времени ушло на поднятие рабочей инфраструктуры например VS2017, Ubuntu, разобраться как работает и запустить gRPC, JSON RPC  и так далее. Так же неприлично много ушло времени на формирование адреса из при ватного ключа (не учел что sha нужно делать по Keccak и что нужно предавать адрес в размере 21 байта, а не 25, они заняли очень много времени.)

## Задание максимум
### - завернуть в docker
В солушене сделан докер.Есть особенность что виртуалка и docker не могут работать вместе из службы Hyper-V, так как одному нужно что бы она была запущена другому выключена.

### Тесты
Тесты сделаны. важная особенность.Тесты в папке garbage не доделаны до конца, так как связаны с проведением транзакции. На них смотреть не нужно. То что по пути GiHub/LaT/Latoken.CurrencyProvider.UnitTests/Protocol/Tron долно быть зеленым.

# Общая информация

По структуре проекта. Пытался держать структуру, перед отправкой проводил чистки, но все что касается Crypto не вычищено, так как модуль подписания транзакции не завершен,по этому строго не судить.

## как поднимать JSON RPC
открываем солушен vs 2017. ставим стартовым проект 050 Latoken.CurrencyProvider.Protocol.Tron.JsonRpcApplication . Билдим солушен. ЗАпускаем проект как консольное приложение для этого нужно выбрать вместо IIS 050 latoken.CurrencyProvider...... поднимиться барузер по адресу http://localhost:18890 это гуд. Нас интересует страница http://localhost:18890/api/trx в ответ получим

