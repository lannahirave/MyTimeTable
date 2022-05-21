using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTimeTable.Models;
using MyTimeTable.ModelsDTO;

namespace MyTimeTable.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FacultiesController : ControllerBase
{
    private readonly MyTimeTableContext _context;

    public FacultiesController(MyTimeTableContext context)
    {
        _context = context;
    }

    // GET: api/Faculties
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FacultyDtoRead>>> GetFaculties()
    {
        var facultiesDtoReads = new List<FacultyDtoRead>();
        var faculties = await _context.Faculties
            .Include(d => d.Organization)
            .Include(d => d.Groups)
            .ToListAsync();
        foreach (var faculty in faculties)
        {
            var groups = await _context.Groups.Where(c => c.FacultyId == faculty.Id)
                .Select(d => d.Name)
                .ToListAsync();

            facultiesDtoReads.Add(
                new FacultyDtoRead
                {
                    Id = faculty.Id,
                    Name = faculty.Name,
                    OrganizationId = faculty.OrganizationId,
                    Organization = faculty.Organization.Name,
                    Groups = groups
                }
            );
        }

        return facultiesDtoReads;
    }

    // GET: api/Faculties/5
    [HttpGet("{id}")]
    public async Task<ActionResult<FacultyDtoRead>> GetFaculty(int id)
    {
        var faculty0 = await _context.Faculties
            .Where(c => c.Id == id)
            .Include(d => d.Organization)
            .Include(d => d.Groups).ToListAsync();

        if (!faculty0.Any()) return NotFound();
        var faculty = faculty0[0];
        var groups = await _context.Groups.Where(c => c.FacultyId == faculty.Id)
            .Select(d => d.Name)
            .ToListAsync();

        var facultyDtoRead = new FacultyDtoRead
        {
            Id = faculty.Id,
            Name = faculty.Name,
            OrganizationId = faculty.OrganizationId,
            Organization = faculty.Organization.Name,
            Groups = groups
        };

        return facultyDtoRead;
    }

    // PUT: api/Faculties/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFaculty([FromRoute] int? id, FacultyDtoWrite facultyDtoWrite)
    {
        if (id is null) return BadRequest("No id");
        var faculty = await _context.Faculties.FindAsync(id);
        if (faculty is null) return NotFound("Bad id");
        var organization = await _context.Organizations.FindAsync(facultyDtoWrite.OrganizationId);
        if (organization is null) return NotFound("Bad organization id");
        var groups = await _context.Groups.Where(c =>
                facultyDtoWrite.GroupsIds != null && facultyDtoWrite.GroupsIds.Contains(c.Id))
            .ToListAsync();

        faculty.Name = facultyDtoWrite.Name;
        faculty.OrganizationId = facultyDtoWrite.OrganizationId;
        faculty.Organization = organization;
        faculty.Groups = groups;

        _context.Entry(faculty).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FacultyExists(id.Value))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Faculties
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<FacultyDtoWrite>> PostFaculty(FacultyDtoWrite facultyDtoWrite)
    {
        var organization = await _context.Organizations.FindAsync(facultyDtoWrite.OrganizationId);
        if (organization is null) return NotFound("Bad organization id.");
        var faculty = new Faculty
        {
            Name = facultyDtoWrite.Name,
            OrganizationId = facultyDtoWrite.OrganizationId,
            Organization = organization
        };
        _context.Faculties.Add(faculty);
        await _context.SaveChangesAsync();

        return RedirectToAction("GetFaculty", new {id = faculty.Id});
    }

    // DELETE: api/Faculties/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFaculty(int id)
    {
        var faculty = await _context.Faculties.FindAsync(id);
        if (faculty == null) return NotFound();

        _context.Faculties.Remove(faculty);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool FacultyExists(int id)
    {
        return (_context.Faculties?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}