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

namespace mindler{
	[RequireComponent(typeof(Trigger))]
	public class InventoryItemInteraction : MonoBehaviour, IInventoryItemContainer, ITriggerCallbacks {

		[BoxGroup("UI Anchors")]
		public GameObject windowToAnchor;
		[BoxGroup("UI Anchors")]
		public UItoWorldAnchor uiToAnchor;

		[BoxGroup("Items Adjusted")]
		public bool ItemAdjusted = false;

		[BoxGroup("Items Units")]
		public InventoryInteractableItemUnit[] ItemUnits;

		[BoxGroup("Items Layouts")]
		public ComponentLayout[] ItemLayouts;

		[BoxGroup("Selected Layout")]
		public ItemSlotAnchors[] SelectedItemLayout;

		[BoxGroup("Item Objects")]
		public InventoryInteractableItemObject[] ItemObjects;


		[SerializeField]
		private InventoryItemBase[] _items = new InventoryItemBase[3];
		private InventoryItemBase[] items
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

			SelectedItemLayout = ItemLayouts[0].Anchors;

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
				if(i.mountingItemGO != null){
					i.mountingItemGO.transform.localPosition = i.itemAnchor.transform.localPosition;
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

			ItemAdjusted = false;
			AdjustItemPostions();
		}

		private void OnItemAdded(IEnumerable<InventoryItemBase> items, uint amount, bool cameFromCollection)
		{
			foreach(InventoryInteractableItemUnit r in ItemUnits){
				if(r.currentItem != r.slot.item){
					r.currentItem = r.slot.item;
					r.SetCurrentItem();
				}
			}

			ItemAdjusted = false;
			AdjustItemPostions();
		}

		private void OnItemSwapped(ItemCollectionBase i, uint u, ItemCollectionBase ii, uint uu)
		{
			foreach(InventoryInteractableItemUnit r in ItemUnits){
				if(r.currentItem != r.slot.item){
					r.currentItem = r.slot.item;
					r.SetCurrentItem();
				}
			}

			ItemAdjusted = false;
			AdjustItemPostions();
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