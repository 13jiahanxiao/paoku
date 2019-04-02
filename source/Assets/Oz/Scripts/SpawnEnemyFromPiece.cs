using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum CoinLanePermission
{
	None,
	All,
	LeftOnly,
	RightOnly,
}


public class SpawnEnemyFromPiece : TrackPieceAttatchment
{
	
	
	
	[System.Serializable]
	public class SpList
	{
		public List<Transform> list = new List<Transform>();
		public List<GameObject> prefabs = new List<GameObject>();
		public Transform CoinStart;
		public Transform CoinEnd;
		public List<Transform> CoinSplineFull = new List<Transform>();
		
		public List<Transform> GemSpawns = new List<Transform>();
		
		public int Difficulty = 0;
		
		public CoinLanePermission coinLanes = CoinLanePermission.All;
		public List<float> CoinArcs = new List<float>();
		
		public Transform this[int index]
		{
			get { return list[index]; }
		}
	}
	public List<SpList> SpawnPointLists = new List<SpList>();
	
	public bool WaitToEnable = true;
	public bool ParentToPiece = true;
	
	private List<GameObject> spawnedObjects;
	
	private static GameObject EnemyPool;

	private List<Transform> spawns;
	
	private static int lastSpawnIndex = 0;
	
	private int decision = -1;
	private SpList Decision
	{
		get {
			if(decision<=-1) 	DecideOnSpawnList();
			if(decision>=0) 	return SpawnPointLists[decision];
			return null;
		}
	}
	
	public CoinLanePermission CurrentCoinPermission
	{
		get {
			SpList dec = Decision;
			if(dec!=null)
				return dec.coinLanes;
			return CoinLanePermission.All;
		}
	}
	
	public List<float> CurrentCoinArcs
	{
		get {
			SpList dec = Decision;
			if(dec!=null)
				return dec.CoinArcs;
			return new List<float>();
		}
	}
	
	
	private SplineNode coinSpline;
	public SplineNode CoinSpline
	{
		get { return coinSpline; }
	}
	
	public void OnSpawned()
	{
		//SpawnEnemies();
	}
	
	public override void OnEnable()
	{
		base.OnEnable();
		
		//SpawnEnemies();
		
//		for(int i=0;i<spawnedObjects.Count;i++)
//		{
//			if(spawnedObjects[i]!=null)
//				spawnedObjects[i].SetActiveRecursively(!WaitToEnable);
//		}
		
//		if(spawnedObjects==null || spawns==null)	return;
	
	}
	
