using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RastreiMe.Core.Helpers;

namespace RastreiMe.Core.Services
{
	public class BaseServiceException : Exception
	{
		public BaseServiceException(string msg) : base(msg) { }
	}

  public class BaseService
  {
    public BaseService()
    {
      _client = new HttpClient();
      _client.Timeout = TimeSpan.FromSeconds(40);
    }

    private readonly HttpClient _client;
		public static string UrlDesenvolvimento = "http://10.211.55.6:49567/api/v1/integracao";
		public static string UrlProducao = "http://10.211.55.6:49567/api/v1/integracao";
		public static string IntegrationKey = "RastreiMeß";

    public const int ERRO_SESSAO = 1001;
    public const int CHECKIN_REALIZADO = 1100;
    public const int NAO_EXISTE_CHECKIN_REALIZADO = 1110;

    private string GetHeaderUrl(string serviceName)
    {
      string url1 = string.Empty;
      if (Util.Ambiente == Ambiente.Desenvolvimento)
        url1 = UrlDesenvolvimento;
      else if (Util.Ambiente == Ambiente.Producao)
        url1 = UrlProducao;

      //JObject json = new JObject(new JProperty("data", DateTime.UtcNow), new JProperty("ikey", BaseService.IntegrationKey));
      //string sjson = JsonConvert.SerializeObject(jso n);
      //string ikey = Convert.ToBase64String(Encoding.UTF8.GetBytes(sjson));
      //var url = string.Format("{0}/{1}/{2}", url1, serviceName, ikey);
      var url = string.Format("{0}/{1}", url1, serviceName);
      return (url);
    }

    protected JObject PostJson(string serviceName)
    {
      JObject ret = PostJson(serviceName, null);
      return (ret);
    }

    protected JObject PostJson(string serviceName, JObject jsonparameters)
    {
      JObject ret = null;
      var url = GetHeaderUrl(serviceName);
      var uri = new Uri(url);

      try
      {
        if (jsonparameters == null)
          jsonparameters = new JObject();

        jsonparameters.Add("dataUtc", DateTime.UtcNow);
        jsonparameters.Add("ikey", BaseService.IntegrationKey);

        string parameters = JsonConvert.SerializeObject(jsonparameters);
        parameters = Convert.ToBase64String(Encoding.UTF8.GetBytes(parameters));

        //INetwork d = Resolver.Resolve<INetwork>();
        //var status = d.InternetConnectionStatus();
        //if (status != NetworkStatus.NotReachable)

        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("User-Agent", "RastreiMe");
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        Dictionary<string, string> pairs = new Dictionary<string, string>();
        pairs.Add("Data", parameters);

        StringContent content = new StringContent(parameters, Encoding.UTF8, "application/json");
        HttpResponseMessage response = _client.PostAsync(uri, content).Result;

        if (!response.IsSuccessStatusCode)
        {
          throw new BaseServiceException("Problema no servidor!");
        }

        string sjson = response.Content.ReadAsStringAsync().Result;

        var bytes = Convert.FromBase64String(sjson);
        sjson = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        ret = JObject.Parse(sjson);

        Debug.WriteLine("BaseService.PostJson");
        Debug.WriteLine(response);

      }
      catch (Exception ex)
      {
        Debug.WriteLine("BaseService.GetJson.Exception");
        Debug.WriteLine(ex.Message);
        ret = new JObject(new JProperty("Success", false), new JProperty("Message", "Problema na comunicação!"));
      }

      return (ret);
    }
  }
}