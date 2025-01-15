using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_testing.Tests
{
    public class GetBookByIdTests : BaseTest
    {
        [Fact]
        public void GetValidBookById_ReturnsCorrectData()
        {
            var book = new
            {
                Title = "Book By ID",
                Author = "Author",
                ISBN = "987654321",
                PublishedDate = System.DateTime.UtcNow.ToString("o")
            };

            var createResponse = ApiClient.Post("/Books", book);
            var createdBook = JsonConvert.DeserializeObject<dynamic>(createResponse.Content);
            string bookId = (string)createdBook.id;

            var response = ApiClient.Get($"/Books/{bookId}");
            Assert.Equal(200, (int)response.StatusCode);

            var fetchedBook = JsonConvert.DeserializeObject<dynamic>(response.Content);
            Assert.Equal(book.Title, (string)fetchedBook.title);
            Assert.Equal(book.Author, (string)fetchedBook.author);
        }

        [Fact]
        public void GetNonExistentBookById_Returns404()
        {
            var response = ApiClient.Get("/Books/nonexistent-id");
            Assert.Equal(404, (int)response.StatusCode);
        }

        [Fact]
        public void GetBookByInvalidId_Returns400()
        {
            var response = ApiClient.Get("/Books/invalid-id");
            Assert.Equal(400, (int)response.StatusCode);
        }
    }

}
