
#if UNITY_IOS
#define USE_LOGS
using UnityEngine;
using UnityEngine.iOS;
using System.Collections;

public class NotificationManager : MonoBehaviour
{
#region Singleton
    public static NotificationManager Instance { private set; get; }

    void Awake()
    {
        Instance = this;
    }

#endregion

    bool tokenSent = false;

    void Start() {
        ResetBadge();
		UnityEngine.iOS.NotificationServices.RegisterForNotifications (NotificationType.Alert | NotificationType.Badge | NotificationType.Sound, true);
//		UnityEngine.iOS.NotificationServices.
		GetToken ();
    }

	void GetToken() {
		DebugConsole.Log ("NotificationManager > GetToken");
		ISN_RemoteNotificationsController.Instance.RegisterForRemoteNotifications ((ISN_RemoteNotificationsRegistrationResult res) => {

			DebugConsole.Log ("ISN_RemoteNotificationsRegistrationResult: " + res.IsSucceeded, "error");
			if(!res.IsSucceeded) {
				
				DebugConsole.Log (res.Error.Code + " / " + res.Error.Message, "error");

			} else {
				DebugConsole.Log (res.Token.DeviceId, "error");
				OnTokenGot(res.Token.DeviceId);
			}


		});
	}



    void Update () {
//        #if !UNITY_EDITOR
        if (!tokenSent) { // tokenSent needs to be defined somewhere (bool tokenSent = false)
            byte[] token  = UnityEngine.iOS.NotificationServices.deviceToken;
			DebugConsole.Log("Checking for token");
            if(token != null) {
                string tokenString =  System.BitConverter.ToString(token).Replace("-", "").ToLower();
				DebugConsole.Log ("OnTokenReived");
				DebugConsole.Log (tokenString);
                OnTokenGot(tokenString);
                tokenSent = true;
            }
        }
//        #endif

    }

    void ResetBadge() {
        UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
        UnityEngine.iOS.NotificationServices.ClearRemoteNotifications();
    }

    void OnTokenGot(string token) {
        DebugConsole.Log("Got device token from device! Token: " + token);
        GameSparksManager.Instance.RegisterForPushNotifications(token);
    }

}

#endif