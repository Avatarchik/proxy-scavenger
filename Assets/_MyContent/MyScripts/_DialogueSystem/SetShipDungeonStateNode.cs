using UnityEngine;
using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.InventoryPro;
using Devdog.QuestSystemPro;
using Devdog.QuestSystemPro.Dialogue;
using mindler.dungeonship;

[System.Serializable]
[Category("MyProject")]
public class SetShipDungeonStateNode : ActionNodeBase {

	[ShowInNode]
	public ShipState SetShipState;

	private GameManager gm;
	private DungeonInfo dinfo;

	public override void OnExecute(IDialogueOwner dialogueOwner​)
	{
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		dinfo = gm.GeneratedDungeonInfo;

		dinfo.SetShipState(SetShipState);

		// Called when the node is executed.
		Finish(true); // Call finish when the node is finished, otherwise the dialogue won't continue to the next node.
	}

	public override NodeBase GetNextNode()
	{
		// Return the next node (using the edge we wish to use).
		return base.GetNextNode();
	}

	public override void OnExit()
	{
		// Called when the node is exited (useful for resource cleanup).
	}
}
