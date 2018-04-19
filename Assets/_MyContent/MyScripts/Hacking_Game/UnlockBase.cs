using UnityEngine;
using System.Collections;
using mindler.dungeonship;
using Sirenix.OdinInspector;

namespace mindler.hacking 
{
	[CreateAssetMenu(fileName = "Data", menuName = "HackingGame/UnlockBase", order = 2)]
	public class UnlockBase : ScriptableObject 
	{
		[BoxGroup("Name")]
		public string objectName = "New Unlock Unit";
		[BoxGroup("Name")]
		public HackingUnlock HackingUnlockName;
	
		[BoxGroup("Terminal Info")]
		public ShipPart ActivateTerminal = ShipPart.None;
		[BoxGroup("Terminal Info")]
		public bool ShowForLocalHack = true;

		[BoxGroup("Unlock Cost")]
		public float Cost = 5000f;
	}
}
