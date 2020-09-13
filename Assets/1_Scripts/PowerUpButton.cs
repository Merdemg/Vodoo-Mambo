using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerUpButton : MonoBehaviour {

	public PowerUp powerUp;
    [SerializeField]private Image iconImage;
    [SerializeField]private Text costText;
    [SerializeField]private GameObject selectionIndicator;


	private Button _button;

	void Awake()
	{
		_button = GetComponent<Button> ();
        costText.text = powerUp.cost.ToString();
//		iconImage = GetComponentInChildren<Image>();
	}

	void Start()
	{
		_button.onClick.AddListener (() => {
			OnClick();
		});
	}

	void OnClick()
	{
//		bool isActive = powerUp.Toggle ();
//		Toggle (isActive);
        powerUp.Toggle();
	}

    public void Select(bool isSelect = true)
    {
        selectionIndicator.SetActive(isSelect);
    }

//	public void SetAppeareance(bool isActive)
//	{
//		// TODO: Transition animation
//		if(isActive)
//		{
//			iconImage.sprite = powerUp.selectedSprite;
//		}
//		else
//		{
//			iconImage.sprite = powerUp.sprite;
//		}
//	}
}
