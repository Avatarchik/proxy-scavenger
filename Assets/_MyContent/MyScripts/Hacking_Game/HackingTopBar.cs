using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace mindler.hacking 
{
	public class HackingTopBar : MonoBehaviour 
	{

		[BoxGroup("UI Elements")]
		public Text CurrentCurrencyText;

		public void UpdateCurrentCurrency(float value){
			CurrentCurrencyText.text = value.ToString("N");
		}
	}
}
