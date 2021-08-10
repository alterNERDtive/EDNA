using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace alterNERDtive.Edna.Edts
{
    public struct StarSystem
    {
        public string Name { get; set; }

        public Location Coordinates { get; set; }
    }

    public struct Location
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public int Precision { get; set; }
    }

    public class EdtsApi
    {
        private static readonly string ApiUrl = "http://edts.thargoid.space/api/v1/";
        private static readonly HttpClient ApiClient;

        static EdtsApi()
        {
            ApiClient = new HttpClient
            {
                BaseAddress = new Uri(ApiUrl)
            };
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static StarSystem Find_System(string name)
        {
            HttpResponseMessage response = ApiClient.GetAsync($"system_position/{name}").Result;
            
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) // 400
            {
                throw new ArgumentException($"“{name}” is not a valid proc gen system name.", "~system");
            }
            
            response.EnsureSuccessStatusCode();
            dynamic json = response.Content.ReadAsAsync<dynamic>().Result["result"];

            return new StarSystem {
                Name = name,
                Coordinates = new Location {
                    X           = json["position"]["x"],
                    Y           = json["position"]["y"],
                    Z           = json["position"]["z"],
                    Precision   = json["uncertainty"]
                }
            };
        }
    }
}