﻿using UnityEngine;
using System.Collections;
using UnityEditor;

namespace AC
{
	
	[CustomEditor (typeof (FollowTintMap))]
	public class FollowTintMapEditor : Editor
	{
		
		public override void OnInspectorGUI ()
		{
			FollowTintMap _target = (FollowTintMap) target;

			_target.useDefaultTintMap = EditorGUILayout.Toggle ("Use scene's default TintMap?", _target.useDefaultTintMap);
			if (!_target.useDefaultTintMap)
			{
				_target.tintMap = (TintMap) EditorGUILayout.ObjectField ("TintMap to use:", _target.tintMap, typeof (TintMap), true);
			}
			_target.affectChildren = EditorGUILayout.Toggle ("Affect children too?", _target.affectChildren);
			_target.intensity = EditorGUILayout.Slider ("Effect intensity:", _target.intensity, 0f, 1f);
			
			if (GUI.changed)
			{
				EditorUtility.SetDirty (_target);
			}
		}
		
	}
	
}