using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTimeTable.Models;
using MyTimeTable.ModelsDTO;

namespace MyTimeTable.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupsController : ControllerBase
{
    private readonly MyTimeTableContext _context;

    public GroupsController(MyTimeTableContext context)
    {
        _context = context;
    }

    // GET: api/Groups
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupDtoRead>>> GetGroups()
    {
        var groups =  await _context.Groups.Include(d => d.Faculty).ToListAsync();
        var groupsDtoRead = new List<GroupDtoRead>();
        foreach (var group in groups)
        {
            groupsDtoRead.Add(
                new GroupDtoRead()
                {
                    Id = group.Id,
                    Name = group.Name,
                    Quantity = group.Quantity,
                    Course = group.Course,
                    FacultyId = group.FacultyId,
                    Faculty = group.Faculty.Name
                }
                );
        }

        return groupsDtoRead;
    }

    // GET: api/Groups/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GroupDtoRead>> GetGroup(int id)
    {
        var groups = await _context.Groups.Where(c=> c.Id == id)
            .Include(d=> d.Faculty).ToListAsync();

        if (!groups.Any()) return NotFound();
        var group = groups[0];
        var groupDtoRead = new GroupDtoRead()
        {
            Id = id,
            Name = group.Name,
            Quantity = group.Quantity,
            Course = group.Course,
            FacultyId = group.FacultyId,
            Faculty = group.Faculty.Name
        };
        return groupDtoRead;
    }

    // PUT: api/Groups/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGroup(int? id, GroupDtoWrite groupDtoWrite)
    {
        if (id is null) return BadRequest("No id");
        var group = await _context.Groups.FindAsync(id);
        if (group is null) return NotFound("Bad id");
        var faculty = await _context.Faculties.FindAsync(groupDtoWrite.FacultyId);
        if (faculty is null) return NotFound("Bad faculty id");
        var timeTables = await _context.TimeTables.Where(c =>
                groupDtoWrite.TimetablesIds != null && groupDtoWrite.TimetablesIds.Contains(c.Id))
            .ToListAsync();

        group.Name = groupDtoWrite.Name;
        group.FacultyId = groupDtoWrite.FacultyId;
        group.Faculty = faculty;
        group.Course = groupDtoWrite.Course;
        group.Quantity = groupDtoWrite.Quantity;
        group.TimeTables = timeTables;
        _context.Entry(group).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GroupExists(id.Value))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Groups
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Group>> PostGroup(GroupDtoWrite groupDtoWrite)
    {
        var faculty = await _context.Faculties.FindAsync(groupDtoWrite.FacultyId);
        if (faculty is null) return NotFound("Bad faculty id.");
        var group = new Group()
        {
            Name = groupDtoWrite.Name,
            FacultyId= groupDtoWrite.FacultyId,
            Course = groupDtoWrite.Course,
            Quantity = groupDtoWrite.Quantity,
            Faculty = faculty
        };
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();
        return RedirectToAction("GetGroup", new {id = group.Id});
    }

    // DELETE: api/Groups/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null) return NotFound();

        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool GroupExists(int id)
    {
        return (_context.Groups?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}