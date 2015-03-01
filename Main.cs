using CompLing;
using System;
using System.Diagnostics;

namespace CLP
{
	public class Root
	{
		static int Main(string[] args)
		{
            AutomatedTester.FullTest("test2.txt");
            Console.ReadKey();
            return 0;
		}
	}
}

