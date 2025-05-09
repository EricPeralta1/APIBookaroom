using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using APIBookaroom.Models;

namespace APIBookaroom.Controllers
{
    public class ButacasController : ApiController
    {
        private bookaroomEntities2 db = new bookaroomEntities2();

        // GET: api/Butacas
        public IQueryable<Butaca> GetButaca()
        {
            return db.Butaca;
        }

        // GET: api/Butacas/5
        [ResponseType(typeof(Butaca))]
        public async Task<IHttpActionResult> GetButaca(int id)
        {
            Butaca butaca = await db.Butaca.FindAsync(id);
            if (butaca == null)
            {
                return NotFound();
            }

            return Ok(butaca);
        }

        // GET: api/Butacas/fromroom/5
        [HttpGet]
        [Route("api/Butacas/fromroom/{roomId}")]
        [ResponseType(typeof(object))]
        public IHttpActionResult GetButacasFromRoom(int roomId)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var butacas = db.Butaca
                .Where(e => e.room_id == roomId)
                .Select(u => new
                {
                    seat_id = u.seat_id,
                    room_id = u.room_id,
                    row_number = u.row_number,
                    seat_number = u.seat_number,
                })
                .ToList();

            return Ok(butacas);
        }



        // PUT: api/Butacas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutButaca(int id, Butaca butaca)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != butaca.seat_id)
            {
                return BadRequest();
            }

            db.Entry(butaca).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ButacaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Butacas
        [ResponseType(typeof(Butaca))]
        public async Task<IHttpActionResult> PostButaca(Butaca butaca)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Butaca.Add(butaca);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = butaca.seat_id }, butaca);
        }

        // DELETE: api/Butacas/5
        [ResponseType(typeof(Butaca))]
        public async Task<IHttpActionResult> DeleteButaca(int id)
        {
            Butaca butaca = await db.Butaca.FindAsync(id);
            if (butaca == null)
            {
                return NotFound();
            }

            db.Butaca.Remove(butaca);
            await db.SaveChangesAsync();

            return Ok(butaca);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ButacaExists(int id)
        {
            return db.Butaca.Count(e => e.seat_id == id) > 0;
        }
    }
}