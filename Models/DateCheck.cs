using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.ViewModels;
using WebApplication3.Controllers;
using System.IO;
using System.Threading;

namespace WebApplication3.Models
{
    public class DateCheck
    {
        public static FileResultListVM DirectoryTree_DateCheck(System.IO.DirectoryInfo root, FileResultListVM list, DateTime date, int Total)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            //makes a list of all the files
            files = root.GetFiles();

            foreach (System.IO.FileInfo fi in files)
            {
                //File.GetCreationTime(fi.DirectoryName) <= date
                if (fi.CreationTime <= date)
                {
                    string md5fi = "#" + MD5Hash.CalculateMD5Hash(fi.FullName).Substring(26);
                    list.Result.Add(new KeyValuePair<FileInfo, string>(fi, md5fi));
                }
                FindFileController.ProgressBar.Count++;
                FindFileController.ProgressBar.CurrentDirectory = fi.FullName;
                FindFileController.ProgressBar.Percentage = 10 + Math.Floor(((FindFileController.ProgressBar.Count * 100) / Total) * 0.9);
                //Thread.Sleep(1000);
            }

            //checkes all the directories
            subDirs = root.GetDirectories();
            foreach (System.IO.DirectoryInfo dirInfo in subDirs)
            {
                DirectoryTree_DateCheck(dirInfo, list, date, Total);//recursive call in order to go deep in the directories
            }
            return list;

        }
    }
}