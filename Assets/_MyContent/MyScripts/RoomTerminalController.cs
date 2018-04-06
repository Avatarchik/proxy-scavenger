using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.General.UI;
using Devdog.General;
using Sirenix.OdinInspector;

namespace mindler.dungeonship 
{
	public class RoomTerminalController : MonoBehaviour, ITriggerCallbacks{

		public PrimaryRoomInfo PRI;


		public bool OnTriggerUsed(Player player)
		{
			Debug.Log("Room Terminal Used");

			PRI.OpenWindow();
			return false;
			// The trigger has been used.

			// Return true to consume the event (other callback listeners won't receive it)
			// Return false to not cosnume the event.
		}

		public bool OnTriggerUnUsed(Player player)
		{
			Debug.Log("Trigger Unused");
			PRI.HideWindow();

			// The trigger has een unused
			// Return true to consume the event (other callback listeners won't receive it)
			// Return false to not cosnume the event.
			return false;
		}
	}
}
