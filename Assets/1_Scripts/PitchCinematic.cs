using UnityEngine;
using System;
using System.Collections;

// 
// Feature: Removing Odin/Sirenix to be able to update libs and Unity
// 2019.12.28 - LEVON
// 
// using Sirenix.OdinInspector;
// 
// End of Feature
// 

public class PitchCinematic : MonoBehaviour
{
    #region Singleton
    public static PitchCinematic Instance { private set; get; }

    void Awake()
    {
        Instance = this;
    }

    #endregion

    [SerializeField] float _cinematicDuration = 10f;
    [SerializeField] GameObject _cinematicResource;
    [SerializeField] GameObject _cinematicInstance;
    [SerializeField] bool isShowing = false;

    private const string IsCinematicShownPrefKey = "PitchCinematicShown";

    public static bool IsCinematicShown {
        get {
            return PlayerPrefs.GetInt(IsCinematicShownPrefKey, 0) == 1;
        }
        set {
            PlayerPrefs.SetInt(IsCinematicShownPrefKey, value ? 1 : 0);
        }
    }
    private void LoadCinematicResources()
    {

		string prefabName = "Cinematic";

		var prefab = Resources.Load( prefabName, typeof( GameObject ) ) as GameObject;

        _cinematicInstance = Instantiate(prefab);

        _cinematicInstance.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(Skip);

    }

    Action finishCallback;
    public void ShowCinematic(Action callback = null)
    {
        finishCallback = callback;
        IsCinematicShown = true;
        LoadCinematicResources();
        //_cinematicInstance.transform.SetParent(UIManager.Instance.mainCanvas.transform, false);


        Utility.Instance.DoAfter(_cinematicDuration, ()=>{
            DestroyCinematic();
        });
    }

    private void DestroyCinematic() {
        var lt = LeanTween.alphaCanvas(_cinematicInstance.GetComponent<CanvasGroup>(), 0, GPM.Instance.panelTransitionTime).setEase(GPM.Instance.panelTransitionType).setOnComplete(()=>{
			if(finishCallback != null) {
                finishCallback();
                finishCallback = null;
				//lt.setOnComplete(callback);
			}
			Destroy(_cinematicInstance);
			Resources.UnloadUnusedAssets();
        });
    }
    public void Skip() {
        DestroyCinematic();
    }

// 
// Feature: Removing Odin/Sirenix to be able to update libs and Unity
// 2019.12.28 - LEVON
// 
// #if UNITY_EDITOR
//     [Button]
//     private void ResetPrefs() {
//         PlayerPrefs.DeleteKey(IsCinematicShownPrefKey);
//     }
// #endif
// 
// End of Feature
// 
}
