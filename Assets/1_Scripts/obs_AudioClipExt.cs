using UnityEngine;
using System;
using System.Collections;

public class obs_AudioClipExt : MonoBehaviour
{
	public enum AudioType {
		SFX,
		Music
	}

	/// <summary>
	/// To do on stop Detected
	/// </summary>
	public Action onStopDetected;
	public AudioSource currentSource;
	public AudioType type;
	AudioClip clip;

	public AudioClip AudioClip{
		get{
			return AudioClip;
		}
	}

	void Update()
	{
		if(currentSource != null)
		{
			if(!currentSource.isPlaying)
			{
				onStopDetected ();
			}
		}
	}

	public void OnPlay()
	{
		
	}

	public void OnStop ()
	{
		
	}
}

