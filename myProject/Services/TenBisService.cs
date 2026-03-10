using myProject.Interfaces;
using myProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace myProject.Services
{
    public class TenBisService : ITenBisService
    {
        private List<TenBIs> list;

        public TenBisService()
        {
            list = new List<TenBIs>
            {
                new TenBIs { Id = 1, Name = "Belgian waffle", IsMilky = true, UserId = 1 },
                new TenBIs { Id = 2, Name = "toast", IsMilky = true, UserId = 1 },
                new TenBIs { Id = 3, Name = "Shakshuka", IsMilky = false, UserId = 2 },
                new TenBIs { Id = 4, Name = "Hummus with Bread", IsMilky = false, UserId = 5 },
                new TenBIs { Id = 5, Name = "Falafel Plate", IsMilky = false, UserId = 6 }
            };
        }

        public List<TenBIs> Get()
        {
            return list;
        }

        public TenBIs? Find(int id)
        {
            return list.FirstOrDefault(p => p.Id == id);
        }

        public TenBIs Get(int id) => Find(id);

        public TenBIs Create(TenBIs newTenBIs)
        {
            var maxId = list.Max(p => p.Id);
            newTenBIs.Id = maxId + 1;
            list.Add(newTenBIs);
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

            return true;
        }

        public bool Delete(int id)
        {
            var tenBis = Find(id);
            if (tenBis == null)
                return false;
            list.Remove(tenBis);
            return true;
        }
        
        // delete all tenbis for given user id
        public void DeleteByUserId(int userId)
        {
            list.RemoveAll(t => t.UserId == userId);
        }
    }
}