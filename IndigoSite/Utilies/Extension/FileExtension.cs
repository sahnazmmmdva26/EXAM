using System.Text.RegularExpressions;

namespace IndigoSite.Utilies.Extension
{
    public static class FileExtension
    {
        public static string CheckValidate(this IFormFile? formFile,string type,int kb)
        {
            if(formFile.Length>kb*1024)
            {
                return "faylin olcusu boyukdur";
            }  
            if(!formFile.ContentType.Contains(type))
            {
                return "faylin tipi uygun deyil";
            }
            return "";
        }
        public static string ChangeFileName(this string filename)
        {
            string newname = Guid.NewGuid() + filename;
            return newname;
        }
        public static string SaveFile(this IFormFile file,string path) 
        {
            string filename = file.FileName.ChangeFileName();
            using (FileStream fs = new FileStream(Path.Combine(path, filename), FileMode.Create))
            {
                file.CopyTo(fs);
            }
            return filename;
        }
        public static void DeleteFile(this string filename,string path,string folder)
        
        {
            string newpath= Path.Combine(path,folder, filename);
            if(File.Exists(newpath))
            {
                File.Delete(newpath);
            }

        }
    }
}
