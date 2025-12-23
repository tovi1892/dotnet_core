using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic; 
using myProject.Interfaces;
using Microsoft.AspNetCore.Authorization; 

namespace myProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TenBIsController : ControllerBase
    {
        private readonly ITenBisService _tenBIsService; 
        public TenBIsController(ITenBisService tenBIsService)
        {
            _tenBIsService = tenBIsService; 
        }

        [HttpGet()]
        public ActionResult<IEnumerable<TenBIs>> Get()
        {
            return _tenBIsService.Get(); 
        }

        [HttpGet("{id}")]
        public ActionResult<TenBIs> Get(int id)
        {
            var tenBIs = _tenBIsService.Get(id); 
            if (tenBIs == null)
                return NotFound();
            return tenBIs; 
        }

        [HttpPost]
        public ActionResult Create(TenBIs newTenBIs)
        {
            var postedTenBIs = _tenBIsService.Create(newTenBIs); 
            return CreatedAtAction(nameof(Get), new { id = postedTenBIs.Id }, postedTenBIs);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, TenBIs newTenBIs)
        {
            var tenBIs = _tenBIsService.Find(id);
            if (tenBIs == null)
                return NotFound();
            newTenBIs.Id = id;
            if (!_tenBIsService.Update(id, newTenBIs))
                return BadRequest();
            return Ok(newTenBIs);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var tenBIs = _tenBIsService.Find(id);
            if (tenBIs == null)
                return NotFound();
            if (!_tenBIsService.Delete(id))
                return NotFound();
            return Ok(tenBIs);
        }
    }
}