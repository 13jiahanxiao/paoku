using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Reflection;

public enum UiScreenName { IDOL, MAIN, UPGRADES, OBJECTIVES, LEADERBOARDS, POSTRUN, WORLDOFOZ, }
public enum NotificationType { Powerup, Modifier, Consumable, MoreCoins, Challenge, Character, Land, 
								ArtifactSale, ConsumableSale, PowerupSale, CharacterSale, None, }

// wxj, add activity notification type
public enum OneShotNotificationType { NewWeeklyChallenge, WeeklyChallengeCompleted, LegendaryChallengeCompleted,
										TopScorePositionWentUp, TopScorePositionWentDown, 
										TopDistancePositionWentUp, TopDistancePositionWentDown, 
										NewTeamChallenge, TeamChallengeCompleted, NewActivityChallenge, ActivityChallengeCompleted }

public class WrappedDict	// necessary for JSON serialization & deserialization of Notification Clears
{
	public Dictionary<int,Int64> dict = new Dictionary<int,Int64>();
	
	public WrappedDict() { }
	
	public WrappedDict(Dictionary<int,Int64> sourceDict)
	{
		dict = sourceDict;
	}
}

public class NotificationSystem : MonoBehaviour 
{
	public NotificationIcons notificationIcons;
	
	private AppCounters appCounters;	
	private int totalAppTime;

	private int powerupClearTime = 360;		//30;									// seconds before the clear is lifted
	private int modifierClearTime = 540;	//45;
	private int consumableClearTime = 180;	//15;
	private int landClearTime = 180;		//15;
	private int characterClearTime = 540;	//45;
	private int saleClearTime = 180;		//15;
	
	private List<int> powerupsPurchasable = new List<int>();						// all purchasable
	private List<int> modifiersPurchasable = new List<int>();						
	private List<int> consumablesPurchasable = new List<int>();
	private List<int> landsDownloadable = new List<int>();
	private List<int> charactersPurchasable = new List<int>();
	private List<int> artifactSalesViewable = new List<int>();
	private List<int> consumableSalesViewable = new List<int>();
	private List<int> powerupSalesViewable = new List<int>();
	private List<int> characterSalesViewable = new List<int>();
	
	// notification clears that expire after certain amount of time
	private Dictionary<int,Int64> powerupsCleared = new Dictionary<int,Int64>();	// logging all clicks/views that clear modifiers, if active
	private Dictionary<int,Int64> modifiersCleared = new Dictionary<int,Int64>();	
	private Dictionary<int,Int64> consumablesCleared = new Dictionary<int,Int64>();	
	private Dictionary<int,Int64> landsCleared = new Dictionary<int,Int64>();	
	private Dictionary<int,Int64> charactersCleared = new Dictionary<int,Int64>();	
	private Dictionary<int,Int64> artifactSalesCleared = new Dictionary<int,Int64>();	
	private Dictionary<int,Int64> consumableSalesCleared = new Dictionary<int,Int64>();	
	private Dictionary<int,Int64> powerupSalesCleared = new Dictionary<int,Int64>();	
	private Dictionary<int,Int64> characterSalesCleared = new Dictionary<int,Int64>();	
	
	// notification clears that don't expire (that stay cleared forever)
	private Dictionary<int,Int64> oneShotNotifications = new Dictionary<int,Int64>(); // Key = ID of whatever it is, Value = (int)OneShotNotificationType

	private int charactersNewFeature;		// for notification on first run of app, that new character feature exists
	
	//private UiScreenName currentScreenName;
	
	//int newWeeklyObjectives = 0;
	//int weeklyObjectivesCompleted = 0;
	//int legendaryObjectivesCompleted = 0;
	
	//int leaderboardNotifications = 0;
	//int moreCoinsNotifications = 0;
	
	protected static Notify notify; 
	
	void Awake()
	{
		notify = new Notify(this.GetType().Name);
	}
	
	void Start()
	{
		appCounters = Services.Get<AppCounters>();
		charactersNewFeature = PlayerPrefs.GetInt("charactersNewFeature", 1);	// '0' = cleared, '1' = new
		RestoreAllClearedNotificationsFromPlayerPrefs();
		RefreshAllNotifications();
		notify.Debug("modifiersCleared JSON = " + SerializationUtils.ToJson(modifiersCleared));
	}
	
	public static void ResetClearedNotificationsInPlayerPrefs()
	{
		PlayerPrefs.SetString("powerupsCleared", "{}");
		PlayerPrefs.SetString("modifiersCleared", "{}");
		PlayerPrefs.SetString("consumablesCleared", "{}");
		PlayerPrefs.SetString("landsCleared", "{}");
		PlayerPrefs.SetString("charactersCleared", "{}");
		PlayerPrefs.SetString("artifactSalesCleared", "{}");
		PlayerPrefs.SetString("consumableSalesCleared", "{}");
		PlayerPrefs.SetString("powerupSalesCleared", "{}");
		PlayerPrefs.SetString("characterSalesCleared", "{}");
		PlayerPrefs.SetString("oneShotNotifications", "{}");
		PlayerPrefs.Save();
	}

	public void SendOneShotNotificationEvent(int id, int type)	//OneShotNotificationType type,
	{
		if (!oneShotNotifications.ContainsKey(id))	//(int)type))	// skip this if there is already an old uncleared one-shot notification
			oneShotNotifications.Add(id, type);		// log the one-shot notification
		
		notify.Debug("oneShotNotification count = " + oneShotNotifications.Count.ToString());

		foreach (KeyValuePair<int, Int64> kvp in oneShotNotifications)
			notify.Debug("oneShotNotification key = " + kvp.Key.ToString() + ", value = " + kvp.Value.ToString());
	}
	
	public void ClearCharactersNewFeatureNotification()
	{
		charactersNewFeature = 0;
		PlayerPrefs.SetInt("charactersNewFeature", 0);
	}
	
