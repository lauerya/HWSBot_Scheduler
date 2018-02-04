
using System;
using System.Linq;
using RedditSharp;
using RedditSharp.Things;
using System.Collections.Generic;

namespace HWSPriceBot
{
	class PriceBot
	{
		static void Main(string[] args)
		{
            List<ExtractedData> itemDetailList;
            ExtractedData extractedData;
            itemDetailList = new List<ExtractedData>();
            var reddit = new Reddit();
            reddit.LogIn("HWSPriceGrabber", "reddit1159");
            var subreddit = reddit.GetSubreddit("/r/hardwareswap");
            subreddit.Subscribe();
            DataExctractor de = new DataExctractor();

            foreach (Post post in subreddit.New.Take(25))
            {
                if (post.LinkFlairText == "Selling")
                {
                    extractedData = new ExtractedData
                    {
                        Price = de.GetPrice(post),
                        Item = de.ParseTitle(post.Title),
                        Date = post.Created,
                        Author = post.Author.Name,
                        Url = post.Url
                    };
                    Console.Write("Post by " + post.Author.Name + " has been processed\n");
                    bool itemInDatabase = de.ValueExistsInDatabase(extractedData);
                    if (!itemInDatabase)
                    {
                        Console.Write("Post by " + extractedData.Author + " is being written to database\n");
                        de.AddToDatabase(extractedData);
                    }
                }
            }
            
        }
	}
}
/*
 * Post options*
 *
 AuthorName:String
 Author:rRedditUser
 Comments:Comment[]
 ApprovedBy:String
 AuthorFrlairText:string
 SelfTest:string
 Title:string
 Url:Uri  
 */
