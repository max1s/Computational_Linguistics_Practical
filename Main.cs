using CompLing;
using System;
using System.Diagnostics;

namespace CLP
{
	public class Root
	{
		static int Main(string[] args)
		{
            //specify a fileName for the output. Automated tester handles the rest.
            AutomatedTester.FullTest("test4.txt");
            Console.ReadKey();
            return 0;
		}
	}
}

