﻿/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2015
 *	
 *	"ActionComment.cs"
 * 
 *	This action simply displays a comment in the Editor / Inspector.
 * 
 */

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{
	
	[System.Serializable]
	public class ActionComment : Action
	{
		
		public string commentText = "";
		public bool outputToDebugger;
		
		
		public ActionComment ()
		{
			this.isDisplayed = true;
			category = ActionCategory.ActionList;
			title = "Comment";
			description = "Prints a comment for debug purposes.";
		}


		public override float Run ()
		{
			if (outputToDebugger && commentText.Length > 0)
			{
				Debug.Log (commentText);
			}
			return 0f;
		}
		
		
		#if UNITY_EDITOR
		
		override public void ShowGUI ()
		{
			commentText = EditorGUILayout.TextArea (commentText);
			outputToDebugger = EditorGUILayout.Toggle ("Print in Console?", outputToDebugger);
			
			AfterRunningOption ();
		}
		
		
		public override string SetLabel ()
		{
			if (commentText.Length > 0)
			{
				int i = commentText.IndexOf ("\n");
				if (i > 0)
				{
					return (" (" + commentText.Substring (0, i) + ")");
				}
				return (" (" + commentText + ")");
			}
			return "";
		}
		
		#endif
		
	}
	
}