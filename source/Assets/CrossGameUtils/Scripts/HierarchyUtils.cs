using UnityEngine;
using System.Collections;

public class HierarchyUtils
{	
    public static GameObject GetChildByName(string name, GameObject go)	// Find child of GameObject by its name
    {
        GameObject result = null;
        Transform t = go.transform.FindChild(name);

        if (t != null) { return t.gameObject; }

        for(int i=0; i<go.transform.childCount; i++)
		{
			t = go.transform.GetChild(i);
			result = GetChildByName(name, t.gameObject);
			if (result != null) { return result; }
		}
		return null;
    }
	
	/// <summary>
	/// Gets the string hierarchy for the given transform
	/// </summary>
	/// <returns>
	/// The hierarchy path string
	/// </returns>
	/// <param name='trans'>
	/// the transform to traverse
	/// </param>
	/// <param name='parentsFirst'>
	/// if true, returned string will have the parents first, just like a regular file path
	/// if false, then transform will be at the start, followed by the parent, followed by the grandparent, etc
	/// </param>
	public static string GetHierarchyPath(Transform trans, bool parentsFirst = true)
	{
		//TODO this is meant to be a debugging tool, if this MUST in some Update
		// probably better to change this to a for loop
		string result = "";
		if (trans != null)
		{
			if (parentsFirst)
			{
				result = GetHierarchyPath(trans.parent) + "/" + trans.name;	
			}
			else
			{
				result = trans.name + "/" + GetHierarchyPath(trans.parent);
			}
		}
		return result;
	}
	
	/// <summary>
	/// Gets the string hierarchy for the given game object
	/// </summary>
	/// <returns>
	/// The hierarchy path string
	/// </returns>
	/// <param name='trans'>
	/// the game object to traverse
	/// </param>
	/// <param name='parentsFirst'>
	/// if true, returned string will have the parents first, just like a regular file path
	/// if false, then transform will be at the start, followed by the parent, followed by the grandparent, etc
	/// </param>
	public static string GetHierarchyPath(GameObject go, bool parentsFirst = true)
	{
		string result = "";
		if (go != null)
		{
			result = HierarchyUtils.GetHierarchyPath(go.transform);
		}
		return result;
			
	}
}

// another version, this should work too!

    //Find a child in go by its name
//    private GameObject GetChildByName(string name, GameObject go)
//    {
//        GameObject result = null;
//        Transform t = go.transform.FindChild(name);
//
//        if (t != null)
//            result = t.gameObject;
//
//        return result;
//    }