
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
            GetHardwareSwap();
            GetAvExchange();
            GetMechMarket();
           
        }
        static void GetMechMarket()
        {
            var reddit = new Reddit();
            reddit.LogIn("avexchangegrabber", "avexchangegrabber");
            var subreddit = reddit.GetSubreddit("/r/mechmarket");
            subreddit.Subscribe();

            AnalyzeSubreddit(subreddit);
        }

        static void GetAvExchange()
        {
            var reddit = new Reddit();
            reddit.LogIn("avexchangegrabber", "avexchangegrabber");
            var subreddit = reddit.GetSubreddit("/r/appleswap");
            subreddit.Subscribe();
            AnalyzeSubreddit(subreddit);
        }

        static void GetHardwareSwap()
        {
            var reddit = new Reddit();
            reddit.LogIn("HWSPriceGrabber", "reddit1159");
            var subreddit = reddit.GetSubreddit("/r/hardwareswap");
            subreddit.Subscribe();

            AnalyzeSubreddit(subreddit);
        }
        private static void AnalyzeSubreddit(Subreddit subreddit)
        {
            List<ExtractedData> itemDetailList;
            ExtractedData extractedData;
            itemDetailList = new List<ExtractedData>();
            DataExctractor de = new DataExctractor();
            foreach (Post post in subreddit.New.Take(30))
            {
                if (post.LinkFlairText.ToUpper() == "SELLING")
                {
                    if (ContainsTable(post))
                    {
                        ExtractTable(post);
                    }
                    extractedData = new ExtractedData
                    {
                        Price = de.GetPrice(post),
                        Item = de.ParseTitle(post.Title),
                        Date = post.Created,
                        Author = post.Author.Name,
                        Url = post.Url,
                        Text = post.SelfText,
                        TextHtml = post.SelfTextHtml,
                        Flair = post.LinkFlairText,
                        Subreddit = post.SubredditName,
                        Title = post.Title,
                        AuthorDto = new Author
                        {
                            Name = post.Author.Name,
                            PostKarma = post.Author.LinkKarma,
                            CommentKarma = post.Author.CommentKarma,
                            Created = post.Author.Created.ToString()
                        }

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

        private static void ExtractTable(Post post)
        {
            List<ExtractedData> extractedDataList;
            extractedDataList = new List<ExtractedData>();


        }

        private static bool ContainsTable(Post post)
        {
            return false;
           // return post.SelfText.Contains("--|") || post.SelfText.Contains("|:-") || post.SelfText.Contains("-:|");
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
