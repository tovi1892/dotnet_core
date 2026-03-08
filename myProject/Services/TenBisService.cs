using Microsoft.AspNetCore.Mvc;
using System;
using myProject.Interfaces;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;

namespace myProject.Services;

public class TenBisService : ITenBisService

{
    private  List<TenBIs> list;

    public  TenBisService()
    {
           list = new List<TenBIs>
        {
            // User 1's items
            new TenBIs { Id = 1, Name = "Belgian waffle", Ismilky = true, UserId = 1 },
            new TenBIs { Id = 2, Name = "toast", Ismilky = true, UserId = 1 },
            // User 2's items
            new TenBIs { Id = 3, Name = "Shakshuka", Ismilky = false, UserId = 2 },
            // User 5's items  
            new TenBIs { Id = 4, Name = "Hummus with Bread", Ismilky = false, UserId = 5 },
            // User 6's items
            new TenBIs { Id = 5, Name = "Falafel Plate", Ismilky = false, UserId = 6 }
        };
    }
  

    public List<TenBIs> Get()
    {
        return list;
    }

    public TenBIs Find(int id)
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
        var TenBIs = Find(id);
        if (TenBIs == null)
            return false;
        if (TenBIs.Id != newTenBIs.Id)
            return false;

        var index = list.IndexOf(TenBIs);
        list[index] = newTenBIs;

        return true;
    }

    public bool Delete(int id)
    {
        var TenBIs = Find(id);
        if (TenBIs == null)
            return false;
        list.Remove(TenBIs);
        return true;
    }
      
}

public static class TenBisSExtension
    {
    public   static void AddTenBis(this IServiceCollection services)
        {
            services.AddSingleton<ITenBisService,TenBisService>();          
        }
    }

