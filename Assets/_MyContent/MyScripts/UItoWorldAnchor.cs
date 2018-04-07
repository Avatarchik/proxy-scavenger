using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.General;
using Devdog.InventoryPro;
using Devdog.General.UI;
using Devdog.InventoryPro.UI;
using Sirenix.OdinInspector;

public class UItoWorldAnchor : MonoBehaviour {

	[BoxGroup("World Anchor")]
	public RectTransform targetCanvas;

	[BoxGroup("Follow Object")]
	public Transform objectToFollow;

	[BoxGroup("Transforms to Follow")]
	public Transform positionAToFollow;
	[BoxGroup("Transforms to Follow")]
	public Transform positionBToFollow;
	[BoxGroup("Transforms to Follow")]
	public Transform positionCToFollow;

	[BoxGroup("UI & UI Positions")]
	public RectTransform UI;
	[BoxGroup("UI & UI Positions")]
	public RectTransform PostionAUI;
	[BoxGroup("UI & UI Positions")]
	public RectTransform PositionBUI;
	[BoxGroup("UI & UI Positions")]
	public RectTransform PositionCUI;

	[BoxGroup("UI Slots")]
	public ItemCollectionSlotUI SlotA;
	[BoxGroup("UI Slots")]
	public ItemCollectionSlotUI SlotB;
	[BoxGroup("UI Slots")]
	public ItemCollectionSlotUI SlotC;

	[BoxGroup("World to 2D")]
	public Camera MainCam;
	[BoxGroup("World to 2D")]
	public Vector3 ScreenPoint;
	[BoxGroup("World to 2D")]
	private Vector2 positionCorrection = new Vector2(0, 0);


	// Update is called once per frame
	void Update () {
		if(positionAToFollow != null){
			RepositionUI(positionAToFollow, PostionAUI);
		}
		if(positionBToFollow != null){
			RepositionUI(positionBToFollow, PositionBUI);
		}
		if(positionCToFollow != null){
			RepositionUI(positionCToFollow, PositionCUI);
		}
	}

	private void RepositionUI(Transform objToFollow, RectTransform anchoredUI)
	{
		Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(objToFollow.position);
		Vector2 WorldObject_ScreenPosition = new Vector2(
			((ViewportPosition.x * targetCanvas.sizeDelta.x) - (targetCanvas.sizeDelta.x * 0.5f)),
			((ViewportPosition.y * targetCanvas.sizeDelta.y) - (targetCanvas.sizeDelta.y * 0.5f)));
		//now you can set the position of the ui element
		anchoredUI.anchoredPosition = WorldObject_ScreenPosition;
	}

	/*
	void Update () {
		if(wiresToFollow != null){
			FollowItem(wiresToFollow, WiresUI);
			FollowItem(powerToFollow, PowerUI);
			FollowItem(coolantToFollow, CoolantUI);
		}
	}

	public void FollowItem(Transform FollowObject, GameObject UIElement){
		//Vector3 pos = MainCam.WorldToScreenPoint(FollowObject.transform.position);
		if(!MainCam){
			MainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		}
		Vector3 screenP = MainCam.WorldToScreenPoint(FollowObject.position);
		UIElement.transform.position = screenP;
	}
	*/
}
