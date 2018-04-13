using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
using Devdog.General;
using Devdog.InventoryPro;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.General.UI;
using Sirenix.OdinInspector;
using DunGen;
using UnityEngine.UI;
using mindler.hacking;
using mindler.dungeonship;

public enum HazardType {
	None = 0,
	Heat = 1,
	Toxic = 2,
	Radiation = 3,
	Cold = 4,
	Deoxygenation = 5
}

public enum HazardLevel {
	None = 0,
	Low = 1,
	Medium = 2,
	High = 3,
	Extreme = 4
}

public enum Quality {
	Basic = 0,
	Standard = 1,
	Specialized = 2,
	Superior = 3,
	HighEnd = 4
}

public enum ShipSalvage {
	Salvageable = 0,
	Unsalvageable = 1
}

public enum ShipRecovery {
	Unrecovered = 0,
	Recovered = 1
}

public class GameManager : MonoBehaviour {

	[FoldoutGroup("Hazard Icons")]
	public GameObject HeatIcon;
	[FoldoutGroup("Hazard Icons")]
	public GameObject ToxicIcon;
	[FoldoutGroup("Hazard Icons")]
	public GameObject RadiationIcon;
	[FoldoutGroup("Hazard Icons")]
	public GameObject ColdIcon;
	[FoldoutGroup("Hazard Icons")]
	public GameObject o2Icon;

	[FoldoutGroup("Hazard Screen Effects")]
	public GameObject HeatScreenFX;
	[FoldoutGroup("Hazard Screen Effects")]
	public GameObject ToxicScreenFX;
	[FoldoutGroup("Hazard Screen Effects")]
	public GameObject RadiationScreenFX;
	[FoldoutGroup("Hazard Screen Effects")]
	public GameObject ColdScreenFX;
	[FoldoutGroup("Hazard Screen Effects")]
	public GameObject o2ScreenFX;

	[BoxGroup("Hazard Info")]
	public bool InHazard = false;
	[BoxGroup("Hazard Info")]
	public float HazardDamage = 1f;
	[BoxGroup("Hazard Info")]
	public HazardLevel CurrentHazardLevel = HazardLevel.None;
	[BoxGroup("Hazard Info")]
	public int HeatIndex = 0;
	[BoxGroup("Hazard Info")]
	public int ColdIndex = 0;
	[BoxGroup("Hazard Info")]
	public int ToxicIndex = 0;
	[BoxGroup("Hazard Info")]
	public int RadiationIndex = 0;
	[BoxGroup("Hazard Info")]
	public int OxygenIndex = 0;

	[BoxGroup("Ship Dock")]
	public GameObject SpaceStation;
	[BoxGroup("Ship Dock")]
	public GameObject DungeonGenerator;
	[BoxGroup("Ship Dock")]
	public Transform DungeonStart;
	[BoxGroup("Ship Dock")]
	public GameObject GeneratedDungeon;
	[BoxGroup("Ship Dock")]
	public DotHskDoor ControlRoomDoor;
	[BoxGroup("Ship Dock")]
	public Text ShipCourseText;
	[BoxGroup("Ship Dock")]
	public DungeonInfo GeneratedDungeonInfo;

	[BoxGroup("Dungeon Info")]
	public DungenCharacter DungenCharacter;

	[BoxGroup("Character Inventory Info")]
	public CharacterUI CharacterInventoryUI;
	[BoxGroup("Character Inventory Info")]
	public InventoryItemBase CurrentEquippedItem;
	[BoxGroup("Character Inventory Info")]
	public uint EquipmentSlotUint;

	[BoxGroup("Map")]
	public GameObject MapCamera;
	[BoxGroup("Map")]
	public bool ShowMap = false;

	[BoxGroup("Managers")]
	public HackingGameManager HackManager;
	[BoxGroup("Managers")]
	public DungeonShipManager DungeonManager;

	private Tile newTile = null;
	private Tile prevTile = null;

