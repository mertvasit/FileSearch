using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.ViewModels;
using WebApplication3.Models;
using System.IO;
using WebApplication3.Controllers;
using System.Diagnostics;


namespace WebApplication3.Models
{
    public class ValidityCheck
    {
        public static FileResultListVM DirectoryTree_ValidityCheck(System.IO.DirectoryInfo root, FileResultListVM list, List<string> ForbiddenNames, List<string> ForbiddenExtensions, int Total)
         {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;


            //makes a list of all the files
            files = root.GetFiles();

            foreach (System.IO.FileInfo fi in files)
            {
                if (WordCheck.CheckingWord(Path.GetFileNameWithoutExtension(fi.Name).ToLower(), ForbiddenNames) || ForbiddenExtensions.Contains(fi.Extension))
                {
                    string md5fi = "#" + MD5Hash.CalculateMD5Hash(fi.FullName).Substring(26);
                    list.Result.Add(new KeyValuePair<FileInfo, string>(fi, md5fi));
                }
                FindFileController.ProgressBar.Count++;
                FindFileController.ProgressBar.CurrentDirectory = fi.FullName;
                FindFileController.ProgressBar.Percentage = 10 + Math.Floor(((FindFileController.ProgressBar.Count * 100) / Total)*0.9);
                //Thread.Sleep(1000);

            }
            
           
            double Checked = FindFileController.ProgressBar.Percentage;
            //checkes all the directories
            subDirs = root.GetDirectories();
            foreach (System.IO.DirectoryInfo dirInfo in subDirs)
            {
                //recursive call in order to go deep in the directories
                DirectoryTree_ValidityCheck(dirInfo, list, ForbiddenNames, ForbiddenExtensions, Total);
            }
            return list;
        }
    }
}