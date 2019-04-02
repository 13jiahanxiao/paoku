using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;



/// <summary>
/// All the data needed for one environment set
/// all public members will be saved, key for json is the member name
/// </summary>
public class EnvironmentSetData 
{
	protected static Notify  notify = new Notify("EnvironmentSetData");
	/// <summary>
	/// The environment set identifier, e.g. 0 for machu, 1 for ww, 2 for df, 3 for ybr
	/// </summary>
	public int SetId = -1;
	
	/// <summary>
	/// short string abbreviation of the Title, e.g. ww, ybr df
	/// </summary>
	public string SetCode = "";	
	
	/// <summary>
	/// the full human readable name of this environment set
	/// </summary>
	public string Title = "";	
	private string localizedTitle = "";	

	/// <summary>
	/// The path to the material to apply for the track piece when it is near.
	/// </summary>
	public string OpaqueMaterialPath = "";
	
	/// <summary>
	/// The path to the material to apply for the track piece when it is far. The farther away the more fade out it is
	/// </summary>
	public string FadeOutMaterialPath = "";
	
	/// <summary>
	/// instead of the opaque material, apply this material instead for decals (e.g. leaves, fences)
	/// </summary>
	public string DecalMaterialPath = "";
	
	/// <summary>
	/// The path to the material to apply for the track piece when it is far. The farther away the more fade out it is
	/// </summary>
	public string DecalFadeOutMaterialPath = "";

	/// <summary>
	/// Extra material to swap on objects with extra1 in their name
	/// </summary>
	public string Extra1MaterialPath = "";	

	/// <summary>
	/// Extra material to swap on objects with extra1_fade in their name
	/// </summary>
	public string Extra1FadeMaterialPath = "";		

	/// <summary>
	/// Extra material to swap on objects with extra2 in their name
	/// </summary>
	public string Extra2MaterialPath = "";	

	/// <summary>
	/// Extra material to swap on objects with extra2_fade in their name
	/// </summary>
	public string Extra2FadeMaterialPath = "";			
	
	/// <summary>
	/// The skybox prefab we load for this environment set
	/// </summary>
	public string SkyboxPrefabPath = "";
	
	/// <summary>
	/// The opening tile prefab we load for this environment set
	/// </summary>
	public string OpeningTilePrefabPath = "";
	
	/// <summary>
	/// The music file we load for this environment set
	/// </summary>
	public string MusicFile = "";
	public string FootstepFile1 = "";
	public string FootstepFile2 = "";
	public string FootstepFile3 = "";
	public string FootstepFile4 = "";
	public string FootstepFile5 = "";
	
	
	public int hardSurfaceEnv0;
	public int hardSurfaceEnv1;
	public int hardSurfaceEnv2;
	
	//Env 0 distances
	public float Env0MinLength = 200;
	public float Env0MaxLength = 350;
	
	/// <summary>
	/// Used to be called ForestMinLength, minimum distance you'll run in Env 1
	/// </summary>
	public float  Env1MinLength = 250;
	/// <summary>
	/// hen we go over this distance, we'll force the env1 end piece
	/// </summary>
	public float  Env1MaxLength = 350;
	/// <summary>
	/// Used to be called MineMinLength, minimum distance you'll run in Env 2
	/// </summary>
	public float  Env2MinLength = 120;
	/// <summary>
	/// When we go over this distance, we'll force the env2 end piece
	/// </summary>
	public float  Env2MaxLength = 220;
	/// <summary>
	/// what distance do we fly in the clouds
	/// </summary>
	public float  BalloonMinLength = 10000;
	
	public float Env9MinLength { get 
		{ return BalloonMinLength;}}
	
	/// <summary>
	/// If we are this far from the tunnel entrance, don't allow new environment sets
	/// </summary>
	public float TunnelBufferDistance { get; private set;}
	
	public float DistanceToWW = 1000f;
	public float DistanceToDF = 1000f;
	public float DistanceToYBR = 1000f;
	
	public string LandingSoundEffect = "";
	public string JumpingSoundEffect = "";
	public string SlidingSoundEffect = "";
	public string TurnRightSoundEffect = "";
	public string TurnLeftSoundEffect = "";
	public string FailPostBalloonPiece = "";
	/// <summary>
	/// The number of post ballon fail pieces. really long fail pieces can bring this number down, ybr is at 2
	/// </summary>
	public int	  NumberOfPostBallonFailPieces = 4;
	
