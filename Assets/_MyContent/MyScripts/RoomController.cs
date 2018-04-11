using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public class RoomController : MonoBehaviour {

	public GameObject room;
	public bool active = true;
	public bool hideAtStart = true;
	public float secondsUntilHide = 5.0f;
	public bool cull = true;

	public void Start(){

		MessageDispatcher.AddListener("ShipDungeonComplete", OnShipComplete);
		MessageDispatcher.AddListener("Ship Finished Generating", OnShipCompleted);

		if(hideAtStart){
			StartCoroutine(WaitToHide());
		}
	}

	public void OnShipCompleted(IMessage rMessage){
		Debug.Log(" Room Got the Message that the dungeon was completed(completed)!");
		if(!cull){
			ShowRoom();
		}
	}

	public void OnShipComplete(IMessage rMessage){
		Debug.Log( "Room Got the Message that the dungeon was complete(complete)!");
		if(!cull){
			ShowRoom();
		}
	}

	public void OnShipCompleteFromList(){
		Debug.Log(this.gameObject.name + " : Got the message that the dungeon was complete from the forloop");
	}

	public void ShowRoom(){
		if(room != null){
			room.SetActive(true);
		}
		active = true;
		Debug.Log("ShowRoom Called");
	}

	public void HideRoom(){
		room.SetActive(false);
		active = false;
	}

	void OnTriggerEnter(Collider other) {
		if(cull){
			Debug.Log(other.tag + " This was seen entering");
			if(other.tag == "RoomTrigger"){
				ShowRoom();
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(cull){
			Debug.Log(other.tag + " This was seen leaving");
			if(other.tag == "RoomTrigger"){
				MessageDispatcher.SendMessage(this, "HidingRoom", room, 0);
				HideRoom();
			}
		}
	}

	IEnumerator WaitToHide()
	{
		
		yield return new WaitForSeconds(secondsUntilHide);
		HideRoom();
	}
}
