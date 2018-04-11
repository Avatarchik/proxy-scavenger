using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DunGen;
using Devdog.General.UI;
using Sirenix.OdinInspector;
using System.Linq;
using System.Collections.ObjectModel;

namespace mindler.dungeonship 
{
	public enum ShipPartState {
		Damaged = 0,
		Active = 1,
		Removed = 2,
		Destroyed = 3
	}

	public enum ShipPart {
		Engine = 0,
		PowerCore = 1,
		LifeSupport = 2,
		WarpDrive = 3,
		ThermalShielding = 4,
		Controls = 5
	}

	public enum ShipState {
		Damaged = 0,
		Active = 1,
		Unrecoverable = 2,
		Recovered = 3
	}

	public class DungeonShipManager : MonoBehaviour {

		public UIWindow TerminalWindow;
		public TerminalCyclePowerButton CyclePowerButton;
		public TerminalWindow WindowController;

		[BoxGroup("Ship Quality")]
		public Quality ShipQuality = Quality.Basic;

		[BoxGroup("Ship Hazard")]
		public HazardType hazard = HazardType.None;
		[BoxGroup("Ship Hazard")]
		public HazardLevel hazardRating = HazardLevel.None;

		public PrimaryRoomInfo EngineInfo;
		public PrimaryRoomInfo PowerCoreInfo;
		public PrimaryRoomInfo LifeSupportInfo;
		public PrimaryRoomInfo WarpDriveInfo;
		public PrimaryRoomInfo ThermalShieldingInfo;
		public PrimaryRoomInfo ControlsInfo;

		/*
		public bool EngineTerminalActive = true;
		public bool PowerCoreTerminalActive = true;
		public bool LifeSupportTerminalActive = true;
		public bool WarpDriveTerminalActive = true;
		public bool ThermalShieldingTerminalActive = true;
		public bool ControlsTerminalActive = true;
		*/

		public PrimaryRoomInfo[] RoomsInfo = new PrimaryRoomInfo[6];

		private int EngineCounter = 0;
		private int PowerCoreCounter = 0;
		private int LifeSupportCounter = 0;
		private int WarpDriveCounter = 0;
		private int ThermalShieldingCounter = 0;
		private int ControlsCounter = 0;


		public void SetupPrimaryRooms(ReadOnlyCollection<Tile> TileList){
			foreach(Tile t in TileList){
				PrimaryRoomInfo p = t.gameObject.GetComponent<PrimaryRoomInfo>();
				if(p != null){
					SetPrimaryRoom(p.ShipPartRoom, p);
				} else {
					Debug.Log("Yeah, no primary room info here");
				}
			}

			if(EngineCounter > 1){
				Debug.Log("Too Many Engines!");
			} else if (EngineCounter == 0){
				Debug.Log("Uh Oh! No Engines");
			}

			if(PowerCoreCounter > 1){
				Debug.Log("Too Many PowerCore!");
			} else if (PowerCoreCounter == 0){
				Debug.Log("Uh Oh! No PowerCore");
			}

			if(LifeSupportCounter > 1){
				Debug.Log("Too Many LifeSupport!");
			} else if (LifeSupportCounter == 0){
				Debug.Log("Uh Oh! No LifeSupport");
			}

			if(WarpDriveCounter > 1){
				Debug.Log("Too Many WarpDrive!");
			} else if (WarpDriveCounter == 0){
				Debug.Log("Uh Oh! No WarpDrive");
			}

			if(ThermalShieldingCounter > 1){
				Debug.Log("Too Many Thermal!");
			} else if (ThermalShieldingCounter == 0){
				Debug.Log("Uh Oh! No Thermal");
			}

			if(ControlsCounter > 1){
				Debug.Log("Too Many Controls!");
			} else if (ControlsCounter == 0){
				Debug.Log("Uh Oh! No Controls");
			}
		}

		public void SetAllTerminalsActive(){
			foreach(PrimaryRoomInfo p in RoomsInfo){
				p.TerminalActive = true;
			}
		}

