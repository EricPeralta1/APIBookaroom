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
    public class UsuarisController : ApiController
    {
        private bookaroomEntities2 db = new bookaroomEntities2();

        // GET: api/users
        [ResponseType(typeof(user))]
        public IHttpActionResult Getuser()
        {
            db.Configuration.LazyLoadingEnabled = false;

            var users = db.Usuaris
                .Select(u => new
                {
                    user_id = u.user_id,
                    name = u.name,
                    surname = u.surname,
                    email = u.email,
                    password = u.password,
                    role = u.role,
                    active = u.active
                })
                .ToList();

            return Ok(users);
        }

        // GET: api/Usuaris/5
        [ResponseType(typeof(Usuaris))]
        public async Task<IHttpActionResult> GetUsuarisAsync(int id)
        {
            IHttpActionResult result;

            var user = await db.Usuaris
                .Where(u => u.user_id == id)
                .Select(u => new
                {
                    idUser = u.user_id,
                    name = u.name,
                    surname = u.surname,
                    email = u.email,
                    password = u.password,
                    role = u.role,
                    active = u.active
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(user);
            }

            return result; ;
        }

        // PUT: api/Usuaris/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsuaris(int id, Usuaris usuaris)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuaris.user_id)
            {
                return BadRequest();
            }

            db.Entry(usuaris).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarisExists(id))
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

        // POST: api/Usuaris
        [ResponseType(typeof(Usuaris))]
        public IHttpActionResult PostUsuaris(Usuaris usuaris)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var allowedRoles = new List<string> { "Superadmin", "Common User", "Event Organizer" };

            if (db.Usuaris.Any(u => u.user_id == usuaris.user_id))
            {
                return Conflict();
            } else if (db.Usuaris.Any((u => u.email == usuaris.email))){ 
                return Conflict();
            }else if (!allowedRoles.Contains(usuaris.role))
            {
                return BadRequest("El rol debe ser 'Superadmin', 'Common User' o 'Event Organizer'.");
            } else {
                db.Usuaris.Add(usuaris);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = usuaris.user_id }, usuaris);
            }

        }

        // DELETE: api/Usuaris/5
        [ResponseType(typeof(Usuaris))]
        public IHttpActionResult DeleteUsuaris(int id)
        {
            Usuaris usuaris = db.Usuaris.Find(id);
            if (usuaris == null)
            {
                return NotFound();
            }

            db.Usuaris.Remove(usuaris);
            db.SaveChanges();

            return Ok(usuaris);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarisExists(int id)
        {
            return db.Usuaris.Count(e => e.user_id == id) > 0;
        }
    }
}