using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
    public class GetFileVM
    {
        [Required]
        public string InputDirectory { get; set; }

        public string Checked { get; set; }

        [DataType(DataType.Date)]
        public DateTime BeforeDate { get; set; }

        public int CurrentProgBarValue { get; set; }
        public double ResultProgBarValue { get; set; }

        public string Directory { get; set; }
        public bool IsDirectoryExists { get; set; }

        public int Counted { get; set; }
        public int TotalFile { get; set; }
        public string StatusPB { get; set; }

        public List<NameSearch> TableResultName = new List<NameSearch>();
        public List<DateSearch> TableResultDate = new List<DateSearch>();
        public List<DuplicateSearch> TableResultDuplicate = new List<DuplicateSearch>();

        public List<KeyValuePair<string, int>> DenemeListe { get; set; }

    }
}