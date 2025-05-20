using System.Text.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using static Befragung1.Components.Pages.Befragung;
using System.Text.Json.Serialization;


namespace Befragung1
{

    public class WraperPage {
        public int dummy { get; set; }
        [JsonInclude] public UIPage[] WrapperUIPages { get; set; }
    };

    public class SaveLoadServices
    {
        private readonly string DateiPfad = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pageArray.json");


        public async Task SaveUIPageAsync(UIPage[] pageArray)
        {
            WraperPage wraperPage = new WraperPage
            {
                dummy = 1,
                WrapperUIPages = pageArray
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never, // 👈 Niemals `null` ignorieren
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase  // 👈 Einheitliche JSON-Namen
            };

            string json = JsonSerializer.Serialize(wraperPage, options);
            await File.WriteAllTextAsync(DateiPfad, json);
        }

        public async Task<UIPage[]> LoadUIPageAsync()
        {
            WraperPage? wraperPage = new WraperPage();

            if (!File.Exists(DateiPfad))
            {
                return null;
            }

            string json = await File.ReadAllTextAsync(DateiPfad);

            if(json == string.Empty)
            {
                return null;
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never, // 👈 Niemals `null` ignorieren
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase  // 👈 Einheitliche JSON-Namen
            };

            wraperPage = JsonSerializer.Deserialize<WraperPage>(json, options);

            return wraperPage.WrapperUIPages;
        }
    }
}