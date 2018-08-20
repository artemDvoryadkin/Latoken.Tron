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

===================
====
=====
=======

balanceByAddressDB
{"jsonrpc": "2.0", "method": "txGetSendingResult", "params":["41c712036e6be669c4586005c573af516cdd9223e308b4b6f3d1314c3f0f88e2"], "id" :1}
