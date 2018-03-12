using System;
using System.Collections.Generic;
using Devdog.General;
using UnityEngine;
using Devdog.InventoryPro;
using Devdog.InventoryPro.UI;
using Devdog.General.UI;

public class MyItemTrigger : ItemTrigger {

	public bool handleWindowDirectly
	{
		get
		{
			if (windowContainer != null && windowContainer.Equals(null) == false)
			{
				return false;
			}

			return true;
		}
	}

	/// <summary>
	/// Only required if handling the window directly
	/// </summary>
	[Header("The window")]
	[SerializeField]
	private UIWindowField _window;
	public UIWindowField window
	{
		get { return _window; }
		set { _window = value; }
	}

	/// <summary>
	/// The window this trigger will use;
	/// If a ITriggerWindowContainer is present it will grab it's window, if not the UIWindowField (this.window) will be used.
	/// </summary>
	public UIWindow windowToUse
	{
		get
		{
			if (windowContainer != null)
				return windowContainer.window;

			return window.window;
		}
	}

	[Header("Animations & Audio")]
	public MotionInfo useAnimation;
	public MotionInfo unUseAnimation;

	public AudioClipInfo useAudioClip;
	public AudioClipInfo unUseAudioClip;

	protected Animator animator;
	protected ITriggerWindowContainer windowContainer;

	protected virtual void WindowOnHide()
	{
		DoUnUse(PlayerManager.instance.currentPlayer);
	}

	protected virtual void WindowOnShow()
	{

	}

	private void SubscribeToWindowEvents(UIWindow window)
	{
		if (window != null)
		{
			window.OnShow += WindowOnShow;
			window.OnHide += WindowOnHide;

			if (handleWindowDirectly)
			{
				window.Show();
			}
		}
	}

	private void UnSubscribeFromWindowEvents(UIWindow window)
	{
		if (window != null)
		{
			window.OnShow -= WindowOnShow;
			window.OnHide -= WindowOnHide;

			if (handleWindowDirectly)
			{
				window.Hide();
			}
		}
	}

	protected virtual void DoUnUse(Player player)
	{
		UnSubscribeFromWindowEvents(windowToUse);
		UndoVisuals();

		TriggerManager.currentActiveTrigger = null;
		NotifyTriggerUnUsed(player);
	}
}
