using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace mindler.hacking 
{
	public class GeneratorUnit : MonoBehaviour 
	{
		[BoxGroup("UI Elements")]
		public Slider progressBar;
		[BoxGroup("UI Elements")]
		public Image icon;
		[BoxGroup("UI Elements")]
		public Text generatorCount;
		[BoxGroup("UI Elements")]
		public Text newGeneratorCost;
		[BoxGroup("UI Elements")]
		public Text generatingAmount;
		[BoxGroup("UI Elements")]
		public Button purchaseButton;

		[BoxGroup("Hacking Game Manager")]
		public HackingGameManager HGM;

		/*
		public float initialRevenue = 100f;
		public float initialNewGeneratorCost = 1000f;
		public float initialProgressTime = 2f; //seconds
		public float initialProgressTimeMultiplier = -0.2f;
		public float newGeneratorRevenueMultiplier = 1.05f; 
		public float newGeneratorCostMultiplier = 1.13f;
		public float newGeneratorTimeMultiplier = -0.2f; //reduce how much time it takes to generate
		*/

		[BoxGroup("Base Generator Object")]
		public GeneratorBase baseObject;

		[BoxGroup("Current Progress Stats")]
		public float currentProgress = 0f;
		[BoxGroup("Current Progress Stats")]
		public float currentProgressTime = 2f;
		[BoxGroup("Current Progress Stats")]
		public float currentProgressTimeMultiplier = -0.2f;

		[BoxGroup("Current Revenue Stats")]
		public float currentRevenue = 100f;
		[BoxGroup("Current Revenue Stats")]
		public float currentRevenueMultiplier = 1.05f;

		[BoxGroup("Current Generator Stats")]
		public float currentNewGeneratorCost = 1000f;
		[BoxGroup("Current Generator Stats")]
		public float currentGeneratorCount = 0f;

		[BoxGroup("Temporary Stats")]
		public float tempTimeStart = 0f;
		[BoxGroup("Temporary Stats")]
		public float tempTimeEnd = 0f;
		[BoxGroup("Temporary Stats")]
		public float capturedTime = 0f;

		[BoxGroup("Locked or Unlocked")]
		public bool Locked = true;

		[BoxGroup("Purchaseable")]
		public bool Purchaseable = false;


		public void init(){
			icon.sprite = baseObject.Icon;
			if(!Locked){

				currentGeneratorCount = 1f;
				currentNewGeneratorCost = baseObject.initialNewGeneratorCost;
				currentRevenueMultiplier = baseObject.newGeneratorRevenueMultiplier;
				currentRevenue = baseObject.initialRevenue;
				currentProgressTimeMultiplier = baseObject.initialProgressTimeMultiplier;
				currentProgressTime = baseObject.initialProgressTime;

			} else {
				
				// remove locked overlay
			}

			UpdateUI();
		}

		public void AddGenerator(int increase)
		{
			currentGeneratorCount += (float)increase;

			//currentProgressTimeMultiplier =  baseObject.initialProgressTimeMultiplier * currentGeneratorCount; 
			currentProgressTime = currentProgressTime * baseObject.initialProgressTimeMultiplier;

			currentRevenueMultiplier = baseObject.newGeneratorRevenueMultiplier * currentGeneratorCount;
			currentRevenue = baseObject.initialRevenue * currentRevenueMultiplier;

			currentNewGeneratorCost = baseObject.initialNewGeneratorCost * (baseObject.newGeneratorCostMultiplier * currentGeneratorCount);
			UpdateUI();
		}

		public void UpdateUI(){
			progressBar.value = currentProgress;
			Debug.Log(progressBar.value + " progress bar's value");
			generatorCount.text = currentGeneratorCount.ToString();
			newGeneratorCost.text = currentNewGeneratorCost.ToString();
			generatingAmount.text = currentRevenue.ToString();
		}

		public void GeneratorUpdate()
		{
			
			/*
			if(tempTimeEnd == 0f){
				tempTimeEnd = t + currentProgressTime;
			}
			*/

			float progress = (Time.time - capturedTime) / tempTimeEnd;

			Debug.Log(Time.time + " Time.time / " + tempTimeEnd + " tempTimeEnd = " + progress + " currentProgress");

			if(progress >= 1f){
				Debug.Log(progress + " Progress is GREATER than 1f");
				capturedTime = Time.time;
				tempTimeEnd = currentProgressTime;
				currentProgress = 0f;
				HGM.AddCurrency(currentRevenue); //send generate notification
			} else if(progress < 1f) {
				Debug.Log(progress + " Progress is LESS than 1f");
				currentProgress = progress;
			} else {
				Debug.Log("What the fuckkk");
			}
			if(currentProgress < 0){
				currentProgress = currentProgress * -1f;
			}
			UpdateUI();
			Debug.Log("After WEIRD calculations : " + capturedTime + " capturedTime | "+ Time.time + " Time.time / " + tempTimeEnd + " tempTimeEnd = " + currentProgress + " currentProgress");
			//Debug.Log("After calculations : " + Time.time + " Time.time / " + tempTimeEnd + " tempTimeEnd = " + currentProgress + " currentProgress");
			//Debug.Log(currentProgress + " Progress bar progress");

			CheckPurchaseable();


		}

		public float GetGeneratorCost()
		{
			return currentNewGeneratorCost;
		}

		public float GenerateRevenue()
		{
			return currentRevenue;
		}

		public void PurchaseButton(){
			if(Purchaseable){
				HGM.RemoveCurrency(currentNewGeneratorCost);
				AddGenerator(1);
			} else {
				Debug.Log("NOT ENOUGH MONEY");
			}
		}

		public void Reset()
		{
			currentProgress = 0f;
			currentProgressTime = baseObject.initialProgressTime;
			currentProgressTimeMultiplier = baseObject.initialProgressTimeMultiplier;

			progressBar.value = 0f;

			currentRevenue = baseObject.initialRevenue;
			currentRevenueMultiplier = baseObject.newGeneratorRevenueMultiplier;
			currentNewGeneratorCost = baseObject.initialNewGeneratorCost;
			currentGeneratorCount = 0f;

			tempTimeEnd = 0f;
		}

		public bool IsThisLocked(){
			return Locked;
		}

		public void SetLock(bool l){
			Locked = l;
		}

		public void CheckPurchaseable(){
			if(Locked){
				if(HGM.GetCurrentCurrency() >= baseObject.unlockCost){
					Purchaseable = true;
					purchaseButton.interactable = true;
				} else {
					Purchaseable = false;
					purchaseButton.interactable = false;
				}
			} else {
				if(HGM.GetCurrentCurrency() >= currentNewGeneratorCost){
					Purchaseable = true;
				} else {
					Purchaseable = false;
					purchaseButton.interactable = false;
				}
			}
		}

		public bool IsThisPurchaseable()
		{
			CheckPurchaseable();

			return Purchaseable;
		}

		public GeneratorType GetType()
		{
			return baseObject.GeneratorName;
		}

		public void Unlock(){
			Locked = false;
			init();
		}
	}
}