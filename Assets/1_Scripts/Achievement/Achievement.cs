using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Achievement : MonoBehaviour {

	public bool earned = false;
	public string name;
	public string description;
	public int coinValue;
	public string shortCode;
	public Sprite icon;
//	public AchievementEntry entry;

    void Start()
    {
        Init();
    }

	public void Init()
	{
		//Check if achievement earned
//        if(GameSparksManager.Instance.player.earnedAchievements.Contains(shortCode))

        string[] earnedAchievements = (string[])MetaManager.Instance.Get(MetaManager.MetaType.achievements);
        List<string> earnedAchievementsList = new List<string>(earnedAchievements);

        if (earnedAchievementsList.Contains(shortCode))
		{
			earned = true;
		}
		else
		{
			earned = false;
		}
	}


	/// <summary>
	/// Checks if achievement is achieved.
	/// </summary>
    public virtual bool CheckNew() {
        return false;
    }

	public virtual void Achieve()
	{
		earned = true;

        MetaManager.Instance.UpdateMeta(MetaManager.MetaType.coins, coinValue);
        MetaManager.Instance.UpdateMeta(MetaManager.MetaType.achievements, this);

		UIManager.Instance.newAchievementPanel.OpenWithAchievement (this);
//		UIManager.Instance.endGamePanel.RefreshAchievements ();
	}
}
