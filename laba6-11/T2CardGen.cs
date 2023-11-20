using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using static System.Net.Mime.MediaTypeNames;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;

namespace laba6_11
{
    class T2CardGen
    {
        public void genDock(string templatePath, string newDocumentPath, string[] findText, string[] replaceText)
        {

            ReplaceTextInWordDocument(templatePath, newDocumentPath, findText, replaceText);
        }

        static void ReplaceTextInWordDocument(string documentPath, string newDocumentPath, string[] findText, string[] replaceText)
        {
            File.Copy(documentPath, newDocumentPath, true);

            using (WordprocessingDocument newDoc = WordprocessingDocument.Open(newDocumentPath, true))
            {
                var body = newDoc.MainDocumentPart.Document.Body;
                for (int i = 0; i < findText.Length; i++)
                {
                    ReplaceTextInElements(body, findText[i], replaceText[i]);
                }
                newDoc.MainDocumentPart.Document.Save();
                newDoc.Close();
            }
        }

        static void ReplaceTextInElements(OpenXmlElement element, string findText, string replaceText)
        {
            foreach (var childElement in element.Elements())
            {
                if (childElement is DocumentFormat.OpenXml.Wordprocessing.Run run)
                {
                    foreach (var text in run.Elements<DocumentFormat.OpenXml.Wordprocessing.Text>())
                    {
                        if (text.Text.Contains(findText))
                        {
                            text.Text = text.Text.Replace(findText, replaceText);
                        }
                    }
                }
                else if (childElement is Paragraph paragraph)
                {
                    ReplaceTextInElements(paragraph, findText, replaceText);
                }
                else if (childElement is Table table)
                {
                    foreach (var row in table.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>())
                    {
                        foreach (var cell in row.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>())
                        {
                            ReplaceTextInElements(cell, findText, replaceText);
                        }
                    }
                }
                else
                {
                    ReplaceTextInElements(childElement, findText, replaceText);
                }
            }
        }
    }
}

