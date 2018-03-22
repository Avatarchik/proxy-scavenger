using System.Collections.Generic;
using Devdog.General.ThirdParty.UniLinq;
using System.Text;
using UnityEngine;
using Devdog.General;
using Devdog.InventoryPro;
using Sirenix.OdinInspector;

[System.Serializable]
public partial class RepairableUnitPart {
	[FoldoutGroup(" Repairable Unit Part")]
	//[BoxGroup("Parenting Object")]
	public GameObject parentObject;

	[FoldoutGroup(" Repairable Unit Part")]
	//[BoxGroup("Type and State")]
	public RepairUnitPart repairablePart;
	[FoldoutGroup(" Repairable Unit Part")]
	//[BoxGroup("Type and State")]
	public RepairUnitPartState partState;

	[FoldoutGroup(" Repairable Unit Part")]
	//[BoxGroup("Inventory Item Reference")]
	public GameObject workingInventoryItem;
	[FoldoutGroup(" Repairable Unit Part")]
	//[BoxGroup("Inventory Item Reference")]
	public GameObject brokenInventoryItem;

	[FoldoutGroup(" Repairable Unit Part")]
	//[BoxGroup("Item Anchors")]
	public GameObject itemAnchor;

	[FoldoutGroup(" Repairable Unit Part")]
	//[BoxGroup("Visual Item Objects")]
	public GameObject visualItemBroken;
	[FoldoutGroup(" Repairable Unit Part")]
	//[BoxGroup("Visual Item Objects")]
	public GameObject visualItemWorking;

	[FoldoutGroup(" Repairable Unit Part")]
	//[BoxGroup("Visual Item Items")]
	public GameObject visualItemBrokenItem;
	[FoldoutGroup(" Repairable Unit Part")]
	//[BoxGroup("Visual Item Items")]
	public GameObject visualItemWorkingItem;

	[FoldoutGroup(" Repairable Unit Part")]
	//[BoxGroup("UI Window Slot")]
	public ItemCollectionSlotUI slot;

	[FoldoutGroup(" Repairable Unit Part")]
	//[BoxGroup("Current Inventory Item")]
	public GameObject currentItemGO;

	[FoldoutGroup(" Repairable Unit Part")]
	//[BoxGroup("Current Inventory Item")]
	public InventoryItemBase currentItem;

	public void init(){
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

	public void Working(){
		visualItemBroken.SetActive(false);
		//visualItemWorking.SetActive(true);
		visualItemWorking.SetActive(false);

		visualItemWorkingItem.SetActive(true);
		visualItemBrokenItem.SetActive(false);
	}

	public void Broken(){
		//visualItemBroken.SetActive(true);
		visualItemWorking.SetActive(false);
		visualItemBroken.SetActive(false);

		visualItemBrokenItem.SetActive(true);
		visualItemWorkingItem.SetActive(false);
	}

	public void None(){
		visualItemBroken.SetActive(false);
		visualItemWorking.SetActive(false);

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

		Debug.Log(i.transform.position + " i - position : " + i.transform.rotation + " i - rotation : " + i.transform.localScale + " i - localScale");
		Debug.Log(i.transform.localPosition + " i - LocalPosition : " + i.transform.localRotation + " i - LocalRotation : " + i.transform.localScale + " i - LocalScale");
		return i;
	}
}
