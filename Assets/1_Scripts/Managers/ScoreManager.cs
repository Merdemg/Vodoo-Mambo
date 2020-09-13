using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour
{

	#region Singleton
	public static ScoreManager Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	#endregion
//    [Header("Parameters")]
//    [SerializeField]private string highscorePlayerPrefName = "pitchpincher_localhighscore";
    [Header(" - Score Types")]
	public int PinchScore;
	public int ExplodeScore;
	public int GameEndExplodeScore;
	public int BlackholeScore;
    [Header(" - Multipliers")]
//    public float scoreLevelIncrementMultiplier = .2f;
//	public float scoreTrickMultiplier = 1.2f;

    [Header(" - Combo")]
    [SerializeField] float comboTimeLimit;
    [SerializeField] int comboMultiplier;
    public int ComboMultiplier
    {
        get{
            return comboMultiplier;
        }
        set{
            comboMultiplier = value;
            UIManager.Instance.SetInGameComboText(value);
        }
    }

    [Space]
    public int minimumComboMultiplier;
    public int maximumComboMultiplier;

    [Header("Info")]
    // TODO: Debug 
    public float activeComboTimeRemaining = 1;
    [SerializeField]private int _score;
    public int Score{
        get{
            return _score;
        }
        private set{
            _score = value;
            UIManager.Instance.SetIngameScore(value);
        }
    }

    /// <summary>
	/// Used as an extention to Awake for organizational purposes.
	/// </summary>

	void Update() {
		ComboTimerUpdate ();
	}

	// Combo
	private void ComboTimerUpdate()
	{
		if(GameManager.Instance.GameplayState == GameplayState.Playing)
		{
//			Trace.Msg ("UpdateComboTime");
			activeComboTimeRemaining -= Time.deltaTime;
			
			if (activeComboTimeRemaining < 0)
			{
                activeComboTimeRemaining = comboTimeLimit;
                ResetComboMultiplier();
			}
		}
	}

    public void IncrementComboMultiplier(Vector3? position, bool increment = false)
	{
        // Reset time
		activeComboTimeRemaining = comboTimeLimit;

        // Increase
		if (ComboMultiplier < maximumComboMultiplier)
		{
            if(increment)
			    ComboMultiplier *= 2;

//            // Combo
//            if(position != null && increment)
//            {
//                SpawnManager.Instance.SpawnComboTextAtPosition((Vector3)position, ComboMultiplier);
//
//            }

            // Max Combo

            UIManager.Instance.Vignette((float)comboMultiplier/(float)maximumComboMultiplier);
            if(ComboMultiplier == maximumComboMultiplier)
            {
            	string shamanMessage = Lean.Localization.LeanLocalization.GetTranslationText( "Shaman--Nice" );
				UIManager.Instance.ingameShaman.Show( shamanMessage );
			}
		}
	}

    public void DecreaseComboMultiplier()
    {
        ComboMultiplier /= 2;

        if (ComboMultiplier < minimumComboMultiplier)
            ComboMultiplier = minimumComboMultiplier;
    }
		
	public void ResetComboMultiplier()
	{
		ComboMultiplier = minimumComboMultiplier;
        UIManager.Instance.Vignette(0);
	}

	// Score
    public void AddScore(ScoreType type, Vector3? position = null, int? level = null, bool canCombo = true, bool fillBar = true)
	{
		int scoreToAdd = 0;

		switch (type) {
		case ScoreType.Pinch:
			scoreToAdd += PinchScore;
			break;
		case ScoreType.Explode:
			scoreToAdd += ExplodeScore;
			break;
		case ScoreType.Blackhole:
			scoreToAdd += BlackholeScore;
			break;
		case ScoreType.GameEndExplode:
			scoreToAdd += GameEndExplodeScore;
			break;
		default:
			break;
		}

		if(level != null)
		{
            scoreToAdd *= (int)level + 1;

            // (1 + ( level * scoreLevelIncMul ) )
//			scoreToAdd = scoreToAdd + (int)(level * scoreLevelIncrementMultiplier * scoreToAdd);
		}

		scoreToAdd *= ComboMultiplier;

        // Score larin arasini acmak icin trick
//        if(comboMultiplier == maximumComboMultiplier)
//        {
//            scoreToAdd = (int)(scoreTrickMultiplier * scoreToAdd);
//        }

		if(position != null)
		{
			SpawnManager.Instance.SpawnScoreTextAtPosition ((Vector3)position, scoreToAdd);
		}

		Score += scoreToAdd;

        if(fillBar) {
			GameManager.Instance.newTimer.ScoreUpdated();
        }
            

//        if(canCombo)
            IncrementComboMultiplier (position, canCombo);
        
	}

    public int GetComboLevel()
    {
        int comb = ComboMultiplier;
        int lvl = 0;
        while(comb >= 2)
        {
            lvl++;
            comb /= 2;
        }

        return lvl;
    }

	public void ResetScore()
	{
		Score = 0;
	}

//	public int GetLocalHighScore()
//	{
//		return PlayerPrefs.GetInt (highscorePlayerPrefName, 0);
//	}
//
//	/// <summary>
//	/// Returns local highscore if not logged in
//	/// </summary>
//	/// <returns>The highscore.</returns>
//	public int GetHighscore()
//	{
////		int localHighscore = 0;
////		if(PlayerPrefs.HasKey(highscorePlayerPrefName))
////		{
////			localHighscore = PlayerPrefs.GetInt (highscorePlayerPrefName);
////		}
//
//		int highscore = 0;
//		if(GameSparksManager.Instance.player != null)
//		{
//			highscore = GameSparksManager.Instance.player.highscore;
//		}
//		else
//		{
//			highscore = GetLocalHighScore ();
//		}
//
//		Trace.Msg ("Returning highscore, " + highscore);
//		return highscore;
//		// Local highscore is the prioritised
////		if( localHighscore >= serverHighscore)
////		{
////			return localHighscore;
////		}
////		else
////		{
////			return serverHighscore;
////		}
//
////		return PlayerPrefs.GetInt (highscorePlayerPrefName);
//	}
//
//	public void SetLocalHighscore(int score)
//	{
//		_localHighscore = score;
//
//		Trace.Msg (("Local Highscore Set score: "+score).Colored (Colors.fuchsia));
//		PlayerPrefs.SetInt (highscorePlayerPrefName, score);
//	}
//
//	public void DeleteLocalHighscore()
//	{
//		Trace.Msg (("Local Highscore Deleted!").Colored (Colors.fuchsia));
//		PlayerPrefs.DeleteKey (highscorePlayerPrefName);
//	}
//
//	public void obs_SetHighscore()
//	{
//		GameSparksManager.Instance.PostScore(Score);
//		GameCenterManager.Instance.PostScoreOnLeaderBoard (Score);
//
//		// Local
//		if(_score > _localHighscore)
//		{
//			SetLocalHighscore (_score);
//		}
//
//		_localHighscore = _score;
//
//	}

}

