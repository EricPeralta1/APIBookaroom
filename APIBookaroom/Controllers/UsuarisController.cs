using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using APIBookaroom.Models;

namespace APIBookaroom.Controllers
{
    public class UsuarisController : ApiController
    {
        private bookaroomEntities2 db = new bookaroomEntities2();

        // GET: api/Usuaris
        public IQueryable<Usuaris> GetUsuaris()
        {
            db.Configuration.LazyLoadingEnabled = false;


            var users = db.user
                .Select(u => new
                {
                    idUser = u.idUser,
                    idRol = u.idRol,
                    name = u.name,
                    email = u.email,
                    password = u.password
                })
                .ToList();

            return Ok(users);
        }

        // GET: api/Usuaris/5
        [ResponseType(typeof(Usuaris))]
        public IHttpActionResult GetUsuaris(int id)
        {
            Usuaris usuaris = db.Usuaris.Find(id);
            if (usuaris == null)
            {
                return NotFound();
            }

            return Ok(usuaris);
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

            db.Usuaris.Add(usuaris);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = usuaris.user_id }, usuaris);
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