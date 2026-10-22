using System;
using UnityEngine;

[Serializable]
public class TutorialPage : MonoBehaviour
{
	public string key;

	public bool pauseGame;

	public bool showAlways;

	public string showNextPageKey;

	public bool showNextPageOnlyWithSpecial;

	[HideInInspector]
	public bool shown;

	public virtual void activate()
	{
		shown = true;
	}
}
