using UnityEngine;
using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.InventoryPro;
using Devdog.QuestSystemPro;
using Devdog.QuestSystemPro.Dialogue;


public class ShipDungeonQualityEdge : SimpleEdgeCondition {

	[Required]
	public Quality IfShipQuality;

	private GameManager gm;
	private DungeonInfo dinfo;

	public override bool CanUse(Dialogue dialogue)
	{
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		dinfo = gm.GeneratedDungeonInfo;
		if(dinfo.ShipQuality == IfShipQuality){
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
		return "Ship Quality - I don't know";
	}
}
