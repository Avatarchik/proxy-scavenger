using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mindler.hacking 
{
	public class NumberStringFormatter : MonoBehaviour {

		private float trillion = 1000000000000;
		private float billion = 1000000000;
		private float million = 1000000;

		public string FormatNumber (float number, int decimals){
			float tempNum = 0;
			string textNum = "";
			double d;
			string numberText = "";
			bool format = false;

			if(number > trillion){
				tempNum = number / trillion;
				textNum = " Trillion";
				format = true;
			} else if(number > billion){
				tempNum = number / billion;
				textNum = " Billion";
				format = true;
			} else if(number > million){
				tempNum = number / million;
				textNum = " Million";
				format = true;
			} else {
				format = false;
			}

			if(format){
				d = System.Math.Round(tempNum, decimals);
				numberText = d + textNum;
			} else {
				numberText = number.ToString("N");
			}

			return numberText;
		}
	}
}