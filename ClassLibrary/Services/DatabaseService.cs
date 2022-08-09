using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.Helpers;
using ClassLibrary.Models.Database;
using ClassLibrary.Models.Enums;
using ClassLibrary.Models.Filter;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Services
{
    public class DatabaseService
    {
        
        private ApplicationDbContext Database { get; }
        
        public DatabaseService(ApplicationDbContext database)
        {
            Database = database;
        }

        public async Task<List<Category>> SearchCategories(CategoryFilter filter, bool allowAll = false)
        {
            return await Database.Categories
                .Where(c => !c.IsPrivate || allowAll)
                .Include(c => c.TitleArt)
                .Filter(filter)
                .ToListAsync();
        }

        public async Task<Category> LoadCategory(int id, bool allowAll = false)
        {
            return await Database.Categories
                .Where(c => !c.IsPrivate || allowAll)
                .Include(c => c.Arts)
                .Include(c => c.TitleArt)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<TextPage> LoadText(int id, bool allowAll = false)
        {
            return await Database.TextPages
                .Where(t => !t.IsPrivate || allowAll) 
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Art> LoadArt(int id, bool allowAll = false)
        {
            return await Database.Arts
                .Where(a => !a.IsPrivate || allowAll)
                .Include(a => a.Category)
                .Include(a => a.Details)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Art>> SearchArt(ArtFilter filter)
        {
            return await Database.Arts
                .Include(a => a.Category)
                .Filter(filter)
                .ToListAsync();
        }

        public async Task<List<TextPage>> SearchText(TextFilter filter)
        {
            return await Database.TextPages
                .Filter(filter)
                .ToListAsync();
        }

        public async Task<TextPage> LoadText(TextType type)
        {
            return await Database.TextPages
                .Where(p => p.Type == type)
                .FirstOrDefaultAsync();
        }

        public async Task Delete(Detail detail)
        {
            detail.Art.Details.Remove(detail);
            Database.Details.Remove(detail);
            await Database.SaveChangesAsync();
        }

        public async Task SaveText(TextPage text)
        {
            if (text.Id == 0)
                Database.TextPages.Add(text);
            else
                Database.TextPages.Update(text);
            await Database.SaveChangesAsync();
        }
        
        public async Task SaveCategory(Category category)
        {
            if (category.Id == 0)
                Database.Categories.Add(category);
            else
                Database.Categories.Update(category);
            await Database.SaveChangesAsync();
        }
        
        public async Task SaveArt(Art art)
        {
            if (art.Id == 0)
                Database.Arts.Add(art);
            else 
                Database.Arts.Update(art);
            await Database.SaveChangesAsync();
        }

        public async Task SaveArt(List<Art> artList)
        {
            foreach (var art in artList)
            {
                if (art.Id == 0)
                    Database.Arts.Add(art);
                else 
                    Database.Arts.Update(art);
            }
            await Database.SaveChangesAsync();
        }
        
    }
}