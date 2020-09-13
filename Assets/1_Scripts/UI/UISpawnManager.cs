using UnityEngine;
using System.Collections;

/// <summary>
/// OBSELETE!!
/// </summary>
public class UISpawnManager : MonoBehaviour
{

	#region Singleton
	public static UISpawnManager Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	#endregion

    public GameObject LeaderboardEntryPrefab;
	public GameObject LeaderboardEntryMainMenuPrefab;

    /// <summary>
    /// OBSELETE!!
    /// </summary>
    /// <returns>The leaderboard entry.</returns>
	public GameObject SpawnLeaderboardEntry()
	{
		GameObject leaderboardEntryGameObject = Instantiate(LeaderboardEntryPrefab, Vector3.zero, Quaternion.identity) as GameObject;

		// TODO: Is this necessary?
		leaderboardEntryGameObject.transform.SetParent(UIManager.Instance.mainCanvas.transform, false);
		return leaderboardEntryGameObject;
	}

}

