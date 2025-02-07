using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using WebApplication7.Data;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    [Authorize]
    public class SignaturesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SignaturesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Signatures
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Get the current user's name
            var userName = User.Identity.Name;

            var signaturesForCurrentUser = await _context.Signature
                                                    .Include(s => s.Petition)
                                                    .Where(s => s.UserName == userName)
                                                    .ToListAsync();
            return View(signaturesForCurrentUser);
        }

        // GET: Signatures/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Signature == null)
            {
                return NotFound();
            }

            var signature = await _context.Signature
                .Include(s => s.Petition)
                .FirstOrDefaultAsync(m => m.SignatureId == id);
            if (signature == null)
            {
                return NotFound();
            }

            return View(signature);
        }

        // GET: Signatures/Create/id
        public IActionResult Create(int petitionId)
        {
            Console.WriteLine(petitionId);
            // Fetch the petition based on the petitionId
            var petition = _context.Petition.FirstOrDefault(p => p.PetitionId == petitionId);

            var petitionsWithSignatures = _context.Petition.Include(p => p.Signatures).ToList();


            if (petition == null)
            {
                // Petition not found, handle the scenario (e.g., return a not found view)
                return NotFound();
            }

            // Pass the petition data to the view
            ViewData["Petition"] = ViewData["Petition"] = petition.PetitionId;


            return View();
        }

        // POST: Signatures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SignatureId,PetitionId,UserName,SignatureDate")] Signature signature)
        {

            var petition = _context.Petition.FirstOrDefault(p => p.PetitionId == signature.PetitionId);
            
            signature.Petition = petition;
            _context.Add(signature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Signatures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Signature == null)
            {
                return NotFound();
            }

            var signature = await _context.Signature.FindAsync(id);
            if (signature == null)
            {
                return NotFound();
            }
            ViewData["PetitionId"] = new SelectList(_context.Petition, "PetitionId", "Description", signature.PetitionId);
            return View(signature);
        }

        // POST: Signatures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SignatureId,PetitionId,UserName,SignatureDate")] Signature signature)
        {
            if (id != signature.SignatureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(signature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SignatureExists(signature.SignatureId))
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
            ViewData["PetitionId"] = new SelectList(_context.Petition, "PetitionId", "Description", signature.PetitionId);
            return View(signature);
        }

        // GET: Signatures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var signature = await _context.Signature
                .Include(s => s.Petition)
                .FirstOrDefaultAsync(m => m.SignatureId == id);
           
            if (signature == null)
            {
                return NotFound();
            }

            return View(signature);
        }

        // POST: Signatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Signature == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Signature'  is null.");
            }
            var signature = await _context.Signature.FindAsync(id);
            if (signature != null)
            {
                _context.Signature.Remove(signature);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SignatureExists(int id)
        {
          return (_context.Signature?.Any(e => e.SignatureId == id)).GetValueOrDefault();
        }
    }
}
