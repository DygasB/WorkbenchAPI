using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using WorkbenchAPI.Entities;
using WorkbenchAPI.Models;
using WorkbenchAPI.Services;

namespace WorkbenchAPI.Controllers
{
    [Route("[controller]/shop")]
    [ApiController]
    public class ShopController : ControllerBase
    {

        private readonly IShopService _shopservice;

        public ShopController(IShopService shopservice)
        {
            _shopservice = shopservice;
        }
        [HttpGet]
        //[AllowAnonymous]
        [Authorize(Policy ="HasNationality")]
        public ActionResult<IEnumerable<Shop>> GetAll()
        {
            var shops = _shopservice.GetAll();
            return Ok(shops);
        }
        [HttpGet("{id}")]
        //[Authorize]
        [Authorize(Policy= "Atleast20")]
        public ActionResult<Shop> GetById([FromRoute]int id)
        {
            var shop = _shopservice.GetById(id);
            return Ok(shop);
            
        }
        [HttpPost]
        [Authorize(Roles = "Boss,Leader")]
        public ActionResult CreateRestaurant([FromBody] CreateShopDto dto)
        {
            //HttpContext.User.IsInRole("Boss");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var id = _shopservice.CreateRestaurant(dto);

            return Created("/Shop/shop/{id}", null);
        }
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _shopservice.DeleteById(id);
            return NoContent();
        }
        [HttpPut("{id}")]
        public ActionResult UpdatePut([FromRoute] int id,[FromBody] UpdateShopDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _shopservice.UpdatePut(id, dto);
            return Ok();
                
            
        }
    }
}
