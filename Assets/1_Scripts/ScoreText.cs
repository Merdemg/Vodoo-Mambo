using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
	public float Duration;
	public float OffsetY;


    [SerializeField]private Text _text;
    [SerializeField]private Image _sparks;

	public int offsetX = 0;

	void Awake()
	{
        _text = GetComponentInChildren<Text>();
        _sparks = GetComponentInChildren<Image>();
	}

    public void Initialize(Vector2 position, int score, bool isCombo)
	{
//		Vector3 targetPositionOnScreen = Camera.main.WorldToScreenPoint(position);
//		Vector3 targetPositionOnScreen = RectTransformUtility.WorldToScreenPoint (Camera.main, position);
		Vector3 targetPositionOnScreen = position;

        string text = isCombo ? ("x" + score) : score.ToString();
        _text.text = text;

		CanvasGroup canvasGroup = GetComponent<CanvasGroup> ();
		RectTransform rectT = GetComponent<RectTransform> ();

		rectT.anchoredPosition = targetPositionOnScreen;

//		Trace.Msg ("Mid Point of Balls = " + targetPositionOnScreen);

        // Score
        if(!isCombo)
        {
//            Vector3 finalPos = UIManager.Instance.ScoreText.transform.position;
            LeanTween.move(gameObject, targetPositionOnScreen + new Vector3(0f, OffsetY, 0f), Duration);
//            LeanTween.move(_text.gameObject, finalPos, Duration).setDelay(Duration/2);
            LeanTween.scale(gameObject, Vector3.one * 0.4f, Duration);
            // TODO: FIX THIS
            canvasGroup.alpha = 1;
            LeanTween.alphaCanvas(canvasGroup, 0, Duration/2).setDelay(Duration/2).setOnComplete(() =>
                {
                    Destroy(gameObject);
                });
        }
        // Combo
        else
        {
            LeanTween.move(gameObject, targetPositionOnScreen + new Vector3(0f, OffsetY, 0f), Duration);
            LeanTween.scale(gameObject, Vector3.one * 1.5f, Duration);

            canvasGroup.alpha = 1;
            LeanTween.alphaCanvas(canvasGroup, 0, Duration).setDelay(.5f).setOnComplete(() =>
                {
//                    FastPoolManager.GetPool()
//                    Destroy(gameObject);
                    SpawnManager.Instance.DeSpawnScoreText(gameObject);
                });
        }


//		LeanTween.value(gameObject, (value) =>
//			{
//				canvasGroup.alpha = value;
//
//			}, 1f, 0f, Duration).setOnComplete(() =>
//				{
//					Destroy(gameObject);
//				});
	}
}
