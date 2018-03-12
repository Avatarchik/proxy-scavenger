using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using com.ootii.Messages;

public enum ShipPartState {
	Broken = 0,
	Fixed = 1,
	Removed = 2,
	Destroyed = 3
}

public enum ShipPart {
	Engine = 0,
	PowerCore = 1,
	LifeSupport = 2,
	WarpDrive = 3,
	ThermalShielding = 4,
	Controls = 5
}

public enum ShipState {
	Broken = 0,
	Fixed = 1,
	Unrecoverable = 2,
	Recovered = 3
}

[System.Serializable]
public class DungeonInfo : MonoBehaviour {

	[BoxGroup("Ship Quality")]
	public Quality ShipQuality = Quality.Basic;

	[BoxGroup("Ship Hazard")]
	public HazardType hazard = HazardType.None;
	[BoxGroup("Ship Hazard")]
	public HazardLevel hazardRating = HazardLevel.None;

	[BoxGroup("Ship State")]
	public ShipState CurrentShipState = ShipState.Broken;
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

		Engine.init(ShipPart.Engine, ShipPartState.Fixed);
		Power.init(ShipPart.PowerCore, ShipPartState.Fixed);
		LifeSupport.init(ShipPart.LifeSupport, ShipPartState.Fixed);
		Warp.init(ShipPart.WarpDrive, ShipPartState.Fixed);
		Thermal.init(ShipPart.ThermalShielding, ShipPartState.Fixed);
		Controls.init(ShipPart.Controls, ShipPartState.Fixed);

		BreakHazardComponent();
	}

	private void BreakHazardComponent(){
		switch(hazard){
		case HazardType.Heat:
			Engine.ShipComponentState = ShipPartState.Broken;
			break;
		case HazardType.Cold:
			Thermal.ShipComponentState = ShipPartState.Broken;
			break;
		case HazardType.Radiation:
			Power.ShipComponentState = ShipPartState.Broken;
			break;
		case HazardType.Toxic:
			Warp.ShipComponentState = ShipPartState.Broken;
			break;
		case HazardType.Deoxygenation:
			LifeSupport.ShipComponentState = ShipPartState.Broken;
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
				CurrentShipState = ShipState.Broken;
				//BreakShip();
			}

			if(Engine.ShipComponentState == ShipPartState.Fixed && 
				Power.ShipComponentState == ShipPartState.Fixed && 
				Controls.ShipComponentState == ShipPartState.Fixed && 
				LifeSupport.ShipComponentState == ShipPartState.Fixed && 
				Warp.ShipComponentState == ShipPartState.Fixed && 
				Thermal.ShipComponentState == ShipPartState.Fixed)
			{
				CurrentShipSalvage = ShipSalvage.Salvageable;
				CurrentShipState = ShipState.Fixed;
				//BreakShip();
			}
		}
	}

	public void BreakShip(){
		if(Engine.ShipComponentState != ShipPartState.Removed){
			Engine.ShipComponentState = ShipPartState.Broken;
		}
		if(Power.ShipComponentState != ShipPartState.Removed){
			Power.ShipComponentState = ShipPartState.Broken;
		}
		if(Thermal.ShipComponentState != ShipPartState.Removed){
			Thermal.ShipComponentState = ShipPartState.Broken;
		}
		if(Warp.ShipComponentState != ShipPartState.Removed){
			Warp.ShipComponentState = ShipPartState.Broken;
		}
		if(LifeSupport.ShipComponentState != ShipPartState.Removed){
			LifeSupport.ShipComponentState = ShipPartState.Broken;
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
			s = ShipPartState.Broken;
			Debug.Log("Dungeon Info : GetShipPartState - Returning Default of Broken");
			break;
		}

		return s;
	}

	public void SetShipState(ShipState s){
		if(s == ShipState.Fixed){
			MessageDispatcher.SendMessage(this, "ShipFixed", "Ship is now Fixed", 0);
		}
		CurrentShipState = s;
	}
		
}
