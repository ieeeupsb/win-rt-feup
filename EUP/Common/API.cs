using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class API
    {
        public enum Actions
        {
            GetFEUPCanteen
        }

        private const string BASE_URI = "http://nuieee.fe.up.pt/motion/";
        private const string REQUEST_FORMAT = ".php";

        private static Uri getUrlFromAction(Actions action)
        {
            switch (action)
            {
                case Actions.GetFEUPCanteen:
                    return new Uri("https://sigarra.up.pt/feup/pt/mob_eme_geral.cantinas");
            }
            return null;
        }

        public static async Task<T> GetAsync<T>(Actions action)
        {
            Uri uri = getUrlFromAction(action);

            HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp(uri);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            var postStream = await request.GetRequestStreamAsync();
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

            using (Stream streamResponse = response.GetResponseStream())
            using (StreamReader streamRead = new StreamReader(streamResponse, Encoding.GetEncoding("ISO-8859-15")))
            {
                var responseString = streamRead.ReadToEnd();

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
                return (T) jsonSerializer.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(responseString)));
            }
        }
    }
}
