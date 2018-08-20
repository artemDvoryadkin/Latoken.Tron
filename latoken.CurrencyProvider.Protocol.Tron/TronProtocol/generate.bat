protoc  --csharp_out=..\Grpc\Core\ core\Tron.proto --plugin=grpc_csharp_plugin.exe
protoc  --csharp_out=..\Grpc\Core\ core\Contract.proto --plugin=grpc_csharp_plugin.exe
protoc  --csharp_out=..\Grpc\Core\ core\Discover.proto --plugin=grpc_csharp_plugin.exe
protoc  --csharp_out=..\Grpc\Core\ core\TronInventoryItems.proto --plugin=grpc_csharp_plugin.exe
protoc  --csharp_out=..\Grpc\Api\ api\api.proto --plugin=grpc_csharp_plugin.exe


protoc api\api.proto --plugin=protoc-gen-grpc=grpc_csharp_plugin.exe --grpc_out=..\Grpc\Api 