	private InventoryPlayer pInventory;
	private Stat statPower;
	private Stat statShields;
	private Stat statHealth;
	private Stat statHeatReduction;
	private Stat statToxicReduction;
	private Stat statRadiationReduction;
	private Stat statColdReduction;
	private Stat statO2Reduction;

	private bool powerDepleted = false;
	private bool shieldsDepleted = false;
	private bool healthDepleted = false;



	//public GameObject ColdIcon;

	// Use this for initialization
	void Start () {

		pInventory = PlayerManager.instance.currentPlayer.inventoryPlayer;
		statPower = (Stat)pInventory.stats.Get("Suit","Power");
		statHealth = (Stat)pInventory.stats.Get("Suit","Health");
		statShields = (Stat)pInventory.stats.Get("Suit","Shields");
		statHeatReduction = (Stat)pInventory.stats.Get("Hazard","Heat");
		statToxicReduction = (Stat)pInventory.stats.Get("Hazard","Toxic");
		statRadiationReduction = (Stat)pInventory.stats.Get("Hazard","Radiation");
		statColdReduction = (Stat)pInventory.stats.Get("Hazard","Cold");
		statO2Reduction = (Stat)pInventory.stats.Get("Hazard","Oxegyn");

		HeatIcon.SetActive(false);
		ToxicIcon.SetActive(false);
		RadiationIcon.SetActive(false);
		ColdIcon.SetActive(false);
		o2Icon.SetActive(false);

		HeatScreenFX.SetActive(false);
		ToxicScreenFX.SetActive(false);
		RadiationScreenFX.SetActive(false);
		ColdScreenFX.SetActive(false);
		o2ScreenFX.SetActive(false);

		SpaceStation.SetActive(false);
		//HeatDungeon.SetActive(false);

		if(HackManager == null){
			HackManager = this.gameObject.GetComponentInChildren<HackingGameManager>();
		}
		if(DungeonManager == null){
			DungeonManager = this.gameObject.GetComponentInChildren<DungeonShipManager>();
		}

		MessageDispatcher.AddListener("Hazard", Hazards, true);
		MessageDispatcher.AddListener("Hazards", Hazard, true); 

		foreach(EquippableSlot i in CharacterInventoryUI.equippableSlots){
			if(i.name == "Equipment Slot"){
				EquipmentSlotUint = i.index;
			}
		}

		CharacterInventoryUI.OnAddedItem += ItemAdded;
		CharacterInventoryUI.OnRemovedItem += ItemRemoved;
		CharacterInventoryUI.OnSwappedItems += ItemSwapped;

		DungenCharacter.OnTileChanged += OnCharacterTileChanged;

		StartCoroutine ("UpdateTick");
	}

	private void ItemAdded(IEnumerable<InventoryItemBase> items, uint u, bool b){
		foreach(EquippableSlot i in CharacterInventoryUI.equippableSlots){
			if(i.name == "Equipment Slot"){
				CurrentEquippedItem = i.slot.item;
				if(CurrentEquippedItem != null){
					var bc = CurrentEquippedItem.gameObject.GetComponents<Collider>();
					foreach(Collider box in bc){
						box.enabled = false;
					}
					var cc = CurrentEquippedItem.gameObject.GetComponentsInChildren<Collider>();
					foreach(Collider ccc in cc){
						ccc.enabled = false;
					}
				}
			}
		}
	}

	private void ItemRemoved(InventoryItemBase i, uint ID, uint slot, uint amount){
		InventoryItemBase c = (InventoryItemBase)CurrentEquippedItem;
		if(ID == EquipmentSlotUint && c == i){
			CurrentEquippedItem = null;
			var bc = CurrentEquippedItem.gameObject.GetComponents<BoxCollider>();
			foreach(BoxCollider box in bc){
				//box.enabled = true;
			}
		}
	}

