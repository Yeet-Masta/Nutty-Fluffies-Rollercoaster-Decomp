using System;
using UnityEngine;

[Serializable]
[ExecuteInEditMode]
public class VGUIAnchor : VGUIElement
{
	private Vector3 anchorScreenPosition;

	private Vector2 prevAnchorPoint;

	private Vector2 prevScreenSize;

	public float distanceFromCamera;

	public Vector2 anchorPoint;

	public override void Awake()
	{
		reCalculatePosition();
		base.Awake();
	}

	public virtual void reCalculatePosition()
	{
		anchorScreenPosition = new Vector3(anchorPoint.x * (float)Screen.width, anchorPoint.y * (float)Screen.height, distanceFromCamera);
		prevAnchorPoint = new Vector2(anchorPoint.x, anchorPoint.y);
		prevScreenSize = new Vector2(Screen.width, Screen.height);
	}

	public override void Update()
	{
		if (anchorPoint.x != prevAnchorPoint.x || anchorPoint.y != prevAnchorPoint.y || prevScreenSize.x != (float)Screen.width || prevScreenSize.y != (float)Screen.height)
		{
			reCalculatePosition();
		}
		Vector3 position = Camera.main.ScreenToWorldPoint(anchorScreenPosition);
		transform.position = position;
	}

	public virtual void Main()
	{
	}
}
