using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompLing
{
    public class NaiveBayes
    {
        //our feature vectors Tag -> Vector["name", val]
        Dictionary<string, Dictionary<string, int>> featureVectors = new Dictionary<string, Dictionary<string, int>>();

        public NaiveBayes()  {  }

        //Adds a feature to our featureVector dictionary
        public void AddFeatures(string tag, string word, List<string> context)
        {
            if (featureVectors.ContainsKey(tag))
            {
                EditFeature(tag, word, context);
            }
            else
            {
                featureVectors.Add(tag, CreateFeatureDictionary());
                EditFeature(tag, word, context);
            }
        }

        //Edits the existing features
        public void EditFeature(string tag, string word, List<string> context)
        {
            var dict = CalculateFeatures(tag, word, context);
            foreach (var pair in dict)
            {
                featureVectors[tag][pair.Key] += pair.Value;
            }

        }

        //BoilerPlate for feature dictionary
        private Dictionary<string, int> CreateFeatureDictionary()
        {
            var dict = new Dictionary<string, int>();
            dict.Add("sandwich", 0);
            dict.Add("capitalLetter", 0);
            dict.Add("punctuation", 0);
            dict.Add("endingInY", 0);
            dict.Add("short", 0);
            dict.Add("long", 0);
            dict.Add("leftOfPunctuation", 0);
            dict.Add("rightOfPunctuation", 0);
            return dict;
        }


        //returns our guess
        public float EstimateProbabilityBasedOnFeatures(string tag, string word, List<string> context)
        {

            var bernoulliFeature = CalculateFeatures(tag, word, context);
            
            float cumulative = 0f;
            float total = 0f;


            foreach (var feature in bernoulliFeature)
            {
                cumulative += feature.Value * featureVectors[tag][feature.Key];
                total += featureVectors[tag][feature.Key];
            }
        
            return (cumulative/total)/5000f; //5000f used for scaling, underflow probs can occur otherwise
        }

        //Calculate the features mentioned
        public Dictionary<string, int> CalculateFeatures(string tag, string word, List<string> context)
        {
            context = GenerateContext(word, context);
            var dict = CreateFeatureDictionary();
            dict["sandwich"] = context.First().All(x => !char.IsPunctuation(x)) && context.Last().All(x => !char.IsPunctuation(x)) ? 1 : 0;
            dict["capitalLetter"] = word.Any(x => char.IsUpper(x)) ? 1 : 0;
            dict["punctuation"] = word.Any(x => char.IsPunctuation(x)) ? 1 : 0;
            dict["endingInY"] =  word.Last().Equals('y') ? 1 : 0;
            dict["short"] = word.Length < 5 ? 1 : 0;
            dict["long"] = word.Length > 7 ? 1 : 0;
            dict["leftOfPunctuation"] = context.Last().Any(x => char.IsPunctuation(x)) ? 1 : 0;
            dict["rightOfPunctuation"] = context.First().Any(x => char.IsPunctuation(x)) ? 1 : 0;
            return dict;
        }

        //generate context data
        private List<string> GenerateContext(string word, List<string> context)
        {
            string last = ".";
            bool flag = false;
            var ret = new List<string>();
            foreach (string con in context)
            {
                if (con.Equals(word))
                {
                    ret.Add(last);
                    ret.Add(con);
                    flag = true;
                    continue;
                }

                if (flag)
                {
                    ret.Add(con);
                    return ret;
                }

                last = con;
            }
            ret.Add(".");
            return ret;

        }


    }
}
