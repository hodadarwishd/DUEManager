using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Demo.PL.Helpers
{
    public class DocumentSettings
    {
        public static async Task <string> UploadFile(IFormFile file ,string FolderName)
        {
            //1 - get located folder path 
            // string folderPath = "D:\\netcore_train2\\net_core_mvc\\Demo.PL\\wwwroot\\files\\"+FolderName;
            //  string folderPath=Directory.GetCurrentDirectory()+ "\\wwwroot\\files\\" + FolderName
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName);

            //  2- get filename and make it unique 
            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            //3- get file path
            string filePath=Path.Combine(folderPath, fileName);

            //4- save file as stream ==>[data per time ]
            var fileStream=new FileStream (filePath, FileMode.Create);
           await  file.CopyToAsync (fileStream);
            return fileName ;
        }

        public static void DeleteFile(string fileName,string FolderName)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName,fileName);
            if (File.Exists(folderPath))
            {
                File.Delete(folderPath);
            }
        }
    }
}
