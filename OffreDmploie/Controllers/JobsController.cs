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

    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public JobsController(ApplicationDbContext context , UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> CloseJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();
            job.Statusdeloffre = "Fermé";
            _context.Update(job);
            await _context.SaveChangesAsync();
            var applicationDbContext = _context.Candidatures.Include(c => c.Job).Include(c => c.User).Where(c => c.Statusdecondidature == "En cours");
            foreach(var item in applicationDbContext)
            {
                var candidature = await _context.Candidatures.FindAsync(item.Id);

                if (candidature == null) return NotFound();

                candidature.Statusdecondidature = "Rejetée";
                _context.Update(candidature);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> OuvrirJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();

            job.Statusdeloffre = "Ouvert";
            _context.Update(job);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Index(List<int> domaines, string Lieu)
        {
            if (User.IsInRole("Entreprise"))
            {
                var user = await _userManager.GetUserAsync(User);
                var applicationDbContext = _context.Jobs
                    .Include(j => j.Domaine)
                    .Include(j => j.User)
                    .Where(c => c.IdUser == user.Id);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var query = _context.Jobs
                    .Include(j => j.Domaine)
                    .Include(j => j.User)
                    .AsQueryable();

                if (domaines != null && domaines.Any())
                {
                    query = query.Where(o => domaines.Contains(o.IdDomaine));
                }

                if (!string.IsNullOrEmpty(Lieu))
                {
                    query = query.Where(o => o.Lieu == Lieu);
                }

                return View(await query.ToListAsync());
            }




        }

        // GET: Jobs
        

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.User)
                .Include(j => j.Candidatures)
                .Include(j => j.Domaine)
                .ThenInclude(t => t.Competences)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Jobs/Create
        [Authorize(Roles = ("Entreprise"))]
        public IActionResult Create()
        {
            ViewData["IdDomaine"] = new SelectList(_context.Domaines, "id", "Nomdomaine");
            ViewData["IdUser"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = ("Entreprise"))]
        public async Task<IActionResult> Create([Bind("Id,NomDeLoffre,Descriptiondeloffre,Datecreation,Datefin,Lieu,Typedeloffre,Typedecontrat,Statusdeloffre,IdDomaine,IdUser")] Job job)
        {
            var user = await _userManager.GetUserAsync(User);
            job.IdUser = user.Id;
            if (ModelState.IsValid)
            {
                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDomaine"] = new SelectList(_context.Domaines, "id", "Nomdomaine", job.IdDomaine);
            return View(job);
        }

        // GET: Jobs/Edit/5
        [Authorize(Roles = ("Entreprise"))]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            ViewData["IdDomaine"] = new SelectList(_context.Domaines, "id", "Nomdomaine", job.IdDomaine);
            ViewData["IdUser"] = new SelectList(_context.Users, "Id", "Id", job.IdUser);
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Entreprise"))]

        public async Task<IActionResult> Edit(int id, [Bind("Id,NomDeLoffre,Descriptiondeloffre,Datecreation,Datefin,Lieu,Typedeloffre,Typedecontrat,Statusdeloffre,IdDomaine,IdUser")] Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            job.IdUser = user.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.Id))
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
            ViewData["IdDomaine"] = new SelectList(_context.Domaines, "id", "Nomdomaine", job.IdDomaine);
            return View(job);
        }
       

        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.Domaine)
                .Include(j => j.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                _context.Jobs.Remove(job);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.Id == id);
        }
    }
}
