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
		[BoxGroup("Improve Units")]
		public float ImprovementMultiplier = 2f;

		[BoxGroup("Cost")]
		public float Cost = 5000f;
	}

}