	public List<int> GetLandsDownloadable()
	{
		return landsDownloadable;
	}
	
	public int GetOneShotCount(OneShotNotificationType type)
	{
		//return oneShotNotifications.ContainsValue((int)type);	//.ContainsKey(id);	//(int)type);
		
		int count = 0;
		
		foreach (KeyValuePair<int, Int64> kvp in oneShotNotifications)
		{
			if (kvp.Value == (int)type)
				count++;
		}
		
		return count;
	}
	
	public void ClearOneShotNotification(int id)	
	{
		oneShotNotifications.Remove(id);
	}		

	public void ClearOneShotNotification(OneShotNotificationType type)
	{
		List<int> keysToRemove = new List<int>();
		
		foreach (KeyValuePair<int,Int64> kvp in oneShotNotifications)
		{
			if (kvp.Value == (int)type)
				keysToRemove.Add(kvp.Key);
		}
		
		foreach (int key in keysToRemove)
			oneShotNotifications.Remove(key);
	}		
	
	public void ClearNotification(NotificationType type, int id)
	{
		switch (type)
		{
			case NotificationType.Powerup:
				ClearSingleNotification(powerupsPurchasable, ref powerupsCleared, id);
				break;
			case NotificationType.Modifier:
				ClearSingleNotification(modifiersPurchasable, ref modifiersCleared, id);
				break;			
			case NotificationType.Consumable:
				ClearSingleNotification(consumablesPurchasable, ref consumablesCleared, id);
				break;
			case NotificationType.Land:
				foreach (int setID in landsDownloadable)
				{
					if (!landsCleared.ContainsKey(setID))
					{
						landsCleared.Add(setID, appCounters.GetSecondsSpentInApp());
					}
				}
				break;
			case NotificationType.Character:
				ClearSingleNotification(charactersPurchasable, ref charactersCleared, id);
				break;	
			case NotificationType.ArtifactSale:
				ClearSingleNotification(artifactSalesViewable, ref artifactSalesCleared, id);
				break;		
			case NotificationType.ConsumableSale:
				ClearSingleNotification(consumableSalesViewable, ref consumableSalesCleared, id);
				break;	
			case NotificationType.PowerupSale:
				ClearSingleNotification(powerupSalesViewable, ref powerupSalesCleared, id);
				break;				
			case NotificationType.CharacterSale:
				ClearSingleNotification(characterSalesViewable, ref characterSalesCleared, id);
				break;				
			case NotificationType.MoreCoins:
				break;		
		}
	}
	
	private void ClearSingleNotification(List<int> purchasableList, ref Dictionary<int, Int64> clearDict, int id)
	{
		if (purchasableList.Contains(id))
		{
			if (!clearDict.ContainsKey(id))
				clearDict.Add(id, appCounters.GetSecondsSpentInApp());
		}		
	}
	
