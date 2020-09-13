//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using System.Collections.Generic;
//
//public class UI_PlayerProfile : MonoBehaviour {
//
//	public static UI_PlayerProfile i { private set; get; }
//
//	void Awake()
//	{
//		i = this;
//	}
//
//	public GameObject panel;
//	public Image pictureImage;
//	public Text nameText;
//	public GameObject achivements;
//	public Text highscoreText;
//	public Text rankText;
//	public Text goldMedalCountText;
//	public Text silverMedalCountText;
//	public Text bronzeMedalCountText;
//
//	public void LoadProfile(string playerId, string name, Sprite picture, string rank, string score, int[] medals)
//	{
//		panel.SetActive (true);
//
//		nameText.text = name;
//		pictureImage.sprite = picture;
//		highscoreText.text = score;
//		rankText.text = rank;
//
//		GameSparksManager.Instance.GetAchivementsByID (playerId, (List<string> earnedAchievementsList) => {
//
//			ShowAchievements(earnedAchievementsList);
//		});
//
//		//		ShowAchievements (playerId);
//	}
//
//	public void LoadProfile(string playerId, string name, Sprite picture)
//	{
//		panel.SetActive (true);
//
//		//TODO ask for player info to show on panel
//
//		nameText.text = name;
//		pictureImage.sprite = picture;
//
//		// Medals
//		GameSparksManager.Instance.GetMedalsByID (playerId, (medalCountArray) => {
//
//			goldMedalCountText.text = medalCountArray[0].ToString();
//			silverMedalCountText.text = medalCountArray[1].ToString();
//			bronzeMedalCountText.text = medalCountArray[2].ToString();
//
//		});
//
//		// Rank & Highscore
//		GameSparksManager.Instance.GetBestRankAndScoreByID (playerId, (rankAndScoreArray) => {
//
//			rankText.text = rankAndScoreArray[0].ToString();
//			highscoreText.text = rankAndScoreArray[1].ToString();
//
//		});
//
//		// Achievements
//		GameSparksManager.Instance.GetAchivementsByID (playerId, (List<string> earnedAchievementsList) => {
//
//			ShowAchievements(earnedAchievementsList);
//		});
//	}
//
//	public void Close()
//	{
//		panel.SetActive (false);
//	}
//
//	private void ShowAchievements(List<string> earnedAchievementsList)
//	{
//		foreach (var achievement in AchievementManager.Instance.achievements) 
//		{
//			achievement.LoadProfileEntryWithEarnedArray (earnedAchievementsList);
//		}
//	}
//}