		/*
		public bool GetTerminalActive(ShipPart part){
			bool active = true;
			switch(part){
			case ShipPart.Controls:
				active = RoomsInfo[0].TerminalActive;
				break;
			case ShipPart.Engine:
				active = RoomsInfo[1].TerminalActive;
				break;
			case ShipPart.LifeSupport:
				active = RoomsInfo[2].TerminalActive;
				break;
			case ShipPart.PowerCore:
				active = RoomsInfo[3].TerminalActive;
				break;
			case ShipPart.ThermalShielding:
				active = RoomsInfo[4].TerminalActive;
				break;
			case ShipPart.WarpDrive:
				active = RoomsInfo[5].TerminalActive;
				break;
			}
		}
		*/
		public void SetTerminalActive(ShipPart part, bool value){
			switch(part){
			case ShipPart.Controls:
				RoomsInfo[0].TerminalActive = value;
				break;
			case ShipPart.Engine:
				RoomsInfo[1].TerminalActive = value;
				break;
			case ShipPart.LifeSupport:
				RoomsInfo[2].TerminalActive = value;
				break;
			case ShipPart.PowerCore:
				RoomsInfo[3].TerminalActive = value;
				break;
			case ShipPart.ThermalShielding:
				RoomsInfo[4].TerminalActive = value;
				break;
			case ShipPart.WarpDrive:
				RoomsInfo[5].TerminalActive = value;
				break;
			}
		}

		public void SetPrimaryRoom(ShipPart part, PrimaryRoomInfo info){

			switch(part){
			case ShipPart.Controls:
				ControlsInfo = info;
				RoomsInfo[0] = ControlsInfo;
				ControlsCounter ++;
				break;
			case ShipPart.Engine:
				EngineInfo = info;
				RoomsInfo[1] = EngineInfo;
				EngineCounter ++;
				break;
			case ShipPart.LifeSupport:
				LifeSupportInfo = info;
				RoomsInfo[2] = LifeSupportInfo;
				LifeSupportCounter ++;
				break;
			case ShipPart.PowerCore:
				PowerCoreInfo = info;
				RoomsInfo[3] = PowerCoreInfo;
				PowerCoreCounter ++;
				break;
			case ShipPart.ThermalShielding:
				ThermalShieldingInfo = info;
				RoomsInfo[4] = ThermalShieldingInfo;
				ThermalShieldingCounter ++;
				break;
			case ShipPart.WarpDrive:
				ControlsInfo = info;
				RoomsInfo[5] = ControlsInfo;
				WarpDriveCounter ++;
				break;
			}
			ScanInfo();
		}

		public void ScanInfo(){
			int i = 0;
			foreach(PrimaryRoomInfo p in RoomsInfo){
				if(p == null){
					i++;
				}
			}
			if(i == 0){
				BreakHazardComponent();
			}
		}

		public void OpenTerminalWindow(string terminalName, bool online, bool damaged, PrimaryRoomInfo info){
			Debug.Log("Opening Terminal Window from DSM");
			TerminalWindow.Show();
			WindowController.SetName(terminalName);
			WindowController.SetOnline(online);
			WindowController.SetDamaged(damaged);
			WindowController.OpenWindow();
			WindowController.SetRoomInfo(info);
		}

		public void UpdateTerminalWindow(bool damaged){
			WindowController.SetDamaged(damaged);
			WindowController.OpenWindow();
		}

		public void HideTerminalWindow(){
			TerminalWindow.Hide();
		}

		private void BreakHazardComponent(){
			switch(hazard){
			case HazardType.Heat:
				RoomsInfo[1].PartState = ShipPartState.Damaged;
				RoomsInfo[1].ShipPartDamaged = true;
				RoomsInfo[1].TerminalActive = false;
				break;
			case HazardType.Cold:
				RoomsInfo[4].PartState = ShipPartState.Damaged;
				RoomsInfo[4].ShipPartDamaged = true;
				RoomsInfo[4].TerminalActive = false;
				break;
			case HazardType.Radiation:
				RoomsInfo[3].PartState = ShipPartState.Damaged;
				RoomsInfo[3].ShipPartDamaged = true;
				RoomsInfo[3].TerminalActive = false;
				break;
			case HazardType.Toxic:
				RoomsInfo[5].PartState = ShipPartState.Damaged;
				RoomsInfo[5].ShipPartDamaged = true;
				RoomsInfo[5].TerminalActive = false;
				break;
			case HazardType.Deoxygenation:
				RoomsInfo[2].PartState = ShipPartState.Damaged;
				RoomsInfo[2].ShipPartDamaged = true;
				RoomsInfo[2].TerminalActive = false;
				break;
			case HazardType.None:
				// No Ship Parts are broken
				break;
			}
		}
	}
}
