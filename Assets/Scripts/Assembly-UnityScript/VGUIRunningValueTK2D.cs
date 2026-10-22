using System;
using UnityEngine;

[Serializable]
public class VGUIRunningValueTK2D : VGUIRunningValue
{
	public tk2dTextMesh textMeshTK;

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
			Debug.LogError("VGUIText does not have tk2dTextMesh! " + textMeshTK);
		}
	}

	public override void setText(string to)
	{
		if (textMeshTK == null)
		{
			findTextMesh();
		}
		text = to;
		textMeshTK.setText(text);
	}
}
