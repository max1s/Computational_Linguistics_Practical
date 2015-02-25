using System;

namespace CLP
{
	public class Root
	{
		static int Main(string[] args)
		{
            Viterbi v = new Viterbi("", "12");
            v.Test();
            return 0;
		}
	}
}

