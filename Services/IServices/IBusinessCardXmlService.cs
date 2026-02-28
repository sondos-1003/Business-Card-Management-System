using NeoRxTask.Entities;

namespace NeoRxTask.Services.IServices
{
    public interface IBusinessCardXmlService
    {

        string ExportToXml(List<BusinessCard> cards);
        List<BusinessCard> ImportFromXml(string xmlData);




    }
}
