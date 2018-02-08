using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MathNet.Numerics.LinearAlgebra;

namespace CryptoMatrix
{
	class Cryto
	{
		readonly Dictionary<char, float> Codex = new Dictionary<char, float> {
			{ 'a', 1 },
			{ 'b', 2 },
			{ 'c', 3 },
			{ 'd', 4 },
			{ 'e', 5 },
			{ 'f', 6 },
			{ 'g', 7 },
			{ 'h', 8 },
			{ 'i', 9 },
			{ 'j', 10 },
			{ 'k', 11 },
			{ 'l', 12 },
			{ 'm', 13 },
			{ 'n', 14 },
			{ 'o', 15 },
			{ 'p', 16 },
			{ 'q', 17 },
			{ 'r', 18 },
			{ 's', 19 },
			{ 't', 20 },
			{ 'u', 21 },
			{ 'v', 22 },
			{ 'w', 23 },
			{ 'x', 24 },
			{ 'y', 25 },
			{ 'z', 26 },
			{ ' ', 27 },
		};

		readonly Matrix<float> encode;
		readonly Matrix<float> decode;

		public Cryto(int size = 3)
		{
			encode = Matrix<float>.Build.Random(size, size);
			decode = encode.Inverse();
		}

		public float[] Encode(string message)
		{
			var padding = encode.RowCount - (message.Length % encode.RowCount);
			var converted = new float[message.Length + padding];

			for (int i = 0; i < message.Length; i++)
				converted[i] = Codex[message[i]];

			for (int i = converted.Length - 1; i >= converted.Length - padding; i--)
				converted[i] = Codex[' '];

			var M = Matrix<float>.Build;
			var columns = (converted.Length / 3);
			var messageMatrix = M.Dense(encode.RowCount, columns, converted);

			var encoded = encode.Multiply(messageMatrix);

			return encoded.ToColumnMajorArray();
		}

		public string Decode(float[] encodedMessage)
		{
			var columns = encodedMessage.Length / 3;
			var encodedMatrix = Matrix<float>.Build.DenseOfColumnMajor(encode.RowCount, columns, encodedMessage);

			var decodedMatrix = decode.Multiply(encodedMatrix);
			var decodedArray = decodedMatrix.ToColumnMajorArray();

			var sb = new StringBuilder();

			foreach (var d in decodedArray) {
				var pair = Codex.FirstOrDefault(x => x.Value == Math.Round(d));
				sb.Append(pair.Key);
			}

			return sb.ToString().TrimEnd();
		}
	}
}
