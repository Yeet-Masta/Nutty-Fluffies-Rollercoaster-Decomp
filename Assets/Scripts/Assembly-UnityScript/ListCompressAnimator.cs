using System.Collections.Generic;
using UnityEngine;

public class ListCompressAnimator : VGUIAnimator
{
	private VGUIList list;

	protected override void activateInternal()
	{
		list = target.gameObject.GetComponent<VGUIList>();
	}

	public override void Update()
	{
		float timeInterpolator = getTimeInterpolator();
		List<GameObject> entryList = list.getEntryList();
		for (int i = 0; i < entryList.Count; i++)
		{
			float y = entryList[i].transform.localPosition.y;
			float y2 = y + ((float)(-i) * list.verticalSpacing - y) * timeInterpolator;
			Vector3 localPosition = entryList[i].transform.localPosition;
			localPosition.y = y2;
			entryList[i].transform.localPosition = localPosition;
		}
		checkComplete(timeInterpolator);
	}
}
