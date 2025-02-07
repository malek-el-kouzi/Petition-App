using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Data;
using WebApplication7.Models;
using Microsoft.AspNetCore.Identity;

namespace WebApplication7.Controllers
{
    public class PetitionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public PetitionsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Petitions
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "All Petitions";
            return _context.Petition != null ? 
                          View(await _context.Petition.Include(p => p.Signatures).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Petition'  is null.");
        }

        [Authorize]
        public async Task<IActionResult> UserPetitions()
        {
            var userPetitions = await _context.Petition
            .Where(p => p.CreatedBy == User.Identity.Name)
            .Include(p => p.Signatures)
            .ToListAsync();

            ViewData["Title"] = "Petitions Started by You";
            return View("Index", userPetitions);
        }

        [Authorize]
        public async Task<IActionResult> UserPetitionsById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Handle the case where the user wasn't found
                return NotFound();
            }

            var userPetitions = await _context.Petition
            .Where(p => p.CreatedBy == user.UserName)
            .Include(p => p.Signatures)
            .ToListAsync();

            ViewData["Title"] = "Petitions Started by " + user.UserName;
            return View("Index", userPetitions);
        }

        // GET: Petitions/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Petition == null)
            {
                return NotFound();
            }

            var petition = await _context.Petition
                .FirstOrDefaultAsync(m => m.PetitionId == id);
            if (petition == null)
            {
                return NotFound();
            }

            return View(petition);
        }

        // GET: Petitions/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Petitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PetitionId,Title,Description,ImagePath,DateCreated,CreatedBy")] Petition petition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(petition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(petition);
        }

        // GET: Petitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Petition == null)
            {
                return NotFound();
            }

            var petition = await _context.Petition.FindAsync(id);
            if (petition == null)
            {
                return NotFound();
            }
            return View(petition);
        }

        // POST: Petitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PetitionId,Title,Description,ImagePath,DateCreated,CreatedBy")] Petition petition)
        {
            if (id != petition.PetitionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(petition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetitionExists(petition.PetitionId))
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
            return View(petition);
        }

        // GET: Petitions/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Petition == null)
            {
                return NotFound();
            }

            var petition = await _context.Petition
                .FirstOrDefaultAsync(m => m.PetitionId == id);
            if (petition == null)
            {
                return NotFound();
            }

            return View(petition);
        }

        // POST: Petitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Petition == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Petition'  is null.");
            }
            var petition = await _context.Petition.FindAsync(id);
            if (petition != null)
            {
                _context.Petition.Remove(petition);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetitionExists(int id)
        {
          return (_context.Petition?.Any(e => e.PetitionId == id)).GetValueOrDefault();
        }
    }
}
