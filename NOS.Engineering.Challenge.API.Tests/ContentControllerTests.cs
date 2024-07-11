using Microsoft.AspNetCore.Mvc;
using Moq;
using NOS.Engineering.Challenge.API.Controllers;
using NOS.Engineering.Challenge.API.Models;
using NOS.Engineering.Challenge.Managers;
using NOS.Engineering.Challenge.Models;

public class ContentControllerTests
{
        private readonly Mock<IContentsManager> _mockManager;
        private readonly ContentController _controller;

        public ContentControllerTests()
        {
            _mockManager = new Mock<IContentsManager>();
            _controller = new ContentController(_mockManager.Object);
        }

        [Fact]
        public async Task AddGenre_OkTest()
        {
            var id = Guid.NewGuid();
            var genresRequest = new List<string> { "Horror", "Comedy" };
            var content = new Content(id, "tit", "subt", "desc", "url", 132, DateTime.Now, DateTime.Now, genresRequest);

            _mockManager.Setup(mock => mock.AddGenres(id, genresRequest)).ReturnsAsync(content);

            var response = await _controller.AddGenres(id, genresRequest);

            var result = Assert.IsType<OkObjectResult>(response);
            var returned = Assert.IsType<Content>(result.Value);
            Assert.Equal(id, returned.Id);
        }

        [Fact]
        public async Task AddGenres_NotFoundTest()
        {
          var response = await _controller.AddGenres(Guid.NewGuid(), new List<string> { "Drama"});

            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task RemoveGenres_OkTest()
        {
            
            var id = Guid.NewGuid();
            var genresRequest = new List<string> { "Action" };
            var content = new Content(id, "t", "subt", "desc", "url", 156, DateTime.Now, DateTime.Now, new List<string>());

             _mockManager.Setup(m => m.RemoveGenre(id, genresRequest)).ReturnsAsync(content);

            var response = await _controller.RemoveGenres(id, genresRequest);

            var result = Assert.IsType<OkObjectResult>(response);
            var returned = Assert.IsType<Content>(result.Value);
            Assert.Equal(id, returned.Id);

        }

        [Fact]
        public async Task RemoveGenres_NotFoundTest()
        {
            
            var result = await _controller.RemoveGenres(Guid.NewGuid(), new List<string>());

            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        public async Task FilterContents_OKTest()
        {
            var titleFilter = "Title";
            var contents = new List<Content>() { 
              new Content(Guid.NewGuid(), "Title", "subtitle", "desc", "url", 
              182, DateTime.Now, DateTime.Now, new List<string>())};

            _mockManager.Setup(m => m.GetManyContents()).ReturnsAsync(contents);

            var response = await _controller.FilterContents(titleFilter);

            var result = Assert.IsType<OkObjectResult>(response);
            var returned = Assert.IsType<List<Content?>>(result.Value);
            Assert.Equal(titleFilter, returned[0].Title);
        }

        [Fact]
        public async Task FilterContents_NoFoundTest()
        {
            var result = await _controller.FilterContents("testTitle", "testGenre");

            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task GetContent_OkTeste()
        {
            var id = Guid.NewGuid();
            var content = new Content(id, "title", "subtitle", "desc", "url", 182, DateTime.Now, DateTime.Now, new List<string>());

            _mockManager.Setup(mock => mock.GetContent(id)).ReturnsAsync(content);

            var response = await _controller.GetContent(id);

            var result = Assert.IsType<OkObjectResult>(response);
            var returned = Assert.IsType<Content>(result.Value);
            Assert.Equal(id, returned.Id);
        }

        [Fact]
        public async Task GetContent_NotFoundTest()
        {
            var id = Guid.NewGuid();

            var response = await _controller.GetContent(id);

            Assert.IsType<NotFoundResult>(response);
        }

        
        [Fact]
        public async Task CreateContent_NotFoundTest()
        {
            var response = await _controller.CreateContent(new ContentInput{});

            var returned = Assert.IsType<ObjectResult>(response);
            Assert.Equal(500,returned.StatusCode);
        }

        [Fact]
        public async Task UpdateContent_NotFoundTest()
        {

            var result = await _controller.UpdateContent(Guid.NewGuid(), new ContentInput());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteContent_OkTest()
        {
            var id = Guid.NewGuid();

            _mockManager.Setup(m => m.DeleteContent(id)).ReturnsAsync(id);
            
            var response = await _controller.DeleteContent(id);

            var result = Assert.IsType<OkObjectResult>(response);
            var returned = Assert.IsType<Guid>(result.Value);
            Assert.Equal(id, returned);
        }

        

        [Fact]
        public async Task GetManyContents_NotFoundTest()
        {
            var response= await _controller.GetManyContents();

            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task GetManyContents_OkTest()
        {
            var contents = new List<Content>
            {
                new Content(Guid.NewGuid(), "title", "subtitle", "desc", "url", 182, DateTime.Now, DateTime.Now, new List<string>())
            };

            _mockManager.Setup(mock => mock.GetManyContents()).ReturnsAsync(contents);

            var response = await _controller.GetManyContents();

            var result = Assert.IsType<OkObjectResult>(response);
            var returned = Assert.IsType<List<Content>>(result.Value);
            Assert.NotEmpty(returned);
        }
}