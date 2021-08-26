using System.Collections.Generic;

namespace Alifery.Aligo.DocumentParser.Models
{
    public class DocumentContentModel
    {
        public DocumentContentModel()
        {
            Pages = new List<DocumentPageModel>();
            Errors = new List<string>();
        }

        public object DocumentInfo { get; set; }

        public List<DocumentPageModel> Pages { get; set; }
        public List<WordDocumentContentModel> Content { get; set; }
        public HeaderFooterInformationModel HeaderFooter { get; set; }

        public List<string> Errors { get; set; }
    }

    public class WordDocumentContentModel
    {
        public List<DocumentSectionModel> Sections { get; set; }
    }

    public class DocumentSectionModel
    {
        public List<DocumentParagraphsModel> Paragraphs { get; set; }
        public List<DocumentTablesModel> Tables { get; set; }
        public List<DocumentStylesModel> Styles { get; set; }
    }

    public class DocumentParagraphsModel
    {
        public List<DocumentSentenceModel> Sentences { get; set; }
        public string PlainText { get; set; }
    }


    public class DocumentTablesModel
    {
    }

    public class DocumentSentenceModel
    {
    }

    public class DocumentStylesModel
    {
    }

    public class DocumentPageModel
    {
        public DocumentPageModel()
        {
            ContentLines = new List<string>();
        }

        public List<string> ContentLines { get; set; }
    }


    public class HeaderFooterInformationModel
    {
        public List<string> ContentLines { get; set; }
    }
}