using System;
using System.Collections.Generic;
using Grpc.Core;

namespace Latoken.CurrencyProvider.Common.Configuration
{
	public class ProtocolConfiguration : IApiHostConfig
	{
		public Channel Channel { get; }
		public ProtocolConfiguration()
		{
			//		Host = System.Configuration.ConfigurationManager.AppSettings[HostKey];
			//		Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings[PortKey]);
	//		if (typeNet == TypeNet.Main) configuration = new AppSettingsChannelConfiguration("13.125.249.129", 50051);

			//if (typeNet == TypeNet.Test) configuration = new AppSettingsChannelConfiguration("39.105.72.181", 18888);
	//		if (typeNet == TypeNet.Test) configuration = new AppSettingsChannelConfiguration("192.168.19.135", 18888);


			//	Host = "13.125.249.129";
	//		Host = "192.168.19.135"; Port = int.Parse("50051");// локальный узел 
			//	Host = "39.105.72.181"; Port = int.Parse("18888"); // тестовый узел

			//	

			//	Port = int.Parse("50052");
			MaxConcurrentStreams = 22;// System.Configuration.ConfigurationManager.AppSettings[MaxConcurrentStreamsKey];
			TimeOutMs = TimeSpan.FromMilliseconds(5000);//System.Configuration.ConfigurationManager.AppSettings[TimeOutMsKey];
														/*		if (string.IsNullOrWhiteSpace(maxConcurrentStreams) == false)
																{
																	MaxConcurrentStreams = int.Parse(maxConcurrentStreams);
																}

																if (string.IsNullOrWhiteSpace(timeOutMs) == false)
																{
																	TimeOutMs = TimeSpan.FromMilliseconds(int.Parse(timeOutMs));
																}
																*/
			
		}

		public ProtocolConfiguration(TypeNet typeNet):this(typeNet, typeNet== TypeNet.Main ? "13.125.249.129" : "39.105.72.181", typeNet == TypeNet.Main ? 50051 : 18888)
		{
			TypeNet = typeNet;
		}

		public ProtocolConfiguration(TypeNet typeNet, string host, int port) : this(typeNet, host, port, 20, 5000)
		{
			TypeNet = typeNet;
			Host = host;
			Port = port;
		}

		public ProtocolConfiguration(TypeNet typeNet, string host, int port, int? maxConcurrentStreams, int timeOutMilliSeconds)
		{
			TypeNet = typeNet;
			Host = host;
			Port = port;
			MaxConcurrentStreams = maxConcurrentStreams;
			TimeOutMs = TimeSpan.FromMilliseconds(timeOutMilliSeconds);

			Channel = new Channel(Host, Port, ChannelCredentials.Insecure,
				new List<ChannelOption>()
				{
					new ChannelOption(ChannelOptions.MaxConcurrentStreams, maxConcurrentStreams == null?22: maxConcurrentStreams.Value)
				});
		}

		public int Port { get; }

		public string Host { get; }

		public int? MaxConcurrentStreams { get; }

		public TimeSpan? TimeOutMs { get; }

		public string Url { get; }
		public TypeNet TypeNet { get; }
	}
}