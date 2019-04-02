using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GameCircleManager : MonoBehaviour
{
#if UNITY_ANDROID
	// Fired when the AGS server is ready for use
	public static event Action serviceReadyEvent;
	
	// Fires when a problem occurred initializing AGS. The string parameter will have the error description.
	public static event Action<string> serviceNotReadyEvent;

	// Fired when the player alias is received
	public static event Action<string> playerAliasReceivedEvent;
	
	// Fired when the request to load the player alias fails
	public static event Action<string> playerAliasFailedEvent;
	
	// Fired when submitting a score fails
	public static event Action<string> submitScoreFailedEvent;
	
	// Fired when submitting a score succeeds
	public static event Action submitScoreSucceededEvent;
	
	// Fired when the leaderboard request fails with the error description
	public static event Action<string> requestLeaderboardsFailedEvent;
	
	// Fired when the leaderboard request has finished loading
	public static event Action<List<GameCircleLeaderboard>> requestLeaderboardsSucceededEvent;
	
	// Fired when requesting the local players score fails
	public static event Action<string> requestLocalPlayerScoreFailedEvent;
	
	// Fired when requesting the local players score succeeds. Sends the rank and score
	public static event Action<string,string> requestLocalPlayerScoreSucceededEvent;
	
	// Fired when requesting scores fails
	public static event Action<string> requestScoresFailedEvent;
	
	// Fired when requesting scores succeeds
	public static event Action<GameCircleLeaderboard> requestScoresSucceededEvent;
	
	// Fired when updating an achievement fails
	public static event Action<string> updateAchievementFailedEvent;
	
	// Fired when updating an achievement succeeds
	public static event Action updateAchievementSucceededEvent;
	
	// Fired when loading an icon fails
	public static event Action<string> loadIconFailedEvent;
	
	// Fire when loading an icon succeeds
	public static event Action<string> loadIconSucceededEvent;
	
	// Fired when a sync is requested and AGS is already synchronizied
	public static event Action onAlreadySynchronizedEvent;
	
	// Fired when a sync requests conflict deferred method is called
	public static event Action onConflictDeferralEvent;
	
	// Fired when a sync request is uploads data successfully
	public static event Action onGameUploadSuccessEvent;
	
	// Fired when a sync request fails
	public static event Action<string> onSynchronizeFailureEvent;
	
	// Fired when new game data arrives. By default it will be unpacked automatically for you
	public static event Action onNewGameDataEvent;
	
	// Fired when a player cancels the sync
	public static event Action onPlayerCancelledEvent;
	
	// Fired when a whispersync revert fails
	public static event Action<string> onRevertFailureEvent;
	
	// Fired when a whispersync revert succeeds. By default it will be unpacked automatically for you.
	public static event Action onRevertedGameDataEvent;

	
	
	void Awake()
	{
		// Set the GameObject name to the class name for easy access from native code
		gameObject.name = this.GetType().ToString();
		DontDestroyOnLoad( this );
	}


	public void serviceReady( string empty )
	{
		GameCircle.requestLocalPlayerProfile();
		
		if( serviceReadyEvent != null )
			serviceReadyEvent();
	}


	public void serviceNotReady( string param )
	{
		if( serviceNotReadyEvent != null )
			serviceNotReadyEvent( param );
	}
	
	
	public void playerAliasReceived( string playerAlias )
	{
		if( playerAliasReceivedEvent != null )
			playerAliasReceivedEvent( playerAlias );
	}
	
	
	public void playerAliasFailed( string error )
	{
		if( playerAliasFailedEvent != null )
			playerAliasFailedEvent( error );
	}
	

	public void submitScoreFailed( string param )
	{
		if( submitScoreFailedEvent != null )
			submitScoreFailedEvent( param );
	}


	public void submitScoreSucceeded( string empty )
	{
		if( submitScoreSucceededEvent != null )
			submitScoreSucceededEvent();
	}


	public void requestLeaderboardsFailed( string param )
	{
		if( requestLeaderboardsFailedEvent != null )
			requestLeaderboardsFailedEvent( param );
	}


	public void requestLeaderboardsSucceeded( string json )
	{
		if( requestLeaderboardsSucceededEvent != null )
		{
			var leaderboards = new List<GameCircleLeaderboard>();
			var arrayList = json.arrayListFromJson();
			foreach( Hashtable ht in arrayList )
				leaderboards.Add( GameCircleLeaderboard.fromHashtable( ht ) );
			
			requestLeaderboardsSucceededEvent( leaderboards );
		}
	}


	public void requestLocalPlayerScoreFailed( string param )
	{
		if( requestLocalPlayerScoreFailedEvent != null )
			requestLocalPlayerScoreFailedEvent( param );
	}


	public void requestLocalPlayerScoreSucceeded( string scoreInfo )
	{
		if( requestLocalPlayerScoreSucceededEvent != null )
		{
			var parts = scoreInfo.Split( new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries );
			if( parts.Length == 2 )
				requestLocalPlayerScoreSucceededEvent( parts[0], parts[1] );
		}
	}

	
	public void requestScoresFailed( string error )
	{
		if( requestScoresFailedEvent != null )
			requestScoresFailedEvent( error );
	}


	public void requestScoresSucceeded( string json )
	{
		if( requestScoresSucceededEvent != null )
		{
			var leaderboard = GameCircleLeaderboard.fromHashtable( json.hashtableFromJson() );
			requestScoresSucceededEvent( leaderboard );
		}
	}


	public void updateAchievementFailed( string param )
	{
		if( updateAchievementFailedEvent != null )
			updateAchievementFailedEvent( param );
	}


	public void updateAchievementSucceeded( string empty )
	{
		if( updateAchievementSucceededEvent != null )
			updateAchievementSucceededEvent();
	}


	public void loadIconFailed( string param )
	{
		if( loadIconFailedEvent != null )
			loadIconFailedEvent( param );
	}


	public void loadIconSucceeded( string file )
	{
		if( loadIconSucceededEvent != null )
			loadIconSucceededEvent( file );
	}


	public void onAlreadySynchronized( string empty )
	{
		if( onAlreadySynchronizedEvent != null )
			onAlreadySynchronizedEvent();
	}


	public void onConflictDeferral( string empty )
	{
		if( onConflictDeferralEvent != null )
			onConflictDeferralEvent();
	}


	public void onGameUploadSuccess( string empty )
	{
		if( onGameUploadSuccessEvent != null )
			onGameUploadSuccessEvent();
	}


	public void onSynchronizeFailure( string error )
	{
		if( onSynchronizeFailureEvent != null )
			onSynchronizeFailureEvent( error );
	}


	public void onNewGameData( string empty )
	{
		if( onNewGameDataEvent != null )
			onNewGameDataEvent();
	}


	public void onPlayerCancelled( string empty )
	{
		if( onPlayerCancelledEvent != null )
			onPlayerCancelledEvent();
	}


	public void onRevertFailure( string error )
	{
		if( onRevertFailureEvent != null )
			onRevertFailureEvent( error );
	}


	public void onRevertedGameData( string empty )
	{
		if( onRevertedGameDataEvent != null )
			onRevertedGameDataEvent();
	}
#endif
}

