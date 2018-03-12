using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public class TileHazardZone : MonoBehaviour {

	public HazardType hazard = HazardType.None;
	public HazardLevel hazardRating = HazardLevel.None;
	public bool ManualHazard = false;

	public void Awake()
	{
		if(!ManualHazard){
			GameObject g = GameObject.FindGameObjectWithTag("DungeonHead");
			DungeonInfo d = g.GetComponent<DungeonInfo>();
			hazard = d.hazard;
			hazardRating = d.hazardRating;
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Player"){
			HazardMessage lMessage = HazardMessage.Allocate();
			lMessage.Type = "Hazards";
			lMessage.Hazard = hazard;
			lMessage.HazardRating = hazardRating;
			lMessage.HazardIndex = 1;
			lMessage.Sender = this;
			lMessage.Data = "HazardOn";
			lMessage.Delay = 0f;
			MessageDispatcher.SendMessage(lMessage);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player"){
			HazardMessage lMessage = HazardMessage.Allocate();
			lMessage.Type = "Hazards";
			lMessage.Hazard = hazard;
			lMessage.HazardRating = hazardRating;
			lMessage.HazardIndex = -1;
			lMessage.Sender = this;
			lMessage.Data = "HazardOff";
			lMessage.Delay = 0f;
			MessageDispatcher.SendMessage(lMessage);
		}
	}
}
