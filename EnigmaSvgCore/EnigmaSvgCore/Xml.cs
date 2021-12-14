using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EnigmaSvgCore
{
    public class Xml
    {
        public string[] Lines { get; set; }

        public Xml()
        { 
        
        }

        public Xml(string[] xmlLines)
        {
            Lines = new string[xmlLines.Length];
            Array.Copy(xmlLines, Lines, Lines.Length);
        }

        public Xml(string xmlText)
        {
            Lines = xmlText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        public static Xml ReadFile(string filename)
        {
            return new Xml(File.ReadAllLines(filename));
        }

        public Xml(byte[] fileBytes)
        {
            using (StreamReader stream = new StreamReader(new MemoryStream(fileBytes)))
            {
                Lines = stream.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            }
        }

        public void InsertStyleContent(string content)
        {
            int styleClosingLineIndex = Array.FindIndex(Lines, l => l.Contains("</style>"));
            string styleString = Lines.Where(l => l.Contains("<style>")).FirstOrDefault();

            if (styleClosingLineIndex != -1 && !string.IsNullOrEmpty(styleString))
            {
                string[] contentLines = content.Split('\n');
                // вставляем TAB-символы в начало каждой новой строки добавляемого контента
                int tabsCount = Regex.Matches(styleString, @"^[\t]*").Count;
                
                if (tabsCount > 0)
                {
                    string preLineTabs = "";
                    for (int i = 0; i < tabsCount; i++)
                    {
                        preLineTabs += "\t";
                    }
                    for (int i = 0; i < contentLines.Length; i++)
                    {
                        contentLines[i] = preLineTabs + contentLines[i];
                    }
                }
                string[] newXmlLines = new string[Lines.Length + contentLines.Length]; // новый xml с добавленными данными
                
                Array.Copy(Lines, newXmlLines, styleClosingLineIndex); // копируем все строки, идущие до '</style>'
                Array.Copy(contentLines, 0, newXmlLines, styleClosingLineIndex, contentLines.Length); // копируем строки добавляемого контента
                Array.Copy(Lines, styleClosingLineIndex, newXmlLines, styleClosingLineIndex + contentLines.Length, Lines.Length - styleClosingLineIndex); // копируем оставшиеся строки
                Lines = new string[newXmlLines.Length];
                Array.Copy(newXmlLines, Lines, Lines.Length); // заменяем старые строки Xml новыми
            }
        }

        public void SaveAsFile(string filename)
        {
            File.WriteAllLines(filename, Lines);
        }

        public void InsertLines(int startLineIndex, string[] contentLines, int leadingTabs = 0)
        {
            // вставляем TAB-символы в начало каждой новой строки добавляемого контента
            
            if (leadingTabs > 0)
            {
                string preLineTabs = "";
                for (int i = 0; i < leadingTabs; i++)
                {
                    preLineTabs += "\t";
                }
                for (int i = 0; i < contentLines.Length; i++)
                {
                    contentLines[i] = preLineTabs + contentLines[i];
                }
            }
            string[] newXmlLines = new string[Lines.Length + contentLines.Length]; // новый xml с добавленными данными

            Array.Copy(Lines, newXmlLines, startLineIndex); // копируем все строки, идущие до места вставки
            Array.Copy(contentLines, 0, newXmlLines, startLineIndex, contentLines.Length); // копируем строки добавляемого контента
            Array.Copy(Lines, startLineIndex, newXmlLines, startLineIndex + contentLines.Length, Lines.Length - startLineIndex); // копируем оставшиеся строки
            Lines = new string[newXmlLines.Length];
            Array.Copy(newXmlLines, Lines, Lines.Length); // заменяем старые строки Xml новыми
        }
    }
}
