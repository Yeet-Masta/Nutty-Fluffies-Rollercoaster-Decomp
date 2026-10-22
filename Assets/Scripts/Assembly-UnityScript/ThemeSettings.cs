using System;
using System.Collections.Generic;

[Serializable]
public class ThemeSettings
{
	public string name;

	public List<string> musicNames;

	public List<string> allowedObjectPacks;

	public CustomSettings customSettings;

	public ThemeSettings()
	{
		name = "UntitledTheme";
		musicNames = new List<string>();
		allowedObjectPacks = new List<string>();
		customSettings = new CustomSettings();
	}

	public virtual bool isPackAllowed(string pack)
	{
		return allowedObjectPacks.Contains(pack);
	}

	public virtual void setPackAllowed(string pack, bool allow)
	{
		if (allowedObjectPacks.Contains(pack) != allow)
		{
			if (allow)
			{
				allowedObjectPacks.Add(pack);
			}
			else
			{
				allowedObjectPacks.Remove(pack);
			}
		}
	}

	public virtual bool isMusicAllowed(string mus)
	{
		return musicNames.Contains(mus);
	}

	public virtual void setMusicAllowed(string mus, bool allow)
	{
		if (musicNames.Contains(mus) != allow)
		{
			if (allow)
			{
				musicNames.Add(mus);
			}
			else
			{
				musicNames.Remove(mus);
			}
		}
	}
}
