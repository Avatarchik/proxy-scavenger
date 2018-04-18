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
		public bool ProductionImprovement = false;
		[BoxGroup("Improve Units")]
		public float ProductionImprovementMultiplier = 2f;
		[BoxGroup("Improve Units")]
		public bool SpeedImprovement = false;
		[BoxGroup("Improve Units")]
		public float SpeedImprovementMultiplier = 1f;

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

		[BoxGroup("Number Formatter")]
		public NumberStringFormatter NumberFormatter;

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

			ProductionImprovement = baseObject.ProductionImprovement;
			ProductionImprovementMultiplier = baseObject.ProductionImprovementMultiplier;

			SpeedImprovement = baseObject.SpeedImprovement;
			SpeedImprovementMultiplier = baseObject.SpeedImprovementMultiplier;

			NumberFormatter = HGM.NumberTextFormatter;

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
				if(ProductionImprovement){
					HGM.ApplyAllBonus(ProductionImprovementMultiplier);
				}

				if (SpeedImprovement){
					HGM.ApplyAllSpeedBonus(SpeedImprovementMultiplier);
				}
			} else {
				if (ProductionImprovement){
					HGM.ApplySpecificBonus(ProductionImprovementMultiplier, ImproveUnit);
				}

				if (SpeedImprovement){
					HGM.ApplySpecificSpeedBonus(SpeedImprovementMultiplier, ImproveUnit);
				}
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
			string improvementText = "";

			if(ProductionImprovement && SpeedImprovement){
				improvementText = "+ " + ProductionImprovementMultiplier + "x Production & + " + SpeedImprovementMultiplier + "x Speed" ;
			} else if(ProductionImprovement){
				improvementText = "+ " + ProductionImprovementMultiplier + "x Production";
			} else if (SpeedImprovement){
				improvementText = "+ " + SpeedImprovementMultiplier + "x Speed";
			}

			LockedNameText.text = baseObject.Name;
			LockedMultiplierText.text = improvementText;
			LockedAmountText.text = NumberFormatter.FormatNumber(Cost, 0);
			//LockedAmountText.text = Cost.ToString("#,#");
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
