using UnityEngine;
using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.InventoryPro;
using Devdog.QuestSystemPro;
using Devdog.QuestSystemPro.Dialogue;


public class ShipDungeonRecoveryEdge : SimpleEdgeCondition {

	[Required]
	public ShipRecovery IfShipRecovery;

	private GameManager gm;
	private DungeonInfo dinfo;

	public override bool CanUse(Dialogue dialogue)
	{
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		dinfo = gm.GeneratedDungeonInfo;
		if(dinfo.CurrenShipRecovery == IfShipRecovery){
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

		switch(IfShipRecovery){
		case ShipRecovery.Unrecovered:
			r = "Ship is Unrecovered";
			break;
		case ShipRecovery.Recovered:
			r = "Ship Recovered";
			break;
		}
			
		return r;
	}
}
