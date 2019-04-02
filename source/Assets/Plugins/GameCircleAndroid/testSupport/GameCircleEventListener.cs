using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GameCircleEventListener : MonoBehaviour
{
#if UNITY_ANDROID
	void OnEnable()
	{
		// Listen to all events for illustration purposes
		GameCircleManager.serviceReadyEvent += serviceReadyEvent;
		GameCircleManager.serviceNotReadyEvent += serviceNotReadyEvent;
		GameCircleManager.playerAliasReceivedEvent += playerAliasReceivedEvent;
		GameCircleManager.playerAliasFailedEvent += playerAliasFailedEvent;
		GameCircleManager.submitScoreFailedEvent += submitScoreFailedEvent;
		GameCircleManager.submitScoreSucceededEvent += submitScoreSucceededEvent;
		GameCircleManager.requestLeaderboardsFailedEvent += requestLeaderboardsFailedEvent;
		GameCircleManager.requestLeaderboardsSucceededEvent += requestLeaderboardsSucceededEvent;
		GameCircleManager.requestLocalPlayerScoreFailedEvent += requestLocalPlayerScoreFailedEvent;
		GameCircleManager.requestLocalPlayerScoreSucceededEvent += requestLocalPlayerScoreSucceededEvent;
		GameCircleManager.requestScoresFailedEvent += requestScoreFailedEvent;
		GameCircleManager.requestScoresSucceededEvent += requestScoreSucceededEvent;
		GameCircleManager.updateAchievementFailedEvent += updateAchievementFailedEvent;
		GameCircleManager.updateAchievementSucceededEvent += updateAchievementSucceededEvent;
		GameCircleManager.loadIconFailedEvent += loadIconFailedEvent;
		GameCircleManager.loadIconSucceededEvent += loadIconSucceededEvent;
		GameCircleManager.onAlreadySynchronizedEvent += onAlreadySynchronizedEvent;
		GameCircleManager.onConflictDeferralEvent += onConflictDeferralEvent;
		GameCircleManager.onGameUploadSuccessEvent += onGameUploadSuccessEvent;
		GameCircleManager.onSynchronizeFailureEvent += onSynchronizeFailureEvent;
		GameCircleManager.onNewGameDataEvent += onNewGameDataEvent;
		GameCircleManager.onPlayerCancelledEvent += onPlayerCancelledEvent;
		GameCircleManager.onRevertFailureEvent += onRevertFailureEvent;
		GameCircleManager.onRevertedGameDataEvent += onRevertedGameDataEvent;
	}


	void OnDisable()
	{
		// Remove all event handlers
		GameCircleManager.serviceReadyEvent -= serviceReadyEvent;
		GameCircleManager.serviceNotReadyEvent -= serviceNotReadyEvent;
		GameCircleManager.playerAliasReceivedEvent -= playerAliasReceivedEvent;
		GameCircleManager.playerAliasFailedEvent -= playerAliasFailedEvent;
		GameCircleManager.submitScoreFailedEvent -= submitScoreFailedEvent;
		GameCircleManager.submitScoreSucceededEvent -= submitScoreSucceededEvent;
		GameCircleManager.requestLeaderboardsFailedEvent -= requestLeaderboardsFailedEvent;
		GameCircleManager.requestLeaderboardsSucceededEvent -= requestLeaderboardsSucceededEvent;
		GameCircleManager.requestLocalPlayerScoreFailedEvent -= requestLocalPlayerScoreFailedEvent;
		GameCircleManager.requestLocalPlayerScoreSucceededEvent -= requestLocalPlayerScoreSucceededEvent;
		GameCircleManager.requestScoresFailedEvent -= requestScoreFailedEvent;
		GameCircleManager.requestScoresSucceededEvent -= requestScoreSucceededEvent;
		GameCircleManager.updateAchievementFailedEvent -= updateAchievementFailedEvent;
		GameCircleManager.updateAchievementSucceededEvent -= updateAchievementSucceededEvent;
		GameCircleManager.loadIconFailedEvent -= loadIconFailedEvent;
		GameCircleManager.loadIconSucceededEvent -= loadIconSucceededEvent;
		GameCircleManager.onAlreadySynchronizedEvent -= onAlreadySynchronizedEvent;
		GameCircleManager.onConflictDeferralEvent -= onConflictDeferralEvent;
		GameCircleManager.onGameUploadSuccessEvent -= onGameUploadSuccessEvent;
		GameCircleManager.onSynchronizeFailureEvent -= onSynchronizeFailureEvent;
		GameCircleManager.onNewGameDataEvent -= onNewGameDataEvent;
		GameCircleManager.onPlayerCancelledEvent -= onPlayerCancelledEvent;
		GameCircleManager.onRevertFailureEvent -= onRevertFailureEvent;
		GameCircleManager.onRevertedGameDataEvent -= onRevertedGameDataEvent;
	}



	void serviceReadyEvent()
	{
		Debug.Log( "serviceReadyEvent" );
	}


	void serviceNotReadyEvent( string param )
	{
		Debug.Log( "serviceNotReadyEvent: " + param );
	}
	
	
	void playerAliasReceivedEvent( string playerAlias )
	{
		Debug.Log( "playerAliasReceivedEvent: " + playerAlias );
	}
	
	
	void playerAliasFailedEvent( string error )
	{
		Debug.Log( "playerAliasFailedEvent: " + error );
	}


	void submitScoreFailedEvent( string param )
	{
		Debug.Log( "submitScoreFailedEvent: " + param );
	}


	void submitScoreSucceededEvent()
	{
		Debug.Log( "submitScoreSucceededEvent" );
	}


	void requestLeaderboardsFailedEvent( string param )
	{
		Debug.Log( "requestLeaderboardsFailedEvent: " + param );
	}


	void requestLeaderboardsSucceededEvent( List<GameCircleLeaderboard> leaderboards )
	{
		Debug.Log( "requestLeaderboardsSucceededEvent" );
		foreach( var l in leaderboards )
			Debug.Log( l );
	}


	void requestLocalPlayerScoreFailedEvent( string error )
	{
		Debug.Log( "requestLocalPlayerScoreFailedEvent: " + error );
	}


	void requestLocalPlayerScoreSucceededEvent( string rank, string score )
	{
		Debug.Log( "requestLocalPlayerScoreSucceededEvent with rank: " + rank + ", score: " + score );
	}
	

	void requestScoreFailedEvent( string error )
	{
		Debug.Log( "requestScoreFailedEvent: " + error );
	}


	void requestScoreSucceededEvent( GameCircleLeaderboard leaderboard )
	{
		Debug.Log( "requestScoreSucceededEvent: " + leaderboard );
		foreach( var s in leaderboard.scores )
			Debug.Log( s );
	}


	void updateAchievementFailedEvent( string param )
	{
		Debug.Log( "updateAchievementFailedEvent: " + param );
	}


	void updateAchievementSucceededEvent()
	{
		Debug.Log( "updateAchievementSucceededEvent" );
	}


	void loadIconFailedEvent( string param )
	{
		Debug.Log( "loadIconFailedEvent: " + param );
	}


	void loadIconSucceededEvent( string file )
	{
		Debug.Log( "loadIconSucceededEvent: " + file );
	}


	void onAlreadySynchronizedEvent()
	{
		Debug.Log( "onAlreadySynchronizedEvent" );
	}


	void onConflictDeferralEvent()
	{
		Debug.Log( "onConflictDeferralEvent" );
	}


	void onGameUploadSuccessEvent()
	{
		Debug.Log( "onGameUploadSuccessEvent" );
	}


	void onSynchronizeFailureEvent( string error )
	{
		Debug.Log( "onSynchronizeFailureEvent: " + error );
	}


	void onNewGameDataEvent()
	{
		Debug.Log( "onNewGameDataEvent" );
	}


	void onPlayerCancelledEvent()
	{
		Debug.Log( "onPlayerCancelledEvent" );
	}


	void onRevertFailureEvent( string error )
	{
		Debug.Log( "onRevertFailureEvent: " + error );
	}


	void onRevertedGameDataEvent()
	{
		Debug.Log( "onRevertedGameDataEvent" );
	}
#endif
}


