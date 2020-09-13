#define USE_LOGS
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IngameShaman : MonoBehaviour {

	public float minShamanShowDelay = 3f;

	public RectTransform shaman;
	public RectTransform textBubble;
	public Text text;

	public RectTransform visibleShaman;

	public float shamanShowDuration = 1f;
	public float bubbleShowDuration = 1f;
	public float delay = 1f;
	public LeanTweenType shamanShowEasing;
	public LeanTweenType bubbleShowEasing;

	private float lastShamanShowTime = 0;


	public void Show(string shamanText = "")
	{
        Trace.Msg (("Showing Shaman").Colored (Colors.red));
        if(Time.time - lastShamanShowTime < minShamanShowDelay)
        {
            return;
        }

        AudioManager2.Instance.Play (SoundsManager.Instance.shamanYell);

		lastShamanShowTime = Time.time;

		shaman.gameObject.SetActive (true);

		if ( shamanText.Length < 1 ) {
			shamanText = Lean.Localization.LeanLocalization.GetTranslationText( "Shaman--Yeah" ); }

		text.text = shamanText;

		// Cache and initilize sizes
		Vector3 cachedShamanPos = shaman.localPosition;
		Vector3 cachedBubbleSize = textBubble.localScale;
		textBubble.localScale = Vector3.zero;
		textBubble.gameObject.SetActive(true);

		// Shaman In
		LeanTween.moveLocal (shaman.gameObject, visibleShaman.localPosition, shamanShowDuration).setEase (shamanShowEasing);
		// Text Scale In
		LeanTween.scale (textBubble, cachedBubbleSize, bubbleShowDuration).setDelay(shamanShowDuration ).setEase(bubbleShowEasing);
		// Text Out
		LeanTween.scale (textBubble, Vector3.zero, bubbleShowDuration).setDelay(shamanShowDuration + bubbleShowDuration + delay).setEase(bubbleShowEasing);
		// Shaman Out
		LeanTween.moveLocal (shaman.gameObject, cachedShamanPos, shamanShowDuration).setDelay(shamanShowDuration + bubbleShowDuration + delay).setEase (shamanShowEasing).setOnComplete(()=> {

			// Clean up
			textBubble.localScale = cachedBubbleSize;
			shaman.localPosition = cachedShamanPos;

			shaman.gameObject.SetActive (false);
			textBubble.gameObject.SetActive (false);

		});
	}

}
