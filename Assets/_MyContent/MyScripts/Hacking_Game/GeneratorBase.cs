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
		public float initialRevenueMultiplier = 2f;
		[BoxGroup("Initial Values")]
		public float initialNewGeneratorCost = 1000f;
		[BoxGroup("Initial Values")]
		public float initialProgressTime = 2f; //seconds
		[BoxGroup("Initial Values")]
		public float initialProgressTimeMultiplier = 0.95f;

		/*
		[BoxGroup("New Generator Values")]
		public float newGeneratorRevenueMultiplier = 1.05f; 
		[BoxGroup("New Generator Values")]
		public float newGeneratorCostMultiplier = 1.13f;
		[BoxGroup("New Generator Values")]
		public float newGeneratorTimeMultiplier = 0.95f; //reduce how much time it takes to generate
		*/

		[BoxGroup("Unlock Cost")]
		public float unlockCost = 1000f;

		[BoxGroup("Locked")]
		public bool Lock = true;

		//[BoxGroup("Graduated Time Multiplier")]
		[System.NonSerialized]
		private Vector3[] GraduatedTimeMultiplier = new Vector3[11]{new Vector3(1f,1f,0f),
			new Vector3(10f,0.9f, 0f),
			new Vector3(20f,0.75f, 0f),
			new Vector3(30f,0.97f, 0f),
			new Vector3(40f,0.98f, 0f),
			new Vector3(50f,0.75f, 0f),
			new Vector3(60f,0.95f, 0f),
			new Vector3(70f,0.96f, 0f),
			new Vector3(80f,0.97f, 0f),
			new Vector3(90f,0.98f, 0f),
			new Vector3(100f,0.75f, 0f)};

		public Vector3[] GraduatedTimeMultipliers
		{
			get {return GraduatedTimeMultiplier; }
			set{GraduatedTimeMultiplier = value;}
		}

		//[BoxGroup("Graduated Cost Multiplier")]
		[System.NonSerialized]
		private Vector3[] GraduatedCostMultiplier = new Vector3[12]{new Vector3(1f,1.2f,0f),
			new Vector3(3f,1.3f, 0f),
			new Vector3(10f,1.4f, 0f),
			new Vector3(20f,1.55f, 0f),
			new Vector3(30f,1.65f, 0f),
			new Vector3(40f,1.7f, 0f),
			new Vector3(50f,1.8f, 0f),
			new Vector3(60f,1.9f, 0f),
			new Vector3(70f,2f, 0f),
			new Vector3(80f,2.5f, 0f),
			new Vector3(90f,2.8f, 0f),
			new Vector3(100f,3f, 0f)};

		public Vector3[] GraduatedCostMultipliers
		{
			get {return GraduatedCostMultiplier; }
			set{GraduatedCostMultiplier = value;}
		}

		//[BoxGroup("Graduated Revenue Multiplier")]
		[System.NonSerialized]
		private Vector3[] GraduatedRevenueMultiplier = new Vector3[27]{new Vector3(1f,2.0f,0f),
			new Vector3(2f,1.5f, 0f),
			new Vector3(3f,1.3f, 0f),
			new Vector3(4f,1.2821f, 0f),
			new Vector3(5f,1.2f, 0f),
			new Vector3(9f,2f, 0f),
			new Vector3(10f,1.1f, 0f),
			new Vector3(19f,1.5f, 0f),
			new Vector3(20f,1.09f, 0f),
			new Vector3(29f,2f, 0f),
			new Vector3(30f,1.08f, 0f),
			new Vector3(39f,2f, 0f),
			new Vector3(40f,1.07f, 0f),
			new Vector3(49f,2f, 0f),
			new Vector3(50f,1.06f, 0f),
			new Vector3(59f,1.5f, 0f),
			new Vector3(60f,1.05f, 0f),
			new Vector3(69f,1.5f, 0f),
			new Vector3(70f,1.04f, 0f),
			new Vector3(79f,1.5f, 0f),
			new Vector3(80f,1.03f, 0f),
			new Vector3(89f,1.5f, 0f),
			new Vector3(90f,1.02f, 0f),
			new Vector3(99f,1.5f, 0f),
			new Vector3(100f,1.01f, 0f),
			new Vector3(109f,1.25f, 0f),
			new Vector3(110f,1.005f, 0f)};

		public Vector3[] GraduatedRevenueMultipliers
		{
			get {return GraduatedRevenueMultiplier; }
			set{GraduatedRevenueMultiplier = value;}
		}



	}
}