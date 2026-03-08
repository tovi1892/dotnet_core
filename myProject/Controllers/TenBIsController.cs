using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using myProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using myProject.Models;

namespace myProject.Controllers
{
 [ApiController]
[Route("api/[controller]")]
[Authorize]
public class TenBIsController : ControllerBase
{
    private readonly ITenBisService _tenBisService; 
    
    public TenBIsController(ITenBisService tenBisService)
    {
        _tenBisService = tenBisService; 
    }

    [HttpGet]
    public ActionResult<IEnumerable<TenBIs>> Get()
    {
        // הסרוויס כבר מחזיר רק את הפריטים של המשתמש המחובר!
        return Ok(_tenBisService.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<TenBIs> Get(int id)
    {
        var item = _tenBisService.GetById(id);
        if (item == null) return NotFound(); // יחזיר NotFound גם אם הפריט שייך למישהו אחר
        return Ok(item); 
    }

    [HttpPost]
    public ActionResult Create(TenBIs newTenBIs)
    {
        _tenBisService.Add(newTenBIs); // ה-UserId מוצמד אוטומטית בפנים
        return CreatedAtAction(nameof(Get), new { id = newTenBIs.Id }, newTenBIs);
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, TenBIs newTenBIs)
    {
        newTenBIs.Id = id;
        if (!_tenBisService.Update(newTenBIs))
            return Forbid(); // או NotFound
            
        return Ok(newTenBIs);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if (!_tenBisService.Delete(id))
            return NotFound();
            
        return NoContent();
    }
}
}