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

		[BoxGroup("Text Formatting")]
		public NumberStringFormatter TextFormatter;

		public void Start(){
			if(TextFormatter == null){
				TextFormatter = GameObject.FindGameObjectWithTag("GameManager").GetComponentInChildren<NumberStringFormatter>();
			}
		}

		public void UpdateCurrentCurrency(float value){
			if(TextFormatter == null){
				TextFormatter = GameObject.FindGameObjectWithTag("GameManager").GetComponentInChildren<NumberStringFormatter>();
			}
			string t = TextFormatter.FormatNumber(value, 2);
			CurrentCurrencyText.text = t;
			//CurrentCurrencyText.text = value.ToString("N");
		}
	}
}
