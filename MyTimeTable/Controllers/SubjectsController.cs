using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTimeTable.Models;
using MyTimeTable.ModelsDTO;

namespace MyTimeTable.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubjectsController : ControllerBase
{
    private readonly MyTimeTableContext _context;

    public SubjectsController(MyTimeTableContext context)
    {
        _context = context;
    }

    // GET: api/Subjects
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjects()
    {
        var subjectsDto = new List<SubjectDto>();
        var subjects = await _context.Subjects.ToListAsync();
        foreach (var subject in subjects)
        {
            var control = await _context.Controls.FindAsync(subject.ControlId);
            subjectsDto.Add(new SubjectDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Type = subject.Type,
                Hours = subject.Hours,
                ControlId = subject.ControlId,
                ControlType = control!.Type
            });
        }

        return subjectsDto;
    }

    // GET: api/Subjects/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SubjectDto>> GetSubject([FromRoute] int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null) return NotFound();
        var control = await _context.Controls.FindAsync(subject.ControlId);
        var subjectDto = new SubjectDto
        {
            Id = subject.Id,
            Name = subject.Name,
            Type = subject.Type,
            Hours = subject.Hours,
            ControlId = subject.ControlId,
            ControlType = control!.Type
        };
        return subjectDto;
    }

    // PUT: api/Subjects/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSubject([FromRoute] int? id, SubjectDtoWrite subjectDtoWrite)
    {
        if (id is null) return BadRequest("No id");
        var control = await _context.Controls.Where(e => e.Id == subjectDtoWrite.ControlId).ToListAsync();
        if (control.Count == 0) return BadRequest("Bad ControlId");
        var subject = await _context.Subjects.FindAsync(id);
        if (subject is null) return BadRequest("Bad id");
        subject.Name = subjectDtoWrite.Name;
        subject.Type = subjectDtoWrite.Type;
        subject.Hours = subjectDtoWrite.Hours;
        subject.ControlId = subjectDtoWrite.ControlId;
        subject.Control = control.First();

        _context.Entry(subject).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SubjectExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Subjects
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<SubjectDto>> PostSubject(SubjectDto subjectDto)
    {
        var control = await _context.Controls.FindAsync(subjectDto.ControlId);
        if (control is null) return BadRequest();
        var subject = new Subject
        {
            Name = subjectDto.Name,
            Hours = subjectDto.Hours,
            Type = subjectDto.Type,
            ControlId = subjectDto.ControlId,
            Control = control
        };

        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();

        return RedirectToAction("GetSubject", new {id = subject.Id});
    }

    // DELETE: api/Subjects/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject([FromRoute] int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null) return NotFound();

        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool SubjectExists(int? id)
    {
        return (_context.Subjects?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}