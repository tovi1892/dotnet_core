using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using myProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        private int GetUserId()
        {
            return int.Parse(User.FindFirst("userid")?.Value ?? "0");
        }

        [HttpGet()]
        public ActionResult<IEnumerable<TenBIs>> Get()
        {
            var userId = GetUserId();
            return _tenBisService.Get().Where(x => x.UserId == userId).ToList(); 
        }

        [HttpGet("me")]
        public ActionResult<IEnumerable<TenBIs>> GetMe()
        {
            var userId = GetUserId();
            return _tenBisService.Get().Where(x => x.UserId == userId).ToList(); 
        }

        [HttpGet("{id}")]
        public ActionResult<TenBIs> Get(int id)
        {
            var userId = GetUserId();
            var tenBis = _tenBisService.Get(id); 
            if (tenBis == null || tenBis.UserId != userId)
                return NotFound();
            return tenBis; 
        }

        [HttpPost]
        public ActionResult Create(TenBIs newTenBIs)
        {
            newTenBIs.UserId = GetUserId();
            var postedTenBIs = _tenBisService.Create(newTenBIs); 
            return CreatedAtAction(nameof(Get), new { id = postedTenBIs.Id }, postedTenBIs);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, TenBIs newTenBIs)
        {
            var userId = GetUserId();
            var tenBis = _tenBisService.Find(id);
            if (tenBis == null || tenBis.UserId != userId)
                return NotFound();
            newTenBIs.Id = id;
            newTenBIs.UserId = userId;
            if (!_tenBisService.Update(id, newTenBIs))
                return BadRequest();
            return Ok(newTenBIs);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var userId = GetUserId();
            var tenBis = _tenBisService.Find(id);
            if (tenBis == null || tenBis.UserId != userId)
                return NotFound();
            if (!_tenBisService.Delete(id))
                return NotFound();
            return Ok(tenBis);
        }
    }
}