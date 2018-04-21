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
		public bool ShipPartDamaged = true;

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

			init();
		}
		
		public void init(){
			CreateRepairBoxes();
			CheckPartState();
		}

		public void DungeonComplete(){
			foreach(GameObject g in RepairBoxes){
				GrabObjectSlide r = g.GetComponent<GrabObjectSlide>();
				r.DungeonComplete();
			}

			foreach(RepairableObject ro in RepairBoards){
				ro.DungeonComplete();
			}
		}

		public void RepairableBoxStateCheck(){
			int b = 0;
			foreach(RepairableObject r in RepairBoards){
				if(r != null){
					if(r.ObjectRepaired == true){
						b++;
					}
				} else {
					Debug.Log("repairable object in array is null?");
				}
			}

			if(b == 3){
				ShipPartDamaged = false;
				PartState = ShipPartState.Damaged;
			} else {
				ShipPartDamaged = true;
				PartState = ShipPartState.Active;
			}
		}

		public void CreateRepairBoxes(){
			int i = 0;
			foreach(GameObject a in RepairBoxAnchors){
				
				//GameObject g = GameObject.Instantiate(RepairBoxObject,RepairBoxAnchors[i].transform.position, RepairBoxAnchors[i].transform.rotation);
				GameObject g;
				if(RepairBoxParent != null){
					g = GameObject.Instantiate(RepairBoxObject, RepairBoxParent.transform);
					Debug.Log(this.gameObject.name + " : " + ShipPartName + " RepairBoxParent is NOT null");
				} else {
					g = GameObject.Instantiate(RepairBoxObject, this.transform);
					Debug.Log(this.gameObject.name + " : " + ShipPartName + " Fucking RepairBoxParent is NULL!? THE FUCK!?");
				}
				g.transform.position = RepairBoxAnchors[i].transform.position;
				g.transform.rotation = RepairBoxAnchors[i].transform.rotation;
				g.GetComponent<GrabObjectSlide>().StartPosition();
				RepairBoxes[i] = g;
				//RepairBoxes[i].transform.parent = RepairBoxParent.transform;
				RepairableObject r = g.GetComponentInChildren<RepairableObject>();
				RepairBoards[i] = r;

				i++;
			}

			int numberOfBrokenBoards = 0;
			if(PartState != ShipPartState.Active){
				numberOfBrokenBoards = UnityEngine.Random.Range(0,3);
			}
			int p = 0;
			foreach (RepairableObject r in RepairBoards){
				if(PartState == ShipPartState.Active){
					r.repairState = RepairObjectRepairState.Working;
				} else {
					if(p <= numberOfBrokenBoards){
						r.repairState = RepairObjectRepairState.Damaged;
					} else {
						r.repairState = RepairObjectRepairState.Working;
					}
				}

				r.init();
			}
		}

		public void UpdateWindow(){
			DSM.UpdateTerminalWindow(ShipPartDamaged);
		}

		public void OpenWindow(){
			Debug.Log("Opening Window from PRI");
			CheckPartState();
			DSM.OpenTerminalWindow(ShipPartName, TerminalActive, ShipPartDamaged, this);
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
