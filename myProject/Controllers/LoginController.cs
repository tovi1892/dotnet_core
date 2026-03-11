using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using myProject.Models;
using myProject.Services;

namespace myProject.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    public LoginController() { }

    [HttpPost]
    public ActionResult Login(LoginRequest request)
    {
        Console.WriteLine($"Login attempt: Name='{request.Name}', Password='{request.Password}'");
        var dt = DateTime.Now;

        if (request.Name != "test" || request.Password != $"t{dt.Year}#{dt.Day}!" )
        {
            Console.WriteLine("User found: False");
            return Unauthorized();
        }
        Console.WriteLine("User found: True");

        // create claims similar to other login implementations
        var claims = new List<Claim>
        {
            new Claim("username", request.Name),
            new Claim("userid", "11235813"),
            new Claim("usertype", "User")
        };

        var token = UserTokenService.GetToken(claims);
        var tokenString = UserTokenService.WriteToken(token);

        return Ok(new { token = tokenString });
    }
}