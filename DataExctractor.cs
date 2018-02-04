using RedditSharp.Things;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HWSPriceBot
{
    public class DataExctractor : IDataExtractor
    {
        ExtractedData extractedData;
        string connectionString = "Data Source=DESKTOP-G3VPCBF;Initial Catalog=HardwareSwap;Integrated Security=True";


        public List<string> GetPrice(Post post)
        {
            extractedData = new ExtractedData();
            extractedData.Price = new List<string>();

            extractedData.Date = post.Created;

            var postText = post.SelfText;
            var charSearch = '$';
            int j = 0;

            for (int i = 0; i < postText.Length; i++)
            {
                string totalPrice = "";

                if (postText[i] == charSearch)
                {

                    i++;
                    while (Char.IsDigit(postText[i]) && i < postText.Length)
                    {
                        totalPrice += postText[i];
                        if (i + 1 == postText.Length)
                        {
                            break;
                        }
                        i++;
                      
                    }
                    extractedData.Price.Add(totalPrice);
                    j++;
                }
            }
            if (extractedData.Price.Count < 0)
            { extractedData.Price = new List<string>(); }

            return extractedData.Price;
        }

		public string ParseTitle(string title)
        {
            if(title.Contains("[H]") || title.Contains("[h]"))
            {
				return title.Split(']')[2].Split('[')[0];
            }
            return "0";
        }

		public void AddToDatabase(ExtractedData data)
		{

            SqlConnection myConnection = new SqlConnection(connectionString);

            SqlCommand myCommand = new SqlCommand("dbo.MergePost", myConnection);
            myCommand.CommandType = System.Data.CommandType.StoredProcedure;
			myCommand.Parameters.AddWithValue("@Name", data.Author);
			myCommand.Parameters.AddWithValue("@Items", data.Item);
			myCommand.Parameters.AddWithValue("@Date", data.Date.ToString());
            myCommand.Parameters.AddWithValue("@Price", String.Join(", ", data.Price.ToArray()));
            myCommand.Parameters.AddWithValue("@Url", data.Url.ToString());
            
            myConnection.Open();

            myCommand.ExecuteNonQuery();
        }

        public bool ValueExistsInDatabase(ExtractedData data)
		{
            SqlConnection myConnection = new SqlConnection(connectionString);
            bool valueExists = false;

            SqlCommand myCommand = new SqlCommand("dbo.CheckPost", myConnection);
            myCommand.CommandType = System.Data.CommandType.StoredProcedure;
            myCommand.Parameters.AddWithValue("@Name", data.Author);
            myCommand.Parameters.AddWithValue("@Items", data.Item);
            myCommand.Parameters.AddWithValue("@Date", data.Date.ToString());
            myCommand.Parameters.AddWithValue("@Price", String.Join(", ", data.Price.ToArray()));
            ;
            myConnection.Open();

            var reader = myCommand.ExecuteReader();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    valueExists = true;
                }
            }
            reader.Close();
            return valueExists;

        }

		public void WriteToFile(Post post, string items, List<string> price)
		{
			string combindedPrice = string.Join(",", price.ToArray());
            string directory = Environment.CurrentDirectory + "WriteLines.txt";

			using (System.IO.StreamWriter file =
			new System.IO.StreamWriter(directory, true))
			{
				file.WriteLine("");
				file.WriteLine("Flair: " + post.LinkFlairText);
				file.WriteLine("Name: " + post.AuthorName);
				file.WriteLine("Date: " + post.Created);
				file.WriteLine("Items: " + items);
				file.WriteLine("Prices: " + combindedPrice);

			}
		}
    }
}
