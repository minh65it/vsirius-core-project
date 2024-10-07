using Bogus;
using Moq;
using QgoldIntegrationDataApi._Repositories;
using QgoldIntegrationDataApi._Services.Services;
using QgoldIntegrationDataApi.Dtos.Qgold;
using QgoldIntegrationDataApi.Helpers.Ultilities;

namespace QgoldIntegrationDataApiTest
{
    public class UnitTest1
    {
        private readonly Mock<IRepositoryAccessor> _mockRepositoryAccessor;
        public UnitTest1()
        {
            _mockRepositoryAccessor = new Mock<IRepositoryAccessor>();
        }
        [Fact]
        public void GetData_Returns_Paggination()
        {
            List<QgoldCsvData_DTO> data = GenerateData(201);
            PaginationParam paginationParam = new PaginationParam()
            {
                 PageNumber = 1,
                 PageSize = 50,
            };

            var paginationUtility = PaginationUtility<QgoldCsvData_DTO>.Create(data, paginationParam.PageNumber, paginationParam.PageSize, true);

            Assert.Equal(paginationParam.PageSize, paginationUtility.Result.Count()); // Kiểm tra số lượng phần tử trả về
            Assert.Equal(paginationParam.PageNumber, paginationUtility.Pagination.PageNumber); // Kiểm tra trang hiện tại
            Assert.Equal(5, paginationUtility.Pagination.TotalPage); // Kiểm tra kích thước trang
            Assert.Equal(200, paginationUtility.Pagination.TotalCount); // Kiểm tra tổng số phần tử
        }
        private List<QgoldCsvData_DTO> GenerateData(int count)
        {
            var faker = new Faker<QgoldCsvData_DTO>()
            .RuleFor(c => c.Book, f => f.Commerce.ProductName())
            .RuleFor(c => c.AttributesText, f => f.Commerce.Department())
            .RuleFor(c => c.CategoriesText, f => f.Commerce.ProductAdjective());
            return faker.Generate(count);
        }
    }
}