using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using myProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace myProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]  // ← CHANGED: Add /api prefix for REST convention
    [Authorize]
    public class TenBIsController : ControllerBase
    {
        private readonly ITenBisService _tenBIsService; 
        
        public TenBIsController(ITenBisService tenBIsService)
        {
            _tenBIsService = tenBIsService; 
        }

        // ← NEW METHOD: Helper to extract userId from JWT token
        private int GetUserId()
        {
            return int.Parse(User.FindFirst("userid")?.Value ?? "0");
        }

        [HttpGet()]
        public ActionResult<IEnumerable<TenBIs>> Get()
        {
            var userId = GetUserId();
            // ← CHANGED: Filter items to only show current user's items
            return _tenBIsService.Get().Where(x => x.UserId == userId).ToList(); 
        }

        // ← NEW ENDPOINT: Get current user's items (convenience endpoint)
        [HttpGet("me")]
        public ActionResult<IEnumerable<TenBIs>> GetMe()
        {
            var userId = GetUserId();
            return _tenBIsService.Get().Where(x => x.UserId == userId).ToList(); 
        }

        [HttpGet("{id}")]
        public ActionResult<TenBIs> Get(int id)
        {
            var userId = GetUserId();
            var tenBIs = _tenBIsService.Get(id);
            // ← NEW: Validate ownership before returning
            if (tenBIs == null || tenBIs.UserId != userId)
                return NotFound();
            return tenBIs; 
        }

        [HttpPost]
        public ActionResult Create(TenBIs newTenBIs)
        {
            // ← NEW: Automatically assign UserId from token (prevent user from changing it)
            newTenBIs.UserId = GetUserId();
            var postedTenBIs = _tenBIsService.Create(newTenBIs); 
            return CreatedAtAction(nameof(Get), new { id = postedTenBIs.Id }, postedTenBIs);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, TenBIs newTenBIs)
        {
            var userId = GetUserId();
            var tenBIs = _tenBIsService.Find(id);
            // ← NEW: Validate ownership before allowing update
            if (tenBIs == null || tenBIs.UserId != userId)
                return NotFound();
            newTenBIs.Id = id;
            newTenBIs.UserId = userId;  // ← NEW: Enforce user ownership
            if (!_tenBIsService.Update(id, newTenBIs))
                return BadRequest();
            return Ok(newTenBIs);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var userId = GetUserId();
            var tenBIs = _tenBIsService.Find(id);
            // ← NEW: Validate ownership before allowing deletion
            if (tenBIs == null || tenBIs.UserId != userId)
                return NotFound();
            if (!_tenBIsService.Delete(id))
                return NotFound();
            return Ok(tenBIs);
        }
    }
}