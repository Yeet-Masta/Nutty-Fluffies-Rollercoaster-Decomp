using System;

[Serializable]
public class ParamList
{
	[Serializable]
	public enum ParamType
	{
		FLOAT = 0,
		OBJECT_TARGETS = 1,
		BOOLEAN = 2,
		INT = 3,
		EMITTER = 4
	}

	[Serializable]
	public class Param
	{
		public ParamType type;

		public string tag;

		public string desc;

		public Param(string _tag, ParamType _type, string _desc)
		{
			tag = _tag;
			type = _type;
			desc = _desc;
		}
	}

	public int numParams;

	public Param[] @params;

	public ParamList(int nParams)
	{
		numParams = nParams;
	}

	public ParamList(Param[] prams)
	{
		numParams = prams.Length;
		@params = prams;
	}
}
