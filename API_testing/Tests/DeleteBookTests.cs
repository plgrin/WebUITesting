using AventStack.ExtentReports;
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
            StartTest(nameof(DeleteValidBook_Returns204AndCannotRetrieve));

            var book = new
            {
                Title = "Book To Delete",
                Author = "Author",
                ISBN = "789789789",
                PublishedDate = System.DateTime.UtcNow.ToString("o")
            };

            try
            {
                Test.Log(Status.Info, "Creating a book to delete.");
                var createResponse = ApiClient.Post("/Books", book);
                Test.Log(Status.Info, $"Create Response: {createResponse.Content}");

                var createdBook = JsonConvert.DeserializeObject<dynamic>(createResponse.Content);
                string bookId = (string)createdBook.id;

                Test.Log(Status.Info, "Sending DELETE request.");
                var deleteResponse = ApiClient.Delete($"/Books/{bookId}");
                Test.Log(Status.Info, $"Delete Response: {deleteResponse.Content}");

                Assert.Equal(204, (int)deleteResponse.StatusCode);

                Test.Log(Status.Info, "Verifying the book is no longer retrievable.");
                var getResponse = ApiClient.Get($"/Books/{bookId}");
                Test.Log(Status.Info, $"Get Response: {getResponse.Content}");

                Assert.Equal(404, (int)getResponse.StatusCode);

                Test.Log(Status.Pass, "Book deleted successfully.");
            }
            catch (Exception ex)
            {
                Test.Log(Status.Fail, "Error during book deletion test: " + ex.Message);
                throw;
            }
        }

        [Fact]
        public void DeleteNonExistentBook_Returns404()
        {
            StartTest(nameof(DeleteNonExistentBook_Returns404));

            try
            {
                Test.Log(Status.Info, "Attempting to delete a non-existent book.");
                var response = ApiClient.Delete("/Books/nonexistent-id");
                Test.Log(Status.Info, $"Response: {response.Content}");

                Assert.Equal(404, (int)response.StatusCode);

                Test.Log(Status.Pass, "Non-existent book deletion correctly returned 404.");
            }
            catch (Exception ex)
            {
                Test.Log(Status.Fail, "Error during non-existent book deletion test: " + ex.Message);
                throw;
            }
        }

        [Fact]
        public void DeleteBookByInvalidId_Returns400()
        {
            StartTest(nameof(DeleteBookByInvalidId_Returns400));

            try
            {
                Test.Log(Status.Info, "Attempting to delete a book with an invalid ID.");
                var response = ApiClient.Delete("/Books/invalid-id");
                Test.Log(Status.Info, $"Response: {response.Content}");

                Assert.Equal(400, (int)response.StatusCode);

                Test.Log(Status.Pass, "Invalid book ID deletion correctly returned 400.");
            }
            catch (Exception ex)
            {
                Test.Log(Status.Fail, "Error during invalid book ID deletion test: " + ex.Message);
                throw;
            }
        }
    }
}
