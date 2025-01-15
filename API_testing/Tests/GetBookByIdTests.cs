using AventStack.ExtentReports;
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
            StartTest(nameof(GetValidBookById_ReturnsCorrectData));

            var book = new
            {
                Title = "Book By ID",
                Author = "Author",
                ISBN = "987654321",
                PublishedDate = System.DateTime.UtcNow.ToString("o")
            };

            try
            {
                Test.Log(Status.Info, "Creating a book to fetch by ID.");
                var createResponse = ApiClient.Post("/Books", book);
                Test.Log(Status.Info, $"Create Response: {createResponse.Content}");

                var createdBook = JsonConvert.DeserializeObject<dynamic>(createResponse.Content);
                string bookId = (string)createdBook.id;

                Test.Log(Status.Info, "Fetching the book by ID.");
                var response = ApiClient.Get($"/Books/{bookId}");
                Test.Log(Status.Info, $"Response: {response.Content}");

                Assert.Equal(200, (int)response.StatusCode);

                var fetchedBook = JsonConvert.DeserializeObject<dynamic>(response.Content);
                Assert.Equal(book.Title, (string)fetchedBook.title);
                Assert.Equal(book.Author, (string)fetchedBook.author);

                Test.Log(Status.Pass, "Book retrieved successfully by ID.");
            }
            catch (Exception ex)
            {
                Test.Log(Status.Fail, "Error during retrieving book by ID test: " + ex.Message);
                throw;
            }
        }

        [Fact]
        public void GetNonExistentBookById_Returns404()
        {
            StartTest(nameof(GetNonExistentBookById_Returns404));

            try
            {
                Test.Log(Status.Info, "Fetching a non-existent book by ID.");
                var response = ApiClient.Get("/Books/nonexistent-id");
                Test.Log(Status.Info, $"Response: {response.Content}");

                Assert.Equal(404, (int)response.StatusCode);

                Test.Log(Status.Pass, "Non-existent book fetch correctly returned 404.");
            }
            catch (Exception ex)
            {
                Test.Log(Status.Fail, "Error during non-existent book fetch test: " + ex.Message);
                throw;
            }
        }

        [Fact]
        public void GetBookByInvalidId_Returns400()
        {
            StartTest(nameof(GetBookByInvalidId_Returns400));

            try
            {
                Test.Log(Status.Info, "Fetching a book with an invalid ID.");
                var response = ApiClient.Get("/Books/invalid-id");
                Test.Log(Status.Info, $"Response: {response.Content}");

                Assert.Equal(400, (int)response.StatusCode);

                Test.Log(Status.Pass, "Invalid book ID fetch correctly returned 400.");
            }
            catch (Exception ex)
            {
                Test.Log(Status.Fail, "Error during invalid book ID fetch test: " + ex.Message);
                throw;
            }
        }
    }

}
