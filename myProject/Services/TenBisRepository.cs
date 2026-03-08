using myProject.Interfaces;
using myProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace myProject.Services
{
    public class TenBisRepository : ITenBisRepository
    {
        private List<TenBIs> _items;

        public TenBisRepository()
        {
            _items = new List<TenBIs>
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
            return _items;
        }

        public TenBIs? Find(int id)
        {
            return _items.FirstOrDefault(p => p.Id == id);
        }

        public TenBIs Create(TenBIs item)
        {
            var maxId = _items.Any() ? _items.Max(p => p.Id) : 0;
            item.Id = maxId + 1;
            _items.Add(item);
            return item;
        }

        public bool Update(int id, TenBIs item)
        {
            var existing = Find(id);
            if (existing == null)
                return false;

            var index = _items.IndexOf(existing);
            _items[index] = item;
            return true;
        }

        public bool Delete(int id)
        {
            var item = Find(id);
            if (item == null)
                return false;

            _items.Remove(item);
            return true;
        }
    }
}
