using System;
using System.IO;
using System.Data;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Extensions.Logging;
using Lab3_ED1.Models;

namespace Lab3_ED1.Controllers
{
    public class HomeController : Controller
    {
        public static int asgmtPos;
        public static bool start = false;

        private IHostingEnvironment Environment;
        public HomeController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        public IActionResult Index()
        {
            try
            {
                if (!start)
                {
                    start = true;
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

                                if (Singleton.Instance1.devTable[getHashcode(fields[0])] == null)
                                {
                                    Singleton.Instance1.devTable[getHashcode(fields[0])] = new E_Arboles.PriorityQueue<int, string>(20);
                                }

                                Singleton.Instance.hashTable[getHashcode(fields[1])].Add(newAssignment);
                                Singleton.Instance1.devTable[getHashcode(fields[0])].Add(Convert.ToInt32(fields[4]), fields[1]);
                            }
                        }
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
                if (Singleton.Instance.hashTable[i] != null)
                {
                    foreach (var item in Singleton.Instance.hashTable[i])
                    {
                        writer.WriteLine(item.Name + ";" + item.Title + ";" + item.Project + ";" + item.Description + ";" + item.Priority + ";" + item.Date);
                    }
                }
            }
            writer.Close();
        }

        public IActionResult addAsgmt()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult addAsgmt(IFormCollection collection)
        {
            try
            {
                var newAssignment = new Assignment
                {
                    Name = collection["Name"].ToString().ToUpper(),
                    Title = collection["Title"],
                    Project = collection["Project"],
                    Description = collection["Description"],
                    Priority = Convert.ToInt32(collection["Priority"]),
                    Date = Convert.ToDateTime(collection["Date"])
                };

                if (Singleton.Instance.hashTable[getHashcode(collection["Title"])] == null)
                {
                    Singleton.Instance.hashTable[getHashcode(collection["Title"])] = new ELineales.Lista<Assignment>();

                    if (Singleton.Instance1.devTable[getHashcode(collection["Name"].ToString().ToUpper())] == null)
                    {
                        Singleton.Instance1.devTable[getHashcode(collection["Name"].ToString().ToUpper())] = new E_Arboles.PriorityQueue<int, string>(20);
                    }
                    Singleton.Instance.hashTable[getHashcode(collection["Title"])].Add(newAssignment);
                    Singleton.Instance1.devTable[getHashcode(collection["Name"].ToString().ToUpper())].Add(Convert.ToInt32(collection["Priority"]), collection["Title"]);
                }
                else
                {
                    ViewData["Error"] = "La tarea ya existe, por favor intente nuevamente.";
                }
                updateFile();
                return View();
            }
            catch
            {
                ViewData["Error"] = "Ingrese todos los datos pedidos";
                return View();
            }
        }

        public IActionResult devSearch()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult devSearch(IFormCollection collection)
        {
            try
            {
                if (Singleton.Instance1.devTable[getHashcode(collection["Name"].ToString().ToUpper())] == null)
                {
                    ViewData["Error"] = "El desarrollador no ha sido encontrado";
                    return View();
                }
                else if (Singleton.Instance.hashTable[getHashcode(Singleton.Instance1.devTable[getHashcode(collection["Name"].ToString().ToUpper())].Peek())] != null)
                {
                    asgmtPos = getHashcode(Singleton.Instance1.devTable[getHashcode(collection["Name"].ToString().ToUpper())].Peek());
                    return RedirectToAction(nameof(Developer));
                }
                else if (Singleton.Instance1.devTable[getHashcode(collection["Name"].ToString().ToUpper())].Peek() == null)
                {
                    ViewData["Error"] = "El desarrollador no tiene proyectos pendientes.";
                    return View();
                }
                return View();
            }
            catch
            {
                ViewData["Error"] = "Un error inesperado ha ocurrido, por favor intente nuevamente.";
                return View();
            }
        }

        public IActionResult completeAsgmt()
        {
            Singleton.Instance1.devTable[getHashcode(Singleton.Instance.hashTable[asgmtPos][0].Name)].Pop();
            Singleton.Instance.hashTable[asgmtPos] = null;
            updateFile();
            return RedirectToAction(nameof(devSearch));
        }

        public IActionResult Developer()
        {
            var viewAsgmt = Singleton.Instance.hashTable[asgmtPos][0];
            return View(viewAsgmt);
        }

        public IActionResult proyectManager()
        {
            Singleton.Instance2.proyectManager.Clear();
            for (int i = 0; i < Singleton.Instance.hashTable.Length; i++)
            {
                if (Singleton.Instance.hashTable[i] != null)
                {
                    Singleton.Instance2.proyectManager.Add(Singleton.Instance.hashTable[i][0]);
                }
            }
            for (int j = 0; j < Singleton.Instance2.proyectManager.Count(); j++)
            {
                for (int i = 0; i < Singleton.Instance2.proyectManager.Count() - 1; i++)
                {
                    if (Singleton.Instance2.proyectManager[i+1].Priority < Singleton.Instance2.proyectManager[i].Priority)
                    {
                        Assignment temp = Singleton.Instance2.proyectManager[i + 1];
                        Singleton.Instance2.proyectManager[i + 1] = Singleton.Instance2.proyectManager[i];
                        Singleton.Instance2.proyectManager[i] = temp;
                    }
                }
            }
            return View(Singleton.Instance2.proyectManager);
        }
    }
}
