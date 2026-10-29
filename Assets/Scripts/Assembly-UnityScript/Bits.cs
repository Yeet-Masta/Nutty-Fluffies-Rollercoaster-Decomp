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
		int index = 0;
		while ((bit & 1) == 0)
		{
			bit >>= 1;
			index++;
		}
		return index;
	}

	public static int GetMaskShift(int mask)
	{
		int shift = 0;
		while ((mask & 1) == 0)
		{
			mask >>= 1;
			shift++;
		}
		return shift;
	}
}
