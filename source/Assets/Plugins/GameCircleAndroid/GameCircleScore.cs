using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GameCircleScore
{
	public string playerAlias;
	public int rank;
	public string scoreString;
	public long scoreValue;
	
	
	public static GameCircleScore fromHashtable( Hashtable ht )
	{
		var score = new GameCircleScore();
		score.playerAlias = ht["playerAlias"].ToString();
		score.rank = int.Parse( ht["rank"].ToString() );
		score.scoreString = ht["scoreString"].ToString();
		score.scoreValue = long.Parse( ht["scoreValue"].ToString() );
		
		return score;
	}
	
	
	public static List<GameCircleScore> fromArrayList( ArrayList list )
	{
		var scores = new List<GameCircleScore>();
		
		foreach( Hashtable ht in list )
			scores.Add( GameCircleScore.fromHashtable( ht ) );
		
		return scores;
	}
	
	
	public override string ToString()
	{
		return string.Format( "playerAlias: {0}, rank: {1}, scoreString: {2}", playerAlias, rank, scoreString );
	}
	
}
