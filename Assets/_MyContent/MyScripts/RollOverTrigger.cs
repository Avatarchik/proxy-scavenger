using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.InventoryPro.UI;
using Devdog.InventoryPro;
using Devdog.General;
using Devdog.General.UI;
using UnityEngine.UI;

[RequireComponent(typeof(UIWindow))]
public class RollOverTrigger : MonoBehaviour {

	private UIWindow _window;
	private TriggerBase currentTrigger;

	protected virtual void Awake()
	{
		_window = GetComponent<UIWindow>();
	}

	protected virtual void Start()
	{
		PlayerManager.instance.OnPlayerChanged += OnPlayerChanged;
		if (PlayerManager.instance.currentPlayer != null)
		{
			OnPlayerChanged(null, PlayerManager.instance.currentPlayer);
		}
	}
	
	private void OnPlayerChanged(Player oldPlayer, Player newPlayer)
	{
		if (oldPlayer != null)
		{
			oldPlayer.triggerHandler.OnSelectedTriggerChanged -= BestTriggerChanged;
		}

		newPlayer.triggerHandler.OnSelectedTriggerChanged += BestTriggerChanged;
		BestTriggerChanged(null, newPlayer.triggerHandler.selectedTrigger);
	}

	private void BestTriggerChanged(TriggerBase old, TriggerBase newBest)
	{
		if (newBest != null)
		{
			_window.Show();
			currentTrigger = newBest;
			/*
			Repaint(newBest);
			if (moveToTriggerLocation)
			{
				UpdatePosition(newBest);
			}
			*/
		}
		else
		{
			_window.Hide();
		}
	}

	public void UseTrigger(){
		currentTrigger.Use();
	}
}
