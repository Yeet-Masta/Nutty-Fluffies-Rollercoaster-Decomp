using System.Collections.Generic;
using UnityEngine;

public class RollerCoasterSounds : SoundManager
{
	public AudioClip checkPointSound;

	public AudioClip crashSound;

	public AudioClip smallCrashSound;

	public AudioClip startSound;

	public AudioClip finishSound;

	public AudioClip vehicleStartSound;

	public AudioClip vehicleFinishSound;

	public AudioClip fireworksSound;

	public AudioClip[] scoreSound;

	public AudioClip[] screamSound;

	public AudioClip[] fallSounds;

	public AudioClip lightCollisionSound;

	public AudioClip heavyCollisionSound;

	public AudioClip[] squealSounds;

	public AudioClip brakeSound;

	public AudioClip[] specialLoop;

	public AudioClip[] specialActivation;

	public AudioClip specialDeactivation;

	public AudioClip specialReady;

	public AudioClip resultMusic;

	public AudioClip resultMusicEpic;

	public AudioClip collectCoinSound;

	public AudioClip finishArriveSound;

	public AudioClip[] moneyDropSounds;

	private float screamTimeTreshold;

	private int lastScream;

	private int musicIndex = -1;

	public AudioSource[] screamSource;

	public AudioSource specialSource;

	private List<int> availableAnimals = new List<int>();

	private const int SCREAMS_PER_ANIMAL = 2;

	private Vehicle vehicle;

	private float lastScoreTime;

	private float lastSpecialScoreTime;

	private float scorePitch = 1f;

	private float specialScorePitch = 1f;

	private string currentMusic;

	private float lastCollisionPlay;

	private bool specialWasPlaying;

	public new static RollerCoasterSounds getInstance()
	{
		return SoundManager.instance as RollerCoasterSounds;
	}

	public override void Awake()
	{
		base.Awake();
		lastScoreTime = Time.time;
		lastSpecialScoreTime = Time.time;
	}

	public virtual void prepare()
	{
		vehicle = GameManager.getInstance().getVehicle();
		availableAnimals.Clear();
		for (int i = 0; i < 5; i++)
		{
			foreach (Cart cart in vehicle.carts)
			{
				if (cart.animalIndex == i)
				{
					availableAnimals.Add(i);
					break;
				}
			}
		}
	}

	public virtual void playStart()
	{
		GeneralSoundEffects.playSound(startSound);
		Invoke("playVehicleStart", 0.2f);
	}

	public virtual void playVehicleStart()
	{
		GeneralSoundEffects.playSound(vehicleStartSound);
	}

	public virtual void playFinish()
	{
		screamTimeTreshold = Time.time + 10f;
		screamSource[0].Stop();
		screamSource[0].clip = vehicleFinishSound;
		screamSource[0].volume = 1f;
		screamSource[0].loop = false;
		screamSource[1].pitch = 1f;
		screamSource[0].Play();
		Invoke("playFinishSound", 1f);
	}

	public virtual void playFinishSound()
	{
		screamSource[0].Stop();
		screamSource[1].clip = finishSound;
		screamSource[1].volume = 1f;
		screamSource[1].pitch = 1f;
		screamSource[1].loop = false;
		screamSource[1].Play();
	}

	public virtual void playFireworks()
	{
		GeneralSoundEffects.playSound(fireworksSound);
	}

	public virtual void playFinishArriveSound()
	{
		GeneralSoundEffects.playSound(finishArriveSound);
	}

	public virtual void playCrash()
	{
		GeneralSoundEffects.playSound(crashSound);
	}

	public virtual void playCheckPoint()
	{
		GeneralSoundEffects.playSound(checkPointSound);
	}

	public virtual void playCollectCoin()
	{
		GeneralSoundEffects.playSound(collectCoinSound);
	}

	public virtual void playMoneyDropSound(int level)
	{
		GeneralSoundEffects.playSound(moneyDropSounds[level], 1f, 1f);
	}

	public virtual void playScore()
	{
		if (!(Time.time - lastScoreTime <= 0.75f))
		{
			scorePitch = 1f;
		}
		else if (!(scorePitch >= 1.8f))
		{
			scorePitch += 0.02f;
		}
		GeneralSoundEffects.playSound(scoreSound[1], 0.9f, scorePitch);
		lastScoreTime = Time.time;
	}

	public virtual void playSpecialScore()
	{
		if (!(Time.time - lastSpecialScoreTime <= 0.75f))
		{
			specialScorePitch = 1f;
		}
		else if (!(specialScorePitch >= 1.8f))
		{
			specialScorePitch += 0.02f;
		}
		GeneralSoundEffects.playSound(scoreSound[0], 0.9f, specialScorePitch);
		lastSpecialScoreTime = Time.time;
	}

