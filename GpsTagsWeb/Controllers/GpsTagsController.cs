using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using GpsTagsWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GpsTagsWeb.Controllers
{
    [Authorize]
    [RoutePrefix("api/gpstags")]
    public class GpsTagsController : ApiController
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            db.Entry(user).Collection("GpsTags").Load();
            var arr = user.GpsTags.ToArray();
            return Ok(arr);
        }

        [Route("{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            GpsTag gpsTrack = await db.GpsTracks.FindAsync(id);
            if (gpsTrack == null)
                return NotFound();

            return Ok(gpsTrack);
        }

        [Route("{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, [FromBody] GpsTag gpsTrack)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != gpsTrack.Id)
                return BadRequest();

            db.Entry(gpsTrack).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GpsTrackExists(id))
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

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Post(GpsTagViewModel[] gpsTrack)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = db.Users.Find(User.Identity.GetUserId()); ;
            foreach (var viewModel in gpsTrack)
            {
                db.GpsTracks.Add(viewModel.ConvertToUser(user));
            }
            await db.SaveChangesAsync();
            return Ok();
        }

        [Route("{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            GpsTag gpsTrack = await db.GpsTracks.FindAsync(id);
            if (gpsTrack == null)
                return NotFound();

            db.GpsTracks.Remove(gpsTrack);
            await db.SaveChangesAsync();

            return Ok(gpsTrack);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GpsTrackExists(int id) => db.GpsTracks.Count(e => e.Id == id) > 0;
    }

}
