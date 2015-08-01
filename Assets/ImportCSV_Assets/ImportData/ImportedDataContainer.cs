using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImportedDataContainer {

	public string ID;

	List<ImportedDataPoint> data = new List<ImportedDataPoint>();

	public void AddDataPoint(ImportedDataPoint d){
		data.Add(d);
	}
	
	public ImportedDataPoint GetData(string id){
		for(int i = 0; i < data.Count; i++){
			if( data[i].key == id) return data[i];
		}
		Debug.LogWarning("Could not find Value for Key "+id);
		return null;
	}
}
