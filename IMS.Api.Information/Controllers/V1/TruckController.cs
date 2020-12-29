﻿using IMS.Domain.Entity;
using IMS.Infrastructure.Context;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Api.Information.Controllers.V1
{
    //[AllowAnonymous]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TruckController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Truck>>> Get([FromServices] ApplicationContext context)
        {
            var users = await context.Truck.ToListAsync();
            return users;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Truck>> Get([FromServices] ApplicationContext context, int id)
        {
            try
            {
                var user = context.Truck.FirstOrDefault(x => x.TruckId == id);
                return user;
            }
            catch
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Truck>> Post([FromServices] ApplicationContext context, [FromBody]Truck model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.Truck.Add(model);
                    await context.SaveChangesAsync();
                    return model;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Truck>> Put([FromServices] ApplicationContext context, [FromBody] Truck model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.Truck.Update(model);
                    await context.SaveChangesAsync();
                    return model;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete([FromServices] ApplicationContext context, int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = context.Truck.FirstOrDefault(x => x.TruckId == id);
                    context.Truck.Remove(user);
                    await context.SaveChangesAsync();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
