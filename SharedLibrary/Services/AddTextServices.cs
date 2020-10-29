using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;

namespace SharedLibrary.Services
{
    public class AddTextServices
    {

        public static void AddTextToJson(string filepath, Person person)
        {
            try
            {
                using StreamReader reader = new StreamReader(filepath);
                var json = reader.ReadToEnd();
                reader.Close();

                if (json != string.Empty)
                {
                    var list = JsonConvert.DeserializeObject<List<Person>>(json);
                    list.Add(person);

                    var json2 = JsonConvert.SerializeObject(list);

                    using StreamWriter writer = new StreamWriter(filepath);

                    writer.WriteLine(json2);
                    writer.Close();
                }
            }
            catch
            {
                using StreamWriter writer = new StreamWriter(filepath);
                var list = new List<Person>() { person };
                var json = JsonConvert.SerializeObject(list);

                writer.Write(json);
                writer.Close();

            }



        }

        public static void AddTextToCsv(string filepath, string content)
        {
            var lines = new List<string>() { content };
            File.AppendAllLines(filepath, lines);
        }

        public static void AddTextToTxt(string filepath, string content)
        {
            var lines = new List<string>() { content };
            File.AppendAllLines(filepath, lines);
        }

        public static void WriteToFileXMLTest(string filepath, string firstname, string lastname, string age, string city)
        {
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = ("  "),
                CloseOutput = true
            };

            using XmlWriter xml = XmlWriter.Create(filepath, settings);

            xml.WriteStartElement("persons");
            xml.WriteStartElement("person");
            xml.WriteAttributeString("fullinfo", $"{firstname} {lastname} {age} {city}");
            xml.WriteEndElement();
            xml.WriteEndElement();

            xml.Flush();
        }

        
    }
}
