/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2014
 *	
 *	"RememberName.cs"
 * 
 *	This script is attached to gameObjects in the scene
 *	with a name we wish to save.
 * 
 */

using UnityEngine;
using System.Collections;

namespace AC
{

	/**
	 * This script is attached to GameObject in the scene whose change in name we wish to save.
	 */
	public class RememberName : Remember
	{

		/**
		 * <summary>Serialises appropriate GameObject values into a string.</summary>
		 * <returns>The data, serialised as a string</returns>
		 */
		public override string SaveData ()
		{
			NameData nameData = new NameData();
			nameData.objectID = constantID;
			nameData.newName = gameObject.name;

			return Serializer.SaveScriptData <NameData> (nameData);
		}


		/**
		 * <summary>Deserialises a string of data, and restores the GameObject to it's previous state.</summary>
		 * <param name = "stringData">The data, serialised as a string</param>
		 */
		public override void LoadData (string stringData)
		{
			NameData data = Serializer.LoadScriptData <NameData> (stringData);
			if (data == null) return;

			gameObject.name = data.newName;
		}

	}


	/**
	 * A data container used by the RememberName script.
	 */
	[System.Serializable]
	public class NameData : RememberData
	{

		/** The GameObject's new name */
		public string newName;

		/**
		 * The default Constructor.
		 */
		public NameData () { }

	}

}