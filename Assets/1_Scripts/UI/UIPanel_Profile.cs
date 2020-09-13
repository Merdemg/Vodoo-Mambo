using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel_Profile : UIPanel {
	[Header("- Parameters")]
	[SerializeField]Text nameText;
	[SerializeField]Text highscoreText;
    [SerializeField]Text rankText;
    [SerializeField]Button loginButton;
	[Header("Medals")]
	[SerializeField]Text medalGoldCountText;
	[SerializeField]GameObject medalGoldCountImage;
	[Space]
	[SerializeField]Text medalSilverCountText;
	[SerializeField]GameObject medalSilverCountImage;
	[Space]
	[SerializeField]Text medalBronzeCountText;
	[SerializeField]GameObject medalBronzeCountImage;
	[Space]
	[SerializeField]Transform achievementsParent;
	[Header("- Info")]
	[SerializeField]UIAchievementEntry[] achievementEntries;

    protected override void Start()
    {
        base.Start();

        achievementEntries = achievementsParent.GetComponentsInChildren<UIAchievementEntry> ();

        loginButton.gameObject.SetActive (false);

        loginButton.onClick.AddListener (() => {
            loginButton.gameObject.SetActive(false);
            FacebookManager.Instance.Login();
        });

        FacebookManager.Instance.LoginEvent += (isLoggedIn) =>
        {
                loginButton.gameObject.SetActive (!isLoggedIn);  
        };
    }


//	virtual protected void Start()
//	{
//		base.Start ();
//		
//	}

	public void notUsing_LoadProfile(string playerId, string name, string rank, string score, int[] medals)
	{
//		panel.SetActive (true);

		nameText.text = name;
//		pictureImage.sprite = picture;
		highscoreText.text = score;
        // if rank is zero, write -
//        rankText.text = rank == 0 ? "-" : rank;


		GameSparksManager.Instance.GetAchivementsByID (playerId, (List<string> earnedAchievementsList) => {

			ShowAchievements(earnedAchievementsList);
		});

		//		ShowAchievements (playerId);
	}

	public void LoadProfile(string playerId, string name) {
		// Name
		nameText.text = name;

		// Medals
		// Clear
		medalGoldCountText.text = "";
		medalSilverCountText.text = "";
		medalBronzeCountText.text = "";

		medalGoldCountImage.SetActive(false);
		medalSilverCountImage.SetActive(false);
		medalBronzeCountImage.SetActive(false);
		// Get new info
		GameSparksManager.Instance.GetMedalsByID (playerId, (medalCountArray) => {

			medalGoldCountText.text = medalCountArray[0].ToString();
			medalSilverCountText.text = medalCountArray[1].ToString();
			medalBronzeCountText.text = medalCountArray[2].ToString();

			medalGoldCountImage.SetActive(medalCountArray[0] > 0);
			medalSilverCountImage.SetActive(medalCountArray[1] > 0);
			medalBronzeCountImage.SetActive(medalCountArray[2] > 0);
		});
		// Rank & Highscore
		// Clear first
		rankText.text = "";
		highscoreText.text = "";
		// Get new
		GameSparksManager.Instance.GetBestRankAndScoreByID (playerId, (rankAndScoreArray) => {

            int rank = rankAndScoreArray[0];
            int hiscore = rankAndScoreArray[1];

            // if rank is zero, write -
            rankText.text = rank == 0 ?
                "-" : 
                "#" + rankAndScoreArray[0].ToString();
            
            highscoreText.text = hiscore.ToString();

		});
		// Achievements
		// Clear first
		foreach (var item in achievementEntries) 
		{
			item.gameObject.SetActive (false);
		}
		// Get new
		GameSparksManager.Instance.GetAchivementsByID (playerId, (List<string> earnedAchievementsList) => {

			ShowAchievements(earnedAchievementsList);
		});
	}

	public void LoadProfileLocalAndOpen()
	{
		Open ();
		if(GameSparksManager.Instance.player != null)
		{
			LoadProfile (GameSparksManager.Instance.player.userId, GameSparksManager.Instance.player.name);
		}
		else
		{
            int[] medalCountArray = (int[])MetaManager.Instance.Get(MetaManager.MetaType.medal);
         

            nameText.text = (string)MetaManager.Instance.Get(MetaManager.MetaType.name);
            highscoreText.text = ((int)MetaManager.Instance.Get(MetaManager.MetaType.hiscore)).ToString();
            rankText.text = "#" + ((int)MetaManager.Instance.Get(MetaManager.MetaType.rank)).ToString();
            medalGoldCountText.text = medalCountArray[0].ToString();
            medalSilverCountText.text = medalCountArray[1].ToString();
            medalBronzeCountText.text = medalCountArray[2].ToString();

            medalGoldCountImage.SetActive(medalCountArray[0] > 0);
            medalSilverCountImage.SetActive(medalCountArray[1] > 0);
            medalBronzeCountImage.SetActive(medalCountArray[2] > 0);
		}
	}

	private void ShowAchievements(List<string> earnedAchievementsList)
	{
		foreach (var achievementEntry in achievementEntries) 
		{
			achievementEntry.LoadWithEarnedArray (earnedAchievementsList);
		}
	}
}
