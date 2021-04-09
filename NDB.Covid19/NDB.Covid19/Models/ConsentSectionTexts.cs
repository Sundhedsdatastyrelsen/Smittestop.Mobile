namespace NDB.Covid19.Models
{
    public class ConsentViewModel
    {
        public class ConsentSectionTexts
        {
            public ConsentSectionTexts(string title, string paragraph, string paragraphAccessibilityText)
            {
                Title = title;
                Paragraph = paragraph;
                ParagraphAccessibilityText = paragraphAccessibilityText;
            }

            public string Title { get; }
            public string Paragraph { get; }
            public string ParagraphAccessibilityText { get; }
        }
    }
}