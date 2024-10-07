using QgoldIntegrationDataApi._Repositories.Interfaces;
using QgoldIntegrationDataApi._Services.Interfaces;
using QgoldIntegrationDataApi.Helpers.Ultilities;
using QgoldIntegrationDataApi.Helpers.Params;
using QgoldIntegrationDataApi._Repositories;
using QgoldIntegrationDataApi.Helpers;
using QgoldIntegrationDataApi.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QgoldIntegrationDataApi.Data;
using EFCore.BulkExtensions;
using Korzh.EasyQuery.Linq;
using System.Data;
using AutoMapper;
using LinqKit;
using System;
using QgoldIntegrationDataApi.Dtos.Qgold;

namespace QgoldIntegrationDataApi._Services.Services
{
    public class QgoldCsvDataService : IQgoldCsvDataService
    {
        private readonly IRepositoryAccessor _repositoryAccessor;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly DBContext _context;
        private readonly IAzureStorage _storage;
        public QgoldCsvDataService(
            IRepositoryAccessor repositoryAccessor,
            IMapper mapper,
            MapperConfiguration mapperConfiguration,
            DBContext context,
            IAzureStorage storage
            )
        {
            _repositoryAccessor = repositoryAccessor;
            _mapper = mapper;
            _mapperConfiguration = mapperConfiguration;
            _context = context;
            _storage = storage;
        }

        public async Task<OperationResult> Create(QgoldCsvData_DTO dto)
        {
            var data = _mapper.Map<QgoldProduct>(dto);
            data.CreateDate = DateTime.Now;
            _repositoryAccessor.QgoldCsvDatas.Add(data);
            try
            {
                await _repositoryAccessor.SaveChangesAsync();
                return new OperationResult { IsSuccess = true, Data = data };
            }
            catch (Exception ex)
            {
                return new OperationResult { IsSuccess = false, Error = ex.Message };
            }
        }

        public async Task<OperationResult> Delete(QgoldCsvData_DTO dto)
        {
            var data = _mapper.Map<QgoldProduct>(dto);
            data.UpdateDate = DateTime.Now;
            _repositoryAccessor.QgoldCsvDatas.Update(data);
            try
            {
                await _repositoryAccessor.SaveChangesAsync();
                return new OperationResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new OperationResult { IsSuccess = false, Error = ex.Message };
            }
        }
        public async Task<OperationResult> GenDataByExcel(IFormFile fileExcel)
        {
            string fileExtension = Path.GetExtension(fileExcel.FileName);
            string[] arrNameFile = new string[] {
                ".xlsx", ".xls"
            };

            if (!arrNameFile.Contains(fileExtension))
            {
                return new OperationResult(false, "Not file excel");
            }
            var dataTable = Common.ReadExcelFileToDynamic(fileExcel);
            try
            {
                var curDate = DateTime.Now;
                var data = _mapper.Map<List<QgoldProduct>>(dataTable);
                data.ForEach(x => x.CreateDate = curDate);
                _repositoryAccessor.QgoldCsvDatas.AddMultiple(data);
                await _repositoryAccessor.SaveChangesAsync();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }

        }

        public async Task<List<KeyValuePair<string, string>>> GetBookList()
        {
            return await _repositoryAccessor.QgoldCsvDatas.FindAll(x => x.isDeleted == false).Select(x => new KeyValuePair<string, string>(
                x.Book,
                x.Book
            )).ToListAsync();
        }

        public async Task<PaginationUtility<QgoldCsvData_DTO>> GetData(PaginationParam pagination, QgoldCsvDataFillter dataFillter, bool isPaging = true)
        {
            var predicate = PredicateBuilder.New<QgoldProduct>(x => x.isDeleted != true);

            //if (!string.IsNullOrWhiteSpace(dataFillter.book) || dataFillter.book != null)
            //    predicate.And(x => x.Book.Contains(dataFillter.book));

            //if (!string.IsNullOrWhiteSpace(dataFillter.country_Of_Origin) || dataFillter.country_Of_Origin != null)
            //    predicate.And(x => x.Country_Of_Origin.Contains(dataFillter.country_Of_Origin));

            predicate.And(x => x.Image1Link != " " && x.Status == "Active");

            var data = _repositoryAccessor.QgoldCsvDatas
                                         .FindAll(predicate)
                                         .FullTextSearchQuery(dataFillter?.keyword)
                                         .ProjectTo<QgoldCsvData_DTO>(_mapperConfiguration).AsNoTracking();
            return await PaginationUtility<QgoldCsvData_DTO>.CreateAsync(data, pagination.PageNumber, pagination.PageSize, isPaging);
        }

