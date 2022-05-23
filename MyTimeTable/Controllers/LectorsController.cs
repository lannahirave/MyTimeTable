using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTimeTable.Models;
using MyTimeTable.ModelsDTO;

namespace MyTimeTable.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LectorsController : ControllerBase
{
    private readonly MyTimeTableContext _context;

    public LectorsController(MyTimeTableContext context)
    {
        _context = context;
    }

    // GET: api/Lectors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LectorDtoRead>>> GetLectors()
    {
        var lectors  =  await _context.Lectors.Include(d => d.Organizations).ToListAsync();
        var lectorsDtoRead = new List<LectorDtoRead>();
        foreach (var lector in lectors)
        {
            var organizationsIds = await _context.OrganizationsLectors
                .Where(c => c.LectorId == lector.Id).Select(c => c.OrganizationId)
                .ToListAsync();
            var organizations = await _context.Organizations.Where(c => organizationsIds.Contains(c.Id))
                .Select(d => d.Name).ToListAsync();
            lectorsDtoRead.Add(
                new LectorDtoRead()
                {
                    Id = lector.Id,
                    Degree = lector.Degree,
                    FullName = lector.FullName,
                    Phone = lector.Phone,
                    OrganizationsIds = organizationsIds,
                    Organizations = organizations
                });
        }
        return lectorsDtoRead;
    }

    // GET: api/Lectors/5
    [HttpGet("{id}")]
    public async Task<ActionResult<LectorDtoRead>> GetLector(int id)
    {
        var lector = await _context.Lectors.Where(c=> c.Id == id).Include(d=>d.Organizations)
            .FirstOrDefaultAsync();

        if (lector == null) return NotFound();

        var organizationsIds = lector.Organizations.Select(d => d.Id).ToList();
        var organizations = lector.Organizations.Select(d => d.Name).ToList();
        var lectorDtoRead = new LectorDtoRead()
        {
            Id = lector.Id,
            Degree = lector.Degree,
            FullName = lector.FullName,
            Phone = lector.Phone,
            OrganizationsIds = organizationsIds,
            Organizations = organizations
        };
        return lectorDtoRead;
    }

    // PUT: api/Lectors/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLector(int? id, LectorsDtoWrite lectorsDtoWrite)
    {
        if (id is null) return NotFound("Id is null.");
        var lector = await _context.Lectors.FindAsync(id);
        if (lector is null) return NotFound("Lector is not found.");
        var organizationsIds = lectorsDtoWrite.OrganizationsIds;
        
        if (organizationsIds.Count() != 0)
        {
            var organizationsToDelete = await _context.OrganizationsLectors
                .Where(c => c.LectorId == id).ToListAsync();
            foreach (var organizationToDelete in organizationsToDelete)
            {
                _context.OrganizationsLectors.Remove(organizationToDelete);
            }
            await _context.SaveChangesAsync();
            var organizations = await _context.Organizations
                .Where(c => organizationsIds.Contains(c.Id)).ToListAsync();
            lector.Organizations = organizations;
        }

        lector.FullName = lectorsDtoWrite.FullName;
        lector.Degree = lectorsDtoWrite.Degree;
        lector.Degree = lectorsDtoWrite.Degree;
        
        _context.Entry(lector).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LectorExists(id.Value))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Lectors
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Lector>> PostLector(LectorsDtoWrite lectorsDtoWrite)
    {
        if (lectorsDtoWrite.OrganizationsIds != null && !lectorsDtoWrite.OrganizationsIds.Any()) return NotFound("No organization id.");
        var organizations = await _context.Organizations
            .Where( c=> lectorsDtoWrite.OrganizationsIds.Contains(c.Id)).ToListAsync();
        if (!organizations.Any()) return NotFound("Bad organization id(s)");
        var lector = new Lector()
        {
            FullName = lectorsDtoWrite.FullName,
            Phone = lectorsDtoWrite.Phone,
            Degree = lectorsDtoWrite.Degree,
            Organizations = organizations
        };
        _context.Lectors.Add(lector);
        await _context.SaveChangesAsync();

        return RedirectToAction("GetLector", new {id = lector.Id});
    }

    // DELETE: api/Lectors/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLector(int id)
    {
        var lector = await _context.Lectors.FindAsync(id);
        if (lector == null) return NotFound();

        _context.Lectors.Remove(lector);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool LectorExists(int id)
    {
        return (_context.Lectors?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}