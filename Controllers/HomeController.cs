using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Lab3_ED1.Models;

namespace Lab3_ED1.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment Environment;
        public HomeController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }


        public IActionResult Index()
        {
            try
            {
                using (TextFieldParser txtParser = new TextFieldParser("Storage_File.txt"))
                {
                    txtParser.CommentTokens = new string[] { "#" };
                    txtParser.SetDelimiters(new string[] { ";" });
                    txtParser.HasFieldsEnclosedInQuotes = true;

                    while (!txtParser.EndOfData)
                    {
                        string[] fields = txtParser.ReadFields();

                        var newAssignment = new Assignment
                        {
                            Name = fields[0],
                            Title = fields[1],
                            Project = fields[2],
                            Description = fields[3],
                            Priority = Convert.ToInt32(fields[4]),
                            Date = Convert.ToDateTime(fields[5])
                        };

                        if (Singleton.Instance.hashTable[getHashcode(fields[1])] == null)
                        {
                            Singleton.Instance.hashTable[getHashcode(fields[1])] = new ELineales.Lista<Assignment>();
                        }
                            Project = fields[1],
                            Description = fields[2],
                            Priority = Convert.ToInt32(fields[3]),
                            Date = Convert.ToDateTime(fields[4])
                        };  
                        Singleton.Instance.hashTable[getHashcode(fields[1])].Add(newAssignment);
                    }
                }
                return View();
            }
            catch
            {
                using (new FileStream("Storage_File.txt", FileMode.CreateNew)) { }
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public int getHashcode(string key)
        {
            byte[] code = Encoding.ASCII.GetBytes(key);
            int hash = 0;
            for (int i = 0; i < code.Count(); i++)
            {
                hash += Convert.ToInt32(code[i]);
            }
            hash = (hash * code.Count()) % 20;
            return hash;
        }

        public void updateFile()
        {
            System.IO.File.WriteAllText("Storage_File.txt", String.Empty);
            StreamWriter writer = new StreamWriter("Storage_File.txt", true);
            for (int i = 0; i < Singleton.Instance.hashTable.Length; i++)
            {
                if (Singleton.Instance.hashTable[i].Count() > 0)
                {
                    foreach (var item in Singleton.Instance.hashTable[i])
                    {
                        writer.WriteLine(item.Name + ";" + item.Title + ";" + item.Project + ";" + item.Description + ";" + item.Priority + ";" + item.Date);
                    }
                }
            }
            writer.Close();
        }

        public IActionResult AddAssignment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAssignment(IFormCollection collection)
        {
            int p = 0;
            if (Convert.ToString(collection["Priority"]) == "Alta")
            {
                p = 0;
            }
            else if(Convert.ToString(collection["Priority"]) == "Media")
            {
                p = 1;
            }
            else
            {
                p = 2;
            }
            var newAssignment = new Assignment
            {
                Name = collection["Name"],
                Description = collection["Description"],
                Date = Convert.ToDateTime(collection["Date"]),
                Project = Convert.ToString(collection["Project"]),
                Priority = p,
                Task = collection["Task"]
            };

            if (Singleton.Instance.hashTable[getHashcode(collection["Name"])] == null)
            {
                Singleton.Instance.hashTable[getHashcode(collection["Name"])] = new ELineales.Lista<Assignment>();
            }
            Singleton.Instance.hashTable[getHashcode(collection["Name"])].Add(newAssignment);
            return View();
        }
    }
}
