using UnityEngine;
using System.IO;
using System.Collections.Generic;


public class GameCircleUIManager : MonoBehaviour
{
#if UNITY_ANDROID
	void OnGUI()
	{
		float yPos = 5.0f;
		float xPos = 5.0f;
		float width = 320;
		float height = 80;
		float heightPlus = height + 10.0f;
	
	
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Init" ) )
		{
			GameCircle.init( false );
		}
	
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Submit Score" ) )
		{
			GameCircle.submitScore( "leaderboard_1", Random.Range( 10, 99999 ) );
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Request Leaderboards" ) )
		{
			GameCircle.requestLeaderboards();
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Show Leaderboard" ) )
		{
			GameCircle.showLeaderboardsOverlay();
		}
			
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Request Local Players Score" ) )
		{
			GameCircle.requestLocalPlayerScore( "leaderboard_1", GameCircleLeaderboardScope.GlobalAllTime );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Request All Scores" ) )
		{
			GameCircle.requestScores( "leaderboard_1", GameCircleLeaderboardScope.GlobalAllTime );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Do Not Auto Unpack Synced Data" ) )
		{
			GameCircle.setShouldAutoUnpackSyncedData( false );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Manually Unpack Synced Data" ) )
		{
			GameCircle.whisperSyncUnpackNewMultiFileGameData();
		}
		
	
	
		xPos = Screen.width - width - 5.0f;
		yPos = 5.0f;
		
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Update Achievement" ) )
		{
			GameCircle.updateAchievementProgress( "achievement_1", Random.Range( 10, 100 ) );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Show Achievements Overlay" ) )
		{
			GameCircle.showAchievementsOverlay();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Set Popup Location" ) )
		{
			GameCircle.setPopUpLocation( GameCirclePopupLocation.BOTTOM_RIGHT );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Load Icon" ) )
		{
			GameCircle.loadIcon( "achievement_1" );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Whisper Sync Synchronize Progress" ) )
		{
			// dump a file to disk and sync it. Note that only files placed in the /data/data/YOUR_BUNDLE_ID folder will be synchronized
			// the AGSAndroid.rootWhisperSyncFolder field stores the correct folder as a convenience for you.
			var path = Path.Combine( GameCircle.rootWhisperSyncFolder, "file.txt" );
			var someText = "blah blah blah blah text";
			File.WriteAllText( path, someText );
			
			Debug.Log( "dumping file to: " + path );
			
			GameCircle.whisperSyncSynchronizeProgress( "description_of_progress", "txt", GameCircleConflictStrategy.AUTO_RESOLVE_TO_CLOUD );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Dump Sync Directory To Log" ) )
		{
			foreach( var file in Directory.GetFiles( GameCircle.rootWhisperSyncFolder ) )
				Debug.Log( string.Format( "[{0}] {1}", file, File.ReadAllText( file ) ) );
		}

		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Whisper Sync Synchronize" ) )
		{
			GameCircle.whisperSyncSynchronize();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Whisper Sync Request Revert" ) )
		{
			GameCircle.whisperSyncRequestRevert();
		}

	}
#endif
}
