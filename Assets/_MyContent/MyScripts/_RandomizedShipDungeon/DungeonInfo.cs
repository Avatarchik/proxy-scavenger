using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using mindler.dungeonship;
using com.ootii.Messages;



[System.Serializable]
public class DungeonInfo : MonoBehaviour {

	[BoxGroup("Ship Quality")]
	public Quality ShipQuality = Quality.Basic;

	[BoxGroup("Ship Hazard")]
	public HazardType hazard = HazardType.None;
	[BoxGroup("Ship Hazard")]
	public HazardLevel hazardRating = HazardLevel.None;

	[BoxGroup("Ship State")]
	public ShipState CurrentShipState = ShipState.Damaged;
	[BoxGroup("Ship State")]
	public ShipRecovery CurrenShipRecovery = ShipRecovery.Unrecovered;
	[BoxGroup("Ship State")]
	public ShipSalvage CurrentShipSalvage = ShipSalvage.Salvageable;

	//[BoxGroup("Ship Parts")]

	[BoxGroup("Ship Parts")]
	[SerializeField]
	public ShipPieceState Engine = new ShipPieceState();
	[BoxGroup("Ship Parts")]
	[SerializeField]
	public ShipPieceState Power = new ShipPieceState();
	[BoxGroup("Ship Parts")]
	public ShipPieceState LifeSupport = new ShipPieceState();
	[BoxGroup("Ship Parts")]
	public ShipPieceState Warp = new ShipPieceState();
	[BoxGroup("Ship Parts")]
	public ShipPieceState Thermal = new ShipPieceState();
	[BoxGroup("Ship Parts")]
	public ShipPieceState Controls = new ShipPieceState();

	public bool GenerationComplete = false;

	/*
	public bool Unrecoverable = false;
	public bool ShipRecovered = false;
	public bool ShipFixed = false;
	public bool ShipBroken = false;

	public bool EngineFixed = false;
	public bool EngineTaken = false;
	public bool PowerFixed = false;
	public bool PowerTaken = false;
	public bool ControlsFixed = false;
	public bool ControlsTaken = false;
	public bool O2Fixed = false;
	public bool O2Taken = false;
	public bool WarpFixed = false;
	public bool WarpTaken = false;
	*/

	public void Awake(){
		//init();
	}

	public void init(){
		Engine = new ShipPieceState();
		Power = new ShipPieceState();
		LifeSupport = new ShipPieceState();
		Warp = new ShipPieceState();
		Thermal = new ShipPieceState();
		Controls = new ShipPieceState();

		Engine.init(ShipPart.Engine, ShipPartState.Active);
		Power.init(ShipPart.PowerCore, ShipPartState.Active);
		LifeSupport.init(ShipPart.LifeSupport, ShipPartState.Active);
		Warp.init(ShipPart.WarpDrive, ShipPartState.Active);
		Thermal.init(ShipPart.ThermalShielding, ShipPartState.Active);
		Controls.init(ShipPart.Controls, ShipPartState.Active);

		BreakHazardComponent();
	}

	private void BreakHazardComponent(){
		switch(hazard){
		case HazardType.Heat:
			Engine.ShipComponentState = ShipPartState.Damaged;
			break;
		case HazardType.Cold:
			Thermal.ShipComponentState = ShipPartState.Damaged;
			break;
		case HazardType.Radiation:
			Power.ShipComponentState = ShipPartState.Damaged;
			break;
		case HazardType.Toxic:
			Warp.ShipComponentState = ShipPartState.Damaged;
			break;
		case HazardType.Deoxygenation:
			LifeSupport.ShipComponentState = ShipPartState.Damaged;
			break;
		case HazardType.None:
			// No Ship Parts are broken
			break;
		}
	}

	public void Update () {
		//DungeonState();
		ManageShipDungeon();
	}

	private void ManageShipDungeon(){
		if(CurrenShipRecovery == ShipRecovery.Unrecovered){
			
			if(Engine.ShipComponentState == ShipPartState.Removed || 
				Power.ShipComponentState == ShipPartState.Removed || 
				Controls.ShipComponentState == ShipPartState.Removed || 
				LifeSupport.ShipComponentState == ShipPartState.Removed || 
				Warp.ShipComponentState == ShipPartState.Removed || 
				Thermal.ShipComponentState == ShipPartState.Removed)
			{
				CurrentShipSalvage = ShipSalvage.Unsalvageable;
				CurrentShipState = ShipState.Damaged;
				//BreakShip();
			}

			if(Engine.ShipComponentState == ShipPartState.Active && 
				Power.ShipComponentState == ShipPartState.Active && 
				Controls.ShipComponentState == ShipPartState.Active && 
				LifeSupport.ShipComponentState == ShipPartState.Active && 
				Warp.ShipComponentState == ShipPartState.Active && 
				Thermal.ShipComponentState == ShipPartState.Active)
			{
				CurrentShipSalvage = ShipSalvage.Salvageable;
				CurrentShipState = ShipState.Active;
				//BreakShip();
			}
		}
	}

	public void BreakShip(){
		if(Engine.ShipComponentState != ShipPartState.Removed){
			Engine.ShipComponentState = ShipPartState.Damaged;
		}
		if(Power.ShipComponentState != ShipPartState.Removed){
			Power.ShipComponentState = ShipPartState.Damaged;
		}
		if(Thermal.ShipComponentState != ShipPartState.Removed){
			Thermal.ShipComponentState = ShipPartState.Damaged;
		}
		if(Warp.ShipComponentState != ShipPartState.Removed){
			Warp.ShipComponentState = ShipPartState.Damaged;
		}
		if(LifeSupport.ShipComponentState != ShipPartState.Removed){
			LifeSupport.ShipComponentState = ShipPartState.Damaged;
		}
	}

	public void SetShipPartState(ShipPart p, ShipPartState s){
		switch(p){
		case ShipPart.Engine:
			Engine.ShipComponentState = s;
			break;
		case ShipPart.PowerCore:
			Power.ShipComponentState = s;
			break;
		case ShipPart.WarpDrive:
			Warp.ShipComponentState = s;
			break;
		case ShipPart.LifeSupport:
			LifeSupport.ShipComponentState = s;
			break;
		case ShipPart.ThermalShielding:
			Thermal.ShipComponentState = s;
			break;
		case ShipPart.Controls:
			Controls.ShipComponentState = s;
			break;
		}
	}

	public ShipPartState GetShipPartState(ShipPart p){
		ShipPartState s;

		switch(p){
		case ShipPart.Engine:
			s = Engine.ShipComponentState;
			break;
		case ShipPart.PowerCore:
			s = Power.ShipComponentState;
			break;
		case ShipPart.WarpDrive:
			s = Warp.ShipComponentState;
			break;
		case ShipPart.LifeSupport:
			s = LifeSupport.ShipComponentState;
			break;
		case ShipPart.ThermalShielding:
			s = Thermal.ShipComponentState;
			break;
		case ShipPart.Controls:
			s = Controls.ShipComponentState;
			break;
		default:
			s = ShipPartState.Damaged;
			Debug.Log("Dungeon Info : GetShipPartState - Returning Default of Broken");
			break;
		}

		return s;
	}

	public void SetShipState(ShipState s){
		if(s == ShipState.Active){
			MessageDispatcher.SendMessage(this, "ShipFixed", "Ship is now Fixed", 0);
		}
		CurrentShipState = s;
	}
		
}
