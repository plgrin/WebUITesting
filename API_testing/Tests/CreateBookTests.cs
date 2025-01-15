using AventStack.ExtentReports;
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
            StartTest(nameof(CreateValidBook_Returns201AndMatchesInput));

            var book = new
            {
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "123456789",
                PublishedDate = System.DateTime.UtcNow.ToString("o")
            };

            try
            {
                Test.Log(Status.Info, "sending POST request to create a book.");
                var response = ApiClient.Post("/Books", book);
                Test.Log(Status.Info, $"response: {response.Content}");

                Assert.Equal(201, (int)response.StatusCode);

                var responseData = JsonConvert.DeserializeObject<dynamic>(response.Content);
                Assert.Equal(book.Title, (string)responseData.title);
                Assert.Equal(book.Author, (string)responseData.author);
                Assert.Equal(book.ISBN, (string)responseData.isbn);

                Test.Log(Status.Pass, "book was created.");
            }
            catch (Exception ex)
            {
                Test.Log(Status.Fail, "error: " + ex.Message);
                throw;
            }
        }

        [Fact]
        public void Test_Authentication_With_Token()
        {
            try
            {
                Test.Log(Status.Info, "Sending GET request to verify authentication.");
                var response = ApiClient.Get("/Books");
                Test.Log(Status.Info, $"Response: {response.Content}");

                Assert.Equal(200, (int)response.StatusCode);
                Assert.NotEmpty(response.Content);

                Test.Log(Status.Pass, "Authentication verified successfully.");
            }
            catch (Exception ex)
            {
                Test.Log(Status.Fail, "Error during authentication test: " + ex.Message);
                throw;
            }
        }

        [Fact]
        public void CreateDuplicateBook_ReturnsConflict()
        {
            StartTest(nameof(CreateDuplicateBook_ReturnsConflict));

            var book = new
            {
                Title = "Duplicate Book",
                Author = "Author",
                ISBN = "123456789",
                PublishedDate = System.DateTime.UtcNow.ToString("o")
            };

            try
            {
                Test.Log(Status.Info, "Creating a book for the first time.");
                ApiClient.Post("/Books", book);

                Test.Log(Status.Info, "Attempting to create a duplicate book.");
                var response = ApiClient.Post("/Books", book);
                Test.Log(Status.Info, $"Response: {response.Content}");

                Assert.Equal(409, (int)response.StatusCode);

                Test.Log(Status.Pass, "Duplicate book creation correctly returned conflict.");
            }
            catch (Exception ex)
            {
                Test.Log(Status.Fail, "Error during duplicate book creation test: " + ex.Message);
                throw;
            }
        }
    }
}
