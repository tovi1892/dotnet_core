
using System.Collections.Generic;
using myProject.Models;

namespace myProject.Interfaces
{
    public interface ITenBisService
    {
        bool Delete(int id);
        // Delete all tenbis that belong to a specific user
        void DeleteByUserId(int userId);
        bool Update(int id, TenBIs newTenBIs);
        TenBIs Create(TenBIs newTenBIs);
TenBIs? Find(int id);
        List<TenBIs> Get();
        TenBIs Get(int id);

    }
}