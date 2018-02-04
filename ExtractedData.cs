using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWSPriceBot
{
    public class ExtractedData
    {
        public List<string> Price { get; set; }
        public DateTime Date { get; set; }
        public string Item { get; set; }
        public string Author { get; set; }
        public Uri Url { get; set; }
    }
}
