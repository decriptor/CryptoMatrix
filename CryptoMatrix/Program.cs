using System;
using System.Linq;

namespace CryptoMatrix
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.Write("Enter string to encode: ");
			var message = Console.ReadLine();
			var crypto = new Cryto();
			var result = crypto.Encode(message);
			var retVal = crypto.Decode(result);

			Console.WriteLine(retVal);
		}
	}
}
