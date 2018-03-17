using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UItoWorldAnchor : MonoBehaviour {

	public RectTransform targetCanvas;
	public Transform objectToFollow;
	public Transform wiresToFollow;
	public Transform powerToFollow;
	public Transform coolantToFollow;

	public RectTransform UI;
	public RectTransform WiresUI;
	public RectTransform PowerUI;
	public RectTransform CoolantUI;

	public Camera MainCam;
	public Vector3 ScreenPoint;

	private Vector2 positionCorrection = new Vector2(0, 0);


	// Update is called once per frame
	void Update () {
		RepositionUI(wiresToFollow, WiresUI);
		RepositionUI(powerToFollow, PowerUI);
		RepositionUI(coolantToFollow, CoolantUI);
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
