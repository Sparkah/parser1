using System;
using System.IO;
using System.Net;

namespace htmlparser
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //Засекаем время начала парсинга
            DateTime start = DateTime.Now;

            //Загружаем html код с сайта
            WebClient client = new WebClient();
            string htmlCode;
            htmlCode = client.DownloadString("https://www.marathonbet.ru/su/?cpcids=all&liveTab=26418&csids=45356,43658,22723,1372932,139722,433799,414329,23690&cplcids=68957,296528,461946");

            //Попытка парсить сайт
            try
            {
                ParseHTML parser = new ParseHTML();
                parser.Parse(htmlCode, start);

            }
            //Если не парсит то выдает ошибку
            //Можно прописать логику обхода защиты от парсинга
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        Console.WriteLine("HTTP Status Code: " + (int)response.StatusCode);
                    }
                }
            }
        }
    }

    //Класс приобразования html в читаемые данные
    //Можно сделать похожие классы для парсинга названия команды и организовать многопоточность для оптимизации скорости
    class ParseHTML
    {
        //Получаем данные коэффициента ставки.
        public void Parse(string html, DateTime start)
        {
            string[] splitParseHtml = html.Split(new string[] { "data-selection-price=" }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string parse in splitParseHtml)
            {
                var reader = new StringReader(parse);
                string first = reader.ReadLine();
                StructureHTML structureHTML = new StructureHTML();
                structureHTML.Struct(first);
            }
            CalculateTimeTaken(start);
        }

        //Считаем сколько времени занял парсинг
        private void CalculateTimeTaken(DateTime start)
        {
            DateTime end = DateTime.Now;
            Console.WriteLine();
            Console.WriteLine("Start parsing at " + start);
            Console.WriteLine("End parsing at " + end);
            Console.WriteLine("Parsing time: " + (end - start));
        }
    }

    //Структурируем html для читаемости и передачи данных в БД
    class StructureHTML
    {
        public void Struct(string first)
        {
            string[] sorter = first.Split('"');
            for(int i = 0; i<sorter.Length;i++)
            {
                Console.Write(sorter[i]);
            }
            Console.WriteLine();
        }
    }
}