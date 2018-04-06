using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace mindler.dungeonship 
{
	public class PrimaryRoomInfo : MonoBehaviour {

		public string ShipPartName = "";
		[SerializeField]
		public ShipPart ShipPartRoom = ShipPart.Controls;
		[SerializeField]
		public ShipPartState PartState = ShipPartState.Active;

		public bool TerminalActive = true;
		public bool ShipPartDamaged = false;

		public DungeonShipManager DSM;

		public RoomTerminalController TerminalController;

		public GameObject RepairBoxParent;

		public GameObject RepairBoxObject;

		[SerializeField]
		public GameObject[] RepairBoxes = new GameObject[3];
		[SerializeField]
		public RepairableObject[] RepairBoards = new RepairableObject[3];
		[SerializeField]
		public GameObject[] RepairBoxAnchors = new GameObject[3];

		// Use this for initialization
		void Start () {
			if(DSM == null){
				DSM = GameObject.FindGameObjectWithTag("GameManager").GetComponentInChildren<DungeonShipManager>();
			}

			DSM.SetPrimaryRoom(ShipPartRoom, this);

			if(ShipPartName == ""){
				SetName();
			}
			CreateRepairBoxes();
			CheckPartState();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void CreateRepairBoxes(){
			int i = 0;
			foreach(GameObject a in RepairBoxAnchors){
				GameObject g = GameObject.Instantiate(RepairBoxObject,RepairBoxAnchors[i].transform.position, RepairBoxAnchors[i].transform.rotation);
				RepairBoxes[i] = g;
				RepairBoxes[i].transform.parent = RepairBoxParent.transform;
				RepairableObject r = g.GetComponentInChildren<RepairableObject>();
				RepairBoards[i] = r;
				i++;
			}
		}

		public void OpenWindow(){
			Debug.Log("Opening Window from PRI");
			CheckPartState();
			DSM.OpenTerminalWindow(ShipPartName, TerminalActive, ShipPartDamaged);
		}

		public void HideWindow(){
			DSM.HideTerminalWindow();
		}

		public void CheckPartState(){
			switch(PartState){
			case ShipPartState.Active:
				ShipPartDamaged = false;
				break;
			case ShipPartState.Damaged:
				ShipPartDamaged = true;
				break;
			case ShipPartState.Destroyed:
				ShipPartDamaged = true;
				break;
			case ShipPartState.Removed:
				ShipPartDamaged = true;
				break;
			}
		}

		public void SetPartState(ShipPartState state){
			PartState = state;
		}

		private void SetName(){
			switch(ShipPartRoom){
			case ShipPart.Controls:
				ShipPartName = "CONTROLS";
				break;
			case ShipPart.Engine:
				ShipPartName = "ENGINE";
				break;
			case ShipPart.LifeSupport:
				ShipPartName = "LIFE SUPPORT";
				break;
			case ShipPart.PowerCore:
				ShipPartName = "POWER CORE";
				break;
			case ShipPart.ThermalShielding:
				ShipPartName = "THERMAL SHIELDING";
				break;
			case ShipPart.WarpDrive:
				ShipPartName = "WARP DRIVE";
				break;
			}
		}
	}
}
