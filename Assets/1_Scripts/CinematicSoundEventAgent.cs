using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicSoundEventAgent : MonoBehaviour {

    [SerializeField]
    public AudioClip[] cinematicSfxs;
    public AudioClip cinematicMusic;

    private AudioClipExtended currentPlayingMusic;

    public void PlayCinematicSfx(int index) {
        AudioManager2.Instance.Play(cinematicSfxs[index]);
    }

    public void PlayMusic() {
        currentPlayingMusic = AudioManager2.Instance.Play(cinematicMusic, AudioClipExtended.AudioType.Music);
    }

    public void StopMusic() {
        AudioManager2.Instance.Stop(currentPlayingMusic);
    }

}
