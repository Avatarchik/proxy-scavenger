using UnityEngine;
using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.InventoryPro;
using Devdog.QuestSystemPro;
using Devdog.QuestSystemPro.Dialogue;


public class ShipDungeonSalvageEdge : SimpleEdgeCondition {

	[Required]
	public ShipSalvage IfShipSalvageable;

	private GameManager gm;
	private DungeonInfo dinfo;

	public override bool CanUse(Dialogue dialogue)
	{
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		dinfo = gm.GeneratedDungeonInfo;
		if(dinfo.CurrentShipSalvage == IfShipSalvageable){
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

		switch(IfShipSalvageable){
		case ShipSalvage.Salvageable:
			r = "Ship is Salvageable";
			break;
		case ShipSalvage.Unsalvageable:
			r = "Ship is Unsalvageable";
			break;
		}
			
		return r;
	}
}
