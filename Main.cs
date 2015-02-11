using System;

namespace CLP
{
	public class Root
	{
		static int Main(string[] args)
		{
			Parser parser = new Parser ();
			parser.ParseText ();
			return 0;
		}
	}
}

