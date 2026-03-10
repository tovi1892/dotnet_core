using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using myProject.Interfaces;
using System.Security.Claims;
using myProject.Services;
using myProject.Models;
using Microsoft.AspNetCore.Authorization;
namespace myProject.Controllers;

[ApiController]
[Route("api/[controller]")]  // ← CHANGED: Add /api prefix
[Authorize]
public class UserController : ControllerBase
{
    IUserService userService;
    private readonly myProject.Interfaces.ITenBisService _tenBisService;

    public UserController(IUserService userService, myProject.Interfaces.ITenBisService tenBisService)
    {
        this.userService = userService;
        this._tenBisService = tenBisService;
    }

    // ← NEW ENDPOINT: Get current user's own profile
    [HttpGet("me")]
    public ActionResult<User> GetMe()
    {
        var userId = int.Parse(User.FindFirst("userid")?.Value ?? "0");
        var user = userService.Get(userId);
        if (user == null)
            return NotFound();
        return user;
    }

    [HttpGet()]
    [Authorize(Policy = "Admin")]  // ← NEW: Only Admins can list all users
    public ActionResult<IEnumerable<User>> Get()
    {
        return userService.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        var currentUserId = int.Parse(User.FindFirst("userid")?.Value ?? "0");
        var isAdmin = User.FindFirst("usertype")?.Value == "Admin";
        if (!isAdmin && currentUserId != id)
            return Forbid();  // משתמש רגיל יכול לגשת רק לפרופיל שלו

        var user = userService.Get(id);
        if (user == null)
            return NotFound();
        return user;
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]  // ← NEW: Only Admins can create users
    public ActionResult Create(User newUser)
    {
        var postedUser = userService.Create(newUser);
        return CreatedAtAction(nameof(Get), new { id = postedUser.Id }, postedUser);
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, User newUser)
    {
        // ← NEW: Users can update themselves, Admins can update anyone
        var currentUserId = int.Parse(User.FindFirst("userid")?.Value ?? "0");
        var isAdmin = User.FindFirst("usertype")?.Value == "Admin";
        
        if (currentUserId != id && !isAdmin)
            return Forbid();

        var user = userService.find(id);
        if (user == null)
            return NotFound();
        newUser.Id = id;
        if (!userService.Update(id, newUser))
            return BadRequest();
        return Ok(newUser);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]  // ← NEW: Only Admins can delete users
    public ActionResult Delete(int id)
    {
        var user = userService.find(id);
        if (user == null)
            return NotFound();
        // remove user's tenbis first
        try
        {
            _tenBisService?.DeleteByUserId(id);
        }
        catch
        {
            // if tenbis deletion fails, continue to attempt user deletion
        }
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
        
        // Dynamic usertype assignment - checks if user is admin
        var userType = user.Name == "admin" || user.Name == "sari Rabinovitch" ? "Admin" : "User";
        
        var claims = new List<Claim>
        {
            new Claim("username", user.Name),
            new Claim("userid", user.Id.ToString()),
            new Claim("usertype", userType)
        };
        var token = UserTokenService.GetToken(claims);
        var tokenString = UserTokenService.WriteToken(token);
        return Ok(new { token = tokenString });
    }
}