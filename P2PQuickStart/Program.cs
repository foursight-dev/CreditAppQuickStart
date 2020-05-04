using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ILendingQuickStart
{
    class Program
    {
        /// <summary>
        /// Enter your authentication token to begin. Ping service and/or send xml data. Uncomment PostApplication when ready to send xml.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string authenticationToken = ""; // Enter your authentication token here.
            //Sample new application XML key data
            string sampleXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" 
                                + "<dt_application active=\"yes\" status=\"new\" dtversion=\"2.0\">"
                                + "<key_data optout=\"no\">"
                                + "<dt_lender_id>FOU</dt_lender_id>"
                                + "<dt_dealer_id>123456</dt_dealer_id> "
                                + "<dt_app_id>ABC123</dt_app_id> "
                                + "<lender_dealer_id>123_1</lender_dealer_id>"
                                + "<lender_app_id/>"
                                + "<requestdate>2020-04-29T15:40:33</requestdate>"
                                + "<credit_type type =\"individual\"/>"
                                + "<app_type type =\"personal\"/>"
                                + "<product_type type =\"retail\" paymentcall =\"no\"/>"
                                + "<vehicle_type type =\"used\" trade =\"no\"/>"
                                + "<cust_credit_type type =\"none\"/>"
                                + "<loan_type type =\"auto\"/>"
                                + "<source/>"
                                + "<user_name> SYDNEY FAKENAME </user_name>"
                                + " </key_data>"
                                + "</dt_application>";



            string url = "https://creditapp-p2p.azurewebsites.net"; 
            string pingUrl = $"{url}/API/Admin/GetBuildDate";
            string postApplicationUrl = $"{url}/API/Inbound/PostApplication?authenticationToken={authenticationToken}";

            PingService(pingUrl);
            //PostApplication(postApplicationUrl, sampleXml);

            Console.ReadLine();
        }

        /// <summary>
        /// Basic http POST to send xml data for partner application transmission. Returns a response which includes the newly generated app number if successful.
        /// </summary>
        /// <param name="postAppUrl">url to post xml to</param>
        /// <param name="data">xml data</param>
        public static void PostApplication(string postAppUrl, string data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(postAppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                var httpContent = new StringContent(data);

                //HTTP POST
                var postResponse = client.PostAsync(postAppUrl, httpContent).Result;
                if (postResponse.IsSuccessStatusCode)
                {
                    //Get the response content and parse it.
                    string responseString = postResponse.Content.ReadAsStringAsync().Result.ToString();
                    Console.WriteLine("Response is {0}", responseString);
                }
                else
                {
                    Console.WriteLine("The request failed with a status of '{0}'", postResponse.ReasonPhrase);
                }
            }
        }

        /// <summary>
        /// Basic http GET to ping the service. Returns the date of the latest build if successful.
        /// </summary>
        /// <param name="pingUrl">url for build date</param>
        public static void PingService(string pingUrl)
        {
            using (var client = new HttpClient())
            {
                //HTTP GET
                var getResponse = client.GetAsync(pingUrl).Result;

                if (getResponse.IsSuccessStatusCode)
                {
                    //Get the response content and parse it.
                    string responseString = getResponse.Content.ReadAsStringAsync().Result.ToString();
                    Console.WriteLine("Response is {0}", responseString);
                }
                else
                {
                    Console.WriteLine("The request failed with a status of '{0}'", getResponse.ReasonPhrase);
                }
            }
        }
    }
}
