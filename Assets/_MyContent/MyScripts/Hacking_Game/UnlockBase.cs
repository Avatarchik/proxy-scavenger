using UnityEngine;
using System.Collections;


namespace mindler.hacking 
{
	[CreateAssetMenu(fileName = "Data", menuName = "HackingGame/UnlockBase", order = 2)]
	public class UnlockBase : ScriptableObject 
	{
		public string objectName = "New Unlock Unit";
		public HackingUnlock HackingUnlockName;

		public float Cost = 5000f;
	}
}
