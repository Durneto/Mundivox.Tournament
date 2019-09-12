using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mundivox.Tournament.Data;
using Mundivox.Tournament.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Mundivox.Tournament.Web.Controllers
{
    public class PhaseController : Controller
    {
        private readonly DataContext _context;

        public PhaseController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _context.Phase.ToListAsync();

            if(list.Count == 0)
            {
                _context.Add(new Phase { Name = "Semifinal" });
                _context.Add(new Phase { Name = "Final" });
                await _context.SaveChangesAsync();
            }

            return View(await _context.Phase.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phase = await _context.Phase
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phase == null)
            {
                return NotFound();
            }

            return View(phase);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Phase phase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phase);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phase = await _context.Phase.FindAsync(id);
            if (phase == null)
            {
                return NotFound();
            }
            return View(phase);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Phase phase)
        {
            if (id != phase.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhaseExists(phase.Id))
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
            return View(phase);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phase = await _context.Phase
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phase == null)
            {
                return NotFound();
            }

            return View(phase);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phase = await _context.Phase.FindAsync(id);
            _context.Phase.Remove(phase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhaseExists(int id)
        {
            return _context.Phase.Any(e => e.Id == id);
        }
    }
}
