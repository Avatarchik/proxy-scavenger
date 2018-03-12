using UnityEngine;
using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.InventoryPro;
using Devdog.QuestSystemPro;
using Devdog.QuestSystemPro.Dialogue;


public class ShipDungeonHazardEdge : SimpleEdgeCondition {

	[Required]
	public HazardType IfShipHazardType;

	private GameManager gm;
	private DungeonInfo dinfo;

	public override bool CanUse(Dialogue dialogue)
	{
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		dinfo = gm.GeneratedDungeonInfo;
		if(dinfo.hazard == IfShipHazardType){
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

		switch(IfShipHazardType){
		case HazardType.Heat:
			r = "If Ship Hazard is Heat";
			break;
		case HazardType.Cold:
			r = "If Ship Hazard is Cold";
			break;
		case HazardType.Radiation:
			r = "If Ship Hazard is Radiation";
			break;
		case HazardType.Toxic:
			r = "If Ship Hazard is Toxic";
			break;
		case HazardType.Deoxygenation:
			r = "If Ship Hazard is Deoxygenation";
			break;
		case HazardType.None:
			r = "If Ship has no Hazard";
			break;
		}
			
		return r;
	}
}
