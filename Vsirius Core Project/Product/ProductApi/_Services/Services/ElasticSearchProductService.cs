using AutoMapper;
using Nest;
using Newtonsoft.Json;
using ProductApi._Services.Interfaces;
using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;
using VsiriusDataModel.DataModel;

namespace ProductApi._Services.Services
{
    public class ElasticSearchProductService : IElasticSearchProductService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly string _table;
        public ElasticSearchProductService(
            IMapper mapper,
            IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
            _table = _configuration["ElasticConnectString:Table"]!;
            client = new ElasticClient(
            new ConnectionSettings(
            new Uri(_configuration["ElasticConnectString:API"]!))
            .DefaultIndex(_table)
            );
        }

        public ElasticClient client { get; set; }

        public async Task BulkInsertProductToElastic(List<VsiriusProduct> elasticProducts)
        {
            var indexExistsResponse = await client.Indices.ExistsAsync(_table);
            if (!indexExistsResponse.Exists)
            {
                var createIndexResponse = await client.Indices.CreateAsync(_table, c => c
                    .Map<VsiriusProduct>(m => m
                        .AutoMap()
                    )
                );
            }
            var updateSettingsResponse = await client.Indices.UpdateSettingsAsync(_table, s => s
                    .IndexSettings(i => i
                        .Setting("index.mapping.total_fields.limit", 1000)
                    )
                );

            var indexResponse = await client.IndexManyAsync(elasticProducts);
        }


        public async Task DeleteAllProductInElastic()
        {
            var deleteIndexResponse = await client.Indices.DeleteAsync(_table);
            //var deleteByQueryResponse = await client.DeleteByQueryAsync<VsiriusProduct>(q => q
            //.Query(rq => rq.MatchAll())
            //);
        }

        public async Task<PaginationUtility<VsiriusProductDto>> GetAllBackOffice(
            List<VsiriusFilterParamDto>? filterParam,
            PaginationParam pagination,
            SearchProductParam searchParam)
        {
            string fullTextSearch = searchParam?.Keyword == null ? string.Empty : searchParam?.Keyword.Trim().ToLower()!;
            var results = new List<string>();
            VsiriusProductDto dto = new();
            var propertiesValid = dto.GetType().GetProperties();

            var client = new ElasticClient(new Uri(_configuration["ElasticConnectString:API"]!));
            var searchDescriptor = new SearchDescriptor<VsiriusProduct>();

            var mustQueries = new List<QueryContainer>
            {
                new TermQuery { Field = "isDelete", Value = false }
            };

            if (filterParam != null && filterParam.Count != 0)
            {
                foreach (var item in filterParam.Where(x => x.Value.Count > 0))
                {
                    foreach (var property in propertiesValid)
                    {
                        if (property.Name == item.FieldName)
                        {
                            var concatString = string.Join(" ", item.Value);
                            mustQueries.Add(new QueryStringQuery()
                            {
                                Name = property.Name,
                                Query = concatString,
                                Fields = new[] { property.Name }
                            });
                        }
                    }
                }
            }

            if (searchParam?.IncludeNoStock != true)
            {
                mustQueries.Add(new NumericRangeQuery
                {
                    Field = "quantity",
                    GreaterThan = 0
                });
            }

            if (searchParam != null && (searchParam.MaxPrice != null || searchParam.MinPrice != null))
            {
                if (searchParam.MaxPrice != null)
                {
                    mustQueries.Add(new NumericRangeQuery
                    {
                        Field = "standard_price",
                        LessThanOrEqualTo = (double)searchParam.MaxPrice + 0.01
                    });
                }

                if (searchParam.MinPrice != null)
                {
                    mustQueries.Add(new NumericRangeQuery
                    {
                        Field = "standard_price",
                        GreaterThanOrEqualTo = (double)searchParam.MinPrice - 0.01
                    });
                }
            }

            if (!string.IsNullOrEmpty(fullTextSearch))
            {
                mustQueries.Add(new MultiMatchQuery
                {
                    Fields = new[] { "item_name", "item_sku", "product_description" },
                    Query = fullTextSearch,
                    Fuzziness = Fuzziness.Auto
                });
            }

            searchDescriptor.Query(q => q.Bool(b => b.Must(mustQueries.ToArray())))
                .From((pagination.PageNumber - 1) * pagination.PageSize)
                .Size(pagination.PageSize);

            var searchResponse = await client.SearchAsync<VsiriusProduct>(searchDescriptor.Index(_table));
            var data = searchResponse.Documents.ToList();
            return new PaginationUtility<VsiriusProductDto>(
                new PaginationUtility<VsiriusProductDto>.PaginationResult(
                    (int)searchResponse.Total,
                    pagination.PageNumber,
                    pagination.PageSize,
                    pagination.PageSize * pagination.PageNumber),
                _mapper.Map<List<VsiriusProductDto>>(data)
            );
        }

