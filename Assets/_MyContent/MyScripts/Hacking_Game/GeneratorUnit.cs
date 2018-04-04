﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace mindler.hacking 
{
	public class GeneratorUnit : MonoBehaviour 
	{

		[BoxGroup("Name and Type")]
		public string name;
		[BoxGroup("Name and Type")]
		public GeneratorType type;

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
		[BoxGroup("UI Elements")]
		public Button lockedButton;
		[BoxGroup("UI Elements")]
		public Text lockText;

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
		[BoxGroup("Current Progress Stats")]
		public float currentProgressBonus = 1f;
		[BoxGroup("Current Progress Stats")]
		public float adjustedProgressTime = 1f;

		[BoxGroup("Current Revenue Stats")]
		public float currentRevenue = 100f;
		[BoxGroup("Current Revenue Stats")]
		public float currentRevenueMultiplier = 1.05f;
		[BoxGroup("Current Revenue Stats")]
		public float currentRevenueBonus = 1f;
		[BoxGroup("Current Revenue Stats")]
		public float adjustedRevenue = 100f;

		[BoxGroup("Current Generator Stats")]
		public float currentNewGeneratorCost = 1000f;
		[BoxGroup("Current Generator Stats")]
		public float currentGeneratorCount = 0f;
		[BoxGroup("Current Generator Stats")]
		public float currentGeneratorCostMultiplier = 1.1f;

		[BoxGroup("Temporary Stats")]
		public float tempTimeStart = 0f;
		[BoxGroup("Temporary Stats")]
		public float tempTimeEnd = 0f;
		[BoxGroup("Temporary Stats")]
		public float capturedTime = 0f;

		[BoxGroup("Locked or Unlocked")]
		public bool Locked = true;
		[BoxGroup("Locked or Unlocked")]
		public bool LockOnReset = true;

		[BoxGroup("Purchaseable")]
		public bool Purchaseable = false;



		public void init(){
			name = baseObject.objectName;
			type = baseObject.GeneratorName;

			icon.sprite = baseObject.Icon;

			if(!Locked){
				lockedButton.gameObject.SetActive(false);
			} else {
				lockedButton.gameObject.SetActive(true);
				lockText.text = "Unlock : " + baseObject.unlockCost.ToString("#,#");
			}

			ResetGraduatedValues();

			currentGeneratorCount = 1f;
			currentNewGeneratorCost = baseObject.initialNewGeneratorCost;

			//currentRevenueMultiplier = baseObject.initialRevenueMultiplier;
			currentRevenueMultiplier = baseObject.GraduatedRevenueMultipliers[0].y;
			currentRevenue = baseObject.initialRevenue;


			//currentProgressTimeMultiplier = baseObject.initialProgressTimeMultiplier;

			Debug.Log(name + " : " + currentRevenueMultiplier + " Current Revenue Multiplier");

			currentProgressTimeMultiplier = baseObject.GraduatedTimeMultipliers[0].y;
			currentProgressTime = baseObject.initialProgressTime;
			AdjustSpeed();
			Debug.Log(name + " : " + currentProgressTimeMultiplier + " Current Time Multiplier | " + adjustedProgressTime + " Adjusted Progress Time");


			//currentGeneratorCostMultiplier = baseObject.GraduatedCostMultipliers[0].y;

			CheckPurchaseable();

			UpdateUI();
		}

		public void AddGenerator(int increase)
		{
			currentGeneratorCount += (float)increase;


			//currentProgressTimeMultiplier =  baseObject.initialProgressTimeMultiplier * currentGeneratorCount; 
			//currentProgressTime = currentProgressTime * baseObject.initialProgressTimeMultiplier;


			//currentRevenueMultiplier = baseObject.newGeneratorRevenueMultiplier * currentGeneratorCount;
			//currentRevenue = baseObject.initialRevenue * currentRevenueMultiplier;

			Vector3[] v1 = baseObject.GraduatedTimeMultipliers;
			int y = 0;
			foreach(Vector3 t in v1){
				if(t.x == currentGeneratorCount && t.z != 1f){
					currentProgressTimeMultiplier = t.y;
					currentProgressTime = currentProgressTime * currentProgressTimeMultiplier;
					Vector3 vv = t;
					vv.z = 1f;
					baseObject.GraduatedTimeMultipliers[y] = vv;
				}
				y++;
			}
			AdjustSpeed();

			currentRevenue = currentRevenue * currentRevenueMultiplier;
			double d = System.Math.Round(currentRevenue, 2);
			currentRevenue = (float)d;
			AdjustRevenue();

			//currentNewGeneratorCost = baseObject.initialNewGeneratorCost * (baseObject.newGeneratorCostMultiplier * currentGeneratorCount);
			currentNewGeneratorCost = currentNewGeneratorCost * currentGeneratorCostMultiplier;
			double dd = System.Math.Round(currentNewGeneratorCost, 2);
			currentNewGeneratorCost = (float)dd;
			Debug.Log(name + " Current Generator Cost Multiplier : " + currentGeneratorCostMultiplier);

			CheckMultipliers();

			Debug.Log(name + " New Generator Cost Multiplier : " + currentGeneratorCostMultiplier);

			UpdateUI();
		}

		public void UpdateUI(){

			if(currentProgressTime <= 0.05f){
				progressBar.value = 1f;
			} else {
				progressBar.value = currentProgress;
			}
			//Debug.Log(progressBar.value + " progress bar's value");
			generatorCount.text = currentGeneratorCount.ToString();
			newGeneratorCost.text = currentNewGeneratorCost.ToString("N");
			generatingAmount.text = adjustedRevenue.ToString("N");
		}

		public void GeneratorUpdate()
		{
			
			float progress = (Time.time - capturedTime) / tempTimeEnd;

			if(progress >= 1f){
				capturedTime = Time.time;
				//tempTimeEnd = currentProgressTime;
				tempTimeEnd = adjustedProgressTime;
				currentProgress = 0f;
				HGM.AddCurrency(adjustedRevenue); //send generate notification
			} else if(progress < 1f) {
				currentProgress = progress;
			}
			if(currentProgress < 0){
				currentProgress = currentProgress * -1f;
			}
			CheckPurchaseable();
			UpdateUI();
		}

		public void AddSpeedBonus(float value){
			currentProgressBonus -= value;
			AdjustSpeed();
		}

		public void AdjustSpeed(){
			adjustedProgressTime = currentProgressTime * currentProgressBonus;
		}

		public void AddBonus(float value){
			currentRevenueBonus += value;
			AdjustRevenue();
		}

		public void AdjustRevenue(){
			adjustedRevenue = currentRevenue * currentRevenueBonus;
			generatingAmount.text = adjustedRevenue.ToString("N");
		}

		public void CheckMultipliers(){
			
			Vector3[] v1 = baseObject.GraduatedTimeMultipliers;
			Vector3[] v2 = baseObject.GraduatedCostMultipliers;
			Vector3[] v3 = baseObject.GraduatedRevenueMultipliers;

			int y = 0;

			foreach(Vector3 t in v1){
				if(t.x == currentGeneratorCount && t.z != 1f){
					currentProgressTimeMultiplier = t.y;
					Vector3 vv = t;
					vv.z = 1f;
					baseObject.GraduatedTimeMultipliers[y] = vv;
				}
				y++;
			}

			int yy = 0;
			foreach(Vector3 c in v2){
				if(c.x == currentGeneratorCount && c.z != 1f){
					
					currentGeneratorCostMultiplier = c.y;
					Vector3 vv = c;
					vv.z = 1f;
					baseObject.GraduatedCostMultipliers[yy] = vv;
				}
				yy++;
			}

			int yyy = 0;
			foreach(Vector3 r in v3){
				if(r.x == currentGeneratorCount && r.z != 1f){
					currentRevenueMultiplier = r.y;
					Vector3 vv = r;
					vv.z = 1f;
					baseObject.GraduatedRevenueMultipliers[yyy] = vv;
				}
				yyy++;
			}

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
			progressBar.value = 0f;
			currentGeneratorCount = 0f;
			tempTimeEnd = 0f;
			Locked = baseObject.Lock;
			ResetGraduatedValues();
			currentRevenue = baseObject.initialRevenue;
			adjustedRevenue = currentRevenue;

			currentRevenueBonus = 1f;
			currentProgressBonus = 1f;

			init();
		}

		public void ResetGraduatedValues(){
			Vector3[] v1 = baseObject.GraduatedTimeMultipliers;
			Vector3[] v2 = baseObject.GraduatedCostMultipliers;
			Vector3[] v3 = baseObject.GraduatedRevenueMultipliers;

			int ii = 0;
			foreach(Vector3 i in v1){
				Vector3 v = baseObject.GraduatedTimeMultipliers[ii];
				v.z = 0f;
				baseObject.GraduatedTimeMultipliers[ii] = v;
				ii++;
			}

			int jj = 0;
			foreach(Vector3 j in v2){
				Vector3 v = j;
				v.z = 0f;
				baseObject.GraduatedCostMultipliers[jj] = v;
				jj++;
			}

			int kk = 0;
			foreach(Vector3 k in v3){
				Vector3 v = k;
				v.z = 0f;
				baseObject.GraduatedRevenueMultipliers[kk] = v;
				kk++;
			}
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
					lockedButton.interactable = true;
				} else {
					Purchaseable = false;
					lockedButton.interactable = false;
				}
			}

			if(!Locked){
				if(HGM.GetCurrentCurrency() >= currentNewGeneratorCost){
					Purchaseable = true;
					purchaseButton.interactable = true;
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
			if(Purchaseable){
				HGM.RemoveCurrency(baseObject.unlockCost);
				Locked = false;
				init();
			}
		}
	}
}