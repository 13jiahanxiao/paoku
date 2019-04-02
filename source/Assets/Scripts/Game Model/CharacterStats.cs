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

public enum CostType
{
	Coin = 0,
	Special = 1,
	RealMoney = 2,
	GetCoinsBack = 3,
	Total
}

public enum BuffType
{
	Powerup = 0,
	Artifact = 1,
	
	Total
}

public enum GearSlotType
{
	kGearSlotSkin = 0,
	kGearSlotHat = 1,
	kGearSlotBelt = 2,
	kGearSlotShoes = 3,
	kGearSlotCount
}

public enum ArtifactSlotType
{
	One = 0,
	Two = 1,
	Three = 2,
	Total
}



[Serializable]
public class CharacterStats
{
	public static int Version = 1;
	public string displayName;
	public int characterId;
	public List<int> gearItems;
	public bool unlocked;
	public int unlockCost;
	public CostType unlockCostType;
	public int protoVisualIndex;
	public List<int> artifactItems;
	public int powerID;
	public string IconName = "";
	public float colliderRadius = 1f;
	public string deathImagePrefix = "";	
	
	public CharacterSounds sounds;
	
	[NonSerialized]
	public int ServerCost = 0;
	
	public int SortPriority { get; set; }
	
	public int DefaultSortPriority { get; set; }
	
	public int DiscountCost
	{
		get
		{
			return ServerCost;
		}
	}
	
	public int GetFinalCost()
	{
		if ( ServerCost > 0 )
		{
			return ServerCost;
		}
		else
		{
			return unlockCost;
		}
	}
	
	public CharacterStats(int newCharacterId)
	{
		gearItems = new List<int>((int)GearSlotType.kGearSlotCount);
		artifactItems = new List<int>((int)ArtifactSlotType.Total);
		
		characterId = newCharacterId;
		Reset();
	}
	
	public CharacterStats(Dictionary<string, object> data) {
		
		gearItems = new List<int>((int)GearSlotType.kGearSlotCount);
		artifactItems = new List<int>((int)ArtifactSlotType.Total);
		
		characterId = 0;
		Reset();
		
		int version = 0;
		if(data.ContainsKey("v")) {
			object obj = data["v"];
			version = JSONTools.ReadInt(obj);
		}
		
		if(version >= 1) {
			if(data.ContainsKey("displayname")) {
				displayName = (string)data["displayname"];
			}
			if(data.ContainsKey("cid")) {
				object obj = data["cid"];
				characterId = JSONTools.ReadInt(obj);
			}
			if(data.ContainsKey("gear")) {
				gearItems.Clear();
				IList tempList = data["gear"] as IList;
				foreach (var item in tempList) {
					gearItems.Add ((int)((long) item));
				}
			}
			if(data.ContainsKey("u")) {
				unlocked = (bool)data["u"];
			}
			if(data.ContainsKey("uc")) {
				object obj = data["uc"];
				unlockCost = JSONTools.ReadInt(obj);
			}
			if(data.ContainsKey("pvi")) {
				object obj = data["pvi"];
				protoVisualIndex = JSONTools.ReadInt(obj);
			}
			if(data.ContainsKey("art")) {
				artifactItems.Clear();
				IList tempList = data["art"] as IList;
				foreach (var item in tempList) {
					artifactItems.Add ((int)((long) item));
				}
			}
			if(data.ContainsKey("pid")) {
				object obj = data["pid"];
				powerID = JSONTools.ReadInt(obj);
			}
			unlockCostType = CostType.Coin;
			if(data.ContainsKey("ct")) {
				if(Enum.IsDefined(typeof(CostType), data["ct"])) {
					unlockCostType = (CostType)Enum.Parse(typeof(CostType), (string)data["ct"]);
				}
			}
			//TR.LOG ("NEW Unlockcost {0}:{1}", this, unlockCost);
		}
		
	}
	
