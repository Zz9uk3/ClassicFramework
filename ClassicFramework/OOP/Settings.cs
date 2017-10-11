using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ClassicFramework.OOP
{
    internal static class Settings
    {
        internal static void Recreate(string parPathToWoW)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode settingsNode = doc.CreateElement("Settings");
            doc.AppendChild(settingsNode);
            XmlNode pathNode = doc.CreateElement("Path");
            pathNode.InnerText = parPathToWoW;
            XmlNode autolootNode = doc.CreateElement("AutoLoot");
            autolootNode.InnerText = "false";
            settingsNode.AppendChild(pathNode);
            settingsNode.AppendChild(autolootNode);
            doc.Save(".\\Settings.xml");
        }
    }
}
