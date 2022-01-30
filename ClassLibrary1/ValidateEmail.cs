using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Plugins
{
    

    public class ValidateEmail : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)
            serviceProvider.GetService(typeof(IPluginExecutionContext));
            Entity entity = (Entity)context.InputParameters["Target"];
            if (entity.Contains("emailaddress1"))
            {
                string email = (string)entity["emailaddress1"];
                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (isEmail)
                {
                    entity["cr1f8_valid"] = true;
                    PostRequest(email, true);
                }
                else
                {
                    entity["cr1f8_valid"] = false;
                    PostRequest(email, false);
                }
            }
            else
            {
                entity["cr1f8_valid"] = false;
                PostRequest("empty@empty.empty", false);
            }
        }

        public HttpStatusCode PostRequest(string email, bool valid)
        {
            var url = "https://crane-hire.azurewebsites.net/api/email_validation?code=Bwp9Qq81v7Q2u2hgFaRqzKuh394wlBoSRFU32Z5UO5v2yd/tVNvhZg==";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.Accept = "application/json";
            httpRequest.Headers["Authorization"] = "Bearer ";
            httpRequest.ContentType = "application/json";

            var data = @"{
                          ""email"": """ + email + @""",
                          ""valid"": " + valid.ToString().ToLower() + @",
                          ""token"": """"
                        }";
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.WriteLine(data);
            }
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            return httpResponse.StatusCode;
        }
    }
}
