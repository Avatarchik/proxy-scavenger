using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using mindler.dungeonship;
using Sirenix.OdinInspector;

namespace mindler.hacking 
{
	public class UnlockUnit : MonoBehaviour {

		[BoxGroup("Name and Type")]
		public string Name;
		[BoxGroup("Name and Type")]
		public HackingUnlock Type;

		[BoxGroup("Price")]
		public float Cost;

		[BoxGroup("UI Elements")]
		public Button UnlockButton;
		[BoxGroup("UI Elements")]
		public Text LockedNameText;
		[BoxGroup("UI Elements")]
		public Text LockedAmountText;
		[BoxGroup("UI Elements")]
		public Text UnlockNameText;

		[BoxGroup("Hacking Game Manager")]
		public HackingGameManager HGM;

		[BoxGroup("Base Unlock Object")]
		public UnlockBase baseObject;

		[BoxGroup("Locked or Unlocked")]
		public bool Locked = true;
		[BoxGroup("Locked or Unlocked")]
		public bool LockOnReset = true;

		[BoxGroup("Purchaseable")]
		public bool Purchaseable = false;


		public void init(){
			Name = baseObject.objectName;
			Type = baseObject.HackingUnlockName;

			//UnlockType = baseObject.UnlockType;

			Cost = baseObject.Cost;

			UpdateUI();

			CheckButtonLockStatus();
		}

		public void CheckButtonLockStatus(){
			if(!Locked){
				UnlockButton.gameObject.SetActive(false);
			} else {
				UnlockButton.gameObject.SetActive(true);
				UpdateUI();
				CheckPurchaseable();
			}
		}

		public void PurchaseButton(){
			if(Purchaseable){
				HGM.RemoveCurrency(Cost);
				Locked = false;
				CheckButtonLockStatus();
				ActivateUnlockAction();
				UpdateUI();
			} else {
				Debug.Log("NOT ENOUGH MONEY");
			}
		}

		public void ActivateUnlockAction(){
			switch(Type){
				case HackingUnlock.UnlockAllTerminals:
					HGM.ActivateTerminals(true, baseObject.ActivateTerminal);
				break;
				case HackingUnlock.UnlockSpecificTerminal:
					HGM.ActivateTerminals(false, baseObject.ActivateTerminal);
				break;
				case HackingUnlock.UnlockMap:
					//unlock map
				break;
				case HackingUnlock.UnlockBrokenUnitsLocation:
					//unlock broken part location
				break;
				case HackingUnlock.UnlockDoors:
					//unlock doors
				break;
				case HackingUnlock.UnlockImproveShipData:
					//improve ship location unit data
				break;
			}
		}

		public void CheckPurchaseable(){
			if(Locked){
				if(HGM.GetCurrentCurrency() >= Cost){
					Purchaseable = true;
					UnlockButton.interactable = true;
				} else {
					Purchaseable = false;
					UnlockButton.interactable = false;
				}
			}

			if(!Locked){
				if(HGM.GetCurrentCurrency() >= Cost){
					Purchaseable = true;
					UnlockButton.interactable = true;
				} else {
					Purchaseable = false;
					UnlockButton.interactable = false;
				}
			}
		}

		public void UpdateUI(){
			
			LockedNameText.text = baseObject.objectName;
			LockedAmountText.text = Cost.ToString("#,#");
			UnlockNameText.text = baseObject.objectName;

			//LockedImage.sprite = baseObject.Icon;
			//UnlockImage.sprite = baseObject.Icon;
		}

		public void Reset()
		{
			Locked = true;
			init();
		}
	}
}
