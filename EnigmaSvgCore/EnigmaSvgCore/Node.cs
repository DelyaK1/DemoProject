using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EnigmaSvgCore
{
    public class Node
    {
        public enum NodeType
        {
            XmlDeclaration,
            DocType,
            Opening,
            Closing,
            Single,
            Content,
            Comment,
            CDATA
        }
        public string Name { get; set; }
        public NodeType Type { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public string Value { get; set; } = null;
        public int Depth { get; set; }
        public int Order { get; set; }
        public Node()
        {
        }

        public Node(Node node)
        {
            Name = node.Name;
            Type = node.Type;
            Attributes = new Dictionary<string, string>(node.Attributes);
            Value = node.Value;
            Depth = node.Depth;
            Order = node.Order;
        }

        public bool Hidden()
        {
            bool Hidden = false;
            if (Type == NodeType.Opening)
            {
                if (Name == "text")
                {
                    try
                    {
                        if (Regex.IsMatch(Attributes["style"], "(fill:#ffffff)"))
                        {
                            Hidden = true;
                        }
                    }
                    catch { }
                }
                if (Name == "path")
                {
                    try
                    {
                        if (Attributes["stroke"] == "#ffffff")
                        {
                            Hidden = true;
                        }
                    }
                    catch { }
                }
            }
            
            return Hidden;
        }

        public bool IsAttributeExist(string attributeName)
        {
            bool attributeExist = false;
            if (Attributes.ContainsKey(attributeName))
            {
                attributeExist = true;
            }
            return attributeExist;
        }

        public bool RemoveAttribute(string attributeName)
        {
            bool success = false;
            if (IsAttributeExist(attributeName))
            {
                Attributes.Remove(attributeName);
                success = true;
            }

            return success;
        }

        public bool AddAttribute(string attributeName, string attributeValue)
        {
            bool success = true;
            if (IsAttributeExist(attributeName))
            {
                success = false;
            }
            else
            {
                Attributes.Add(attributeName, attributeValue);
            }
            return success;
        }

        public bool HasAttributes()
        {
            bool hasAttributes = false;
            if (Attributes.Count > 0)
            {
                hasAttributes = true;
            }
            return hasAttributes;
        }

        public void SetTextColor(string hexColor = "#FF0000")
        {
            if (IsAttributeExist("style"))
            {
                Attributes["style"] = Regex.Replace(Attributes["style"], @"(#)[A-Fa-f0-9]{6}", hexColor);
            }
        }

        public void SetPathColor(string hexColor = "#FF0000")
        {
            if (IsAttributeExist("stroke"))
            {
                Attributes["stroke"] = hexColor;
            }

            if (IsAttributeExist("fill"))
            {
                if (Attributes["fill"] != "#FFFFFF" && Attributes["fill"] !="none") // если заливка не белая
                {
                    Attributes["fill"] = hexColor;
                }
            }
        }

        
    }
}
