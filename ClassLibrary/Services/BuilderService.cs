using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Models.Database;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;

namespace ClassLibrary.Services
{
    public class BuilderService
    {
        private ApplicationDbContext Database { get; }
        private IConfiguration Configuration { get; }
        
        public BuilderService(ApplicationDbContext database, IConfiguration configuration)
        {
            Database = database;
            Configuration = configuration;
        }

        private async Task Build(string uri, string file)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            await File.WriteAllTextAsync(file, Cleanup(content));
        }

        private string Cleanup(string input)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(input);
            document.DocumentNode.Descendants()
                .Where(n => n.NodeType == HtmlNodeType.Comment || n.GetAttributeValue("src", "value") == "_framework/blazor.server.js")
                .ToList()
                .ForEach(n => n.Remove());
            var e = document.GetElementbyId("blazor-error-ui");
            e?.Remove();
            return document.DocumentNode.InnerHtml;
        }
        
        public async Task BuildHome()
        {
            await Build($"{Homepage}", $"{StaticContentRoot}/index.html");
        }
        
        public async Task Build(Art art)
        {
            await Build($"{Homepage}/art/{art.Id}",
                $"{StaticContentRoot}/art/{art.Id}");
        }

        public async Task Build(Category category)
        {
            await Build($"{Homepage}/category/{category.Id}",
                $"{StaticContentRoot}/category/{category.Id}");
        }

        public async Task Build(TextPage text)
        {
            await Build($"{Homepage}/text/{text.Id}",
                $"{StaticContentRoot}/text/{text.Id}");
        }
        
        public async Task BuildAllArts()
        {
            await foreach (var art in Database.Arts)
                await Build(art);
        }
        
        public async Task BuildAllCategories()
        {
            await foreach (var category in Database.Categories)
                await Build(category);
        }
        
        public async Task BuildAllTexts()
        {
            await foreach (var texts in Database.TextPages)
                await Build(texts);
        }
        
        public async Task BuildAll()
        {
            await BuildHome();
            await BuildAllArts();
            await BuildAllCategories();
            await BuildAllTexts();
        }
        
        private string StaticContentRoot => Configuration["StaticContentRoot"];
        private string Homepage => Configuration["Homepage"];

    }
}