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
            var response = ApiClient.Get("/Books");
            Assert.Equal(200, (int)response.StatusCode);

            var books = JsonConvert.DeserializeObject<List<dynamic>>(response.Content);
            Assert.NotEmpty(books);

            foreach (var book in books)
            {
                Assert.NotNull((string)book.title);
                Assert.NotNull((string)book.author);
                Assert.NotNull((string)book.publishedDate);
            }
        }
    }
}