	/// <summary>
	/// Sets the notification icons for this page.
	/// The convention being followed: -1 = don't show icon at all, 0 = show exclamation point, 1-9 show number, >9 show exclamation point
	/// </summary>
	/// <param name='screenName'>
	/// Screen name.
	/// </param>
	public void SetNotificationIconsForThisPage(UiScreenName screenName)
	{
		UIIdolMenuViewControllerOz idolMenuVC = UIManagerOz.SharedInstance.idolMenuVC;
		UIMainMenuViewControllerOz mainVC = UIManagerOz.SharedInstance.mainVC;
		UIInventoryViewControllerOz inventoryVC = UIManagerOz.SharedInstance.inventoryVC;
		UILeaderboardViewControllerOz leaderboardVC = UIManagerOz.SharedInstance.leaderboardVC;
		UIObjectivesViewControllerOz ObjectivesVC = UIManagerOz.SharedInstance.ObjectivesVC;
		UIPostGameViewControllerOz postGameVC = UIManagerOz.SharedInstance.postGameVC;
		UIWorldOfOzViewControllerOz worldOfOzVC = UIManagerOz.SharedInstance.worldOfOzVC;
		
		//currentScreenName = screenName;	// store page reference, for internal updating
		
		RefreshAllNotifications();		// refresh everything every time for now, to make sure it works right first
		
		List<int> pows = GetActualNotifications(powerupsPurchasable, powerupsCleared);
		List<int> mods = GetActualNotifications(modifiersPurchasable, modifiersCleared);
		List<int> cons = GetActualNotifications(consumablesPurchasable, consumablesCleared);
		int totalStore = pows.Count + mods.Count + cons.Count;	
		
		int newWeekly = GetOneShotCount(OneShotNotificationType.NewWeeklyChallenge);
		int newTeam = GetOneShotCount( OneShotNotificationType.NewTeamChallenge );
		int totalNew = newWeekly + newTeam;
		
		int weeklyCompleted = GetOneShotCount(OneShotNotificationType.WeeklyChallengeCompleted);
		int teamCompleted = GetOneShotCount ( OneShotNotificationType.TeamChallengeCompleted );
		int legendaryCompleted = GetOneShotCount(OneShotNotificationType.LegendaryChallengeCompleted);		
		int totalChallenges = weeklyCompleted + teamCompleted + legendaryCompleted; //(weekly ? 1 : 0) + (legendary ? 1 : 0);
		
		int distances = GetOneShotCount(OneShotNotificationType.TopDistancePositionWentUp) + 
			GetOneShotCount(OneShotNotificationType.TopDistancePositionWentDown);
		int scores = GetOneShotCount(OneShotNotificationType.TopScorePositionWentUp) +
			GetOneShotCount(OneShotNotificationType.TopScorePositionWentDown);		
		int totalLeaderboards = distances + scores;	//(distances ? 1 : 0) + (scores ? 1 : 0);
		
		List<int> lands = GetActualNotifications(landsDownloadable, landsCleared);
		List<int> characters = GetActualNotifications(charactersPurchasable, charactersCleared);
		int totalWorldOfOz = ((lands.Count > 0) ? 1 : 0) + characters.Count + charactersNewFeature;
		
		int totalMenu = totalStore + totalChallenges + totalLeaderboards + totalWorldOfOz;
		
		// for sale banners, -1 = don't show banner, 0 = show banner
		int artifactSales = GetActualNotifications(artifactSalesViewable, artifactSalesCleared).Count;
		int consumableSales = GetActualNotifications(consumableSalesViewable, consumableSalesCleared).Count;
		int powerupSales = GetActualNotifications(powerupSalesViewable, powerupSalesCleared).Count;
		int storeSales = artifactSales + consumableSales + powerupSales;
		int characterSales = GetActualNotifications(characterSalesViewable, characterSalesCleared).Count;
		int menuSales = storeSales + characterSales;

		switch (screenName)
		{
			case UiScreenName.IDOL:
				idolMenuVC.SetNotificationIcon(0, (totalStore == 0) ? -1 : totalMenu);	//totalStore);	// btn_menu1
				break;
			case UiScreenName.MAIN:
				mainVC.SetNotificationIcon(0, (totalStore == 0) ? -1 : totalStore);						// btn_store1
				mainVC.SetNotificationIcon(1, GetChallengesIconValue(totalChallenges, totalNew));		// btn_challenges
				mainVC.SetNotificationIcon(2, (totalLeaderboards == 0) ? -1 : 0); //totalLeaderboards);	// btn_leaderboards
				mainVC.SetNotificationIcon(3, -1);														// btn_more_coins
				mainVC.SetNotificationIcon(4, (totalWorldOfOz == 0) ? -1 : totalWorldOfOz);				// btn_world_of_oz
				mainVC.SetNotificationIcon(5, (characters.Count == 0) ? -1 : characters.Count);			// btn_characters
				mainVC.SetNotificationIcon(6, (storeSales == 0) ? -1 : 0);								// store sale banner
				mainVC.SetNotificationIcon(7, (characterSales == 0) ? -1 : 0);							// characters sale banner
				mainVC.SetNotificationIcon(8, (characterSales == 0) ? -1 : 0);							// world of oz sale banner			
				break;
			case UiScreenName.UPGRADES:
				inventoryVC.SetNotificationIcon(0, (pows.Count == 0) ? -1 : pows.Count);				// tab_powerups / powerups
				inventoryVC.SetNotificationIcon(1, (mods.Count == 0) ? -1 : mods.Count);				// tab_abilities / modifiers
				inventoryVC.SetNotificationIcon(2, (cons.Count == 0) ? -1 : cons.Count);				// tab_utilities / consumables
				inventoryVC.SetNotificationIcon(3, -1);													// tab_more_coins / product store
				break;
			case UiScreenName.OBJECTIVES:	
				ObjectivesVC.SetNotificationIcon(0, GetChallengesIconValue(weeklyCompleted, newWeekly));	// tab_weekly_challenges
				ObjectivesVC.SetNotificationIcon( 1, GetChallengesIconValue( teamCompleted, newTeam ) );	// tab_team_challenges
				ObjectivesVC.SetNotificationIcon( 2, (legendaryCompleted == 0) ? -1 : legendaryCompleted);	// tab_legendary_objectives			
				break;
			case UiScreenName.LEADERBOARDS:					
				leaderboardVC.SetNotificationIcon(0, (distances == 0) ? -1 : 0);//distances); //0 : -1);// top_distances
				leaderboardVC.SetNotificationIcon(1, (scores == 0) ? -1 : 0);	//scores);	//0 : -1);	// tab_top_scores				
				break;
			case UiScreenName.POSTRUN:	
				postGameVC.SetNotificationIcon(0, (totalMenu == 0) ? -1 : totalMenu);					// btn_menu2
				postGameVC.SetNotificationIcon(1, (totalStore == 0) ? -1 : totalStore);					// btn_store2	
				postGameVC.SetNotificationIcon(2, (menuSales == 0) ? -1 : 0);							// menu sale banner
				postGameVC.SetNotificationIcon(3, (storeSales == 0) ? -1 : 0);							// store sale banner
				break;
			case UiScreenName.WORLDOFOZ:
				worldOfOzVC.SetNotificationIcon(0, (lands.Count == 0) ? -1 : 0);						// tab_lands
				worldOfOzVC.SetNotificationIcon(1, (charactersNewFeature == 1) ? 0 : 					// tab_characters
					(characters.Count == 0) ? -1 : characters.Count);						
				break;
		}
	}

	private int GetChallengesIconValue(int completed, int newChallenges)
	{
		int iconValue = -1;
		
		if (completed > 0)
			iconValue = completed;						// if any completed challenges, show how many in notification icon
		else
			iconValue = (newChallenges > 0) ? 0 : -1;	// otherwise, if we have new challenges, show exclamation point
		
		return iconValue;
	}	
	
