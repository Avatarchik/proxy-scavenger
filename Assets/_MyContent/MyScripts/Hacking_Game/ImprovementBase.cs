using UnityEngine;
using System.Collections;


namespace mindler.hacking 
{
	[CreateAssetMenu(fileName = "Data", menuName = "HackingGame/UnlockBase", order = 2)]
	public class ImprovementBase : ScriptableObject 
	{
		public string objectName = "New Improvement Unit";
		public HackingUnlock HackingUnlockName;
		public GeneratorType ImproveUnit;
		public bool ImproveAllUnits = false;

		public float Cost = 5000f;
	}
}
