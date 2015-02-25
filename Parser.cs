using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace CLP
{
    public class Parser
    {

        public Parser()
        {
        }



        public List<Tuple<string,string>> ParseTrainingText(string testNumber)
        {
            List<Tuple<string, string>> text = new List<Tuple<string, string>>();
            var fileRoot = @"C:\\Users\\Max1s\\Dropbox\\CompLing\\CompLing\\treeBank\\" + testNumber;
            var directory = new DirectoryInfo(fileRoot);
            foreach (var file in directory.GetFiles())
            {
                var lines = File.ReadAllLines(file.FullName);
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
                        text.Add(new Tuple<string,string>(vals[0], vals[1]));
                    }

                }

            }
            return text;
        }

        public WordTagDict ParseLearningText(string testException)
		{
			var wtd = new WordTagDict ();
            var fileRoot = @"C:\\Users\\Max1s\\Dropbox\\CompLing\\CompLing\\treeBank";
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