	// TODO move out PopuplateWWPieces, PopulateYBRPieces, PopulateDFPieces, and put the data to create them here

	/// <summary>
	/// Sets the data from a dictionary. the dictionary is probably coming from a json object
	/// </summary>
	/// <param name='data'>
	/// the dictionary.
	/// </param>
	public void SetDataFromDictionary(Dictionary<string, object> data) 
	{
		if (data == null)
		{
			notify.Warning("EnvironmentSetData.SetDataFromDictionary dictionary is null");
			return;
		}
		
        SerializationUtils.SetDataFromDictionary( this, data);
		TunnelBufferDistance = ComputeTunnelBufferDistance();
	}	
	
	/// <summary>
	/// returns a dictionary representation of this object
	/// </summary>
	/// <returns>
	/// The dict.
	/// </returns>
	public Dictionary<string, object> ToDict()
	{
		return SerializationUtils.ToDict( this); 
	}
	
	/// <summary>
	/// Stub for when localization actually works
	/// </summary> 
	public string GetLocalizedTitle()
	{
		return localizedTitle;//Localization.SharedInstance.Get(Title);
	}
	
	private void SetLocalizedTitle()
	{
		localizedTitle = Localization.SharedInstance.Get(Title);
	}

	public void OnLanguageChanged(string language)
	{
		SetLocalizedTitle();// = Localization.SharedInstance.Get(newData.Title);
	}
	
	/// <summary>
	/// Validates the values, cross references with bootstrap info, checks resources are theres
	/// </summary>
	/// <returns>
	/// true if all good, false otherwise, console log will have more info on the errors
	/// </returns>
	public bool ValidateValues()
	{
		// normally I hate returns in the middle of a function,  but this makes a little more
		// sense given the amount of checks we make
		if (SetId < 0)
		{
			notify.Warning("SetId must be 0 or greater");
			return false;	
		}
		
		if (SetCode == "")
		{
			notify.Warning("SetCode must not be blank");	
			return false;
		}
		
		/// try to load bootstrap info and verify the setcode is there as well
		bool bootStrapLoaded = EnvironmentSetBootstrap.LoadFile();
		if (!bootStrapLoaded)
		{
			notify.Warning("Could not load the EnvironmentSet Bootstrap data file");
			return false;
		}
		
		// Todo there should be a way to do the for loop with .Contains()
		bool found = false;
		foreach( EnvironmentSetBootstrapData tempBootstrap in EnvironmentSetBootstrap.BootstrapList)
		{
			if (tempBootstrap.SetCode == SetCode)
			{
				found = true;
				break;
			}
		}
		if ( ! found)
		{
			notify.Warning("The Environment Set Bootstrap data file does not contain the SetCode " + SetCode);
			return false;
		}
		
		if ( Title == "")
		{
			localizedTitle = "<not set>";
			notify.Warning("Title must not be blank");
			return false;
		}

		// because we added sufffixes at runtime to these paths, this check is no longer valid
		//string []  paths= { OpaqueMaterialPath, FadeOutMaterialPath, DecalMaterialPath};
		string [] paths = {};
		
		foreach ( string path in paths)
		{
			if (path == "")
			{
				notify.Warning("path must not be blank");	
				return false;
			}
			
			Material testMat = Resources.Load(path) as Material;
			if (testMat == null)
			{
				notify.Warning("could not load the material at " + path);
				return false;
			}
		}
		
		GameObject prefab = Resources.Load(SkyboxPrefabPath) as GameObject;
		if (prefab == null)
		{
			notify.Warning("could not load skybox prefab at " + SkyboxPrefabPath);
			return false;
		}
		
		prefab = Resources.Load(OpeningTilePrefabPath) as GameObject;
		if (prefab == null)
		{
			notify.Warning("could not opening tile prefab at " + OpeningTilePrefabPath);
			return false;
		}
		
		return true;
	}
	
