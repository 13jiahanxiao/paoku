using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/*
 * Yeah, I forgot  that I had put in a specific DiscountItemType.  Fortunately, Alex didn't.
public enum ItemType
{
	Consumable = 0,
	Artifact = 1,
	StoreItem = 2, //IAP
	PowerUp = 3,
	Character = 4
}
 */
 
public class WeeklyDiscountManager : MonoBehaviour
{
	protected static Notify notify;
	
	private List<WeeklyDiscountProtoData> weeklyDiscounts = new List<WeeklyDiscountProtoData>();
	public List<WeeklyDiscountProtoData> GetWeeklyDiscounts () {return weeklyDiscounts; }
	
	/// <summary>
	/// Gets a list of discounts by the Item Type.
	/// </summary>
	/// <returns>
	/// List of Weekly Discounts
	/// </returns>
	/// <param name='itemType'>
	/// Item type.
	/// </param>
	public List<WeeklyDiscountProtoData> GetDiscountsByItemType( DiscountItemType itemType )
	{
		List<WeeklyDiscountProtoData> discountList = new List<WeeklyDiscountProtoData>();
		
	/*	if ( weeklyDiscounts != null && weeklyDiscounts.Count > 0 )
		{
			foreach ( WeeklyDiscountProtoData discount in weeklyDiscounts )
			{
				if ( discount._itemType == itemType )
				{
					discountList.Add( discount );
				}
			}
		}*/
		return discountList;
	}

	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);
	}
	
	// Use this for initialization
	void Start ()
	{
		/* GetTheSales is deprecated.
		 
		if ( ! Settings.GetBool("use-init-msg", true))
		{
			// GetTheSales();
		}
		*/
	}
	
	private void SaveWeeklyDiscounts()
	{
/*		using (MemoryStream stream = new MemoryStream())
		{
			string fileName = Application.persistentDataPath + Path.DirectorySeparatorChar +
				"weeklydiscount.txt";
			List <object> tempList = new List<object>();
			foreach (WeeklyDiscountProtoData data in weeklyDiscounts)
			{
				tempList.Add(data.ToDict());	
			}
			
			Dictionary<string, object> saveDict = new Dictionary<string, object>();
			
			saveDict.Add("sales", tempList);
			
			Dictionary<string, object> secureData = SaveLoad.Save(saveDict);
			string dictString = MiniJSON.Json.Serialize(secureData);
			
			try {
				using (StreamWriter fileWriter = File.CreateText(fileName))
				{
					fileWriter.WriteLine(dictString);
					fileWriter.Close();
				}
			} catch (System.Exception ex) {
				notify.Warning("Weekly Discount Save Exxception: " + ex.Message);
			}
		}
		*/
	}
	
	private void LoadWeeklyDiscounts()
	{
	/*	string fileName = Application.persistentDataPath + Path.DirectorySeparatorChar +
			"weeklydiscount.txt";
		
		if (File.Exists(fileName) == false)
		{
			notify.Debug("No weeklydiscount.txt file exists.");
		}
		else
		{
			StreamReader reader = File.OpenText(fileName);
			string jsonString = reader.ReadToEnd();
			reader.Close();
			
			try {
				Dictionary<string, object> loadedData =
					MiniJSON.Json.Deserialize(jsonString) as Dictionary<string, object>;
			
				if (SaveLoad.Load(loadedData) == false) { return; }
			
				Dictionary<string, object> dataDict = loadedData["data"]
					as Dictionary<string, object>;
			
				ParseJsonDict(dataDict);
			} catch (Exception ex) {
				notify.Warning("WeeklyDiscountManager - error reading weeklydiscount.txt "+ex.ToString());
				return;
			}
		}
		*/
	}
	
	/// <summary>
	/// Parses a decoded json dictionary
	/// </summary>
	/// <param name='dataDict'>
	/// the json data dict.
	/// </param>
	public void ParseJsonDict( Dictionary<string,object> dataDict)
	{
		if (dataDict == null) { return; }
		
//		List<object> loadObjList = dataDict["sales"] as List<object>;
		
	//	foreach (object loadObj in loadObjList)
	//	{
	//		WeeklyDiscountProtoData weekDiscountObj = DecodeSaleJsonObject(loadObj);
	//		if (weekDiscountObj._endDate > DateTime.UtcNow)
	//		{
	//			weeklyDiscounts.Add(weekDiscountObj);
	//		}
	//	}
		notify.Debug("Read file in Weekly Discounts.  Length: " + weeklyDiscounts.Count);
	}

	[Obsolete ("GetTheSales is deprecated, please use ApplySalesFromInit", true ) ]
	private void GetTheSales ()
	{
	//	NetAgent.Submit(new NetRequest("/store", GotTheSales));
	}
	
	[Obsolete ("GotTheSales is deprecated, please use ApplySalesFromInit", true ) ]
	private bool GotTheSales(WWW www, bool noErrors, object results)
	{
	/*	notify.Debug("GotTheSales noErrors=" + noErrors + " www.error=" + www.error);		
		bool result = false;
		
		//Grab values from local storage.
		LoadWeeklyDiscounts();
		
		if (noErrors)
		{
			if (results == null)
			{
				notify.Error("No results!  Must not be connected... " + www.text);
				return false;
			}
			
			notify.Debug("GotTheSales " + www.text + " " + www.error);
			try {
				Dictionary<string, object> rootDict = results as Dictionary<string, object>;
				
				int responseCode = int.Parse(rootDict["responseCode"].ToString());
				
				//Server Response was successful, and results were found.
				//Clear the stored results, and load the results from the web
				if (responseCode == 200)
				{
					weeklyDiscounts.Clear();
					
					List<object> webSaleList = rootDict["sales"] as List<object>;
					
					foreach (object oneObject in webSaleList)
					{
						WeeklyDiscountProtoData oneItem = DecodeSaleJsonObject(oneObject);
						weeklyDiscounts.Add(oneItem);
					}
					result = true;
					SaveWeeklyDiscounts();
				}
				// Server response was successful, but no results were found
				// Clear the discounts and save.
				else if (responseCode == 204)
				{
					weeklyDiscounts.Clear();
					SaveWeeklyDiscounts();
				}
				// if the server responds with a code other than 200 or 204,
				// only use the just loaded discounts (if they exist).
				
			} catch (System.Exception theException) {
				notify.Warning("GotTheSales www.text= " + www.text + 
					" exception " + theException.Message
				);
			}
		} else {
			notify.Debug("No connection. Load stored discounts if they exist");	
		}
		
		//Cycle through all of the sales, and compare each discount item to the stores items;
		List<int> storeItemDiscountList = new List<int>();
		
		foreach (WeeklyDiscountProtoData sale in weeklyDiscounts)
		{
			foreach (DiscountItemProtoData discount in sale._itemList)
			{
				foreach(StoreItem storeItem in Store.StoreItems) {
					//From all sales received, only apply one discount to a unique item
					// regardless the number of discounts received across all sales.
					if (storeItem.id == discount._id 
						&& !storeItemDiscountList.Contains(discount._id)
					) {
						storeItemDiscountList.Add(discount._id);
						notify.Debug("Weekly Discount applied to item ID: " + storeItem.id +
							"From discount id: " + discount._id);
						
						storeItem.cost = discount._costValue;
						storeItem.sortPriority = discount._salePriority;
						
						notify.Debug(discount._discountIcon.ToString());
						
						if (discount._discountIcon != DiscountIcon.None) {
							storeItem.description+= " - " + discount._discountIcon.ToString();
						}
						
						if (discount._discountIcon == DiscountIcon.LimitedTimeOffer) {
							TimeSpan timeLeft = sale._endDate.Subtract(DateTime.UtcNow);
							
							if (timeLeft.Days == 0) {
								storeItem.description += " - " + timeLeft.Hours 
									+ ":" + timeLeft.Minutes + ":" + timeLeft.Seconds;
							} else {
								storeItem.description += " - " + timeLeft.Days;	
							}
								
						}
					}
				}
			}
		}
		*/
		return false;
	}
	
	/// <summary>
	/// Set the discount costs to zero for all Consumables, Powers, Artifacts, and Characters.
	/// </summary>
	private void _resetDiscountCostsAndSortPriority()
	{
	/*	foreach ( BaseConsumable consumable in ConsumableStore.consumablesList )
		{
			consumable.ServerCost = 0;
		}
		
		foreach ( BasePower power in PowerStore.Powers )
		{
			power.ServerCost = 0;
		}
		
		foreach ( ArtifactProtoData artifact in ArtifactStore.Artifacts )
		{
			artifact.ServerCost_Lvl1 = 0;
			artifact.ServerCost_Lvl2 = 0;
			artifact.ServerCost_Lvl3 = 0;
			artifact.ServerCost_Lvl4 = 0;
			artifact.ServerCost_Lvl5 = 0;
			
		}
		
		foreach ( CharacterStats character in GameProfile.SharedInstance.Characters )
		{
			character.ServerCost = 0;
		}
		
		// Reset character sort priority from CharacterOrder
		for ( int i = 0; i < GameProfile.SharedInstance.Characters.Count; i++ )
		{
			GameProfile.SharedInstance.Characters[i].SortPriority = GameProfile.SharedInstance.Characters[i].DefaultSortPriority;
		}
		
		for ( int i = 0; i < ArtifactStore.Artifacts.Count; i++ )
		{
			ArtifactStore.Artifacts[i]._sortPriority = ArtifactStore.Artifacts[i].DefaultSortPriority;
		}
		
		for ( int i = 0; i < ConsumableStore.consumablesList.Count; i++ )
		{
			ConsumableStore.consumablesList[i].SortPriority = ConsumableStore.consumablesList[i].DefaultSortPriority;
		}
		
		for ( int i = 0; i < PowerStore.Powers.Count; i++ )
		{
			PowerStore.Powers[i].SortPriority = PowerStore.Powers[i].DefaultSortPriority;
		}
		*/
	}
	
	public void ExpireDiscounts ()
	{
	/*	List<WeeklyDiscountProtoData> resetList = new List<WeeklyDiscountProtoData>();
		
		foreach ( WeeklyDiscountProtoData sale in weeklyDiscounts )
		{
			if ( sale._endDate > DateTime.UtcNow )
			{
				resetList.Add( sale );
			}
		}
		weeklyDiscounts.Clear();

		// Retrieve
		foreach ( WeeklyDiscountProtoData weekSale in resetList )
		{
			weeklyDiscounts.Add( weekSale );
		}
		
		_resetDiscountCostsAndSortPriority();
		
		_applyCostsFromSale();
		*/
	}
	
	private void _applyCostsFromSale()
	{
		/*
		Dictionary<DiscountItemType, List<int>> preventDuplicatesDict = new Dictionary<DiscountItemType, List<int>>();
		
		foreach (WeeklyDiscountProtoData sale in weeklyDiscounts)
		{
			foreach (DiscountItemProtoData discount in sale._itemList)
			{
				if (!preventDuplicatesDict.ContainsKey(discount.ItemType) ||
					!preventDuplicatesDict[discount.ItemType].Contains(discount._id))
				{
					float percentage = discount._costValue / 100f;
					
					switch (discount.ItemType)
					{
						case DiscountItemType.Artifact:
							ArtifactProtoData artifact = ArtifactStore.GetArtifactProtoData(discount._id);
							if (artifact != null)
							{
								// If the sale priority of discount is zero, use the item's default priority
								if ( discount._salePriority != 0 )
								{
									artifact._sortPriority = discount._salePriority;
								}
							
								artifact.ServerCost_Lvl1 = Mathf.RoundToInt ((float) artifact._cost     * percentage );
								artifact.ServerCost_Lvl2 = Mathf.RoundToInt ((float) artifact._cost_lv2 * percentage );
								artifact.ServerCost_Lvl3 = Mathf.RoundToInt ((float) artifact._cost_lv3 * percentage );
								artifact.ServerCost_Lvl4 = Mathf.RoundToInt ((float) artifact._cost_lv4 * percentage );
								artifact.ServerCost_Lvl5 = Mathf.RoundToInt ((float) artifact._cost_lv5 * percentage );
							}
						break;
						
						case DiscountItemType.Consumable:
							BaseConsumable consumable = ConsumableStore.ConsumableFromID(discount._id);
							if (consumable != null)
							{														
								if ( discount._salePriority != 0 )
								{
									consumable.SortPriority = discount._salePriority;
								}
							
								consumable.ServerCost = Mathf.RoundToInt( (float) consumable.Cost * percentage );
							}
						break;
						
						case DiscountItemType.Powerup:
							BasePower power = PowerStore.PowerFromID(discount._id);
							if (power != null)
							{
							
								if ( discount._salePriority != 0 )
								{
									power.SortPriority = discount._salePriority;
								}
							
								power.ServerCost = Mathf.RoundToInt( (float) power.Cost * percentage );
							}
						break;
						case DiscountItemType.Character:
							List<CharacterStats> characterList = GameProfile.SharedInstance.Characters;
						
							CharacterStats characterData = null;
						
							// Search for the character id
							foreach ( CharacterStats character in characterList )
							{
								if ( discount._id == character.characterId )
								{
									characterData = character;
									break;
								}
							}
						
							if ( characterData != null )
							{
								if ( discount._salePriority != 0 )
								{
									characterData.SortPriority = discount._salePriority;
								}
							
								characterData.ServerCost = Mathf.RoundToInt( (float) characterData.unlockCost * percentage );
							}
						break;
					}
					if (!preventDuplicatesDict.ContainsKey(discount.ItemType))
					{
						preventDuplicatesDict[discount.ItemType] = new List<int>();
					}
					preventDuplicatesDict[discount.ItemType].Add(discount._id);
				}
				/*
				foreach(StoreItem storeItem in Store.StoreItems) {
					//From all sales received, only apply one discount to a unique item
					// regardless the number of discounts received across all sales.
					if (storeItem.id == discount._id 
						&& !storeItemDiscountList.Contains(discount._id)
					) {
						storeItemDiscountList.Add(discount._id);
						notify.Debug("Weekly Discount applied to item ID: " + storeItem.id +
							"From discount id: " + discount._id);
						
						storeItem.cost = discount._costValue;
						storeItem.sortPriority = discount._salePriority;
						
						notify.Debug(discount._discountIcon.ToString());
						
						if (discount._discountIcon != DiscountIcon.None) {
							storeItem.description+= " - " + discount._discountIcon.ToString();
						}
						
						if (discount._discountIcon == DiscountIcon.LimitedTimeOffer) {
							TimeSpan timeLeft = sale._endDate.Subtract(DateTime.UtcNow);
							
							if (timeLeft.Days == 0) {
								storeItem.description += " - " + timeLeft.Hours 
									+ ":" + timeLeft.Minutes + ":" + timeLeft.Seconds;
							} else {
								storeItem.description += " - " + timeLeft.Days;	
							}
								
						}
					}
				}
				
			}
		}
	*/
	}
	
	public bool ApplySalesFromInit(List<object> webSaleList, int responseCode)
	{	
		bool result = false;
	/*	
		//Grab values from local storage.
		LoadWeeklyDiscounts();
			
		//Server Response was successful, and results were found.
		//Clear the stored results, and load the results from the web
		if (responseCode == 200)
		{
			weeklyDiscounts.Clear();
			
			foreach (object oneObject in webSaleList)
			{
				WeeklyDiscountProtoData oneItem = DecodeSaleJsonObject(oneObject);
				weeklyDiscounts.Add(oneItem);
			}
			result = true;
			
			SaveWeeklyDiscounts();
		}
		// Server response was successful, but no results were found
		// Clear the discounts and save.
		else if (responseCode == 204)
		{
			weeklyDiscounts.Clear();
			SaveWeeklyDiscounts();
		}
		// if the server responds with a code other than 200 or 204,
		// only use the just loaded discounts (if they exist).
			
	
		// Reset the in-memory discount costs for Consumables, Powers, Artifacts, and Characters.
		_resetDiscountCostsAndSortPriority();
		
		//Cycle through all of the sales, and compare each discount item to the stores items;
		_applyCostsFromSale();
		*/	
		return result;
	}

	private WeeklyDiscountProtoData DecodeSaleJsonObject(object _saleObj)
	{	
		
		Dictionary<string, object> dict = _saleObj as Dictionary<string, object>;
		WeeklyDiscountProtoData sale = new WeeklyDiscountProtoData(dict);
		return sale;
	}
	
/*	// Update is called once per frame
	void Update ()
	{
	
	}
*/
}

