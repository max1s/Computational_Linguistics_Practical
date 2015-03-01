using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CLP
{
	public class Viterbi
	{
		WordTagDict learntDictionary;
        List<List<string>> testSentences;
        List<List<Tuple<string, string>>> testSentencesWithTag;
        List<List<Tuple<string, string>>> testPredictions;
        List<string> tags;


        public Viterbi(WordTagDict wtd, List<List<Tuple<string, string>>> tswt)
        {
            learntDictionary = wtd;
            testSentencesWithTag = tswt;
            testSentences = testSentencesWithTag.Select(x => x.Select(y => y.Item1).ToList()).ToList();
            tags = learntDictionary.Keys;
        }

        public double WordGivenTag(string word, string tag)
        {
          
            var learntInfo = learntDictionary[tag, true];
            if (learntInfo.ContainsKey(word))
                return learntInfo[word];
            else
                return 0.001d;
            
        }

        public double TagGivenTag(string thisTag, string previousTag)
        {
            var learntInfo = learntDictionary[previousTag, false];
            if (learntInfo.ContainsKey(thisTag))
                return learntInfo[thisTag];
            else
                return 0.001d;
        }

        public float Test()
		{
            var percentageCorrect = new List<float>();
            for (int sentence = 0; sentence < testSentences.Count; sentence++)
            {
                percentageCorrect.Add(Compare(TestSentence(testSentences[sentence]), testSentencesWithTag[sentence]));
                //Debug.WriteLine(percentageCorrect[sentence]);
            }

            return percentageCorrect.Average();
		}

        public List<Tuple<string, string>> TestSentence(List<string> sentence)
        {
            var score = Tools.CreateArray<double>(sentence.Count, tags.Count);
            var scoreTracer = Tools.CreateArray<int>(sentence.Count, tags.Count);

            for (int j = 0; j < tags.Count; j++)
            {
                score[0][j] = WordGivenTag(sentence[0], tags[j]) * TagGivenTag(tags[j], "START");

            }

            for (int word = 1; word < sentence.Count; word++)
            {
                for (int tag = 0; tag < tags.Count; tag++)
                {
                    double maxKGenerated = 0f;
                    int kPointer = 0;
                    for (int previousTag = 0; previousTag < tags.Count; previousTag++)
                    {
                        double tempk = score[word - 1][previousTag] * TagGivenTag(tags[tag], tags[previousTag]) * WordGivenTag(sentence[word], tags[tag]);
                        if (tempk > maxKGenerated)
                        {
                            maxKGenerated = tempk;
                            kPointer = previousTag;
                        }
                    }
                    score[word][tag] = maxKGenerated;
                    //Debug.WriteLine(word + " " + tag + " " + score[word, tag]);
                    scoreTracer[word][tag] = kPointer;
                }
            }
            return AssembleGuess(sentence, tags, scoreTracer, Tools.MaxPointer(score[sentence.Count - 1]));
        }



        public List<Tuple<string, string>> AssembleGuess(List<string> sentence, List<string> tags, int[][] scoreTracer, int startingPointer)
        {
            var tagGuesses = new List<string>();
            var tracer = startingPointer;
            tagGuesses.Add(tags[startingPointer]);
            for (int i = sentence.Count - 2; i >= 0; i--)
            {
                tagGuesses.Add(tags[scoreTracer[i + 1][tracer]]);
                tracer = scoreTracer[i + 1][tracer];

            }
            tagGuesses.Reverse();
            //Debug.WriteLine(tagGuesses.Count + " " + sentence.Count);
            var zip = new List<Tuple<string, string>>();
            for( int i = 0; i < sentence.Count; i++) 
            {
                zip.Add(new Tuple<string,string>(sentence[i], tagGuesses[i]));
            }
            return zip;
        }
			

        public float Compare(List<Tuple<string, string>> guess, List<Tuple<string, string>> actual)
        {
            float percentage = 0f;
            for (int i = 0; i < guess.Count; i++)
            {
                if (guess[i].Item2 == actual[i].Item2)
                    percentage += 1;
            }
            return percentage / guess.Count;
        }
	}
}