	/// <summary>
	/// Saves our info in the appropriate place, the filename is the SetCode followed by .txt
	/// </summary>
	/// <returns>
	/// true if the file was saved successfully
	/// </returns>
	public bool SaveFile()
	{
		bool result = true;
		string fullPath = Application.dataPath + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar + 
			EnvironmentSetBootstrap.EnvironmentSetResourcesDirectory + Path.DirectorySeparatorChar + SetCode + ".txt";
		notify.Info ("saving to " + fullPath);
		string listString = SerializationUtils.ToJson(this);
		try 
		{
			using (StreamWriter fileWriter = File.CreateText(fullPath))
			{ 
				fileWriter.WriteLine(listString); 
				fileWriter.Close(); 
			}
		}
		catch (System.Exception e) 
		{
			result = false;
			notify.Warning("Save Exception: " + e);
		}
		return result;
	}
	
	/// <summary>
	/// Tries to load the environment set data from a file.
	/// </summary>
	/// <returns>
	/// true if we loaded successfully
	/// </returns>
	/// <param name='setCode'>
	/// the set code which would correspond to the file name as well
	/// </param>
	/// <param name='loadedObject'>
	/// will be null if there was an error,  otherwise should contain valid data
	/// </param>
	public static bool LoadFile(string setCode, out EnvironmentSetData loadedObject, bool validate = false)
	{
		bool result = true;
		loadedObject = null;
		EnvironmentSetData newData = new EnvironmentSetData();
		try
		{
			string resourcePath = EnvironmentSetBootstrap.EnvironmentSetResourcesDirectory + "/" + setCode;
			notify.Debug("loading " + resourcePath);
			TextAsset jsonText = Resources.Load(resourcePath) as TextAsset;
			notify.Debug("EnvironmentSetData " + jsonText.text);
			
			bool deserialized = SerializationUtils.FromJson( newData, jsonText.text);
			if ( ! deserialized)
			{
				notify.Warning("EnvironmentSetData.LoadFile, could not parse {0}", jsonText.text);	
			}
			if (validate)
			{
				bool valid = newData.ValidateValues();
				result = valid;
			}
			Localization.RegisterForOnLanguageChanged(newData.OnLanguageChanged);
			newData.SetLocalizedTitle();
			loadedObject = newData;		
			
		}
		catch (System.Exception e) 
		{
			result = false;
			notify.Error("Load Exception: " + e);			
		}
		return result;
	}
	
	/// <summary>
	/// If we are this far from the tunnel entrance, don't allow new environments to spawn
	/// </summary>
	/// <returns>
	/// The tunnel buffer distance.
	/// </returns>
	public float ComputeTunnelBufferDistance()
	{
		float bufferDistance = 50f;
		
		bufferDistance = Mathf.Max( bufferDistance, this.Env0MaxLength);
		bufferDistance = Mathf.Max( bufferDistance, this.Env1MaxLength);
		bufferDistance = Mathf.Max ( bufferDistance, this.Env2MaxLength);
		// then add just a little bit more to be safe
		bufferDistance += 50f;
		return bufferDistance;
	}
	
	/// <summary>
	/// If true, the environment set is an assetbundle saved under Assets/StreamingAssets
	/// if false, then the environment set could be under Resources/Prefabs/Temple/Environments
	///           or a real asset bundle that we download
	/// </summary>
	public bool IsEmbeddedAssetBundle()
	{
		bool result = false;
		foreach( EnvironmentSetBootstrapData tempBootstrap in EnvironmentSetBootstrap.BootstrapList)
		{
			if (tempBootstrap.SetCode == SetCode)
			{
				result = tempBootstrap.EmbeddedAssetBundle;
				break;
			}
		}	
		return result;
	}
	
	/// <summary>
	/// If true, the environment set prefabs are saved under Resources/Prefabs/Temple/Environments
	/// if false, then the environment set could be an assetbundle under Assets/StreamingAssets
	///           or a real asset bundle that we download
	/// </summary>
	public bool IsEmbeddedAsResource()
	{
		bool result = false;
		foreach( EnvironmentSetBootstrapData tempBootstrap in EnvironmentSetBootstrap.BootstrapList)
		{
			if (tempBootstrap.SetCode == SetCode)
			{
				result = tempBootstrap.Embedded && ! tempBootstrap.EmbeddedAssetBundle;
				break;
			}
		}	
		return result;
	}
}
