using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Devdog.General;
using Devdog.General.UI;
using Devdog.InventoryPro;
using Devdog.InventoryPro.UI;
using UnityEngine.Serialization;
using Devdog.InventoryPro.Integration.UFPS;
using Devdog.General.ThirdParty.UniLinq;
using Sirenix.OdinInspector;

public enum RepairUnitPartState {
	None = 0,
	Broken = 1,
	Working = 2,
}

public enum RepairUnitPart {
	PowerUnit = 0,
	WireUnit = 1,
	CoolantUnit = 2,
}

[RequireComponent(typeof(Trigger))]
public class RepairableObject : MonoBehaviour, IInventoryItemContainer, ITriggerCallbacks {

	[BoxGroup("Visual Indicator Objects")]
	public GameObject visualWorkingIndicator;
	[BoxGroup("Visual Indicator Objects")]
	public GameObject visualBrokenIndicator;

	public RepairableUnitPart[] RepairableParts;

	[SerializeField]
	private InventoryItemBase[] _items = new InventoryItemBase[3];
	public InventoryItemBase[] items
	{
		get { return _items; }
		set { _items = value; }
	}

	[SerializeField]
	private string _uniqueName;
	public string uniqueName
	{
		get { return _uniqueName; }
		set { _uniqueName = value; }
	}

	private CollectionToArraySyncer _syncer;
	private ItemCollectionBase _collection;

	private ItemCollectionBase _windowCollection;

	private UIWindow window;
	private bool windowShown = false;

	private InventoryItemBase PowerUnitItem;
	private InventoryItemBase WireUnitItem;
	private InventoryItemBase CoolantUnitItem;

	// Use this for initialization
	void Awake () {
		var trigger = GetComponent<Trigger>();
		// The collection we want to place the items into.
		_collection = trigger.window.window.GetComponent<ItemCollectionBase>();

		Debug.Log(_collection + " is this null?");

		int i = 0;

		foreach(RepairableUnitPart r in RepairableParts){
			
			r.init();
			if(r.partState == RepairUnitPartState.Broken){
				r.currentItem = r.GetBrokenItem();
			} else if (r.partState == RepairUnitPartState.Working){
				r.currentItem = r.GetWorkingItem();
			} else {
				r.currentItem = null;
			}

			if(r.currentItem != null){
				r.currentItem = GameObject.Instantiate<InventoryItemBase>(r.currentItem);
				r.currentItem.transform.SetParent(transform);
				r.currentItem.gameObject.SetActive(false);
				_items[i] = r.currentItem;
			}

			i++;
		}

		FunctionalityCheck();
	}

	private void OnItemRemoved(InventoryItemBase item, uint itemID, uint slot, uint amount){
		foreach(RepairableUnitPart u in RepairableParts){
			if(item == u.currentItem){
				u.currentItem = null;
			}
		}
		FunctionalityCheck();
	}

	private void OnItemAdded(IEnumerable<InventoryItemBase> items, uint amount, bool cameFromCollection){
		foreach(RepairableUnitPart r in RepairableParts){
			r.currentItem = r.slot.item;
		}
		FunctionalityCheck();
	}

	private void OnItemSwapped(ItemCollectionBase i, uint u, ItemCollectionBase ii, uint uu){
		Debug.Log("Item Swapped");
		foreach(RepairableUnitPart r in RepairableParts){
			r.currentItem = r.slot.item;
		}
		FunctionalityCheck();
	}

	private void FunctionalityCheck(){

		int working = 0;
		int numberOfParts = 0;


		foreach(RepairableUnitPart r in RepairableParts){
			numberOfParts ++;
			if(r.currentItem != null){
				InventoryItemBase w = r.workingInventoryItem.GetComponent<InventoryItemBase>();
				InventoryItemBase b = r.brokenInventoryItem.GetComponent<InventoryItemBase>();
				Debug.Log(r.currentItem.ID + " - " + w.ID + " - " + b.ID + " current + working ID + broken ID");
				if(r.currentItem.ID == w.ID){
					working++;
					r.Working();
				} else if (r.currentItem.ID == b.ID){
					r.Broken();
				} else {
					r.None();
				}
			} else {
				r.None();
			}
		}

		if(numberOfParts == working){
			visualWorkingIndicator.SetActive(true);
			visualBrokenIndicator.SetActive(false);
		} else {
			visualWorkingIndicator.SetActive(false);
			visualBrokenIndicator.SetActive(true);
		}
	}

	public bool OnTriggerUsed(Player player){

		_collection.OnRemovedItem += OnItemRemoved;
		_collection.OnAddedItem += OnItemAdded;
		_collection.OnSwappedItems += OnItemSwapped;

		foreach(RepairableUnitPart ru in RepairableParts){
			_collection.SetItem(ru.slot.index, ru.currentItem, true);
		}

		_syncer = new CollectionToArraySyncer(_collection, items);
		_syncer.StartSyncing();
		return false;
	}
	
	public bool OnTriggerUnUsed(Player player)
	{
		_collection.OnRemovedItem -= OnItemRemoved;
		_collection.OnAddedItem -= OnItemAdded;
		_collection.OnSwappedItems -= OnItemSwapped;

		_syncer.StopSyncing();
		return false;
	}
}
