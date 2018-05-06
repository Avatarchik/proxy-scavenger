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

public enum InteractiveInventoryItemType {
	None = 0,
	HackingUnit = 1,
	RepairUnit = 2,
}

namespace mindler{

	[RequireComponent(typeof(Trigger))]
	public class InventoryItemInteraction : MonoBehaviour, IInventoryItemContainer, ITriggerCallbacks {

		[BoxGroup("UI Anchors")]
		public GameObject windowToAnchor;
		[BoxGroup("UI Anchors")]
		public UItoWorldAnchor uiToAnchor;

		//[BoxGroup("Interactive Inventory Item Type")]
		public InteractiveInventoryItemType Type = InteractiveInventoryItemType.None;

		[BoxGroup("Items Adjusted")]
		public bool ItemAdjusted = false;

		[ToggleGroup("PreloadWithItems")]
		public bool PreloadWithItems = false;

		[ToggleGroup("RandomizePreloadedItemsFromList"), ShowIf("PreloadWithItems")]
		public bool RandomizePreloadedItemsFromList = false;
		[ShowIf("PreloadWithItems"), ShowIf("RandomizePreloadedItemsFromList")]
		public ItemObjectSelectionList ItemSelectionList;
		[ShowIf("PreloadWithItems")]
		public InventoryInteractableItemObject[] PreloadItems;

		//[ShowIf("Type", InteractiveInventoryItemType.RepairUnit)]
		[FoldoutGroup("Repair Item Options")]
		public bool RandomizeRepairItemUnitState = true;
		//[ShowIf("Type", InteractiveInventoryItemType.RepairUnit)]
		[FoldoutGroup("Repair Item Options")]
		public RepairUnitPartState[] RepairUnitPartStates;


		[BoxGroup("Items Units")]
		public InventoryInteractableItemUnit[] ItemUnits;

		[BoxGroup("Items Layouts")]
		public ComponentLayout[] ItemLayouts;

		[BoxGroup("Layout Selector")]
		public int ChosenLayout = 0;
		[BoxGroup("Layout Selector")]
		public bool RandomizeLayout = true;

		//[BoxGroup("Item Change Notifiers")]
		public delegate void ItemChange();
		//[BoxGroup("Item Change Notifiers")]
		public event ItemChange OnItemChange;

		//[BoxGroup("Selected Layout")]
		private ItemSlotAnchors[] SelectedItemLayout;

		//[BoxGroup("Item Objects")]
		//public InventoryInteractableItemObject[] ItemObjects;


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

		public UIWindow window;
		private bool windowShown = false;

		public void Start(){
			init();
		}


		public void DungeonComplete()
		{
			AdjustItemPostions();
		}

		public void init()
		{
			windowToAnchor = GameObject.FindGameObjectWithTag("WorldItemWindow");
			uiToAnchor = windowToAnchor.GetComponent<UItoWorldAnchor>();
			window = windowToAnchor.GetComponent<UIWindow>();

			if(ItemSelectionList){
				Type = ItemSelectionList.Type;
			}

			if(RandomizeLayout){
				if(ItemLayouts.Length > 1){
					ChosenLayout = UnityEngine.Random.Range(0, ItemLayouts.Length);
				} else {
					ChosenLayout = 0;
				}
			}

			SelectedItemLayout = ItemLayouts[ChosenLayout].Anchors;

			for(var i = 0; i < SelectedItemLayout.Length; i++){
				SelectedItemLayout[i].SlotUI = uiToAnchor.Slots[i];
			}

			var trigger = GetComponent<Trigger>();
			trigger.window.window = window;
			// The collection we want to place the items into.
			_collection = window.GetComponent<ItemCollectionBase>();

			initParts();
		}

		private void initParts()
		{
			// init parts?
			ItemUnits = new InventoryInteractableItemUnit[SelectedItemLayout.Length];

			for(var i = 0; i < ItemUnits.Length; i++){
				ItemUnits[i] = new InventoryInteractableItemUnit();
				ItemSlotAnchors a = SelectedItemLayout[i];
				ItemUnits[i].Setup(this.gameObject, a.ItemAnchor, a.SlotUI);

				ItemPreloader(i);

				switch(Type){
					case InteractiveInventoryItemType.HackingUnit:
						// Do Special Hacking Unit Code
						Debug.Log("Hacking Unit Item Setup");
						break;
					case InteractiveInventoryItemType.RepairUnit:
						RepairUnitItemSetup(i);
						Debug.Log("Repair Unit Item Setup");
						break;
					case InteractiveInventoryItemType.None:
						// Do Special None Code
						Debug.Log("None Unit Item Setup");
						break;
				}

				ItemUnits[i].init();
			}
			ItemAdjusted = false;
		}

