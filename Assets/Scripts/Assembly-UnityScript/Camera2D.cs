using UnityEngine;

public class Camera2D : MonoBehaviour
{
	public float initialZoomAdjust;

	protected float initialViewSize;

	protected Camera cameraRef;

	protected Vector2 minLimit = new Vector2(-1000f, -1000f);

	protected Vector2 maxLimit = new Vector2(1000f, 1000f);

	protected Vector2 limitedSize;

	public virtual void Awake()
	{
		cameraRef = gameObject.GetComponent<Camera>() as Camera;
		initialViewSize = 18f / cameraRef.aspect * 0.5f;
	}

	public virtual float getZoomFactor()
	{
		return cameraRef.orthographicSize / initialViewSize;
	}

	public virtual float getZoom()
	{
		return cameraRef.orthographicSize;
	}

	public virtual void resetZoom()
	{
		cameraRef.orthographicSize = initialViewSize;
	}

	public virtual void setZoom(float z)
	{
		cameraRef.orthographicSize = z;
	}

	public virtual void FixedUpdate()
	{
	}
}
