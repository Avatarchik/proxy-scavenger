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


		//[BoxGroup("Graduated Time Multiplier")]
		//public Vector2[] GeneratorGraduatedTimeMultiplier;

		//[BoxGroup("Graduated Cost Multiplier")]
		//public Vector2[] GeneratorGraduatedCostMultiplier;

		//[BoxGroup("Graduated Time Multiplier")]
		private Vector3[] GraduatedTimeMultiplier = new Vector3[11]{new Vector3(1f,1f,0f),
			new Vector3(10f,0.95f, 0f),
			new Vector3(20f,0.96f, 0f),
			new Vector3(30f,0.97f, 0f),
			new Vector3(40f,0.98f, 0f),
			new Vector3(50f,0.99f, 0f),
			new Vector3(60f,0.995f, 0f),
			new Vector3(70f,0.996f, 0f),
			new Vector3(80f,0.997f, 0f),
			new Vector3(90f,0.998f, 0f),
			new Vector3(100f,0.999f, 0f)};

		public Vector3[] GraduatedTimeMultipliers
		{
			get {return GraduatedTimeMultiplier; }
			set{GraduatedTimeMultiplier = value;}
		}

		//[BoxGroup("Graduated Cost Multiplier")]
		private Vector3[] GraduatedCostMultiplier = new Vector3[12]{new Vector3(1f,1.13f,0f),
			new Vector3(10f,1.14f, 0f),
			new Vector3(20f,1.15f, 0f),
			new Vector3(30f,1.16f, 0f),
			new Vector3(40f,1.17f, 0f),
			new Vector3(50f,1.18f, 0f),
			new Vector3(60f,1.19f, 0f),
			new Vector3(70f,1.2f, 0f),
			new Vector3(80f,1.21f, 0f),
			new Vector3(90f,1.22f, 0f),
			new Vector3(100f,1.23f, 0f),
			new Vector3(110f,1.24f, 0f)};

		public Vector3[] GraduatedCostMultipliers
		{
			get {return GraduatedCostMultiplier; }
			set{GraduatedCostMultiplier = value;}
		}

		//[BoxGroup("Graduated Revenue Multiplier")]
		private Vector3[] GraduatedRevenueMultiplier = new Vector3[22]{new Vector3(1f,2.0f,0f),
			new Vector3(2f,1.5f, 0f),
			new Vector3(3f,1.3f, 0f),
			new Vector3(4f,1.2821f, 0f),
			new Vector3(5f,1.2f, 0f),
			new Vector3(9f,2f, 0f),
			new Vector3(10f,1.18f, 0f),
			new Vector3(19f,1.5f, 0f),
			new Vector3(20f,1.17f, 0f),
			new Vector3(29f,1.4f, 0f),
			new Vector3(30f,1.16f, 0f),
			new Vector3(39f,1.3f, 0f),
			new Vector3(40f,1.15f, 0f),
			new Vector3(49f,2f, 0f),
			new Vector3(50f,1.18f, 0f),
			new Vector3(60f,1.19f, 0f),
			new Vector3(70f,1.2f, 0f),
			new Vector3(80f,1.21f, 0f),
			new Vector3(90f,1.22f, 0f),
			new Vector3(99f,2f, 0f),
			new Vector3(100f,1.23f, 0f),
			new Vector3(110f,1.24f, 0f)};

		public Vector3[] GraduatedRevenueMultipliers
		{
			get {return GraduatedRevenueMultiplier; }
			set{GraduatedRevenueMultiplier = value;}
		}



	}
}