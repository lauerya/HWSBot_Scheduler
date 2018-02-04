using RedditSharp.Things;
using System;
using System.Collections.Generic;

namespace HWSPriceBot
{
    public interface IDataExtractor
    {
       List<string> GetPrice(Post post);

        string ParseTitle(string title);

    }
}