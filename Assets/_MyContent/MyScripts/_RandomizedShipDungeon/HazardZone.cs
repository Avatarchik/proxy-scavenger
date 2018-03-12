using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public class HazardZone : MonoBehaviour {
	public HazardType hazard = HazardType.None;
	public HazardLevel hazardRating = HazardLevel.None;


	void OnTriggerEnter(Collider other) {
		
		if(other.tag == "Player"){
			switch(hazard){
			case HazardType.Heat:
				MessageDispatcher.SendMessage(this, "Hazard", "HeatOn", 0);
				break;
			case HazardType.Toxic:
				MessageDispatcher.SendMessage(this, "Hazard", "ToxicOn", 0);
				break;
			case HazardType.Radiation:
				MessageDispatcher.SendMessage(this, "Hazard", "RadiationOn", 0);
				break;
			case HazardType.Cold:
				MessageDispatcher.SendMessage(this, "Hazard", "ColdOn", 0);
				break;
			case HazardType.Deoxygenation:
				MessageDispatcher.SendMessage(this, "Hazard", "o2On", 0);
				break;
				default:
				break;
			}

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
			switch(hazard){
			case HazardType.Heat:
				MessageDispatcher.SendMessage(this, "Hazard", "HeatOff", 0);
				break;
			case HazardType.Toxic:
				MessageDispatcher.SendMessage(this, "Hazard", "ToxicOff", 0);
				break;
			case HazardType.Radiation:
				MessageDispatcher.SendMessage(this, "Hazard", "RadiationOff", 0);
				break;
			case HazardType.Cold:
				MessageDispatcher.SendMessage(this, "Hazard", "ColdOff", 0);
				break;
			case HazardType.Deoxygenation:
				MessageDispatcher.SendMessage(this, "Hazard", "o2Off", 0);
				break;
			default:
				break;
			}

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
