using System;
using System.Collections;
using UnityEngine;

public class TwitterManager : MonoBehaviour
{
	public static event Action loginSucceededEvent;

	public static event Action<string> loginFailedEvent;

	public static event Action postSucceededEvent;

	public static event Action<string> postFailedEvent;

	public static event Action<ArrayList> homeTimelineReceivedEvent;

	public static event Action<string> homeTimelineFailedEvent;

	public static event Action<object> requestDidFinishEvent;

	public static event Action<string> requestDidFailEvent;

	public static event Action<bool> tweetSheetCompletedEvent;
}
