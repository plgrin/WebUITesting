using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_testing.Tests
{
    public class CreateBookTests : BaseTest
    {
        [Fact]
        public void CreateValidBook_Returns201AndMatchesInput()
        {
            var book = new
            {
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "123456789",
                PublishedDate = System.DateTime.UtcNow.ToString("o")
            };
            
            var response = ApiClient.Post("/Books", book);
            Assert.Equal(201, (int)response.StatusCode);

            var responseData = JsonConvert.DeserializeObject<dynamic>(response.Content);
            Assert.Equal(book.Title, (string)responseData.title);
            Assert.Equal(book.Author, (string)responseData.author);
            Assert.Equal(book.ISBN, (string)responseData.isbn);
        }

        [Fact]
        public void Test_Authentication_With_Token()
        {
            var response = ApiClient.Get("/Books");
            Assert.Equal(200, (int)response.StatusCode);
            Assert.NotEmpty(response.Content); // Проверяем, что контент не пустой
        }

        [Fact]
        public void CreateDuplicateBook_ReturnsConflict()
        {
            var book = new
            {
                Title = "Duplicate Book",
                Author = "Author",
                ISBN = "123456789",
                PublishedDate = System.DateTime.UtcNow.ToString("o")
            };

            ApiClient.Post("/Books", book); // First creation
            var response = ApiClient.Post("/Books", book); // Duplicate

            Assert.Equal(409, (int)response.StatusCode);
        }
    }
}