	public void UpdateWithSerializedData(Dictionary<string, object> data) {
		
	//	gearItems = new List<int>((int)GearSlotType.kGearSlotCount);
	//	artifactItems = new List<int>((int)ArtifactSlotType.Total);
		
		int version = 0;
		if(data.ContainsKey("v")) {
			object obj = data["v"];
			version = JSONTools.ReadInt(obj);
		}
		
		if(version >= 1) {
			/*if(data.ContainsKey("gear")) {
				gearItems.Clear();
				IList tempList = data["gear"] as IList;
				foreach (var item in tempList) {
					gearItems.Add ((int)((long) item));
				}
			}*/
			if(data.ContainsKey("u")) {
				unlocked = (bool)data["u"];
			}
			/*if(data.ContainsKey("art")) {
				artifactItems.Clear();
				IList tempList = data["art"] as IList;
				foreach (var item in tempList) {
					artifactItems.Add ((int)((long) item));
				}
			}*/
			if(data.ContainsKey("pid")) {
				object obj = data["pid"];
				powerID = JSONTools.ReadInt(obj);
			}
		}
		
	}
	
	public Dictionary<string, object> ToDict() {
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add ("v", CharacterStats.Version);
		data.Add ("displayname", displayName);
		data.Add ("cid", characterId);
		data.Add ("gear", gearItems);
		data.Add ("u", unlocked);
		data.Add ("uc", unlockCost);
		data.Add ("ct", unlockCostType.ToString());
		data.Add ("pvi", protoVisualIndex);
		data.Add ("art", artifactItems);
		data.Add ("pid", powerID);
		//TR.LOG ("saving Unlockcost {0}:{1}", this, unlockCost);
		return data;
	}

	public void Reset()
	{
		displayName = "?";
		unlocked = false;
		unlockCost = 1200;
		//TR.LOG ("Reset Unlockcost {0}:{1}", this, unlockCost);
		unlockCostType = CostType.Coin;
		protoVisualIndex = 0;
		
		ResetGearAndArtifacts();
	}
	
	public void ResetGearAndArtifacts() {
		//-- Default everyone to skin 0, and nothing in the other slots.
		gearItems.Clear();
		gearItems.Add (0);
		gearItems.Add (-1);
		gearItems.Add (-1);
		gearItems.Add (-1);
		artifactItems.Clear();
		artifactItems.Add (-1);
		artifactItems.Add (-1);
		artifactItems.Add (-1);
		
		//Default powerup
		powerID = 2;
		//TR.LOG ("ResetGearAndArtifacts Unlockcost {0}:{1}", this, unlockCost);
	}
	
//	public int getArtifactForSlot(ArtifactSlotType slotIndex) {
//		if(slotIndex < 0 || slotIndex >= ArtifactSlotType.Total)
//			return -1;
//		return artifactItems[(int)slotIndex];
//	}
//	
//	public void equipArtifactForSlot(int artifactID, ArtifactSlotType slotIndex) {
//		if(slotIndex < 0 || slotIndex >= ArtifactSlotType.Total)
//			return;
//		if(artifactID < 0)
//			return;
//		
//		artifactItems[(int)slotIndex] = artifactID;
//	}
	
//	public bool isArtifactEquipped(int artifactID) {
//		if(artifactID < 0)
//			return false;
//		if(artifactItems[(int)ArtifactSlotType.One] == artifactID)
//			return true;
//		if(artifactItems[(int)ArtifactSlotType.Two] == artifactID)
//			return true;
//		if(artifactItems[(int)ArtifactSlotType.Three] == artifactID)
//			return true;
//		
//		return false;
//	}
	
//	public bool hasArtifactsEquipped() {
//		if(isArtifactEquipped((int)ArtifactSlotType.One) == true)
//			return true;
//		if(isArtifactEquipped((int)ArtifactSlotType.Two) == true)
//			return true;
//		if(isArtifactEquipped((int)ArtifactSlotType.Three) == true)
//			return true;
//		
//		return false;
//	}
};


