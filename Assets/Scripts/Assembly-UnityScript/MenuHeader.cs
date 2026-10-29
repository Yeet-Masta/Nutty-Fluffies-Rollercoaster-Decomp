using UnityEngine;

public class MenuHeader : MonoBehaviour
{
	public tk2dTextMesh oneLine;

	public tk2dTextMesh[] twoLine;

	public virtual void setText(string title)
	{
		if (title == string.Empty || title == null)
		{
			gameObject.SetActiveRecursively(false);
			return;
		}
		string text = LocalizationManager.localize(title);
		string[] array = text.Split("\n"[0]);
		gameObject.SetActiveRecursively(true);
		if (array.Length == 1)
		{
			twoLine[0].setText(string.Empty);
			twoLine[1].setText(string.Empty);
			oneLine.setText(array[0]);
		}
		else if (array.Length == 2)
		{
			twoLine[0].setText(array[0]);
			twoLine[1].setText(array[1]);
			oneLine.setText(string.Empty);
		}
		else
		{
			Debug.LogError("TOO MANY LINES IN HEADER");
		}
	}
}
