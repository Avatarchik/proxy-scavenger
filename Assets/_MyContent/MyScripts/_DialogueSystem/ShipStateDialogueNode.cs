using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.QuestSystemPro;
using Devdog.QuestSystemPro.Dialogue;

[System.Serializable]
//[Category("MyProject")]
public class ShipStateDialogueNode : ActionNodeBase {

	[ShowInNode]
	public int myInt = 3;

	public override void OnExecute(IDialogueOwner dialogueOwner​)
	{
		// Called when the node is executed.
		Finish(false); // Call finish when the node is finished, otherwise the dialogue won't continue to the next node.
	}

	public override NodeBase GetNextNode()
	{
		// Return the next node (using the edge we wish to use).
		return base.GetNextNode();
	}
	/*
	public override void OnExit()
	{
		// Called when the node is exited (useful for resource cleanup).
	}



	public override ValidationInfo Validate()
	{
		// Validate the state of the node.
		return new ValidationInfo(ValidationType.Error, "Wah! Something is wrong.");
	}
	*/
}
