using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OffreDmploie.Data;
using OffreDmploie.Models;

namespace OffreDmploie.Controllers
{
    public class UserCompetencesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserCompetencesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserCompetences
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserCompetences.Include(u => u.Competence).Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserCompetences/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCompetences = await _context.UserCompetences
                .Include(u => u.Competence)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userCompetences == null)
            {
                return NotFound();
            }

            return View(userCompetences);
        }

        // GET: UserCompetences/Create
        public IActionResult Create()
        {
            ViewData["CompetenceId"] = new SelectList(_context.Competences, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserCompetences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,CompetenceId")] UserCompetences userCompetences)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userCompetences);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompetenceId"] = new SelectList(_context.Competences, "Id", "Id", userCompetences.CompetenceId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCompetences.UserId);
            return View(userCompetences);
        }

        // GET: UserCompetences/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCompetences = await _context.UserCompetences.FindAsync(id);
            if (userCompetences == null)
            {
                return NotFound();
            }
            ViewData["CompetenceId"] = new SelectList(_context.Competences, "Id", "Id", userCompetences.CompetenceId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCompetences.UserId);
            return View(userCompetences);
        }

        // POST: UserCompetences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserId,CompetenceId")] UserCompetences userCompetences)
        {
            if (id != userCompetences.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userCompetences);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCompetencesExists(userCompetences.UserId))
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
            ViewData["CompetenceId"] = new SelectList(_context.Competences, "Id", "Id", userCompetences.CompetenceId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCompetences.UserId);
            return View(userCompetences);
        }

        // GET: UserCompetences/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCompetences = await _context.UserCompetences
                .Include(u => u.Competence)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userCompetences == null)
            {
                return NotFound();
            }

            return View(userCompetences);
        }

        // POST: UserCompetences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userCompetences = await _context.UserCompetences.FindAsync(id);
            if (userCompetences != null)
            {
                _context.UserCompetences.Remove(userCompetences);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserCompetencesExists(string id)
        {
            return _context.UserCompetences.Any(e => e.UserId == id);
        }
    }
}
