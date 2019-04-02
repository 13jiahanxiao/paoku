using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

public enum DeathTypes
{
	Unknown = -1,
	Fall,
	Eaten,
	Fire,
	Mine, //<--DUCK
	MineLedge,
	Fence,
	Wheel,
	WaterFall,
	Ledge,
	SceneryTree,
	SceneryRock,
	Baboon,
}
	
[Serializable]
public class ProtoDeathMessage
{
	public string 			spriteName = null;
	public Dictionary<string, object> characters = null;
	public List<string> 	messageChoices = null;
	public DeathTypes		deathType = DeathTypes.Fall;
	
	public string ToJson() {
		Dictionary<string, object> d = ToDict();
		return MiniJSON.Json.Serialize(d);
	}
	
	public Dictionary<string, object> ToDict() {
		Dictionary<string, object> d = new Dictionary<string, object>();
		d.Add ("spriteName", spriteName);
		d.Add ("messageChoices", messageChoices);
		d.Add ("characters", characters);
		d.Add ("deathtype", deathType.ToString());
		return d;
	}
	
	public ProtoDeathMessage() {
		spriteName = "";
		messageChoices = new List<string>();
	}
	
	public string getRandomCharacterMessage(int characterID) {
		string characterKey = characterID.ToString();
		if(characters.ContainsKey(characterKey)) {
			Dictionary<string, object> foo = characters[characterKey] as Dictionary<string, object>;
			if(foo != null) {
				if(foo.ContainsKey("msgs")) {
					List<object> msgs = foo["msgs"] as List<object>;
					if(msgs == null || msgs.Count == 0)
						return null;
					
					return msgs[UnityEngine.Random.Range(0, msgs.Count)] as string;
				}
			}
		}
		return null;
	}
	
	public string getCharacterSpriteName(int characterID) {
		string characterKey = characterID.ToString();
		if(characters.ContainsKey(characterKey)) {
			Dictionary<string, object> foo = characters[characterKey] as Dictionary<string, object>;
			if(foo != null) {
				if(foo.ContainsKey("spriteName")) {
					return foo["spriteName"] as string;
				}
			}
		}
		return null;
	}
	
	public ProtoDeathMessage(Dictionary<string, object> dict) {
		spriteName = "";
		if(dict.ContainsKey("spriteName")) {
			spriteName = (string)dict["spriteName"];	
		}
		
		if(dict.ContainsKey("characters")) {
			characters = dict["characters"] as Dictionary<string, object>;	
		}
		
		if(dict.ContainsKey("messageChoices")) {
			messageChoices = new List<string>();
			IList tempList = dict["messageChoices"] as IList;
			foreach (var item in tempList) {
				messageChoices.Add ((string)item);
			}
		}
		deathType = DeathTypes.Fall;
		if(dict.ContainsKey("deathtype")) {
			if(Enum.IsDefined(typeof(DeathTypes), dict["deathtype"])) {
				deathType = (DeathTypes)Enum.Parse(typeof(DeathTypes), (string)dict["deathtype"]);
			}
		}
	}
}


public class DeathMessage {
	protected static Notify notify= new Notify("DeathMessage");
	public static List<ProtoDeathMessage> Messages = null;
	
	public static bool LoadFile()
	{
		string fileName = "TRGameData/DeathMessages";
		TextAsset artText = Resources.Load(fileName) as TextAsset;
		if(artText == null)
		{
			notify.Warning("No DeathMessages.txt exists at: " + fileName);	
			return false;
		}
		if(Messages == null) {
			Messages = new List<ProtoDeathMessage>();
		}
		else {
			Messages.Clear();
		}
		
		//notify.Debug("DeathMessage " + artText.text);
		Dictionary<string, object> loadedData = MiniJSON.Json.Deserialize(artText.text) as Dictionary<string, object>;
		List<object> secureData = loadedData["data"] as List<object>;
		
		bool success = DeseralizeVersion1(secureData);
		
		//-- Failed to load, what to do?
		if(success == false) {
		}
		return success;
	}
	
	private static bool DeseralizeVersion1(List<object> loadedData) {
		bool success = true;
		foreach (object item in loadedData) {
			Dictionary<string, object> data = item as Dictionary<string, object>;
			Messages.Add ( new ProtoDeathMessage(data));
		}
		return success;
	}
	
	public static void SaveFile()
	{
#if UNITY_EDITOR		
		using (MemoryStream stream = new MemoryStream()) {
			string fileName = Application.dataPath + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar + "TRGameData/DeathMessages.txt";
			List<object> store = new List<object>();
			foreach(ProtoDeathMessage data in Messages) {
				store.Add(data.ToDict());
			}
			
			//-- Hash before we save.
			Dictionary<string, object> secureData = SaveLoad.Save(store);
			string storeString = MiniJSON.Json.Serialize(secureData);
			
			try {
				using (StreamWriter fileWriter = File.CreateText(fileName)) {
					fileWriter.WriteLine(storeString);
					fileWriter.Close(); 
				}
			}
			catch (Exception e) {
				Dictionary<string,string> d = new Dictionary<string, string>();
				d.Add("Exception",e.ToString());
				notify.Warning("Save Exception: " + e);
			}
		}
#endif
	}
	
	
	static public ProtoDeathMessage GetRandomMessageForDeathType(DeathTypes death) {
		if(DeathMessage.Messages == null)
			return null;
		
//		string charName = null;
//		int activeCharID = GameProfile.SharedInstance.GetActiveCharacter().characterId;
//		switch(activeCharID) {
//			case 0:
//				charName = "Guy_";
//				break;
//			case 1:
//				charName = "Scarlett_";
//				break;
//			case 2:
//				charName = "Barry_";
//				break;
//			case 3:
//				charName = "Karma_";
//				break;
//		}
		
		List<ProtoDeathMessage> specificFound = new List<ProtoDeathMessage>();
		foreach (ProtoDeathMessage item in Messages) {
			if(item == null)
				continue;
			if(item.deathType != death)
				continue;
			specificFound.Add (item);
		}
		
		if(specificFound.Count == 0) {
			return null;
		}
		return specificFound[UnityEngine.Random.Range(0, specificFound.Count)];
	}
}
