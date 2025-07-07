using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

class Program
{
    static async Task<string> Kisalt(string uzunLink)
    {
        using var client = new HttpClient();
        var values = new Dictionary<string, string>
        {
            { "url", uzunLink }
        };

        var content = new FormUrlEncodedContent(values);
        var response = await client.PostAsync("https://cleanuri.com/api/v1/shorten", content);
        var responseBody = await response.Content.ReadAsStringAsync();

        using var jsonDoc = JsonDocument.Parse(responseBody);
        if (jsonDoc.RootElement.TryGetProperty("result_url", out var result))
        {
            return result.GetString();
        }
        else
        {
            throw new Exception("Yanıtta kısa link bulunamadı.");
        }
    }

    static async Task Main(string[] args)
    {
        Console.Write("🔗 Uzun linki girin: ");
        string uzunLink = Console.ReadLine();

        try
        {
            string sonuc = await Kisalt(uzunLink);
            Console.WriteLine("✅ Kısa linkiniz: " + sonuc);
        }
        catch (Exception ex)
        {
            Console.WriteLine("⚠️ Hata oluştu: " + ex.Message);
        }
    }
}
