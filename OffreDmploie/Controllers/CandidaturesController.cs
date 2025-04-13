using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OffreDmploie.Data;
using OffreDmploie.Models;

namespace OffreDmploie.Controllers
{
    [Authorize]
    public class CandidaturesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CandidaturesController(ApplicationDbContext context , UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Candidatures
        

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Candidat"))
            {
                var user = await _userManager.GetUserAsync(User);
                var applicationDbContext = _context.Candidatures.Include(c => c.Job).Include(c => c.User)
                    .Where(c => c.IdUser == user.Id);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {

                var user = await _userManager.GetUserAsync(User);
                var applicationDbContext = _context.Candidatures.Include(c => c.Job).Include(c => c.User);
                return View(await applicationDbContext.ToListAsync());
            }
        }

        [HttpGet]
        public async Task<IActionResult> RejectCand(int id)
        {
            var candidature = await _context.Candidatures
                .Include(c => c.Job)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (candidature == null) return NotFound();

            candidature.Statusdecondidature = "Rejetée";
            _context.Update(candidature);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Jobs", new { id = candidature.Job.Id });
        }

        [HttpGet]
        public async Task<IActionResult> AcceptCand(int id)
        {
            var candidature = await _context.Candidatures
                .Include(c => c.Job)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (candidature == null) return NotFound();

            candidature.Statusdecondidature = "Acceptée";
            _context.Update(candidature);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Jobs", new { id = candidature.Job.Id });
        }

        public async Task<IActionResult> DownloadResume(int id)
        {
            var candidature = await _context.Candidatures.FindAsync(id);
            if (candidature == null || candidature.ResumeFile == null)
            {
                return NotFound();
            }

            return File(candidature.ResumeFile, candidature.ResumeContentType ?? "application/octet-stream", candidature.ResumeFileName ?? "resume.pdf");
        }


        // GET: Candidatures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidature = await _context.Candidatures
                .Include(c => c.Job)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidature == null)
            {
                return NotFound();
            }

            return View(candidature);
        }

        // GET: Candidatures/Create
        public  IActionResult Create(int id)
        {
            var job = _context.Jobs.Find(id);
            if (job == null)
            {
                return NotFound(); // Job inexistant => stop
            }
            ViewData["IdUser"] = new SelectList(_context.Users, "Id", "Id");

            return View();

        }

        // POST: Candidatures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id , [Bind("DescriptionCondidature,Datedesoumis,Statusdecondidature")] Candidature candidature , IFormFile Resume)
        {
            var user = await _userManager.GetUserAsync(User);
      

            candidature.IdJob = id;
            candidature.IdUser = user.Id;
            var job = _context.Jobs.Find(id);

            if (Resume != null &&Resume.Length > 0 && ModelState.IsValid)
            {
                using var memoryStream = new MemoryStream();
                await Resume.CopyToAsync(memoryStream);
                candidature.ResumeFile = memoryStream.ToArray();
                candidature.ResumeFileName = Resume.FileName;
                candidature.ResumeContentType = Resume.ContentType;
                _context.Add(candidature);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdJob"] = new SelectList(_context.Jobs, "Id", "Id", candidature.IdJob);
            ViewData["IdUser"] = new SelectList(_context.Users, "Id", "Id", candidature.IdUser);
            return View(job );
        }
        // GET: Candidatures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidature = await _context.Candidatures.FindAsync(id);
            if (candidature == null)
            {
                return NotFound();
            }

            ViewData["IdJob"] = new SelectList(_context.Jobs, "Id", "Id", candidature.IdJob);
            ViewData["IdUser"] = new SelectList(_context.Users, "Id", "Id", candidature.IdUser);
            return View(candidature);
        }

        // POST: Candidatures/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DescriptionCondidature,Datedesoumis,Statusdecondidature")] Candidature candidatureInput, IFormFile Resume)
        {
            if (id != candidatureInput.Id)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var candidature = await _context.Candidatures.FindAsync(id);
            if (candidature == null)
            {
                return NotFound();
            }

            candidature.DescriptionCondidature = candidatureInput.DescriptionCondidature;
            candidature.Datedesoumis = candidatureInput.Datedesoumis;
            candidature.Statusdecondidature = candidatureInput.Statusdecondidature;
            candidature.IdUser = user.Id;

            if (Resume != null && Resume.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await Resume.CopyToAsync(memoryStream);
                candidature.ResumeFile = memoryStream.ToArray();
                candidature.ResumeFileName = Resume.FileName;
                candidature.ResumeContentType = Resume.ContentType;
            }

            try
            {
                _context.Update(candidature);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CandidatureExists(candidature.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: Candidatures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidature = await _context.Candidatures
                .Include(c => c.Job)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidature == null)
            {
                return NotFound();
            }

            return View(candidature);
        }

        // POST: Candidatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var candidature = await _context.Candidatures.FindAsync(id);
            if (candidature != null)
            {
                _context.Candidatures.Remove(candidature);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidatureExists(int id)
        {
            return _context.Candidatures.Any(e => e.Id == id);
        }
    }
}
