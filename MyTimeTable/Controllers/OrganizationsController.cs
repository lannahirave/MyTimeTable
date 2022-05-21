using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTimeTable.Models;
using MyTimeTable.ModelsDTO;
namespace MyTimeTable.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrganizationsController : ControllerBase
{
    private readonly MyTimeTableContext _context;

    public OrganizationsController(MyTimeTableContext context)
    {
        _context = context;
    }

    // GET: api/Organizations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrganizationDtoRead>>> GetOrganizations()
    {
        var organizations = await _context.Organizations.OrderBy(c => c.Id).ToListAsync();
        var organizationsDtoRead = new List<OrganizationDtoRead>();
        foreach (var org in organizations)
        {
            var faculty = await _context.Faculties.Where(c => c.OrganizationId == org.Id).ToListAsync();
            organizationsDtoRead.Add(
                new OrganizationDtoRead
                {
                    Id = org.Id,
                    Name = org.Name,
                    Faculties = faculty.Select(c => c.Name).ToList()
                }
            );
        }
        return organizationsDtoRead;
    }

    // GET: api/Organizations/5
    [HttpGet("{id}")]
    public async Task<ActionResult<OrganizationDtoRead>> GetOrganization(int id)
    {
        var organization = await _context.Organizations.Where(c => c.Id == id).Include(d => d.Faculties).ToListAsync();

        if (!organization.Any()) return NotFound();
        var organizationDtoRead = new OrganizationDtoRead
        {
            Id = id,
            Name = organization[0].Name,
            Faculties = organization[0].Faculties.Select(d => d.Name).ToList()
        };
        return organizationDtoRead;
    }

    // PUT: api/Organizations/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutOrganization([FromRoute] int? id, OrganizationDto organizationDto)
    {
        if (id is null) return BadRequest("No id");
        var organization = await _context.Organizations.FindAsync(id);
        if (organization is null) return NotFound("Bad id");
        var faculties = await _context.Faculties.Where(c =>
                organizationDto.FacultiesIds != null && organizationDto.FacultiesIds.Contains(c.Id))
            .ToListAsync();

        organization.Name = organizationDto.Name;
        organization.Faculties = faculties;

        _context.Entry(organization).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OrganizationExists(id.Value))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Organizations
    [HttpPost]
    public async Task<ActionResult<OrganizationDto>> PostOrganization(OrganizationDto organizationDto)
    {
        var organization = new Organization
        {
            Name = organizationDto.Name
        };

        _context.Organizations.Add(organization);
        await _context.SaveChangesAsync();

        return RedirectToAction("GetOrganization", new {id = organization.Id});
    }

    // DELETE: api/Organizations/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrganization(int id)
    {
        var organization = await _context.Organizations.FindAsync(id);
        if (organization == null) return NotFound("No id");

        _context.Organizations.Remove(organization);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool OrganizationExists(int id)
    {
        return (_context.Organizations?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}