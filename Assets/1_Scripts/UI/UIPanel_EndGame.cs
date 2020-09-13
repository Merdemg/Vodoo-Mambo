#define USE_LOGS
//#define USE_LOGS_DC
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook;
using Facebook.Unity;

public class UIPanel_EndGame : UIPanel
{
	[Header("Parameters")]
	[Header ("-Score")]
	public GameObject scoreTextContainer;
	public Text scoreText;
	public Text highscoreText;
	public UIBallFiller[] ballFillers;
	[Header ("-Coins")]
	public UICoinBox coinBox;
	public GameObject coinsIcon;
    [SerializeField]int coinsEarned = 0;
    public int CoinsEarned{
        get{
            return coinsEarned;
        }
        set{
            coinsEarned = value;
            coinsEarnedText.text = value.ToString();
        }
    }
	public Text coinsEarnedText;
	[Space]
	[SerializeField]private Transform parentForCoins;
	[SerializeField]private GameObject coinPrefab;
	[Space]
    [Header ("--Animation")]
	[SerializeField]float delayBetweenCoins = .4f;
    [SerializeField]float endgameCoinMoveDuration = .1f;
    [Space]
    [Header ("-Other")]
    [SerializeField]Transform achievementsParent;
    [SerializeField]AnimationCurve scoreAnimCurve;
	[SerializeField]Button rewardedAdButton;
	[SerializeField]Button shareButton;
	[Space]
	public UILeaderBoard leaderboard;
	[Header("Info")]
	[SerializeField]UIAchievementEntry[] achievementEntries;
	[SerializeField]int lastScore = 0;
    [SerializeField] AudioClipExtended activeAudioClip;

	override protected void Start() {
		base.Start ();
		achievementEntries = achievementsParent.GetComponentsInChildren<UIAchievementEntry> ();

		rewardedAdButton.onClick.AddListener (ShowRewardedVideo);
		shareButton.onClick.AddListener (()=>{
			if(lastScore > 0)
				Share(lastScore);
		});
	}

    public override void Open()
    {
        base.Open();

//        coinBox.Amount = (int)MetaManager.Instance.Get(MetaManager.MetaType.coins);


        // Set ball
//        int[] ballCounts = (int[])(MetaManager.Instance.Get(MetaManager.MetaType.ballCounts));

//        InitBallFillers(ballCounts);

//        var i = 0;
//        foreach (var item in ballFillers)
//        {
//            item.BallCount = ballCounts[i];
//            i++;
//        }
    }

    public void Initialize(int[] newBallCounts, int score, int coinsEarned, int initialCoins, int[] initialBallCounts, Achievement[] newAchievements)
	{

        activeAudioClip = AudioManager2.Instance.Play(SoundsManager.Instance.endgameMusic, AudioClipExtended.AudioType.Music);
		lastScore = score;
		// Ad Button activated
		rewardedAdButton.interactable = true;
        // Coins
        Trace.Msg(coinsEarned);
        CoinsEarned = 0;
        // Score
        scoreText.text = score.ToString ();
        scoreTextContainer.transform.localScale = Vector3.one * .4f;
        // Doing animation stuff delayed
        Utility.Instance.DoAfter (.5f, () => {
            BallFill2 (initialBallCounts, newBallCounts);
            LeanTween.scale (scoreTextContainer, Vector3.one, 1).setEase (scoreAnimCurve);
        });
		// High Score
        highscoreText.text = ((int)MetaManager.Instance.Get(MetaManager.MetaType.hiscore)).ToString();
		// Achievenets
        RefreshAchievements(new List<Achievement>(newAchievements));
	}

	//TODOALP: private
	public void BallFill2(int[] currentBallCounts, int[] ballCounts)
	{
		AudioManager2.Instance.Play(SoundsManager.Instance.ballFill);

        // Ball Records
        float totalDelay = 0;
        for (int i = 0; i < ballCounts.Length; i++) {

            UIBallFiller ballFiller = ballFillers [i];
            ballFiller.Reset();

            int startBallCount = currentBallCounts[i] ;
            int finalBallCount = ballCounts [i];
			int ballMaxCount = GPM.Instance.ballRecordMaxValues [i];


            float duration = GPM.Instance.ballFillDuration * ((float)(finalBallCount - startBallCount) / (float)ballMaxCount);
//			AudioClipExtended clipExt = null;
			LeanTween.value (gameObject, (float)startBallCount, (float)(finalBallCount), duration).setDelay(totalDelay).setEase(GPM.Instance.ballFillEasing)
				.setOnStart(()=>{


//					clipExt = AudioManager2.Instance.Play(SoundsManager.Instance.ballFill, AudioClipExtended.AudioType.LoopedSFX, 1);

				}).setOnUpdate ((val) => {

                    ballFiller.SetBallCountAnimated(val);

				}).setOnComplete(()=>{
					Trace.Msg("Ball fill ended");
//					AudioManager2.Instance.Stop(clipExt);
                    AudioManager2.Instance.Play(SoundsManager.Instance.ballFillLevelDone, AudioClipExtended.AudioType.SFX);
				});

			totalDelay += duration - .2f;

            // Check if panel is still open
            if(!IsOpen)
            {
                return;
            }
		}
	}

