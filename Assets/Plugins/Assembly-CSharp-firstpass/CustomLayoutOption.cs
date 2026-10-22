public class CustomLayoutOption
{
	public enum Type
	{
		minWidth = 0,
		maxWidth = 1,
		minHeight = 2,
		maxHeight = 3
	}

	public Type type;

	public object value;

	protected CustomLayoutOption(Type t, object o)
	{
		type = t;
		value = o;
	}

	public static CustomLayoutOption MinWidth(float width)
	{
		return new CustomLayoutOption(Type.minWidth, width);
	}
}
