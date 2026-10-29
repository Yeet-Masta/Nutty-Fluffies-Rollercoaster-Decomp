using UnityEngine;

public class VGUIGrid : VGUIList
{
	public float horizontalSpacing = 0.32f;

	public int maxColumns = 2;

	public int maxRows = 2;

	public override Vector3 calculatePosition()
	{
		float left = (0f - horizontalSpacing * (float)(maxRows - 1)) * 0.5f;
		float top = verticalSpacing * (float)(maxColumns - 1) * 0.5f;
		int column = entryRefs.Count % maxColumns;
		int row = entryRefs.Count / maxColumns;
		return new Vector3(left + (float)column * horizontalSpacing, top - (float)row * verticalSpacing, 0.2f);
	}
}
