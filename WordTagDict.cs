using System;
using System.Collections.Generic;

namespace CLP
{
	public class WordTagDict
	{
		internal  Dictionary<string, Dictionary<string, int>> wordDictionary = new Dictionary<string, Dictionary<string, int>>();
		internal  Dictionary<string,  Dictionary<string, int>> tagDictionary = new Dictionary<string, Dictionary<string, int>>();
		//internal  Dictionary<string, string> wordtoTagMapping = new Dictionary<string, string>();

		public WordTagDict ()
		{
		}

		public List<Tuple<string, int>> this[string word]
		{
			get
			{
				var values = new List<Tuple<string, int>>();
				if (wordDictionary.ContainsKey(word))
				{
					foreach (KeyValuePair<string, Dictionary<string,int>> item in tagDictionary)
					{
						if (item.Value.ContainsKey(word))
						{
							values.Add( new Tuple<string, int>(item.Key, tagDictionary[item.Key][word]));
						}

					}
					return values;
				}
				if (tagDictionary.ContainsKey (word))
				{
					foreach (KeyValuePair<string, Dictionary<string,int>> item in wordDictionary)
					{
						if (item.Value.ContainsKey(word))
						{
							values.Add( new Tuple<string, int>(item.Key, wordDictionary[item.Key][word]));
						}

					}
					return values;
				}


				throw new KeyNotFoundException("word/key not found: " + word.ToString());
			}
		}

		public int this[string word, string tag]
		{
			get
			{
				if ((wordDictionary.ContainsKey (word) && tagDictionary.ContainsKey (tag)) ||
				   (wordDictionary.ContainsKey (tag) && tagDictionary.ContainsKey (word)))
				{
					return wordDictionary.ContainsKey (word) ? wordDictionary [word] [tag] : wordDictionary [tag] [word];
				}

				throw new KeyNotFoundException("word/key not found: " + word.ToString() + tag.ToString());
			}
		}

		public void Add(string word, string tag)
		{
			if (wordDictionary.ContainsKey (word) && tagDictionary.ContainsKey (tag)) 
			{
				Edit (word, tag);
				return;
			}
			if (wordDictionary.ContainsKey (word))
			{
				wordDictionary[word].Add (tag, 1);
				var val = new Dictionary<string,int> ();
				val.Add (word, 1);
				tagDictionary.Add (tag, val);
				return;
			}
			if (tagDictionary.ContainsKey (tag))
			{
				tagDictionary [tag].Add (word, 1);
				var val = new Dictionary<string,int> ();
				val.Add (tag, 1);
				wordDictionary.Add (word, val);
				return;
			}
			var val1 = new Dictionary<string,int> ();
			val1.Add (tag, 1);
			var val2 = new Dictionary<string,int> ();
			val2.Add (word, 1);

			wordDictionary.Add (word, val1);
			tagDictionary.Add (tag, val2);
		}

		public void Edit(string word, string tag)
		{
			Console.WriteLine (word + " " + tag);
			wordDictionary [word] [tag] += 1;
			tagDictionary [tag] [word] += 1;

		}


	}
}

