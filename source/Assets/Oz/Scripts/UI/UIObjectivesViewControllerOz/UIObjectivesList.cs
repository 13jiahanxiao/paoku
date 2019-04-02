using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIObjectivesList : MonoBehaviour 
{
	public TimeLeftCountdown countdownTimer;
	public string objectivesToLoad;
	public bool IsInitialized { get; private set; }
	public UILabel comingSoonText;
	public UILabel friendCountText;
	
	private GameObject grid;
	private List<GameObject> childObjectiveCells = new List<GameObject>();
	private List<ObjectiveProtoData> dataList = new List<ObjectiveProtoData>();
	
	protected static Notify notify;
	
	void Awake() 
	{ 
		notify = new Notify(this.GetType().Name);
		IsInitialized = false;
		
		
		if (objectivesToLoad == "weekly")	// trigger request for weekly challenges
		{
			dataList = Services.Get<ObjectivesManager>().GetWeeklyObjectives();	
		}
		//else if ( objectivesToLoad == "team" )
		//{
		//	dataList = Services.Get<ObjectivesManager>().GetWeeklyObjectivesClass().TeamObjectiveList;
		//}
		else if (objectivesToLoad == "legendary")
		{
			dataList = ObjectivesManager.LegendaryObjectives;
			dataList = Services.Get<ObjectivesManager>().SortGridItemsByPriority(dataList);	
			Initialize();
		}
		// wxj
		else if(objectivesToLoad == "activity")
		{
			dataList = ObjectivesManager.ActivityObjectives;
			Initialize();
		}
		
	}
	
	
	
	public void PopulateWeeklyObjectiveData()
	{
		
		if (objectivesToLoad == "weekly")
		{	
			dataList = Services.Get<ObjectivesManager>().GetWeeklyObjectives();
			
			if (dataList.Count > 0)
			{
				if (countdownTimer != null)
					countdownTimer.SetExpirationDateTime(dataList[0]._endDate);					
				
				// remove any possible duplication in dataList prior to creating cells
				dataList = dataList.GroupBy(x => x._id).Select(y => y.First()).ToList();;	
				
				dataList = Services.Get<ObjectivesManager>().SortGridItemsByPriority(dataList);	// sort

				if (!IsInitialized)
					Initialize();
				else
					RefreshCells();
			}
		}
		else if ( objectivesToLoad == "team" )
		{
			notify.Debug( "[UIObjectivesList] - Refresh Team Challenge" );
			
			dataList = Services.Get<ObjectivesManager>().GetWeeklyObjectivesClass().TeamObjectiveList;
			
			if ( dataList.Count > 0 )
			{
				if ( countdownTimer != null )
				{
					countdownTimer.SetExpirationDateTime( dataList[0]._endDate );
				}

				if (ProfileManager.SharedInstance.userServerData != null && friendCountText != null)
				{
					int friendCount = ProfileManager.SharedInstance.userServerData._neighborList.Count;
				
					if (friendCount > 0   )
					{
						friendCountText.enabled = true;
						friendCountText.text = string.Format(Localization.SharedInstance.Get("Lbl_FriendCount"), friendCount);	
					}
					else
					{
						friendCountText.text = string.Format( Localization.SharedInstance.Get( "Lbl_FriendCount" ), 0 );
						//friendCountText.enabled = false;
					}
				}
				else
				{
					friendCountText.enabled = false;
				}	
					
				dataList = dataList.GroupBy( x => x._id ).Select( y => y.First() ).ToList();;
				
				dataList = Services.Get<ObjectivesManager>().SortGridItemsByPriority( dataList );
				
				if ( !IsInitialized )
				{
					Initialize();
				}
				else
				{
					RefreshCells();
				}
			}
		}
	}
	
	public void Refresh()
	{
		if (objectivesToLoad == "weekly" && Initializer.IsBuildVersionPassThreshold())
		{
			PopulateWeeklyObjectiveData();
		}
		else if ( objectivesToLoad == "team" && Initializer.IsBuildVersionPassThreshold() )
		{
			PopulateWeeklyObjectiveData();
		}
		else if (objectivesToLoad == "legendary")
		{
			RefreshCells();
		}
		// wxj
		else if(objectivesToLoad == "activity")
		{
			RefreshCells();
			if (countdownTimer != null)
					countdownTimer.SetExpirationDateTime(dataList[0]._endDate);	
		}
	}
	
	public void RefreshCells()
	{
		int i=0;

		foreach (GameObject childCell in childObjectiveCells)
		{
			//childCell.GetComponent<ChallengeCellData>().SetData(dataList[i]);
			childCell.GetComponent<ObjectiveCellData>().SetData(dataList[i],objectivesToLoad);
			i++;
		}	
	}
	
	public void Initialize()
	{
		grid = gameObject.transform.Find("Grid").gameObject; 	// connect to this panel's grid automatically
		ClearGrid(grid);										// kill all old objects under grid, prior to initialization
		childObjectiveCells = CreateCells();					// create cell GameObjects for all objectives
		grid.GetComponent<UIGrid>().Reposition();				// reset/correct positioning of all objects inside grid
		grid.transform.parent.GetComponent<UIDraggablePanel>().ResetPosition();
		IsInitialized = true;
	}
	
	private void ClearGrid(GameObject _grid)
	{
		UIDragPanelContents[] contentArray = _grid.GetComponentsInChildren<UIDragPanelContents>();
		foreach (UIDragPanelContents contents in contentArray) 
		{ 
			//DestroyImmediate(contents.gameObject); 
			contents.transform.parent = null;	// unparent first to remove bug when calling NGUI's UIGrid.Reposition(), because Destroy() is not immediate!
			Destroy(contents.gameObject); 
		}	
	}		
	
	private List<GameObject> CreateCells()
	{
		List<GameObject> newObjs = new List<GameObject>();

		// create cells
		foreach (ObjectiveProtoData objectiveData in dataList)
			newObjs.Add(CreateObjectivePanel(objectiveData));
		
		return newObjs;
	}	

	private GameObject CreateObjectivePanel(ObjectiveProtoData _objectiveData)	//string _title, string _description)
	{
		// instantiate objective from prefab
		GameObject obj = (GameObject)Instantiate(Resources.Load("ObjectiveCellOz"));	//01"));		
		obj.transform.parent = grid.transform;
		obj.transform.localPosition = Vector3.zero;		
		obj.transform.localScale = Vector3.one;
		obj.transform.rotation = grid.transform.rotation;
		//obj.GetComponent<ChallengeCellData>().SetData(_objectiveData);	// store reference to data for this objective
		obj.GetComponent<ObjectiveCellData>().SetData(_objectiveData,objectivesToLoad);	// store reference to data for this objective
		
		//if (objectivesToLoad == "weekly")	// turn off background if weekly challenge cell
		//	obj.transform.Find("CellContents/GraphicsAnchor/SlicedSprite (bg_storecell)").GetComponent<UISlicedSprite>().enabled = false;
		
		return obj;
	}	
}








		//dataList = dataList.Distinct(new DistinctItemComparer());
		//dataList = RemoveDuplicates(dataList);

