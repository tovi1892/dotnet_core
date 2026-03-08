
using System.Collections.Generic;
using myProject.Models;

namespace myProject.Interfaces
{
    public interface ITenBisService
    {
        List<TenBIs> GetAll();     
        TenBIs? GetById(int id);  
        void Add(TenBIs item);    
        bool Update(TenBIs item); 
        bool Delete(int id);     
    }
}