using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.ViewModels;
using WebApplication3.Models;
using System.IO;
using System;
using System.Security.Cryptography;

namespace WebApplication3.Models
{
    public class DuplicateCheck
    {
        public static Tuple<FileResultListVM, List<string>> DirectoryTree_DuplicateCheck(System.IO.DirectoryInfo root, FileResultListVM list, List<string> listFullName, string fileName)
        {

            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            FileInfo fileNameFI = new FileInfo($"{fileName}");

            //makes a list of all the files
            files = root.GetFiles();
            
            foreach (System.IO.FileInfo fi in files)
            {
                string md5fi ="#" + MD5Hash.CalculateMD5Hash(fi.FullName).Substring(26);//calculating MD5 HashFunct of iterative file
                if (Path.GetFileNameWithoutExtension(fi.Name.ToLower()) == Path.GetFileNameWithoutExtension(fileNameFI.Name.ToLower()) && fi.FullName != fileNameFI.FullName /*&& md5fi != md5fileName && dict.Result.ContainsValue(md5fi) == false*/)
                {
                    list.Result.Add(new KeyValuePair<FileInfo, string>(fi, md5fi));
                    listFullName.Add(fi.FullName);
                }
            }

            //checkes all the directories
            subDirs = root.GetDirectories();
            foreach (System.IO.DirectoryInfo dirInfo in subDirs)
            {
                DirectoryTree_DuplicateCheck(dirInfo, list, listFullName, fileName);//recursive call in order to go deeper directories
            }
            
            return Tuple.Create(list, listFullName);   
        }
    }

}