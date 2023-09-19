using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using StringContent = System.Net.Http.StringContent;

namespace ConsoleApp1
{
    internal class Program
    {

        static void Main(string[] args)
        {
            //set up header
            string apiKey = "patLiCnsw9GKAq3a5.cfe9b45db7af6d959df19f781990e75229a6edd1d757ce96d4d9ca780da9c5e5";
            string baseUrl = "https://api.airtable.com/v0/appKWRViveRaGKmOZ/Students?maxRecodrs=1000&view=Students";
            HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            // ViewAllRecords(http, baseUrl);
            CreateStudent(http, baseUrl).Wait();          
        }
        public static void ViewAllRecords(HttpClient http, string Url)
        {
            string reposn = http.GetStringAsync(Url).Result.ToString();
            JObject h = JObject.Parse(reposn);
            var json = h["records"];
            if (h.ContainsKey("records"))
            {
                int i = 1;
                foreach (var item in json)
                {
                    
                    Console.WriteLine("STT:   " + i);
                    Console.WriteLine(item["id"]);
                    var fields = item["fields"];
                    Console.WriteLine($"Student ID: {fields["student_id"]}");
                    Console.WriteLine($"Name: {fields["name"]}");
                    Console.WriteLine($"Birthday: {fields["birthday"]}");
                    Console.WriteLine();
                    i++;
                }
            }
            else
            {
                Console.WriteLine("Error Network");
            }
        }
        public static async Task CreateStudent(HttpClient http, string baseUrl)
        {

            Console.Write("nhập thông tin học sin");
            Console.WriteLine("nhập id");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("nhap ten");
            string name = Console.ReadLine();
            Console.WriteLine("ngay sinh");
            string date = Console.ReadLine();
            var studentData = new
            {
                records = new[]
        {
            new
            {
                fields = new
                {
                    student_id = id,
                    name = name,
                    birthday = date
                }
            }
        }
            };
            string jsonData = JsonConvert.SerializeObject(studentData);

            // Tạo đối tượng StringContent từ chuỗi JSON
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

          HttpResponseMessage create = await http.PostAsync(baseUrl,content);
            if (create.IsSuccessStatusCode)
            {
                Console.WriteLine("POST Success!");
                // You can handle the response content here if needed.
            }
            else
            {
                Console.WriteLine($"POST request failed with status code: {create.StatusCode}");
            }
            
        }
        public static async Task Test()
        {
            HttpClient http = new HttpClient();
            
        }
    }
}