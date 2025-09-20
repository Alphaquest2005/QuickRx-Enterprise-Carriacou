using System;
using System.Xml;

namespace QuickBooks
{
    public class ItemInventoryViewModel
    {
        
        public static XmlDocument BuildModifiedItemInventoryQuery(int days)
        {
            
            XmlDocument inputXMLDoc = new XmlDocument();
            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbposxml", "version=\"1.0\""));
            XmlElement qbXML = inputXMLDoc.CreateElement("QBPOSXML");
            inputXMLDoc.AppendChild(qbXML);
            XmlElement qbXMLMsgsRq = inputXMLDoc.CreateElement("QBPOSXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
            XmlElement inventoryQueryRq = inputXMLDoc.CreateElement("ItemInventoryQueryRq");
            qbXMLMsgsRq.AppendChild(inventoryQueryRq);
            inventoryQueryRq.SetAttribute("requestID", "1");


            XmlElement itemNumberRange = inputXMLDoc.CreateElement("TimeModifiedRangeFilter");
            itemNumberRange.AppendChild(inputXMLDoc.CreateElement("FromTimeModified")).InnerText = DateTime.Now.AddMinutes(-45).ToString("yyyy-MM-dd");
            itemNumberRange.AppendChild(inputXMLDoc.CreateElement("ToTimeModified")).InnerText = DateTime.Now.ToString("yyyy-MM-dd");
            inventoryQueryRq.AppendChild(itemNumberRange);

            return inputXMLDoc;
        }

        public static XmlDocument BuildCreatedItemInventoryQuery(int days)
        {
            try
            {


                XmlDocument inputXMLDoc = new XmlDocument();
                inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
                inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbposxml", "version=\"1.0\""));
                XmlElement qbXML = inputXMLDoc.CreateElement("QBPOSXML");
                inputXMLDoc.AppendChild(qbXML);
                XmlElement qbXMLMsgsRq = inputXMLDoc.CreateElement("QBPOSXMLMsgsRq");
                qbXML.AppendChild(qbXMLMsgsRq);
                qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
                XmlElement inventoryQueryRq = inputXMLDoc.CreateElement("ItemInventoryQueryRq");
                qbXMLMsgsRq.AppendChild(inventoryQueryRq);
                inventoryQueryRq.SetAttribute("requestID", "1");


                XmlElement itemNumberRange = inputXMLDoc.CreateElement("TimeCreatedRangeFilter");
                itemNumberRange.AppendChild(inputXMLDoc.CreateElement("FromTimeCreated")).InnerText =
                    DateTime.Now.AddMinutes(-45).ToString("yyyy-MM-dd");
                itemNumberRange.AppendChild(inputXMLDoc.CreateElement("ToTimeCreated")).InnerText =
                    DateTime.Now.ToString("yyyy-MM-dd");
                inventoryQueryRq.AppendChild(itemNumberRange);

                return inputXMLDoc;
            }
            catch (Exception)
            {

                throw;
            }

        }



        public static XmlDocument BuildItemInventoryQueryRq(int FromItemNumber, int ToItemNumber)
        {
            XmlDocument inputXMLDoc = new XmlDocument();
            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbposxml", "version=\"1.0\""));
            XmlElement qbXML = inputXMLDoc.CreateElement("QBPOSXML");
            inputXMLDoc.AppendChild(qbXML);
            XmlElement qbXMLMsgsRq = inputXMLDoc.CreateElement("QBPOSXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
            XmlElement inventoryQueryRq = inputXMLDoc.CreateElement("ItemInventoryQueryRq");
            qbXMLMsgsRq.AppendChild(inventoryQueryRq);
            inventoryQueryRq.SetAttribute("requestID", "1");

            
            XmlElement itemNumberRange = inputXMLDoc.CreateElement("ItemNumberRangeFilter");
            itemNumberRange.AppendChild(inputXMLDoc.CreateElement("FromItemNumber")).InnerText = FromItemNumber.ToString();
            itemNumberRange.AppendChild(inputXMLDoc.CreateElement("ToItemNumber")).InnerText = ToItemNumber.ToString();
            inventoryQueryRq.AppendChild(itemNumberRange);

            return inputXMLDoc;
        }

        public static XmlDocument BuildItemInventoryQueryRq(string listId)
        {
            XmlDocument inputXMLDoc = new XmlDocument();
            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbposxml", "version=\"1.0\""));
            XmlElement qbXML = inputXMLDoc.CreateElement("QBPOSXML");
            inputXMLDoc.AppendChild(qbXML);
            XmlElement qbXMLMsgsRq = inputXMLDoc.CreateElement("QBPOSXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
            XmlElement inventoryQueryRq = inputXMLDoc.CreateElement("ItemInventoryQueryRq");
            qbXMLMsgsRq.AppendChild(inventoryQueryRq);
            inventoryQueryRq.SetAttribute("requestID", "1");


            //XmlElement itemNumberRange = inputXMLDoc.CreateElement("ItemNumberRangeFilter");
            //itemNumberRange.AppendChild(inputXMLDoc.CreateElement("FromItemNumber")).InnerText = FromItemNumber.ToString();
            //itemNumberRange.AppendChild(inputXMLDoc.CreateElement("ToItemNumber")).InnerText = ToItemNumber.ToString();

            inventoryQueryRq.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = listId;

            return inputXMLDoc;
        }
    }
}
