using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.IO;

namespace PhoebeContact
{
    class XmlCare
    {
        protected XmlDocument m_doc = new XmlDocument();
        protected XmlElement m_root = null;
        private string m_file = "";

        public XmlCare(string file)
        {
            m_file = file;
            LoadConfig();
        }

        public virtual void LoadConfig()
        {
            m_doc.Load(m_file);
            m_root = m_doc.DocumentElement;
        }

        public XmlElement GetElement(string elem_name)
        {
            XmlNodeList nodes = m_root.GetElementsByTagName(elem_name);
            Debug.Assert(nodes.Count > 0);
            return nodes[0] as XmlElement;
        }

        public string GetAttribute(string elem_name, string attr_name)
        {
            XmlElement node = GetElement(elem_name);
            string attr = node.GetAttribute(attr_name);
            return attr;
        }

        public void SetAttribute(string elem_name, string attr_name, string attr_value)
        {
            XmlElement node = GetElement(elem_name);
            node.SetAttribute(attr_name, attr_value);
            Save();
        }

        public void Save()
        {
            m_doc.Save(m_file);
        }
    }
}
