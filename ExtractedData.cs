using System;
using System.Collections.Generic;

namespace HWSPriceBot
{
    public class ExtractedData
    {
        public List<string> Price { get; set; }
        public DateTime Date { get; set; }
        public string Item { get; set; }
        public string Author { get; set; }
        public Uri Url { get; set; }
        public string Text { get; set; }
        public string TextHtml { get; set; }
        public string Flair { get; set; }
        public string Subreddit { get; set; }
        public string Title { get; set; }
        public Author AuthorDto { get; set; }
    }
}
