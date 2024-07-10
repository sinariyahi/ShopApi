using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileInDisk(IFormFile file);
        Task<ShopActionResult<int>> CreateFolder(string schema, string entity);

        Task<ShopActionResult<int>> SaveFileInFolder(List<IFormFile> list, string schema, string entity);
        Task<ShopActionResult<int>> SaveNewItemInFolder(IFormFile model, string schema, string entity, string finallyEntity, object val, object fKeyId, DateTime createDate, Guid userId, bool isCreateNewfolder = true);


        Task<string> GetFileInFolder(string entity, object valueId, CompanyAttachmentType companyAttachmentType, SupplierAttachmentType supplierAttachmentType, VendorAttachmentType vendorAttachmentType);

        Task<List<string>> GetListOfFiles(string entity, object valueId, CompanyAttachmentType companyAttachmentType, SupplierAttachmentType supplierAttachmentType, VendorAttachmentType vendorAttachmentType);


        string GetFileInFolderWithFilePath(string filePath);

        Task<ShopActionResult<int>> DeleteFileInFolder(IFormFile model, string schema, string entity);

        Task<ShopActionResult<int>> CreateFolderNewItem(string path, string entity);
        Task<ShopActionResult<List<FileModel>>> GetArchiveMedia(int companyid, CompanyType companyType);
        Task<bool> DeleteFile(FileItemDto file);

    }
}
