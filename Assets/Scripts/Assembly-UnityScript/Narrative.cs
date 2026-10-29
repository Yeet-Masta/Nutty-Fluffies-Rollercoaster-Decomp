using System.Collections;
using System.IO;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

public class Narrative : VGUISwitchObject
{
	public string autoLoadLinesFrom = string.Empty;

	public bool loadFromResources = true;

	public bool beginOnAwake = true;

	public Subtitle subtitle;

	public GameObject commandCallTarget;

	private UnityScript.Lang.Array lines;

	private int currentLineIndex = -1;

	private NarrativeLine currentLine;

	private int currentCommandIndex = -1;

	public virtual void Start()
	{
		if (autoLoadLinesFrom != string.Empty)
		{
			loadLines(autoLoadLinesFrom);
		}
		if (beginOnAwake)
		{
			begin();
		}
	}

	public virtual void begin()
	{
		showNextLine();
	}

	public virtual bool showNextLine()
	{
		currentLineIndex++;
		if (currentLineIndex >= lines.length)
		{
			return false;
		}
		showLine(lines[currentLineIndex] as NarrativeLine);
		return true;
	}

	public virtual bool showCurrentLine()
	{
		if (currentLineIndex == -1)
		{
			return false;
		}
		if (currentLineIndex >= lines.length)
		{
			return false;
		}
		showLine(lines[currentLineIndex] as NarrativeLine);
		return true;
	}

	public virtual void showLine(NarrativeLine line)
	{
		currentLine = line;
		subtitle.gameObject.SetActiveRecursively(true);
		scheduleLineCommands();
		subtitle.startDisplaying(line);
	}

	public virtual void loadLines(string fileName)
	{
		lines = new UnityScript.Lang.Array();
		StreamReader streamReader = null;
		MemoryStream memoryStream = null;
		FileStream fileStream = null;
		if (loadFromResources)
		{
			TextAsset textAsset = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
			if (textAsset == null)
			{
				Debug.LogWarning("Could not load recording from \"" + fileName + "\" using empty recording...");
				return;
			}
			memoryStream = new MemoryStream((textAsset as TextAsset).bytes);
			streamReader = new StreamReader(memoryStream);
		}
		else
		{
			fileStream = File.OpenRead(fileName);
			streamReader = new StreamReader(fileStream);
		}
		string paragraph = string.Empty;
		string line = streamReader.ReadLine();
		while (line != null)
		{
			if (line.Trim().Length == 0)
			{
				appendLine(paragraph);
				paragraph = string.Empty;
			}
			else
			{
				paragraph += line + "\n";
			}
			line = streamReader.ReadLine();
		}
		appendLine(paragraph);
		streamReader.Close();
		if (fileStream != null)
		{
			fileStream.Close();
		}
		if (memoryStream != null)
		{
			memoryStream.Close();
		}
	}

	private void appendLine(string textContent)
	{
		lines.Push(new NarrativeLine(textContent));
	}

	public virtual void scheduleLineCommands()
	{
		CancelInvoke();
		currentCommandIndex = -1;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(currentLine.commands);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is NarrativeCommand))
			{
				obj = RuntimeServices.Coerce(obj, typeof(NarrativeCommand));
			}
			NarrativeCommand narrativeCommand = (NarrativeCommand)obj;
			float delay = (float)narrativeCommand.position * currentLine.duration / (float)currentLine.textContent.Length;
			UnityRuntimeServices.Update(enumerator, narrativeCommand);
			if (!(delay <= 0.25f))
			{
				Invoke(narrativeCommand.name, delay);
				UnityRuntimeServices.Update(enumerator, narrativeCommand);
			}
			else
			{
				gameObject.SendMessage(narrativeCommand.name);
				UnityRuntimeServices.Update(enumerator, narrativeCommand);
			}
		}
	}

	private NarrativeCommand getNextCommand()
	{
		currentCommandIndex++;
		object obj = currentLine.commands[currentCommandIndex];
		if (!(obj is NarrativeCommand))
		{
			obj = RuntimeServices.Coerce(obj, typeof(NarrativeCommand));
		}
		return (NarrativeCommand)obj;
	}

	public override void activate()
	{
		NarrativeCommand nextCommand = getNextCommand();
		Transform transform = this.transform.Find(nextCommand.parameters[0]);
		if (transform != null)
		{
			transform.gameObject.SetActiveRecursively(true);
		}
		else
		{
			Debug.LogWarning("Active target " + nextCommand.parameters[0] + " not found");
		}
	}

	public override void deactivate()
	{
		NarrativeCommand nextCommand = getNextCommand();
		Transform transform = this.transform.Find(nextCommand.parameters[0]);
		if (transform != null)
		{
			transform.gameObject.SetActiveRecursively(false);
		}
		else
		{
			Debug.LogWarning("DeActive target " + nextCommand.parameters[0] + " not found");
		}
	}

	public virtual void call()
	{
		NarrativeCommand nextCommand = getNextCommand();
		commandCallTarget.SendMessage(nextCommand.parameters[0]);
	}

	public virtual void narrateAudio()
	{
		getNextCommand();
		GeneralSoundEffects.playSound(currentLine.narrateAudio);
	}

	public virtual void playAudio()
	{
		NarrativeCommand nextCommand = getNextCommand();
		GeneralSoundEffects.loadAndPlaySound(nextCommand.parameters[0]);
	}
}
