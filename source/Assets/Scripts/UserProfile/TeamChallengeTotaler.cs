using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamChallengeTotaler
{
	protected static Notify notify = new Notify( "TeamChallengeTotaler" );
	
	private static Dictionary<int,int> _totalValueByChallenge = new Dictionary<int, int>();

	// Use this for initialization
	/*
	void Start ()
	{
		
	}
	*/
	
	public static void RefreshData( List<NeighborProtoData> neighborList)
	{		
		if ( neighborList == null || neighborList.Count == 0 )
		{
			return;
		}
		
		_totalValueByChallenge.Clear();
		
		foreach( NeighborProtoData neighbor in neighborList )
		{
			if ( neighbor.ChallengeProgressList != null && neighbor.ChallengeProgressList.Count > 0 )
			{
				foreach ( GuildChallengeProtoData challengeProgress in neighbor.ChallengeProgressList )
				{
					if ( _totalValueByChallenge.ContainsKey( challengeProgress._challengeIndex ) )
					{
						_totalValueByChallenge[ challengeProgress._challengeIndex ] += challengeProgress._conditionValue;
					}
					else
					{
						_totalValueByChallenge.Add( challengeProgress._challengeIndex, challengeProgress._conditionValue );
					}
				}
			}
		}
	}
	
	public static int GetChallengeNeighborTotal( int challengeIndex )
	{
		if ( _totalValueByChallenge.ContainsKey( challengeIndex ) )
		{
			return _totalValueByChallenge[ challengeIndex ];
		}
		else
		{
			return 0;
		}
	}
	
	// Update is called once per frame
	/*
	void Update ()
	{
	
	}
	*/
}

