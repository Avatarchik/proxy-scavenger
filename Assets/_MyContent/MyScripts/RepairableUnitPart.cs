﻿using System.Collections.Generic;
using Devdog.General.ThirdParty.UniLinq;
using System.Text;
using UnityEngine;
using Devdog.General;
using Devdog.InventoryPro;
using Sirenix.OdinInspector;

[System.Serializable]
public partial class RepairableUnitPart {
	[FoldoutGroup(" Repairable Unit Part")]
	public GameObject parentObject;

	[FoldoutGroup(" Repairable Unit Part")]
	public RepairUnitPart repairablePart;
	[FoldoutGroup(" Repairable Unit Part")]
	public RepairUnitPartState partState;
	[FoldoutGroup(" Repairable Unit Part")]
	public RepairUnitSlotPosition unitSlotPosition;

	[FoldoutGroup(" Repairable Unit Part")]
	public GameObject workingInventoryItem;
	[FoldoutGroup(" Repairable Unit Part")]
	public GameObject brokenInventoryItem;

	[FoldoutGroup(" Repairable Unit Part")]
	public GameObject itemAnchor;

	[FoldoutGroup(" Repairable Unit Part")]
	public GameObject visualItemBrokenItem;
	[FoldoutGroup(" Repairable Unit Part")]
	public GameObject visualItemWorkingItem;

	[FoldoutGroup(" Repairable Unit Part")]
	public ItemCollectionSlotUI slot;

	[FoldoutGroup(" Repairable Unit Part")]
	public GameObject currentItemGO;

	[FoldoutGroup(" Repairable Unit Part")]
	public InventoryItemBase currentItem;

	public void init(){
		if(unitSlotPosition != RepairUnitSlotPosition.RepairUnitSlotPositionNone){
			GameObject b = MakeVisualObject(brokenInventoryItem);
			GameObject w = MakeVisualObject(workingInventoryItem);

			visualItemBrokenItem = b;
			visualItemWorkingItem = w;

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
	}

	public void Working(){
		visualItemWorkingItem.SetActive(true);
		visualItemBrokenItem.SetActive(false);
	}

	public void Broken(){
		visualItemBrokenItem.SetActive(true);
		visualItemWorkingItem.SetActive(false);
	}

	public void None(){
		
		Debug.Log("None called fully");
		visualItemBrokenItem.SetActive(false);
		visualItemWorkingItem.SetActive(false);
		
	}

	public void SetOffUnitPart(){
		GameObject go = currentItem.gameObject;
		currentItemGO = MakeVisualObject(go);
		currentItemGO.SetActive(true);
	}

	public void DestroyOffUnitPart(){
		GameObject.Destroy(currentItemGO);
	}

	public InventoryItemBase GetBrokenItem(){
		InventoryItemBase i = brokenInventoryItem.GetComponent<InventoryItemBase>();
		return i;
	}

	public InventoryItemBase GetWorkingItem(){
		InventoryItemBase i = workingInventoryItem.GetComponent<InventoryItemBase>();
		return i;
	}

	public GameObject MakeVisualObject(GameObject g){
		Vector3 v = Vector3.zero;
		Quaternion q = Quaternion.identity;

		GameObject i = GameObject.Instantiate(g,v,q);
		UnityEngine.Object.Destroy(i.GetComponent<ITriggerInputHandler>() as UnityEngine.Component);
		UnityEngine.Object.Destroy(i.GetComponent<TriggerBase>());
		UnityEngine.Object.Destroy(i.GetComponent<InventoryItemBase>());
		UnityEngine.Object.Destroy(i.GetComponent<SphereCollider>());
		UnityEngine.Object.Destroy(i.GetComponent<BoxCollider>());
		UnityEngine.Object.Destroy(i.GetComponent<Rigidbody>());

		Vector3 p = itemAnchor.transform.position;

		i.transform.position = p;
		i.transform.rotation = q;
		i.transform.localScale = Vector3.one;

		i.transform.parent = parentObject.transform;

		Vector3 lp = itemAnchor.transform.localPosition;

		i.transform.localPosition = lp;
		i.transform.localRotation = q;
				
		return i;
	}
}
