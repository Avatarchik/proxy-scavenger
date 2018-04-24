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

public enum RepairObjectRepairState {
	Working = 0,
	Damaged = 1,
	Broken = 2,
	Destroyed = 3
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

	public GameObject PositionAAnchor;
	public GameObject PositionBAnchor;
	public GameObject PositionCAnchor;

	public GameObject windowToAnchor;

	public UItoWorldAnchor uiToAnchor;

	public ItemCollectionSlotUI[] uiSlots = new ItemCollectionSlotUI[3];

	public bool ItemAdjusted = false;

	public bool ObjectRepaired = false;

	public bool ObjectActive = false;

	public RepairObjectRepairState repairState = RepairObjectRepairState.Working;

	//public bool BoardBroken = true;
	//public bool BoardDestroyed = false;

	[BoxGroup("Visual Indicator Objects")]
	public GameObject visualWorkingIndicator;
	[BoxGroup("Visual Indicator Objects")]
	public GameObject visualBrokenIndicator;

	public RepairableUnitPart[] RepairableParts;
	public ComponentLayout[] ComponentLayouts;

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

	private ComponentPart[] ComponentParts;


	// Use this for initialization
	void Awake () {
		//init();
	}

	public void DungeonComplete(){
		AdjustItemPostions();
	}

	public void init(){
		var trigger = GetComponent<Trigger>();
		// The collection we want to place the items into.
		_collection = trigger.window.window.GetComponent<ItemCollectionBase>();

		Debug.Log(_collection + " is this null?");

		GameObject gm = GameObject.FindGameObjectWithTag("GameManager");
		ComponentManager cm = gm.GetComponentInChildren<ComponentManager>();
		ComponentParts = cm.GetComponentParts();
		windowToAnchor = GameObject.FindGameObjectWithTag("RepairWindow");
		uiToAnchor = windowToAnchor.GetComponent<UItoWorldAnchor>();
		uiSlots[0] = uiToAnchor.SlotA;
		uiSlots[1] = uiToAnchor.SlotB;
		uiSlots[2] = uiToAnchor.SlotC;

		/*
		 int i = 0;
		  
		int NumberOfComponentParts = 0;
		foreach(ComponentPart cp in ComponentParts){
			NumberOfComponentParts ++;
		}

		int HighestNumberOfItems = 0;
		foreach(ComponentLayout cl in ComponentLayouts){
			if(cl.SlotCount > HighestNumberOfItems){
				HighestNumberOfItems = cl.SlotCount;
			}
		}

		int rrPartCount = UnityEngine.Random.Range(0,(HighestNumberOfItems + 1));
		Debug.Log("Random Range for picking how many repair items are in the unit : " + rrPartCount);
		rrPartCount = 3; // setting it to 3 just for now until more layouts are made.

		RepairableParts = new RepairableUnitPart[rrPartCount];
		//RepairableUnitPart[] rp = new RepairableUnitPart[rrPartCount];

		for(int l = 0; l < rrPartCount; l++){
			RepairableParts[l] = new RepairableUnitPart();

			int RandomUnitPart = UnityEngine.Random.Range(0, NumberOfComponentParts);
			int RandomWorkingLevel = UnityEngine.Random.Range(0,3);
			RepairUnitPartState rups = (RepairUnitPartState)RandomWorkingLevel;

			RepairableParts[l].Setup(this.gameObject, ComponentParts[RandomUnitPart].GetPart(), rups, ComponentParts[RandomUnitPart].GetWorkingItem(), ComponentParts[RandomUnitPart].GetBrokenItem(), ComponentLayouts[0].Anchors[l].ItemAnchor, ComponentLayouts[0].Anchors[l].SlotUI, ComponentParts[RandomUnitPart].mountingObject);
		}


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
		*/

		initParts();

		FunctionalityCheck();
	}

	private void initParts(){
		int i = 0;

		int NumberOfComponentParts = 0;
		foreach(ComponentPart cp in ComponentParts){
			NumberOfComponentParts ++;
		}

		int HighestNumberOfItems = 0;
		foreach(ComponentLayout cl in ComponentLayouts){
			if(cl.Anchors.Length > HighestNumberOfItems){
				HighestNumberOfItems = cl.Anchors.Length;
			}
		}

		int rrPartCount = UnityEngine.Random.Range(0,(HighestNumberOfItems + 1));
		Debug.Log("Random Range for picking how many repair items are in the unit : " + rrPartCount);
		rrPartCount = 3; // setting it to 3 just for now until more layouts are made.

		RepairableParts = new RepairableUnitPart[rrPartCount];

		for(int l = 0; l < rrPartCount; l++){
			RepairableParts[l] = new RepairableUnitPart();

			int RandomWorkingLevel = 0;
			int RandomUnitPart = UnityEngine.Random.Range(0, NumberOfComponentParts);

			switch(repairState){
			case RepairObjectRepairState.Working:
				RandomWorkingLevel = 2;
				break;
			case RepairObjectRepairState.Damaged:
				RandomWorkingLevel = UnityEngine.Random.Range(1,3);
				break;
			case RepairObjectRepairState.Broken:
				RandomWorkingLevel = UnityEngine.Random.Range(0,3);
				break;
			case RepairObjectRepairState.Destroyed:
				RandomWorkingLevel = 0;
				break;
			}

			RepairUnitPartState rups = (RepairUnitPartState)RandomWorkingLevel;

			RepairableParts[l].Setup(this.gameObject, ComponentParts[RandomUnitPart].GetPart(), rups, ComponentParts[RandomUnitPart].GetWorkingItem(), ComponentParts[RandomUnitPart].GetBrokenItem(), ComponentLayouts[0].Anchors[l].ItemAnchor, uiSlots[l], ComponentParts[RandomUnitPart].mountingObject);
		}

		if(repairState == RepairObjectRepairState.Broken || repairState == RepairObjectRepairState.Damaged){
			int b = 0;
			foreach(RepairableUnitPart p in RepairableParts){
				if(p.partState != RepairUnitPartState.Working){
					b++;
				}
			}
			if(b == 0){
				int rr = UnityEngine.Random.Range(0, rrPartCount + 1);
				RepairableParts[rr].partState = RepairUnitPartState.Broken;
			}
		}

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
	}

	void Update(){
		if(!ItemAdjusted){
			AdjustItemPostions();
		}
	}

	private void AdjustItemPostions(){
		
		foreach(RepairableUnitPart r in RepairableParts){
			
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
			r.mountingItemGO.transform.localPosition = r.itemAnchor.transform.localPosition;
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

			} else {
				Debug.Log("r.None() on slot position none");
				r.None();
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
