using Application.Interfaces;
using Domain.Entities.Base;
using Domain.Entities.Blog;
using Domain.Entities.Catalog;
using Domain.Entities.Media;
using Domain.Entities.Support;
using Domain;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NanoidDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XAct;
using Newtonsoft.Json;

namespace Application.Services
{
    public class FileService : IFileService
    {


        //string _filePath = "\\project\\SepidAria Projects\\HematBackEnd\\API\\Files";
        private readonly string _filePath;
        private readonly string _websiteAddress;
        private BIContext _context;
        private readonly ILogger _logger;
        private readonly Configs _configs;

        public FileService(BIContext context, IOptions<Configs> options, ILogger<FileService> logger)
        {
            this._context = context;
            this._filePath = options.Value.FilePath;
            this._websiteAddress = options.Value.WebSiteAddress;
            this._logger = logger;
            _configs = options.Value;

        }

        public async Task<ShopActionResult<int>> CreateFolder(string schema, string entity)
        {
            var result = new ShopActionResult<int>();
            try
            {
                var folder = _filePath + "\\" + schema + "\\" + entity;
                //string folder = Path.Combine(schemaEntity, entity);

                if (Directory.Exists(folder))
                {
                    result.IsSuccess = true;
                    result.Message = "Has Folder";
                    return result;
                }

                else
                {
                    Directory.CreateDirectory(folder);
                    result.IsSuccess = true;
                    result.Message = "CreateFolder";
                    return result;

                }
            }
            catch (Exception)
            {

                result.IsSuccess = false;
                return result;
            }



        }

        public async Task<ShopActionResult<int>> CreateFolderNewItem(string path, string entity)
        {
            var result = new ShopActionResult<int>();
            try
            {
                var folder = path + "\\" + entity;
                //string folder = Path.Combine(schemaEntity, entity);

                if (Directory.Exists(folder))
                {
                    result.IsSuccess = true;
                    result.Message = "Has Folder";
                    return result;
                }

                else
                {
                    Directory.CreateDirectory(folder);
                    result.IsSuccess = true;
                    result.Message = "CreateFolder";
                    return result;

                }
            }
            catch (Exception)
            {

                result.IsSuccess = false;
                return result;
            }



        }




