#define DEBUG

#if DEBUG
 #define USE_DEBUG_CONSOLE
#endif

using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

/// <summary>
/// Redmond Urbino
/// Debug console to let us enter commands at runtime
/// Deliberately not a mono behavior, see DebugConsoleGui
/// To add game specific commands, create a new csharp file declaring inheriting from  DebugConsole
/// and then define the methods in there with the signature of private static void myCommand(string param);
/// see DebugConsoleOz.cs for an example
/// </summary>
#if USE_DEBUG_CONSOLE
public abstract class DebugConsoleBase
{
	
}

public  class DebugConsole  {
	protected static Notify notify = new Notify("DebugConsole");
		
	public static bool showTextEdit = false;
	public static string typedString = "";
	public static GUIStyle textAreaStyle;
	public static GUIStyle buttonStyle;
	protected static HashSet<string> memberStringsToHide = new HashSet<string>();
	
	private static HashSet<string> availableCommands;
	private static HashSet<string> gameSpecificCommands;
	
	private static float plusWidth = 50;
	
	protected static DebugConsole _instance;
	public  static DebugConsole Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new DebugConsole();
				notify.Debug ("creating new instance of Debug Console");
			}
			notify.Debug ("DebugConsole.Instance returning " + _instance.ToString());
			return _instance;
		}
	}
	
	protected Type MyType()
	{
		return _instance.GetType();
	}
	
	/// <summary>
	/// Initializes the <see cref="DebugConsole"/> class. Makes sure memberStringsToHide is setup
	/// </summary> 
	static DebugConsole()
	{
		notify.Debug ("start of DebugConsole");
		textAreaStyle = new GUIStyle(GUI.skin.textArea);
		buttonStyle  = new GUIStyle(GUI.skin.button);
		
		// don't show some members when we type help
		memberStringsToHide.Add (".cctor");
		memberStringsToHide.Add ("notify");
		memberStringsToHide.Add ("memberStringsToHide");
		memberStringsToHide.Add ("_instance");
		memberStringsToHide.Add ("availableCommands");
		memberStringsToHide.Add ("gameSpecificCommands");
		memberStringsToHide.Add ("plusWidth");
		
		if (plusWidth < Screen.width * 0.08f )
		{
			plusWidth = Screen.width * 0.08f;	
		}
	}
	
	/// <summary>
	/// draw and get input for our debug console
	/// </summary>
	public static void OnGUI()
	{
		GUILayout.BeginArea(new Rect(0,0, Screen.width, Screen.height));
		if (showTextEdit)
		{
			typedString = GUILayout.TextArea(typedString, textAreaStyle, GUILayout.MinWidth(Screen.width), GUILayout.MinHeight(150));
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Close", buttonStyle, GUILayout.MinHeight(50), GUILayout.MinWidth(100)))
			{
				showTextEdit = false;	
			}
			if (GUILayout.Button("Clear", buttonStyle, GUILayout.MinHeight(50), GUILayout.MinWidth(100)))
			{
				typedString = "";	
			}
			if (GUILayout.Button("Run Command", buttonStyle, GUILayout.MinHeight(50)))
			{
				runCommand(typedString);
			}
		}
		else{
			if (GUILayout.Button("+", GUILayout.ExpandWidth(false), GUILayout.MinWidth(plusWidth), GUILayout.MinHeight(plusWidth)))
			{
				showTextEdit = true;	
			}
		}
		GUILayout.EndArea();

	}
	
	/// <summary>
	/// Runs the command.
	/// </summary>
	/// <param name='cmd'>
	/// the text the user typed
	/// </param> 
	public static void runCommand(string cmd)
	{
		if (cmd == "")
		{
			typedString = "try typing 'help'";
		}
		else
		{
			string trimmedLine = cmd.Trim();
			try
			{
				string [] splits = trimmedLine.Split (new Char [] {' '});
				if (splits.Length > 0)
				{
					// use c#'s InvokeMember assuming his first word is a method defined below
					Type calledType = Instance.MyType();
					notify.Debug ("calledType = " + calledType.Name);
					string loweredCommand = splits[0].ToLower();
					// for some reason calledType.GetMethod fails, hence using GetGameSpecificCommands
					if (GetGameSpecificCommands().Contains(loweredCommand))
					{
						notify.Debug ("found a method called " + loweredCommand);
						calledType.InvokeMember(loweredCommand, 
							BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy, null,null,
							new System.Object[] {trimmedLine});
					}
					else
					{
						notify.Debug ("Did not find  a method called " + loweredCommand);
						calledType = typeof(DebugConsole);
						calledType.InvokeMember(loweredCommand, 
							BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy, null,null,
							new System.Object[] {trimmedLine});
					}
				}
			}
			catch (Exception theException)
			{
				typedString = theException + " Exception caught.";	
			}
		}
	}
	

	public static HashSet<string> GetAvailableCommands()
	{
		if ( availableCommands == null)
		{
			availableCommands = new HashSet<string>();
			
			if (Instance.MyType() != typeof(DebugConsole))
			{
				// also add the cross game commands
				Type baseType = typeof (DebugConsole);
				MemberInfo [] baseMembers = baseType.GetMembers( BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy );
	
				foreach (MemberInfo info in baseMembers)
				{
					if (memberStringsToHide.Contains(info.Name) == false)
					{
						availableCommands.Add(info.Name);	
					}
				}
				
			}
			availableCommands.UnionWith( GetGameSpecificCommands());
		}
		return availableCommands;
	}
	
	public static HashSet<string> GetGameSpecificCommands()
	{
		if ( gameSpecificCommands == null)
		{
			gameSpecificCommands = new HashSet<string>();

			// these should list the game specific commands
			Type type = Instance.MyType();
			MemberInfo [] members = type.GetMembers( BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);
	
			foreach (MemberInfo info in members)
			{
				if (memberStringsToHide.Contains(info.Name) == false)
				{
					gameSpecificCommands.Add(info.Name);	
				}
			}
		}
		return gameSpecificCommands;
	}
	
	
	/// ------------------------ Put the cross game console commands in this section -------------------------------
	#region crossgame_commands
	
	private static bool help(string cmd)
	{
		typedString  = "commands available= " ;
		
		foreach (string command in GetAvailableCommands())
		{
			typedString  += command + " ";
		}
		
		return true;
		
	}
	
	/// <summary>
	/// Sbs Set a boolean setting
	/// </summary>
	/// <param name='cmd'>
	/// expected parameters are the name of the setting, followed by a space and then true or false
	/// </param>
	private static bool sbs(string cmd)
	{
		bool result = true;
		string [] splits = cmd.Split(' ');
		if ( splits.Length < 3)
		{
			typedString = "Set Boolean Setting usage : sbs <settingname> <true|false>";
			result = false;
		}
		else
		{
			string key = splits[1];
			string value = splits[2];
			if (value != "true" && value != "false")
			{
				typedString = "change " + value + " to either true or false";
				result = false;
			}
			else
			{
				bool newValue = false;
				Boolean.TryParse(value, out newValue);
				Settings.SetBool(key, newValue);
				typedString = "Setting " + key + " is now " + newValue;
			}
		}
		return result;
	}
	
	/// <summary>
	/// Sis Set a int setting
	/// </summary>
	/// <param name='cmd'>
	/// expected parameters are the name of the setting, followed by a space and then true or false
	/// </param>
	private static bool sis(string cmd)
	{
		bool result = true;
		string [] splits = cmd.Split(' ');
		if ( splits.Length < 3)
		{
			typedString = "Set Int Setting usage : sis <settingname> <int>";
			result = false;
		}
		else
		{
			string key = splits[1];
			string value = splits[2];

			int newValue = 0;
			if ( int.TryParse(value, out newValue))
			{
				Settings.SetInt(key, newValue);
				typedString = "Setting " + key + " is now " + newValue;
			}
			else
			{
				typedString = "Couldn't parse " + value + " as an int";	
			}
		}
		return result;
	}
	
	/// <summary>
	/// Sfs Set a float setting
	/// </summary>
	/// <param name='cmd'>
	/// expected parameters are the name of the setting, followed by a space and then true or false
	/// </param>
	private static bool sfs(string cmd)
	{
		bool result = true;
		string [] splits = cmd.Split(' ');
		if ( splits.Length < 3)
		{
			typedString = "Set Float Setting usage : sfs <settingname> <int>";
			result = false;
		}
		else
		{
			string key = splits[1];
			string value = splits[2];

			float newValue = 0;
			if ( float.TryParse(value, out newValue))
			{
				Settings.SetFloat(key, newValue);
				typedString = "Setting " + key + " is now " + newValue;
			}
			else
			{
				typedString = "Couldn't parse " + value + " as an float";	
			}
		}
		return result;
	}	

	/// <summary>
	/// Be able to change the time scale to 0 or 1
	/// </summary>
	/// <param name='cmd'>
	/// typically the parameters are either 0 or 1, but you could enter 0.5 for halfspeed
	/// </param> 
	/// 
	private static bool ts(string cmd)
	{
		bool result =true;
		string [] splits = cmd.Split(' ');
		if ( splits.Length < 2)
		{
			typedString = "Time Scale usage : ts <1|0>";
			result = false;
		}
		else
		{
			float newValue = 0;
			if (float.TryParse(splits[1], out newValue))
			{
				Time.timeScale = newValue;
			}
			else
			{
				typedString = "couldn't parse " + splits[1] + " as a float";
				result = false;
			}
		}
		return result;
		
	}
	
	/// <summary>
	/// force a garbage collect
	/// </summary>
	/// <param name='cmd'>
	/// unused
	/// </param>
	private static bool gc(string cmd)
	{
		Resources.UnloadUnusedAssets();
		System.GC.Collect();
		typedString = "called System.GC.Collect() and Resources.UnloadUnusedAssets()";
		return true;
	}
	
	/// <summary>
	/// turn on or turn off the DebugDirectionalLight
	/// </summary>
	/// <param name='cmd'>
	/// parameters should be either on or off
	/// </param>
	private static bool dl(string cmd)
	{
		const string objName = "DebugLight";
		bool result = true;
		string [] splits = cmd.Split(' ');
		if ( splits.Length < 2)
		{
			typedString = "Directional Light usage : dl  <on|off>";
			result = false;
		}
		else
		{
			if (splits[1] == "on")
			{
				GameObject dlObj = GameObject.Find (objName);
				if (dlObj)
				{
					Light dl = dlObj.GetComponent<Light>();
					dl.enabled = true;
				}
				else
				{
					dlObj = new GameObject(objName);
					dlObj.transform.localEulerAngles = new Vector3(50, -30, 0);
					Light dl = dlObj.AddComponent<Light>();
					dl.type = LightType.Directional;
					dl.intensity = 0.5f;
				}
			}
			else if (splits[1] == "off")
			{
				GameObject dlObj = GameObject.Find (objName);
				if (dlObj)
				{
					Light dl = dlObj.GetComponent<Light>();
					dl.enabled = false;
				}
				else
				{
					typedString = "No DebugLight to turn off";
				}				
			}
			else
			{
				typedString = "parameter must be 'on' or 'off' ";
				result = false;
			}	
		}
		return result;
	}
	
	
	public static bool DebugDirectionalLight(bool on)
	{
		bool result;
		if (on)
		{
			result = dl ("dl on");
		}
		else
		{
			result = dl ("dl off");
		}
		return result;
	}
	#endregion
	
}
#else
public partial class DebugConsole 
{
	public static void ONGUI(){}
}
#endif
