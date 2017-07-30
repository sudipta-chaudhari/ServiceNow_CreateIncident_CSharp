using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace SNOW.Logger
{
    public class SNOWLogger
    {
        public static string CreateIncidentServiceNow(string shortDescription, string description)
        {
            try
            {
                string username = ConfigurationManager.AppSettings["ServiceNowUserName"];
                string password = ConfigurationManager.AppSettings["ServiceNowPassword"];
                string url = ConfigurationManager.AppSettings["ServiceNowUrl"];

                var auth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password));

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Headers.Add("Authorization", auth);
                request.Method = "Post";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(new
                    {
                        description = shortDescription + Environment.NewLine + Environment.NewLine + description,
                        short_description = ConfigurationManager.AppSettings["ServiceNowTicketShortDescription"],
                        contact_type = ConfigurationManager.AppSettings["ServiceNowContactType"],
                        category = ConfigurationManager.AppSettings["ServiceNowCategory"],
                        subcategory = ConfigurationManager.AppSettings["ServiceNowSubCategory"],
                        assignment_group = ConfigurationManager.AppSettings["ServiceNowAssignmentGroup"],
                        impact = ConfigurationManager.AppSettings["ServiceNowIncidentImpact"],
                        priority = ConfigurationManager.AppSettings["ServiceNowIncidentPriority"],
                        caller_id = ConfigurationManager.AppSettings["ServiceNowCallerId"],
                        cmdb_ci = ConfigurationManager.AppSettings["ServiceNowCatalogueName"],
                        comments = ConfigurationManager.AppSettings["ServiceNowTicketShortDescription"]
                    });

                    streamWriter.Write(json);
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    var res = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    JObject joResponse = JObject.Parse(res.ToString());
                    JObject ojObject = (JObject)joResponse["result"];
                    string incNumber = ((JValue)ojObject.SelectToken("number")).Value.ToString();

                    return incNumber;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
