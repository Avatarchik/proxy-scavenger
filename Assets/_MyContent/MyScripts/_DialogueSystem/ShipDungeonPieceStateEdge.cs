using UnityEngine;
using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.InventoryPro;
using Devdog.QuestSystemPro;
using Devdog.QuestSystemPro.Dialogue;

public class ShipDungeonPieceStateEdge : SimpleEdgeCondition {

	[Required]
	public ShipPart IfShipPart;

	[Required]
	public ShipPartState IfShipPartState;

	private GameManager gm;
	private DungeonInfo dinfo;

	public override bool CanUse(Dialogue dialogue)
	{
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		dinfo = gm.GeneratedDungeonInfo;
		ShipPartState s = dinfo.GetShipPartState(IfShipPart);
		if(IfShipPartState == s){
			return true;
		} else {
			return false;
		}
	}

	public override string FormattedString()
	{
		string s = "";
		switch(IfShipPart){
		case ShipPart.Controls:
			s = "Controls ";
			break;
		case ShipPart.Engine:
			s = "Engine ";
			break;
		case ShipPart.LifeSupport:
			s = "Life Support ";
			break;
		case ShipPart.PowerCore:
			s = "Power Core ";
			break;
		case ShipPart.ThermalShielding:
			s = "Thermal Shielding ";
			break;
		case ShipPart.WarpDrive:
			s = "Warp Drive ";
			break;
		}

		string r = "";

		switch(IfShipPartState){
		case ShipPartState.Broken:
			r = "is Broken";
			break;
		case ShipPartState.Fixed:
			r = "is Fixed";
			break;
		case ShipPartState.Removed:
			r = "is Removed";
			break;
		case ShipPartState.Destroyed:
			r = "is Destroyed";
			break;
		}

		string rr = s + r;

		return rr;
	}
}
