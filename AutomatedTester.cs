using CLP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompLing
{
    public static class AutomatedTester
    {
        //just systematically goes through creating the objects and then using cross validation in the two loops
        public static void FullTest(string outputFileName)
        {
            StreamWriter file = new System.IO.StreamWriter(outputFileName);
            Parser p = new Parser();

            Console.WriteLine("Generating Naive Bayes Knowledge...");
            NaiveBayes nbs = p.GenerateBayes();


            var unseenText = new List<List<Tuple<string, string>>>();
            var result = new List<float>();

            WordTagDict fullCorpus = p.ParseTrainingText("None");
            fullCorpus.NormalizeDictionary();

            //first loop for the 100%
            for (int i = 2; i < 13; i++)
            {
                if (i < 10)
                {
                    unseenText = p.ParseUnseenText("0" + i.ToString());
                }
                else
                {
                    unseenText = p.ParseUnseenText(i.ToString());
                }
                Viterbi alg = new Viterbi(fullCorpus, unseenText.Take(30).ToList(), nbs);
                result.Add(alg.Test());
                Console.WriteLine(i.ToString() + " " + result[i - 2].ToString());
                file.WriteLine(i.ToString() + " " + result[i - 2].ToString());
            }

            Console.WriteLine("AVG: " + result.Average().ToString());
            file.WriteLine("AVG:  " + result.Average().ToString());

            ///NOW FOR 90% 10%
            result = new List<float>();
            Console.WriteLine("=============== 90% ===============");
            file.WriteLine("=============== 90% ===============");
            for (int i = 2; i < 13; i++)
            {
                WordTagDict ninetyCorpus = p.ParseTrainingText(i.ToString());
                ninetyCorpus.NormalizeDictionary();
                if (i < 10)
                {
                    unseenText = p.ParseUnseenText("0" + i.ToString());
                }
                else
                {
                    unseenText = p.ParseUnseenText(i.ToString());
                }

                Viterbi alg = new Viterbi(ninetyCorpus, unseenText.Take(30).ToList(), nbs);
                result.Add(alg.Test());
                Console.WriteLine(i.ToString() + " " + result[i-2].ToString());
                file.WriteLine(i.ToString() + " " + result[i-2].ToString());
            }

            Console.WriteLine("AVG: " + result.Average().ToString());
            file.WriteLine("AVG:  " + result.Average().ToString());
           
            file.Close();
        
        }
    }
}
