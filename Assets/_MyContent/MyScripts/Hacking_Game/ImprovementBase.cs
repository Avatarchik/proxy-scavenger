using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace mindler.hacking 
{
	[CreateAssetMenu(fileName = "Data", menuName = "HackingGame/ImprovementBase", order = 2)]
	public class ImprovementBase : ScriptableObject 
	{
		[BoxGroup("Name and Type")]
		public string Name;
		[BoxGroup("Name and Type")]
		public GeneratorType ImproveUnit;

		[InlineEditor(InlineEditorModes.LargePreview)]
		public Sprite Icon;

		[BoxGroup("Improve Units")]
		public bool ImproveAllUnits = false;

		[BoxGroup("Improve Production")]
		public bool ProductionImprovement = true;
		[BoxGroup("Improve Production")]
		public float ProductionImprovementMultiplier = 2f;

		[BoxGroup("Speed Improvement")]
		public bool SpeedImprovement = false;
		[BoxGroup("Speed Improvement")]
		public float SpeedImprovementMultiplier = 1f;

		[BoxGroup("Cost")]
		public float Cost = 5000f;
	}

}
