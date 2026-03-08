using myProject.Models;
using System.Collections.Generic;

namespace myProject.Interfaces
{
    public interface ITenBisRepository
    {
        List<TenBIs> Get();     
        TenBIs? Find(int id);  
        TenBIs Create(TenBIs item);    
        bool Update(int id, TenBIs item); 
        bool Delete(int id);     
    }
}