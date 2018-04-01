using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.InventoryPro;
using Devdog.General;
using Devdog.General.UI;
using Devdog.InventoryPro.Dialogs;
using Devdog.General.ThirdParty.UniLinq;

public partial class HackableBox : MonoBehaviour, ITriggerCallbacks{

	public GameManager GM;
	public InventoryItemBase CurrentEquippedItem;
	public GameObject HackingItem;
	public GameObject HackingObject;

	// Use this for initialization
	void Start () {
		GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		CurrentEquippedItem = HackingItem.GetComponent<InventoryItemBase>();
	}

	void Update(){
		CurrentEquippedItem = GM.CurrentEquippedItem;
	}

	public bool OnTriggerUsed(Player player)
	{
		Debug.Log("Hacking Box Used!");
		if(CurrentEquippedItem != null){
			if(GM.CurrentEquippedItem == CurrentEquippedItem){
				Debug.Log("Hacking Item Used on Box");
				HackingObject.SetActive(true);
				GM.RemoveCurrentItem();
			}
		} else {
			Debug.Log("CurrentItem is null");
		}
		OnTriggerUnUsed(player);
		return true;
		// The trigger has been used.

		// Return true to consume the event (other callback listeners won't receive it)
		// Return false to not cosnume the event.
	}

	public bool OnTriggerUnUsed(Player player)
	{
		Debug.Log("Trigger Unused?");

		// The trigger has een unused
		// Return true to consume the event (other callback listeners won't receive it)
		// Return false to not cosnume the event.
		return true;
	}
}
