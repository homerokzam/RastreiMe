using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using Xamarin.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RastreiMe.Core.Services
{
  public class RastreiMeServiceException : Exception
  {
    public RastreiMeServiceException(string msg) : base(msg) { }
  }

  public class RastreiMeService : BaseService
  {
    public RastreiMeService() : base() { }

    private JObject TestarConexao()
    {
      string ret = string.Empty;
      string url = "Testar";
      JObject jret = PostJson(url);
      return (jret);
    }

    public Task<JObject> TestarConexaoAsync()
    {
      return (Task.Run(() =>
      {
        return (TestarConexao());
      }));
    }

		public JObject GetAparelhos(string idUsuario)
		{
			string url = "GetAparelhos";
			JObject json = new JObject(
			  new JProperty("idUsuario", idUsuario)
			);

			JObject jret = PostJson(url, json);

			Boolean success = jret["Success"].Value<Boolean>();
			if (!success)
			{
				string message = jret["Message"].Value<string>();
				throw new RastreiMeServiceException(message);
			}

			return (jret);
		}

		public Task<JObject> GetAparelhosAsync(string idUsuario)
		{
			return (Task.Run(() =>
			{
				JObject ret = GetAparelhos(idUsuario);
				return (ret);
			}));
		}

		public JObject GetLocalizacoes(Int32 id)
    {
      string url = "GetLocalizacoes";
      JObject json = new JObject(
        new JProperty("id", id)
      );

      JObject jret = PostJson(url, json);

      Boolean success = jret["Success"].Value<Boolean>();
      if (!success)
      {
        string message = jret["Message"].Value<string>();
        throw new RastreiMeServiceException(message);
      }

      return (jret);
    }

    public Task<JObject> GetLocalizacoesAsync(Int32 id)
    {
      return (Task.Run(() =>
      {
        JObject ret = GetLocalizacoes(id);
        return (ret);
      }));
    }
	}
}