using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ClassLibrary.Models.Database;
using ClassLibrary.Models.Enums;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace ClassLibrary.Services
{
    public class ImageService
    {
        
        private DatabaseService DatabaseService { get; }
        private IConfiguration Configuration { get; }
        
        public ImageService(DatabaseService databaseService, IConfiguration configuration)
        {
            DatabaseService = databaseService;
            Configuration = configuration;
        }

        private int SizeLimit(ArtSize size) => size switch
        {
            ArtSize.Min => 600,
            ArtSize.Normal => 1600,
            ArtSize.Max => 4096,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        private async Task Resize(string source, string target, int limit)
        {
            Image image = await Image.LoadAsync(source);
            float factor = ScaleFactor(image, limit);
            image.Mutate(i
                => i.Resize((int)(image.Width * factor), (int)(image.Height * factor)));
            var encoder = new JpegEncoder()
            {
                Quality = 30
            };
            await image.SaveAsync(target, encoder);
        }

        private async Task Resize(string name, ArtSize size)
        {
            await Resize($"{SystemRoot}/max/{name}", $"{SystemRoot}/{SizeFolder(size)}/{name}", SizeLimit(size));
        }

        public async Task ResizeAll()
        {
            foreach (var file in Directory.GetFiles($"{SystemRoot}/max"))
            {
                var name = Path.GetFileName(file);
                await Resize(name, ArtSize.Normal);
                await Resize(name, ArtSize.Min);
            }
        }

        private string SizeFolder(ArtSize size) => size.ToString().ToLower();
        
        private float ScaleFactor(IImageInfo image, int limit)
        {
            float factor = 1f;
            float max = image.Width > image.Height ? image.Width : image.Height;
            if (max > factor)
                factor = (float)limit / max;
            return factor;
        }
        
        public async Task<string> UploadImage(IBrowserFile file)
        {
            var name = file.Name.Replace(" ", "");
            await using FileStream fs = new($"{SystemRoot}/max/{name}", FileMode.Create);
            await file.OpenReadStream( 1024 * 1024 * 1024).CopyToAsync(fs);
            await Resize(name, ArtSize.Min);
            await Resize(name, ArtSize.Normal);
            return name;
        }

        public async Task UploadDetails(Art art, IEnumerable<IBrowserFile> files)
        {
            
            foreach (var file in files)
            {
                var name = await UploadImage(file);
                Detail detail = new()
                {
                    Art = art,
                    Image = name
                };
                art.Details.Add(detail);
            }

            await DatabaseService.SaveArt(art);
        }
        
        public async Task BulkUpload(Category category, IEnumerable<IBrowserFile> files)
        {
            List<Art> artList = new();
            foreach (var file in files)
            {
                var name = await UploadImage(file);
                Art art = ParseArt(name);
                art.Category = category;
                artList.Add(art);
            }

            await DatabaseService.SaveArt(artList);
        }

        private Art ParseArt(string name)
        {
            Art art = new();
            var split = name.Split('_');
            art.Date = split[0];
            art.Title = split[1].Replace('-',' ');
            if (split.Length >= 3)
            {
                var sizeSplit = split[2].Split('x');
                int width = 0;
                int height = 0;
                if (sizeSplit.Length >= 2)
                {
                    int.TryParse(sizeSplit[0], out width);
                    int.TryParse(sizeSplit[1], out height);
                }

                art.Width = width;
                art.Height = height;
            }
            art.Image = name;
            return art;
        }

        public string ImagePath(Art art, ArtSize size)
        {
            return art == null ? "" : $"{ContentRoot}/{SizeFolder(size)}/{art.Image}";
        }
        
        public string ImagePath(Detail detail, ArtSize size) 
            => detail == null ? "" : $"{ContentRoot}/{SizeFolder(size)}/{detail.Image}";
        
        private string SystemRoot => Configuration["ImageRoot"];
        private string ContentRoot => Configuration["ImageContentRoot"];
        
    }
}