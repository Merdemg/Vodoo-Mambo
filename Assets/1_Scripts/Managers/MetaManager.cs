//#define USE_LOGS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaManager : MonoBehaviour
{

    #region Singleton

    public static MetaManager Instance { private set; get; }

    void Awake()
    {
        Instance = this;
    }

    #endregion

    public enum MetaType
    {
        name,
        coins,
        hiscore,
        weeklyScore,
        achievements,
        medal,
        ballCounts,
        rank
    }

    [SerializeField]string basePrefString = "pitchpincher_localmeta_";
    [Header("Info")]
    [SerializeField]string _localName;
    [SerializeField]int _localCoins;
    [SerializeField]string[] _localAchievements;

    void Start()
    {
        _localName = (string)GetLocal(MetaType.name);
        _localCoins = (int)GetLocal(MetaType.coins);
        _localAchievements = (string[])GetLocal(MetaType.achievements); 
    }

    private object GetLocal(MetaType type)
    {
        Trace.Msg("Getting local meta type: " + type.ToString());
        string prefString = basePrefString + type.ToString();

        object returnObject;
        switch (type)
        {
            case MetaType.name:
                
                returnObject = PlayerPrefs.GetString(prefString, "");
                break;
            case MetaType.coins:
                
                returnObject = PlayerPrefs.GetInt(prefString, 0);
                break;
            case MetaType.achievements:
                returnObject =  PlayerPrefsX.GetStringArray(prefString, "", 0);
                break;
            case MetaType.hiscore:
                returnObject =  PlayerPrefs.GetInt(prefString, 0);
                break;
            case MetaType.weeklyScore:
                returnObject =  PlayerPrefs.GetInt(prefString, 0);
                break;
            case MetaType.medal:
                returnObject =  PlayerPrefsX.GetIntArray(prefString, 0, 3);

                break;
            case MetaType.ballCounts:
                returnObject =  PlayerPrefsX.GetIntArray(prefString, 0, GPM.Instance.ballMaxLevel + 1);
                foreach (var item in (int[])returnObject)
                {
                    Trace.Msg(item.ToString());
                }
                break;
            case MetaType.rank:
                returnObject =  PlayerPrefs.GetInt(prefString, 0);
                break;
            default:
                returnObject = null;
                break;
        }

        Trace.Msg("Value is " + returnObject);
        return returnObject;
    }

    private void SetLocal(MetaType type, object value)
    {
        Trace.Msg("Setting local meta type: " + type.ToString());
        string prefString = basePrefString + type.ToString();

        switch (type)
        {
            case MetaType.name:
                PlayerPrefs.SetString(prefString, (string)value);
                break;
            case MetaType.coins:
                PlayerPrefs.SetInt(prefString, (int)value);
                break;
            case MetaType.achievements:
                PlayerPrefsX.SetStringArray(prefString, (string[])value);
                break;
            case MetaType.hiscore:
                PlayerPrefs.SetInt(prefString, (int)value);
                break;
            case MetaType.weeklyScore:
                PlayerPrefs.SetInt(prefString, (int)value);
                break;
            case MetaType.medal:
                PlayerPrefsX.SetIntArray(prefString, (int[])value);
                break;
            case MetaType.ballCounts:
                PlayerPrefsX.SetIntArray(prefString, (int[])value);
                break;
            case MetaType.rank:
                PlayerPrefs.SetInt(prefString, (int)value);
                break;
            default:
                return;
        }
    }

    private void DeleteLocal(MetaType type)
    {
        Trace.Msg("Deleting local meta type: " + type.ToString());
        string prefString = basePrefString + type.ToString();

        PlayerPrefs.DeleteKey(prefString);
    }

    /// <summary>
	/// Additional parameters differ by metatype!
	/// //
	/// Coins: (isAdd:bool, updateUI:bool)
	/// //
    /// </summary>
    /// <param name="type">Type.</param>
    /// <param name="value">Value.</param>
    /// <param name="additionalParameters">
    /// </param>
    public void UpdateMeta(MetaType type, object value, object[] additionalParameters = null)
    {
        Trace.Msg("Updating Meta! Type: " + type.ToString());
        switch (type)
        {
            // Add or remove
            // first additional parameter: isAdd, default is true
            // seond additional parameter: updateUI, default is true
            case MetaType.coins: // 

                int coinsValue = (int)value;

                int currentCoins = (int)Get(MetaType.coins);

                // Additional parameters

                // Check if add - default: true
                bool isAdd = additionalParameters == null || additionalParameters.Length < 1
                    ? true : (bool)additionalParameters[0];
                // Check if should update UI - default: true
                bool updateUI = additionalParameters == null || additionalParameters.Length < 2
                    ? true : (bool)additionalParameters[1];

                // TODO: Set coins diye bi sey yap
                // Server
                if (isAdd)
                {
                    GameSparksManager.Instance.AddCoins(coinsValue); 
                }
                else
                {
                    GameSparksManager.Instance.RemoveCoins(coinsValue); 
                }

                // Add or Remove
                if (!isAdd)
                {
                    coinsValue = ( -1 * coinsValue );
                }

                // Refresh UI
                if (updateUI)
                {
                    UIManager.Instance.RefreshCurrencyTexts(currentCoins + coinsValue);
                }

                Trace.Msg(coinsValue);

                SetLocal(MetaType.coins, currentCoins + coinsValue); 
                break;

            // Add
            case MetaType.achievements:

                var shortCode = ((Achievement)value).shortCode;
//                string achievementShortCode = 

                GameSparksManager.Instance.Achieve(shortCode);

                List<string> currentLocalAchievements = new List<string>((string[])GetLocal(MetaType.achievements));
//
                // Add value to currentLocalAchievements
                currentLocalAchievements.Add(shortCode);
                SetLocal(MetaType.achievements, currentLocalAchievements.ToArray());

                // Send achievement 


                break;
            
            case MetaType.hiscore:
                
                Debug.LogError("Can't update hiscore directly, update weeklyscore!");
                break;

            // Network: Post
            // Local: Replace if bigger
            case MetaType.weeklyScore:

                int score = (int)value;

                GameSparksManager.Instance.PostScore(score);
                GameCenterManager.Instance.PostScoreOnLeaderBoard(score);

                // Set local score only when online. Use this to update server score when first logged in.
                int localWeeklyScore = (int)GetLocal(MetaType.weeklyScore);
                if (!GameSparksManager.Instance.IsLoggedIn &&  score > localWeeklyScore)
                {
                    SetLocal (MetaType.weeklyScore, score);
                }

                // Hiscore
                if (score > (int)GetLocal(MetaType.hiscore))
                {
                    SetLocal(MetaType.hiscore, score);
                }
                break;

            // Network: Post
            case MetaType.ballCounts:

                int[] ballcounts = (int[])value;
//                int[] currentBallCounts = (int[])Get(MetaType.ballCounts);

//                for (int i = 0; i < ballcounts.Length; i++)
//                {
//                    ballcounts[i] += currentBallCounts[i];
//                }

                GameSparksManager.Instance.SetBallCounts(ballcounts);
                SetLocal(MetaType.ballCounts, ballcounts);

                break;

            default:
                Debug.LogError("Can't update "+ type.ToString() + "!");
                return;
        }
    }

    public object Get(MetaType type)
    {
        Trace.Msg("Getting meta type: " + type.ToString());
        if(GameSparksManager.Instance.IsLoggedIn)
        {

            switch (type)
            {
                case MetaType.name:
                    return GameSparksManager.Instance.player.name;
                case MetaType.coins:
                    return GameSparksManager.Instance.player.coins;
                case MetaType.achievements:
                    return GameSparksManager.Instance.player.earnedAchievements.ToArray();
                case MetaType.hiscore:
                    return GameSparksManager.Instance.player.highscore;
                case MetaType.medal:
                    return GameSparksManager.Instance.player.medals;
                case MetaType.ballCounts:
                    return GameSparksManager.Instance.player.ballCounts.ToArray();
                case MetaType.rank:
                    return GameSparksManager.Instance.player.bestRank;
                default:
                    return null;
            }
        }
        else
        {
            return GetLocal(type);
        }
    }

    public void SyncMeta(MetaType type)
    {
        Trace.Msg("Syncing Meta! Type: " + type.ToString());
        if(GameSparksManager.Instance.player == null)
        {
            Debug.LogError("Can't sync meta, is offline!!");
        }

        switch (type)
        {
            // Local -> Server
            case MetaType.name:
                SetLocal(MetaType.name, GameSparksManager.Instance.player.name);

                break;

            // Local -> Server
            case MetaType.coins:

                Trace.Msg("Meta coin sync");
                int localCoins = (int)GetLocal(MetaType.coins);

                Trace.Msg("Local: " + localCoins);
                Trace.Msg("Server: " + GameSparksManager.Instance.player.coins);

                if(localCoins > 0)
                {
                    GameSparksManager.Instance.SetCoins(localCoins);
                }

//                if(localCoins > GameSparksManager.Instance.player.coins)
//                {
//                    Trace.Msg("Local coins are bigger");
//                    GameSparksManager.Instance.SetCoins(localCoins);
//                }
//                else 
//                {
//                    Trace.Msg("Server coins are bigger or same");
//                    SetLocal(MetaType.coins, GameSparksManager.Instance.player.coins);
//                }

                break;

            // Local <-> Server (bigger)
            case MetaType.hiscore:

                Trace.Msg("Meta hiscore sync");
                int localHiscore = (int)GetLocal(MetaType.hiscore);

                if (localHiscore > GameSparksManager.Instance.player.highscore)
                {
                    Trace.Msg("Local hiscore is bigger");
                    GameSparksManager.Instance.SetHiscore(localHiscore);
                }
                else
                {
                    Trace.Msg("Server hiscore is bigger or same");
                    SetLocal(MetaType.hiscore, GameSparksManager.Instance.player.highscore);
                }

                break;

            // Local -> Server
            case MetaType.weeklyScore:

                int localScore = (int)GetLocal(MetaType.weeklyScore);

                // Post score if it exist and reset local weekly score
                if( localScore > 0)
                {
                    Trace.Msg("Local score exists, posting");
                    GameSparksManager.Instance.PostScore(localScore);

//                    SetLocal(MetaType.weeklyScore, 0);
                    DeleteLocal(MetaType.weeklyScore);
                }

                break;

            // Server -> Local
            case MetaType.rank:
                int localRank = (int)GetLocal(MetaType.rank);

                if(GameSparksManager.Instance.player.bestRank < localRank)
                {
                    Trace.Msg("Updating best rank!");
                    SetLocal(MetaType.rank, GameSparksManager.Instance.player.bestRank);
                }
                break;

            // Server <-> Local
            case MetaType.achievements:

                string[] localAchievementsShortcodes = (string[])GetLocal(MetaType.achievements);
                List<string> serverAchievementsShortcodes = new List<string>(GameSparksManager.Instance.player.earnedAchievements);
//                List<string> serverAchievementsShortcodes = GameSparksManager.Instance.player == null ?
//                    new List<string>() :
//                    new List<string>(GameSparksManager.Instance.player.earnedAchievements);


                foreach (var localAchShortCode in localAchievementsShortcodes)
                {
                    if (!serverAchievementsShortcodes.Contains(localAchShortCode))
                    {
//                        Achievement ach = AchievementManager.Instance.GetAchievementByShortCode(localAchShortCode);
                        GameSparksManager.Instance.Achieve(localAchShortCode);
                    }
                }

                SetLocal(MetaType.achievements, (object)serverAchievementsShortcodes.ToArray() );

//                int localRank = (int)GetLocal(MetaType.rank);
//
//                if(GameSparksManager.Instance.player.bestRank < localRank)
//                {
//                    Trace.Msg("Updating best rank!");
//                    SetLocal(MetaType.rank, GameSparksManager.Instance.player.bestRank);
//                }
                break;

            // Server -> Local
            case MetaType.medal:    

                Trace.Msg("Updating medals!");
                SetLocal(MetaType.medal, GameSparksManager.Instance.player.medals.ToArray());

                break;

            case MetaType.ballCounts:

                int[] localBallCounts = (int[])GetLocal(MetaType.ballCounts);

                int totalLocalBallCount = 0;
                foreach (var ct in localBallCounts)
                {
                    totalLocalBallCount += ct;
                }

                int[] serverBallCounts = GameSparksManager.Instance.player.ballCounts.ToArray();

                int totalServerBallCount = 0;
                foreach (var ct in serverBallCounts)
                {
                    totalServerBallCount += ct;
                }


                if(totalLocalBallCount > totalServerBallCount)
                {
                    Trace.Msg("Local totalBallCount are bigger");
                    GameSparksManager.Instance.SetBallCounts(localBallCounts);
                }
                else if(totalLocalBallCount < totalServerBallCount)
                {
                    Trace.Msg("Server totalBallCount are bigger");
                    SetLocal(MetaType.ballCounts, serverBallCounts);

                    foreach (var item in serverBallCounts)
                    {
                        Trace.Msg(item.ToString());
                    }
                }
                else
                {
                    Trace.Msg("Server and local totalBallCount are same!");
                }
                break;
            
            default:
                return;
        }
    }

    public void SyncAllMeta()
    {
        Trace.Msg("Syncing all meta");
        foreach (MetaType item in System.Enum.GetValues(typeof(MetaType)))
        {
            SyncMeta(item);
        }
    }
}
