using UnityEngine;

public class MissionListEntry : MonoBehaviour
{
	public VGUIText[] description;

	public tk2dTextMesh completedText;

	public VGUIText score;

	public GameObject completed;

	public virtual void setDescription(string text)
	{
		string[] array = text.Split("|"[0]);
		for (int i = 0; i < description.Length; i++)
		{
			if (i < array.Length)
			{
				description[i].setText(array[i]);
			}
			else
			{
				description[i].setText(string.Empty);
			}
		}
		completedText.setText(TextData.MISSION_TEXT_COMPLETED);
	}
}
