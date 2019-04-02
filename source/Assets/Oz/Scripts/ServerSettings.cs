using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class ServerSettings : MonoBehaviour
{
	
	private static Dictionary<string, bool> _boolDictionary = new Dictionary<string, bool>();
	private static Dictionary<string, int>  _intDictionary = new Dictionary<string, int>();
	private static Dictionary<string, float> _floatDictionary = new Dictionary<string, float>();
	private static Dictionary<string, string> _stringDictionary = new Dictionary<string, string>();
	
	protected static Notify notify;
	
	/// <summary>
	/// Apply the server settings to what the rest of the game normally accesses, e.g. Settings.GetInt("key", 1);
	/// </summary>
	private void _applyToSettings()
	{
		/// note that some of these may not take if we have keys that trump it in local-settings.txt
		foreach ( KeyValuePair<string, bool> pair in _boolDictionary)
		{
			Settings.SetBool(pair.Key, pair.Value, Settings.SourcePriority.GameServer);	
		}
		foreach ( KeyValuePair<string, int> pair in _intDictionary)
		{
			Settings.SetInt(pair.Key, pair.Value, Settings.SourcePriority.GameServer);	
		}
		foreach ( KeyValuePair<string, float> pair in _floatDictionary)
		{
			Settings.SetFloat(pair.Key, pair.Value, Settings.SourcePriority.GameServer);	
		}
		foreach ( KeyValuePair<string, string> pair in _stringDictionary)
		{
			Settings.SetString(pair.Key, pair.Value, Settings.SourcePriority.GameServer);	
		}
	}
	
	private void _loadLocalCopyServerSettings()
	{
		string fileName = Application.persistentDataPath + Path.DirectorySeparatorChar + "server-settings.txt";
		
		if ( File.Exists( fileName ) == false )
		{
			notify.Debug( "[ServerSettings] - _loadLocalCopyServerSettings: 'server-settings.txt' not found." );
			return;
		}
		
		StreamReader fileReader = File.OpenText( fileName );
		string settingsJsonString = fileReader.ReadToEnd();
		fileReader.Close();

		notify.Debug( "[ServerSettings] - _loadLocalCopyServerSettings: attempting to deserialize." );
		
		try 
		{
			Dictionary<string, object> loadedDataDictionary 
				= MiniJSON.Json.Deserialize( settingsJsonString ) as Dictionary<string, object>;
			
			if ( loadedDataDictionary != null )
			{
				if ( SaveLoad.Load( loadedDataDictionary ) == false )
				{
					return;
				}
				
				Dictionary<string, object> dataDictionary = loadedDataDictionary["data"] 
					as Dictionary<string, object>;
				
				if ( dataDictionary == null )
				{
					return;
				}
				
				Dictionary<string, object> boolSettingDictionary = dataDictionary["boolSettings"]
					as Dictionary<string, object>;
				
				Dictionary<string, object> intSettingsDictionary = dataDictionary["intSettings"]
					as Dictionary<string, object>;
				
				Dictionary<string, object> floatSettingsDictionary = dataDictionary["floatSettings"]
					as Dictionary<string, object>;
				
				Dictionary<string, object> stringSettingsDictionary = dataDictionary["stringSettings"]
					as Dictionary<string, object>;
				
				if ( boolSettingDictionary != null )
				{
					_boolDictionary.Clear();
					
					foreach ( KeyValuePair<string, object> boolKVP in boolSettingDictionary )
					{
						_boolDictionary.Add( boolKVP.Key, bool.Parse( boolKVP.Value.ToString() ) );
					}
				}
				
				if ( intSettingsDictionary != null )
				{
					_intDictionary.Clear();
					
					foreach ( KeyValuePair<string, object> intKVP in intSettingsDictionary )
					{
						_intDictionary.Add( intKVP.Key, int.Parse( intKVP.Value.ToString() ) );
					}
				}
				
				if ( floatSettingsDictionary != null )
				{
					_floatDictionary.Clear();
					
					foreach ( KeyValuePair<string, object> floatKVP in floatSettingsDictionary )
					{
						_floatDictionary.Add( floatKVP.Key, float.Parse( floatKVP.Value.ToString() ) );
					}
				}
				
				if ( stringSettingsDictionary != null )
				{	
					_stringDictionary.Clear();
					
					foreach ( KeyValuePair<string, object> stringKVP in stringSettingsDictionary )
					{
						_stringDictionary.Add( stringKVP.Key, stringKVP.ToString() );
					}
				}
			}
		}
		catch ( Exception ex )
		{
			notify.Warning( "[ServerSettings] - _loadLocalCopyServerSettings: Error Deserializing - " + ex.Message );
		}
	}
	
	public void ApplyServerSettingsLocalCopy ()
	{
		_loadLocalCopyServerSettings();
		_applyToSettings();
	}
	
	private void _saveLocalCopyServerSettings()
	{
		using ( MemoryStream stream = new MemoryStream() )
		{
			string fileName = Application.persistentDataPath + Path.DirectorySeparatorChar + "server-settings.txt";
			
			Dictionary<string, object> settingsDataDictionary = new Dictionary<string, object>();
			
			settingsDataDictionary.Add( "boolSettings", _boolDictionary );
			settingsDataDictionary.Add( "intSettings", _intDictionary );
			settingsDataDictionary.Add( "floatSettings", _floatDictionary );
			settingsDataDictionary.Add( "stringSettings", _stringDictionary );
			
			Dictionary<string, object> secureDataDictionary = SaveLoad.Save( settingsDataDictionary );
			
			string settingsJsonString = MiniJSON.Json.Serialize( secureDataDictionary );
			
			try
			{
				using ( StreamWriter fileWriter = File.CreateText( fileName ) )
				{
					fileWriter.WriteLine( settingsJsonString );
					fileWriter.Close();
				}
			}
			catch ( Exception ex )
			{
				notify.Error( "[ServerSettings] - _saveLocalCopyServerSettings: Error Saving " + ex.Message );
			}
			
		}
	}
	
	// Use this for initialization
	void Start ()
	{
		notify = new Notify(this.GetType().Name);
	}
	
	/// <summary>
	/// Apply server settings parameters to internal dictionaries.
	/// </summary>
	/// <param name='serverSettingsList'>
	/// Server settings list.
	/// </param>
	/// <param name='responseCode'>
	/// Web response code.
	/// </param>
	public void ApplyServerSettings( List<object> serverSettingsList, int responseCode )
	{
		if ( responseCode == 200 )
		{
			notify.Debug( "[ServerSettings] - ApplyServerSettings: Got Response" );
			
			// Clear Dictionaries in memory.
			_boolDictionary.Clear();
			_intDictionary.Clear();
			_floatDictionary.Clear();
			_stringDictionary.Clear();
			
			if ( serverSettingsList != null && serverSettingsList.Count > 0 )
			{
				notify.Debug( "[ServerSettings] - ApplyServerSettings: Settings List not null" );
				
				foreach ( object settingsObject in serverSettingsList )
				{
					notify.Debug( "[ServerSettings] - ApplyServerSettings: Looping through objects" );
						
					try 
					{
						Dictionary<string, object> settingsDictionary = settingsObject as Dictionary<string, object>;
						
						notify.Debug( "[ServerSettings] - ApplyServerSettings: Attempting to Deserialize" );
					
						if ( settingsDictionary != null 
							&& settingsDictionary.ContainsKey( "type" )  
							&& settingsDictionary.ContainsKey( "name" ) 
							&& settingsDictionary.ContainsKey( "value" )
						) {
							string settingType = settingsDictionary["type"].ToString();
							string settingName = settingsDictionary["name"].ToString();
							
							switch ( settingType )
							{
								case "Boolean":
									bool boolValue = bool.Parse( settingsDictionary["value"].ToString() );
									_boolDictionary.Add( settingName, boolValue );
									break;
								
								case "Int":
									int intValue = int.Parse( settingsDictionary["value"].ToString() );
									_intDictionary.Add( settingName, intValue );
									break;
								
								case "Float":
									float floatValue = float.Parse( settingsDictionary["value"].ToString() );
									_floatDictionary.Add( settingName, floatValue );
									break;
								
								case "String":
									_stringDictionary.Add( settingName, settingsDictionary["value"].ToString() );
									break;
							}
							
							notify.Debug( "[ServerSettings] ApplyServerSettings - Setting '" + settingName
								+ "' with value: " + settingsDictionary["value"].ToString() );
						}
					}
					catch ( Exception ex )
					{
						notify.Error( "[ServerSettings] ApplyServerSettings - Deserializing Error: " + ex.Message );
					}
				}
				
				_saveLocalCopyServerSettings();
				_applyToSettings();
			}
		}
	}
	
	/*
	// Update is called once per frame
	void Update ()
	{
	
	}
	*/
}

