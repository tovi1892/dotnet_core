using myProject.Interfaces;
using myProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace myProject.Services
{
    public class TenBisService : ITenBisService
    {
        private readonly ITenBisRepository _repository;
        private readonly IActiveUser _activeUser;

        public TenBisService(ITenBisRepository repository, IActiveUser activeUser)
        {
            _repository = repository;
            _activeUser = activeUser;
        }

        private int GetActiveUserId()
        {
            return _activeUser?.ActiveUser?.Id 
                ?? throw new System.UnauthorizedAccessException("חובה להיות מחובר למערכת!");
        }

        public List<TenBIs> GetAll()
        {
            var activeUserId = GetActiveUserId();
            return _repository.Get()
                .Where(t => t.UserId == activeUserId)
                .ToList();
        }

        public TenBIs? GetById(int id)
        {
            var activeUserId = GetActiveUserId();
            var item = _repository.Find(id);
            
            if (item != null && item.UserId == activeUserId)
                return item;
            
            return null;
        }

        public void Add(TenBIs item)
        {
            var activeUserId = GetActiveUserId();
            item.UserId = activeUserId;
            _repository.Create(item);
        }

        public bool Update(TenBIs item)
        {
            var activeUserId = GetActiveUserId();
            var existing = _repository.Find(item.Id);
            
            if (existing == null || existing.UserId != activeUserId)
                return false;

            item.UserId = activeUserId;
            return _repository.Update(item.Id, item);
        }

        public bool Delete(int id)
        {
            var activeUserId = GetActiveUserId();
            var item = _repository.Find(id);
            
            if (item == null || item.UserId != activeUserId)
                return false;

            return _repository.Delete(id);
        }
    }

    
}
