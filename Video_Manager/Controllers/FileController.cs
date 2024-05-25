using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Video_Manager.Data;
using Video_Manager.Models;


public class VideoFilesController : Controller
{
    private readonly ApplicationDbContext _context;

    public VideoFilesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: VideoFiles/Index
    public async Task<IActionResult> Index(int productId)
    {
        var videoFiles = await _context.VideoFiles.ToListAsync();
        return View(videoFiles);
    }

    // GET: VideoFiles/Upload
    public IActionResult Upload(int productId)
    {
        ViewBag.ProductId = productId;
        return View();
    }

    // POST: VideoFiles/Upload Upload
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(int productId, IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var videoFile = new VideoFile
            {
                FileName = file.FileName,
                FilePath = filePath,
            };

            _context.VideoFiles.Add(videoFile);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { productId });
        }

        ViewBag.ProductId = productId;
        return View();
    }


    // GET: VideoFiles/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var videoFile = await _context.VideoFiles.FindAsync(id);
        if (videoFile == null)
        {
            return NotFound();
        }

        return View(videoFile);
    }

    // POST: VideoFiles/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, VideoFile videoFile)
    {
        if (id != videoFile.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(videoFile);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoFileExists(videoFile.Id))
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
        return View(videoFile);
    }

    private bool VideoFileExists(int id)
    {
        return _context.VideoFiles.Any(e => e.Id == id);
    }


    // GET: VideoFiles/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var videoFile = await _context.VideoFiles.FindAsync(id);
        if (videoFile == null)
        {
            return NotFound();
        }

        return View(videoFile);
    }

    // POST: VideoFiles/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var videoFile = await _context.VideoFiles.FindAsync(id);
        _context.VideoFiles.Remove(videoFile);
        await _context.SaveChangesAsync();

        if (System.IO.File.Exists(videoFile.FilePath))
        {
            System.IO.File.Delete(videoFile.FilePath);
        }

        return RedirectToAction(nameof(Index));
    }
}
