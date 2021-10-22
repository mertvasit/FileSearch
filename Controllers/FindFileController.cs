using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using WebApplication3.ViewModels;
using WebApplication3.Models;
using System.Security.Cryptography;
using System.Configuration;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Security.Principal;
using System.Data.SqlClient;
using System.Web.Script.Serialization;



namespace WebApplication3.Controllers
{
    public class FindFileController : Controller
    {
        public class ProgressBar
        {
            public static double Percentage { get; set; }
            public static string CurrentDirectory { get; set; }
            public static int Count { get; set; }
            public static int TotalFi { get; set; }
            public static string Status { get; set; }

        }
        // GET: FindFile
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GoBack()
        {
            return View("Index");
        }

        public ActionResult Exit()
        {
            ProgressBar.Percentage = 0;
            ProgressBar.Count = 0;
            ProgressBar.TotalFi = 0;
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            tokenSource.Cancel();
            return RedirectToAction("Index");
        }

        public ActionResult LastFiveResults(GetFileVM searchType)
        {
            FileSearchProject2Entities db = new FileSearchProject2Entities();
            FileResultListVM result = new FileResultListVM();
            result.Selected = searchType.Checked;

            switch (searchType.Checked)
            {
                case "Duplicate":
                    List<DuplicateSearch> ListFiveDup = db.DuplicateSearch.ToList();
                    ListFiveDup.Reverse();
                    result.TableResultDuplicate = ListFiveDup.Take(5).ToList();
                    result.Explanation = "בדוק קיום קבצים משוכפל";

                    result.Quantity = result.TableResultDuplicate.GroupBy(x => x.Name + x.Extension).Where(x => x.Count() > 1).OrderByDescending(x => x.Count()).ToDictionary(x => x.Key, x => x.Count());

                    result.FileMaxName = result.Quantity.First().Key;
                    result.FileMaxNumber = result.Quantity.First().Value;
                    KeyValuePair<string, int> extentionQuantity = result.TableResultDuplicate.GroupBy(x => x.Extension).OrderByDescending(x => x.Count()).ToDictionary(x => x.Key, y => y.Count()).First();
                    result.ExtensionMaxName = extentionQuantity.Key;
                    result.ExtensionMaxNumber = extentionQuantity.Value;

                    return View("PrintResult", result);

                case "IllegalName":
                    List<NameSearch> ListFiveName = db.NameSearch.ToList();
                    ListFiveName.Reverse();
                    result.TableResultName = ListFiveName.Take(5).ToList();
                    result.Explanation = "בדוק שימוש בשם לא חוקי";

                    result.Quantity = result.TableResultName.GroupBy(x => x.Name + x.Extension).Where(x => x.Count() > 1).OrderByDescending(x => x.Count()).ToDictionary(x => x.Key, x => x.Count());

                    result.FileMaxName = result.Quantity.First().Key;
                    result.FileMaxNumber = result.Quantity.First().Value;
                    KeyValuePair<string, int> extentionQuantityName = result.TableResultName.GroupBy(x => x.Extension).OrderByDescending(x => x.Count()).ToDictionary(x => x.Key, y => y.Count()).First();
                    result.ExtensionMaxName = extentionQuantityName.Key;
                    result.ExtensionMaxNumber = extentionQuantityName.Value;


                    return View("PrintResult", result);

                case "CreationDate":
                    List<DateSearch> ListFiveDate = db.DateSearch.ToList();
                    ListFiveDate.Reverse();
                    result.TableResultDate = ListFiveDate.Take(5).ToList();
                    result.DateResult = DateTime.Parse(result.TableResultDate[0].History.SearchDate);
                    result.Explanation = "קבצים לפי זמן היצירה";

                    result.Quantity = result.TableResultDate.GroupBy(x => x.Name + x.Extension).Where(x => x.Count() > 1).OrderByDescending(x => x.Count()).ToDictionary(x => x.Key, x => x.Count());
                    result.FileMaxName = result.Quantity.First().Key;
                    result.FileMaxNumber = result.Quantity.First().Value;

                    KeyValuePair<string, int> extentionQuantityDate = result.TableResultDate.GroupBy(x => x.Extension).OrderByDescending(x => x.Count()).ToDictionary(x => x.Key, y => y.Count()).First();
                    result.ExtensionMaxName = extentionQuantityDate.Key;
                    result.ExtensionMaxNumber = extentionQuantityDate.Value;

                    return View("PrintResult", result);
            }

            return View("Index");
        }

