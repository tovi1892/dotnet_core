
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using myProject.Interfaces;
using System.Security.Claims;
using myProject.Services;
using myProject.Models;
using Microsoft.AspNetCore.Authorization;
namespace myProject.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }


    [HttpGet()]
    public ActionResult<IEnumerable<User>> Get()
    {
        return userService.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        var user = userService.Get(id);
        if (user == null)
            return NotFound();
        return user;
    }

    [HttpPost]
    public ActionResult Create(User newUser)
    {
        var postedUser = userService.Create(newUser);
        return CreatedAtAction(nameof(Get), new { id = postedUser.Id }, postedUser);
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, User newUser)
    {
        var user = userService.find(id);
        if (user == null)
            return NotFound();
        newUser.Id = id;
        if (!userService.Update(id, newUser))
            return BadRequest();
        return Ok(newUser);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var user = userService.find(id);

        if (user == null)
            return NotFound();
        if (!userService.Delete(id))
            return NotFound();
        return Ok(user);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult Login(LoginRequest request)
    {
        var user = userService.Login(request.Name, request.Password);
        if (user == null)
            return Unauthorized();
        var claims = new List<Claim>
        {
            new Claim("username", user.Name),
            new Claim("userid", user.Id.ToString()),
            new Claim("usertype", "User")
        };
        var token = FbiTokenService.GetToken(claims);
        var tokenString = FbiTokenService.WriteToken(token);
        return Ok(new { token = tokenString });
    }

}