        public async Task<ShopActionResult<int>> DeleteFileInFolder(IFormFile model, string schema, string entity)
        {
            var result = new ShopActionResult<int>();
            try
            {
                var schemaEntity = _filePath + "\\" + schema;
                string floder = Path.Combine(schemaEntity, entity);

                string path = Path.Combine(Directory.GetCurrentDirectory(), floder, model.ToString());

                System.IO.File.Delete(path);
                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {

                result.IsSuccess = false;
                return result;
            }
        }

        public string GetFileInFolderWithFilePath(string filePath)
        {
            try
            {

                byte[] fileContents = File.ReadAllBytes(filePath);
                if (fileContents != null)
                {
                    return FileUtility.ConvertToBase64(fileContents);

                }
                else
                {
                    return "";
                }

            }
            catch (Exception ex)
            {

                return "";
            }

        }

        public async Task<string> GetFileInFolder(string entity, object valueId, CompanyAttachmentType companyAttachmentType, SupplierAttachmentType supplierAttachmentType, VendorAttachmentType vendorAttachmentType)
        {
            var finalFilePath = "";

            try
            {
                switch (entity)
                {
                    //case "CompanyAttachment":
                    //    finalFilePath += _context.CompanyAttachments.FirstOrDefault(f => f.CompanyId == Convert.ToInt32(valueId) && f.CompanyAttachmentType == companyAttachmentType)?.FilePath;
                    //    break;
                    //case "EvalutionAttachment":
                    //    finalFilePath += _context.CompanyRequestEvalutionAttachments.FirstOrDefault(f => f.CompanyRequestEvalutionId == Convert.ToInt32(valueId))?.FilePath;
                    //    break;
                    //case "SupplierCompanyAttachment":
                    //    finalFilePath += _context.SupplierCompanyAttachments.FirstOrDefault(f => f.SupplierCompanyInfoId == Convert.ToInt32(valueId) && f.SupplierAttachmentType == supplierAttachmentType)?.FilePath;
                    //    break;
                    //case "VendorCompanyAttachment":
                    //    finalFilePath += _context.VendorCompanyAttachments.FirstOrDefault(f => f.VendorCompanyInfoId == Convert.ToInt32(valueId) && f.VendorAttachmentType == vendorAttachmentType)?.FilePath;
                    //    break;
                    //case "SupplierProductCompanyAttachment":
                    //    finalFilePath += _context.SupplierProductCompanyAttachments.FirstOrDefault(f => f.SupplierCompanyProductId == Convert.ToInt32(valueId) && f.SupplierAttachmentType == supplierAttachmentType)?.FilePath;
                    //    break;
                    //case "VendorProductCompanyAttachment":
                    //    finalFilePath += _context.VendorProductCompanyAttachments.FirstOrDefault(f => f.VendorCompanyProductId == Convert.ToInt32(valueId) && f.VendorAttachmentType == vendorAttachmentType)?.FilePath;
                    //    break;
                    //case "VendorCertificateCompanyAttachment":
                    //    finalFilePath += _context.VendorCertificateCompanyAttachments.FirstOrDefault(f => f.VendorCompanyCertificateId == Convert.ToInt32(valueId) && f.VendorAttachmentType == vendorAttachmentType)?.FilePath;
                    //    break;

                }

                //filePath = filePath.Replace(@"\", "/");

                return finalFilePath;

            }
            catch (Exception ex)
            {

                return finalFilePath;
            }

        }



        public async Task<ShopActionResult<int>> SaveFileInFolder(List<IFormFile> list, string schema, string entity)
        {
            var result = new ShopActionResult<int>();
            try
            {

                var folder = _filePath + "\\" + schema + "\\" + entity;
                //string folder = Path.Combine(schemaEntity, entity);

                if (Directory.Exists(folder))
                {

                    foreach (var index in list)
                    {

                        foreach (var item in index.GetType().GetProperties())
                        {


                            if (item.PropertyType.Name == "IFormFile")
                            {

                                //  گرفتن فایل اپلود شده
                                string fileName = JsonConvert.SerializeObject(item.GetValue(index));

                                // گرفتن نوع فایل
                                var fileExtension = Path.GetExtension(fileName).ToLower();

                                // (اسم مد نظر یا از اسم خودش میتونیم یا یه اسم جدید میذاریم براش)
                                // یه اسم جدید برای فایل میسازیم
                                //ذخیره میکنیم db فقط اینو در 
                                string newFileName = Guid.NewGuid().ToString() + fileExtension;

                                /// Save in Db newFileName



                                // ادرس فایل جایی که میخوایم ذخیره کنیم
                                await CreateFolderNewItem(folder, item.Name);
                                string shareFolderPath = folder + "\\" + item.Name;

                                //  ادرس نهایی برای ذخیره  

                                string newFilePath = shareFolderPath + "\\" + newFileName.Replace("\"}", "");

                                //save file in media share folder =>  ادرس نهایی و فایل اپلود شده 
                                // چون داره کپی میشه قبلی پاک نمیشه
                                if (fileExtension != "")
                                {
                                    var ImageFile = (IFormFile)item.GetValue(index);
                                    using (FileStream stream = new FileStream(newFilePath, FileMode.Create))
                                    {
                                        await ImageFile.CopyToAsync(stream);
                                    }
                                }

                            }
                        }
                    }
                    result.IsSuccess = true;
                    return result;
                }
                else
                {
                    result.Message = "Has not Folder";
                    result.IsSuccess = true;
                    return result;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null);
                result.IsSuccess = false;
                return result;
            }


        }


        public async Task<string> SaveFileInDisk(IFormFile file)
        {
            var newFileName = Guid.NewGuid().ToString();
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            var filePath = $"{_configs.EditorMediaFolder}\\{newFileName}{fileExtension}";
            var wwwFilePath = $"{_configs.MediaServer}/{newFileName}{fileExtension}";

            //save file in media share folder
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return wwwFilePath;
        }

        public async Task<ShopActionResult<int>> SaveNewItemInFolder(IFormFile model, string schema, string entity, string finallyEntity, object valId, object fKeyId, DateTime createDate, Guid userId, bool isCreateNewfolder)
        {
            var result = new ShopActionResult<int>();
            try
            {

                var folder = _filePath + "\\" + schema + "\\" + entity;
                //string folder = Path.Combine(schemaEntity, entity);

                if (Directory.Exists(folder))
                {
                    if (model != null)
                    {

                        //  گرفتن فایل اپلود شده
                        string fileName = model.FileName;

                        // گرفتن نوع فایل
                        var fileExtension = Path.GetExtension(model.FileName).ToLower();

                        fileName = fileName.Replace(fileExtension, "");

                        // (اسم مد نظر یا از اسم خودش میتونیم یا یه اسم جدید میذاریم براش)
                        // یه اسم جدید برای فایل میسازیم
                        //ذخیره میکنیم db فقط اینو در 
                        string newFileName = String.Empty;
                        string uniqueCode = Nanoid.Generate("0123456789", 5).ToString();

                        newFileName = uniqueCode.ToString() + fileExtension;

                        string shareFolderPath = "";
                        // ادرس فایل جایی که میخوایم ذخیره کنیم
                        if (isCreateNewfolder == true)
                        {
                            await CreateFolderNewItem(folder, finallyEntity);
                            shareFolderPath = folder + "\\" + finallyEntity;
                        }
                        else
                        {
                            shareFolderPath = folder;
                        }


                        //  ادرس نهایی برای ذخیره  
                        string newFilePath = shareFolderPath + "\\" + fileName + "_" + newFileName.Replace("\"}", "");



                        /// Save in Db newFileName

                        switch (entity)
                        {
                            #region Catalog

                            case "BrandAttachments":

                                await _context.BrandAttachments.AddAsync(
                                    new BrandAttachment()
                                    {
                                        CreateDate = createDate,
                                        FileContentType = model.ContentType,
                                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                                        FileName = fileName,
                                        FileSize = model.Length.ToString(),
                                        BrandId = Convert.ToInt32(fKeyId),
                                        FileType = model.ContentType.Contains("image") ? FileType.Image : model.ContentType.Contains("doc") ? FileType.Word :
                                        model.ContentType.Contains("pdf") ? FileType.Pdf : model.ContentType.Contains("zip") ? FileType.ZIP : model.ContentType.Contains("mp4") ? FileType.MP4 :
                                        model.ContentType.Contains("ogg") ? FileType.OGG : model.ContentType.Contains("rar") ? FileType.RAR : FileType.Excel,
                                        Id = Guid.NewGuid(),
                                        FilePath = newFilePath,
                                    });
                                await _context.SaveChangesAsync();
                                break;

                            case "CategoryAttachments":

                                await _context.CategoryAttachments.AddAsync(
                                    new CategoryAttachment()
                                    {
                                        CreateDate = createDate,
                                        FileContentType = model.ContentType,
                                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                                        FileName = fileName,
                                        FileSize = model.Length.ToString(),
                                        CategoryId = Convert.ToInt32(fKeyId),
                                        FileType = model.ContentType.Contains("image") ? FileType.Image : model.ContentType.Contains("doc") ? FileType.Word :
                                        model.ContentType.Contains("pdf") ? FileType.Pdf : model.ContentType.Contains("zip") ? FileType.ZIP : model.ContentType.Contains("mp4") ? FileType.MP4 :
                                        model.ContentType.Contains("ogg") ? FileType.OGG : model.ContentType.Contains("rar") ? FileType.RAR : FileType.Excel,
                                        Id = Guid.NewGuid(),
                                        FilePath = newFilePath,
                                    });
                                await _context.SaveChangesAsync();
                                break;


                            case "MainFeatureAttachments":

                                await _context.MainFeatureAttachments.AddAsync(
                                    new MainFeatureAttachment()
                                    {
                                        CreateDate = createDate,
                                        FileContentType = model.ContentType,
                                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                                        FileName = fileName,
                                        FileSize = model.Length.ToString(),
                                        CategoryId = Convert.ToInt32(fKeyId),
                                        MainFeatureId = Convert.ToInt32(valId),
                                        FileType = model.ContentType.Contains("image") ? FileType.Image : model.ContentType.Contains("doc") ? FileType.Word :
                                        model.ContentType.Contains("pdf") ? FileType.Pdf : model.ContentType.Contains("zip") ? FileType.ZIP : model.ContentType.Contains("mp4") ? FileType.MP4 :
                                        model.ContentType.Contains("ogg") ? FileType.OGG : model.ContentType.Contains("rar") ? FileType.RAR : FileType.Excel,
                                        Id = Guid.NewGuid(),
                                        FilePath = newFilePath,
                                    });
                                await _context.SaveChangesAsync();
                                break;

                            case "ProductCoverAttachments":

                                await _context.ProductCoverAttachments.AddAsync(
                                    new ProductCoverAttachment()
                                    {
                                        CreateDate = createDate,
                                        FileContentType = model.ContentType,
                                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                                        FileName = fileName,
                                        FileSize = model.Length.ToString(),
                                        ProductId = Convert.ToInt32(fKeyId),
                                        FileType = model.ContentType.Contains("image") ? FileType.Image : model.ContentType.Contains("doc") ? FileType.Word :
                                        model.ContentType.Contains("pdf") ? FileType.Pdf : model.ContentType.Contains("zip") ? FileType.ZIP : model.ContentType.Contains("mp4") ? FileType.MP4 :
                                        model.ContentType.Contains("ogg") ? FileType.OGG : model.ContentType.Contains("rar") ? FileType.RAR : FileType.Excel,
                                        Id = Guid.NewGuid(),
                                        FilePath = newFilePath,
                                    });
                                await _context.SaveChangesAsync();
                                break;
                            case "Catalog":

                                await _context.ProductAttachments.AddAsync(
                                    new ProductAttachment()
                                    {
                                        CreateDate = createDate,
                                        FileContentType = model.ContentType,
                                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                                        FileName = fileName,
                                        FileSize = model.Length.ToString(),
                                        ProductId = Convert.ToInt32(fKeyId),
                                        ProductAttachmentType = EnumHelpers.ParseEnum<ProductAttachmentType>(valId.ToString()),
                                        FileType = model.ContentType.Contains("image") ? FileType.Image : model.ContentType.Contains("doc") ? FileType.Word :
                                        model.ContentType.Contains("pdf") ? FileType.Pdf : model.ContentType.Contains("zip") ? FileType.ZIP : model.ContentType.Contains("mp4") ? FileType.MP4 :
                                        model.ContentType.Contains("ogg") ? FileType.OGG : model.ContentType.Contains("rar") ? FileType.RAR : FileType.Excel,
                                        Id = Guid.NewGuid(),
                                        FilePath = newFilePath,
                                    });
                                await _context.SaveChangesAsync();
                                break;


                            case "OtherFiles":

                                await _context.ProductAttachments.AddAsync(
                                    new ProductAttachment()
                                    {
                                        CreateDate = createDate,
                                        FileContentType = model.ContentType,
                                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                                        FileName = fileName,
                                        FileSize = model.Length.ToString(),
                                        ProductId = Convert.ToInt32(fKeyId),
                                        ProductAttachmentType = EnumHelpers.ParseEnum<ProductAttachmentType>(valId.ToString()),
                                        FileType = model.ContentType.Contains("image") ? FileType.Image : model.ContentType.Contains("doc") ? FileType.Word :
                                        model.ContentType.Contains("pdf") ? FileType.Pdf : model.ContentType.Contains("zip") ? FileType.ZIP : model.ContentType.Contains("mp4") ? FileType.MP4 :
                                        model.ContentType.Contains("ogg") ? FileType.OGG : model.ContentType.Contains("rar") ? FileType.RAR : FileType.Excel,
                                        Id = Guid.NewGuid(),
                                        FilePath = newFilePath,
                                    });
                                await _context.SaveChangesAsync();
                                break;


                            case "OrginalFiles":

                                await _context.ProductAttachments.AddAsync(
                                    new ProductAttachment()
                                    {
                                        CreateDate = createDate,
                                        FileContentType = model.ContentType,
                                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                                        FileName = fileName,
                                        FileSize = model.Length.ToString(),
                                        ProductId = Convert.ToInt32(fKeyId),
                                        ProductAttachmentType = EnumHelpers.ParseEnum<ProductAttachmentType>(valId.ToString()),
                                        FileType = model.ContentType.Contains("image") ? FileType.Image : model.ContentType.Contains("doc") ? FileType.Word :
                                        model.ContentType.Contains("pdf") ? FileType.Pdf : model.ContentType.Contains("zip") ? FileType.ZIP : model.ContentType.Contains("mp4") ? FileType.MP4 :
                                        model.ContentType.Contains("ogg") ? FileType.OGG : model.ContentType.Contains("rar") ? FileType.RAR : FileType.Excel,
                                        Id = Guid.NewGuid(),
                                        FilePath = newFilePath,
                                    });
                                await _context.SaveChangesAsync();
                                break;

                            case "VideoFileProductAttachments":

                                await _context.VideoFileProductAttachments.AddAsync(
                                    new VideoFileProductAttachment()
                                    {
                                        CreateDate = createDate,
                                        FileContentType = model.ContentType,
                                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                                        FileName = fileName,
                                        FileSize = model.Length.ToString(),
                                        VideoProductAttachmentId = Convert.ToInt32(fKeyId),
                                        FileType = model.ContentType.Contains("image") ? FileType.Image : model.ContentType.Contains("doc") ? FileType.Word :
                                        model.ContentType.Contains("pdf") ? FileType.Pdf : model.ContentType.Contains("zip") ? FileType.ZIP : model.ContentType.Contains("mp4") ? FileType.MP4 :
                                        model.ContentType.Contains("ogg") ? FileType.OGG : model.ContentType.Contains("rar") ? FileType.RAR : FileType.Excel,
                                        Id = Guid.NewGuid(),
                                        FilePath = newFilePath,
                                    });
                                await _context.SaveChangesAsync();
                                break;
                            #endregion

                            #region Blog
                            case "ArticleAttachments":

                                await _context.ArticleAttachments.AddAsync(
                                    new ArticleAttachment()
                                    {
                                        CreateDate = createDate,
                                        FileContentType = model.ContentType,
                                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                                        FileName = fileName,
                                        FileSize = model.Length.ToString(),
                                        ArticleId = Convert.ToInt32(fKeyId),
                                        FileType = model.ContentType.Contains("image") ? FileType.Image : model.ContentType.Contains("doc") ? FileType.Word : model.ContentType.Contains("pdf") ? FileType.Pdf : model.ContentType.Contains("zip") ? FileType.ZIP : model.ContentType.Contains("Rar") ? FileType.RAR : FileType.Excel,
                                        Id = Guid.NewGuid(),
                                        FilePath = newFilePath,
                                    });
                                await _context.SaveChangesAsync();
                                break;
                            #endregion

                            #region Media
                            case "VideoAttachments":

                                await _context.VideoAttachments.AddAsync(
                                    new VideoAttachment()
                                    {
                                        CreateDate = createDate,
                                        FileContentType = model.ContentType,
                                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                                        FileName = fileName,
                                        FileSize = model.Length.ToString(),
                                        VideoId = Convert.ToInt32(fKeyId),
                                        FileType = model.ContentType.Contains("image") ? FileType.Image : model.ContentType.Contains("doc") ? FileType.Word :
                                        model.ContentType.Contains("pdf") ? FileType.Pdf : model.ContentType.Contains("zip") ? FileType.ZIP : model.ContentType.Contains("mp4") ? FileType.MP4 :
                                        model.ContentType.Contains("ogg") ? FileType.OGG : model.ContentType.Contains("rar") ? FileType.RAR : FileType.Excel,
                                        Id = Guid.NewGuid(),
                                        FilePath = newFilePath,
                                    });
                                await _context.SaveChangesAsync();
                                break;

                            case "BannerAttachments":

                                await _context.BannerAttachments.AddAsync(
                                    new BannerAttachment()
                                    {
                                        CreateDate = createDate,
                                        FileContentType = model.ContentType,
                                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                                        FileName = fileName,
                                        FileSize = model.Length.ToString(),
                                        BannerId = Convert.ToInt32(fKeyId),
                                        FileType = model.ContentType.Contains("image") ? FileType.Image : model.ContentType.Contains("doc") ? FileType.Word :
                                        model.ContentType.Contains("pdf") ? FileType.Pdf : model.ContentType.Contains("zip") ? FileType.ZIP : model.ContentType.Contains("mp4") ? FileType.MP4 :
                                        model.ContentType.Contains("ogg") ? FileType.OGG : model.ContentType.Contains("rar") ? FileType.RAR : FileType.Excel,
                                        Id = Guid.NewGuid(),
                                        FilePath = newFilePath,
                                    });
                                await _context.SaveChangesAsync();
                                break;


                            case "SliderAttachments":

                                await _context.SliderAttachments.AddAsync(
                                    new SliderAttachment()
                                    {
                                        CreateDate = createDate,
                                        FileContentType = model.ContentType,
                                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                                        FileName = fileName,
                                        FileSize = model.Length.ToString(),
                                        SliderId = Convert.ToInt32(fKeyId),
                                        Device = EnumHelpers.ParseEnum<Device>(valId.ToString()),
                                        FileType = model.ContentType.Contains("image") ? FileType.Image : model.ContentType.Contains("doc") ? FileType.Word :
                                        model.ContentType.Contains("pdf") ? FileType.Pdf : model.ContentType.Contains("zip") ? FileType.ZIP : model.ContentType.Contains("mp4") ? FileType.MP4 :
                                        model.ContentType.Contains("ogg") ? FileType.OGG : model.ContentType.Contains("rar") ? FileType.RAR : FileType.Excel,
                                        Id = Guid.NewGuid(),
                                        FilePath = newFilePath,
                                    });
                                await _context.SaveChangesAsync();
                                break;


                            case "VideoCoverAttachments":

                                await _context.VideoCoverAttachments.AddAsync(
                                    new VideoCoverAttachment()
                                    {
                                        CreateDate = createDate,
                                        FileContentType = model.ContentType,
                                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                                        FileName = fileName,
                                        FileSize = model.Length.ToString(),
                                        VideoId = Convert.ToInt32(fKeyId),
                                        FileType = model.ContentType.Contains("image") ? FileType.Image : model.ContentType.Contains("doc") ? FileType.Word :
                                        model.ContentType.Contains("pdf") ? FileType.Pdf : model.ContentType.Contains("zip") ? FileType.ZIP : model.ContentType.Contains("mp4") ? FileType.MP4 :
                                        model.ContentType.Contains("ogg") ? FileType.OGG : model.ContentType.Contains("rar") ? FileType.RAR : FileType.Excel,
                                        Id = Guid.NewGuid(),
                                        FilePath = newFilePath,
                                    });
                                await _context.SaveChangesAsync();
                                break;
                            #endregion

                            #region Base
                            case "SocialMediaAttachments":

                                await _context.SocialMediaAttachments.AddAsync(
                                    new SocialMediaAttachment()
                                    {
                                        CreateDate = createDate,
                                        FileContentType = model.ContentType,
                                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                                        FileName = fileName,
                                        FileSize = model.Length.ToString(),
                                        SocialMediaId = Convert.ToInt32(fKeyId),
                                        FileType = model.ContentType.Contains("image") ? FileType.Image : model.ContentType.Contains("doc") ? FileType.Word :
                                        model.ContentType.Contains("pdf") ? FileType.Pdf : model.ContentType.Contains("zip") ? FileType.ZIP : model.ContentType.Contains("mp4") ? FileType.MP4 :
                                        model.ContentType.Contains("ogg") ? FileType.OGG : model.ContentType.Contains("rar") ? FileType.RAR : FileType.Excel,
                                        Id = Guid.NewGuid(),
                                        FilePath = newFilePath,
                                    });
                                await _context.SaveChangesAsync();
                                break;
                            #endregion


                            #region Support
                            case "CooperationFormAttachments":

                                await _context.CooperationFormAttachments.AddAsync(
                                    new CooperationFormAttachment()
                                    {
                                        CreateDate = createDate,
                                        FileContentType = model.ContentType,
                                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                                        FileName = fileName,
                                        FileSize = model.Length.ToString(),
                                        CooperationFormId = Convert.ToInt32(fKeyId),
                                        FileType = model.ContentType.Contains("image") ? FileType.Image : model.ContentType.Contains("doc") ? FileType.Word :
                                        model.ContentType.Contains("pdf") ? FileType.Pdf : model.ContentType.Contains("zip") ? FileType.ZIP : model.ContentType.Contains("mp4") ? FileType.MP4 :
                                        model.ContentType.Contains("ogg") ? FileType.OGG : model.ContentType.Contains("rar") ? FileType.RAR : FileType.Excel,
                                        Id = Guid.NewGuid(),
                                        FilePath = newFilePath,
                                    });
                                await _context.SaveChangesAsync();
                                break;

                            case "ContactFormAttachments":

                                await _context.ContactFormAttachments.AddAsync(
                                    new ContactFormAttachment()
                                    {
                                        CreateDate = createDate,
                                        FileContentType = model.ContentType,
                                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                                        FileName = fileName,
                                        FileSize = model.Length.ToString(),
                                        ContactFormId = Convert.ToInt32(fKeyId),
                                        FileType = model.ContentType.Contains("image") ? FileType.Image : model.ContentType.Contains("doc") ? FileType.Word :
                                        model.ContentType.Contains("pdf") ? FileType.Pdf : model.ContentType.Contains("zip") ? FileType.ZIP : model.ContentType.Contains("mp4") ? FileType.MP4 :
                                        model.ContentType.Contains("ogg") ? FileType.OGG : model.ContentType.Contains("rar") ? FileType.RAR : FileType.Excel,
                                        Id = Guid.NewGuid(),
                                        FilePath = newFilePath,
                                    });
                                await _context.SaveChangesAsync();
                                break;






                                #endregion
                        }



                        //save file in media share folder
                        using (FileStream stream = new FileStream(newFilePath, FileMode.Create))
                        {
                            await model.CopyToAsync(stream);
                        }

                    }

                    result.IsSuccess = true;
                    return result;
                }
                else
                {
                    result.Message = "Has not Folder";
                    result.IsSuccess = true;
                    return result;
                }

            }
            catch (Exception ex)
            {

                result.IsSuccess = false;
                return result;
            }
        }

        public async Task<ShopActionResult<List<FileModel>>> GetArchiveMedia(int companyid, CompanyType companyType)
        {
            var result = new ShopActionResult<List<FileModel>>();
            var finalyList = new List<FileModel>();

            var subFile = new List<FileModel>();

            var guidCodeForCompnay = double.Parse(Regex.Replace(EncryptionUtility.GenerateName(), "[^1-9]", ""));
            var guidCodeForVendorCompnay = double.Parse(Regex.Replace(EncryptionUtility.GenerateName(), "[^1-9]", ""));
            var guidCodeForSupplierCompnay = double.Parse(Regex.Replace(EncryptionUtility.GenerateName(), "[^1-9]", ""));

            var guidCodeForVendorProduct = double.Parse(Regex.Replace(EncryptionUtility.GenerateName(), "[^1-9]", ""));
            var guidCodeForVendorCertificate = double.Parse(Regex.Replace(EncryptionUtility.GenerateName(), "[^1-9]", ""));
            var guidCodeForVendorAttachmentType = double.Parse(Regex.Replace(EncryptionUtility.GenerateName(), "[^1-9]", ""));

            var guidCodeForSupplierAttachmentType = double.Parse(Regex.Replace(EncryptionUtility.GenerateName(), "[^1-9]", ""));
            var guidCodeForSupplierProduct = double.Parse(Regex.Replace(EncryptionUtility.GenerateName(), "[^1-9]", ""));

            var guidCodeForCompanyRequestEvalution = double.Parse(Regex.Replace(EncryptionUtility.GenerateName(), "[^1-9]", ""));


            //var company = await _context.Companies.SingleOrDefaultAsync(s => s.Id == companyid);
            //var tempFileList = new List<FileModel>();

            //if (company != null)
            //{
            //    if (companyType == 0)
            //    {
            //        //if (company.IsAcceptedSupplier == true)
            //        //{
            //        tempFileList.Add(new FileModel
            //        {
            //            CompanyId = companyid,
            //            CompanyType = CompanyType.Supplier,
            //            FilePath = "",
            //            CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Supplier).ToString(),
            //            Key = guidCodeForSupplierCompnay,
            //            Value = guidCodeForSupplierCompnay,
            //            Title = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Supplier).ToString(),
            //            ParentId = 0,

            //        });

            //        //if (company.IsAcceptedVendor == true)
            //        //{
            //        tempFileList.Add(new FileModel
            //        {
            //            CompanyId = companyid,
            //            CompanyType = CompanyType.Vendor,
            //            FilePath = "",
            //            CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Vendor).ToString(),
            //            Key = guidCodeForVendorCompnay,
            //            Value = guidCodeForVendorCompnay,
            //            Title = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Vendor).ToString(),
            //            ParentId = 0,

            //        });
            //    }
            //    if (companyType == CompanyType.Supplier)
            //    {
            //        tempFileList.Add(new FileModel
            //        {
            //            CompanyId = companyid,
            //            CompanyType = CompanyType.Supplier,
            //            FilePath = "",
            //            CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Supplier).ToString(),
            //            Key = guidCodeForSupplierCompnay,
            //            Value = guidCodeForSupplierCompnay,
            //            Title = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Supplier).ToString(),
            //            ParentId = 0,

            //        });
            //    }
            //    if (companyType == CompanyType.Vendor)
            //    {
            //        tempFileList.Add(new FileModel
            //        {
            //            CompanyId = companyid,
            //            CompanyType = CompanyType.Vendor,
            //            FilePath = "",
            //            CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Vendor).ToString(),
            //            Key = guidCodeForVendorCompnay,
            //            Value = guidCodeForVendorCompnay,
            //            Title = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Vendor).ToString(),
            //            ParentId = 0,

            //        });
            //    }

            //    tempFileList.Add(new FileModel
            //    {
            //        CompanyId = companyid,
            //        CompanyType = 0,
            //        FilePath = "",
            //        CompanyTypeTitle = "CompanyInfo",
            //        Key = guidCodeForCompnay,
            //        Value = guidCodeForCompnay,
            //        Title = "CompanyInfo",
            //        ParentId = 0,

            //    });

            //    tempFileList.Add(new FileModel
            //    {
            //        CompanyId = companyid,
            //        CompanyType = 0,
            //        FilePath = "",
            //        CompanyTypeTitle = "Initial Evalution",
            //        Key = guidCodeForCompanyRequestEvalution,
            //        Value = guidCodeForCompanyRequestEvalution,
            //        Title = "Initial Evalution",
            //        ParentId = 0,

            //    });

            //    subFile = tempFileList;
            //}

            //if (subFile.Count() > 0)
            //{
            //    foreach (var item in subFile)
            //    {
            //        // Files For Supplier
            //        if (item.CompanyType == CompanyType.Supplier)
            //        {
            //            item.Children.Add(new FileModel
            //            {
            //                CompanyId = companyid,
            //                CompanyType = CompanyType.Supplier,
            //                FilePath = "",
            //                CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Supplier).ToString(),
            //                Key = guidCodeForSupplierAttachmentType,
            //                Value = guidCodeForSupplierAttachmentType,
            //                Title = "SupplierCompanyInfo",
            //                ParentId = guidCodeForSupplierCompnay,

            //            });

            //            item.Children.Add(new FileModel
            //            {
            //                CompanyId = companyid,
            //                CompanyType = CompanyType.Supplier,
            //                FilePath = "",
            //                CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Supplier).ToString(),
            //                Key = guidCodeForSupplierProduct,
            //                Value = guidCodeForSupplierProduct,
            //                Title = "SupplierProductCompany",
            //                ParentId = guidCodeForSupplierCompnay,

            //            });
            //        }

            //        // Files For Vendor
            //        if (item.CompanyType == CompanyType.Vendor)
            //        {
            //            item.Children.Add(new FileModel
            //            {
            //                CompanyId = companyid,
            //                CompanyType = CompanyType.Vendor,
            //                FilePath = "",
            //                CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Vendor).ToString(),
            //                Key = guidCodeForVendorAttachmentType,
            //                Value = guidCodeForVendorAttachmentType,
            //                Title = "VendorCompanyInfo",
            //                ParentId = guidCodeForVendorCompnay,

            //            });

            //            item.Children.Add(new FileModel
            //            {
            //                CompanyId = companyid,
            //                CompanyType = CompanyType.Vendor,
            //                FilePath = "",
            //                CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Vendor).ToString(),
            //                Key = guidCodeForVendorProduct,
            //                Value = guidCodeForVendorProduct,
            //                Title = "VendorProductCompany",
            //                ParentId = guidCodeForVendorCompnay,

            //            });

            //            item.Children.Add(new FileModel
            //            {
            //                CompanyId = companyid,
            //                CompanyType = CompanyType.Vendor,
            //                FilePath = "",
            //                CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Vendor).ToString(),
            //                Key = guidCodeForVendorCertificate,
            //                Value = guidCodeForVendorCertificate,
            //                Title = "VendorCertificates",
            //                ParentId = guidCodeForVendorCompnay,

            //            });
            //        }

            //        if (item.CompanyTypeTitle == "CompanyInfo")
            //        {
            //            var compnyInfoAttachment = await _context.CompanyAttachments.Where(w => w.CompanyId == companyid).ToListAsync();
            //            foreach (var compnyInfo in compnyInfoAttachment)
            //            {
            //                item.Children.Add(new FileModel
            //                {
            //                    CompanyId = companyid,
            //                    CompanyType = 0,
            //                    FilePath = compnyInfo.FilePath,
            //                    CompanyTypeTitle = "CompanyInfo",
            //                    Key = double.Parse(Regex.Replace(compnyInfo.Id.ToString(), "[^1-9]", "")),
            //                    Value = double.Parse(Regex.Replace(compnyInfo.Id.ToString(), "[^1-9]", "")),
            //                    Title = compnyInfo.FileName,
            //                    CompanyAttachmentType = compnyInfo.CompanyAttachmentType,
            //                    ParentId = guidCodeForCompnay
            //                });
            //            }
            //        }

            //        if (item.CompanyTypeTitle == "Initial Evalution")
            //        {

            //            var companyRequestEvalutionAttachment = await _context.CompanyRequestEvalutionAttachments.Include(i=>i.User).Where(w => w.CompanyId == companyid).ToListAsync();
            //            //foreach (var companyRequestEvalution in companyRequestEvalutionAttachment)
            //            for (int i = 1; i < companyRequestEvalutionAttachment.Count; i++)
            //            {
            //                item.Children.Add(new FileModel
            //                {
            //                    CompanyId = companyid,
            //                    CompanyType = 0,
            //                    FilePath = companyRequestEvalutionAttachment[i].FilePath,
            //                    CompanyTypeTitle = "Initial Evalution",
            //                    Key = double.Parse(Regex.Replace(companyRequestEvalutionAttachment[i].Id.ToString(), "[^1-9]", "")),
            //                    Value = double.Parse(Regex.Replace(companyRequestEvalutionAttachment[i].Id.ToString(), "[^1-9]", "")),
            //                    Title = i.ToString() + " - " + companyRequestEvalutionAttachment[i].FileName + "-" + companyRequestEvalutionAttachment[i].User.FullName,
            //                    ParentId = guidCodeForCompanyRequestEvalution
            //                });
            //            }
            //        }

            //    }

            //}

            ////foreach (var child in subFile)
            //{

            //    var dataChild = new FileModel();
            //    dataChild.CompanyId = child.CompanyId;
            //    dataChild.CompanyType = child.CompanyType;
            //    dataChild.FilePath = child.FilePath;
            //    dataChild.CompanyTypeTitle = child.CompanyTypeTitle;
            //    dataChild.Key = child.Key;
            //    dataChild.Value = child.Key;
            //    dataChild.ParentId = child.ParentId;
            //    List<FileModel> children = new List<FileModel>();
            //    if (child.Children.Where(w => w.ParentId != 0).Count() > 0)
            //    {

            //        foreach (var item in child.Children.Where(w => w.ParentId != 0))
            //        {

            //            item.Children = GetArchiveMediaForDetails(item, child.Children,
            //            guidCodeForVendorAttachmentType, guidCodeForSupplierAttachmentType, guidCodeForVendorCertificate, guidCodeForSupplierProduct, guidCodeForVendorProduct);
            //            if (item.Title == "VendorCertificates")
            //            {
            //                item.Title = item.Title + " - " + item.Children.Count + " Files";
            //            }
            //        }

            //    }
            //    dataChild.Children = child.Children;
            //    dataChild.Title = dataChild.CompanyTypeTitle == "CompanyInfo" ? child.Title + " - " + child.Children.Count + " Files" :
            //                      dataChild.CompanyTypeTitle == "CompanyRequestEvalution" ? child.Title + " - " + child.Children.Count + " Files" : child.Title;

            //    finalyList.Add(dataChild);



            //}

            result.Data = finalyList;
            result.IsSuccess = true;
            return result;
        }


        private List<FileModel> GetArchiveMediaForDetails(FileModel model, List<FileModel> items,
            double guidCodeForVendorAttachmentType, double guidCodeForSupplierAttachmentType,
            double guidCodeForVendorCertificate, double guidCodeForSupplierProduct, double guidCodeForVendorProduct)
        {
            var children = new List<FileModel>();

            var vendorAttachmentTypeList = Enum.GetValues(typeof(VendorAttachmentType)).Cast<VendorAttachmentType>().ToList();

            var supplierAttachmentTypeList = Enum.GetValues(typeof(SupplierAttachmentType)).Cast<SupplierAttachmentType>().ToList();

            //foreach (var item in items)
            //{
            //    if (model.CompanyType == item.CompanyType)
            //    {

            //        switch (item.CompanyType)
            //        {
            //            case CompanyType.Vendor:
            //                //VendorProduct
            //                if (item.Title == model.Title && item.Key == guidCodeForVendorProduct)
            //                {

            //                    var vendorCompanyProducts = _context.VendorCompanyProduct.Include(i => i.Category).Include(i => i.VendorProductCompanyAttachments)
            //                     .Include(i => i.VendorCompanyInfo).Where(w => w.VendorCompanyInfo.CompanyId == item.CompanyId).ToList();

            //                    foreach (var vendorAttachmentTypeItem in vendorAttachmentTypeList)
            //                    {
            //                        if (vendorAttachmentTypeItem == VendorAttachmentType.Catalogue || vendorAttachmentTypeItem == VendorAttachmentType.UnderLicensedAgreement && vendorAttachmentTypeItem != VendorAttachmentType.Certificates)
            //                        {
            //                            var vendorProductAttachmentTypeItemCode = double.Parse(Regex.Replace(EncryptionUtility.GenerateName(), "[^1-9]", ""));
            //                            var vendorAttachmentChildren = GetArchiveMediaForFiles(vendorAttachmentTypeItem, 0, 0, null, vendorCompanyProducts, null, null, vendorProductAttachmentTypeItemCode);

            //                            children.Add(new FileModel
            //                            {
            //                                CompanyType = CompanyType.Vendor,
            //                                CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Vendor).ToString(),
            //                                Key = vendorProductAttachmentTypeItemCode,
            //                                Value = vendorProductAttachmentTypeItemCode,
            //                                Title = EnumHelpers.GetNameAttribute<VendorAttachmentType>(vendorAttachmentTypeItem).ToString() + " - " + vendorAttachmentChildren.Count + " Files",
            //                                VendorAttachmentType = vendorAttachmentTypeItem,
            //                                ParentId = model.Key,
            //                                Children = vendorAttachmentChildren
            //                            });
            //                        }

            //                    }

            //                }

            //                //VendorAttachment
            //                if (item.Title == model.Title && item.Key == guidCodeForVendorAttachmentType)
            //                {

            //                    var vendorCompanyInfo = _context.VendorCompanyInfoes
            //                                             .OrderByDescending(q => q.CreateDate)
            //                                             .FirstOrDefault(q => q.CompanyId == item.CompanyId);
            //                    if (vendorCompanyInfo != null)
            //                    {
            //                        var vendorCompanyInfoAttach = _context.VendorCompanyAttachments.Include(i => i.VendorCompanyInfo).Where(w => w.VendorCompanyInfoId == vendorCompanyInfo.Id).ToList();

            //                        foreach (var vendorAttachmentTypeItem in vendorAttachmentTypeList)
            //                        {
            //                            if (vendorAttachmentTypeItem != VendorAttachmentType.Catalogue && vendorAttachmentTypeItem != VendorAttachmentType.UnderLicensedAgreement && vendorAttachmentTypeItem != VendorAttachmentType.Certificates)
            //                            {
            //                                var vendorAttachmentTypeItemCode = double.Parse(Regex.Replace(EncryptionUtility.GenerateName(), "[^1-9]", ""));

            //                                var vendorAttachmentChildren = GetArchiveMediaForFiles(vendorAttachmentTypeItem, 0, 0, vendorCompanyInfoAttach, null, null, null, vendorAttachmentTypeItemCode);
            //                                children.Add(new FileModel
            //                                {
            //                                    CompanyType = CompanyType.Vendor,
            //                                    CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Vendor).ToString(),
            //                                    Key = vendorAttachmentTypeItemCode,
            //                                    Value = vendorAttachmentTypeItemCode,
            //                                    Title = EnumHelpers.GetNameAttribute<VendorAttachmentType>(vendorAttachmentTypeItem).ToString() + " - " + vendorAttachmentChildren.Count + " Files",
            //                                    VendorAttachmentType = vendorAttachmentTypeItem,
            //                                    ParentId = model.Key,
            //                                    Children = vendorAttachmentChildren
            //                                });
            //                            }

            //                        }

            //                    }
            //                }

            //                //VendorCertificate
            //                if (item.Title == model.Title && item.Key == guidCodeForVendorCertificate)
            //                {
            //                    var vendorCertificates = _context.VendorCompanyCertificates.Include(i=>i.VendorCompanyBaseCertificate)
            //                    .Include(i => i.VendorCompanyInfo).Where(w => w.VendorCompanyInfo.CompanyId == item.CompanyId).ToList();

            //                    int indexVendorCertificatesitem = 0;
            //                    int indexVendorCertificates = 0;

            //                    for (int c = 0; c < vendorCertificates.Count(); c++)
            //                    //foreach (var certificate in vendorCertificates)
            //                    {
            //                        var vendorCompanyCertificateFile = _context.VendorCertificateCompanyAttachments.Include(q => q.VendorCompanyCertificate)
            //                                        .OrderByDescending(o => o.CreateDate)
            //                                        .Where(s => s.VendorCompanyCertificate.VendorCompanyInfoId == vendorCertificates[c].VendorCompanyInfoId
            //                                        && s.VendorCompanyCertificate.VendorCompanyBaseCertificateId == vendorCertificates[c].VendorCompanyBaseCertificateId).ToList();
            //                        indexVendorCertificatesitem++;


            //                        if (vendorCompanyCertificateFile != null)
            //                        {
            //                            foreach (var certificate in vendorCompanyCertificateFile)
            //                            {
            //                                indexVendorCertificates++;
            //                                children.Add(new FileModel
            //                                {
            //                                    CompanyType = CompanyType.Supplier,
            //                                    FilePath = certificate.FilePath,
            //                                    CompanyTypeTitle = "VendorCertificates",
            //                                    Key = double.Parse(Regex.Replace(certificate.Id.ToString(), "[^1-9]", "")),
            //                                    Value = double.Parse(Regex.Replace(certificate.Id.ToString(), "[^1-9]", "")),
            //                                    Title = indexVendorCertificates.ToString() + " - " + (vendorCertificates[c].VendorCompanyBaseCertificate.Title)  + " - " + certificate.FileName ,
            //                                    VendorAttachmentType = certificate.VendorAttachmentType,
            //                                    ParentId = model.Key
            //                                });
            //                            }

            //                        }


            //                    }

            //                }

            //                break;
            //            case CompanyType.Supplier:

            //                //SupplierProduct
            //                if (item.Title == model.Title && item.Key == guidCodeForSupplierProduct)
            //                {
            //                    var supplierCompanyProducts = _context.SupplierCompanyProducts.Include(i=>i.Category).Include(i=>i.SupplierProductCompanyAttachments)
            //                    .Include(i => i.SupplierCompanyInfo).Where(w => w.SupplierCompanyInfo.CompanyId == item.CompanyId).ToList();

            //                    foreach (var supplierAttachmentTypeItem in supplierAttachmentTypeList)
            //                    {
            //                        if (supplierAttachmentTypeItem == SupplierAttachmentType.Catalogue || supplierAttachmentTypeItem == SupplierAttachmentType.AuthorizationLetter)
            //                        {
            //                            var supplierProductAttachmentTypeItemCode = double.Parse(Regex.Replace(EncryptionUtility.GenerateName(), "[^1-9]", ""));

            //                            var supplierProductAttachmentTypeChildren = GetArchiveMediaForFiles(0, supplierAttachmentTypeItem, 0, null, null, null, supplierCompanyProducts, supplierProductAttachmentTypeItemCode);
            //                            children.Add(new FileModel
            //                            {
            //                                CompanyType = CompanyType.Vendor,
            //                                CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Vendor).ToString(),
            //                                Key = supplierProductAttachmentTypeItemCode,
            //                                Value = supplierProductAttachmentTypeItemCode,
            //                                Title = EnumHelpers.GetNameAttribute<SupplierAttachmentType>(supplierAttachmentTypeItem).ToString() + " - " + supplierProductAttachmentTypeChildren.Count + " Files",
            //                                SupplierAttachmentType = supplierAttachmentTypeItem,
            //                                ParentId = model.Key,
            //                                Children = supplierProductAttachmentTypeChildren
            //                            });
            //                        }

            //                    }

            //                }

            //                //SupplierAttachmen
            //                if (item.Title == model.Title && item.Key == guidCodeForSupplierAttachmentType)
            //                {
            //                    var supplierCompanyInfo = _context.SupplierCompanyInfoes
            //                                     .OrderByDescending(q => q.CreateDate)
            //                                     .FirstOrDefault(q => q.CompanyId == item.CompanyId);
            //                    if (supplierCompanyInfo != null)
            //                    {

            //                        var supplierCompanyInfoAttach = _context.SupplierCompanyAttachments.Include(i => i.SupplierCompanyInfo).Where(w => w.SupplierCompanyInfoId == supplierCompanyInfo.Id).ToList();

            //                        foreach (var supplierAttachmentTypeItem in supplierAttachmentTypeList)
            //                        {
            //                            if (supplierAttachmentTypeItem != SupplierAttachmentType.Catalogue && supplierAttachmentTypeItem != SupplierAttachmentType.AuthorizationLetter)
            //                            {
            //                                var supplierAttachmentTypeItemCode = double.Parse(Regex.Replace(EncryptionUtility.GenerateName(), "[^1-9]", ""));

            //                                var supplierCompanyInfoChildren = GetArchiveMediaForFiles(0, supplierAttachmentTypeItem, 0, null, null, supplierCompanyInfoAttach, null, supplierAttachmentTypeItemCode);

            //                                children.Add(new FileModel
            //                                {
            //                                    CompanyType = CompanyType.Vendor,
            //                                    CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Vendor).ToString(),
            //                                    Key = supplierAttachmentTypeItemCode,
            //                                    Value = supplierAttachmentTypeItemCode,
            //                                    Title = EnumHelpers.GetNameAttribute<SupplierAttachmentType>(supplierAttachmentTypeItem).ToString() + " - " + supplierCompanyInfoChildren.Count + " Files",
            //                                    SupplierAttachmentType = supplierAttachmentTypeItem,
            //                                    ParentId = model.Key,
            //                                    Children = supplierCompanyInfoChildren
            //                                });
            //                            }

            //                        }
            //                    }
            //                }

            //                break;

            //        }
            //    }



            //}
            return children;

        }



        //private List<FileModel> GetArchiveMediaForFiles(VendorAttachmentType vendorAttachmentType, SupplierAttachmentType supplierAttachmentType,
        //    CompanyAttachmentType companyAttachmentType,
        //    List<VendorCompanyAttachment> vendorCompanyAttachments,
        //    List<VendorCompanyProduct> vendorCompanyProduct,
        //    List<SupplierCompanyAttachment> supplierCompanyAttachments,
        //    List<SupplierCompanyProduct> supplierCompanyProduct, double key
        //    )
        //{
        //    var children = new List<FileModel>();
        //    if (vendorCompanyAttachments != null)
        //    {
        //        int indexvendorCompanyAttachmentsItem = 0;
        //        for (int i = 0; i < vendorCompanyAttachments.Count(); i++)
        //        {
        //            if (vendorCompanyAttachments[i].VendorAttachmentType == vendorAttachmentType)
        //            {
        //                indexvendorCompanyAttachmentsItem++;

        //                children.Add(new FileModel
        //                {
        //                    CompanyType = CompanyType.Vendor,
        //                    FilePath = vendorCompanyAttachments[i].FilePath,
        //                    CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Vendor).ToString(),
        //                    Key = double.Parse(Regex.Replace(vendorCompanyAttachments[i].Id.ToString(), "[^1-9]", "")),
        //                    Value = double.Parse(Regex.Replace(vendorCompanyAttachments[i].Id.ToString(), "[^1-9]", "")),
        //                    Title = indexvendorCompanyAttachmentsItem.ToString() + " - " + vendorCompanyAttachments[i].FileName,
        //                    VendorAttachmentType = vendorCompanyAttachments[i].VendorAttachmentType,
        //                    ParentId = key,

        //                });
        //            }

        //        }
        //    }

        //    if (supplierCompanyAttachments != null)
        //    {
        //        int indexsupplierCompanyAttachmentsItem = 0;

        //        for (int i = 0; i < supplierCompanyAttachments.Count(); i++)
        //        {

        //            if (supplierCompanyAttachments[i].SupplierAttachmentType == supplierAttachmentType)
        //            {
        //                indexsupplierCompanyAttachmentsItem++;
        //                children.Add(new FileModel
        //                {
        //                    CompanyType = CompanyType.Vendor,
        //                    FilePath = supplierCompanyAttachments[i].FilePath,
        //                    CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Vendor).ToString(),
        //                    Key = double.Parse(Regex.Replace(supplierCompanyAttachments[i].Id.ToString(), "[^1-9]", "")),
        //                    Value = double.Parse(Regex.Replace(supplierCompanyAttachments[i].Id.ToString(), "[^1-9]", "")),
        //                    Title = indexsupplierCompanyAttachmentsItem.ToString() + " - " + supplierCompanyAttachments[i].FileName,
        //                    SupplierAttachmentType = supplierCompanyAttachments[i].SupplierAttachmentType,
        //                    ParentId = key,

        //                });
        //            }

        //        }
        //    }

        //    if (vendorCompanyProduct != null)
        //    {
        //        int indexUnderLicensedAgreementFileItem = 0;
        //        int indexCatalogueFileItem = 0;

        //        for (int i = 0; i < vendorCompanyProduct.Count(); i++)
        //        {
        //            var vendorProductCompanyAttachFiles = vendorCompanyProduct.FirstOrDefault(f => f.Id == vendorCompanyProduct[i].Id);

        //            //var vendorCatalogueFile = _context.VendorProductCompanyAttachments.OrderByDescending(o => o.CreateDate).FirstOrDefault(f => f.VendorAttachmentType == VendorAttachmentType.Catalogue && f.VendorCompanyProductId == vendorCompanyProduct[i].Id);
        //            if (vendorAttachmentType == VendorAttachmentType.UnderLicensedAgreement)
        //            {

        //                if (vendorProductCompanyAttachFiles != null)
        //                {


        //                    foreach (var item in vendorProductCompanyAttachFiles.VendorProductCompanyAttachments.Where(w => w.VendorAttachmentType == VendorAttachmentType.UnderLicensedAgreement))
        //                    {
        //                        indexUnderLicensedAgreementFileItem++;

        //                        var vendorUnderLicensedAgreementFile = Guid.NewGuid().ToString();

        //                        children.Add(new FileModel
        //                        {
        //                            CompanyType = CompanyType.Supplier,
        //                            FilePath = item.FilePath,
        //                            CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Vendor).ToString(),
        //                            Key = double.Parse(Regex.Replace(item.Id.ToString(), "[^1-9]", "")),
        //                            Value = double.Parse(Regex.Replace(item.Id.ToString(), "[^1-9]", "")),
        //                            Title = indexUnderLicensedAgreementFileItem.ToString() + " - " + item.FileName + " - "+(vendorCompanyProduct[i].ProductName != null ? vendorCompanyProduct[i].ProductName : vendorCompanyProduct[i].Category.CategoryName),
        //                            VendorAttachmentType = item.VendorAttachmentType,
        //                            ParentId = key
        //                        });
        //                    }





        //                }
        //            }

        //            if (vendorAttachmentType == VendorAttachmentType.Catalogue)
        //            {
        //                if (vendorProductCompanyAttachFiles != null)
        //                {


        //                    foreach (var item in vendorProductCompanyAttachFiles.VendorProductCompanyAttachments.Where(w => w.VendorAttachmentType == VendorAttachmentType.Catalogue))
        //                    {
        //                        indexCatalogueFileItem++;

        //                        var vendorUnderLicensedAgreementFile = Guid.NewGuid().ToString();

        //                        children.Add(new FileModel
        //                        {
        //                            CompanyType = CompanyType.Supplier,
        //                            FilePath = item.FilePath,
        //                            CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Vendor).ToString(),
        //                            Key = double.Parse(Regex.Replace(item.Id.ToString(), "[^1-9]", "")),
        //                            Value = double.Parse(Regex.Replace(item.Id.ToString(), "[^1-9]", "")),
        //                            Title = indexCatalogueFileItem.ToString() + " - " + item.FileName + " - " + (vendorCompanyProduct[i].ProductName != null ? vendorCompanyProduct[i].ProductName : vendorCompanyProduct[i].Category.CategoryName),
        //                            VendorAttachmentType = item.VendorAttachmentType,
        //                            ParentId = key
        //                        });
        //                    }

        //                }

        //            }


        //        }
        //    }

        //    if (supplierCompanyProduct != null)
        //    {
        //        int indexCatalogueFileItem = 0;
        //        int indexAuthorizationLetterItem = 0;

        //        for (int i = 0; i < supplierCompanyProduct.Count(); i++)
        //        {
        //            var supplierProductCompanyAttachFiles = supplierCompanyProduct.FirstOrDefault(f => f.Id == supplierCompanyProduct[i].Id );
        //            //var supplierProductCompanyAttachAuthorizationLetterFile = _context.SupplierProductCompanyAttachments.OrderByDescending(o => o.CreateDate).FirstOrDefault(f => f.SupplierCompanyProductId == supplierCompanyProduct[i].Id && f.SupplierAttachmentType == SupplierAttachmentType.AuthorizationLetter);
        //            if (supplierAttachmentType == SupplierAttachmentType.Catalogue)
        //            {
        //                if (supplierProductCompanyAttachFiles != null)
        //                {

        //                    foreach (var item in supplierProductCompanyAttachFiles.SupplierProductCompanyAttachments.Where(w=>w.SupplierAttachmentType==SupplierAttachmentType.Catalogue))
        //                    {
        //                        indexCatalogueFileItem++;

        //                        var supplierProductCodeCatalogueFile = Guid.NewGuid().ToString();

        //                        children.Add(new FileModel
        //                        {
        //                            CompanyType = CompanyType.Supplier,
        //                            FilePath = item.FilePath,
        //                            CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Supplier).ToString(),
        //                            Key = double.Parse(Regex.Replace(supplierProductCodeCatalogueFile, "[^1-9]", "")),
        //                            Value = double.Parse(Regex.Replace(supplierProductCodeCatalogueFile, "[^1-9]", "")),
        //                            Title = indexCatalogueFileItem.ToString() + " - " + item.FileName + " - " + (supplierCompanyProduct[i].ProductName !=null ? supplierCompanyProduct[i].ProductName : supplierCompanyProduct[i].Category.CategoryName ),
        //                            SupplierAttachmentType = item.SupplierAttachmentType,
        //                            ParentId = key
        //                        });
        //                    }

        //                }
        //            }
        //            if (supplierAttachmentType == SupplierAttachmentType.AuthorizationLetter)
        //            {
        //                if (supplierProductCompanyAttachFiles != null)
        //                {

        //                    foreach (var item in supplierProductCompanyAttachFiles.SupplierProductCompanyAttachments.Where(w => w.SupplierAttachmentType == SupplierAttachmentType.AuthorizationLetter))
        //                    {
        //                        indexAuthorizationLetterItem++;

        //                        var supplierProductCodeAuthorizationLetterFile = Guid.NewGuid().ToString();

        //                        children.Add(new FileModel
        //                        {
        //                            CompanyType = CompanyType.Supplier,
        //                            FilePath = item.FilePath,
        //                            CompanyTypeTitle = EnumHelpers.GetNameAttribute<CompanyType>(CompanyType.Supplier).ToString(),
        //                            Key = double.Parse(Regex.Replace(supplierProductCodeAuthorizationLetterFile, "[^1-9]", "")),
        //                            Value = double.Parse(Regex.Replace(supplierProductCodeAuthorizationLetterFile, "[^1-9]", "")),
        //                            Title = indexAuthorizationLetterItem.ToString() + " - " + item.FileName + " - " + (supplierCompanyProduct[i].ProductName != null ? supplierCompanyProduct[i].ProductName : supplierCompanyProduct[i].Category.CategoryName),
        //                            SupplierAttachmentType = item.SupplierAttachmentType,
        //                            ParentId = key
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return children;
        //}

        public async Task<List<string>> GetListOfFiles(string entity, object valueId, CompanyAttachmentType companyAttachmentType, SupplierAttachmentType supplierAttachmentType, VendorAttachmentType vendorAttachmentType)
        {
            var finalFilePath = new List<string>();

            try
            {
                //switch (entity)
                //{
                //    case "CompanyAttachment":
                //        finalFilePath = _context.CompanyAttachments.Where(f => f.CompanyId == Convert.ToInt32(valueId) && f.CompanyAttachmentType == companyAttachmentType).Select(s => s.FilePath).ToList();
                //        break;
                //    case "EvalutionAttachment":
                //        finalFilePath = _context.CompanyRequestEvalutionAttachments.Where(f => f.CompanyRequestEvalutionId == Convert.ToInt32(valueId)).Select(s => s.FilePath).ToList(); ;
                //        break;
                //    case "SupplierCompanyAttachment":
                //        finalFilePath = _context.SupplierCompanyAttachments.Where(f => f.SupplierCompanyInfoId == Convert.ToInt32(valueId) && f.SupplierAttachmentType == supplierAttachmentType).Select(s => s.FilePath).ToList();
                //        break;
                //    case "VendorCompanyAttachment":
                //        finalFilePath = _context.VendorCompanyAttachments.Where(f => f.VendorCompanyInfoId == Convert.ToInt32(valueId) && f.VendorAttachmentType == vendorAttachmentType).Select(s => s.FilePath).ToList();
                //        break;
                //    case "SupplierProductCompanyAttachment":
                //        finalFilePath = _context.SupplierProductCompanyAttachments.Where(f => f.SupplierCompanyProductId == Convert.ToInt32(valueId) && f.SupplierAttachmentType == supplierAttachmentType).Select(s => s.FilePath).ToList();
                //        break;
                //    case "VendorProductCompanyAttachment":
                //        finalFilePath = _context.VendorProductCompanyAttachments.Where(f => f.VendorCompanyProductId == Convert.ToInt32(valueId) && f.VendorAttachmentType == vendorAttachmentType).Select(s => s.FilePath).ToList();
                //        break;
                //    case "VendorCertificateCompanyAttachment":
                //        finalFilePath = _context.VendorCertificateCompanyAttachments.Where(f => f.VendorCompanyCertificateId == Convert.ToInt32(valueId) && f.VendorAttachmentType == vendorAttachmentType).Select(s => s.FilePath).ToList();
                //        break;

                //}

                //filePath = filePath.Replace(@"\", "/");

                return finalFilePath;

            }
            catch (Exception ex)
            {

                return finalFilePath;
            }
        }

        public async Task<bool> DeleteFile(FileItemDto file)
        {

            var type = typeof(Product).Assembly.GetTypes().Where(x => x.Name == file.Entity.TrimEnd('s')).First();
            var schema = type.FullName.Split('.')[2];


            var affectedRows = _context.Database.ExecuteSqlRaw(@"Delete " + schema + "." + file.Entity + " Where FilePath={0}", file.FilePath);
            return affectedRows > 0;
        }
    }
}