	public void MoveCoins (Vector3 ballFillerTransform, int amount, System.Action onAnimationEnd = null)
	{
//		Trace.Msg (scoreText.transform.position);
		float animationTime = 0;

        //		var lastFlooredValue = 0;
        //		LeanTween.value (0, amount).setOnUpdate ((value) => {
        //			var flooredAmount = Mathf.FloorToInt(value);
        //			if(flooredAmount > lastFlooredValue) {
        //				lastFlooredValue = flooredAmount;
        //
        //				Trace.Msg ("Moving Coin");
        //				// TODO: Create coin and increase coins 
        //				GameObject coin = Instantiate(coinPrefab) as GameObject;
        //				Transform parentTransform = parentForCoins;
        //				coin.transform.SetParent(parentTransform, false);
        //
        //				coin.transform.position = ballFillerTransform;
        //				coin.transform.localScale = Vector3.zero;
        //
        //				//Scale and Move coin
        //				//            var endgameCoinMoveDuration = GPM.Instance.endgameCoinMoveDuration;
        //
        //				LeanTween.scale(coin, Vector3.one, endgameCoinMoveDuration * .4f ).setEase(GPM.Instance.endgameCoinMoveEasing)
        //					.setDelay(animationTime);
        //				LeanTween.move (coin, coinsIcon.transform.position, endgameCoinMoveDuration).setEase (GPM.Instance.endgameCoinMoveEasing)
        //					.setDelay (animationTime)
        //					.setOnComplete (() => {
        //						//					coinBox.Amount++;
        //						CoinsEarned++;
        //					});
        //				animationTime += endgameCoinMoveDuration;
        //
        //				//Scale up and down the coin
        //				LeanTween.scale(coin, Vector3.one * 1.2f, endgameCoinMoveDuration / 2).setEase(GPM.Instance.endgameCoinMoveEasing)
        //					.setDelay(animationTime);
        //
        //				animationTime += endgameCoinMoveDuration / 2;
        //
        //				LeanTween.scale(coin, Vector3.zero, endgameCoinMoveDuration / 2).setEase(GPM.Instance.endgameCoinMoveEasing)
        //					.setDelay(animationTime)
        //					.setOnComplete(()=>{
        //
        //						Destroy(coin);
        //
        //					});
        //
        //
        //			}
        //		});
        //
        //		return;

		for (int i = 0; i < amount; i++) {

			Trace.Msg ("Moving Coin");
			// TODO: Create coin and increase coins 
			GameObject coin = Instantiate(coinPrefab) as GameObject;
			Transform parentTransform = parentForCoins;
			coin.transform.SetParent(parentTransform, false);

			coin.transform.position = ballFillerTransform;
			coin.transform.localScale = Vector3.zero;

			//Scale and Move coin
//            var endgameCoinMoveDuration = GPM.Instance.endgameCoinMoveDuration;

			LeanTween.scale(coin, Vector3.one, endgameCoinMoveDuration * .4f ).setEase(GPM.Instance.endgameCoinMoveEasing)
				.setDelay(animationTime);
			LeanTween.move (coin, coinsIcon.transform.position, endgameCoinMoveDuration).setEase (GPM.Instance.endgameCoinMoveEasing)
				.setDelay (animationTime)
				.setOnComplete (() => {
//					coinBox.Amount++;
                    CoinsEarned++;
				});
			animationTime += endgameCoinMoveDuration;

			//Scale up and down the coin
			LeanTween.scale(coin, Vector3.one * 1.2f, endgameCoinMoveDuration / 2).setEase(GPM.Instance.endgameCoinMoveEasing)
				.setDelay(animationTime);
            
			animationTime += endgameCoinMoveDuration / 2;

			LeanTween.scale(coin, Vector3.zero, endgameCoinMoveDuration / 2).setEase(GPM.Instance.endgameCoinMoveEasing)
				.setDelay(animationTime)
				.setOnComplete(()=>{

					Destroy(coin);

				});

			// Delay between coins
			animationTime = delayBetweenCoins;
		}
        Utility.Instance.DoAfter (animationTime, onAnimationEnd);

	}

