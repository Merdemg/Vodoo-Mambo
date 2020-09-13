//#define USE_LOGS
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager2 : MonoBehaviour
{
	public static AudioManager2 Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	[Header("Parameters")]
	[SerializeField] float musicFadeDuration = .4f;
	[SerializeField] float sfxFadeDuration = .1f;
    [Header("Ducking")]
	[SerializeField] float duckedVolume = .4f;
    [SerializeField]float duckingTime = .4f;
	//	[SerializeField] float sfxLoopedFadeDuration = .1f;

	[Header("Info")]
	[SerializeField]List<AudioClipExtended> activeClips = new List<AudioClipExtended> ();
	[SerializeField]List<AudioSource> audioSourcePool = new List<AudioSource>();
	[SerializeField]List<AudioSource> activeAudioSources = new List<AudioSource>();
	public bool soundOn = true;
	public bool musicOn = true;

	public void SetSound(bool isSoundOn, bool isMusicOn )
	{
		soundOn = isSoundOn;
		musicOn = isMusicOn;

		foreach (var clip in activeClips)
		{
            if(clip.type == AudioClipExtended.AudioType.Music)
            {
                clip.Volume = isSoundOn && isMusicOn ? 1 : 0;
            }
            else
            {
                clip.Volume = isSoundOn ? 1 : 0;
            }
		}

	}

	void PoolSource(AudioSource source)
	{
		audioSourcePool.Add (source);
		activeAudioSources.Remove (source);
		source.enabled = false;
	}

	AudioSource GetSource()
	{
		if(audioSourcePool.Count > 0)
		{
			AudioSource source = audioSourcePool [0];
			source.enabled = true;
			audioSourcePool.Remove (source);
			activeAudioSources.Add(source);
			return source;
		}
		else
		{
			// Create new source
			AudioSource newAudioSource = gameObject.AddComponent<AudioSource> ();
			newAudioSource.playOnAwake = false;
			activeAudioSources.Add(newAudioSource);
			return newAudioSource;
		}
	}

	/// <summary>
	/// Plays random clip as SFX, between clips
	/// </summary>
	/// <param name="clips">Clips.</param>
    public void Play(AudioClip[] clips, float volume = .8f, bool isFade = true)
	{
        int rng = Random.Range (0, clips.Length);

        Play (clips [rng],AudioClipExtended.AudioType.SFX, volume, isFade);
	}

    public AudioClipExtended Play(AudioClip clip, AudioClipExtended.AudioType type = AudioClipExtended.AudioType.SFX, float volume = .8f, bool isFade = true)
	{
		if(clip == null)
		{
			return null;
		}

		float finalVolume = volume;

		if(!soundOn || (type == AudioClipExtended.AudioType.Music && !musicOn) )
		{
			finalVolume = 0;
		}

		Trace.Msg (("Play, "+clip.name).Colored (Colors.aqua) + ", Volume: " + finalVolume);

		// Create clipExt 
        AudioClipExtended clipExt = new AudioClipExtended(clip, type, finalVolume);

		// Start Playing
        StartPlaying (clipExt, finalVolume, isFade);

		return clipExt;
	}

    void StartPlaying(AudioClipExtended clipExt, float volume, bool isFade = true)
	{
		Trace.Msg (("StartPlaying, "+clipExt.clip.name).Colored (Colors.aqua));

//		#region Interrupt Current Active
		// Stop any existing current music if type is music
		// Or dont start playing if music is already active
		AudioClipExtended activeClipExtToStop = null;

		if(clipExt.type == AudioClipExtended.AudioType.Music)
		{
			foreach (var activeClipExt in activeClips) 
			{
				if(activeClipExt.clip == clipExt.clip)
				{
					Trace.Msg (("This music is already playing.").Colored(Colors.aqua));
					return;
				}

				if(activeClipExt.type == clipExt.type)
				{
					Trace.Msg (("Found other playing music").Colored(Colors.aqua));
					activeClipExtToStop = activeClipExt;
				}
			}
		}
		else if(clipExt.type == AudioClipExtended.AudioType.LoopedSFX)
		{
			foreach (var activeClipExt in activeClips) 
			{
				if(activeClipExt.clip == clipExt.clip)
				{
					Trace.Msg (("This looped sfx is already playing, stoping the old one and restarting.").Colored(Colors.aqua));
					activeClipExtToStop = activeClipExt;
				}


				if(activeClipExt.type == clipExt.type)
				{
					Trace.Msg (("Found other playing music").Colored(Colors.aqua));
					activeClipExtToStop = activeClipExt;
				}
			}
		}


        // Ducking for sfx
        if(clipExt.type != AudioClipExtended.AudioType.Music)
        {
            if(activeClips.Count > 0)
            {
                var lastClip = activeClips[activeClips.Count - 1];
                var currentVolumeCached = lastClip.Volume;

                // Don't duck if volume is zero
                if(currentVolumeCached > 0
                    // Don't duck types other than Music
//                    || lastClip.type != AudioClipExtended.AudioType.Music
                )
                {
                    var finalVolume = currentVolumeCached * duckedVolume;

                    // Unduck after some time
                    var unduckDelay = clipExt.clip.length - (2 * duckingTime);

                    // Check -- if clip is too short for appropriate ducking time
                    if(unduckDelay < 0 )
                    {
//                        duckingTime = clipExt.clip.length / 2;
                        unduckDelay = duckingTime;
                    }

                    Trace.Msg("Ducking " + lastClip.clip.name);
                    LeanTween.value(gameObject, currentVolumeCached, finalVolume, duckingTime).setOnUpdate((value) =>
                        {
                            lastClip.source.volume = value;
//                            Trace.Msg(lastClip.clip.name + "'s volume is " + value + " (ducking)");
                        });

                    LeanTween.value(gameObject, currentVolumeCached * duckedVolume, currentVolumeCached, duckingTime).setDelay(unduckDelay).setOnUpdate((value) =>
                        {
                            lastClip.source.volume = value;
//                            Trace.Msg(lastClip.clip.name + "'s volume is " + value + " (unducking)");
                        });
                }

//                LeanTween.value(gameObject, curentValCached, duckedVolume,)
//                activeClips[activeClips.Count - 1].source.volume = duckedVolume;
            }

            
        }

		if(activeClipExtToStop != null)
		{
            Stop (activeClipExtToStop, isFade);
		}

//		#endregion

		// Setup Source
		AudioSource source = GetSource ();
		source.clip = clipExt.clip;
		source.time = 0;
		source.volume = 0;

		// Clip
		activeClips.Add (clipExt);
		clipExt.source = source;

		// Set Loop if type is music
		source.loop = clipExt.type == AudioClipExtended.AudioType.Music || clipExt.type == AudioClipExtended.AudioType.LoopedSFX;
		source.Play ();

		// Set fade duration by type
		float fadeDuration = clipExt.type == AudioClipExtended.AudioType.Music ? musicFadeDuration : sfxFadeDuration;

		// Tween fadein
        if(isFade)
        {
            LeanTween.value (gameObject, 0f, volume, fadeDuration).setOnUpdate ((val) => {
                
                source.volume = val;
                
            });
        }

		// If type is SFX, stop clip after the duration - fade duration
		if(clipExt.type == AudioClipExtended.AudioType.SFX)
		{
			float clipDuration = clipExt.clip.length;

			Utility.Instance.DoAfter (clipDuration - sfxFadeDuration, ()=>{

                Stop(clipExt, false);
			});
		}
	}

    public void Stop(AudioClipExtended clipExt, bool isFade = true)
	{

        bool clipExists = activeClips.Remove (clipExt);

        foreach (var clip in activeClips)
        {
            Trace.Msg(clip.clip.name);
        }
        // Cancel if clip is null or is not active
        if(!clipExists || clipExt == null)
        {
            Trace.Msg (("Stop interrupted, clip is not active or null").Colored(Colors.aqua));

            return;
        }

        Trace.Msg (("StopPlaying, " + clipExt.clip.name).Colored (Colors.aqua));
		// Set fade duration by type
		float duration = clipExt.type == AudioClipExtended.AudioType.Music ? musicFadeDuration : sfxFadeDuration;


		// Tween fadeout
		float currentVolume = clipExt.source.volume;

        if(isFade)
        {
            LeanTween.value (gameObject, currentVolume, 0f, duration)
                .setOnUpdate ((val) => {
                    
                    clipExt.source.volume = val;
                    
                }).setOnComplete(()=>{
                    
                    // Pool Source
                    clipExt.source.Stop();
                    PoolSource(clipExt.source);
                });
        }
        else
        {

            clipExt.source.Stop();
            PoolSource(clipExt.source);
        }

	}

    private void UpdateMusicDucking()
    {
        // Unduck all only one clip is playing
        if(activeClips.Count == 1)
        {
            activeClips[0].source.volume = activeClips[0].Volume;
        }

        return;

        //---TODO: delete after this

        // Don't duck if not in game
        if(GameManager.Instance.GameplayState == GameplayState.Stopped)
        
        foreach (var clip in activeClips)
        {
            if(clip.type == AudioClipExtended.AudioType.Music)
            {
                
            }
        }
    }


}

public class AudioClipExtended
{

	public AudioClip clip;
	public AudioType type;
    private float volume;
    public AudioSource source;
    public float Volume{
        get{
            return volume;
        }
        set{
            volume = value;
            if(source != null)
            {
                source.volume = value;
            }
        }
    }

    public AudioClipExtended(AudioClip clip, AudioType type, float volume = 1)
	{
		this.clip = clip;
		this.type = type;
        this.volume = volume;
	}

	public enum AudioType {
		SFX,
		LoopedSFX,
		Music
	}

}

