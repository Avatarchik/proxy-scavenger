using System;
using System.Collections.Generic;
using Devdog.General;
using Devdog.General.Localization;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.General.UI;
using Devdog.QuestSystemPro;
using Devdog.QuestSystemPro.UI;
using Devdog.QuestSystemPro.Dialogue;
using Devdog.QuestSystemPro.Dialogue.UI;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.Serialization;

[System.Serializable]
public class PlayerDecisionAndItemNodeUI : DefaultNodeUI {
	
	public PlayerDecisionAndItemUI playerDecisionAndItemUIPrefab;

	[NonSerialized]
	protected List<PlayerDecisionAndItemUI> myDecisions = new List<PlayerDecisionAndItemUI>();

	private PlayerDecisionAndItem[] _decisions;
	protected override void SetDecisions()
	{
		Debug.Log("Set Decisions is at aleast called");
		var decisionNode = (IPlayerDecisionAndItemNode)currentNode;
		_decisions = decisionNode.playerDecisions;

		Debug.Log(decisionNode.playerDecisions.Length + " Decision node length");

		for (int i = 0; i < decisionNode.playerDecisions.Length; i++)
		{
			var decision = decisionNode.playerDecisions[i];
			if (i >= currentNode.edges.Length)
			{
				continue;
			}

			if (currentNode.edges[i].CanViewEndNode(currentNode.owner) == false)
			{
				continue;
			}
			Debug.Log("going to instantiate thing");
			var playerDecisionInst = Instantiate<PlayerDecisionAndItemUI>(playerDecisionAndItemUIPrefab);
			playerDecisionInst.transform.SetParent(playerDecisionsContainer);
			UIUtility.ResetTransform(playerDecisionInst.transform);

			playerDecisionInst.Repaint(decision, currentNode.edges[i], currentNode.edges[i].CanUse(currentNode.owner));

			var tempIndex = i;
			playerDecisionInst.button.onClick.AddListener(() =>
				{
					OnPlayerDecisionClicked(tempIndex);
				});

			myDecisions.Add(playerDecisionInst);
		}

		// No defined decisions, set default
		if (decisionNode.playerDecisions.Length == 0)
		{
			Debug.Log("This is where we set default decisions");
			//SetDefaultPlayerDecision();
		}
	}

	protected virtual void OnPlayerDecisionClicked(int decisionIndex)
	{
		var decisionNode = (IPlayerDecisionAndItemNode)currentNode;
		if (ReferenceEquals(_decisions, decisionNode.playerDecisions) == false)
		{
			DevdogLogger.Log("Player decisions and items changed during repaint and user click - Forcing repaint.");
			Repaint(currentNode); // Force repaint
			return;
		}

		decisionNode.SetPlayerDecisionAndMoveToNextNode(decisionIndex);
	}

	public override void Repaint (NodeBase node)
	{
		myDecisions.Clear();
		currentNode = node;
		RemoveOldDecisions();

		if (gameObject.activeInHierarchy == false)
		{
			return;
		}

		SetText(variablesStringFormatter.Format(currentNode.message, currentNode.owner.variables));
		SetMyDecisions();
		if (navigationHandler != null)
		{
			navigationHandler.HandleNavigation(myDecisions.Select(o => o.button).Cast<Selectable>().ToArray());
		}

		AudioManager.AudioPlayOneShot(onShowAudio);
		animator.Play(onShowAnimation);
	}

	protected virtual void SetMyDecisions(){
		SetDefaultPlayerAndItemDecision();
	}

	protected virtual void SetDefaultPlayerAndItemDecision(){
		
		if (playerDecisionsContainer != null)
		{
			Debug.Log("playerDecisionsContainer != null and now default stuff");
			/*
			var playerDecisionInst = UnityEngine.Object.Instantiate<PlayerDecisionAndItemUI>(playerDecisionAndItemUIPrefab);
			playerDecisionInst.transform.SetParent(playerDecisionsContainer);
			playerDecisionInst.transform.ResetTRSRect();

			playerDecisionInst.Repaint(new PlayerDecisionAndItem() { option = moveToNextNodeText }, null, currentNode.edges.Length == 0 || currentNode.edges.Any(o => o.CanUse(currentNode.owner)));
			playerDecisionInst.button.onClick.AddListener(OnDefaultPlayerDecisionAndItemClicked);

			myDecisions.Add(playerDecisionInst);
			*/
			SetDecisions();
		}
	}

	protected virtual void OnDefaultPlayerDecisionAndItemClicked()
	{
		currentNode.Finish(true);
	}
}