        [HttpGet]
        public JsonResult GetInfo(GetFileVM getData)
        {
            //getting the existing value of progress bar and adds the progress 
            GetFileVM sendData = new GetFileVM { };

            sendData.ResultProgBarValue = ProgressBar.Percentage;
            sendData.Directory = ProgressBar.CurrentDirectory;
            sendData.Counted = ProgressBar.Count;
            sendData.TotalFile = ProgressBar.TotalFi;
            sendData.StatusPB = ProgressBar.Status;
            sendData.IsDirectoryExists = Directory.Exists(getData.Directory);
   
            
            return Json(sendData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OpenWindowsBrowser(string clickedDirectory)
        {

            string properCommand = clickedDirectory.Replace('/', '\\');

            Process.Start("explorer.exe ", properCommand);

            return Json(JsonRequestBehavior.AllowGet);
        }


        //Calling recursive funtcion to find duplicates              
        public ActionResult SearchFile(GetFileVM inp)
        {
            DirectoryInfo root = new DirectoryInfo(inp.InputDirectory);


            FileResultListVM EmptyList = new FileResultListVM { };
            List<string> FileNamesList = new List<string> { };
            ProgressBar.Percentage = 0;
            ProgressBar.Status = "המתן ... בודק את הספרייה";
            ProgressBar.Count = 0;
            ProgressBar.TotalFi = 0;


            FileNamesList.AddRange(Directory.GetFiles(root.ToString()).ToList());
            int counter = FileNamesList.Count;
            foreach (string new_root in Directory.EnumerateDirectories(root.ToString(), ".", SearchOption.AllDirectories))
            {
                FileNamesList.AddRange(Directory.GetFiles(new_root).ToList());
                counter++;
                ProgressBar.Percentage = Math.Floor(((counter * 100) / 5000) * 0.1);
            }

            ProgressBar.TotalFi = FileNamesList.Count;

            switch (inp.Checked)
            {
                case "Duplicate":
                    List<string> hashEmptyList = new List<string> { };
                    List<string> oldNameList = new List<string> { };
                    FileResultListVM Dupp = new FileResultListVM { };
                    ProgressBar.Status = "בודק קבצים ...";


                    //Calling recursive function for all the files in the directory
                    foreach (string FileName in FileNamesList)
                    {
                        
                        FileResultListVM EmptyListDup = new FileResultListVM { };
                        List<string> emptyListDir = new List<string> { };

                        Tuple<FileResultListVM, List<string>> Dupp_adder = DuplicateCheck.DirectoryTree_DuplicateCheck(root, EmptyListDup, emptyListDir, FileName);
                                          
                        var NewNameList = Dupp_adder.Item2.Except(oldNameList);//full directory of new files that founded

                        foreach (KeyValuePair<FileInfo, string> file in Dupp_adder.Item1.Result)
                        {
                            //string md5fi = MD5Hash.CalculateMD5Hash(file.Key.FullName);
                            if (!oldNameList.Contains(file.Key.FullName))//if it doesn"t already exist in the result, add
                            {
                                Dupp.Result.Add(new KeyValuePair<FileInfo, string>(file.Key, file.Value));
                            }
                        }
                        oldNameList.AddRange(NewNameList);//update the hashfunction

                        ProgressBar.Count++;
                        ProgressBar.CurrentDirectory = FileName;
                        ProgressBar.Percentage = 10 + Math.Floor(((ProgressBar.Count * 100) / FileNamesList.Count)*0.9);
                    }

                    Dupp.Quantity = Dupp.Result.GroupBy(x => x.Key.Name).Where(x => x.Count() > 1).ToDictionary(x => x.Key, x => x.Count());

                    Dupp.FileMaxName = Dupp.Result.GroupBy(x => x.Key.Name).OrderByDescending(x => x.Count()).FirstOrDefault().Key;

                    Dupp.Explanation = "בדוק קיום קבצים משוכפל";

                    Dupp.Selected = inp.Checked;

                    var ExtListDupp = Dupp.Result.GroupBy(s => s.Key.Extension).OrderByDescending(s => s.Count());

                    if (Dupp.Quantity.Count() == 0)
                    {
                        Dupp.FileMaxNumber = 0;
                    }
                    else
                    {
                        Dupp.FileMaxNumber = Dupp.Quantity.Values.Max();
                    }

                    Dupp.ExtensionMaxName = ExtListDupp.First().Key;

                    Dupp.ExtensionMaxNumber = ExtListDupp.First().Count();

                    FileSearchProject2Entities dbDuplicateSearch = new FileSearchProject2Entities();

                    History recordHistoryDup = new History
                    {
                        Executer = Environment.UserName,
                        Path = inp.InputDirectory,
                        SearchDate = DateTime.Now.ToString()
                    };

                    foreach (var item in Dupp.Result)
                    {
                        DuplicateSearch objDate = new DuplicateSearch()
                        {
                            HistoryID = recordHistoryDup.Id,
                            Directory = item.Key.Directory.ToString(),
                            Name = Path.GetFileNameWithoutExtension(item.Key.Name),
                            Extension = item.Key.Extension,
                            CreationDate = item.Key.CreationTime,
                            UserID = System.IO.File.GetAccessControl(item.Key.DirectoryName).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString(),
                            Size = Convert.ToInt32(item.Key.Length)
                        };
                        Dupp.TableResultDuplicate.Add(objDate);
                    };

                    dbDuplicateSearch.DuplicateSearch.AddRange(Dupp.TableResultDuplicate.OrderBy(a => a.Name));
                    dbDuplicateSearch.History.Add(recordHistoryDup);
                    dbDuplicateSearch.SaveChanges();

                    return View("PrintResult", Dupp);

                case "IllegalName":
                    List<string> ForbiddenNames = new List<string> { "סודי ביותר", "סיסמה", "סיסמא", "סיסמאות", "סיסמהות", "password", "secret" };
                    List<string> ForbiddenExtensions = new List<string> { ".wav", ".wave", ".aiff", ".aif", ".pcm", ".flac", ".wma", "mp3", ".mp4", ".avi", ".mov", ".wmv" };
                    ProgressBar.Status = "טעינה...";
                    FileResultListVM IllegalNames = ValidityCheck.DirectoryTree_ValidityCheck(root, EmptyList, ForbiddenNames, ForbiddenExtensions, FileNamesList.Count);
                    IllegalNames.Selected = inp.Checked;
                    IllegalNames.Explanation = "בדוק שימוש בשם לא חוקי";

                    IllegalNames.Quantity = IllegalNames.Result.GroupBy(x => x.Key.Name).Where(x => x.Count() > 1).ToDictionary(x => x.Key, x => x.Count());

                    var ExtListIllegal = IllegalNames.Result.GroupBy(s => s.Key.Extension).OrderByDescending(s => s.Count());

                    if (IllegalNames.Quantity.Count() == 0)
                    {
                        IllegalNames.FileMaxNumber = 0;
                        IllegalNames.FileMaxName = "";
                    }
                    else
                    {
                        IllegalNames.FileMaxNumber = IllegalNames.Quantity.Values.Max();
                        IllegalNames.FileMaxName = IllegalNames.Result.GroupBy(x => x.Key.Name).OrderByDescending(x => x.Count()).FirstOrDefault().Key;
                    }

                    if(IllegalNames.Result.Count != 0)
                    {
                        IllegalNames.ExtensionMaxName = ExtListIllegal.First().Key;
                        IllegalNames.ExtensionMaxNumber = ExtListIllegal.First().Count();
                    }


                    FileSearchProject2Entities dbNameSearch = new FileSearchProject2Entities();

                    History recordHistoryNameSearch = new History
                    {
                        Executer = Environment.UserName,
                        Path = inp.InputDirectory,
                        SearchDate = DateTime.Now.ToString()
                    };

                    foreach (var item in IllegalNames.Result)
                    {
                        NameSearch obj = new NameSearch()
                        {
                            HistoryID = recordHistoryNameSearch.Id,
                            Directory = item.Key.Directory.ToString(),
                            Name = Path.GetFileNameWithoutExtension(item.Key.Name),
                            Extension = item.Key.Extension,
                            CreationDate = item.Key.CreationTime,
                            UserID = System.IO.File.GetAccessControl(item.Key.DirectoryName).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString(),
                            Size = Convert.ToInt32(item.Key.Length)
                        };
                        IllegalNames.TableResultName.Add(obj);
                    };

                    dbNameSearch.NameSearch.AddRange(IllegalNames.TableResultName.OrderBy(a => a.Name));
                    dbNameSearch.History.Add(recordHistoryNameSearch);
                    dbNameSearch.SaveChanges();


                    return View("PrintResult", IllegalNames);

                case "CreationDate":
                    ProgressBar.Status = "טעינה...";
                    FileResultListVM DateFiles = DateCheck.DirectoryTree_DateCheck(root, EmptyList, inp.BeforeDate, FileNamesList.Count);
                    DateFiles.Explanation = "קבצים לפי זמן היצירה";
                    DateFiles.Selected = inp.Checked;
                    DateFiles.DateResult = inp.BeforeDate;

                    DateFiles.Quantity = null;
                    DateFiles.Quantity = DateFiles.Result.GroupBy(x => x.Key.Name).Where(x => x.Count() > 1).ToDictionary(x => x.Key, x => x.Count());




                    if (DateFiles.Quantity.Count() == 0)
                    {
                        DateFiles.FileMaxNumber = 0;
                        DateFiles.FileMaxName = "אין קובץ חוזר ונשנה";
                    }
                    else
                    {
                        DateFiles.FileMaxNumber = DateFiles.Quantity.Values.Max();
                        DateFiles.FileMaxName = DateFiles.Result.GroupBy(x => x.Key.Name).OrderByDescending(x => x.Count()).FirstOrDefault().Key;
                    }
                    var ExtListDate = DateFiles.Result.GroupBy(s => s.Key.Extension).OrderByDescending(s => s.Count());

                    if (DateFiles.Result.Count != 0)
                    {
                        DateFiles.ExtensionMaxName = ExtListDate.First().Key;
                        DateFiles.ExtensionMaxNumber = ExtListDate.First().Count();
                    }


                    FileSearchProject2Entities dbDateSearch = new FileSearchProject2Entities();

                    History recordHistoryDateSearch = new History
                    {
                        Executer = Environment.UserName,
                        Path = inp.InputDirectory,
                        SearchDate = DateTime.Now.ToString()
                    };

                    foreach (var item in DateFiles.Result)
                    {
                        DateSearch objDate = new DateSearch()
                        {
                            HistoryID = recordHistoryDateSearch.Id,
                            Directory = item.Key.Directory.ToString(),
                            Name = Path.GetFileNameWithoutExtension(item.Key.Name),
                            Extension = item.Key.Extension,
                            CreationDate = item.Key.CreationTime,
                            UserID = System.IO.File.GetAccessControl(item.Key.DirectoryName).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString(),
                            Size = Convert.ToInt32(item.Key.Length)
                        };
                        DateFiles.TableResultDate.Add(objDate);
                    };

                    dbDateSearch.DateSearch.AddRange(DateFiles.TableResultDate.OrderBy(a => a.Name));
                    dbDateSearch.History.Add(recordHistoryDateSearch);
                    dbDateSearch.SaveChanges();

                    return View("PrintResult", DateFiles);

            }
            return View("Index");
        }
    }

}