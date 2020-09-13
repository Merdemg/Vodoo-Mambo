using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICoinBox : MonoBehaviour {

	[SerializeField]int amount;
	public int Amount{
		get{
			return amount;
		}
		set{
			amount = value;
			amountText.text = value.ToString ();
		}
	}
	[SerializeField]Text amountText;



}
