
using System.Collections.Generic;

namespace myProject.Interfaces
{
    public interface ITenBisService
    {
        bool Delete(int id);
        bool Update(int id, TenBIs newTenBIs);
        TenBIs Create(TenBIs newTenBIs);
        TenBIs Find(int id);
        List<TenBIs> Get();
        TenBIs Get(int id);

    }
}