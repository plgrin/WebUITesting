using AventStack.ExtentReports;
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
            StartTest(nameof(UpdateValidBook_ReturnsUpdatedData));

            var book = new
            {
                Title = "Original Title",
                Author = "Original Author",
                ISBN = "123123123",
                PublishedDate = System.DateTime.UtcNow.ToString("o")
            };

            try
            {
                Test.Log(Status.Info, "Creating a book to update.");
                var createResponse = ApiClient.Post("/Books", book);
                Test.Log(Status.Info, $"Create Response: {createResponse.Content}");

                var createdBook = JsonConvert.DeserializeObject<dynamic>(createResponse.Content);
                string bookId = (string)createdBook.id;

                var updatedBook = new
                {
                    Title = "Updated Title",
                    Author = "Updated Author",
                    ISBN = "321321321",
                    PublishedDate = System.DateTime.UtcNow.ToString("o")
                };

                Test.Log(Status.Info, "Sending PUT request to update the book.");
                var updateResponse = ApiClient.Put($"/Books/{bookId}", updatedBook);
                Test.Log(Status.Info, $"Update Response: {updateResponse.Content}");

                Assert.Equal(204, (int)updateResponse.StatusCode);

                Test.Log(Status.Info, "Fetching the updated book by ID.");
                var getResponse = ApiClient.Get($"/Books/{bookId}");
                Test.Log(Status.Info, $"Get Response: {getResponse.Content}");

                var fetchedBook = JsonConvert.DeserializeObject<dynamic>(getResponse.Content);
                Assert.Equal(updatedBook.Title, (string)fetchedBook.title);
                Assert.Equal(updatedBook.Author, (string)fetchedBook.author);

                Test.Log(Status.Pass, "Book updated successfully.");
            }
            catch (Exception ex)
            {
                Test.Log(Status.Fail, "Error during book update test: " + ex.Message);
                throw;
            }
        }
    }

}
