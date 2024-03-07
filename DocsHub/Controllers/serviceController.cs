using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DocsHub.Data;
using DocsHub.Models;
using DocsHub.Services;
using Npgsql;
using System.Xml.Linq;


namespace DocsHub.Controllers
{
    public class serviceController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public serviceController(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IActionResult> Calculate()
        {
            var docsList = _dbContext.docs.ToList();
            // Создаем XML-документ
            XDocument xmlDoc = new XDocument(
                new XElement("Contract",
                    new XElement("Items",
                        docsList.Select(doc =>
                            new XElement("Item",
                                new XElement("Number", doc.Number),
                                new XElement("Date", doc.Date),
                                new XElement("Title", doc.Title),
                                new XElement("Price", doc.Price)
                            )
                        )
                    )
                )
            );
            //Преобразование XML в строку:
            var xmlData = xmlDoc.ToString();
            //Создание параметра для хранения XML - документа:
            var xmlParam = new NpgsqlParameter("docs_data", NpgsqlTypes.NpgsqlDbType.Xml) { Value = xmlData };
            //вызов процедуры из базы данных
            _dbContext.Database.ExecuteSqlRaw("CALL public.prices_sum(@docs_data)", xmlParam);
            Console.WriteLine("процесс пошел");

            //var result = _dbContext.Database.ExecuteSqlRaw("SELECT TotalPrice from docs").ToString();

            return View(await _dbContext.docs.ToListAsync());


        }

        }
}
