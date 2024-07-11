using NOS.Engineering.Challenge.Database;
using NOS.Engineering.Challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace NOS.Engineering.Challenge.Managers;


public class ContentsManager : IContentsManager
{
    private readonly IDatabase<Content?, ContentDto> _database;

    private readonly ILogger<ContentsManager> _logger;
    private readonly DataContext _dataContext;

    public ContentsManager(IDatabase<Content?, ContentDto> database, 
    ILogger<ContentsManager> logger, DataContext dataContext)
    {
        _database = database;
        _logger = logger;
        _dataContext = dataContext;
    }

    public Task<IEnumerable<Content?>> GetManyContents()
    {
        //to use the Sql server db: 
         //  _dataContext.Content.ToListAsync();
        return _database.ReadAll();
    }

    public Task<Content?> CreateContent(ContentDto content)
    {
        //to use the Sql server db: 
        //_dataContext.Content.Add();
        //_dataContext.SaveChangesAsync();
        return _database.Create(content);
    }

    public Task<Content?> GetContent(Guid id)
    {
        //to use the Sql server db: 
        //_dataContext.Content.FindAsync(id);
        return _database.Read(id);
    }

    public Task<Content?> UpdateContent(Guid id, ContentDto content)
    {
        //to use the Sql server db: 
        //_dataContext.Content.FindAsync(id);
        //update field
        //_dataContext.SaveChangesAsync();
        return _database.Update(id, content);
    }

    public Task<Guid> DeleteContent(Guid id)
    {
        //to use the Sql server db: 
        //var content = _dataContext.Content.FindAsync(id);
        //_dataContext.Content.Remove(content);
        //_dataContext.SaveChangesAsync();
        return _database.Delete(id);
    }

    public async Task<Content?> AddGenres(Guid id, IEnumerable<string> genres)
    {
        var contentFromDB = await _database.Read(id);
        if(contentFromDB == null)
        {
            _logger.LogWarning("No content found with ID: {id}.", id);
            return null;
        }
        var newGenres = genres.Except(contentFromDB.GenreList).ToList();
            if (!newGenres.Any())
            {
                _logger.LogWarning("Theres no new genres to add in content with ID: {id}.", id);
                return null;
            } 

            
        var newGenreList = contentFromDB.GenreList.ToList();

        genres.ToList().ForEach( gen => {
            if(!newGenreList.Contains(gen))
                newGenreList.Add(gen);    
        });

        var content = new ContentDto(null,null,null,null,null,null,null,newGenreList);
        return await _database.Update(id, content);
    }

    public async Task<Content?> RemoveGenre(Guid id, IEnumerable<string> genres)
    {
        var contentFromDB = await _database.Read(id);

        if (contentFromDB == null)
        {
           _logger.LogWarning("No content found with ID: {id}.", id);
           return null;
        } 

        var genresToRemove = genres.Intersect(contentFromDB.GenreList).ToList();

        if (!genresToRemove.Any())
        {
            _logger.LogWarning("The genres send in  the request doesnt exist in the content with id {id}.",id);
            return null;
        }


        var content = new ContentDto(null, null, null,null, null, null,null, genresToRemove);

        return await _database.Update(id, content);
    }
}