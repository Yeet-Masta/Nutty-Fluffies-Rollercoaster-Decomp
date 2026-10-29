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
		string localized = LocalizationManager.localize(title);
		string[] lines = localized.Split("\n"[0]);
		gameObject.SetActiveRecursively(true);
		if (lines.Length == 1)
		{
			twoLine[0].setText(string.Empty);
			twoLine[1].setText(string.Empty);
			oneLine.setText(lines[0]);
		}
		else if (lines.Length == 2)
		{
			twoLine[0].setText(lines[0]);
			twoLine[1].setText(lines[1]);
			oneLine.setText(string.Empty);
		}
		else
		{
			Debug.LogError("TOO MANY LINES IN HEADER");
		}
	}
}
