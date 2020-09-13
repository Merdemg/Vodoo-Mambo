#define USE_LOGS
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UILeaderBoard : MonoBehaviour
{
	[Header("Parameters")]
	public Transform initialPos;
    public GameObject leaderBoardPrefab;
	public List<UILeaderboardEntry> _leaderboardEntries;
	public List<UILeaderboardEntry> _localPlayerLBEntries;

	public RectTransform contentPanelGlobal;
	public RectTransform contentPanelSocial;
	public Text remainingTimeText;
	public Button loginButton;
//	public GameObject friendAlertPanel;
//	public Text friendAlertText;

	public GameObject[] uiTabs;

	void Awake() {
		loginButton.gameObject.SetActive (false);
	}

    void Start()
    {
        ChangeTab (0);

//        loginButton.gameObject.SetActive (false);

        loginButton.onClick.AddListener (() => {
            loginButton.gameObject.SetActive(false);
            FacebookManager.Instance.Login();
        });

        FacebookManager.Instance.LoginEvent += (isLoggedIn) =>
        {
                loginButton.gameObject.SetActive(!isLoggedIn);    
        };
    }

//	void Start()
//	{
//
//	}

	public void AddEntry(string playerId, string facebookId, int rank, string name, int score, bool isSocial)
	{
		RectTransform contentPanel = isSocial ? contentPanelSocial : contentPanelGlobal;

//		GameObject leaderboardEntryGameObject = UISpawnManager.Instance.SpawnLeaderboardEntry();
        GameObject leaderboardEntryGameObject = Instantiate(leaderBoardPrefab) as GameObject;
		leaderboardEntryGameObject.GetComponent<RectTransform>().SetParent(contentPanel, false);

		UILeaderboardEntry leaderboardEntry = leaderboardEntryGameObject.GetComponent<UILeaderboardEntry>();
		leaderboardEntry.Initialize(playerId, facebookId, rank, name, score);
		_leaderboardEntries.Add(leaderboardEntry);

		if(playerId == GameSparksManager.Instance.player.userId) {
			leaderboardEntry.MarkAsLocalPlayer ();
			contentPanel.anchoredPosition = new Vector2 (contentPanel.anchoredPosition.x, -leaderboardEntry.GetComponent<RectTransform> ().anchoredPosition.x);
		}
	}

	public void ResetEntries()
	{
        Trace.Msg("ResetEntries called on " + name);
		loginButton.gameObject.SetActive (false);

		foreach (UILeaderboardEntry leaderboardEntry in _leaderboardEntries)
		{
			Destroy(leaderboardEntry.gameObject);
		}

		_leaderboardEntries = new List<UILeaderboardEntry>();
	}

	/// <summary>
	/// Shows the friend alert if the friend count is less than x
	/// </summary>
//	public void ShowFriendAlert(List<string> friendIDs)
//	{
//		if(friendIDs.Count < 5)
//		{
//			friendAlertPanel.SetActive(true);
//			friendAlertText.text = "You need at least " + GPM.Instance.minFriendCountForSocial + " friends to earn weekly medals. \b You have " + friendIDs.Count + ". :(";
//		}
//	}


	public void InitRemainingTime(int seconds)
	{
		UIManager.Instance.StartCoroutine (RemainingTimeDrain (seconds));
	}

	public IEnumerator RemainingTimeDrain(int seconds)
	{
		float remainingTime = (float)seconds;
		float second = 0;
		int _remainingTime = seconds;

		while(true)
		{
			second += Time.unscaledDeltaTime;

			if(second >= 1)
			{
				second -= 1f;
				_remainingTime--;

				int[] timeArray = Utility.SecondsToDaysHoursMinutesSeconds (_remainingTime);

//				string timeString = timeArray [0] + " days : " + timeArray [1] + " hours : " + timeArray [2] + " minutes : " + timeArray [3] + " seconds";
				// string daysOrdays = timeArray[0] > 1 ? "days" : "day";
				// string timeString = "Resets in " + (timeArray [0] + 1) + " " + daysOrdays;
				// remainingTimeText.text = timeString;


				string resetTime = ( "" + (timeArray [0] + 1) ); // int-to-string
				string resetText = Lean.Localization.LeanLocalization
							.GetTranslationText( "EndGame--LeaderBoard-Reset" )
							.Replace( "%ResetTime%", resetTime );
				remainingTimeText.text = resetText;

			}

			yield return true;
		}
	}

	public void ChangeTab(int tabIndex)
	{
		for (int i = 0; i < uiTabs.Length; i++) {

			GameObject tab = uiTabs [i];

			if(i != tabIndex)
			{
				tab.SetActive (false);
			}
			else
			{
				tab.SetActive (true);
			}
		}
	}

	public void OnLoginFailed()
	{
		loginButton.gameObject.SetActive (true);
	}

	public void AnimateIn()
	{
        GetComponent<CanvasGroup>().alpha = 0;

		Utility.Instance.DoAfter (.1f, () => {

            Vector3 lbCachedPos = gameObject.transform.position;

            gameObject.transform.position = initialPos.position;

            GetComponent<CanvasGroup>().alpha = 1;

			LeanTween.move(gameObject, lbCachedPos, GPM.Instance.leaderboardInDuration).setEase(GPM.Instance.leaderboardInEasing);

		});
	}

	// BI sekilde calismadi - cok ilginc
//	public void DoInitLocalPlayer() {
//
//		foreach (var entry in _localPlayerLBEntries) {
//				entry.MarkAsLocalPlayer ();
//				//				var targetOffsetY = entry.GetComponent<RectTransform> ().anchoredPosition.y;
//				//				var parentLayoutGroup = transform.parent.GetComponent<RectTransform> ();
//				//				parentLayoutGroup.anchoredPosition = new Vector2 (parentLayoutGroup.anchoredPosition.x, -targetOffsetY);
//
//		}
//	}
//
//	public void InitLocalPlayer() {
//		Invoke ("DoInitLocalPlayer", 2f);
//		Utility.Instance.DoAfter (10, () => {
//
//			contentPanelGlobal.anchoredPosition = new Vector2 (contentPanelGlobal.anchoredPosition.x, 50);
//		});



//		List<UILeaderboardEntry> matchedEntries = new List<UILeaderboardEntry>();
//		foreach (var entry in _leaderboardEntries) {
//			if(entry.PlayerId == playerId) {
//				matchedEntries.Add (entry);
//			}
//		}
//		Utility.Instance.DoAfter (2, () => {
//			var matchCount = 0;
//			foreach (var entry in matchedEntries) {
//				matchCount ++;
//				entry.MarkAsLocalPlayer ();
//				var targetOffsetY = entry.GetComponent<RectTransform> ().anchoredPosition.y;
//				var parentLayoutGroup = transform.parent.GetComponent<RectTransform> ();
//				parentLayoutGroup.anchoredPosition = new Vector2 (parentLayoutGroup.anchoredPosition.x, -targetOffsetY);
//
//				print("entry.name :" + entry.name );
//				print("matchCount :" + matchCount );
//				Trace.Msg("targetOffsetY :" + targetOffsetY );
//			}
//		});
//
//	}

}

