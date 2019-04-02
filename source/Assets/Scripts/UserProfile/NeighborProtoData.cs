using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class NeighborProtoData
{
	
	protected static Notify notify = new Notify("NeighborProtoData");
	
	public string _dbId = "";
	public string _gcId = "";
	public string _fbId = "";
	public string _name = "";
	public int _bestScore = 0;
	public int _bestMeters = 0;
	public int _bestCoins = 0;
	public int _bestGems = 0;
	public bool _passedNeighbor = false;
	
	public List<GuildChallengeProtoData> ChallengeProgressList = new List<GuildChallengeProtoData>();
	
	public NeighborProtoData(Dictionary<string,object> dict)
	{
		if ( dict.ContainsKey( "_id" ) ) 
		{ 
			_dbId = (string) dict["_id"]; 
		}
		
		if ( dict.ContainsKey( "_gcId" ) ) 
		{ 
			_gcId = (string) dict["_id"]; 
		}
		else if ( dict.ContainsKey( "gcId" ) ) 
		{ 
			_gcId = (string) dict["_gcId"]; 
		}
		
		if ( dict.ContainsKey( "_fbId" ) ) 
		{ 
			_fbId = (string) dict["_fbId"]; 
		}
		else if ( dict.ContainsKey( "fbId" ) ) 
		{ 
			_fbId = (string) dict["fbId"]; 
		}
		
		if ( dict.ContainsKey( "name" ) ) 
		{ 
			_name = (string) dict["name"]; 
		}
		
		if ( dict.ContainsKey( "bestScore" ) ) 
		{ 
			_bestScore = int.Parse(dict["bestScore"].ToString()); 
		}
		
		if ( dict.ContainsKey( "bestMeters" ) )
		{ 
			_bestMeters = int.Parse(dict["bestMeters"].ToString()); 
		}
		
		//if bestMeters is 0, set passed neighbor to be true
		if ( _bestMeters == 0 ) 
		{
			_passedNeighbor = true;
		}
		
		notify.Debug("User ID" + _dbId + " Best Meters: " + _bestMeters);
		
		if ( dict.ContainsKey( "bestCoins" ) ) 
		{
			_bestCoins = int.Parse(dict["bestCoins"].ToString()); 
		}
		
		if ( dict.ContainsKey( "bestGems" ) ) 
		{ 
			_bestGems = int.Parse(dict["bestGems"].ToString()); 
		}
		
		if ( dict.ContainsKey( "guildChallenge" ) )
		{
			//List<GuildChallengeProtoData> tempTeamChallenge = dict["guildChallenge"] as List<GuildChallengeProtoData>;
			List<object> tempTeamChallenge = dict["guildChallenge"] as List<object>;
			
			if ( tempTeamChallenge != null && tempTeamChallenge.Count > 0 )
			{
				notify.Debug( "[NeighborProtoData] Constructor() - " + tempTeamChallenge.Count.ToString() );
				
				ChallengeProgressList.Clear();
				
				foreach ( object challengeObj in tempTeamChallenge )
				{	
					Dictionary<string, object> challengeDict = challengeObj as Dictionary<string, object>;
					
					GuildChallengeProtoData gcProtoData = new GuildChallengeProtoData(challengeDict);
						
					//ChallengeProgressList.Add( challenge );
					
					notify.Debug( "[NeighborProtoData] Attempting to add team progress" );
					
					if ( gcProtoData != null )
					{
						ChallengeProgressList.Add( gcProtoData );
						notify.Debug( "[NeighborProtoData] Team Progress Added" );
					}
				}
			}
		}
	}
	
	public string ToJson () { return MiniJSON.Json.Serialize(this.ToDict()); }
	
	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> d = new Dictionary<string, object>();
		d.Add( "_id",_dbId );
		d.Add( "_gcId", _gcId );
		d.Add( "_fbId", _fbId );
		d.Add( "name", _name );
		d.Add( "bestScore", _bestScore );
		d.Add( "bestMeters", _bestMeters );
		d.Add( "bestCoins", _bestCoins );
		d.Add( "bestGems", _bestGems );
		d.Add( "guildChallenge", ChallengeProgressList );
		
		return d;
	}
}

