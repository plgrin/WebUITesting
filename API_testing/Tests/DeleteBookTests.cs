using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_testing.Tests
{
    public class DeleteBookTests : BaseTest
    {
        [Fact]
        public void DeleteValidBook_Returns204AndCannotRetrieve()
        {
            var book = new
            {
                Title = "Book To Delete",
                Author = "Author",
                ISBN = "789789789",
                PublishedDate = System.DateTime.UtcNow.ToString("o")
            };

            var createResponse = ApiClient.Post("/Books", book);
            var createdBook = JsonConvert.DeserializeObject<dynamic>(createResponse.Content);
            string bookId = (string)createdBook.id;

            var deleteResponse = ApiClient.Delete($"/Books/{bookId}");
            Assert.Equal(204, (int)deleteResponse.StatusCode);

            var getResponse = ApiClient.Get($"/Books/{bookId}");
            Assert.Equal(404, (int)getResponse.StatusCode);
        }

        [Fact]
        public void DeleteNonExistentBook_Returns404()
        {
            var response = ApiClient.Delete("/Books/nonexistent-id");
            Assert.Equal(404, (int)response.StatusCode);
        }

        [Fact]
        public void DeleteBookByInvalidId_Returns400()
        {
            var response = ApiClient.Delete("/Books/invalid-id");
            Assert.Equal(400, (int)response.StatusCode);
        }
    }
}