	public bool GetNotificationStatusForThisCell(NotificationType type, int id)	
	{
		bool returnVal = false;
		
		switch (type)
		{
			case NotificationType.Powerup:
				returnVal = (powerupsPurchasable.Contains(id) && !powerupsCleared.ContainsKey(id));
				break;
			
			case NotificationType.Modifier:
				returnVal = (modifiersPurchasable.Contains(id) && !modifiersCleared.ContainsKey(id));
				break;		
			
			case NotificationType.Consumable:
				returnVal = (consumablesPurchasable.Contains(id) && !consumablesCleared.ContainsKey(id));
				break;
			
			case NotificationType.Land:
				returnVal = (landsDownloadable.Count != 0);	// start with 'true' only if we have lands to download
			
				foreach (int setID in landsDownloadable)	// check clears only if we have lands to download
				{
					if (landsCleared.ContainsKey(setID))
					{
						returnVal = false;					// turn notification off for downloadable lands only if cleared
					}
				}
				break;
				
			case NotificationType.Character:
				returnVal = (charactersPurchasable.Contains(id) && !charactersCleared.ContainsKey(id));
				break;			
			
			case NotificationType.Challenge:	// only true for completed challenges (weekly or legendary), not for new weekly challenges
				returnVal = false;
			
				if (oneShotNotifications.ContainsKey(id))
				{
					if (oneShotNotifications[id] != (int)OneShotNotificationType.NewWeeklyChallenge && 
						oneShotNotifications[id] != (int)OneShotNotificationType.NewTeamChallenge)
					{
						return true;
					}
				}
				break;
		}
		
		return returnVal;	//false;
	}

	public void SaveClearedNotificationsToPlayerPrefs()
	{
		// serialize 'notifications cleared' dicts to JSON and compress string, then store in player prefs		
		SaveSingleClearedNotificationDict("powerupsCleared", powerupsCleared);
		SaveSingleClearedNotificationDict("modifiersCleared", modifiersCleared);
		SaveSingleClearedNotificationDict("consumablesCleared", consumablesCleared);
		SaveSingleClearedNotificationDict("landsCleared", landsCleared);
		SaveSingleClearedNotificationDict("charactersCleared", charactersCleared);
		SaveSingleClearedNotificationDict("artifactSalesCleared", artifactSalesCleared);
		SaveSingleClearedNotificationDict("consumableSalesCleared", consumableSalesCleared);
		SaveSingleClearedNotificationDict("powerupSalesCleared", powerupSalesCleared);
		SaveSingleClearedNotificationDict("characterSalesCleared", characterSalesCleared);
		SaveSingleClearedNotificationDict("oneShotNotifications", oneShotNotifications);
		PlayerPrefs.Save();
	}	
	
	private void SaveSingleClearedNotificationDict(string prefString, Dictionary<int,Int64> dict)
	{
		//string compressed = StringCompressor.CompressString(MiniJSON.Json.Serialize(dict));
		//string compressed = MiniJSON.Json.Serialize(dict);
		string compressed = SerializationUtils.ToJson(new WrappedDict(dict));
		PlayerPrefs.SetString(prefString, compressed);
		notify.Debug("Saving cleared notification list (" + prefString + "): " + compressed + " / Dict.Count = " + dict.Count);
	}

	private void RestoreAllClearedNotificationsFromPlayerPrefs()
	{
		RestoreSingleClearedNotificationFromPlayerprefs("powerupsCleared", ref powerupsCleared);
		RestoreSingleClearedNotificationFromPlayerprefs("modifiersCleared", ref modifiersCleared);
		RestoreSingleClearedNotificationFromPlayerprefs("consumablesCleared", ref consumablesCleared);
		RestoreSingleClearedNotificationFromPlayerprefs("landsCleared", ref landsCleared);
		RestoreSingleClearedNotificationFromPlayerprefs("charactersCleared", ref charactersCleared);
		RestoreSingleClearedNotificationFromPlayerprefs("artifactSalesCleared", ref artifactSalesCleared);
		RestoreSingleClearedNotificationFromPlayerprefs("consumableSalesCleared", ref consumableSalesCleared);
		RestoreSingleClearedNotificationFromPlayerprefs("powerupSalesCleared", ref powerupSalesCleared);
		RestoreSingleClearedNotificationFromPlayerprefs("characterSalesCleared", ref characterSalesCleared);
		RestoreSingleClearedNotificationFromPlayerprefs("oneShotNotifications", ref oneShotNotifications);	
	}	
	
	private void RestoreSingleClearedNotificationFromPlayerprefs(string prefName, ref Dictionary<int,Int64> clearedDict)
	{
		string pref = PlayerPrefs.GetString(prefName, "{}");	// get string from player prefs
		//pref = (pref == "{}") ? pref : StringCompressor.DecompressString(pref);	// decompress, if applicable
		WrappedDict wrappedDict = new WrappedDict();
		SerializationUtils.FromJson(wrappedDict, pref);
		clearedDict = wrappedDict.dict;
		notify.Debug("Loading cleared notification list (" + prefName + "): " + pref);	
	}
	
	private void RefreshAllNotifications()				// for manually triggering update of notification status
	{
		PlayerStats playerStats = GameProfile.SharedInstance.Player;
		
		UpdateAllNotificationClears();					// remove 'notification clears' that have expired
		
		powerupsPurchasable = GetPowerupsPurchasable(playerStats);
		modifiersPurchasable = GetModifiersPurchasable(playerStats);
		consumablesPurchasable = GetConsumablesPurchasable(playerStats);
		landsDownloadable = GetLandsDownloadable(playerStats);
		charactersPurchasable = GetCharactersPurchasable(playerStats);
		artifactSalesViewable = GetIDsFromDiscountList(GetSalesViewable(DiscountItemType.Artifact));
		consumableSalesViewable = GetIDsFromDiscountList(GetSalesViewable(DiscountItemType.Consumable));
		powerupSalesViewable = GetIDsFromDiscountList(GetSalesViewable(DiscountItemType.Powerup));
		characterSalesViewable = GetIDsFromDiscountList(GetSalesViewable(DiscountItemType.Character));
	}	
	
	private void UpdateAllNotificationClears()	
	{
		UpdateNotificationClear(powerupsCleared, powerupClearTime);		
		UpdateNotificationClear(modifiersCleared, modifierClearTime);	// last int = seconds before the clear is lifted
		UpdateNotificationClear(consumablesCleared, consumableClearTime);
		UpdateNotificationClear(landsCleared, landClearTime);		
		UpdateNotificationClear(charactersCleared, characterClearTime);	
		UpdateNotificationClear(artifactSalesCleared, saleClearTime);	
		UpdateNotificationClear(consumableSalesCleared, saleClearTime);	
		UpdateNotificationClear(powerupSalesCleared, saleClearTime);	
		UpdateNotificationClear(characterSalesCleared, saleClearTime);	
	}
	
