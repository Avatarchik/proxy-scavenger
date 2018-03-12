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

[RequireComponent(typeof(MyItemTrigger))]
public class EquippableEquipmentHolderUFPSInventoryItem : EquippableUFPSInventoryItem, IInventoryItemContainer, ITriggerCallbacks
{
	/*
	public delegate void LootedItem(InventoryItemBase item, uint itemID, uint slot, uint amount);
	public delegate void Empty();

	/// <summary>
	/// Called when an item was looted by a player from this lootable object.
	/// </summary>
	public event LootedItem OnLootedItem;
	public event Empty OnEmpty;
	*/

	[SerializeField]
	private InventoryItemBase[] _items = new InventoryItemBase[0];
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

	public int capacity = 25;

	private CollectionToArraySyncer _syncer;
	private ItemCollectionBase _collection;

	private UIWindow window;
	private bool windowShown = false;
	private Stat heat;

	public void Awake()
	{
		// Create instance objects.
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i] != null)
			{
				items[i] = GameObject.Instantiate<InventoryItemBase>(items[i]);
				items[i].transform.SetParent(transform);
				items[i].gameObject.SetActive(false);
			}
		}

		vp_UnitBankType ub = (vp_UnitBankType)itemType;
		Debug.Log(ub + " : " + ub.Capacity + " Capacity - Awake");
		ub.Capacity = capacity;
		Debug.Log(ub + " : " + ub.Capacity + " New Capacity - Awake");

		// The triggerHandler component, that is always there because of RequireComponent
		var trigger = GetComponent<Trigger>();

		// The collection we want to place the items into.
		if(trigger != null){
			_collection = trigger.window.window.GetComponent<ItemCollectionBase>();
			if(_collection == null){
				Debug.Log("_collection is null from trigger searching");
				GameObject g;
				if(equipmentType.name == "Suit"){
					Debug.Log("Item is a Suit object");
					g = GameObject.FindGameObjectWithTag("SuitInventoryWindow");
				} else {
					Debug.Log("Item is an Equipment object");
					g = GameObject.FindGameObjectWithTag("EquipmentInventoryWindow");
				}
				var w = g.GetComponent<UIWindow>();
				window = w;
				_collection = w.GetComponent<ItemCollectionBase>();
				_collection.OnAddedItem += OnItemAdded;
			}
		}
	}

	public void OnItemAdded(IEnumerable<InventoryItemBase> item, uint amount, bool cameFromCollection ){
		Debug.Log("An Item Was Added to the Collection");
		GetStats();
		GetInfo();
	}

	public bool OnTriggerUsed(Player player)
	{
		/*
		// When the user has triggered this object, set the items in the window
		_collection.SetItems(items, true);

		_syncer = new CollectionToArraySyncer(_collection, items);
		_syncer.StartSyncing();
		*/
		// And done!
		return false;
	}

	public bool OnTriggerUnUsed(Player player)
	{
		//_syncer.StopSyncing();
		return false;
	}

	public override int Use ()
	{
		// When the user has triggered this object, set the items in the window
		if(items == null){
			Debug.Log("items IS null");
		} else {
			Debug.Log("Items are NOT null");
		}

		if(_collection == null){
			Debug.Log("_collection IS null");
			Debug.Log("_collection is null from trigger searching");
			GameObject g;
			if(equipmentType.name == "Suit"){
				Debug.Log("Item is a Suit object");
				g = GameObject.FindGameObjectWithTag("SuitInventoryWindow");
			} else {
				Debug.Log("Item is an Equipment object");
				g = GameObject.FindGameObjectWithTag("EquipmentInventoryWindow");
			}
			var w = g.GetComponent<UIWindow>();
			window = w;
			_collection = w.GetComponent<ItemCollectionBase>();
			_collection.OnAddedItem += OnItemAdded;
			if(_collection == null){
				Debug.Log("What the fuck?");
			}
		} else {
			Debug.Log("_collection is NOT null");
		}

		if(!windowShown){
			windowShown = true;
			GetStats();
			window.Show();
		} else {
			windowShown = false;
			GetStats();
			window.Hide();
		}

		_collection.SetItems(items, true);

		_syncer = new CollectionToArraySyncer(_collection, items);
		_syncer.StartSyncing();
		//return base.Use ();
		return 1;
	}

	public void GetStats(){
		float heat = 0f;
		float cold = 0f;
		float toxic = 0f;
		float rad = 0f;

		foreach(var i in items){
			if(i != null){
				var l = i.stats.Length;
				Debug.Log(l + " Stats Length");
				var s = i.stats;
				foreach(var st in s){
					switch(st.stat.name){
					case "Heat":
						heat += st.floatValue;
						break;
					case "Cold":
						cold += st.floatValue;
						break;
					case "Toxic":
						toxic += st.floatValue;
						break;
					case "Radiation":
						rad += st.floatValue;
						break;
					}
					Debug.Log("Stat! " + st.stat.name + " : " + st.stat.baseValue + " Base Value - " + st.floatValue + " Float Value");
				}
			}
		}
		foreach(var t in stats){
			switch(t.stat.name){
			case "Heat":
				t.floatValue = heat;
				break;
			case "Cold":
				t.floatValue = cold;
				break;
			case "Toxic":
				t.floatValue = toxic;
				break;
			case "Radiation":
				t.floatValue = rad;
				break;
			}
			Debug.Log(t.floatValue + " : " + t.stat.name + " : " + "current value, if over max will set to max");
			if(t.floatValue > t.stat.maxValue){
				t.floatValue = t.stat.maxValue;
			}
		}
	}

	public override LinkedList<ItemInfoRow[]> GetInfo ()
	{

		var list = new LinkedList<ItemInfoRow[]>();

		list.AddLast(new ItemInfoRow[]{
			//new ItemInfoRow("Category", category.name),
			new ItemInfoRow("Type", equipmentType.name),
		});

		var extra = new List<ItemInfoRow>(0);

		if (extra.Count > 0)
		{
			list.AddLast(extra.ToArray());
		}

		var extraProperties = new List<ItemInfoRow>();
		foreach (var property in stats)
		{
			var prop = property.stat;
			if (prop == null)
			{
				continue;
			}

			if(prop.showInUI)
			{
				if(property.isFactor && property.isSingleValue)
					extraProperties.Add(new ItemInfoRow(prop.statName, (property.floatValue - 1.0f) * 100 + "%", prop.color, prop.color));
				else
					extraProperties.Add(new ItemInfoRow(prop.statName, property.value, prop.color, prop.color));
			}
		}

		if(extraProperties.Count > 0)
			list.AddLast(extraProperties.ToArray());

		return list;
		//return base.GetInfo ();
	}
}
