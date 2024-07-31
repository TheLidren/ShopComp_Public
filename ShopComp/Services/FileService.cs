using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ShopComp.Services
{
    public class FileService
    {

        public string EmailUser(IWebHostEnvironment _appEnvironment)
        {
            StreamReader reader = new(_appEnvironment.WebRootPath + "\\file.txt", System.Text.Encoding.UTF8);
            string str = reader.ReadLine();
            reader.Close();
            return str;
        }

    }
}
