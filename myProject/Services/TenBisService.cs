using myProject.Interfaces;
using myProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace myProject.Services
{
    public class TenBisService : ITenBisService
    {
        private List<TenBIs> list;
        private string filePath;

        private List<TenBIs> GetDefaultTenBis()
        {
            return new List<TenBIs>
            {
                new TenBIs { Id = 1, Name = "Belgian waffle", IsMilky = true, UserId = 1 },
                new TenBIs { Id = 2, Name = "toast", IsMilky = true, UserId = 1 },
                new TenBIs { Id = 3, Name = "Shakshuka", IsMilky = false, UserId = 2 },
                new TenBIs { Id = 4, Name = "Hummus with Bread", IsMilky = false, UserId = 5 },
                new TenBIs { Id = 5, Name = "Falafel Plate", IsMilky = false, UserId = 6 }
            };
        }

        public TenBisService(IWebHostEnvironment webHost)
        {
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "tenbis.json");

            list = GetDefaultTenBis();

            if (File.Exists(filePath))
            {
                try
                {
                    using (var jsonFile = File.OpenText(filePath))
                    {
                        var content = jsonFile.ReadToEnd();
                        if (!string.IsNullOrWhiteSpace(content) && content != "[]")
                        {
                            var loadedTenBis = JsonSerializer.Deserialize<List<TenBIs>>(content,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                            if (loadedTenBis != null && loadedTenBis.Count > 0)
                            {
                                list = loadedTenBis;
                            }
                        }
                    }
                }
                catch
                {
                    // אם יש שגיאה בטעינה, נשתמש בברירות המחדל
                    list = GetDefaultTenBis();
                }
            }

            // תמיד שמור את הנתונים כך שיהיו מעודכנים
            saveToFile();
        }

        private void saveToFile()
        {
            var text = JsonSerializer.Serialize(list);
            File.WriteAllText(filePath, text);
        }

        public List<TenBIs> Get()
        {
            saveToFile();
            return list;
        }

        public TenBIs? Find(int id)
        {
            return list.FirstOrDefault(p => p.Id == id);
        }

        public TenBIs Get(int id) => Find(id)!;

        public TenBIs Create(TenBIs newTenBIs)
        {
            var maxId = list.Max(p => p.Id);
            newTenBIs.Id = maxId + 1;
            list.Add(newTenBIs);
            saveToFile();
            return newTenBIs;
        }

        public bool Update(int id, TenBIs newTenBIs)
        {
            var tenBis = Find(id);
            if (tenBis == null)
                return false;
            if (tenBis.Id != newTenBIs.Id)
                return false;

            var index = list.IndexOf(tenBis);
            list[index] = newTenBIs;
            saveToFile();
            return true;
        }

        public bool Delete(int id)
        {
            var tenBis = Find(id);
            if (tenBis == null)
                return false;
            list.Remove(tenBis);
            saveToFile();
            return true;
        }

        public void DeleteByUserId(int userId)
        {
            list.RemoveAll(t => t.UserId == userId);
            saveToFile();
        }
    }

    public static class TenBisExtension
    {
        public static void AddTenBis(this IServiceCollection services)
        {
            services.AddSingleton<ITenBisService, TenBisService>();
        }
    }
}