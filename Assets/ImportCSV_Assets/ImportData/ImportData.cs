/* Author philipp zupke, 2015 phil@bytestyles.net *
 * This class takes care of converting the data from the csv into unity/ c# readable data
 * 
 * -!! IMPORTANT !!-
 * TO LOAD YOUR FILES, ADD THEM TO THE "importNames" array
 * 
 * The only acces from the outside is the "GetContainer" function, which will return the container for a specific ID
 * The data can then be retrieved from that container
 * 
 * There is a lazy initialisation which only loads this class and all the data from csv, when a container is request
 *  
 * 
 * 	Note that a container is simply the ID (the text (header) in the first row), and a bunch of Key-Value Pairs
 *	Your DATA MUST thereyby be structured like this:
 *   __________________________________
 *  | header |       | header |       |
 *  | key    | value | key    | value |
 *  | key    | value | key    | value |
 *  | key    | value | key    | value |
 *  | key    | value | key    | value |
 *  | key    | value | key    | value |
 *  | key    | value | key    | value |
 *  ------------------------------------
 * */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ImportData   {

	static string[] importNames = new string[]  { "example","items" };


	static ImportData instance;

	List<ImportedDataContainer> containers;


	public ImportData(){
		instance = this;
		containers =  new List<ImportedDataContainer>();

		foreach(string file in importNames){		
			ImportedTable _table = ImportedTable.LoadFromFile(file) ;
			if(_table != null){
				containers.AddRange(  getDataFromTable( _table) );	
			} else {
				Debug.LogWarning("Could not import data from "+file);
			}
		}

		Debug.Log("- DataContainers Configured:"+containers.Count );
	}


	public static void Load(){
		new ImportData();
	}


	public static ImportedDataContainer GetContainer(string id){
		if(instance == null){
			Load();
		}

		for(int i = 0; i < instance.containers.Count; i++){
			if( instance.containers[i].ID == id) return instance.containers[i];
		}

		Debug.LogWarning("Couldnt find DataContainer for "+id);
		return null;

	}

	// Creates containers from a table
	static List<ImportedDataContainer> getDataFromTable(ImportedTable _table){

		//Sets pointer to cell 0,0
		_table.ResetPointer();
		 
		//Add the first container [should be under 0,0]
		List<ImportedDataContainer> _containers = new List<ImportedDataContainer>();
		_containers.Add( GetContainer(_table));

		//Add containers, as long the script finds values in the first ROW of your csv
		while(_table.SetNextHeader()){

			_containers.Add (GetContainer(_table));

		}

		return _containers;
	}


	//returns the container from current table-pointer position

	static ImportedDataContainer GetContainer(ImportedTable _table){

		string id = _table.GetStringFromPointer();

		if( string.IsNullOrEmpty(id)){
			return null;
		}

		ImportedDataContainer container = new ImportedDataContainer();
	
		container.ID = id;
		Debug.Log("-- new Container: "+id);

		//iterate through the rows (as long as they are not empty
		//and add DataPoints (Key-Value Pairs) to the current container

		while(_table.SetNextRow(false)){
			container.AddDataPoint( GetDataPoint(_table));			
		}

		return container;

	}

	//Loads the Key Value Pair from the current pointer in the table
	static ImportedDataPoint GetDataPoint(ImportedTable _table){

		ImportedDataPoint p = new ImportedDataPoint();

		string[] pair = _table.GetKeyValuePair();
		if(pair == null) return null;


		p.key = pair[0];
		p.value = pair[1];

		return p;
	}

}
