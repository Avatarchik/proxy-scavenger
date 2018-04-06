using UnityEngine;
using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.InventoryPro;
using Devdog.QuestSystemPro;
using Devdog.QuestSystemPro.Dialogue;
using mindler.dungeonship;


public class ShipDungeonStateEdge : SimpleEdgeCondition {

	[Required]
	public ShipState IfShipState;

	private GameManager gm;
	private DungeonInfo dinfo;

	public override bool CanUse(Dialogue dialogue)
	{
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		dinfo = gm.GeneratedDungeonInfo;
		if(dinfo.CurrentShipState == IfShipState){
			return true;
		} else {
			return false;
		}
	}

	public override string FormattedString()
	{
		/*
		if(IfShipState == null)
		{
			return "(No ship state set)";
		}

		if (dinfo.CurrentShipState == IfShipState)
		{
			return "Ship is " + dinfo.CurrentShipState;
		}
		*/
		string r = "";

		switch(IfShipState){
		case ShipState.Damaged:
			r = "Ship State is Broken";
			break;
		case ShipState.Active:
			r = "Ship State is Fixed";
			break;
		case ShipState.Recovered:
			r = "Ship State is Recovered";
			break;
		case ShipState.Unrecoverable:
			r = "Ship State is Unrecoverable";
			break;
		}
			
		return r;
	}
}
