using System;
using System.IO;
using System.Linq;

namespace CLP
{
	public class Parser
	{

		struct Key 
		{
			public readonly string myWord;
			public readonly string myPosTag;
			public Key(string word, string posTag) 
			{
				myWord = word;
				myPosTag = posTag;
			}
		
		}

		public Parser ()
		{
		}



		public WordTagDict ParseText()
		{
			var wtd = new WordTagDict ();
			var fileRoot = @"C:\Users\Max\Documents\treeBank\treeBank";
			var directory = new DirectoryInfo(fileRoot);

			foreach(var subDirectory in directory.GetDirectories())
			{

				foreach(var file in subDirectory.GetFiles())
				{
					var lines = File.ReadAllLines (file.FullName);
					foreach (string line in lines)
					{
						var word = "";
						var posTag = "";
						var whichOne = false;

						foreach (char c in line)
						{

							if (c == '['|| c == ']' ||c == ' ' ||c == '\u0039' ||  c == '\u0044' ||  c == '=' && !whichOne)
								continue;
							if (c == ' ' && whichOne)
							{
								whichOne = !whichOne;
							}
							if (c == '/')
							{
								whichOne = !whichOne;
								continue;
							}
							if (!whichOne)
								word += c;
							if (whichOne)
								posTag += c;
						}

						wtd.Add (word, posTag);
							

					}
						
				}
			}
			return wtd;	
		}
	}
}

