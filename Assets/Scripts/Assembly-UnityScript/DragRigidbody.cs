using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

public class DragRigidbody : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024DragObject_00241508 : GenericGenerator<object>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<object>, IEnumerator
		{
			internal float _0024oldDrag_00241509;

			internal float _0024oldAngularDrag_00241510;

			internal Camera _0024mainCamera_00241511;

			internal Ray _0024ray_00241512;

			internal float _0024distance_00241513;

			internal DragRigidbody _0024self__00241514;

			public _0024(float distance, DragRigidbody self_)
			{
				_0024distance_00241513 = distance;
				_0024self__00241514 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024oldDrag_00241509 = _0024self__00241514.springJoint.connectedBody.drag;
					_0024oldAngularDrag_00241510 = _0024self__00241514.springJoint.connectedBody.angularDrag;
					_0024self__00241514.springJoint.connectedBody.drag = _0024self__00241514.drag;
					_0024self__00241514.springJoint.connectedBody.angularDrag = _0024self__00241514.angularDrag;
					_0024mainCamera_00241511 = _0024self__00241514.FindCamera();
					goto case 2;
				case 2:
					if (Input.GetMouseButton(0))
					{
						_0024ray_00241512 = _0024mainCamera_00241511.ScreenPointToRay(Input.mousePosition);
						_0024self__00241514.springJoint.transform.position = _0024ray_00241512.GetPoint(_0024distance_00241513);
						result = (YieldDefault(2) ? 1 : 0);
						break;
					}
					if ((bool)_0024self__00241514.springJoint.connectedBody)
					{
						_0024self__00241514.springJoint.connectedBody.drag = _0024oldDrag_00241509;
						_0024self__00241514.springJoint.connectedBody.angularDrag = _0024oldAngularDrag_00241510;
						_0024self__00241514.springJoint.connectedBody = null;
					}
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal float _0024distance_00241515;

		internal DragRigidbody _0024self__00241516;

		public _0024DragObject_00241508(float distance, DragRigidbody self_)
		{
			_0024distance_00241515 = distance;
			_0024self__00241516 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new _0024(_0024distance_00241515, _0024self__00241516);
		}
	}

	public float spring = 50f;

	public float damper = 5f;

	public float drag = 10f;

	public float angularDrag = 5f;

	public float distance = 0.2f;

	public bool attachToCenterOfMass;

	private SpringJoint springJoint;

	public virtual void Update()
	{
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}
		Camera camera = FindCamera();
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hitInfo, 100f) && (bool)hitInfo.rigidbody && !hitInfo.rigidbody.isKinematic)
		{
			if (!springJoint)
			{
				GameObject gameObject = new GameObject("Rigidbody dragger");
				Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>() as Rigidbody;
				springJoint = (SpringJoint)gameObject.AddComponent<SpringJoint>();
				rigidbody.isKinematic = true;
			}
			springJoint.transform.position = hitInfo.point;
			if (attachToCenterOfMass)
			{
				Vector3 position = transform.TransformDirection(hitInfo.rigidbody.centerOfMass) + hitInfo.rigidbody.transform.position;
				position = springJoint.transform.InverseTransformPoint(position);
				springJoint.anchor = position;
			}
			else
			{
				springJoint.anchor = Vector3.zero;
			}
			springJoint.spring = spring;
			springJoint.damper = damper;
			springJoint.maxDistance = distance;
			springJoint.connectedBody = hitInfo.rigidbody;
			StartCoroutine("DragObject", hitInfo.distance);
		}
	}

	public virtual IEnumerator DragObject(float distance)
	{
		return new _0024DragObject_00241508(distance, this).GetEnumerator();
	}

	public virtual Camera FindCamera()
	{
		return (!GetComponent<Camera>()) ? Camera.main : GetComponent<Camera>();
	}
}