	private void ItemSwapped(ItemCollectionBase fromCollection, uint fromSlot, ItemCollectionBase toCollection, uint toSlot){
		foreach(EquippableSlot i in CharacterInventoryUI.equippableSlots){
			if(i.name == "Equipment Slot"){
				if(i.slot.item != null){
					CurrentEquippedItem = i.slot.item;
					var bc = CurrentEquippedItem.gameObject.GetComponents<Collider>();
					foreach(Collider box in bc){
						box.enabled = false;
					}
					var cc = CurrentEquippedItem.gameObject.GetComponentsInChildren<Collider>();
					foreach(Collider ccc in cc){
						ccc.enabled = false;
					}
				} else {
					CurrentEquippedItem = null;
				}
			}
		}
	}

	public void RemoveCurrentItem(){
		ItemCollectionBase icb = CharacterInventoryUI;
		uint itemsRemoved = icb.RemoveItem(CurrentEquippedItem.ID, 1); // (0) ItemID, (1) Amount of items to remove
	}

	IEnumerator UpdateTick()
	{
		yield return new WaitForSeconds(1f);
		ManageUpdateTick();
		StartCoroutine ("UpdateTick");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.M)){
			if(ShowMap){
				MapCamera.SetActive(false);
				ShowMap = false;
			} else {
				MapCamera.SetActive(true);
				ShowMap = true;
			}
		}

		if(CurrentEquippedItem != null){
			if(Input.GetKeyDown(KeyCode.Mouse0) && CurrentEquippedItem.name == HackManager.HackingToolItem.name){
				Debug.Log("LMB Clicked");
				Debug.Log(CurrentEquippedItem.name + " Current item name | " + HackManager.HackingToolItem + " hacking tool item's name");
				HackManager.ShowHackWindow();
			}
		}
	}

	void ManageUpdateTick(){
		
		ManageSuit();
	}

	private void ManageSuit(){
		if(statHealth.currentValue > 0){
			healthDepleted = false;
		}
		if(statShields.currentValue > 0){
			shieldsDepleted = false;
		}
		if(statPower.currentValue > 0){
			powerDepleted = false;
		}

		if(statHealth.currentValue > statHealth.currentMaxValue){
			statHealth.SetCurrentValueRaw(statHealth.currentMaxValue);
		}
		if(statPower.currentValue > statPower.currentMaxValue){
			statPower.SetCurrentValueRaw(statPower.currentMaxValue);
		}
		if(statShields.currentValue > statShields.currentMaxValue){
			statShields.SetCurrentValueRaw(statShields.currentMaxValue);
		}

		if(HeatIndex > 0 || ColdIndex > 0 || RadiationIndex > 0 || ToxicIndex > 0 || OxygenIndex > 0){
			float hazardDMG = 0;
			hazardDMG = HazardDamage * -1;
			if(statPower.currentValue <= 0){
				statPower.SetCurrentValueRaw(0,true);
				powerDepleted = true;
			}
			if(statShields.currentValue <= 0){
				statShields.SetCurrentValueRaw(0,true);
				shieldsDepleted = true;
			}
			if(statHealth.currentValue <= 0){
				statHealth.SetCurrentValueRaw(0,true);
				healthDepleted = true;
			}
			if(healthDepleted){
				// you're dead
			} else if(shieldsDepleted){
				statHealth.ChangeCurrentValueRaw(hazardDMG, true);
			} else if(powerDepleted){
				statShields.ChangeCurrentValueRaw(hazardDMG,true);
			} else {
				statPower.ChangeCurrentValueRaw(hazardDMG,true);
			}
		} else {

			if(statShields.currentValueRaw < statShields.currentMaxValueRaw){
				if(statShields.currentValue <= 0){
					shieldsDepleted = false;
				}
				if(!shieldsDepleted){
					if(statShields.currentValueRaw < statShields.currentMaxValueRaw){
						statShields.ChangeCurrentValueRaw(1,true);
					}
				}
			} else if(statPower.currentValueRaw < statPower.currentMaxValueRaw){
				if(statPower.currentValue <= 0){
					powerDepleted = false;
				}
				if(!powerDepleted){
					if(statPower.currentValueRaw < statPower.currentMaxValueRaw){
						statPower.ChangeCurrentValueRaw(1,true);
					}
				}
			}

			if(statShields.currentValueRaw >= statShields.currentMaxValueRaw){
				statShields.SetCurrentValueRaw(statShields.currentMaxValueRaw,true);
			}
			if(statPower.currentValueRaw >= statPower.currentMaxValueRaw){
				statPower.SetCurrentValueRaw(statPower.currentMaxValueRaw, true);
			}
		}
	}

	private void ManageHazardDamage(){
		float heatdmg = 0f;
		float colddmg = 0f;
		float raddmg = 0f;
		float toxicdmg = 0f;
		float o2dmg = 0f;

		int hdmg = (int)CurrentHazardLevel;
		float hazardDMG = (float) hdmg;

		if(HeatIndex > 0){
			HeatHazard(true);
			float h = statHeatReduction.currentValueRaw * 0.01f;
			h = hazardDMG * h;
			heatdmg = hazardDMG - h;
			if(heatdmg < 0){
				heatdmg = 0;
			}
		} else {
			HeatHazard(false);
		}
		if(ColdIndex > 0){
			ColdHazard(true);
			float c = statColdReduction.currentValueRaw * 0.01f;
			c = hazardDMG * c;
			colddmg = hazardDMG - c;
			if(colddmg < 0){
				colddmg = 0;
			}
		} else {
			ColdHazard(false);
		}
		if(RadiationIndex > 0){
			RadiationHazard(true);
			float r = statRadiationReduction.currentValueRaw * 0.01f;
			r = hazardDMG * r;
			raddmg = hazardDMG - r;
			if(raddmg < 0){
				raddmg = 0;
			}
		} else {
			RadiationHazard(false);
		}
		if(ToxicIndex > 0){
			ToxicHazard(true);
			float t = statToxicReduction.currentValueRaw * 0.01f;
			t = hazardDMG * t;
			toxicdmg = hazardDMG - t;
			if(toxicdmg < 0){
				toxicdmg = 0;
			}
		} else {
			ToxicHazard(false);
		}

		if(OxygenIndex > 0){
			InHazard = true;
			DeoxygenationHazard(true);
			float o = statO2Reduction.currentValueRaw * 0.01f;
			o = hazardDMG * o;
			o2dmg = hazardDMG - o;
			if(o2dmg < 0){
				o2dmg = 0;
			}
		} else {
			DeoxygenationHazard(false);
		}

		if(hdmg == 0){
			InHazard = false;
			heatdmg = 0;
			colddmg = 0;
			raddmg = 0;
			toxicdmg = 0;
			o2dmg = 0;
			HeatHazard(false);
			ColdHazard(false);
			RadiationHazard(false);
			ToxicHazard(false);
			DeoxygenationHazard(false);
		}

		HazardDamage = heatdmg + colddmg + raddmg + toxicdmg + o2dmg;

	}

	void Hazard(IMessage hMessage){
		HazardMessage hm = (HazardMessage)hMessage;

		HazardCalculator(hm.HazardRating, hm.Hazard, hm.HazardIndex);
	}

	public void HazardCalculator(HazardLevel hl, HazardType ht, int Index){
		Debug.Log("HazardCalculator Called! - " + hl + " : " + ht + " : " + Index);
		CurrentHazardLevel = hl;
		switch(ht){
		case HazardType.Heat:
			HeatIndex += Index;
			break;
		case HazardType.Cold:
			ColdIndex += Index;
			break;
		case HazardType.Radiation:
			RadiationIndex += Index;
			break;
		case HazardType.Toxic:
			ToxicIndex += Index;
			break;
		case HazardType.Deoxygenation:
			OxygenIndex += Index;
			break;
		case HazardType.None:
			HeatIndex = 0;
			ColdIndex = 0;
			RadiationIndex = 0;
			ToxicIndex = 0;
			OxygenIndex = 0;
			break;
		}

		ManageHazardDamage();
	}

	private void OnCharacterTileChanged(DungenCharacter character, Tile previousTile, Tile newTile)
	{
		/*
		TileHazardInfo prevTile = previousTile.gameObject.GetComponent<TileHazardInfo>();
		if(prevTile != null){
			HazardCalculator(prevTile.hazardRating, prevTile.hazard, -1);
		}


		TileHazardInfo tileHazard = newTile.gameObject.GetComponent<TileHazardInfo>();
		HazardCalculator(tileHazard.hazardRating, tileHazard.hazard, 1);
		Debug.Log("OnCharacterTileChanged!");
		*/
		TileMapper tm = newTile.gameObject.GetComponent<TileMapper>();
		if(tm){
			tm.ShowMap();
		}
	}

	void Hazards(IMessage rMessage){
		/*
		string data = (string)rMessage.Data;

		switch(data){
		case "HeatOn":
			HeatIcon.SetActive(true);
			HeatScreenFX.SetActive(true);
			InHazard = true;
			HeatHazard();
			break;
		case "HeatOff":
			HeatIcon.SetActive(false);
			HeatScreenFX.SetActive(false);
			InHazard = false;
			break;
		case "ToxicOn":
			ToxicIcon.SetActive(true);
			ToxicScreenFX.SetActive(true);
			InHazard = true;
			ToxicHazard();
			break;
		case "ToxicOff":
			ToxicIcon.SetActive(false);
			ToxicScreenFX.SetActive(false);
			InHazard = false;
			break;
		case "RadiationOn":
			RadiationIcon.SetActive(true);
			RadiationScreenFX.SetActive(true);
			InHazard = true;
			RadiationHazard();
			break;
		case "RadiationOff":
			RadiationIcon.SetActive(false);
			RadiationScreenFX.SetActive(false);
			InHazard = false;
			break;
		case "ColdOn":
			ColdIcon.SetActive(true);
			ColdScreenFX.SetActive(true);
			InHazard = true;
			ColdHazard();
			break;
		case "ColdOff":
			ColdIcon.SetActive(false);
			ColdScreenFX.SetActive(false);
			InHazard = false;
			break;
		case "o2On":
			o2Icon.SetActive(true);
			o2ScreenFX.SetActive(true);
			InHazard = true;
			DeoxygenationHazard();
			break;
		case "o2Off":
			o2Icon.SetActive(false);
			o2ScreenFX.SetActive(false);
			InHazard = false;
			break;
		}
		*/
	}

	private void HeatHazard(bool v){
		HeatIcon.SetActive(v);
		HeatScreenFX.SetActive(v);
	}

	private void ToxicHazard(bool v){
		ToxicIcon.SetActive(v);
		ToxicScreenFX.SetActive(v);
	}

	private void RadiationHazard(bool v){
		RadiationIcon.SetActive(v);
		RadiationScreenFX.SetActive(v);
	}

	private void ColdHazard(bool v){
		ColdIcon.SetActive(v);
		ColdScreenFX.SetActive(v);
	}

	private void DeoxygenationHazard(bool v){
		o2Icon.SetActive(v);
		o2ScreenFX.SetActive(v);
	}

	public void DockSpaceStation(){
		//HeatDungeon.SetActive(false);
		SpaceStation.SetActive(true);
		if(GeneratedDungeon){
			Destroy(GeneratedDungeon);
			GeneratedDungeonInfo = null;
		}
		ShipCourseText.text = "Docked at Space Station";
	}

	public void GenerateRandomDungeon(){
		int randomHazardType = Random.Range(0,6);
		int randomHazardLevel = Random.Range(1,5);
		int randomShipQaulity = Random.Range(0,5);

		GeneratedDungeon = Instantiate(DungeonGenerator,DungeonStart.position, DungeonStart.rotation);
		GeneratedDungeonInfo = GeneratedDungeon.GetComponent<DungeonInfo>();
		GeneratedDungeonInfo.hazard = (HazardType)randomHazardType;
		//GeneratedDungeonInfo.hazard = HazardType.Heat;
		GeneratedDungeonInfo.hazardRating = (HazardLevel)randomShipQaulity; // Hazard level is worse the better the ship
		GeneratedDungeonInfo.ShipQuality = (Quality)randomShipQaulity;
		GeneratedDungeonInfo.init();
		CurrentHazardLevel = GeneratedDungeonInfo.hazardRating;


		RuntimeDungeon r = GeneratedDungeon.GetComponentInChildren<RuntimeDungeon>();
		r.Generator.OnGenerationStatusChanged += OnDungeonChanged;

		ControlRoomDoor.mode = dotHskDoorMode.blocked;
		ShipCourseText.text = "On Course for Ship";
	}

	public void DockHeatDungeon(){
		SpaceStation.SetActive(false);
		//HeatDungeon.SetActive(true);
		if(GeneratedDungeon){
			Destroy(GeneratedDungeon);
		}

		GenerateRandomDungeon();

		/*
		GeneratedDungeon = Instantiate(DungeonGenerator,DungeonStart.position, DungeonStart.rotation);
		GeneratedDungeonInfo = GeneratedDungeon.GetComponent<DungeonInfo>();
		GeneratedDungeonInfo.hazard = HazardType.Heat;
		GeneratedDungeonInfo.hazardRating = HazardLevel.Low;
		GeneratedDungeonInfo.ShipQuality = Quality.Standard;


		RuntimeDungeon r = GeneratedDungeon.GetComponentInChildren<RuntimeDungeon>();
		r.Generator.OnGenerationStatusChanged += OnDungeonChanged;

		ControlRoomDoor.mode = dotHskDoorMode.blocked;

		*/
	}

	void OnDungeonChanged(DungeonGenerator dg, GenerationStatus gs){
		Debug.Log(gs);
		if(gs == GenerationStatus.Complete){
			Debug.Log("Dungeon has completed generating!");
			ShipCourseText.text = "Docked with Ship";
			ControlRoomDoor.mode = dotHskDoorMode.active;

			dg.OnGenerationStatusChanged -= OnDungeonChanged;
			GeneratedDungeonInfo.GenerationComplete = true;

			var tiles = dg.CurrentDungeon.AllTiles;


			DungeonManager.SetupPrimaryRooms(tiles);
			DungeonManager.hazard = GeneratedDungeonInfo.hazard;
			DungeonManager.hazardRating = GeneratedDungeonInfo.hazardRating;
			DungeonManager.ScanInfo();

			int i = 0;
			foreach(Tile t in tiles){
				Debug.Log("Dungeon Tile : " + i);
				var rc = t.gameObject.GetComponent<RoomController>();
				if(rc != null){
					rc.OnShipCompleteFromList();
				}
				i++;
			}
			Debug.Log("Game Manager Finished going through all the tiles");


			MessageDungeonComplete();

		}
		if(gs == GenerationStatus.Failed){
			ShipCourseText.text = "Failed to Dock with Ship";
		}
	}

	private void MessageDungeonComplete(){
		Debug.Log("GameManager Message Dungeon Complete function called");
		MessageDispatcher.SendMessage(this, "ShipDungeonComplete", "Ship Finished Generating", 0);
	}

	public void RestoreSuitPowerToFull(){
		statPower.SetCurrentValueRaw(statPower.currentMaxValue);
	}

	public void RestoreSuitShieldsToFull(){
		statShields.SetCurrentValueRaw(statShields.currentMaxValue);
	}

	public void RestoreHealthToFull(){
		statHealth.SetCurrentValueRaw(statHealth.currentMaxValue);
	}

}
