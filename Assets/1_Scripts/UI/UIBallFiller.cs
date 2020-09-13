using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallFiller : MonoBehaviour {

	[SerializeField]int goldAmount = 0;
//	[SerializeField]int ballCount = 0;
	private int GoldAmount{
		get{
			return goldAmount;
		}
		set{
			
//            if(goldAmount > 0 && value > goldAmount)
//			{
//                // TODO: Sacma bi davranisa sebep oluyor
//				FilledUpAnimation ();
//			}

			goldAmount = value;
//            Trace.Msg(goldAmount);
			text.text = "x" + goldAmount;
		}
	}

	[SerializeField]int ballCount = 0;
	public int BallCount
	{
		get{
			return ballCount;
		}
		set{
			ballCount = value;

            //			// Set the filler
            //			float fill = ((float)ballCount / (float)GPM.Instance.ballRecordMaxValues[level]) % 1f;
            //			filler.fillAmount = fill;

            GoldAmount = Mathf.FloorToInt ( (float)ballCount / (float)GPM.Instance.ballRecordMaxValues [level] ) * GPM.Instance.goldPerBallCountByLevel[level];

            //			print("level :" + level );
            //			print("ballCount :" + ballCount );
            //			print("GoldAmount :" + GoldAmount );
        }
	}
	/// <summary>
	/// Sets the ball count animated.
	/// Cannot decrease ball count.
	/// </summary>
	public float BallCount_Animated
	{
		set{
            // If new value is higher set the ballcount
            if (Mathf.FloorToInt (value) > BallCount)
				BallCount = Mathf.FloorToInt (value);

			// Set the filler
			float fill = (value / (float)GPM.Instance.ballRecordMaxValues[level]) % 1f;
			filler.fillAmount = fill;

		}
	}

	[SerializeField]Text text;
	[SerializeField]int level;
	public int Level{
		get{
			return level;
		}
	}
	public Image filler;
    [SerializeField]int soundIndex = 0;


    public void Reset()
    {
        soundIndex = 0;

        // Fix Bug: Miscalculation of golds' count on consecutive plays
        // 2019.10.21 - LEVON
        //
        BallCount = 0; 
        // End of Fix Bug
    }

    public void SetBallCountAnimated(float newBallCount)
    {

        int initialGold = GoldAmount;
//        Trace.Msg(initialGold);

        // If new value is higher set the ballcount
        if (Mathf.FloorToInt (newBallCount) > BallCount)
            BallCount = Mathf.FloorToInt (newBallCount);

        if (GoldAmount > initialGold)
        {
			UIManager.Instance.endGamePanel.MoveCoins(this.transform.position, GPM.Instance.goldPerBallCountByLevel[Level]);
            FilledUpAnimation();

        }

//        Trace.Msg(GoldAmount);

        // Set the filler
        float fill = (newBallCount / (float)GPM.Instance.ballRecordMaxValues[level]) % 1f;
        filler.fillAmount = fill;
    }

	void FilledUpAnimation()
	{
		float scaleDuration = GPM.Instance.ballFilledUpScaleDuration;
		LeanTweenType scaleEasing = GPM.Instance.ballFilledUpScaleEasing;

//      Trace.Msg ("Scale, timepassed " + timepassed);


        // Play sound and move index one 
        AudioManager2.Instance.Play(SoundsManager.Instance.ballFills[soundIndex], AudioClipExtended.AudioType.SFX);
        soundIndex++;

        //
        //  Fix Bug: Out of Index error when more golds earned than the count of sounds
        // 2020.01.12 - LEVON
        //
        if ( soundIndex >= SoundsManager.Instance.ballFills.Length ) {
        	soundIndex = 0; }
        // 
        // End of Fix Bug
        // 
        

        LeanTween.scale (filler.transform.parent.gameObject, Vector3.one * GPM.Instance.ballFilledUpScale, scaleDuration / 2).setEase(scaleEasing);
		LeanTween.scale (filler.transform.parent.gameObject, Vector3.one , scaleDuration / 2).setEase(scaleEasing).setDelay (scaleDuration / 2);
	}

}
