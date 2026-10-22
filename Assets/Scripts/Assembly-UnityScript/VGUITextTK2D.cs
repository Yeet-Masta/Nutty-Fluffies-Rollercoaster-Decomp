using System;
using UnityEngine;

[Serializable]
public class VGUITextTK2D : VGUIText
{
	private tk2dTextMesh textMeshTK;

	protected override void findTextMesh()
	{
		template = text;
		textMeshTK = (tk2dTextMesh)GetComponent(typeof(tk2dTextMesh));
		if (textMeshTK == null)
		{
			textMeshTK = (tk2dTextMesh)GetComponentInChildren(typeof(tk2dTextMesh));
		}
		if (textMeshTK == null)
		{
			Debug.LogError("VGUIText does not have textMesh! " + textMeshTK);
		}
	}

	public override void setColor(Color to)
	{
	}

	public override void setText(string to)
	{
		text = to;
		if (textMeshTK != null)
		{
			textMeshTK.setText(text);
		}
	}
}
