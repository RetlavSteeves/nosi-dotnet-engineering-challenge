using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NOS.Engineering.Challenge.API.Models;
using NOS.Engineering.Challenge.Managers;

namespace NOS.Engineering.Challenge.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[OutputCache]
public class ContentController : Controller
{
    private readonly IContentsManager _manager;

    public ContentController(IContentsManager manager)
    {
        _manager = manager;
    }
    
    [Obsolete("Endpoint obsolete, instead use the new endpoint /api/v1/Content/Filter")]
    [HttpGet]   
    public async Task<IActionResult> GetManyContents()
    {
        var contents = await _manager.GetManyContents().ConfigureAwait(false);
        if (!contents.Any())
            return NotFound();
        
        return Ok(contents);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContent(Guid id)
    {
        var content = await _manager.GetContent(id).ConfigureAwait(false);

        if (content == null)
            return NotFound();
        
        return Ok(content);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateContent(
        [FromBody] ContentInput content
        )
    {
        var createdContent = await _manager.CreateContent(content.ToDto()).ConfigureAwait(false);

        return createdContent == null ? Problem() : Ok(createdContent);
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateContent(
        Guid id,
        [FromBody] ContentInput content
        )
    {
        var updatedContent = await _manager.UpdateContent(id, content.ToDto()).ConfigureAwait(false);

        return updatedContent == null ? NotFound() : Ok(updatedContent);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContent(
        Guid id
    )
    {
        var deletedId = await _manager.DeleteContent(id).ConfigureAwait(false);
        return Ok(deletedId);
    }
    
    [HttpPost("{id}/genre")]
    public async Task<IActionResult> AddGenres(
        Guid id,
        [FromBody] IEnumerable<string> genres
    )
    {
        var updatedContentWithGenres = await _manager.AddGenres(id, genres).ConfigureAwait(false);

        return updatedContentWithGenres == null ? NotFound() : Ok(updatedContentWithGenres);

    }
    
    [HttpDelete("{id}/genre")]
    public  async Task<IActionResult> RemoveGenres(
        Guid id,
        [FromBody] IEnumerable<string> genres
    )
    { 
        var updatedContentWithouGenresRemoved = await _manager.RemoveGenre(id, genres).ConfigureAwait(false);

        return updatedContentWithouGenresRemoved == null ? NotFound() : Ok(updatedContentWithouGenresRemoved);
    }

    [HttpGet("Filter")]
    public async Task<IActionResult> FilterContents([FromQuery] string title = "", string genre = "")
    {
        var contents = await _manager.GetManyContents().ConfigureAwait(false);

        if (contents == null || !contents.Any())
            return NotFound();

        if (!string.IsNullOrWhiteSpace(title))
            contents = contents.Where(content => content.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(genre))
            contents = contents.Where(content => content.GenreList.Any(g => g.Contains(genre, StringComparison.OrdinalIgnoreCase)));

        return contents.Any() ? Ok(contents.ToList()) : 
            NotFound("No content found with this filter(s)."); 
                                                         
    }
}