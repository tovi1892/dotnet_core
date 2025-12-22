
using Microsoft.AspNetCore.Mvc;
using System;
using myProject.Interfaces;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace myProject;

public class UserService : IUserService
{
    private List<User> Users;
    private string filePath;

    public UserService(IWebHostEnvironment webHost)
    {
        this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "User.json");
        using (var jsonFile = File.OpenText(filePath))
        {
            var content = jsonFile.ReadToEnd();
            if (string.IsNullOrWhiteSpace(content) || content == "[]")
            {
                Users = new List<User>
                {
                    new User { Id = 1, Name = "sari Rabinovitch", Age = 20, Gender = "female"},
                    new User { Id = 2, Name = "Tamer Rotan", Age = 21, Gender = "female"},
                    new User { Id = 4, Name = "Yahakov Cohen", Age = 13, Gender = "male"},
                    new User { Id = 3, Name = "Beni Levi", Age = 23, Gender = "male"}
                };
            }
            else
            {
                Users = JsonSerializer.Deserialize<List<User>>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<User>();
            }
        }
    }

    private void saveToFile()
    {
        var text = JsonSerializer.Serialize(Users);
        File.WriteAllText(filePath, text);
    }

    public List<User> Get()
    {
        saveToFile();
        return Users;
    }

    public User find(int id)
    {
        return Users.FirstOrDefault(p => p.Id == id);
    }

    public User Get(int id) => find(id);

    public User Create(User newUser)
    {
        var maxId = Users.Max(p => p.Id);
        newUser.Id = maxId + 1;
        Users.Add(newUser);
        saveToFile();

        return newUser;
    }

    public bool Update(int id, User newUser)
    {
        var user = find(id);
        if (user == null || user.Id != newUser.Id)
            return false;

        var index = Users.IndexOf(user);
        Users[index] = newUser;
        saveToFile();

        return true;
    }

    public bool Delete(int id)
    {
        var user = find(id);
        if (user == null)
            return false;

        Users.Remove(user);
        saveToFile();

        return true;
    }
}

public static class UserServiceExtension
{
    public static void addUserService(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
    }
}