using System;
using UnityEngine;

[Serializable]
public class VGUIGrid : VGUIList
{
	public float horizontalSpacing;

	public int maxColumns;

	public int maxRows;

	public VGUIGrid()
	{
		horizontalSpacing = 0.32f;
		maxColumns = 2;
		maxRows = 2;
	}

	public override Vector3 calculatePosition()
	{
		float num = (0f - horizontalSpacing * (float)(maxRows - 1)) * 0.5f;
		float num2 = verticalSpacing * (float)(maxColumns - 1) * 0.5f;
		int num3 = entryRefs.Count % maxColumns;
		int num4 = entryRefs.Count / maxColumns;
		return new Vector3(num + (float)num3 * horizontalSpacing, num2 - (float)num4 * verticalSpacing, 0.2f);
	}
}
