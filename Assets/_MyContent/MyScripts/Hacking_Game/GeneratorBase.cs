using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace mindler.hacking 
{
	[CreateAssetMenu(fileName = "Data", menuName = "HackingGame/GeneratorBase", order = 1)]
	public class GeneratorBase : ScriptableObject 
	{
		[BoxGroup("Name and Type")]
		public string objectName = "New MyScriptableObject";
		[BoxGroup("Name and Type")]
		public GeneratorType GeneratorName;

		[InlineEditor(InlineEditorModes.LargePreview)]
		public Sprite Icon;

		[BoxGroup("Initial Values")]
		public float initialRevenue = 100f;
		[BoxGroup("Initial Values")]
		public float initialNewGeneratorCost = 1000f;
		[BoxGroup("Initial Values")]
		public float initialProgressTime = 2f; //seconds
		[BoxGroup("Initial Values")]
		public float initialProgressTimeMultiplier = -0.2f;

		[BoxGroup("New Generator Values")]
		public float newGeneratorRevenueMultiplier = 1.05f; 
		[BoxGroup("New Generator Values")]
		public float newGeneratorCostMultiplier = 1.13f;
		[BoxGroup("New Generator Values")]
		public float newGeneratorTimeMultiplier = -0.2f; //reduce how much time it takes to generate

		[BoxGroup("Unlock Cost")]
		public float unlockCost = 0f;
	}
}