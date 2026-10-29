using System;

[Serializable]
public class Bits
{
	public static int IndexOf(int bit)
	{
		if (bit == 0)
		{
			return -1;
		}
		else
		{
			int num = 0;
			while ((bit & 1) == 0)
			{
				bit >>= 1;
				num++;
			}
			return num;
		}
	}

	public static int GetMaskShift(int mask)
	{
		int num = 0;
		while ((mask & 1) == 0)
		{
			mask >>= 1;
			num++;
		}
		return num;
	}
}
