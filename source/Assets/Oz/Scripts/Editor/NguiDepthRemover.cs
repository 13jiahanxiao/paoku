using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class NguiDepthRemover  {
	
	[MenuItem ("NGUI/Remove Depth")]
	public static void RemoveDepth()
	{
		if (Selection.activeTransform == null)
		{
			string message = "You must select an object before running this command";
			EditorUtility.DisplayDialog ("Error", message, "Ok");		
			return;
		}
		
		GameObject activeObject = Selection.activeTransform.gameObject;
		UIWidget [] widgets = activeObject.GetComponentsInChildren<UIWidget>();
		int lowestDepth = int.MaxValue;
		int highestDepth = int.MinValue;
		if (widgets.Length == 0)
		{
			string message = "No children UI Widgets found";
			EditorUtility.DisplayDialog ("Error", message, "Ok");		
			return;			
		}
		Debug.Log ( "Moving back means an object getting a more positive z potion");
		foreach (UIWidget widget in widgets)
		{
			// so a higher depth brings the object forward, in world space the a higher depth means a more negative z
			int depth = widget.depth;
			if (depth == 0 )
			{
				continue;	
			}
			if (depth < lowestDepth)
			{
				lowestDepth = depth;	
			}
			if (depth > highestDepth)
			{
				highestDepth = depth;	
			}
			if ( depth > 0)
			{
				
				Debug.Log ("Moving " + HierarchyUtils.GetHierarchyPath(widget.gameObject,true) + " forward " + depth );
			}
			else
			{
				Debug.Log (	"Moving " + HierarchyUtils.GetHierarchyPath(widget.gameObject,true)  + " backward " + depth );
			}
			widget.transform.localPosition += new Vector3(0, 0, -depth);
			widget.depth = 0;
		}
		//explanation += "highest depth = " + highestDepth + "\n";
		//explanation += "lowest depth = " + lowestDepth + "\n";
		EditorUtility.DisplayDialog ("Success", "Look at the console log for details", "Ok");
	}
}
