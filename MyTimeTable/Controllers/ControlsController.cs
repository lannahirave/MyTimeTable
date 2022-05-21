using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTimeTable.Models;

namespace MyTimeTable.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ControlsController : ControllerBase
{
    private readonly MyTimeTableContext _context;

    public ControlsController(MyTimeTableContext context)
    {
        _context = context;
    }

    // GET: api/Controls
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Control>>> GetControls()
    {
        return await _context.Controls.OrderBy(c => c.Id).ToListAsync();
    }

    // GET: api/Controls/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Control>> GetControl([FromRoute] int id)
    {
        var control = await _context.Controls.FindAsync(id);

        if (control == null) return NotFound();

        return control;
    }

    // PUT: api/Controls/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutControl(int id, Control control)
    {
        if (id != control.Id) return BadRequest("No id.");
        var controlCheck = await _context.Controls.CountAsync(c => c.Type == control.Type);
        if (controlCheck > 0) return BadRequest();
        _context.Entry(control).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ControlExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Controls
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Control>> PostControl(Control control)
    {
        var controlCheck = await _context.Controls.CountAsync(c => c.Type == control.Type);
        if (controlCheck > 0) return BadRequest("This control already exists.");
        _context.Controls.Add(control);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetControl", new {id = control.Id}, control);
    }

    // DELETE: api/Controls/5
    [HttpDelete("{type}")]
    public async Task<IActionResult> DeleteControl([FromRoute] string? type)
    {
        if (type is null) return BadRequest("No type specified.");
        var controls = await _context.Controls.Where(c => c.Type == type).ToListAsync();
        if (!controls.Any()) return BadRequest("Not found.");

        _context.Controls.Remove(controls[0]);
        await _context.SaveChangesAsync();

        return Accepted(value: "Success.");
    }

    private bool ControlExists(int id)
    {
        return (_context.Controls?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}