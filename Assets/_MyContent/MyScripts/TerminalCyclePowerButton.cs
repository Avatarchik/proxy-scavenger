using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mindler.dungeonship 
{
	public class TerminalCyclePowerButton : MonoBehaviour {

		[SerializeField]
		public ShipPart ShipPartRoom = ShipPart.Controls;
		public PrimaryRoomInfo roomInfo;

		public void init(ShipPart part, PrimaryRoomInfo info){
			ShipPartRoom = part;
			roomInfo = info;
		}

		public void OnClick(){

		}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}
