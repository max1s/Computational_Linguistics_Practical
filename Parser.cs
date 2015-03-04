using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CompLing;


namespace CLP
{
    public class Parser
    {

        public Parser(){}


        //Used to read the files and generate the relevant Bayes information
        public NaiveBayes GenerateBayes()
        {
            NaiveBayes nbs = new NaiveBayes();
            List<List<Tuple<string, string>>> text = new List<List<Tuple<string, string>>>();
            var fileRoot = @"C:\\Users\\Max1s\\Dropbox\\CompLing\\CompLing\\treeBank\\";
			//var fileRoot = @"C:\\Users\\Max\\Documents\\treeBank\\treeBank\\";
            var directory = new DirectoryInfo(fileRoot);

            foreach(var subDirectory in directory.GetDirectories())
			{


                foreach (var file in subDirectory.GetFiles())
                {
                    var lines = File.ReadAllLines(file.FullName);
                    var word = "";
                    var tag = "";
                    var sentence = new List<Tuple<string, string>>();
                    foreach (string line in lines)
                    {
                        Regex regPattern = new Regex(@"(\S)+/(\S)+");
                        foreach (Match match in regPattern.Matches(line))
                        {
                            String[] vals = match.Value.Split('/');
                            word = vals[0];
                            tag = vals[1];
                            sentence.Add(new Tuple<string, string>(vals[0], vals[1]));
                            if (word == ".")
                            {
                                text.Add(sentence);
                                sentence = new List<Tuple<string, string>>();
                            }
                        }

                    }
                }

            }
            foreach (var sentence in text)
            {
                foreach (var word in sentence)
                {
                    nbs.AddFeatures(word.Item2, word.Item1, sentence.Select(x => x.Item1).ToList());
                }
            }
            return nbs;
        }

        //parses the unseen text and returns a list of sentences
        public List<List<Tuple<string,string>>> ParseUnseenText(string testNumber)
        {
            List<List<Tuple<string, string>>> text = new List<List<Tuple<string, string>>>();
            var fileRoot = @"C:\\Users\\Max1s\\Dropbox\\CompLing\\CompLing\\treeBank\\" + testNumber;
			//var fileRoot = @"C:\\Users\\Max\\Documents\\treeBank\\treeBank\\" + testNumber;
            var directory = new DirectoryInfo(fileRoot);
            foreach (var file in directory.GetFiles())
            {
                var lines = File.ReadAllLines(file.FullName);
                var word = "";
                var tag = "";
                var sentence = new List<Tuple<string, string>>();
                foreach (string line in lines)
                {
                    Regex regPattern = new Regex(@"(\S)+/(\S)+");
                    foreach (Match match in regPattern.Matches(line))
                    {
                        String[] vals = match.Value.Split('/');
                        word = vals[0];
                        tag = vals[1];
                        sentence.Add(new Tuple<string,string>(vals[0], vals[1]));
                        if (word == ".")
                        {
                            text.Add(sentence);
                            sentence = new List<Tuple<string, string>>();
                        }
                    }

                }

            }
            return text;
        }

        //Parses the training data using regular expressions and returns a word tag dictionary.
        public WordTagDict ParseTrainingText(string testException)
		{
			var wtd = new WordTagDict ();
            var fileRoot = @"C:\\Users\\Max1s\\Dropbox\\CompLing\\CompLing\\treeBank";
			//var fileRoot = @"C:\Users\Max\Documents\treeBank\treeBank";
			var directory = new DirectoryInfo(fileRoot);
			foreach(var subDirectory in directory.GetDirectories())
			{
                if (subDirectory.Name.Contains(testException))
                    continue;

				foreach(var file in subDirectory.GetFiles())
				{

					var lines = File.ReadAllLines (file.FullName);
                    var previousTag = "START";
				    var word = "";
					var tag = "";
					foreach (string line in lines)
					{
                        Regex regPattern = new Regex(@"(\S)+/(\S)+");
                        foreach (Match match in regPattern.Matches(line))
                        {
                            String[] vals = match.Value.Split('/');
                            word = vals[0];
                            tag = vals[1];
                            if (!(tag.All(x => char.IsUpper(x) || char.IsPunctuation(x))) 
                                || tag.Equals("NNP&T") || tag.Equals("S") || tag.Equals("B") || tag.Equals("NNP&P") || tag.Equals("ABC"))
                            {
                                continue;
                            }
                            wtd.EditWordEmission(tag, word);
                            //Debug.WriteLine(word + " " + tag);
                            wtd.EditTransition(previousTag, tag);
                            if (word.Equals("."))
                                previousTag = "Start";
                            else
                                previousTag = tag;
                        }
					
					}
						
				}
			}
			return wtd;	
		}
    }
}

