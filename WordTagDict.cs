using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CLP
{
	public class WordTagDict
	{
		internal  Dictionary<string, Dictionary<string, double>> wordEmissionDictionary = new Dictionary<string, Dictionary<string, double>>();
		internal  Dictionary<string,  Dictionary<string, double>> tagTransitionDictionary = new Dictionary<string, Dictionary<string, double>>();


		public WordTagDict ()
		{
		}

        public List<string> Keys
        {
            get
            {
				return wordEmissionDictionary.Keys.ToList();
            }
        }

		public Dictionary<string, double> this[string tag, bool emission]
		{
			get
			{
                if(emission)
                {
                    return wordEmissionDictionary[tag];
                }
                else
                {
                    return tagTransitionDictionary[tag];
                }

                throw new KeyNotFoundException("tag/word not found :( . :" + tag.ToString());
				
			}
           
		}


        public void EditWordEmission(string tag, string word)
        {
            if (wordEmissionDictionary.ContainsKey(tag))
            {
                if (wordEmissionDictionary[tag].ContainsKey(word))
                {
                    wordEmissionDictionary[tag][word] += 1f;
                }
                else
                {
                    wordEmissionDictionary[tag].Add(word, 1f);
                }

            }
            else
            {
                Dictionary<string, double> temp = new Dictionary<string, double>();
                temp.Add(word, 1);
                wordEmissionDictionary.Add(tag, temp );
            }
        }

		public void EditTransition(string thisTag, string thatTag)
		{
            if (tagTransitionDictionary.ContainsKey(thisTag))
            {
                if (tagTransitionDictionary[thisTag].ContainsKey(thatTag))
                {
                    tagTransitionDictionary[thisTag][thatTag] += 1f;
                }
                else
                {
                    tagTransitionDictionary[thisTag].Add(thatTag, 1f);
                }

            }
            else
            {
                Dictionary<string, double> temp = new Dictionary<string, double>();
                temp.Add(thatTag, 1);
                tagTransitionDictionary.Add(thisTag, temp);
            }
		}

        public void NormalizeDictionary()
        {

            var keys = new List<string>(wordEmissionDictionary.Keys);
            foreach (var key in keys)
            {
                var innerKeys = new List<string>(wordEmissionDictionary[key].Keys);

                var total = wordEmissionDictionary[key].Values.Sum();

                foreach(var word in innerKeys)
                {
                    wordEmissionDictionary[key][word] =  (wordEmissionDictionary[key][word])/(total/100d);
                }
            }

            var otherKeys = new List<string>(tagTransitionDictionary.Keys);
            foreach (var key in otherKeys)
            {
                var innerKeys = new List<string>(tagTransitionDictionary[key].Keys);

                var total = tagTransitionDictionary[key].Values.Sum();

                foreach (var word in innerKeys)
                {
                    tagTransitionDictionary[key][word] = (tagTransitionDictionary[key][word])/ (total/100d);
                }
            }


        }
    /*    var dictionary = new Dictionary<string, double>();
var keys = new List<string>(dictionary.Keys);
foreach (string key in keys)
{
   dictionary[key] = Math.Round(dictionary[key], 3);
} */

	}
}