	private void UpdateNotificationClear(Dictionary<int,Int64> clearedList, int seconds)
	{
		List<int> listToClear = new List<int>();
		
		foreach (KeyValuePair<int,Int64> clear in clearedList)
		{
			if (clear.Value + seconds < appCounters.GetSecondsSpentInApp())
				listToClear.Add(clear.Key);	// remove clear from list when current time becomes greater than time cleared + time passed value
		}	
		
		foreach (int idToClear in listToClear)	
			clearedList.Remove(idToClear);	// clearedList.Remove(clear.Key);
	}
	
	private List<int> GetActualNotifications(List<int> actionableList, Dictionary<int,Int64> clearedList)	// ones that aren't cleared, so will be shown
	{
		List<int> notificationList = new List<int>();
		
		foreach (int itemID in actionableList)	
		{
			if (!clearedList.ContainsKey(itemID))
				notificationList.Add(itemID);
		}
		
		return notificationList;
	}	
	
	private bool IsPowerupPurchasable(int powerupId)
	{
		return (!GameProfile.SharedInstance.Player.IsPowerPurchased(powerupId) &&		// check if already purchased
				GameProfile.SharedInstance.Player.CanAffordPower(powerupId));			// check if can purchase	
	}		
	
	private List<int> GetPowerupsPurchasable(PlayerStats playerStats)
	{
		List<int> purchaseableList = new List<int>();
		
		foreach (BasePower powerupData in PowerStore.Powers)
		{
			if (IsPowerupPurchasable(powerupData.PowerID))
			{
				purchaseableList.Add(powerupData.PowerID);
			}
		}
		
		return purchaseableList;
	}	
	
	private bool IsModifierPurchasable(int artifactId)
	{
		return (!GameProfile.SharedInstance.Player.IsArtifactPurchased(artifactId) &&	// check if already purchased
			GameProfile.SharedInstance.Player.CanAffordArtifact(artifactId) &&			// check if can purchase
			!GameProfile.SharedInstance.Player.IsArtifactMaxedOut(artifactId));			// check if maxed out
	}		
	
	private List<int> GetModifiersPurchasable(PlayerStats playerStats)
	{
		List<int> purchaseableList = new List<int>();
		
		foreach (ArtifactProtoData artifactData in ArtifactStore.Artifacts)
		{
			if (IsModifierPurchasable(artifactData._id))
			{
				purchaseableList.Add(artifactData._id);
			}
		}
		
		return purchaseableList;
	}
	
	private bool IsConsumablePurchasable(int consumableId)
	{
		return (GameProfile.SharedInstance.Player.CanAffordConsumable(consumableId) &&	// check if can purchase
			!GameProfile.SharedInstance.Player.IsConsumableMaxedOut(consumableId));		// check if maxed out
	}	
	
	private List<int> GetConsumablesPurchasable(PlayerStats playerStats)
	{
		List<int> purchaseableList = new List<int>();
		
		foreach (BaseConsumable consumableData in ConsumableStore.consumablesList)
		{
			if (IsConsumablePurchasable(consumableData.PID))
			{
				purchaseableList.Add(consumableData.PID);
			}
		}
		
		return purchaseableList;
	}		

	private List<int> GetLandsDownloadable(PlayerStats playerStats)
	{
		List<int> downloadableList = new List<int>();
		
		foreach (EnvironmentSetBootstrapData bootData in EnvironmentSetBootstrap.BootstrapList)
		{
			EnvironmentSetData envSetData = EnvironmentSetManager.SharedInstance.AllCode2Dict[bootData.SetCode];
			
			if (!bootData.Embedded && !EnvironmentSetManager.SharedInstance.IsLocallyAvailableAndLatestVersion(envSetData.SetId))
			{
				downloadableList.Add(envSetData.SetId);					// if all true, add it to 'downloadable' list
			}
		}
		
		return downloadableList;
	}
	
	private bool IsCharacterPurchasable(int characterId)
	{
		return (!GameProfile.SharedInstance.Player.IsHeroPurchased(characterId) &&		// check if already purchased
			GameProfile.SharedInstance.Player.CanAffordHero(characterId));				// check if can purchase
	}
	
	private List<int> GetCharactersPurchasable(PlayerStats playerStats)
	{
		List<int> purchaseableList = new List<int>();
		
		foreach (CharacterStats characterStats in GameProfile.SharedInstance.Characters)
		{
			if (IsCharacterPurchasable(characterStats.characterId))
			{
				purchaseableList.Add(characterStats.characterId);
			}
		}
		
		return purchaseableList;
	}

	/// <summary>
	/// Check if the discount index in range for the pass Discount Item Type
	/// </summary>
	/// <returns>
	/// true if the index is within range
	/// </returns>
	/// <param name='discountItemType'>
	/// If set to <c>true</c> discount item type.
	/// </param>
	/// <param name='index'>
	/// If set to <c>true</c> index.
	/// </param>
	private bool _isDiscountIndexInRange( DiscountItemType discountItemType, int index )
	{
		bool result = false;
		
		switch ( discountItemType )
		{
			case DiscountItemType.Artifact:
				if ( ArtifactStore.GetArtifactProtoData( index ) != null )
				{
					result = true;
				}
				break;
			
			case DiscountItemType.Consumable:
				if ( ConsumableStore.ConsumableFromID( index) != null )
				{
					result = true;
				}
				break;
				
			case DiscountItemType.Character:
				if ( GameProfile.SharedInstance.CharacterOrder.Contains( index ) )
				{
					result = true;
				}
				break;
				
			case DiscountItemType.Powerup:
				if ( PowerStore.PowerFromID( index ) != null )
				{
					result = true;
				}
				break;
		}

		return result;
	}

