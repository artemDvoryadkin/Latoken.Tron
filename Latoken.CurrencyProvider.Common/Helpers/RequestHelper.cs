using System;
using System.IO;
using System.Net;

namespace TronToken.BussinesLogic
{
	public class RequestHelper
	{
		private readonly string _host = "api.tronscan.org";
		private readonly string _hostUrl;

		public RequestHelper(string host)
		{
			_host = host;
			_hostUrl = "https://" + _host;
		}

		public string GetUrl(string path)
		{
			if (path.Length > 0 && path[0] != '/') path = "/" + path;

			return _hostUrl + path;
		}

		protected HttpWebRequest GetRequestGet(string url)
		{
			var webRequest = SetupWebRequest(url);
			webRequest.Method = WebRequestMethods.Http.Get;

			return webRequest;
		}

		public HttpWebRequest SetupWebRequest(string url)
		{
			if (!url.Contains("https://")) url = GetUrl(url);
			var webRequest = (HttpWebRequest) WebRequest.Create(url);

			return SetupWebRequest(webRequest);
		}

		private HttpWebRequest SetupWebRequest(HttpWebRequest webRequest)
		{
			//	string authorization = "Bearer " + _token;
			//	webRequest.Headers.Add("authorization", authorization);

			webRequest.Host = _host;
			webRequest.Accept = "*/*";
			webRequest.ContentType = "application/json;charset=utf-8";


			webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/31.0";
			webRequest.Timeout = 20000;

			return webRequest;
		}

		public string GetStringFromHttpWebRequest(HttpWebRequest httpWebRequest)
		{
			//	Console.WriteLine("Start:"+ httpWebRequest.RequestUri.AbsoluteUri);
			WebResponse webResponse = null;
			var readToEnd = "";
			try
			{
				webResponse = httpWebRequest.GetResponse();

				using (var responseStream = webResponse.GetResponseStream())
				{
					using (var streamReader = new StreamReader(responseStream))
					{
						readToEnd = streamReader.ReadToEnd();
					}
				}
			}
			catch (WebException e)
			{
				Console.WriteLine(e);
				using (var data = e.Response.GetResponseStream())

				using (var reader = new StreamReader(data ?? throw new InvalidOperationException()))
				{
					var text = reader.ReadToEnd();
					Console.WriteLine(text);
				}

				throw;
			}
			
			finally
			{
					webResponse?.Close();
					webResponse?.Dispose();
			}
			//		Console.WriteLine("Response:" + httpWebRequest.RequestUri.AbsoluteUri);
			//		Console.WriteLine("Read:" + httpWebRequest.RequestUri.AbsoluteUri);

			return readToEnd;
		}
	}
}