using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairBoardCover : MonoBehaviour {

	public GameObject CoverProp;
	public GameObject Cover;
	public GameObject CoverPropSpawnPoint;
	public DrillDamage BoltA;
	public DrillDamage BoltB;
	public DrillDamage BoltC;
	public DrillDamage BoltD;
	public bool coverSpawned = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(BoltA.CurrentHP == 0 &&
			BoltB.CurrentHP == 0 &&
			BoltC.CurrentHP == 0 &&
			BoltD.CurrentHP == 0 &&
			coverSpawned == false){

			coverSpawned = true;
			HideAndSpawnCover();
		}
	}

	public void HideAndSpawnCover(){
		Cover.SetActive(false);
		GameObject c = GameObject.Instantiate(CoverProp, CoverPropSpawnPoint.transform.position, CoverPropSpawnPoint.transform.rotation);
	}
}
