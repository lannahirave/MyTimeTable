using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTimeTable.Models;
using MyTimeTable.ModelsDTO;

namespace MyTimeTable.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeTableController : ControllerBase
    {
        private readonly MyTimeTableContext _context;

        public TimeTableController(MyTimeTableContext context)
        {
            _context = context;
        }

        // GET: api/TimeTable
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeTableDtoRead>>> GetTimeTables()
        {
            var timeTablesDtoRead = new List<TimeTableDtoRead>();
            var timetables =  await _context.TimeTables
                .Include(d => d.Subject)
                .Include(d => d.Group)
                .Include(d => d.Lector)
                .ToListAsync();
            foreach (var tt in timetables)
            {
                timeTablesDtoRead.Add(
                    new TimeTableDtoRead()
                    {
                        Auditory = tt.Auditory,
                        Day = tt.Day,
                        GroupId = tt.GroupId,
                        Group = tt.Group.Name,
                        Lection = tt.Lection,
                        LectorId = tt.LectorId,
                        Lector = tt.Lector.FullName,
                        SubjectId = tt.SubjectId,
                        Subject = tt.Subject.Name,
                        Id = tt.Id
                    });
            }
            return timeTablesDtoRead;
        }

        // GET: api/TimeTable/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeTableDtoRead>> GetTimeTable(int id)
        {
            var tt = await _context.TimeTables.Where(c => c.Id == id)
                .Include(d => d.Group)
                .Include(d => d.Subject)
                .Include(d => d.Lector).FirstOrDefaultAsync();
            
            if (tt is null)
            {
                return NotFound();
            }
            var timeTableDtoRead = 
                new TimeTableDtoRead()
                {
                    Auditory = tt.Auditory,
                    Day = tt.Day,
                    GroupId = tt.GroupId,
                    Group = tt.Group.Name,
                    Lection = tt.Lection,
                    LectorId = tt.LectorId,
                    Lector = tt.Lector.FullName,
                    SubjectId = tt.SubjectId,
                    Subject = tt.Subject.Name,
                    Id = tt.Id
                };


            return timeTableDtoRead;
        }

        // PUT: api/TimeTable/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimeTable(int? id, TimeTableDtoWrite timeTableDtoWrite)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var timeTable = await _context.TimeTables.FindAsync(id);
            if (timeTable is null) return NotFound("Bad id.");
            
            var group = await _context.Groups.FindAsync(timeTableDtoWrite.GroupId);
            if (group is null) return NotFound("Bad group id.");
            var subject = await _context.Subjects.FindAsync(timeTableDtoWrite.SubjectId);
            if (subject is null) return NotFound("Bad subject id.");
            var lector = await _context.Lectors.FindAsync(timeTableDtoWrite.LectorId);
            if (lector is null) return NotFound("Bad lector id.");

            timeTable.GroupId = timeTableDtoWrite.GroupId;
            timeTable.LectorId = timeTableDtoWrite.LectorId;
            timeTable.SubjectId = timeTableDtoWrite.SubjectId;
            timeTable.Group = group;
            timeTable.Lector = lector;
            timeTable.Subject = subject;
            timeTable.Auditory = timeTableDtoWrite.Auditory;
            timeTable.Day = timeTableDtoWrite.Day;
            timeTable.Lection = timeTableDtoWrite.Lection;
            _context.Entry(timeTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeTableExists(id.Value))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TimeTable
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TimeTable>> PostTimeTable(TimeTableDtoWrite timeTableDtoWrite)
        {
            var group = await _context.Groups.FindAsync(timeTableDtoWrite.GroupId);
            if (group is null) return NotFound("Bad group id.");
            var subject = await _context.Subjects.FindAsync(timeTableDtoWrite.SubjectId);
            if (subject is null) return NotFound("Bad subject id.");
            var lector = await _context.Lectors.FindAsync(timeTableDtoWrite.LectorId);
            if (lector is null) return NotFound("Bad lector id.");
            var timeTable = new TimeTable()
            {
                GroupId = timeTableDtoWrite.GroupId,
                LectorId = timeTableDtoWrite.LectorId,
                SubjectId = timeTableDtoWrite.SubjectId,
                Group = group,
                Lector =lector,
                Subject = subject,
                Auditory = timeTableDtoWrite.Auditory,
                Day = timeTableDtoWrite.Day,
                Lection = timeTableDtoWrite.Lection
            };
            //TODO if only there was a check of same aud fac and time error ^(
            _context.TimeTables.Add(timeTable);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetTimeTable", new { id = timeTable.Id });
        }

        // DELETE: api/TimeTable/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeTable(int id)
        {
            var timeTable = await _context.TimeTables.FindAsync(id);
            if (timeTable == null)
            {
                return NotFound();
            }

            _context.TimeTables.Remove(timeTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TimeTableExists(int id)
        {
            return (_context.TimeTables?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
