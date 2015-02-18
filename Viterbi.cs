using System;

namespace CLP
{
	public class Viterbi
	{
		WordTagDict<string,int> wtDictionary;
		public Viterbi (WordTagDict wtDic)
		{
			wtDictionary = new WordTagDict<string, int> ();
			wtDictionary = wtDic;
		}

		public void Learn()
		{
			Parser p = new Parser ();
			wtDict = p.TrainingData ("90");
		}

		public void Test()
		{
		}
	}
}

