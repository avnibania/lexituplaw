using Alifery.Aligo.DocumentParser.Models;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Alifery.Aligo.DocumentParser.External.Syncfusion
{
    public class ExtractWord : IExtractDoc
    {
        public async Task<DocumentContentModel> ExtractContent(Stream fileStreamInput)
        {
            WordDocument document = new WordDocument(fileStreamInput, FormatType.Word2007);
            foreach (WSection section in document.Sections)

            {

                WTextBody sectionBody = section.Body;

                IterateTextBody(sectionBody);

                WHeadersFooters headersFooters = section.HeadersFooters;

                IterateTextBody(headersFooters.OddHeader);

                IterateTextBody(headersFooters.OddFooter);

            }

            throw new NotImplementedException();
        }

        public async Task<string> ExtractPlainText(Stream fileStreamInput)
        {
            WordDocument document = new WordDocument(fileStreamInput, FormatType.Word2007);
            string returnText = string.Empty;
            foreach (WSection section in document.Sections)
            {
                foreach (WParagraph paragraph in section.Paragraphs)
                {
                    var text = paragraph.Text;
                    returnText += $"{text}{Environment.NewLine}";
                }
            }
            return returnText;
        }

        private static void IterateTextBody(WTextBody textBody)

        {

            //Iterates through each of the child items of WTextBody

            for (int i = 0; i < textBody.ChildEntities.Count; i++)

            {

                //IEntity is the basic unit in DocIO DOM. 

                //Accesses the body items (should be either paragraph or table) as IEntity

                IEntity bodyItemEntity = textBody.ChildEntities[i];

                //A Text body has 2 types of elements - Paragraph and Table

                //Decides the element type by using EntityType

                switch (bodyItemEntity.EntityType)

                {

                    case EntityType.Paragraph:

                        WParagraph paragraph = bodyItemEntity as WParagraph;

                        //Checks for particular style name and removes the paragraph from DOM

                        if (paragraph.StyleName == "MyStyle")

                        {

                            int index = textBody.ChildEntities.IndexOf(paragraph);

                            textBody.ChildEntities.RemoveAt(index);

                        }

                        break;

                    case EntityType.Table:

                        //Table is a collection of rows and cells

                        //Iterates through table's DOM

                        IterateTable(bodyItemEntity as WTable);

                        break;

                }

            }
        }

        private static void IterateTable(WTable table)

        {

            //Iterates the row collection in a table

            foreach (WTableRow row in table.Rows)

            {

                //Iterates the cell collection in a table row

                foreach (WTableCell cell in row.Cells)

                {

                    //Table cell is derived from (also a) TextBody

                    //Reusing the code meant for iterating TextBody

                    IterateTextBody(cell);

                }

            }

        }
    }
}
