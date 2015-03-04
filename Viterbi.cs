using CompLing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CLP
{
	public class Viterbi //class where all of the viterbi algorithm computation is done.
	{
		WordTagDict learntDictionary; //our known corpus
        List<List<string>> testSentences; //our test sentences sans tags
        List<List<Tuple<string, string>>> testSentencesWithTag; // .. with tags
        List<List<Tuple<string, string>>> testPredictions; // our predictions of the tags
        List<string> tags; //list of possible tags
        NaiveBayes nbs; //our naiveBayes information.

        public Viterbi(WordTagDict wtd, List<List<Tuple<string, string>>> tswt, NaiveBayes nb) //constructor takes in most things listed above
        {
            learntDictionary = wtd;
            testSentencesWithTag = tswt;
            testSentences = testSentencesWithTag.Select(x => x.Select(y => y.Item1).ToList()).ToList(); //some clever LINQ, strips the dictionary and leaves just the words
            tags = learntDictionary.Keys;
            nbs = nb;

        }

        // returns the probability of the word given the tag
        public double WordGivenTag(string word, string tag) 
        {
          
            var learntInfo = learntDictionary[tag, true];
            if (learntInfo.ContainsKey(word))
                return learntInfo[word];
            else
                return 0.001d;
            
        }

        //returns the probability of the word given tag with Bayes context
        public double WordGivenTag(string word, string tag, List<string> context)
        {

            var learntInfo = learntDictionary[tag, true];
            if (learntInfo.ContainsKey(word))
                return learntInfo[word];
            else
            {
                return nbs.EstimateProbabilityBasedOnFeatures(tag, word, context);
            }
        }

        //returns the probability of the tag given the tag
        public double TagGivenTag(string thisTag, string previousTag) 
        {
            var learntInfo = learntDictionary[previousTag, false];
            if (learntInfo.ContainsKey(thisTag))
                return learntInfo[thisTag];
            else
                return 0.001d;
        }

        public float Test() //main loop, iterating through all the sentences
		{
            var percentageCorrect = new List<float>();
            for (int sentence = 0; sentence < testSentences.Count; sentence++)
            {
                percentageCorrect.Add(Compare(TestSentence(testSentences[sentence]), testSentencesWithTag[sentence]));
            }

            return percentageCorrect.Average();
		}

        //MAIN VITERBI ALG.
        public List<Tuple<string, string>> TestSentence(List<string> sentence)
        {
            var score = Tools.CreateArray<double>(sentence.Count, tags.Count);
            var scoreTracer = Tools.CreateArray<int>(sentence.Count, tags.Count);

            for (int j = 0; j < tags.Count; j++)
            {
              //  score[0][j] = WordGivenTag(sentence[0], tags[j]) * TagGivenTag(tags[j], "START");
                score[0][j] = WordGivenTag(sentence[0], tags[j], sentence) * TagGivenTag(tags[j], "START");

            }

            for (int word = 1; word < sentence.Count; word++)
            {
                for (int tag = 0; tag < tags.Count; tag++)
                {
                    double maxKGenerated = 0f;
                    int kPointer = 0;
                    for (int previousTag = 0; previousTag < tags.Count; previousTag++)
                    {
                        // double tempk = score[word - 1][previousTag] * TagGivenTag(tags[tag], tags[previousTag]) * WordGivenTag(sentence[word], tags[tag]);
                        double tempk = score[word - 1][previousTag] * TagGivenTag(tags[tag], tags[previousTag]) * WordGivenTag(sentence[word], tags[tag], sentence);
                        if (tempk > maxKGenerated)
                        {
                            maxKGenerated = tempk;
                            kPointer = previousTag;
                        }
                    }
                    score[word][tag] = maxKGenerated;
                    scoreTracer[word][tag] = kPointer;
                }
            }
            return AssembleGuess(sentence, tags, scoreTracer, Tools.MaxPointer(score[sentence.Count - 1]));
        }


        // Implementation of the backpointer alg to generate our guess
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
            var zip = new List<Tuple<string, string>>();
            for( int i = 0; i < sentence.Count; i++) 
            {
                zip.Add(new Tuple<string,string>(sentence[i], tagGuesses[i]));
            }
            return zip;
        }
			
        //Generates a percentage comparison given our guess and the actual result.
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