	public List<WeeklyDiscountProtoData> GetSalesViewable(DiscountItemType discountItemType)
	{
		List<WeeklyDiscountProtoData> discounts =
			Services.Get<Store>().GetComponent<WeeklyDiscountManager>().GetDiscountsByItemType(discountItemType);
	
		List<DiscountItemProtoData> itemsToRemove = new List<DiscountItemProtoData>();
		
		if (discounts.Count > 0)
		{
			foreach (DiscountItemProtoData item in discounts.First()._itemList)
			{
				notify.Debug("[NotificationSystem] - Item Type: " + discountItemType.ToString() + "  ID " + item._id.ToString() );
				
				// To prevent the IndexOutOfRangeException that occurred 05-13-2013, ensure that the
				// current item's type matches the sale's type, and that the item's id is within the 
				// particular store's
				if ( item.ItemType == discountItemType && _isDiscountIndexInRange( discountItemType, item._id ) )
				{
					switch (discountItemType)
					{
						case DiscountItemType.Artifact:
							if (GameProfile.SharedInstance.Player.IsArtifactMaxedOut(item._id))
							{
								itemsToRemove.Add(item);		
							}
							break;
						case DiscountItemType.Consumable:
							if (GameProfile.SharedInstance.Player.IsConsumableMaxedOut(item._id))
							{
								itemsToRemove.Add(item);						
							}
							break;
						case DiscountItemType.Powerup:
							if (GameProfile.SharedInstance.Player.IsPowerPurchased(item._id))
							{
								itemsToRemove.Add(item);
							}
							break;
						case DiscountItemType.Character:
							if (GameProfile.SharedInstance.Player.IsHeroPurchased(item._id))
							{
								itemsToRemove.Add(item);
							}
							break;
					}
				}
				// Index is either out of range, or the item type doesn't match the sales
				else
				{
					if ( !_isDiscountIndexInRange( discountItemType, item._id ) )
					{
						notify.Error( string.Format( "[NotificationSystem] GetSalesViewable -- Index out of range.  Id: {0}, ShortCode: {1}, ItemType{2}.  Get Bryan A. or Redmond.", 
							item._id, item.ShortCode, item.ItemType ) );
					}
					
					if ( item.ItemType != discountItemType )
					{
						notify.Error( string.Format( "[NotificationSystem] GetSalesViewable -- Item type mismatch.  Id: {0}, ShortCode: {1}, ItemType: {2}, Sale's Type: {3}.  Get Bryan A. or Redmond", 
							item._id, item.ShortCode, item.ItemType, discountItemType ) );
					}
				}
			}
			
			foreach (DiscountItemProtoData item in itemsToRemove)
			{
				discounts.First()._itemList.Remove(item);
			}
		
		}
		
		return discounts;
	}	
	
	public List<int> GetIDsFromDiscountList(List<WeeklyDiscountProtoData> discountList)
	{	
		List<int> idList = new List<int>();
		
		if (discountList.Count > 0)
		{
			foreach (DiscountItemProtoData item in discountList.First()._itemList)
			{
				idList.Add(item._id);
			}
		}
		
		return idList;
	}
}


	
	





				
				//if (GetActualNotifications(powerupsPurchasable, powerupsCleared).Contains(id))
				
				//if (GetActualNotifications(modifiersPurchasable, modifiersCleared).Contains(id))
				
				//if (GetActualNotifications(consumablesPurchasable, consumablesCleared).Contains(id))
				
					//if (GetActualNotifications(charactersPurchasable, charactersCleared).Contains(id))

				//if (landsDownloadable.Count > 0 && !landsCleared.ContainsKey(id) && id == 1)	// 'Lands of Oz' cell only, which has ID of 1
				//if (GetActualNotifications(landsDownloadable, landsCleared).Contains(id))
				//	return true;
				//break;					
				


				//mainVC.SetNotificationIcon(1, (totalChallenges == 0) ? -1 : totalChallenges);			// btn_challenges
			
				//ObjectivesVC.SetNotificationIcon(0, (weeklyCompleted == 0) ? -1 : weeklyCompleted);	// tab_weekly_challenges


//
//				if (powerupsPurchasable.Contains(id))
//				{
//					if (!powerupsCleared.ContainsKey(id))
//						powerupsCleared.Add(id,appCounters.GetSecondsSpentInApp());
//				}
//				break;
//			case NotificationType.Modifier:
//				if (modifiersPurchasable.Contains(id))
//				{
//					if (!modifiersCleared.ContainsKey(id))
//						modifiersCleared.Add(id,appCounters.GetSecondsSpentInApp());
//				}
//				break;			
//			case NotificationType.Consumable:
//				if (consumablesPurchasable.Contains(id))
//				{
//					if (!consumablesCleared.ContainsKey(id))
//						consumablesCleared.Add(id,appCounters.GetSecondsSpentInApp());
//				}
//				break;
//			case NotificationType.Character:
//				if (charactersPurchasable.Contains(id))
//				{
//					if (!charactersCleared.ContainsKey(id))
//						charactersCleared.Add(id,appCounters.GetSecondsSpentInApp());
//				}
//				break;	
//			case NotificationType.Land:
//				if (landsDownloadable.Contains(id))
//				{
//					if (!landsCleared.ContainsKey(id))
//						landsCleared.Add(id,appCounters.GetSecondsSpentInApp());
//				}
//				break;
//			case NotificationType.MoreCoins:
//				break;		