	void Kill()
	{
		if(spawnedObjects!=null)
		{
			for(int i=0;i<spawnedObjects.Count;i++)
			{
				spawnedObjects[i].BroadcastMessage("Kill",SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	private void DecideOnSpawnList()
	{
		TrackPiece tp = null;
		Transform cur = transform;
		while(cur!=null && tp==null)
		{
			tp = cur.GetComponent<TrackPiece>();
			cur = cur.parent;
		}
		
		
		List<int> possibilityList = new List<int>();
		int difficulty = (tp==null || !tp.IsBalloon()) ? TrackBuilder.SharedInstance.MaxTrackPieceDifficulty :
														GamePlayer.SharedInstance.balloonDifficulty;
		for(int i=0;i<SpawnPointLists.Count;i++)
		{
			if(SpawnPointLists[i].Difficulty <= difficulty)
				possibilityList.Add(i);
		}
		
		if(possibilityList.Count==0)
		{
			notify.Warning("No obstacle set with the correct difficulty found!");
			return;
		}
		
		//If this choice was chosen last time, try once again.
		int choice = Random.Range(0,possibilityList.Count);
		if(choice==lastSpawnIndex)	choice = Random.Range(0,possibilityList.Count);
		
		decision = possibilityList[choice];
		
		lastSpawnIndex = choice;
	}
	
	public void SpawnEnemies(bool deferActivate = false)
	{	
		if((spawnedObjects==null || spawnedObjects.Count==0) && SpawnPointLists.Count!=0)
		{
			spawnedObjects = new List<GameObject>();
			
			if(decision<=-1)
				DecideOnSpawnList();
			
			if(Decision==null)	return;
			
			spawns = Decision.list;
			for(int i=0;i<spawns.Count;i++)
			{
				spawnedObjects.Add(PoolManager.Pools["Enemies"].Spawn(Decision.prefabs[i].transform, deferActivate).gameObject);

				//spawnedObjects[spawnedObjects.Count-1].SetActiveRecursively(!WaitToEnable);
				
				spawnedObjects[i].transform.position = spawns[i].transform.position;
				spawnedObjects[i].transform.rotation = spawns[i].transform.rotation;
			}
			
			SplineNode[] nodes = GetComponentsInChildren<SplineNode>(true);
		
			if(nodes.Length>=1)
			{
				if(Decision.CoinSplineFull!=null && Decision.CoinSplineFull.Count>=1)
				{
					SplineNode latestNode = null;
					for(int i=0;i<Decision.CoinSplineFull.Count;i++)
					{
						SplineNode newNode;
						if(nodes.Length>i)
							newNode = nodes[i];
						else
							newNode = latestNode.CreateNew();
						
						if(i>0)
							latestNode.Connect(newNode);
						
						latestNode = newNode;
						
						latestNode.transform.position = Decision.CoinSplineFull[i].transform.position;
						latestNode.transform.rotation = Decision.CoinSplineFull[i].transform.rotation;
						latestNode.cpNext = Vector3.forward;
						latestNode.cpLast = -Vector3.forward;
						//latestNode.cpNext = Decision.CoinSplineFull[i].transform.forward;
						//latestNode.cpLast = -Decision.CoinSplineFull[i].transform.forward;
						
						if(latestNode.last!=null) {
							latestNode.last.NormalizeFront();
							latestNode.NormalizeBack();
						}
					}
					latestNode.next=null;
				}
				else if(nodes.Length >= 2)
				{
					if(Decision.CoinStart!=null && Decision.CoinEnd!=null)
					{
						nodes[0].transform.position = Decision.CoinStart.position + Vector3.up*2f;
						nodes[1].transform.position = Decision.CoinEnd.position + Vector3.up*2f;
					}
				}
				
				coinSpline = nodes[0];
				
				if(!GameController.SharedInstance.HasSpawnedGemInBalloon 
					&& Decision.GemSpawns!=null 
					&& Decision.GemSpawns.Count>0
					&& GameController.SharedInstance.DistanceTraveled > TrackBuilder.SharedInstance.MinDistanceBetweenBalloons)
				{
				
					float gemChance = Mathf.Clamp(GameProfile.SharedInstance.BalloonGemSpawnChance + (GamePlayer.SharedInstance.balloonDifficulty * 0.01f),0.05f, 0.2f);
					float v = Random.value;
					
					//Debug.Log ("Random Roll: " + v);
					if(v < gemChance)
					{
						int index = Random.Range(0,Decision.GemSpawns.Count);
						BonusItem item = BonusItem.Create(BonusItem.BonusItemType.Gem);
						transform.parent.parent.GetComponent<TrackPiece>().BonusItems.Add(item);
						item.transform.rotation = Decision.GemSpawns[index].rotation;
						item.transform.position  = Decision.GemSpawns[index].position + Vector3.up*2f; 
						item.transform.Rotate(0,180,0);
						GameController.SharedInstance.HasSpawnedGemInBalloon = true;
					}
				}
			}
		}
	}
	
	public override void OnDisable()
	{
		base.OnDisable();
		
		decision = -1;
		
		//HACK! since we disable right after spawning, set the position then.
		if(spawnedObjects==null || spawns==null)	return;
		
		for(int i=0;i<spawns.Count && i<spawnedObjects.Count;i++)
		{
		//	Debug.Log(i);
			if(spawnedObjects[i]!=null && spawns[i]!=null)
			{
				spawnedObjects[i].transform.position = spawns[i].transform.position;
				spawnedObjects[i].transform.rotation = spawns[i].transform.rotation;
			}
			//if(spawnedObjects[i]!=null)
				//spawnedObjects[i].SetActiveRecursively(false);
		}
	}
	
	public override void OnDespawned()
	{
		base.OnDespawned();
		if(spawnedObjects!=null)
		{
			for(int i=0;i<spawnedObjects.Count;i++)
			{
				SpawnPool pool = PoolManager.Pools["Enemies"];
				if(pool!=null && spawnedObjects[i]!=null)
					pool.Despawn(spawnedObjects[i].transform, null);
			}
			spawnedObjects.Clear();
			spawnedObjects = null;
		}
	}
	
	public override void OnPlayerEnteredPreviousTrackPiece()
	{
		if(spawnedObjects!=null)
		{
			for(int i=0;i<spawnedObjects.Count;i++)
			{
				if(spawnedObjects[i]==null)	continue;
		//		Debug.Log(spawnedObjects[i].name);
				spawnedObjects[i].BroadcastMessage("OnPlayerEnteredPreviousTrackPiece",SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	public override void OnPlayerEnteredTrackPiece()
	{
		
		if(WaitToEnable)
		{
			SpawnEnemies(false);	
		}
		
		if(spawnedObjects==null || GamePlayer.SharedInstance.HasBoost)	return;
				
		for(int i=0;i<spawnedObjects.Count;i++)
		{
			if(spawnedObjects[i]==null)	continue;
			
			
			//spawnedObjects[i].transform.position = spawns[i].transform.position;
			//spawnedObjects[i].transform.rotation = spawns[i].transform.rotation;
			
			//NOTE: This was commented out, but the object needs to be active for BroadcastMessage to work. We need a different solution.
			//this and the broadcast message are very expensive - we should find a way to minimize these calls
			//I changed the SpawnEnemies call to take a flag for deferred activate. So the call above activates the spawns.
			//The check for active below should catch any strays that were spawned with defer but not yet activated
			//Could we add a flag to the spawned object to say if it needs the begin attack message?
			if(!spawnedObjects[i].active)
			{
				spawnedObjects[i].SetActiveRecursively(true);
			}
			//this is not ideal - but fits current usage. Ideally there would ba a flag if attack message is needed
			//if(WaitToEnable)
			//{
				spawnedObjects[i].BroadcastMessage("OnPlayerEnteredTrackPiece",SendMessageOptions.DontRequireReceiver);
			//}
			//Not doing this anymore.
//			if(spawnedObjects[i].GetComponent<FollowObject>()!=null)
//			{
//				Transform oldParent = spawnedObjects[i].transform.parent;
//				spawnedObjects[i].transform.parent = transform;
//				
//				spawnedObjects[i].GetComponent<FollowObject>().Target = GameController.SharedInstance.Player.transform;
//				spawnedObjects[i].GetComponent<FollowObject>().offset = spawnedObjects[i].transform.localPosition;
//				
//				spawnedObjects[i].transform.parent = oldParent;
//			}
		}
	}
}
