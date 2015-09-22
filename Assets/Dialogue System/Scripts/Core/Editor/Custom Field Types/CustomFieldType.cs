using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem {

	/// <summary>
	/// The abstract base custom field type.
	/// </summary>
	public abstract class CustomFieldType {

		public virtual FieldType storeFieldAsType {
			get {
				return FieldType.Text;
			}
		}
		
		public abstract string Draw(string currentValue, DialogueDatabase dataBase);
	}

}
