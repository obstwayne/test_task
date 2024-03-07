using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DocsHub.Data
{
    public class ContractService
    {
        private readonly ApplicationDbContext _dbContext;
        

        public ContractService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string CalculateTotalPrice()
        {
            // Получаем данные из базы данных
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
                      
            var result = _dbContext.Database.ExecuteSqlRaw("SELECT TotalPrice from docs").ToString();
            return result;


        }
    }

}