        public async Task<PaginationUtility<VsiriusProductSearchDto>> GetListSkuFromTextSearch(
            List<VsiriusFilterParamDto>? filterParam,
            PaginationParam pagination,
            SearchProductParam searchParam)
        {
            string fullTextSearch = searchParam?.Keyword == null ? string.Empty : searchParam?.Keyword.Trim().ToLower()!;
            var results = new List<string>();
            VsiriusProductDto dto = new();
            var propertiesValid = dto.GetType().GetProperties();

            var client = new ElasticClient(new Uri(_configuration["ElasticConnectString:API"]!));
            var pingResponse = client.Ping();
            var searchDescriptor = new SearchDescriptor<VsiriusProduct>();

            var mustQueries = new List<QueryContainer>
            {
                new TermQuery { Field = "isDelete", Value = false }
            };

            if (filterParam != null && filterParam.Count != 0)
            {
                foreach (var item in filterParam.Where(x => x.Value.Count > 0))
                {
                    foreach (var property in propertiesValid)
                    {
                        if (property.Name == item.FieldName)
                        {
                            var concatString = string.Join(" ", item.Value);
                            mustQueries.Add(new QueryStringQuery()
                            {
                                Name = property.Name,
                                Query = concatString,
                                Fields = new[] { property.Name }
                            });
                        }
                    }
                }
            }

            if (searchParam?.IncludeNoStock != true)
            {
                mustQueries.Add(new NumericRangeQuery
                {
                    Field = "quantity",
                    GreaterThan = 0
                });
            }

            if (searchParam != null && (searchParam.MaxPrice != null || searchParam.MinPrice != null))
            {
                if (searchParam.MaxPrice != null)
                {
                    mustQueries.Add(new NumericRangeQuery
                    {
                        Field = "standard_price",
                        LessThanOrEqualTo = (double)searchParam.MaxPrice + 0.01
                    });
                }

                if (searchParam.MinPrice != null)
                {
                    mustQueries.Add(new NumericRangeQuery
                    {
                        Field = "standard_price",
                        GreaterThanOrEqualTo = (double)searchParam.MinPrice - 0.01
                    });
                }
            }

            if (!string.IsNullOrEmpty(fullTextSearch))
            {
                mustQueries.Add(new MultiMatchQuery
                {
                    Fields = new[] { "item_name", "item_sku", "product_description" },
                    Query = fullTextSearch,
                    Fuzziness = Fuzziness.Auto
                });
            }

            searchDescriptor.Query(q => q.Bool(b => b.Must(mustQueries.ToArray())))
                .From((pagination.PageNumber - 1) * pagination.PageSize)
                .Size(pagination.PageSize);

            var searchResponse = await client.SearchAsync<VsiriusProduct>(searchDescriptor.Index(_table));

            return new PaginationUtility<VsiriusProductSearchDto>(
                new PaginationUtility<VsiriusProductSearchDto>.PaginationResult(
                    (int)searchResponse.Total,
                    pagination.PageNumber,
                    pagination.PageSize,
                    pagination.PageSize * pagination.PageNumber),
                searchResponse.Documents.Select(x => new VsiriusProductSearchDto()
                {
                    item_sku = x.item_sku,
                    item_name = x.item_name,
                    bullet_point1 = x.bullet_point1,
                    bullet_point2 = x.bullet_point2,
                    bullet_point3 = x.bullet_point3,
                    bullet_point4 = x.bullet_point4,
                    bullet_point5 = x.bullet_point5,
                    bullet_point6 = x.bullet_point6,
                    bullet_point7 = x.bullet_point7,
                    bullet_point8 = x.bullet_point8,
                    bullet_point9 = x.bullet_point9,
                    bullet_point10 = x.bullet_point10,
                    standard_price = x.standard_price.ToString("0.00"),
                    sale_price = x.sale_price,
                    color_name = x.color_name,
                    size_name = x.size_name,
                    main_image_url = x.main_image_url,
                    other_image_url1 = x.other_image_url1,
                    image_standard_1 = x.other_image_url2,
                    image_standard_2 = x.other_image_url3,
                    image_standard_3 = x.other_image_url4,
                    image_standard_4 = x.other_image_url5,
                    image_standard_5 = x.other_image_url6,
                    quantity = x.quantity,
                    color_map = x.color_map,
                    feed_product_type = x.feed_product_type,
                    gem_type1 = x.gem_type1,
                    item_type = x.item_type,
                    metal_type = x.metal_type
                }).ToList()
            );
        }
    }
}
