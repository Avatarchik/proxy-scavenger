using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHazardInfo : MonoBehaviour {

	public HazardType hazard = HazardType.None;
	public HazardLevel hazardRating = HazardLevel.None;

	void Awake () {
		GameObject g = GameObject.FindGameObjectWithTag("DungeonHead");
		DungeonInfo d = g.GetComponent<DungeonInfo>();
		hazard = d.hazard;
		hazardRating = d.hazardRating;
	}
}
