using System;

namespace CLP
{
	public static class Tools
	{

		public static int MaxPointer(double[] vals)
		{
			double max = 0d;
            int maxPointer = 0;
            for (int val = 0; val < vals.Length; val++) 
			{

                if (vals[val] > max)
                {
                    max = vals[val];
                    maxPointer = val;
                }
			}
            return maxPointer;
		}

		public static T[][] CreateArray<T>(int rows, int cols)
		{
			T[][] array = new T[rows][];
			for (int i = 0; i < array.GetLength(0); i++)
				array[i] = new T[cols];

			return array;
		}

	}
}

