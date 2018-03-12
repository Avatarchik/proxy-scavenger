using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.QuestSystemPro.Dialogue;
using Devdog.InventoryPro;

[System.Serializable]
[Category("MyProject")]
public class GiveCurrencyNode : ActionNodeBase {

	[ShowInNode]
	public CurrencyDecorator Currency;

	public override void OnExecute(IDialogueOwner dialogueOwner​)
	{
		InventoryManager.AddCurrency(Currency, 1f);
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