//class DistinctObjectiveComparer : IEqualityComparer<ObjectiveProtoData> 
//{
//    public bool Equals(ObjectiveProtoData x, ObjectiveProtoData y)
//	{
//        return x._id == y._id;
//    }
//
//    public int GetHashCode(ObjectiveProtoData obj) 
//	{
//        return obj._id.GetHashCode();
//    }
//}




//	private List<ObjectiveProtoData> RemoveDuplicates(List<ObjectiveProtoData> originalList)
//    {
//		List<ObjectiveProtoData> cleanedList = new List<ObjectiveProtoData>();
//	
//		foreach (ObjectiveProtoData obj in originalList)
//		{
//			if (!cleanedList.Contains(obj))
//				cleanedList.Add(obj);
//		}
//		
//		return cleanedList;
//    }	
	
	
//		ClearGrid(grid);										// kill all old objectives just in case	


//	private void ClearGrid(GameObject _grid)
//	{
//		UIDragPanelContents[] objectives = _grid.GetComponentsInChildren<UIDragPanelContents>();
//		foreach (UIDragPanelContents contents in objectives) 
//		{ 
//			//DestroyImmediate(contents.gameObject); 
//			contents.transform.parent = null;	// unparent first to remove bug when calling NGUI's UIGrid.Reposition(), because Destroy() is not immediate!
//			Destroy(contents.gameObject); 
//		}	
//	}


//	void Start() 
//	{ 
//		if (objectivesToLoad == "weekly")
//		{
//			PopulateWeeklyObjectiveData();
//			Initialize();
//		}
//	}	
	


	
//	void Start() 
//	{ 
//		if (objectivesToLoad == "weekly")
//		{
//			PopulateWeeklyObjectiveData();
//			Initialize();
//		}
//	}
	
	
		
