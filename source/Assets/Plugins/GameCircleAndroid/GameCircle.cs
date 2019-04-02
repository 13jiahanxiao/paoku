using UnityEngine;
using System.Collections;



#if UNITY_ANDROID
public enum GameCircleLeaderboardScope
{
	GlobalAllTime,
	GlobalWeek,
	GlobalDay,
	FriendsAllTime
}


public enum GameCircleConflictStrategy
{
	AUTO_RESOLVE_TO_CLOUD,
	PLAYER_SELECT,
	AUTO_RESOLVE_TO_IGNORE
}


public enum GameCirclePopupLocation
{
	BOTTOM_LEFT,
	BOTTOM_CENTER,
	BOTTOM_RIGHT,
	TOP_LEFT,
	TOP_CENTER,
	TOP_RIGHT
}


public class GameCircle
{
	private static AndroidJavaObject _plugin;
	public static string rootWhisperSyncFolder;
	

	static GameCircle()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		// find the plugin instance
		using( var pluginClass = new AndroidJavaClass( "com.amazon.GameCirclePlugin" ) )
			_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );

		rootWhisperSyncFolder = string.Format( "/data/data/{0}", _plugin.Get<string>( "packageName" ) );
	}
	
	
	// Starts up the GameCircle system. Must be called before any other methods. If hasNoLocalGameProgress is true a synchronize request will automatically occur
	// after the service is started
	public static void init( bool hasNoLocalGameProgress )
	{
		init( hasNoLocalGameProgress, true, true, true );
	}
	
	public static void init( bool hasNoLocalGameProgress, bool supportsLeaderboards, bool supportsAchievements, bool supportsWhisperSync )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "init", hasNoLocalGameProgress, supportsLeaderboards, supportsAchievements, supportsWhisperSync );
	}
	
	
	// Sends a request to load the local player's profile. This is called automatically for you when GameCircle is intialized.
	public static void requestLocalPlayerProfile()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "requestLocalPlayerProfile" );
	}
	
	
	// Defaults to true. If true, whenever data is synced it is automatically unpacked for you.
	// If false, whenever data is synced you must call the whisperSyncUnpackNewMultiFileGameData method to unpack it
	public static void setShouldAutoUnpackSyncedData( bool shouldAutoUnpackSyncedData )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "setShouldAutoUnpackSyncedData", shouldAutoUnpackSyncedData );
	}


	// Submits a score
	public static void submitScore( string leaderboardId, long score )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "submitScore", leaderboardId, score );
	}


	// Shows the leaderboard overlay
	public static void showLeaderboardsOverlay()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "showLeaderboardsOverlay" );
	}


	// Sends a request to fetch all the leaderboard data
	public static void requestLeaderboards()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "requestLeaderboards" );
	}


	// Sends a request for the current players highest rank and score for the given leaderboardId
	public static void requestLocalPlayerScore( string leaderboardId, GameCircleLeaderboardScope scope )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "requestLocalPlayerScore", leaderboardId, (int)scope );
	}
	
	
	// Sends a request for all the the scores for the given leaderboardId
	public static void requestScores( string leaderboardId, GameCircleLeaderboardScope scope )
	{
		requestScores( leaderboardId, scope, 1, 1000 );
	}
	
	public static void requestScores( string leaderboardId, GameCircleLeaderboardScope scope, int startRank, int count )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "requestScores", leaderboardId, (int)scope, startRank, count );
	}
	

	// Updates the achievement. Progress is from 0 to 100.
	public static void updateAchievementProgress( string achievementId, float progress )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "updateAchievementProgress", achievementId, progress, string.Empty );
	}


	// Sets the location of any popup notifications
	public static void setPopUpLocation( GameCirclePopupLocation location )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "setPopUpLocation", location.ToString() );
	}


	// Shows the achievements overlay
	public static void showAchievementsOverlay()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "showAchievementsOverlay" );
	}


	// Sends a request to load the icon associated with the achievementId
	public static void loadIcon( string achievementId )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "loadIcon", achievementId );
	}

	
	public static void whisperSyncSynchronizeProgress( string description )
	{
		whisperSyncSynchronizeProgress( description, string.Empty, GameCircleConflictStrategy.AUTO_RESOLVE_TO_CLOUD );
	}
	
	// Sends a synchronize progress request optionally with a specific conflict strategy and filename extension filter. If a filenameExtensionFilter is provided,
	// only files that end with the filenameExtensionFilter will be synced
	public static void whisperSyncSynchronizeProgress( string description, string filenameExtensionFilter, GameCircleConflictStrategy conflictStrategy )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "whisperSyncSynchronizeProgress", description, filenameExtensionFilter, conflictStrategy.ToString() );
	}

	
	public static void whisperSyncSynchronize()
	{
		whisperSyncSynchronize( GameCircleConflictStrategy.AUTO_RESOLVE_TO_CLOUD );
	}
	
	// Sends a request to synchronize with optional conflict strategy
	public static void whisperSyncSynchronize( GameCircleConflictStrategy conflictStrategy )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "whisperSyncSynchronize", conflictStrategy.ToString() );
	}


	// Sends a request to revert to the previous whispersync data
	public static void whisperSyncRequestRevert()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "whisperSyncRequestRevert" );
	}


	// Unpacks data received from a sync. It is only necessary to call this if setShouldAutoUnpackSyncedData was set to false.
	public static void whisperSyncUnpackNewMultiFileGameData()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "unpackNewMultiFileGameData" );
	}

}
#endif
