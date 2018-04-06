using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Devdog.General;
using Devdog.General.UI;

namespace mindler.dungeonship 
{
	public class TerminalManager : MonoBehaviour {

		public UIWindow TerminalWindow;
		public TerminalWindow WindowController;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void OpenTerminalWindow(string terminalName, bool online, bool damaged){
			WindowController.SetName(terminalName);
			WindowController.SetOnline(online);
			WindowController.SetDamaged(damaged);
			TerminalWindow.Show();
		}

		public void HideTerminalWindow(){
			TerminalWindow.Hide();
		}
	}
}
