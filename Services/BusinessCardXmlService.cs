using NeoRxTask.Entities;
using NeoRxTask.Services.IServices;
using System.IO;
using System.Xml.Serialization;

namespace NeoRxTask.Services
{
    public class BusinessCardXmlService: IBusinessCardXmlService
    {
        public string ExportToXml(List<BusinessCard> cards){

            var serializer = new XmlSerializer(typeof(List<BusinessCard>));
            using var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, cards);
            return stringWriter.ToString();

        }

        public List<BusinessCard> ImportFromXml(string xmlData)
        {
         var serializer = new XmlSerializer(typeof(List<BusinessCard>));
            using var stringReader= new StringReader(xmlData);
            return (List<BusinessCard>)serializer.Deserialize(stringReader);

        }


    }
}
