using Microsoft.Xna.Framework;
using System;
using System.Runtime.CompilerServices;
using System.Text;


namespace Ash
{
	public static class Utils
	{
		public static string RandomString(int size = 38)
		{
			var builder = new StringBuilder();

			char ch;
			for (int i = 0; i < size; i++)
			{
				ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Rand.NextFloat() + 65)));
				builder.Append(ch);
			}

			return builder.ToString();
		}


		/// <summary>
		/// swaps the two object types
		/// </summary>
		/// <param name="first">First.</param>
		/// <param name="second">Second.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Swap<T>(ref T first, ref T second)
		{
			T temp = first;
			first = second;
			second = temp;
		}

		/// <summary>
		/// Takes a 2d index and width and returns the 1d index.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DimensionIndex(int x, int y, int width)
		{
			return width * y + x;
		}
		/// <summary>
		/// Takes a 2d index and width and returns the 1d index.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DimensionIndex(Point index, int width)
		{
			return width * index.Y + index.X;
		}
		/// <summary>
		/// Takes a 1d index and returns the 2d index.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point DimensionIndex(int index, int width)
		{
			return new Point(index % width, index / width);
		}
	}
}