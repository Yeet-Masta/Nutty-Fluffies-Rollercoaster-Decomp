using System;

[Serializable]
public class TokenCounter : VGUIRunningValueTK2D
{
	public override void setText(string to)
	{
		if (textMeshTK != null)
		{
			textMeshTK.setText(GameData.formatTokenValue(to));
		}
	}
}