//		string pref = PlayerPrefs.GetString("powerupsCleared", "{}");	// get string from player prefs
//		//pref = (pref == "{}") ? pref : StringCompressor.DecompressString(pref);	// decompress, if applicable
//		WrappedDict wrappedDict1 = new WrappedDict();
//		SerializationUtils.FromJson(wrappedDict1, pref);
//		powerupsCleared = wrappedDict1.dict;	//powerupsCleared = SerializationUtils.FromJson(pref) as Dictionary<int,Int64>;
//		notify.Debug("Loading cleared notification list (powerupsCleared): " + pref);
//		
//		pref = PlayerPrefs.GetString("modifiersCleared", "{}");	// get string from player prefs
//		//pref = (pref == "{}") ? pref : StringCompressor.DecompressString(pref);	// decompress, if applicable
//		WrappedDict wrappedDict2 = new WrappedDict();
//		SerializationUtils.FromJson(wrappedDict2, pref);
//		modifiersCleared = wrappedDict2.dict;	//modifiersCleared = MiniJSON.Json.Deserialize(pref) as Dictionary<int,Int64>;
//		notify.Debug("Loading cleared notification list (modifiersCleared): " + pref);
//		
//		pref = PlayerPrefs.GetString("consumablesCleared", "{}");	// get string from player prefs
//		//pref = (pref == "{}") ? pref : StringCompressor.DecompressString(pref);	// decompress, if applicable
//		WrappedDict wrappedDict3 = new WrappedDict();
//		SerializationUtils.FromJson(wrappedDict3, pref);
//		consumablesCleared = wrappedDict3.dict;	//consumablesCleared = SerializationUtils.FromJson(pref) as Dictionary<int,Int64>;
//		notify.Debug("Loading cleared notification list (consumablesCleared): " + pref);	
//		
//		pref = PlayerPrefs.GetString("charactersCleared", "{}");	// get string from player prefs
//		//pref = (pref == "{}") ? pref : StringCompressor.DecompressString(pref);	// decompress, if applicable
//		WrappedDict wrappedDict4 = new WrappedDict();
//		SerializationUtils.FromJson(wrappedDict4, pref);
//		charactersCleared = wrappedDict4.dict;	//charactersCleared = SerializationUtils.FromJson(pref) as Dictionary<int,Int64>;
//		notify.Debug("Loading cleared notification list (charactersCleared): " + pref);			
//		
//		pref = PlayerPrefs.GetString("landsCleared", "{}");	// get string from player prefs
//		//pref = (pref == "{}") ? pref : StringCompressor.DecompressString(pref);	// decompress, if applicable
//		WrappedDict wrappedDict5 = new WrappedDict();
//		SerializationUtils.FromJson(wrappedDict5, pref);
//		landsCleared = wrappedDict5.dict;	//landsCleared = SerializationUtils.FromJson(pref) as Dictionary<int,Int64>;
//		notify.Debug("Loading cleared notification list (landsCleared): " + pref);			
//		
//		pref = PlayerPrefs.GetString("oneShotNotifications", "{}");	// get string from player prefs
//		//pref = (pref == "{}") ? pref : StringCompressor.DecompressString(pref);	// decompress, if applicable
//		WrappedDict wrappedDict6 = new WrappedDict();
//		SerializationUtils.FromJson(wrappedDict6, pref);
//		oneShotNotifications = wrappedDict6.dict;	//oneShotNotifications = SerializationUtils.FromJson(pref) as Dictionary<int,Int64>;
//		notify.Debug("Loading oneShotNotifications list (oneShotNotifications): " + pref);	



		//notify.Warning("ClearNotification called with parameters: type = " + type.ToString() + ", id = " + id.ToString()
		//	+ " at 'seconds spent in app' time: " + appCounters.GetSecondsSpentInApp());
		
		
				//string idList1 = "";
				//foreach (KeyValuePair<int,Int64> clearedKVP in powerupsCleared)
				//	idList1 = idList1 + clearedKVP.Key.ToString() + ",";			
				//notify.Debug("powerupsCleared = " + idList1);			
			
				//string idList2 = "";
				//foreach (KeyValuePair<int,Int64> clearedKVP in modifiersCleared)
				//	idList2 = idList2 + clearedKVP.Key.ToString() + ",";
				//notify.Debug("modifiersCleared = " + idList2);
			
				//string idList3 = "";
				//foreach (KeyValuePair<int,Int64> clearedKVP in modifiersCleared)
				//	idList3 = idList3 + clearedKVP.Key.ToString() + ",";
				//notify.Debug("consumablesCleared = " + idList3);			
			
				//if (!consumablesCleared.ContainsKey(id) && consumablesPurchasable.Contains(id))
				//	consumablesCleared.Add(id,appCounters.GetSecondsSpentInApp());
				//notify.Debug("consumablesCleared.Count = " + consumablesCleared.Count);				
					
		
		//RefreshAllNotifications();
		//UpdateAllNotificationClears();
		//SetNotificationIconsForThisPage(currentScreenName);	// update notification icons on current page


	//void Update () { }

			
//				for (int i=0; i<notificationIcons.notificationIcons.Count; i++)
//					SetNotification(i, -1);		


//	private void SetNotification(int buttonID, int iconValue)		// update actual icon onscreen
//	{
//		//notificationIcons.SetNotification(buttonID, iconValue);
//	}

