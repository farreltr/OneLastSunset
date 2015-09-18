﻿/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2015
 *	
 *	"ActionInstantiate.cs"
 * 
 *	This Action spawns prefabs and deletes
 *  objects from the scene
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionInstantiate : Action
	{
		
		public GameObject gameObject;
		public int parameterID = -1;
		public int constantID = 0; 

		public GameObject replaceGameObject;
		public int replaceParameterID = -1;
		public int replaceConstantID = 0;

		public InvAction invAction;
		public PositionRelativeTo positionRelativeTo = PositionRelativeTo.Nothing;


		public ActionInstantiate ()
		{
			this.isDisplayed = true;
			category = ActionCategory.Object;
			title = "Add or remove";
			description = "Instantiates or deletes GameObjects within the current scene. To ensure this works with save games correctly, place any prefabs to be added in a Resources asset folder.";
		}


		override public void AssignValues (List<ActionParameter> parameters)
		{
			if (invAction == InvAction.Add || invAction == InvAction.Replace)
			{
				gameObject = AssignFile (parameters, parameterID, 0, gameObject);

				if (invAction == InvAction.Replace)
				{
					replaceGameObject = AssignFile (parameters, replaceParameterID, replaceConstantID, replaceGameObject);
				}
			}
			else if (invAction == InvAction.Remove)
			{
				gameObject = AssignFile (parameters, parameterID, constantID, gameObject);
			}
		}
		
		
		override public float Run ()
		{
			if (gameObject == null)
			{
				return 0f;
			}

			if (invAction == InvAction.Add)
			{
				// Instantiate

				GameObject oldOb = AssignFile (constantID, gameObject);
				if (gameObject.activeInHierarchy || (oldOb != null && oldOb.activeInHierarchy))
				{
					Debug.Log (gameObject.name + " won't be instantiated, as it is already present in the scene.");
					return 0f;
				}

				Vector3 position = gameObject.transform.position;
				Quaternion rotation = gameObject.transform.rotation;
				
				if (positionRelativeTo != PositionRelativeTo.Nothing)
				{
					float forward = gameObject.transform.position.z;
					float right = gameObject.transform.position.x;
					float up = gameObject.transform.position.y;

					if (positionRelativeTo == PositionRelativeTo.RelativeToActiveCamera)
					{
						Transform mainCam = KickStarter.mainCamera.transform;
						position = mainCam.position + (mainCam.forward * forward) + (mainCam.right * right) + (mainCam.up * up);
						rotation.eulerAngles += mainCam.transform.rotation.eulerAngles;
					}
					else if (positionRelativeTo == PositionRelativeTo.RelativeToPlayer)
					{
						if (KickStarter.player)
						{
							Transform playerTranform = KickStarter.player.transform;
							position = playerTranform.position + (playerTranform.forward * forward) + (playerTranform.right * right) + (playerTranform.up * up);
							rotation.eulerAngles += playerTranform.rotation.eulerAngles;
						}
					}
				}

				GameObject newObject = (GameObject) Instantiate (gameObject, position, rotation);
				newObject.name = gameObject.name;
				KickStarter.stateHandler.GatherObjects ();
			}
			else if (invAction == InvAction.Remove)
			{
				// Delete
				KickStarter.sceneSettings.ScheduleForDeletion (gameObject);
			}
			else if (invAction == InvAction.Replace)
			{
				if (replaceGameObject == null)
				{
					Debug.LogWarning ("Cannot perform swap because the object to remove was not found in the scene.");
					return 0f;
				}

				Vector3 position = replaceGameObject.transform.position;
				Quaternion rotation = replaceGameObject.transform.rotation;

				GameObject oldOb = AssignFile (constantID, gameObject);
				if (gameObject.activeInHierarchy || (oldOb != null && oldOb.activeInHierarchy))
				{
					Debug.Log (gameObject.name + " won't be instantiated, as it is already present in the scene.");
					return 0f;
				}

				KickStarter.sceneSettings.ScheduleForDeletion (gameObject);

				GameObject newObject = (GameObject) Instantiate (gameObject, position, rotation);
				newObject.name = gameObject.name;
				KickStarter.stateHandler.GatherObjects ();
			}

			return 0f;
		}
		
		
		#if UNITY_EDITOR
		
		override public void ShowGUI (List<ActionParameter> parameters)
		{
			invAction = (InvAction) EditorGUILayout.EnumPopup ("Method:", invAction);

			string _label = "Object to instantiate:";
			if (invAction == InvAction.Remove)
			{
				_label = "Object to delete:";
			}

			parameterID = Action.ChooseParameterGUI (_label, parameters, parameterID, ParameterType.GameObject);
			if (parameterID >= 0)
			{
				constantID = 0;
				gameObject = null;
			}
			else
			{
				gameObject = (GameObject) EditorGUILayout.ObjectField (_label, gameObject, typeof (GameObject), true);

				constantID = FieldToID (gameObject, constantID);
				gameObject = IDToField (gameObject, constantID, false);
			}

			if (invAction == InvAction.Add)
			{
				positionRelativeTo = (PositionRelativeTo) EditorGUILayout.EnumPopup ("Position relative to:", positionRelativeTo);
			}
			else if (invAction == InvAction.Replace)
			{
				EditorGUILayout.Space ();
				replaceParameterID = Action.ChooseParameterGUI ("Object to delete:", parameters, replaceParameterID, ParameterType.GameObject);
				if (replaceParameterID >= 0)
				{
					replaceConstantID = 0;
					replaceGameObject = null;
				}
				else
				{
					replaceGameObject = (GameObject) EditorGUILayout.ObjectField ("Object to delete:", replaceGameObject, typeof (GameObject), true);
					
					replaceConstantID = FieldToID (replaceGameObject, replaceConstantID);
					replaceGameObject = IDToField (replaceGameObject, replaceConstantID, false);
				}
			}

			AfterRunningOption ();
		}
		
		
		public override string SetLabel ()
		{
			string labelAdd = " (" + invAction.ToString ();
			if (gameObject)
			{
				labelAdd += " " + gameObject.name;
			}
			labelAdd += ")";
			return labelAdd;
		}
		
		#endif
		
	}

}