//		switch (objectivesToLoad)
//		{
//			case "current":
//				dataList = GameProfile.SharedInstance.Player.objectivesActive;
//				break;
//			case "weekly":
//				dataList = Services.Get<ObjectivesManager>().GetWeeklyObjectives();
//				dataList = SortGridItemsByPriority(dataList);	
//			
//				if (countdownTimer != null && dataList.Count > 0)
//					countdownTimer.SetExpirationDateTime(dataList[0]._endDate);
//				break;
//			case "legendary":
//				dataList = ObjectivesManager.LegendaryObjectives;
//				dataList = SortGridItemsByPriority(dataList);	
//				break;
//			case "completed":
//				foreach (int objectiveID in GameProfile.SharedInstance.Player.objectivesEarned)
//				{
//					foreach (ObjectiveProtoData objectiveData in ObjectivesManager.Objectives)
//					{
//						if (objectiveData._id == objectiveID)
//							dataList.Add(objectiveData);
//					}
//				}
//				break;			
//			default:
//				break;
//		}
//		
//		Initialize();

	
	//void Update() { }


	//Refresh();

//
//
//		if (objectivesToLoad == "active")						// load only currently active objectives
//		{
//			childObjectiveCells = CreateCells(GameProfile.SharedInstance.Player.objectivesActive);
//		}
//		else if (objectivesToLoad == "challenges")				// load weekly objectives (challenges)
//		{
//			childObjectiveCells = CreateCells(Services.Get<ObjectivesManager>().GetWeeklyObjectives());	
//		}
//		else if (objectivesToLoad == "legendary")				// load legendary objectives
//		{
//			childObjectiveCells = CreateCells(ObjectivesManager.LegendaryObjectives);
//		}		
//

	
//	public void Refresh() 
//	{
//		if (objectivesToLoad == "active")						// refresh data for currently active objectives
//		{
//			RefreshAllCellsInObjectivesList(dataList);
//		}
//		else if (objectivesToLoad == "challenges")				// refresh data for weekly objectives (challenges)
//		{
//			RefreshAllCellsInObjectivesList(Services.Get<ObjectivesManager>().GetWeeklyObjectives());	
//		}
//		else if (objectivesToLoad == "legendary")				// refresh data for legendary objectives
//		{
//			RefreshAllCellsInObjectivesList(ObjectivesManager.LegendaryObjectives);		
//		}			
//	}

//
//			
//			foreach (ObjectiveProtoData objectiveData in ObjectivesManager.LegendaryObjectives)
//			{
//				childObjectiveCells.Add(CreateObjectivePanel(objectiveData));
//			}		
//		
//
//		
//			foreach (ObjectiveProtoData objectiveData in Services.Get<ObjectivesManager>().GetWeeklyObjectives())				
//			{
//				childObjectiveCells.Add(CreateObjectivePanel(objectiveData));
//			}		
//				
//				foreach (ObjectiveProtoData objectiveData in GameProfile.SharedInstance.Player.objectivesActive)
//				{
//					childObjectiveCells.Add(CreateObjectivePanel(objectiveData));
//				}

			//if (GameProfile.SharedInstance != null)	// prevents null when running from within "ObjectivesEdit" scene, since no GameProfile exists
			//{

	//bool loadOnlyActiveObjectives;
	//public string objCategoryPrefix = "";

//				obj.transform.Find("SlicedSpriteBar01").transform.localScale = Vector3.zero;	//.gameObject.active = false;	// don't show progress bar if not in 'active objectives' group
//				obj.transform.Find("SlicedSpriteFill01").transform.localScale = Vector3.zero;	//.gameObject.active = false;
//				obj.transform.Find("SlicedSpriteFill02").transform.localScale = Vector3.zero;	//.gameObject.active = false;				
//				obj.transform.Find("LabelStatProgress").transform.localScale = Vector3.zero;	//.gameObject.active = false;

//				obj.transform.Find("SlicedSpriteBar01").transform.localScale = Vector3.zero;	//.gameObject.active = false;	// don't show progress bar if not in 'active objectives' group
//				obj.transform.Find("SlicedSpriteFill01").transform.localScale = Vector3.zero;	//.gameObject.active = false;
//				obj.transform.Find("SlicedSpriteFill02").transform.localScale = Vector3.zero;	//.gameObject.active = false;				
//				obj.transform.Find("LabelStatProgress").transform.localScale = Vector3.zero;	//.gameObject.active = false;

		
		//obj.transform.Find("LabelTitle01").GetComponent<UILabel>().text = objCategoryPrefix + _objectiveData._title;		
		//obj.transform.Find("LabelDescription01").GetComponent<UILabel>().text = _objectiveData._descriptionPreEarned;	// " (val=" + _objectiveData._statValue + ")";
		//obj.transform.Find("LabelStatProgress").GetComponent<UILabel>().text = "(earned: " + _objectiveData._earnedStatValue + ", need: " + _objectiveData._statValue + ")"; 
		
		//obj.GetComponent<ObjectiveCellData>().data = _objectiveData;	// store reference to data for this objective