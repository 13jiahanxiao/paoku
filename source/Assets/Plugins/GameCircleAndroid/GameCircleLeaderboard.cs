using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GameCircleLeaderboard
{
	public string name;
	public string id;
	public string displayText;
	public string scoreFormat;
	public List<GameCircleScore> scores = new List<GameCircleScore>();
	
	
	public static GameCircleLeaderboard fromHashtable( Hashtable ht )
	{
		var board = new GameCircleLeaderboard();
		board.name = ht["name"].ToString();
		board.id = ht["id"].ToString();
		board.displayText = ht["displayText"].ToString();
		board.scoreFormat = ht["scoreFormat"].ToString();
		
		// handle scores if we have them
		if( ht.ContainsKey( "scores" ) && ht["scores"] is ArrayList )
		{
			var scoresList = ht["scores"] as ArrayList;
			board.scores = GameCircleScore.fromArrayList( scoresList );
		}
		
		return board;
	}
	
	
	public override string ToString()
	{
		return string.Format( "name: {0}, id: {1}, displayText: {2}, scoreFormat: {3}", name, id, displayText, scoreFormat );
	}
	
}