        public async Task<List<string>> GetDataQgoldItem()
        {
            return await _repositoryAccessor.QgoldCsvDatas.FindAll().Select(x => x.Item).ToListAsync();
        }

        public async Task<OperationResult> Update(QgoldCsvData_DTO dto)
        {
            var data = _mapper.Map<QgoldProduct>(dto);
            _repositoryAccessor.QgoldCsvDatas.Update(data);
            try
            {
                await _repositoryAccessor.SaveChangesAsync();
                return new OperationResult { IsSuccess = true, Data = data };
            }
            catch (Exception ex)
            {
                return new OperationResult { IsSuccess = false, Error = ex.Message };
            }
        }

        public async Task<OperationResult> CreateAndUpdateDataQgold(List<QgoldData> dto)
        {
            try
            {
                var curDate = DateTime.Now;
                var listItemQgold = await _repositoryAccessor.QgoldCsvDatas
                    .FindAll(x => x.isDeleted == false)
                    .Select(x => x.Item)
                    .ToListAsync();

                var listItemQgoldDeleted = await _repositoryAccessor.QgoldCsvDatas
                    .FindAll(x => x.isDeleted == true)
                    .Select(x => x.Item)
                    .ToListAsync();

                var newData = _mapper.Map<List<QgoldProduct>>(dto);

                var lstInsert = new List<QgoldProduct>();
                var lstUpdate = new List<QgoldProduct>();

                foreach (var item in newData)
                {
                    if (listItemQgold.Contains(item.Item))
                    {
                        lstUpdate.Add(item);
                    }
                    else
                    {
                        if(!listItemQgoldDeleted.Contains(item.Item))
                        {
                            lstInsert.Add(item);
                        }
                    }
                }

                if (lstInsert.Count > 0)
                {
                    Parallel.ForEach(lstInsert, item =>
                    {
                        item.CreateDate = curDate;
                        item.Image1Link = _storage.UploadWithLink(item.Image1Link, 1, item.Item).Result.Blob.Uri;
                        item.Image2Link = _storage.UploadWithLink(item.Image2Link, 2, item.Item).Result.Blob.Uri;
                        item.Image3Link = _storage.UploadWithLink(item.Image3Link, 3, item.Item).Result.Blob.Uri;
                        item.Image4Link = _storage.UploadWithLink(item.Image4Link, 4, item.Item).Result.Blob.Uri;
                        item.Image5Link = _storage.UploadWithLink(item.Image5Link, 5, item.Item).Result.Blob.Uri;
                        item.Image6Link = _storage.UploadWithLink(item.Image6Link, 6, item.Item).Result.Blob.Uri;
                        item.Image7Link = _storage.UploadWithLink(item.Image7Link, 7, item.Item).Result.Blob.Uri;
                        item.Image8Link = _storage.UploadWithLink(item.Image8Link, 8, item.Item).Result.Blob.Uri;
                        item.Image9Link = _storage.UploadWithLink(item.Image9Link, 9, item.Item).Result.Blob.Uri;
                    });


                    await _context.BulkInsertAsync(lstInsert, bulkConfig: new BulkConfig() { BulkCopyTimeout = 0 });
                }


                if (lstUpdate.Count > 0)
                {
                    lstUpdate.ForEach(
                       x => x.UpdateDate = curDate
                    );

                    await _context.BulkUpdateAsync(lstUpdate, bulkConfig: new BulkConfig()
                    {
                        PropertiesToExclude = new List<string>
                        {
                            "CreateDate",
                            "Image1Link",
                            "Image2Link",
                            "Image3Link",
                            "Image4Link",
                            "Image5Link",
                            "Image6Link",
                            "Image7Link",
                            "Image8Link",
                            "Image9Link",
                        },
                        BulkCopyTimeout = 0
                    });

                }

                return new OperationResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new OperationResult { IsSuccess = false, Error = ex.Message };
            }
        }

        public async Task<OperationResult> DeleteDataQgold(List<string> dto)
        {
            try
            {
                List<QgoldProduct> listDelete = new List<QgoldProduct>();

                await _context.QgoldCsvDatas.Where(x => dto.Contains(x.Item))
                    .ExecuteUpdateAsync(x => x.SetProperty(a => a.isDeleted, a => true));

                return new OperationResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new OperationResult { IsSuccess = false, Error = ex.Message };
            }
        }
    }
}
