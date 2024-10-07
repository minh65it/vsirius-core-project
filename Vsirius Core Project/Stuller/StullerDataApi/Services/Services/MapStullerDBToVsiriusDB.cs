using StullerDataApi.Services.IServices;
using StullerDataApi.Utilities;
using StullerDataApi.Utilities.Mapping.MapStullerDBToVsiriusDB;

namespace StullerDataApi.Services.Services
{
    public class MapStullerDBToVsiriusDB : IMapStullerDBToVsiriusDB
    {
        private readonly IExportDataVsirius _exportDataVsirius;
        private readonly ICallAPIVSiriusWeb _callAPIVSiriusWeb;

        public MapStullerDBToVsiriusDB(
            IExportDataVsirius exportDataVsirius, 
            ICallAPIVSiriusWeb callAPIVSiriusWeb)
        {
            _exportDataVsirius = exportDataVsirius;
            _callAPIVSiriusWeb = callAPIVSiriusWeb;
        }

        public async Task MapToVsiriusDB()
        {
            await Common.WriteLogAsync(@$"StullerToVsiriusWebProcessor start =================================");
            //export data
            await _exportDataVsirius.ExportExcel();

            //Import data
            await Common.WriteLogAsync("Stuller to VsiriusWeb import excel!");
            var listItemSKUDatabase = await _callAPIVSiriusWeb.GetAllItem();
            var checkImport = await _callAPIVSiriusWeb.ImportExcel();
            //Delete data
            if (checkImport)
            {
                await _callAPIVSiriusWeb.DeleteMultiple(listItemSKUDatabase);
            }
            await Common.WriteLogAsync(@$"StullerToVsiriusWebProcessor done ===============================");
        }
    }
}