		private void ItemPreloader(int ItemUnitIndex){
			if(PreloadWithItems)
			{
				if(RandomizePreloadedItemsFromList)
				{
					int r = UnityEngine.Random.Range(0, ItemSelectionList.ItemSelectionList.Length);
					ItemUnits[ItemUnitIndex].itemObject = ItemSelectionList.ItemSelectionList[r];
				} else {
					InventoryInteractableItemUnit x = ItemUnits[ItemUnitIndex];
					int index = ItemUnitIndex;
					if(index >= ItemSelectionList.ItemSelectionList.Length){
						index = ItemSelectionList.ItemSelectionList.Length - 1;
					}
					InventoryInteractableItemObject y = ItemSelectionList.ItemSelectionList[index];

					if(x != null && y != null)
					{
						x.itemObject = y;
					}
				}
			}
		}

		private void RepairUnitItemSetup(int ItemUnitIndex){
			RepairUnitPartState partState = RepairUnitPartState.Working;

			if(RandomizeRepairItemUnitState){
				int randomState = UnityEngine.Random.Range(0, 3);
				partState = (RepairUnitPartState)randomState;
			} else {
				partState = RepairUnitPartStates[ItemUnitIndex];
			}

			Debug.Log(partState + " RepairUnitItemSetup - Part State");

			switch(partState){
				case RepairUnitPartState.None:
					ItemUnits[ItemUnitIndex].initialItemEmpty = true;
					Debug.Log("Current Item set to null because part state is NONE");
					break;
				case RepairUnitPartState.Working:
					ItemUnits[ItemUnitIndex].initialItemEmpty = false;
					ItemUnits[ItemUnitIndex].currentItemGO = ItemUnits[ItemUnitIndex].itemObject.InventoryItem;
					Debug.Log(ItemUnits[ItemUnitIndex].currentItemGO + " - Current Item set to main item because part state is working");
					break;
				case RepairUnitPartState.Broken:
					ItemUnits[ItemUnitIndex].initialItemEmpty = false;
					ItemUnits[ItemUnitIndex].currentItemGO = ItemUnits[ItemUnitIndex].itemObject.BrokenInventoryItemVariant;
					Debug.Log(ItemUnits[ItemUnitIndex].currentItemGO + " - Current Item set to variant item because part state is broken");
					break;
			}
		}

		public void Update(){
			if(!ItemAdjusted){
				AdjustItemPostions();
			}
		}

		private void AdjustItemPostions()
		{
			//Adjust item position
			foreach(InventoryInteractableItemUnit i in ItemUnits){
				if(i.currentItemGO){
					i.currentItemGO.transform.localPosition = Vector3.zero;
					i.currentItemGO.transform.localRotation = Quaternion.identity;
				}
				if(i.visualItem){
					i.visualItem.transform.localPosition = Vector3.zero;
					i.visualItem.transform.localRotation = Quaternion.identity;
					Debug.Log(i.currentItem.name + " : " + i.visualItem.transform.localRotation + " visual item's local rotation");
				}
				if(i.mountingItemGO != null){
					i.mountingItemGO.transform.localPosition = Vector3.zero;
				}
					
			}

			ItemAdjusted = true;
		}

		private void OnItemRemoved(InventoryItemBase item, uint itemID, uint slot, uint amount)
		{
			foreach(InventoryInteractableItemUnit r in ItemUnits){
				if(item == r.currentItem){
					r.DestroyItem();
					r.currentItem = null;
				}
			}

			ItemChanged();
		}

		private void OnItemAdded(IEnumerable<InventoryItemBase> items, uint amount, bool cameFromCollection)
		{
			foreach(InventoryInteractableItemUnit r in ItemUnits){
				if(r.currentItem != r.slot.item){
					r.currentItem = r.slot.item;
					r.SetCurrentItem();
				}
			}

			ItemChanged();
		}

		private void OnItemSwapped(ItemCollectionBase i, uint u, ItemCollectionBase ii, uint uu)
		{
			foreach(InventoryInteractableItemUnit r in ItemUnits){
				if(r.currentItem != r.slot.item){
					r.currentItem = r.slot.item;
					r.SetCurrentItem();
				}
			}

			ItemChanged();
		}

		private void ItemChanged()
		{
			ItemAdjusted = false;
			AdjustItemPostions();

			if(OnItemChange != null)
			{
				OnItemChange();
			}
		}

		public bool OnTriggerUsed(Player player)
		{
			uiToAnchor.positionsToFollow = new Transform[SelectedItemLayout.Length];
			for(int i = 0; i < SelectedItemLayout.Length; i++){
				uiToAnchor.positionsToFollow[i] = SelectedItemLayout[i].ItemAnchor.transform;
			}

			_collection.OnRemovedItem += OnItemRemoved;
			_collection.OnAddedItem += OnItemAdded;
			_collection.OnSwappedItems += OnItemSwapped;

			foreach(InventoryInteractableItemUnit u in ItemUnits){
				_collection.SetItem(u.slot.index, u.currentItem, true);
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
}