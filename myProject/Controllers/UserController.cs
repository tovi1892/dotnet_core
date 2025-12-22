
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using myProject.Interfaces;
namespace myProject.Controllers;

[ApiController]
[Route("[controller]")]
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

}