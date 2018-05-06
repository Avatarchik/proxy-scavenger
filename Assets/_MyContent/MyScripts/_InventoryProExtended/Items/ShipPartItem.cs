using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.General;
using Devdog.InventoryPro;
using Devdog.InventoryPro.Integration.UFPS;
using mindler;

public class ShipPartItem : EquippableEquipmentUFPSInventoryItem {

	public CurrencyDefinition Currency;
	public ItemQualityPrice[] QualityValues;

	protected override void Awake(){
		base.Awake();
		AdjustQualityPrice();
	}

	public void AdjustQualityPrice(){
		foreach(ItemQualityPrice q in QualityValues){
			if(rarity == q.Rarity){
				buyPrice.currency = Currency;
				buyPrice.amount = q.BuyPrice;

				sellPrice.currency = Currency;
				sellPrice.amount = q.SellPrice;
			}
		}
	}
		
}
