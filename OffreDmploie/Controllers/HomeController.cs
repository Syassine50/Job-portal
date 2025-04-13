using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OffreDmploie.Data;
using OffreDmploie.Models;

namespace OffreDmploie.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context , UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
        

        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var applicationDbContextJobs = _context.Jobs
                    .Include(j => j.Domaine)
                    .Include(j => j.User).Where(j=>j.Datefin==DateOnly.FromDateTime(DateTime.Today));
        if (applicationDbContextJobs == null) return NotFound();
        foreach (var job in applicationDbContextJobs)
        {
            var jobToUpdate = await _context.Jobs.FindAsync(job.Id);
            if (jobToUpdate == null) return NotFound();
            jobToUpdate.Statusdeloffre = "Fermé";
            _context.Update(jobToUpdate);
            await _context.SaveChangesAsync();
            var applicationDbContext = _context.Candidatures.Include(c => c.Job).Include(c => c.User).Where(c => c.Statusdecondidature == "En cours");
            foreach (var item in applicationDbContext)
            {
                var candidature = await _context.Candidatures.FindAsync(item.Id);

                if (candidature == null) return NotFound();

                candidature.Statusdecondidature = "Rejetée";
                _context.Update(candidature);
                await _context.SaveChangesAsync();
            }
        }
        var applicationDbContextjojo = _context.Jobs
                    .Include(j => j.Domaine)
                    .Include(j => j.User);
        return View(await applicationDbContextjojo.ToListAsync());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
