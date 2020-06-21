using Newtonsoft.Json;
using PackingSlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace PackingSlip.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Customer Email:");
            string customerMail = System.Console.ReadLine();

            System.Console.WriteLine("Customer Name:");
            string customerName = System.Console.ReadLine();

            System.Console.WriteLine("Agent Name:");
            string agentName = System.Console.ReadLine();

            System.Console.WriteLine("Activate Membership?(Y/N):");
            string membershipActivate = System.Console.ReadLine();

            System.Console.WriteLine("Upgrade Membership?(Y/N):");
            string membershipUpgrade = System.Console.ReadLine();

            System.Console.WriteLine("Has Physical products?(Y/N):");
            string physicalProduct = System.Console.ReadLine();

            System.Console.WriteLine("Has book?(Y/N):");
            string book = System.Console.ReadLine();

            System.Console.WriteLine("Enter item(s):");
            System.Console.WriteLine("Name:");
            List<string> itemList = new List<string>();
            string itemName = System.Console.ReadLine();
            System.Console.WriteLine("Qty:");
            string qty = System.Console.ReadLine();
            if (!string.IsNullOrEmpty(itemName) && !string.IsNullOrEmpty(qty))
            {
                itemList.Add($"{itemName}``{qty}");
            }
            else
            {
                System.Console.WriteLine("Cannot continue with 0 items.");
                System.Console.ReadLine();
            }

            PackingSlipHeader packingSlip = new PackingSlipHeader
            {
                CustomerEmail = customerMail,
                CustomerName = customerName,
                AgentName = agentName,
                ActivateMembership = membershipActivate.ToUpper()=="Y"?true:false,
                UpgradeMembership = membershipUpgrade.ToUpper()=="Y"?true:false,
                HasPhysicalProduct = physicalProduct.ToUpper()=="Y"?true:false,
                HasBook=book.ToUpper()=="Y"?true:false,
            };

            itemList.ForEach(s =>
            {
                string[] itemDetail = s.Split("``");
                packingSlip.PackingSlipItems.Add(new PackingSlipItem
                {
                    Name = itemDetail[0],
                    Quantity = Convert.ToInt32(itemDetail[1])
                });
            });

            string output = PrintPackingSlip(packingSlip);
            System.Console.WriteLine(output);
            System.Console.ReadLine();
        }

        private static string PrintPackingSlip(PackingSlipHeader packingSlip)
        {
            string response = string.Empty;
            string requestBody = JsonConvert.SerializeObject(packingSlip);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44383");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var apiResponse = client.PostAsync("https://localhost:44383/api/packing/SaveSlip", new StringContent(requestBody, Encoding.UTF8, "application/json")).Result;

                if(apiResponse!=null && apiResponse.Content!=null && apiResponse.IsSuccessStatusCode)
                {
                    var message = JsonConvert.DeserializeObject<ResponseMessage>(apiResponse.Content.ReadAsStringAsync().Result);
                    response = message.Detail;
                }
                else
                {
                    response = "Packing slip could not be generated";
                }
            }

            return response;
        }
    }
}
