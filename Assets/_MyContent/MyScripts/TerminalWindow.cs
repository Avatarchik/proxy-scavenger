using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Devdog.General.UI;
using mindler.dungeonship;

public class TerminalWindow : MonoBehaviour {

	public UIWindow TerminalOfflineWindow;
	public UIWindow TerminalOnlineDamagedWindow;
	public UIWindow TerminalOnlineActiveWindow;

	public GameObject TerminalOffline;
	public GameObject TerminalOnlineDamaged;
	public GameObject TerminalOnlineActive;

	public Text OnlineDamagedTerminalName;
	public Text OnlineActiveTerminalName;

	public bool Online = true;
	public bool Damaged = false;

	public PrimaryRoomInfo roomInfo;

	public void OpenWindow(){
		Debug.Log("Opening Window from TerminalWindow");
		if(Online){
			if(Damaged){
				Debug.Log("Online and Damaged");
				TerminalOfflineWindow.Hide();
				TerminalOnlineDamagedWindow.Show();
				TerminalOnlineActiveWindow.Hide();
				//TerminalOffline.SetActive(false);
				//TerminalOnlineDamaged.SetActive(true);
				//TerminalOnlineActive.SetActive(false);
			} else {
				Debug.Log("Online and Active");
				TerminalOfflineWindow.Hide();
				TerminalOnlineDamagedWindow.Hide();
				TerminalOnlineActiveWindow.Show();
				//TerminalOffline.SetActive(false);
				//TerminalOnlineDamaged.SetActive(false);
				//TerminalOnlineActive.SetActive(true);
			}
		} else {
			Debug.Log("Offline");
			TerminalOfflineWindow.Show();
			TerminalOnlineDamagedWindow.Hide();
			TerminalOnlineActiveWindow.Hide();
			//TerminalOffline.SetActive(true);
			//TerminalOnlineDamaged.SetActive(false);
			//TerminalOnlineActive.SetActive(false);
		}
	}

	public void SetName(string Name){
		OnlineDamagedTerminalName.text = Name + " DAMAGED";
		OnlineActiveTerminalName.text = Name + " ACTIVE";
	}

	public void SetOnline(bool value){
		Online = value;
	}

	public void SetDamaged(bool value){
		Damaged = value;
	}

	public void CyclePower(){
		roomInfo.RepairableBoxStateCheck();
		roomInfo.UpdateWindow();
	}

	public void SetRoomInfo(PrimaryRoomInfo info){
		roomInfo = info;
	}
}
