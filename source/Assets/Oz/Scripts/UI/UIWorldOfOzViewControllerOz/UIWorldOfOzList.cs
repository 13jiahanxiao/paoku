using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIWorldOfOzList : MonoBehaviour 
{
	public GameObject viewController;	
	private GameObject grid;	
	private List<GameObject> childCells = new List<GameObject>();	
	private List<WorldOfOzData> dataList = new List<WorldOfOzData>();
	
	void Awake() 
	{ 
		grid = gameObject.transform.Find("Grid").gameObject;			// connect to this panel's grid automatically	
		
		//bool HDinstalled = !DownloadManagerUI.NeedHiresUI();	// check HD upgrade
		//bool DFinstalled = EnvironmentSetManager.SharedInstance.IsLocallyAvailable(EnvironmentSetManager.DarkForestId);	// check Dark Forest
		dataList.Add(new WorldOfOzData(0, "Sto_HD_DL_Title", "Sto_HD_DL_Description", "icon_download_hd", false, true));	//HDinstalled));
		dataList.Add(new WorldOfOzData(1, "Sto_Locs_OZ_Title", "Sto_Locs_OZ_Description", "icon_location_emeraldcity", false, true));	///LandOfOzinstalled));
				
		Initialize();
	}

	void Start() 
	{ 
		// disable HD asset download cell if the device doesn't support it 
			Destroy(childCells[0]);
			//NGUITools.SetActive(childCells[0], false);	
		
		grid.GetComponent<UIGrid>().Reposition();
		
		Refresh();	
	}
	
	public void Refresh()
	{
		// ensure data list is initialized
		if ( dataList != null && dataList.Count > 0 )
		{
			RefreshData();
			
			foreach (GameObject childCell in childCells)
			{
				if (childCell != null)
				{	
					int id = childCell.GetComponent<WorldOfOzCellData>()._data.ID;
					childCell.GetComponent<WorldOfOzCellData>().SetData(dataList[id]);
				}
			}
		}
	}		
	
	public void RefreshData()
	{
		if (UIManagerOz.SharedInstance.deviceSupportsHiRes)
			dataList[0].installed = !DownloadManagerUI.NeedHiresUI();	// check HD upgrade
		
		//dataList[1].installed = EnvironmentSetManager.SharedInstance.IsLocallyAvailable(EnvironmentSetManager.DarkForestId);	// check Dark Forest
		dataList[1].installed = DownloadManager.HaveAllLocationsBeenDownloaded();	// check if all locations available
	}

	public void Initialize()
	{	
		childCells = CreateCells();											// create cell GameObject for each
		grid.GetComponent<UIGrid>().sorted = false;
		grid.GetComponent<UIGrid>().Reposition();							// reset/correct positioning of all objects inside grid
	}
	
	private List<GameObject> CreateCells()
	{
		List<GameObject> newObjs = new List<GameObject>();

		foreach (WorldOfOzData data in dataList)
			newObjs.Add(CreatePanel(data, grid));
		
		return newObjs;
	}	
	
	private GameObject CreatePanel(WorldOfOzData data, GameObject _grid)
	{
		GameObject obj = (GameObject)Instantiate(Resources.Load("WorldOfOzCellOz"));	// instantiate objective from prefab	
		obj.transform.parent = _grid.transform;
		obj.transform.localScale = Vector3.one;
		obj.transform.rotation = grid.transform.rotation;
		obj.transform.localPosition = Vector3.zero;
		obj.name = "cell" + data.ID;
		obj.GetComponent<WorldOfOzCellData>()._data = data;						// store reference to data for this objective
		obj.GetComponent<WorldOfOzCellData>().viewController = viewController;	// pass on reference to view controller, for event response
		obj.GetComponent<WorldOfOzCellData>().scrollList = this.gameObject;
		return obj;
	}

	public void CellButtonPressed(GameObject button)
	{
		if (button.transform.parent.parent.parent.gameObject.name == "cell0")
		{
			UIManagerOz.SharedInstance.StartDownloadPrompts( false, true, true, gameObject);
//			Services.Get<NotificationSystem>().ClearNotification(NotificationType.Land, 0);
//			UIManagerOz.SharedInstance.okayDialog.ShowOkayDialog("Trigger HD Upgrade download", "", "Btn_Ok");
		}
		else if (button.transform.parent.parent.parent.gameObject.name == "cell1")
		{
			UIManagerOz.SharedInstance.StartDownloadPrompts( true, false, true, gameObject);
			
			foreach (int setID in Services.Get<NotificationSystem>().GetLandsDownloadable())
			{
				Services.Get<NotificationSystem>().ClearNotification(NotificationType.Land, setID);
			}
			
			UIManagerOz.SharedInstance.worldOfOzVC.Refresh();

//			UIManagerOz.SharedInstance.okayDialog.ShowOkayDialog("Trigger Dark Forest download", "", "Btn_Ok");		
		}
//		else if (button.transform.parent.parent.parent.gameObject.name == "cell2")
//		{
//			UIManagerOz.SharedInstance.StartDownloadPrompts( true, false, true, gameObject);
////			UIManagerOz.SharedInstance.okayDialog.ShowOkayDialog("Trigger Dark Forest download", "", "Btn_Ok");		
//		}		
	}
	
	public void OnPromptSequenceDone()
	{
		//Debug.LogError("Refresh called");
		Refresh();
	}
}	
