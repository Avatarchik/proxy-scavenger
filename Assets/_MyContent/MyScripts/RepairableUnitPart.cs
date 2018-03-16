using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.InventoryPro;
using Sirenix.OdinInspector;

[System.Serializable]
public partial class RepairableUnitPart {

	[BoxGroup("Type and State")]
	public RepairUnitPart repairablePart;
	[BoxGroup("Type and State")]
	public RepairUnitPartState partState;

	[BoxGroup("Inventory Item Reference")]
	public GameObject workingInventoryItem;
	[BoxGroup("Inventory Item Reference")]
	public GameObject brokenInventoryItem;

	[BoxGroup("Visual Item Objects")]
	public GameObject visualItemBroken;
	[BoxGroup("Visual Item Objects")]
	public GameObject visualItemWorking;

	[BoxGroup("UI Window Slot")]
	public ItemCollectionSlotUI slot;

	[BoxGroup("Current Inventory Item")]
	public InventoryItemBase currentItem;

	public void init(){
		switch(partState){
		case RepairUnitPartState.None:
			None();
			break;
		case RepairUnitPartState.Broken:
			Broken();
			break;
		case RepairUnitPartState.Working:
			Working();
			break;
		}
	}

	public void Working(){
		visualItemBroken.SetActive(false);
		visualItemWorking.SetActive(true);
	}

	public void Broken(){
		visualItemBroken.SetActive(true);
		visualItemWorking.SetActive(false);
	}

	public void None(){
		visualItemBroken.SetActive(false);
		visualItemWorking.SetActive(false);
	}

	public InventoryItemBase GetBrokenItem(){
		InventoryItemBase i = brokenInventoryItem.GetComponent<InventoryItemBase>();
		return i;
	}

	public InventoryItemBase GetWorkingItem(){
		InventoryItemBase i = workingInventoryItem.GetComponent<InventoryItemBase>();
		return i;
	}
}
