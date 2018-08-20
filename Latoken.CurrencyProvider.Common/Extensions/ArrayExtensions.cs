using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Latoken.CurrencyProvider.Common.Helpers.Crypto
{
	[DebuggerStepThrough]
	public static class ArrayExtensions
	{
		public static void Clear(this Array a_array)
		{
			Array.Clear(a_array, 0, a_array.Length);
		}

		public static T[] SubArray <T>(this T[] a_array, int a_index, int a_count = -1)
		{
			if (a_count == -1)
				a_count = a_array.Length - a_index;
			T[] objArray = new T[a_count];
			Array.Copy((Array)a_array, a_index, (Array)objArray, 0, a_count);

            return objArray;
		}

	    public static IEnumerable<string> Append(this string[] arrayInitial, object objectToAppend)
	    {
	        string[] objectArray = null;
	        if (!(objectToAppend is string) && (objectToAppend is System.Collections.IEnumerable))
	        {
	            objectArray = (string[])objectToAppend;
	        }
	        else if (objectToAppend is string)
	        {
	            objectArray = new string[] { (string)objectToAppend };
	        }
	        else
	        {
	            throw new ArgumentException("No valid type supplied.");
	        }
	        string[] ret = new string[arrayInitial.Length + objectArray.Length];
	        arrayInitial.CopyTo(ret, 0);
	        objectArray.CopyTo(ret, arrayInitial.Length);

	        return ret;
	    }
    }
}