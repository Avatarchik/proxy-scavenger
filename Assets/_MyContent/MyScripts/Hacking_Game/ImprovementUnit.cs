using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace mindler.hacking 
{
	public class ImprovementUnit : MonoBehaviour 
	{
		[BoxGroup("Name and Type")]
		public string Name;
		[BoxGroup("Name and Type")]
		public GeneratorType ImproveUnit;

		[BoxGroup("Improve Units")]
		public bool ImproveAllUnits = false;
		[BoxGroup("Improve Units")]
		public float ImprovementMultiplier = 2f;

		[BoxGroup("Cost")]
		public float Cost = 5000f;

		[BoxGroup("UI Elements")]
		public Button UnlockButton;
		[BoxGroup("UI Elements")]
		public Text LockedNameText;
		[BoxGroup("UI Elements")]
		public Text LockedMultiplierText;
		[BoxGroup("UI Elements")]
		public Text LockedAmountText;
		[BoxGroup("UI Elements")]
		public Text UnlockNameText;
		[BoxGroup("UI Elements")]
		public Text UnlockMultiplierText;
		[BoxGroup("UI Elements")]
		public Image LockedImage;
		[BoxGroup("UI Elements")]
		public Image UnlockImage;

		[BoxGroup("Hacking Game Manager")]
		public HackingGameManager HGM;

		[BoxGroup("Base Unlock Object")]
		public ImprovementBase baseObject;

		[BoxGroup("Locked or Unlocked")]
		public bool Locked = true;

		[BoxGroup("Purchaseable")]
		public bool Purchaseable = false;

		[BoxGroup("Purchased")]
		public bool Purchased = false;


		public void init(){
			Name = baseObject.Name;
			ImproveUnit = baseObject.ImproveUnit;
			ImproveAllUnits = baseObject.ImproveAllUnits;

			Cost = baseObject.Cost;

			UpdateUI();

			CheckButtonLockStatus();
		}

		public void CheckButtonLockStatus(){
			if(!Purchased){
				if(!Locked){
					UnlockButton.gameObject.SetActive(false);
				} else {
					UnlockButton.gameObject.SetActive(true);
					UpdateUI();
					CheckPurchaseable();
				}
			} else {
				UnlockButton.gameObject.SetActive(false);
			}
			UpdateUI();
		}

		public void PurchaseButton(){
			if(Purchaseable){
				HGM.RemoveCurrency(Cost);
				Locked = false;
				ApplyBonus();
				Purchased = true;
				CheckButtonLockStatus();
			} else {
				Debug.Log("NOT ENOUGH MONEY");
			}
		}

		public void ApplyBonus(){
			if(ImproveAllUnits){
				HGM.ApplyAllBonus(ImprovementMultiplier);
			} else {
				HGM.ApplySpecificBonus(ImprovementMultiplier, ImproveUnit);
			}
		}

		public void CheckPurchaseable(){
			if(!Purchased){
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
			} else {
				UnlockButton.interactable = false;
			}
		}

		public void UpdateUI(){
			string improvementText = "+ " + baseObject.ImprovementMultiplier + "x";

			LockedNameText.text = baseObject.Name;
			LockedMultiplierText.text = improvementText;
			LockedAmountText.text = Cost.ToString("#,#");
			UnlockNameText.text = baseObject.Name;
			UnlockMultiplierText.text = improvementText;

			LockedImage.sprite = baseObject.Icon;
			UnlockImage.sprite = baseObject.Icon;
		}

		public void Reset(){
			Purchased = false;

			Locked = true;

			init();
		}
	}
}
