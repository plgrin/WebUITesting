using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_testing.Tests
{
    public class UpdateBookTests : BaseTest
    {
        [Fact]
        public void UpdateValidBook_ReturnsUpdatedData()
        {
            var book = new
            {
                Title = "Original Title",
                Author = "Original Author",
                ISBN = "123123123",
                PublishedDate = System.DateTime.UtcNow.ToString("o")
            };

            var createResponse = ApiClient.Post("/Books", book);
            var createdBook = JsonConvert.DeserializeObject<dynamic>(createResponse.Content);
            string bookId = (string)createdBook.id;

            var updatedBook = new
            {
                Title = "Updated Title",
                Author = "Updated Author",
                ISBN = "321321321",
                PublishedDate = System.DateTime.UtcNow.ToString("o")
            };

            var updateResponse = ApiClient.Put($"/Books/{bookId}", updatedBook);
            Assert.Equal(204, (int)updateResponse.StatusCode);

            var getResponse = ApiClient.Get($"/Books/{bookId}");
            var fetchedBook = JsonConvert.DeserializeObject<dynamic>(getResponse.Content);
            Assert.Equal(updatedBook.Title, (string)fetchedBook.title);
            Assert.Equal(updatedBook.Author, (string)fetchedBook.author);
        }
    }

}