	public virtual void playScream(float power)
	{
		if (Time.time <= screamTimeTreshold)
		{
			return;
		}
		if (!(power <= 1f))
		{
			power = 1f;
		}
		int count = availableAnimals.Count;
		if (count <= 0)
		{
			return;
		}
		int pick = (int)(4321f * UnityEngine.Random.value % (float)count);
		for (int i = 0; i < screamSource.Length; i++)
		{
			if (vehicle.getNumberOfToys(availableAnimals[pick % count]) > 0)
			{
				int soundIndex = availableAnimals[pick % count] * 2 + lastScream % 2;
				float length = screamSound[soundIndex].length;
				screamTimeTreshold = Time.time + length + length * 0.5f * UnityEngine.Random.value;
				screamSource[i].clip = screamSound[soundIndex];
				screamSource[i].volume = power * 0.5f;
				screamSource[i].Play();
			}
			if (count > 1)
			{
				pick = (int)(1f + 4321f * UnityEngine.Random.value % (float)(count - 1));
			}
		}
		lastScream++;
	}

	public virtual void playScreamWithOneAnimal(int animalIndex, float power)
	{
		if (!(Time.time <= screamTimeTreshold))
		{
			if (!(power <= 1f))
			{
				power = 1f;
			}
			if (vehicle.getNumberOfToys(animalIndex) > 0)
			{
				int soundIndex = animalIndex * 2 + lastScream % 2;
				float length = screamSound[soundIndex].length;
				screamTimeTreshold = Time.time + length + length * 0.5f * UnityEngine.Random.value;
				screamSource[0].clip = screamSound[soundIndex];
				screamSource[0].volume = power * 0.5f;
				screamSource[0].Play();
			}
			lastScream++;
		}
	}

	public virtual void playScreamSpecial(int animalIndex)
	{
		if (!(Time.time <= screamTimeTreshold))
		{
			int soundIndex = (int)((float)(animalIndex * 2) + UnityEngine.Random.value * 12345f % 2f);
			float length = screamSound[soundIndex].length;
			screamTimeTreshold = Time.time + length + length * 0.5f * UnityEngine.Random.value;
			screamSource[0].clip = screamSound[soundIndex];
			screamSource[0].volume = 0.75f;
			screamSource[0].Play();
		}
	}

	public virtual void playFallSound(int index)
	{
		GeneralSoundEffects.playSound(fallSounds[index]);
		GeneralSoundEffects.playSound(smallCrashSound);
	}

	public virtual void playGameMusic(List<string> nameList)
	{
		if (nameList.Count > 0)
		{
			musicIndex = (int)(UnityEngine.Random.value * 12345f);
			musicIndex %= nameList.Count;
			if (nameList[musicIndex] != currentMusic)
			{
				musicSource.volume = 0.19f;
				musicSource.loop = true;
				playMusic(FileManager.getGameMusicPath() + "/" + nameList[musicIndex]);
				currentMusic = nameList[musicIndex];
			}
		}
	}

	public override void stopMusic()
	{
		base.stopMusic();
		currentMusic = string.Empty;
	}

	public virtual void playResultMusic(bool epic)
	{
		musicSource.volume = 0.4f;
		if (!epic)
		{
			playMusic(resultMusic);
		}
		else
		{
			playMusic(resultMusicEpic);
		}
		musicSource.loop = true;
		currentMusic = string.Empty;
	}

	public virtual bool isPlayingResultMusic()
	{
		return musicSource.clip == resultMusic || musicSource.clip == resultMusicEpic;
	}

	public virtual void playTrackCollision(float impactPower)
	{
		if (Time.time - lastCollisionPlay >= 0.2f)
		{
			if (!(impactPower >= 0.5f))
			{
				GeneralSoundEffects.playRandomSound(squealSounds, 1f);
			}
			else if (!GeneralSoundEffects.isPlaying(heavyCollisionSound))
			{
				GeneralSoundEffects.playSound(heavyCollisionSound, impactPower);
			}
			lastCollisionPlay = Time.time;
		}
	}

	public virtual void playSpecialActivation(int specialType)
	{
		GeneralSoundEffects.playSound(specialActivation[specialType]);
	}

	public virtual void playSpecialDeactivation()
	{
		specialSource.Stop();
		GeneralSoundEffects.playSound(specialDeactivation);
	}

	public virtual void playSpecialReady()
	{
		GeneralSoundEffects.playSound(specialReady);
	}

	public virtual void playSpecialLoop(int specialType)
	{
		specialSource.clip = specialLoop[specialType];
		specialSource.loop = true;
		specialSource.volume = 1f;
		specialSource.Play();
	}

	public virtual void stopGameSounds()
	{
		specialWasPlaying = specialSource.isPlaying;
		if (specialSource.isPlaying)
		{
			specialSource.Pause();
		}
	}

	public virtual void resumeGameSounds()
	{
		if (specialWasPlaying)
		{
			specialSource.Play();
		}
	}
}
