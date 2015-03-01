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
    static class AutomatedTester
    {

        public static void FullTest(string outputFileName)
        {
            StreamWriter file = new System.IO.StreamWriter(outputFileName);
            Parser p = new Parser();
            WordTagDict fullCorpus = p.ParseTrainingText("None");
            fullCorpus.NormalizeDictionary();

            var unseenText = new List<List<Tuple<string, string>>>();
            var result = new List<float>();
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
                Viterbi alg = new Viterbi(fullCorpus, unseenText);
                result.Add(alg.Test());
                Console.WriteLine(i.ToString() + " " + result[i-2].ToString());
                file.WriteLine(i.ToString() + " " + result[i-2].ToString());
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

                Viterbi alg = new Viterbi(ninetyCorpus, unseenText);
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
