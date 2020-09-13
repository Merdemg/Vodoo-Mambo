#define USE_LOGS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class NewTimer : MonoBehaviour {

    [Header("Parameters")]
    [SerializeField]int[] scoreThresholdsByLevel = new int[]{1000, 2000, 3000, 4000, 5000};
    [SerializeField]int[] drainTimesByLevel = new int[]{10, 5, 4, 3, 2};
	[Space]
	[SerializeField] int fillerLerpSpeed = 5;
    [Header("Setup")]
    [SerializeField]Image fillerImage;
    [SerializeField]GameObject fire;
    [SerializeField]Transform fireEmptyPos;
    [SerializeField]Transform fireFullPos;
    [SerializeField]Text levelText;
    [SerializeField]Text scoreText;

    [Header("Info")]
    [SerializeField]bool isActive;
    public bool IsActive {
        get {
			return isActive;
        }
    }
    [SerializeField]int timerLevel = 0;
    [SerializeField]float timeLeft = 30;

    [SerializeField][Range(0,1)]float pos = .5f;
	[Space]
	[SerializeField] bool lastSecondsTriggered = false;
	[SerializeField] AudioClipExtended lastSecondsAudio = null;
    [Space]
    public bool isBonusActive = false;

    float CurrentScoreThreshold {
        get {
	        return GetScoreThreshold(timerLevel);
	        // If timer level is above expected return last threshold + 30 x difference
	        /*var maxLevel = scoreThresholdsByLevel.Length - 1;
	        if(timerLevel > maxLevel)
	        {
		        var difference = timerLevel - scoreThresholdsByLevel.Length - 1;
		        print(scoreThresholdsByLevel[maxLevel] + (difference * 30000));
		        
	        }
	        print(scoreThresholdsByLevel[timerLevel]);*/
        }
    }
    
	[Button]
	private float GetScoreThreshold(int level)
	{
		// If timer level is above expected return last threshold + 30 x difference
		var maxLevel = scoreThresholdsByLevel.Length - 1;
		if(level > maxLevel)
		{
			var difference = level - maxLevel;
			return  scoreThresholdsByLevel[maxLevel] + (difference * 40000);
			
		}
		return scoreThresholdsByLevel[level];
	}

	[ShowInInspector, ReadOnly]
    float DrainTime {
		get {
			// If timer level is above expected return last
			if (timerLevel > drainTimesByLevel.Length - 1)
			{
				return drainTimesByLevel[drainTimesByLevel.Length - 1];
			}

            return drainTimesByLevel[timerLevel];
        }
    }

    void Update()
    {
        if (!isActive)
            return;

        if (GameManager.Instance.GameplayState != GameplayState.Playing)
            return;
        
        UpdateTimer();
		UpdateGraphics();
		CheckGameEnd ();
    }

    public void StartTimer()
    {
        isActive = true;
        timerLevel = 0;
        timeLeft = DrainTime;
        levelText.text = timerLevel.ToString();
        pos = 1;
        UpdateGraphics();
    }

	public void SetPaused(bool isPaused = true) {
		isActive = !isPaused;
	}

    void UpdateGraphics()
    {
		
		fillerImage.fillAmount = Mathf.Lerp (fillerImage.fillAmount, pos, Time.deltaTime * fillerLerpSpeed);
        fire.transform.position = Vector3.Lerp(fireEmptyPos.position, fireFullPos.position, pos);

        // Target
//        fillerImage.fillAmount = Mathf.Lerp(fillerImage.fillAmount, pos, Time.deltaTime * lerpSpeed);
//        Vector3 fireTargetPos = Vector3.Lerp(fireEmptyPos.position, fireFullPos.position, pos);
//        fire.transform.position = Vector3.Lerp(fire.transform.position, fireTargetPos, Time.deltaTime * lerpSpeed);
    }

    void LevelUp()
    {
		// Last Seconds 
		if(lastSecondsAudio != null) {
			AudioManager2.Instance.Stop (lastSecondsAudio);
		}

		lastSecondsTriggered = false;

        timerLevel += 1;
        timeLeft = DrainTime;
        pos = 1f;
        levelText.text = timerLevel.ToString();

        var messageText = ( Lean.Localization.LeanLocalization.GetTranslationText( "Common--Level" ) + " " + timerLevel );
//		UIManager.Instance.ingameShaman.Show( messageText );
		UIManager.Instance.midScreenMessage.Show( messageText );
    }
        
    void UpdateTimer()
    {
        timeLeft -= Time.deltaTime;
        pos = timeLeft / DrainTime;

		if (timeLeft <= 5 && !lastSecondsTriggered) {
			lastSecondsTriggered = true;
			lastSecondsAudio = AudioManager2.Instance.Play (SoundsManager.Instance.lastSeconds, AudioClipExtended.AudioType.SFX);
		}
    }

	void CheckGameEnd () {

		// Fix Bug: Crash on Endgame if DebugManager is deactive
        // 2019.10.29 - LEVON
        //
		var debugDontStopGame = false;
		try {
			debugDontStopGame = DebugManager.Instance.dontStopGame;
		} catch { debugDontStopGame = false; }

		// 
		// if(pos <= 0 && !DebugManager.Instance.dontStopGame)
		// 

		if (!(pos <= 0) || debugDontStopGame) return;
		// Check if bonus active 
		if(isBonusActive) {
			// Set deactive and stop after a time
			var messageText = Lean.Localization.LeanLocalization.GetTranslationText( "Common--Extra-Time" );
			UIManager.Instance.midScreenMessage.Show( messageText, null, false);

			Utility.Instance.DoAfter(5, EndGame);
			return;
		}

		EndGame();
	}

    void EndGame() {
	    
	    isActive = false;
    	var messageText = Lean.Localization.LeanLocalization.GetTranslationText( "Ingame--Time-Up" );
        UIManager.Instance.timesUpMessage.Show( messageText, GameManager.Instance.InitStopGame, false);
        
    }

    public void ScoreUpdated()
    {
//        print("ScoreUpdated");
        var score = ScoreManager.Instance.Score;
        if(score > CurrentScoreThreshold) {
            LevelUp();
        }

        scoreText.text = score.ToString() + "/" + CurrentScoreThreshold;
        UpdateGraphics();
    }

	public void Reset() {
		timerLevel = 0;
		scoreText.text = 0.ToString() + "/" + CurrentScoreThreshold;
		pos = 1;
		UpdateGraphics ();
	}
}
