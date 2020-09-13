using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SA.IOSNative.StoreKit;

public class UIPanel_Shop : UIPanel {

	[Header("Parameters")]
	[SerializeField]int[] coinValues;
	[Space]
	[SerializeField]Button[] coinButtons;
	[Space]
    [SerializeField]Text[] coinValueTexts;
    [SerializeField]Text[] coinPricesTexts;

	protected override void Start()
	{
		base.Start ();

		for (int i = 0; i < coinValues.Length; i++) 
		{
//			int val = coinValues [i];
			int productIndex = i;
			coinButtons[i].onClick.AddListener (() => {
				UIManager.Instance.SetShowLoading (true);

				PaymentManagerPitch.BuyItem(PaymentManagerPitch.CoinIdentifiers[productIndex], (success)=>{
					UIManager.Instance.SetShowLoading (false);

					if(success) {
						MetaManager.Instance.UpdateMeta(MetaManager.MetaType.coins, coinValues[productIndex]);
					}
					else {

						string messageTitle = Lean.Localization.LeanLocalization.GetTranslationText( "Dialog--Purchase-Fail-Title" );
						string messageText = Lean.Localization.LeanLocalization.GetTranslationText( "Dialog--Purchase-Fail-Text" );
						IOSNativePopUpManager.showMessage( messageTitle, messageText );
					}
				});
			});

			coinValueTexts[i].text = coinValues[i].ToString();
		}
	}

    public void SetItemCosts(Product[] products) {
        for (int i = 0; i < products.Length; i++) {
            var price = products[i].LocalizedPrice;
            //var currency = products[i].CurrencySymbol;
            coinPricesTexts[i].text = price;
        }
    }
}
