﻿namespace Sirenix.OdinInspector.Demos
{
	using UnityEngine;

	public class VerticalGroupExamples : MonoBehaviour
	{
#if UNITY_EDITOR
		[PropertyOrder(-1)]
		[OnInspectorGUI]
		public void DrawInfo()
		{
			Sirenix.Utilities.Editor.SirenixEditorGUI.InfoMessageBox(
				"VerticalGroup, similar to HorizontalGroup, groups properties together vertically in the inspector.\n" +
				"By itself it doesn't do much, but combined with other groups, like HorizontalGroup, it can be very useful.");
		}
#endif
		
		[HorizontalGroup("Split")]
		[VerticalGroup("Split/Left")]
		[LabelWidth(100)]
		public Vector3 Vector;

		[VerticalGroup("Split/Left")]
		[LabelWidth(100)]
		public GameObject First;

		[VerticalGroup("Split/Left")]
		[LabelWidth(100)]
		public GameObject Second;

		[HideLabel]
		[VerticalGroup("Split/Right", PaddingTop = 18f)]
		public int A;

		[HideLabel]
		[VerticalGroup("Split/Right")]
		public int B;
	}
}
