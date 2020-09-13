using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour {

	public static AchievementManager Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	public Achievement[] achievements;
	public Achievement[] endGameAchievements;
	public Achievement[] loggedInAchievements;

	public void Init()
	{
		foreach (var achievement in achievements)
		{
			achievement.Init ();
		}
//		UIManager.Instance.InitAchievementPanel ();
	}

    /// <summary>
    /// Checks the end game achievements.
    /// </summary>
    /// <returns>New achieved achievements.</returns>
    public Achievement[] CheckEndGameAchievements()
	{
//		if (GameSparksManager.Instance.player == null)
//			return;
		
        List<Achievement> newAchievements = new List<Achievement>();

		foreach (var achievement in endGameAchievements) 
		{
			bool isNew = achievement.CheckNew ();

            if(isNew)
            {
                newAchievements.Add(achievement);
            }
		}
        return newAchievements.ToArray();
	}

	public void CheckLoggedInGameAchievements()
	{
		if (GameSparksManager.Instance.player == null)
			return;
		
		foreach (var achievement in loggedInAchievements) 
		{
			achievement.CheckNew ();
		}
	}

    public Achievement GetAchievementByShortCode(string code)
    {
        foreach (var ach in achievements)
        {
            if (ach.shortCode == code)
                return ach;
        }
        return null;
    }

}
