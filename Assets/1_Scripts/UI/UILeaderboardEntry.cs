using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UILeaderboardEntry : MonoBehaviour
{
	public Image ProfileImage;
	public Text RankText;
	public Text NameText;
    public Text ScoreText;
    public Image background;

    public Sprite normalBg;
    public Sprite mineBg;
    public Sprite firstBg;
    public Sprite firstAndMineBg;

	private string _playerId;
	public  string PlayerId {
		get {
			return _playerId;
		}
	}
	private string _facebookId;
	private string _name;
	private string _rank;
	private string _score;

	public void Initialize(string playerId, string facebookId, int rank, string name, int score)
	{
		gameObject.name = name + " - LB Entry";
		
		_playerId = playerId;
		_facebookId = facebookId;
		RankText.text = _rank = rank.ToString();
		NameText.text = _name = name;
		ScoreText.text = _score = score.ToString();

        if(background != null) {

            if (GameSparksManager.Instance != null && GameSparksManager.Instance.player != null && _playerId == GameSparksManager.Instance.player.userId) {
                if(rank == 1) 
                    background.sprite = firstAndMineBg;
                else 
					background.sprite = mineBg;
            }
            else if (rank == 1) {
                background.sprite = firstBg;
            }
            else {
                background.sprite = normalBg;
            }
        }

		//TODO move to an always active gameobject
		//        StartCoroutine(getFBPicture());
//		InitFacebookPicture ();
	}

	//this is the function that will pull the profile picture from 
	//facebook
//	public void InitFacebookPicture()
//	{
//
//		FacebookManager.Instance.StartGetFbPicture (_facebookId, (sprite) => {
//			_picture = ProfileImage.sprite = sprite;
//		});
//		//        var www = new WWW("http://graph.facebook.com/" + _facebookId + "/picture?type=square");
//		//
//		//        yield return www;
//		//
//		//        Texture2D tempPic = new Texture2D(100, 100);
//		//        www.LoadImageIntoTexture(tempPic);
//		//
//		//		_picture = ProfileImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
//
//	}
//
	public void OnClick()
	{
		Trace.Msg ("fbId : " + _facebookId);

		//		GameSparksManager.Instance.GetAchivementsById (_playerId);
		//		GameSparksManager.Instance.GetLeaderBoardEntries (_playerId);

		UIManager.Instance.profilePanel.LoadProfile (_playerId, _name);
		UIManager.Instance.profilePanel.Open ();
        UIManager.Instance.PlayButtonSFX();
//		UI_PlayerProfile.i.LoadProfile (_playerId, _name, _picture);
	}

	public void MarkAsLocalPlayer() {
		NameText.color = GPM.Instance.leaderboardLocalPlayerNameColor;
	}

}
