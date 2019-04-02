using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;




public class BalanceSheet : EditorWindow
{
	
	public static BalanceSheet instance;

	[MenuItem("Window/TR Oz/BalanceSheet")]
	public static void Init()
	{
        EditorWindow.GetWindow(typeof(BalanceSheet), false);
	}
	
	private static Vector2 location = Vector2.zero;
	
	private static string toAdd = "";
	
	
		
	const float firstcolumnwidth = 200f;
	const float columnwidth = 120f;
	const float firstrowheight = 50f;
	const float rowheight = 45f;
	
	
	private Texture2D tex;
	
	private string currentTab = "";
	
	
	void OnGUI()
	{
		
		BalanceData data = BalanceData.GetDataForEnvSet(currentTab);
		
		EditorGUILayout.BeginHorizontal();
		
		if(GUILayout.Button("default"))
			currentTab = "";
		if(GUILayout.Button("WhimsyWoods"))
			currentTab = "ww";
		if(GUILayout.Button("DarkForest"))
			currentTab = "df";
		if(GUILayout.Button("YellowBrickRoad"))
			currentTab = "ybr";
		if(GUILayout.Button("EmeraldCity"))
			currentTab = "ec";
		
		
		EditorGUILayout.EndHorizontal();
		
		
		GUILayout.Label("Current: "+currentTab);
		
		
		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal(GUILayout.Height(firstrowheight));
		GUILayout.Label(" ",GUILayout.Width(firstcolumnwidth-5f));
		
			EditorGUILayout.BeginScrollView(new Vector2(location.x,0f),false,false);
			
				EditorGUILayout.BeginHorizontal();
					for(int i=0;i<data.BalanceInfo.Count;i++)
					{
						EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb,GUILayout.Width(columnwidth-5));
						EditorGUILayout.BeginHorizontal();
						if(GUILayout.Button("X",GUILayout.Width(20)))
						{
							data.BalanceInfo.RemoveAt(i);
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.EndVertical();
							break;
						}
						data.BalanceInfo[i].Distance = EditorGUILayout.FloatField(data.BalanceInfo[i].Distance);
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.EndVertical();
					}
					
					if(GUILayout.Button("Add",GUILayout.Width(columnwidth)))
					{
						data.BalanceInfo.Add(new BalanceState());
					}
				EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.EndScrollView();
		
		EditorGUILayout.EndHorizontal();
		
		
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginScrollView(new Vector2(0f,location.y),false,true,GUILayout.Width(firstcolumnwidth));
			
				BalanceState prototype = BalanceData._GetMainUnsafe.BalanceSheetPrototype;//data.BalanceSheetPrototype;
				
				//for(int i=0;i<prototype.StateParameters.Count;i++)
				foreach(string key in prototype.StateParamKeys)
				{
					//string key = prototype.StateParameters.Keys[i];
					EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb,GUILayout.Width(firstcolumnwidth-25f),GUILayout.Height(rowheight));
					EditorGUILayout.BeginVertical();
						prototype[key].Identifier = GUILayout.TextField(prototype[key].Identifier);
						prototype[key].IsBool = GUILayout.Toggle(prototype[key].IsBool,"Is Bool?");
					EditorGUILayout.EndVertical();
					EditorGUILayout.EndHorizontal();
					
					if(key!=prototype[key].Identifier)
					{
						if(prototype.ContainsKey(prototype[key].Identifier))
						{
							prototype[key].Identifier = key;
						}
						else
						{
							for(int i=0;i<data.BalanceInfo.Count;i++)
							{
								data.BalanceInfo[i].Add(prototype[key].Identifier,data.BalanceInfo[i][key]);
								data.BalanceInfo[i].Remove(key);
							}
							prototype.Insert(prototype[key].Identifier,prototype[key],prototype.StateParamKeys.IndexOf(key));
							prototype.Remove(key);
						}
						return;
					}
				}
		
				if(GUILayout.Button("Add Field",GUILayout.Height(rowheight-20),GUILayout.Width(columnwidth)) && toAdd!="" && !prototype.ContainsKey(toAdd))
				{
					prototype.Add(toAdd,new BalanceParameter(toAdd,0f,false));
					toAdd = "";
				}
				toAdd = GUILayout.TextField(toAdd,GUILayout.Width(firstcolumnwidth-25f),GUILayout.Height(20));
			EditorGUILayout.EndScrollView();
		
			location = EditorGUILayout.BeginScrollView(location,true,true);
				
				//for(int i=0;i<prototype.StateParameters.Count;i++)
				foreach(string key in prototype.StateParamKeys)
				{
					string parameter = prototype[key].Identifier;
					bool isBool = prototype[key].IsBool;
			
					EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb,GUILayout.Height(rowheight));
			
						//Find the max and min values for this attribute!
						float min = 9999999;
						float max = -9999999;
						for(int j=0;j<data.BalanceInfo.Count;j++)
						{
							if(!data.BalanceInfo[j].ContainsKey(parameter))
							{
								data.BalanceInfo[j].Add(parameter,new BalanceParameter(parameter,0f,isBool));
							}
							float val = data.BalanceInfo[j][parameter].Value;
							if(val<min)	min = val;
							if(val>max) max = val;
						}
			
						for(int j=0;j<data.BalanceInfo.Count;j++)
						{
			
							EditorGUILayout.BeginHorizontal(GUILayout.Width(columnwidth-1),GUILayout.Height(rowheight-8));
							EditorGUILayout.BeginVertical();
								if(!data.BalanceInfo[j].ContainsKey(parameter))
								{
									data.BalanceInfo[j].Add(parameter,new BalanceParameter(parameter,0f,isBool));
								}
								
								BalanceParameter param = data.BalanceInfo[j][parameter];
								
								param.IsBool = isBool;
								EditorGUILayout.BeginHorizontal();
								if(isBool)
									param.BoolValue = GUILayout.Toggle(param.BoolValue,"true?");
								else {
									param.Value = EditorGUILayout.FloatField(param.Value);
									GUILayout.Label("m",GUILayout.Width(16));
								}
								EditorGUILayout.EndHorizontal();
								
								if(tex==null)	tex = new Texture2D(2,2);
								float t = Mathf.InverseLerp(min,max,param.Value);
								Color col;
								if(param.IsBool)
									col = param.BoolValue ? Color.green : Color.red;
								else
									col = param.Value< 0.5f ? Color.Lerp(Color.green,Color.yellow,t*2f) : Color.Lerp(Color.yellow,Color.red,t*2f-1f);
								col *= 0.75f;
								tex.SetPixel(0,0,col);
								tex.SetPixel(0,1,col);
								tex.SetPixel(1,0,col);
								tex.SetPixel(1,1,col);
								tex.Apply();
								//Rect r = GUILayoutUtility.GetRect(60f,10f,GUILayout.Width(60f));
								//GUI.DrawTexture(r,tex);
				
							EditorGUILayout.EndVertical();
							EditorGUILayout.EndHorizontal();
						}
						//for space
						GUILayout.Label(" ",GUILayout.Width(columnwidth));
					
					EditorGUILayout.EndHorizontal();
				}
				//For space
				GUILayout.Label(" ",GUILayout.Height(rowheight));
		
			EditorGUILayout.EndScrollView();
		EditorGUILayout.EndHorizontal();
		
		if(GUI.changed)
		{
			EditorUtility.SetDirty(data);
		}
			
		
	}
	
	
	
}
