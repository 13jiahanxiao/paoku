using System;
using UnityEngine;

public partial class JSONTools
{
    static public int ReadInt(object obj) 
	{
		if (obj == null) { return 0; }
		if (obj.GetType() == typeof(Int64)) { return (int)((long) obj); }
		else if (obj.GetType() == typeof(int)) { return (int)(obj); }
		return 0;
	}
	
	static public double ReadDouble(object obj) 
	{
		if (obj == null) {Debug.Log("Hm.");return 0.0;}
		if (obj.GetType() == typeof(Int64)) { return (double)((long) obj); }
		else if (obj.GetType() == typeof(Double)) { return (double)(obj); }
		Debug.Log("Hm.2");
		return 0.0;
	}
}