	public void InitBallFillers(int[] ballCounts)
	{
		for (int i = 0; i < ballCounts.Length; i++) 
		{
			ballFillers [i].BallCount = ballCounts [i];
		}
	}

	public void SetCoins(int coinsCount)
	{
		coinBox.Amount = coinsCount;
//		MoveCoins (rewardedAdButton.transform.position, GPM.Instance.videoAdRewardCoinValue);

	}

    public void RefreshAchievements(List<Achievement> newAchievements)
	{
//		if (GameSparksManager.Instance.player == null)
//		{
//			return;
//		}

		foreach (var achievementEntry in achievementEntries) 
		{
            if( newAchievements.Contains(achievementEntry.Achievement) )
            {
                achievementEntry.Init(true);
            }
            else
            {
                achievementEntry.Init(false);
            }
//			achievementEntry.LoadWithEarnedArray (GameSparksManager.Instance.player.earnedAchievements);
		}
	}

	public void ShowRewardedVideo() {
        Debug.Log( "Showing Rewarded Video" );
        Trace.Msg( "Showing Rewarded Video" );
        // IOSNativePopUpManager.showMessage( "Rewarded Video", "Rewarded video should appear in a moment" );

        // Stop Audio clip
        AudioManager2.Instance.Stop(activeAudioClip, true);
		// Ad Button deactivated
		rewardedAdButton.interactable = false;

		AppLovinManager.Instance.ShowAwardedInter ((success) => {
            // Start Audio Clip
            activeAudioClip = AudioManager2.Instance.Play(SoundsManager.Instance.endgameMusic, AudioClipExtended.AudioType.Music);
            // Success
			if(success){
				AppLovinManager.Instance.setRewardedVideoWatch ( true );
				// IOSNativePopUpManager.showMessage( "Rewarded Video", "Rewarded video should have been played" );
				MetaManager.Instance.UpdateMeta(MetaManager.MetaType.coins, CoinsEarned, new object[]{true, false});
				Utility.Instance.DoAfter(1, ()=>{
                    MoveCoins(rewardedAdButton.transform.position, CoinsEarned);
				});
			}
            // Fail
			else {
                rewardedAdButton.interactable = true;
                // IOSNativePopUpManager.showMessage( "Rewarded Video", "Video Err, Alp's note is: Awarded inter is not loaded yet :(" );
				DebugConsole.Log("Awarded inter is not loaded yet :(");
			}
		});
	}

	public void Share(int score) {
		UIManager.Instance.SetShowLoading (true);
		Utility.Instance.DoAfter (1, () => {
			UIManager.Instance.SetShowLoading(false);
		});

		UIManager.Instance.TakeScreenShot ((texture)=>{
//			IOSSocialManager.Instance.FacebookPost("Just scored " + score.ToString() + " at Vodoo Mambo! ", null, texture);
			IOSSocialManager.Instance.ShareMedia("Just scored " + score.ToString() + " at Vodoo Mambo! ", texture);

//			IOSSocialManager.Instance.ShareMedia("Vodoo Mambo! is great.", texture );

//			StartCoroutine(TakeScreenshot());
		});
	}

//	private IEnumerator TakeScreenshot() 
//	{
//
//		UIManager.Instance.SetShowLoading (true);
//		yield return new WaitForEndOfFrame();
//
//		var width = Screen.width;
//		var height = Screen.height;
//		var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
//		// Read screen contents into the texture
//		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
//		tex.Apply();
//		byte[] screenshot = tex.EncodeToPNG();
//
//		var wwwForm = new WWWForm();
//		wwwForm.AddBinaryData("image", screenshot, "Screenshot.png");
//
////		FB.API("me/photos", Facebook.HttpMethod.POST, APICallback, wwwForm);
//		FB.API("me/photos", Facebook.Unity.HttpMethod.POST, (result)=>{
//
//			UIManager.Instance.SetShowLoading (false);
//			print(result);
//
//		}, wwwForm);
//	}
}


