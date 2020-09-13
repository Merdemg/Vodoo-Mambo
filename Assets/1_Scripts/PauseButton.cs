using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour {

//    [SerializeField]Sprite pausedSprite;
//    [SerializeField]Sprite playingSprite;
//    [SerializeField]Image image;
    [SerializeField]Button pauseButton;
    [SerializeField]Button playButton;

    public void Pause()
    {
        GameManager.Instance.PauseGame(true, true, false);
        pauseButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
    }

    public void Unpause()
    {
        GameManager.Instance.PauseGame(false, true, false);
        playButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

}