//			case NotificationClearType.Map:
//				if (!modifiersCleared.ContainsKey(id))
//					modifiersCleared.Add(id,DateTime.UtcNow);
//				break;	

	
//	private void RestoreAllClearedNotificationsFromPlayerPrefs()
//	{
//		RestoreSingleClearedNotificationDict("modifiersCleared", modifiersCleared);
//		RestoreSingleClearedNotificationDict("powerupsCleared", powerupsCleared);
//		RestoreSingleClearedNotificationDict("consumablesCleared", consumablesCleared);
//	}
//	
//	private void RestoreSingleClearedNotificationDict(string prefString, Dictionary<int,Int64> dict)
//	{
//		string pref = PlayerPrefs.GetString("modifiersCleared", "{}");	// get string from player prefs
//		//pref = (pref == "{}") ? pref : StringCompression.UnZip(pref);	// decompress, if applicable
//		MiniJSON.Json.Deserialize(dict, pref);
//		notify.Warning("Loading cleared notification list (" + prefString + "): " + pref);
//	}		


		
//		var pruned = etimes.Where(entry => Convert.ToInt64(entry.Value) >= 
//    		Convert.ToInt64(stime)).ToDictionary(entry => entry.Key, 
//        	entry => entry.Value);
		
//		var keys = (from entry in etimes
//            where Convert.ToInt64(entry.Value) < Convert.ToInt64(stime)
//            select entry.Key).ToList();
//
//		foreach (var key in keys)
//		{
//		    etimes.Remove(key);
//		    count--;
//		}
		
		
		
		


		
//		foreach (GameObject icon in notificationIcons)
//		{
//			iconSprites.Add(icon.transform.Find("Background").GetComponent<UISprite>());
//			iconLabels.Add(icon.transform.Find("Label").GetComponent<UILabel>());
//			iconExclamations.Add(icon.transform.Find("SpriteExclamation").GetComponent<UISprite>());
//		}
//		
//		for (int i=0; i<5; i++)				// turn off all notification icons at launch
//			SetNotification(i,-1);



//	private void SetNotification(int buttonID, int iconValue)
//	{
//		if (iconValue < 0)			// if value < 0, turn off notification
//		{
//			iconSprites[buttonID].enabled = false;
//			iconLabels[buttonID].enabled = false;
//			iconExclamations[buttonID].enabled = false;
//		}
//		else if (iconValue == 0)	// if value == 0, show exclamation point 
//		{
//			iconSprites[buttonID].enabled = true;
//			iconLabels[buttonID].enabled = false;	//true;
//			//iconLabels[buttonID].text = "!";
//			iconExclamations[buttonID].enabled = true;
//		}
//		else if (iconValue > 0)		// if value > 0, show actual number (capped at 9)
//		{
//			iconSprites[buttonID].enabled = true;
//			iconLabels[buttonID].enabled = true;
//			iconLabels[buttonID].text = (Math.Min(iconValue, 9)).ToString();
//			iconExclamations[buttonID].enabled = false;
//		}
//	}




//		string compressed = StringCompression.Zip(SerializationUtils.ToJson(modifiersCleared));
//		PlayerPrefs.SetString("modifiersCleared", SerializationUtils.ToJson(compressed));
//		
//		// repeat for 'powerups cleared'
//		compressed = StringCompression.Zip(SerializationUtils.ToJson(powerupsCleared));
//		PlayerPrefs.SetString("powerupsCleared", SerializationUtils.ToJson(compressed));
//		
//		// repeat for 'consumables cleared'
//		compressed = StringCompression.Zip(SerializationUtils.ToJson(consumablesCleared));
//		PlayerPrefs.SetString("consumablesCleared", SerializationUtils.ToJson(compressed));
//		


		
//		string modString = PlayerPrefs.GetString("modifiersCleared", "{}");					// get string from player prefs
//		modString = (modString == "{}") ? modString : StringCompression.UnZip(modString);	// decompress, if applicable
//		SerializationUtils.FromJson(modifiersCleared, modString);
//		
//		string powString = PlayerPrefs.GetString("powerupsCleared", "{}");
//		powString = (powString == "{}") ? powString : StringCompression.UnZip(powString);
//		SerializationUtils.FromJson(powerupsCleared, powString);
//		
//		
//		string conString = PlayerPrefs.GetString("consumablesCleared", "{}");
//		conString = (conString == "{}") ? conString : StringCompression.UnZip(conString);	
//		SerializationUtils.FromJson(consumablesCleared, conString);	


	
//	private List<T> ToListOf<T>(byte[] array, Func<byte[], int, T> bitConverter)
//	{
//    	var size = Marshal.SizeOf(typeof(T));
//    	return Enumerable.Range(0, array.Length / size)
//                     .Select(i => bitConverter(array, i * size))
//                     .ToList();
//	}
//	
//	private byte[] MakeByteArrayFromDict(Dictionary<int,int> dict)
//	{
//		List<KeyValuePair<int,int>> list = dict.ToList();	
//		byte[] byteArray = (byte[])list.ToArray();
//		return byteArray;
//	}
//	
//	private Dictionary<int,int> MakeDictFromByteArray(byte[] byteArray)
//	{
//		//var dict = new Dictionary<int,int>();
//		
//		List<int> list = ToListOf<int>(byteArray, BitConverter.ToInt32);
//		
//		foreach (string value in list)
//		{
//	    if (!exampleDictionary.ContainsKey(value))
//	    {
//			exampleDictionary.Add(value, 1);
//	    }
//		
//		
//		var exampleDictionary = new Dictionary<string, int>();
//		foreach (int value in list)
//		{
//		    if (!exampleDictionary.ContainsKey(value))
//				exampleDictionary.Add(value, 1);
//		}	
//			
//		
//		
//		List<KeyValuePair<int,int>> list = dict.ToList();	
//		byte[] byteArray = (byte[])list.ToArray();
//		return byteArray;
//			
//			
//
//		
//		byte[] byteArray = (byte[])modifiersCleared.ToArray();
//	
//	
//			
//		
//		
//		
//		
//		modifiersCleared = new Dictionary<int,int>();
//	private Dictionary<int,int> powerupsCleared = new Dictionary<int,int>();
//	private Dictionary<int,int> consumablesCleared = new Dictionary<int,int>();	
//		
//				byte[] byteArray = (byte[])objList.ToArray();			
//	}	
	