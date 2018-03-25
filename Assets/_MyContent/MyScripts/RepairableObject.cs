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
	MemoryUnit = 3,
}

public enum RepairUnitSlotPosition {
	RepairUnitSlotPositionNone = 0,
	RepairUnitSlotPositionA = 1,
	RepairUnitSlotPositionB = 2,
	RepairUnitSlotPositionC = 3,
	RepairUnitSlotPositionD = 4,
	RepairUnitSlotPositionE = 5,
	RepairUnitSlotPositionF = 6,
}

[RequireComponent(typeof(Trigger))]
public class RepairableObject : MonoBehaviour, IInventoryItemContainer, ITriggerCallbacks {

	public GameObject Anchor;
	public GameObject PositionAAnchor;
	public GameObject PositionBAnchor;
	public GameObject PositionCAnchor;
	public GameObject windowToAnchor;

	public bool ItemAdjusted = false;

	public bool ObjectRepaired = false;

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
	private InventoryItemBase MemoryUnitItem;

	// Use this for initialization
	void Awake () {
		var trigger = GetComponent<Trigger>();
		// The collection we want to place the items into.
		_collection = trigger.window.window.GetComponent<ItemCollectionBase>();

		Debug.Log(_collection + " is this null?");

		int i = 0;

		foreach(RepairableUnitPart r in RepairableParts){

			if(r.unitSlotPosition != RepairUnitSlotPosition.RepairUnitSlotPositionNone){
				r.init();

				Debug.Log(r.visualItemBrokenItem.transform.localPosition + " broken local position");
				Debug.Log(r.visualItemWorkingItem.transform.localPosition + " working local position");

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


		}

		FunctionalityCheck();
	}

	void Update(){
		if(!ItemAdjusted){
			AdjustItemPostions();
		}
	}

	private void AdjustItemPostions(){
		
		foreach(RepairableUnitPart r in RepairableParts){
			if(r.unitSlotPosition != RepairUnitSlotPosition.RepairUnitSlotPositionNone){
				if(r.visualItemBrokenItem ){
					if(r.visualItemBrokenItem.transform.localPosition != r.itemAnchor.transform.localPosition){
						r.visualItemBrokenItem.transform.localPosition = r.itemAnchor.transform.localPosition;
						r.visualItemBrokenItem.transform.localRotation = Quaternion.identity;

						Debug.Log(r.visualItemBrokenItem.transform.localPosition + " Repair Box Main File - Local Position of broken item");
					}
				}
				if(r.visualItemWorkingItem){
					if(r.visualItemWorkingItem.transform.localPosition != r.itemAnchor.transform.localPosition){
						r.visualItemWorkingItem.transform.localPosition = r.itemAnchor.transform.localPosition;
						r.visualItemWorkingItem.transform.localRotation = Quaternion.identity;

						Debug.Log(r.visualItemWorkingItem.transform.localPosition + " Repair Box Main File - Local Position of working item");
					}
				}
				if(r.currentItemGO){
					if(r.currentItemGO.transform.localPosition != r.itemAnchor.transform.localPosition){
						r.currentItemGO.transform.localPosition = r.itemAnchor.transform.localPosition;
						r.currentItemGO.transform.localRotation = Quaternion.identity;
					}
				}
			}
		}

		ItemAdjusted = true;
	}

	private void OnItemRemoved(InventoryItemBase item, uint itemID, uint slot, uint amount){
		foreach(RepairableUnitPart u in RepairableParts){
			if(item == u.currentItem){
				u.currentItem = null;
			}
		}
		FunctionalityCheck();
		ItemAdjusted = false;
	}

	private void OnItemAdded(IEnumerable<InventoryItemBase> items, uint amount, bool cameFromCollection){
		foreach(RepairableUnitPart r in RepairableParts){
			r.currentItem = r.slot.item;
		}
		FunctionalityCheck();
		ItemAdjusted = false;
	}

	private void OnItemSwapped(ItemCollectionBase i, uint u, ItemCollectionBase ii, uint uu){
		Debug.Log("Item Swapped");
		foreach(RepairableUnitPart r in RepairableParts){
			r.currentItem = r.slot.item;
		}
		FunctionalityCheck();
		ItemAdjusted = false;
	}

	private void FunctionalityCheck(){

		int working = 0;
		int numberOfParts = 0;


		foreach(RepairableUnitPart r in RepairableParts){
			
			if(r.unitSlotPosition != RepairUnitSlotPosition.RepairUnitSlotPositionNone){
				numberOfParts ++;
				if(r.currentItem != null){
					InventoryItemBase w = r.workingInventoryItem.GetComponent<InventoryItemBase>();
					InventoryItemBase b = r.brokenInventoryItem.GetComponent<InventoryItemBase>();
					Debug.Log(r.currentItem.ID + " - " + w.ID + " - " + b.ID + " current + working ID + broken ID");
					if(r.currentItem.ID == w.ID){
						working++;
						if(r.currentItemGO != null){
							r.DestroyOffUnitPart();
						}
						r.Working();
					} else if (r.currentItem.ID == b.ID){
						if(r.currentItemGO != null){
							r.DestroyOffUnitPart();
						}
						r.Broken();
					} else if (r.currentItem.ID != w.ID && r.currentItem.ID != b.ID){
						Debug.Log("r.currentItem.ID != w.ID or b.IDictionary");
						r.None();
						if(r.currentItemGO != null){
							r.DestroyOffUnitPart();
						}
						r.SetOffUnitPart();
					} else {
						Debug.Log("r.None() being called");
						r.None();
					}
				}
			} else {
				Debug.Log("r.None() on slot position none");
				//r.None();
			}
		}

		if(numberOfParts == working){
			visualWorkingIndicator.SetActive(true);
			visualBrokenIndicator.SetActive(false);
			ObjectRepaired = true;

		} else {
			visualWorkingIndicator.SetActive(false);
			visualBrokenIndicator.SetActive(true);
			ObjectRepaired = false;

		}
	}

	public bool OnTriggerUsed(Player player){

		var windowAnchor = windowToAnchor.GetComponent<UItoWorldAnchor>();
		//windowAnchor.objectToFollow = Anchor.transform;
		windowAnchor.positionAToFollow = PositionAAnchor.transform;
		windowAnchor.positionBToFollow = PositionBAnchor.transform;
		windowAnchor.positionCToFollow = PositionCAnchor.transform;

		_collection.OnRemovedItem += OnItemRemoved;
		_collection.OnAddedItem += OnItemAdded;
		_collection.OnSwappedItems += OnItemSwapped;

		foreach(RepairableUnitPart ru in RepairableParts){
			if(ru.unitSlotPosition != RepairUnitSlotPosition.RepairUnitSlotPositionNone){
				_collection.SetItem(ru.slot.index, ru.currentItem, true);
			}
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
