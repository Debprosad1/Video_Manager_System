using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Video_Manager.Data;
using Video_Manager.Models;

namespace WebApplicationApi.Controllers
{
    public class Video_ManagerController : Controller
    {
        [Route("api/[controller]")]
        [ApiController]
        public class ApiVideoFilesController : ControllerBase
        {
            private readonly Video_Manager.Data.ApplicationDbContext _context;

            public ApiVideoFilesController(ApplicationDbContext context)
            {
                _context = context;
            }

            // GET: api/ApiVideoFiles
            [HttpGet]
            public async Task<ActionResult<IEnumerable<VideoFile>>> GetVideoFiles()
            {
                return await _context.VideoFiles.ToListAsync();
            }

            // GET: api/ApiVideoFiles/5
            [HttpGet("{id}")]
            public async Task<ActionResult<VideoFile>> GetVideoFile(int id)
            {
                var videoFile = await _context.VideoFiles.FindAsync(id);

                if (videoFile == null)
                {
                    return NotFound();
                }

                return videoFile;
            }

            // POST: api/ApiVideoFiles
            [HttpPost]
            public async Task<ActionResult<VideoFile>> PostVideoFile(VideoFile videoFile)
            {
                _context.VideoFiles.Add(videoFile);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetVideoFile), new { id = videoFile.Id }, videoFile);
            }

            // PUT: api/ApiVideoFiles/5
            [HttpPut("{id}")]
            public async Task<IActionResult> PutVideoFile(int id, VideoFile videoFile)
            {
                if (id != videoFile.Id)
                {
                    return BadRequest();
                }

                _context.Entry(videoFile).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoFileExists(id))
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

            // DELETE: api/ApiVideoFiles/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteVideoFile(int id)
            {
                var videoFile = await _context.VideoFiles.FindAsync(id);
                if (videoFile == null)
                {
                    return NotFound();
                }

                _context.VideoFiles.Remove(videoFile);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            private bool VideoFileExists(int id)
            {
                return _context.VideoFiles.Any(e => e.Id == id);
            }
        }
    }
}
