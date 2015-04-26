using System;

namespace Algorithms
{
	/// <summary>
	/// N个数中取M个数的排列组合
	/// </summary>
	public class NMRange
	{
		private int[] data;
		private int length;
		private int M;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="data">N个数</param>
		/// <param name="M">取M个排列组合</param>
		public NMRange(int[] data,int M)
		{
			this.data = data;
			this.length = data.Length;
			this.M = M;
		}
		public void Range()
		{
			this.Range (data, length, M, M, null);
		}
		private void Range(int[] data, int length, int count, int Count, int[] r)
		{
			if (r == null) r = new int[count];

			if (count == 0) //输出
			{
				for (int j = 0; j < Count; j++)
					Console.Write (data [r [j]] + " ");
				Console.WriteLine ();
				return;
			}

			for (int i = length; i >= count; i--)//从后往前
			{
				r[count - 1] = i - 1;//先固定当前的最后一位
				Range(data, i - 1, count - 1, Count, r);// 从前i-1个里面选取count-1个进行递归
			}
		}
	}
}

