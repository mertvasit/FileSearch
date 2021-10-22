using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using WebApplication3.Models;

namespace WebApplication3.ViewModels
{
    public class FileResultListVM
    {
        public List<KeyValuePair<FileInfo, string>> Result = new List<KeyValuePair<FileInfo, string>>();
        public List<NameSearch> TableResultName = new List<NameSearch>();
        public List<DateSearch> TableResultDate = new List<DateSearch>();
        public List<DuplicateSearch> TableResultDuplicate = new List<DuplicateSearch>();

        public Dictionary<string, int> Quantity { get; set; }
        public string FileMaxName { get; set; }
        public int FileMaxNumber { get; set; }
        public int ExtensionMaxNumber { get; set; }
        public string ExtensionMaxName { get; set; }

        public string Selected { get; set; }
        public string Explanation { get; set; }

        public DateTime DateResult { get; set; }
    }
}