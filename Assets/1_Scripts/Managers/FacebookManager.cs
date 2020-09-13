using UnityEngine;
using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;

public class FacebookManager : MonoBehaviour
{
    public static FacebookManager Instance { private set; get; }

//    private string _facebookDisplayName = "";

    public delegate void FacebookLoginEventDelegate(bool loginStatus);
    public FacebookLoginEventDelegate LoginEvent;
//    public FacebookLoginEventDelegate LoginSuccess;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
//        UIManager.Instance.UpdateDebugText("Facebook Initialize called.");
		DebugConsole.Log("Facebook Manager Initialize called.");

		//TODO
		//DOING
//		FB.Init(null);
		FB.Init(this.OnInitComplete, this.OnHideUnity);
//        StartCoroutine(TempFBInitPollCoroutine());
    }

    private void OnInitComplete()
    {
//        UIManager.Instance.UpdateDebugText("Facebook OnInitComplete called");
		DebugConsole.Log("Facebook OnInitComplete called");

        if (FB.IsLoggedIn)
        {
			GameSparksManager.Instance.SendFacebookAuthenticationRequest (AccessToken.CurrentAccessToken.TokenString);
//			QueryProfileInfo();
        }
		else
		{
			DebugConsole.Log ("Facebook is not logged in!", "warning");

            InvokeLoginEvents(false);

			UIManager.Instance.OnLoginFailed ();
		}
    }

    public void InvokeLoginEvents(bool isLoggedIn)
    {
        if(LoginEvent != null)
        {
            LoginEvent(isLoggedIn);
        }
    }

    public void Login()
    {
//        UIManager.Instance.UpdateDebugText("Facebook Login called.");
		DebugConsole.Log("Facebook Login called.");

		//TODO
		//DOING
		FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, OnLoggedIn);
//        FB.LogInWithReadPermissions("public_profile,email,user_friends", null);
//        StartCoroutine(TempFBLoginPollCoroutine());
    }

	private void OnLoggedIn(IResult result)
    {
		if(result.Cancelled)
		{

            InvokeLoginEvents(false);
			UIManager.Instance.OnLoginFailed ();
			DebugConsole.Log("Facebook login cancelled.");

		}
		else if(result.Error == null)
		{
			
//			UIManager.Instance.UpdateDebugText("Facebook OnLoggedIn received.");
			DebugConsole.Log("Facebook OnLoggedIn received.");
//			UIManager.Instance.UpdateDebugText(AccessToken.CurrentAccessToken.TokenString);
			DebugConsole.Log(AccessToken.CurrentAccessToken.TokenString);

			GameSparksManager.Instance.SendFacebookAuthenticationRequest (AccessToken.CurrentAccessToken.TokenString);

//            InvokeLoginEvents(true);

		}
		else
		{
            InvokeLoginEvents(false);

			UIManager.Instance.OnLoginFailed ();
			Debug.LogWarning (result.Error.ToString ());
		}
    }

//	private void QueryProfileInfo ()
//	{
//		QueryName ();
//		QueryPicture ();
//	}
//
//    private void QueryName()
//    {
//        FB.API("me?fields=first_name", HttpMethod.GET, (IGraphResult result) =>
//        {
//				if (result.ResultDictionary.ContainsKey("first_name"))
//	            {
//					string name = result.ResultDictionary["first_name"].ToString();
//
////					_facebookDisplayName = name;
//
////					GameSparksManager.Instance.SendDeviceAuthenticationRequestWithFacebookDisplayName(_facebookDisplayName);
//
//					Debug.Log("Facebook Name is " + name);
//	//                foreach (string key in result.ResultDictionary.Keys)
//	//                {
//	//                    if (key == "first_name")
//	//                    {
//	//                        _facebookDisplayName = result.ResultDictionary[key].ToString();
//	//
//	//                        GameSparksManager.Instance.SendDeviceAuthenticationRequestWithFacebookDisplayName(_facebookDisplayName);
//	//                    }
//	//                }
//	            }
//        });
//    }
//
//	private void QueryPicture ()
//	{
//		FB.API ("/me/picture?type=square&height=128&width=128", HttpMethod.GET, GetPictureCallback);
////		if(reul)
//	}

	void GetPictureCallback (IGraphResult result)
	{
		if(result.Texture != null)
		{
//			UnityEngine.UI.Image = 
			Sprite pictureSprite = Sprite.Create (result.Texture, new Rect (0, 0, 128, 128), new Vector2 ());

//			UIManager.Instance.DebugImage.sprite = pictureSprite;
		}
	}

	private void OnHideUnity(bool hidden)
	{
		
	}

	public void StartGetFbPicture(string _facebookId, Action<Sprite> callback)
	{
		StartCoroutine (getFBPicture (_facebookId, callback));
	}

	public IEnumerator getFBPicture(string _facebookId, Action<Sprite> callback)
	{
		
		var www = new WWW("http://graph.facebook.com/" + _facebookId + "/picture?type=square");

		yield return www;

		Texture2D tempPic = new Texture2D(100, 100);
		www.LoadImageIntoTexture(tempPic);

		Sprite _picture = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));

		callback (_picture);

		//		_sprite = 
	}

//	public string get_data; public string fbname;

} 
