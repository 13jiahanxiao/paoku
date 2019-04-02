using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ListToStringConverter 
{
	public static List<int> GetListFromString(string inputString)
	{
		List<int> list = new List<int>();
		
		if (inputString != "")
		{
			foreach (string s in inputString.Split(','))
		    	list.Add(int.Parse(s));
		}
		
		return list;
	}
	
	public static string MakeStringFromList<T>(List<T> source)	//, string delimiter)
	{
	   	var s = new StringBuilder();
	   	bool first = true;
		
	   	foreach (T t in source)
		{
	      	if (first)
				first = false;
	      	else
	        	s.Append(',');	//delimiter);
			
	      	s.Append(t);
	  	}    
	   	
		return s.ToString();
	}
}
