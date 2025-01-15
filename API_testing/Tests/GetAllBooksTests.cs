using AventStack.ExtentReports;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_testing.Tests
{
    public class GetAllBooksTests : BaseTest
    {
        [Fact]
        public void GetAllBooks_ReturnsListWithCorrectFields()
        {
            StartTest(nameof(GetAllBooks_ReturnsListWithCorrectFields));

            try
            {
                Test.Log(Status.Info, "Sending GET request to retrieve all books.");
                var response = ApiClient.Get("/Books");
                Test.Log(Status.Info, $"Response: {response.Content}");

                Assert.Equal(200, (int)response.StatusCode);

                var books = JsonConvert.DeserializeObject<List<dynamic>>(response.Content);
                Assert.NotEmpty(books);

                foreach (var book in books)
                {
                    Assert.NotNull((string)book.title);
                    Assert.NotNull((string)book.author);
                    Assert.NotNull((string)book.publishedDate);
                }

                Test.Log(Status.Pass, "All books retrieved successfully.");
            }
            catch (Exception ex)
            {
                Test.Log(Status.Fail, "Error during retrieving all books test: " + ex.Message);
                throw;
            }
        }
    }
}
