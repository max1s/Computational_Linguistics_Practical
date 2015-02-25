using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CLP
{
	public class Viterbi
	{
		WordTagDict learntDictionary;
        List<List<string>> testWords;
        List<Tuple<string, string>> testWordsWithTag;
        List<Tuple<string, string>> predictions;

		public Viterbi (string testExceptions, string testNumber)
		{
            predictions = new List<Tuple<string, string>>();
            testWords = new List<List<string>>();

            Parser parser = new Parser();
            learntDictionary = parser.ParseLearningText(testExceptions);
            learntDictionary.NormalizeDictionary();

            testWordsWithTag = parser.ParseTrainingText(testNumber);

            List<string> partOfSentence = new List<string>();
            foreach (var item in testWordsWithTag)
            {
                if (item.Item1.Equals("."))
                {
                    partOfSentence.Add(item.Item1);
                    testWords.Add(partOfSentence);
                    partOfSentence = new List<string>();
                }
                partOfSentence.Add(item.Item1);
            }
		}

        public float WordGivenTag(string word, string tag)
        {
          
            var learntInfo = learntDictionary[tag, true];
            if (learntInfo.ContainsKey(word))
                return learntInfo[word];
            else
                return 0f;
            
        }

        public float TagGivenTag(string thisTag, string previousTag)
        {
            var learntInfo = learntDictionary[previousTag, false];
            if (learntInfo.ContainsKey(thisTag))
                return learntInfo[thisTag];
            else
                return 0f;
        }

		public void Test()
		{
            List<string> tags = learntDictionary.Keys;
            foreach (var sentence in testWords)
            {

                float[,] score = new float[sentence.Count, tags.Count];
                float[,] scoreTracer = new float[sentence.Count, tags.Count];

                for(int j = 0; j < tags.Count; j++)
                {
                    score[0, j] = TagGivenTag(tags[j] , "START") * WordGivenTag(sentence[0], tags[j]);
                }

                for(int word = 1; word < sentence.Count; word++)
                {
                    for (int tag = 0; tag < tags.Count; tag++)
                    {
                        float maxKGenerated = 0f;
                        int kPointer = 0;
                        for(int previousTag = 0; previousTag < tags.Count; previousTag++)
                        {
                            float tempk = score[word - 1, previousTag] * TagGivenTag(tags[tag], tags[previousTag]) * WordGivenTag(sentence[word], tags[tag]);
                            if (tempk > maxKGenerated)
                                maxKGenerated = tempk;
                                kPointer = previousTag;
                        }
                        score[word, tag] = maxKGenerated;
                        //Debug.WriteLine(word + " " + tag + " " + score[word, tag]);
                        scoreTracer[word, tag] = kPointer;
                    }
                }
            }
		}

        public void Compare()
        {
        }
	}
}

