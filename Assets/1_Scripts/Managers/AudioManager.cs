using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[Header("Audio")]
	public AudioClip pop;
	public AudioClip mainMenuMusic;
	public AudioClip inGameMusic;

	[Header("Audio Sources")]
	public AudioSource SFXAudioSource;
	public AudioSource backgroundMusicAudioSource;

	public static AudioManager Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
//		PlayMainMenuMusic ();
	}

	private void PlayMusicLoop(AudioSource thisAudioSource, AudioSource[] otherAudioSources)
	{
		LeanTween.value(gameObject, (value) =>
			{
				thisAudioSource.volume = value;
				foreach (AudioSource otherAudioSource in otherAudioSources)
				{
					otherAudioSource.volume = 1f - value;
				}
			}, 0f, 1f, 0.5f).setEase(LeanTweenType.linear);
	}

	public void PlayIngameMusic()
	{
		backgroundMusicAudioSource.clip = inGameMusic;
		backgroundMusicAudioSource.Play ();
	}

	public void PlayMainMenuMusic()
	{
		backgroundMusicAudioSource.clip = mainMenuMusic;
		backgroundMusicAudioSource.Play ();
	}

	private void PlaySFXOneShot(AudioClip audioClip)
	{
		SFXAudioSource.PlayOneShot(audioClip);
	}

	public void PlayPop()
	{
		SFXAudioSource.PlayOneShot(pop);
	}
}
