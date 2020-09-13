using UnityEngine;
using System.Collections;

// GAME PARAMETERS MANAGER
public class GPM : MonoBehaviour
{
	#region Singleton
	public static GPM Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	#endregion

	public float gameplayTime = 60f;


	[Header("Game Play")]
	public int ballCountLimit = 10;
    public float ballSpawnInterval = 1f;
	public float ballSpawnIntervalMultiplierMax = 4;
	public int ballMaxLevel = 3;
	public float iPhoneCameraSizeModifier = 0.5f;

	[Header("Ball Speed")]
	public float ballVelocityMagnitudeLimit = 10f;
    public float ballNormalVelocity = 3f;
	public float ballNormalEndGameVelocity = 3f;

	[Header("Ball Size")]
	public Vector3 ballDeadSize = Vector3.zero;
	public Vector3 ballNormalSize = Vector3.one;
//	public float ballLevelledMaxSizeFactor = 1.2f;
	public float ballResizeDuration = .75f;
	public float ballDieDuration = .1f;
    public float[] BallColliderRadiuses;
    public float ballNormalEffectorMagnitude = 5;
	public float ballHoldingEffectorMagnitude = 25;

	[Header("Ball Pinch")]
	public float ballPairedReleaseVelocityMagnitude = 4f;
	public float ballPairLifeAfterRelease = .1f;
//	public float pinchDistanceThreshold = .2f;
	public int maxLastReleasedBalls = 4;

    [Header("Ball Hit Reaction")]
    public float bhr1 = .7f;
    public float bhr2 = 1f;
    public LeanTweenType bhr3 = LeanTweenType.easeOutElastic;

	[Header("Animation Timing")]
	public float ballMergeAnimationCrossTime = .8f;
	public float ballMergeFadeInDuration = .2f;
	[Space]
	public float ballShowDuration = .75f;
	public LeanTweenType ballShowEasing = LeanTweenType.easeOutElastic;
	public float ballIconShowDuration = .1f;
	public LeanTweenType ballIconShowEasing = LeanTweenType.easeOutElastic;

	[Header("PowerUp")]
	public float powerUpDestroySize = 0.1f;
	public float powerUpDestroyDuration = 0.4f;

	[Header("Meta")]
	public int[] ballRecordMaxValues;
	public int minFriendCountForSocial = 5;
	public int[] goldPerBallCountByLevel = new int[]{1,3,6,10};

	[Header("Interaction")]
	public float holdThresholdSqr = 1;
	public float ballHoldResponseMultiplier = 1f;

	public int RandomBallLevel 
	{
		get{

			//Get random ball level
			int randomLevel = 0;
			float rng = Random.Range(0,1f);
			if (rng > .98f)
				randomLevel = 2;
			else if (rng > .90f)
				randomLevel = 1;

			return randomLevel;
		}
	}

	[Header ("UI Parameters")]
	public float panelTransitionTime = .2f;
	public LeanTweenType panelTransitionType = LeanTweenType.easeInOutSine;
	[Space]
	public float ballFillDuration = 1f;
	public LeanTweenType ballFillEasing = LeanTweenType.easeInSine;
	[Space]
	public float ballFilledUpScale = 1.5f;
	public float ballFilledUpScaleDuration = 1.5f;
	public LeanTweenType ballFilledUpScaleEasing = LeanTweenType.easeInSine;
	[Space]
	public float endgameCoinMoveDuration = .1f;
	public LeanTweenType endgameCoinMoveEasing = LeanTweenType.easeInOutSine;
	[Space]
	public float toggleDuration = .2f;
	public LeanTweenType toggleEasing = LeanTweenType.easeInOutSine;
	[Space]
	public float leaderboardInDuration = .2f;
	public LeanTweenType leaderboardInEasing = LeanTweenType.easeInOutSine;
	[Space]
	public Color leaderboardLocalPlayerNameColor = Color.red;

}

