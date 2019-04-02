using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class UIPowersList : MonoBehaviour 
{
	protected static Notify notify;
	
	public GameObject viewController;
//	public GameObject equippedPowerupCell;
	private GameObject grid;	
	private PowerCellData powerupToPurchase;	// temp reference, for use when purchasing a specific item
	private List<GameObject> childCells = new List<GameObject>();	
	private List<BasePower> sortedDataList = new List<BasePower>();
	//private SaleBanner saleBanner;
	//private NotificationSystem notificationSystem;
	
	void Awake() 
	{ 
		notify = new Notify(this.GetType().Name);	
		grid = gameObject.transform.Find("powerGrid").gameObject;			// connect to this panel's grid automatically			
		sortedDataList = SortGridItemsByPriority(PowerStore.Powers);	
		Initialize();
	}
	
	void Start()
	{
		//notificationSystem = Services.Get<NotificationSystem>();	
		Refresh();
	}
	
	public void Refresh()
	{
		
		// ensure that the sortedDataList has been initialized
		if ( sortedDataList != null && sortedDataList.Count > 0 )
		{
			/*
		
			if (saleBanner == null)
			{
				saleBanner = viewController.GetComponentsInChildren<SaleBanner>(true).First();
			}
			
			if ( saleBanner != null )
			{
				// set status of sale banner (show only if sale is active)
				saleBanner.SetSaleBannerStatus(gameObject.GetComponent<UIPanel>(), 
					transform.parent.gameObject, DiscountItemType.Powerup, saleBanner);
			}
			*/
			
			sortedDataList = SortGridItemsByPriority(sortedDataList);
			
			/*
			foreach (GameObject childCell in childCells)
			{
				int id = childCell.GetComponent<PowerCellData>()._data.PowerID;
				childCell.GetComponent<PowerCellData>().SetData(PowerStore.PowerFromID(id));	//.Powers[i]);
				//childCell.GetComponent<PowerCellData>().SetData(PowerStore.Powers[i]);
			}
			*/
			
			int cellIndex = 0;
			
			foreach (GameObject childCell in childCells)
			{
				childCell.GetComponent<PowerCellData>().SetData(sortedDataList[cellIndex]);
				
				notify.Debug("[UIPowersList] - childCell name before: " + childCell.name);
				
				childCell.name = GenerateCellLabel(sortedDataList[cellIndex]);
				
				notify.Debug("[UIPowerList] - childCell name after: " + childCell.name);
				
				cellIndex++;
			}
			
			gameObject.GetComponent<UIDraggablePanel>().ResetPosition();		
		}
	}

	public void Initialize()
	{	
		childCells = CreateCells();											// create cell GameObject for each
		grid.GetComponent<UIGrid>().sorted = false;	//true;
		grid.GetComponent<UIGrid>().Reposition();							// reset/correct positioning of all objects inside grid
	}
	
	private List<GameObject> CreateCells()
	{
		List<GameObject> newObjs = new List<GameObject>();

		foreach (BasePower powerData in sortedDataList)
		{
			GameObject panel = CreatePanel(powerData, grid);
			panel.name = GenerateCellLabel(powerData);
			newObjs.Add(panel);
		}		
		
		return newObjs;
	}	
	
	private string GenerateCellLabel(BasePower powerData)
	{
		return ("Cell_" + powerData.SortPriority.ToString("D8") + "_" + powerData.PowerID.ToString("D8"));
	}		
	
	public void OnPowerupCellPressed(GameObject cell) 	
	{
		//Services.Get<NotificationSystem>().ClearNotification(NotificationType.Powerup, cell.transform.parent.GetComponent<PowerCellData>()._data.PowerID);		
	}			

	public void Reposition()
	{
		grid.GetComponent<UIGrid>().Reposition();		
	}			
	
	private List<BasePower> SortGridItemsByPriority(List<BasePower> list)	//unsortedList)
	{
		//List<BasePower> listToSort = unsortedList.ToList();
		//listToSort = listToSort.OrderBy(x => x.SortPriority).ToList(); 
		list.Sort((a1, a2) => a1.SortPriority.CompareTo(a2.SortPriority));
		return list;	//listToSort;
	}
	
	private GameObject CreatePanel(BasePower data, GameObject _grid)
	{
		GameObject obj = (GameObject)Instantiate(Resources.Load("PowerStoreCellOz"));	// instantiate objective from prefab	
		obj.transform.parent = _grid.transform;
		obj.transform.localScale = Vector3.one;
		obj.transform.rotation = grid.transform.rotation;
		obj.transform.localPosition = Vector3.zero;
		obj.GetComponent<PowerCellData>()._data = data;						// store reference to data for this objective
		obj.GetComponent<PowerCellData>().viewController = viewController;	// pass on reference to view controller, for event response
		obj.GetComponent<PowerCellData>().scrollList = this.gameObject;
		//obj.GetComponent<SubPanel>().scrollList = this.gameObject;			// pass on reference to this script's GameObject, for triggering 'reposition' requests		
		
		// move subpanel offscreen and turn it off
		//obj.GetComponent<SubPanel>().TurnSubPanelOff(obj.transform.Find("CellContents").gameObject);			
		return obj;
	}

	private void OnPurchaseYes()
	{
		// set up shorter local identifiers, to keep code easy to read
		//UIInventoryViewControllerOz invViewCont = viewController.GetComponent<UIInventoryViewControllerOz>();		
		PlayerStats playerStats = GameProfile.SharedInstance.Player;
		
		playerStats.PurchasePower(powerupToPurchase._data.PowerID);	// buy it if we can afford it
		UIManagerOz.SharedInstance.PaperVC.UpdateCurrency();
		//invViewCont.UpdateCurrency();								// will update coin and gem counts in UI				
		powerupToPurchase.Refresh();								// ask cell to update its GUI rendering to match data, in case it was updated in the transaction		
		//UIConfirmDialogOz.onNegativeResponse -= OnPurchaseNo;
		//UIConfirmDialogOz.onPositiveResponse -= OnPurchaseYes;
	}	
	
	public void CellBuyButtonPressed(GameObject cell)		// public void OnPowerItemPressed(GameObject cell) 
	{
		//notify.Debug("CellBuyButtonPressed called at: " + Time.realtimeSinceStartup.ToString());
		
		// set up shorter local identifiers, to keep code easy to read
		UIInventoryViewControllerOz invViewCont = viewController.GetComponent<UIInventoryViewControllerOz>();	
		PowerCellData powerCellData = cell.transform.parent.parent.parent.GetComponent<PowerCellData>();		
		//PowerCellData powerCellData = cell.transform.parent.GetComponent<PowerCellData>();
		int powerID = powerCellData._data.PowerID;
		CharacterStats activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
		PlayerStats playerStats = GameProfile.SharedInstance.Player;
		
		Services.Get<NotificationSystem>().ClearNotification(NotificationType.Powerup, powerID);		
		Services.Get<NotificationSystem>().SetNotificationIconsForThisPage(UiScreenName.UPGRADES);	
		
		if (playerStats.IsPowerPurchased(powerID) == false) 		// check if already purchased
		{
			if (playerStats.CanAffordPower(powerID) == true)
			{
				powerupToPurchase = powerCellData;
				//UIConfirmDialogOz.onNegativeResponse += OnPurchaseNo;
				//UIConfirmDialogOz.onPositiveResponse += OnPurchaseYes;
				//UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog(powerCellData.data.Title, "Purchase this powerup?", "Btn_No", "Btn_Yes");
				//playerStats.PurchasePower(powerID);					
				OnPurchaseYes();		// buy it if we can afford it
			}
			else
			{
				UIConfirmDialogOz.onNegativeResponse += invViewCont.OnNeedMoreCoinsNo;
				UIConfirmDialogOz.onPositiveResponse += invViewCont.OnNeedMoreCoinsYes;
				//UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreCoins_Prompt","Lbl_Dialogue_MoreCoins_Confirm", "Btn_No", "Btn_Yes");
				UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreCoins_Prompt", "Btn_No", "Btn_Yes");
			}
		}
		else if (GameProfile.SharedInstance.IsPowerEquipped(powerID, activeCharacter.characterId) == false) 	// check if already equipped
		{
			//Change the power id for each character
			foreach(CharacterStats character in GameProfile.SharedInstance.Characters)
			{
				character.powerID = powerID;
			}
			
			//activeCharacter.powerID = powerID;
			
//			equippedPowerupCell.GetComponent<EquippedPowerupCell>().UpdateEquippedCell(PowerStore.Powers[powerID]);	// change icon next to character
			//invViewCont.characterSelectVC.UpdateCharacterCard(activeCharacter);
			//UIManagerOz.SharedInstance.characterSelectVC.UpdateCharacterCard(activeCharacter);
			
			//AnalyticsInterface.LogGameAction("powerup", "equipped", ((PowerType)powerID).ToString(),GameProfile.GetAreaCharacterString(),0);
			GameProfile.SharedInstance.Serialize();
			
			// ask all power cells to update their GUI renderings, to match updated data
			PowerCellData[] allPowerCells = grid.GetComponentsInChildren<PowerCellData>();
			foreach (PowerCellData powerCell in allPowerCells) { powerCell.Refresh(); }
		}
//		else 	//-- Can't equip this because its already equipped.
//		{
//			UIOkayDialogOz.onPositiveResponse += OnAlreadyEquipped;
//			UIManagerOz.SharedInstance.okayDialog.ShowOkayDialog("Oops!", "That PowerUp is already in use.", "Btn_Ok");	//-- Show error dialog
//		}
		
		UIManagerOz.SharedInstance.PaperVC.UpdateCurrency();
		//invViewCont.UpdateCurrency();								// will update coin and gem counts in UI				
		powerCellData.Refresh();									// ask cell to update its GUI rendering to match data, in case it was updated in the transaction
	}

	//void Update() { }
}	



			//CreatePanel(powerData, grid);									//GameObject obj = CreatePanel(powerData, grid);
	
		//List<StoreItem> sortedDataList = SortGridItemsByPriority(Store.StoreItems);
		

	
//	public void AnimationDone()
//	{
		//animating = false;
//	}
	


		//grid = gameObject.transform.Find("powerGrid").gameObject; 			// connect to this panel's grid automatically
		//ClearGrid(grid);													// kill all old ones just in case	

//		if (!animating)
//		{
//			animating = true;
//			selectedCell = cell.transform.parent.gameObject.GetComponent<SubPanel>().OnCellPressed(cell, selectedCell);
//		}
		

//	public void OnAlreadyEquipped() 
//	{
//		UIOkayDialogOz.onPositiveResponse -= OnAlreadyEquipped;
//	}		
	


	//private List<BasePower> myDataList = new List<BasePower>();		
	//private GameObject selectedCell = null;		
	//private bool animating = false;



//	public void Refresh() 
//	{
//		List<BasePower> sortedDataList = SortGridItemsByPriority(PowerStore.Powers);
		
//		foreach (BasePower powerData in sortedDataList)
//		{
			//CreatePanel(powerData, grid);									//GameObject obj = CreatePanel(powerData, grid);
//			CreatePanel(powerData, grid).name = GenerateCellLabel(powerData);
//		}
		
//		grid.GetComponent<UITable>().sorted = true;
		//grid.GetComponent<UIGrid>().Reposition();							// reset/correct positioning of all objects inside grid	
//		grid.GetComponent<UITable>().Reposition();							// reset/correct positioning of all objects inside grid	
//	}

	
		//ClearGrid(grid);													// kill all old objects under grid, prior to refresh
			
	
//	private void ClearGrid(GameObject _grid)
//	{
//		UIDragPanelContents[] contentArray = _grid.GetComponentsInChildren<UIDragPanelContents>();
//		foreach (UIDragPanelContents contents in contentArray) 
//		{ 
//			//DestroyImmediate(contents.gameObject); 
//			contents.transform.parent = null;	// unparent first to remove bug when calling NGUI's UIGrid.Reposition(), because Destroy() is not immediate!
//			Destroy(contents.gameObject); 
//		}	
//	}	
	
//	private void OnPurchaseNo()
//	{
//		UIConfirmDialogOz.onNegativeResponse -= OnPurchaseNo;
//		UIConfirmDialogOz.onPositiveResponse -= OnPurchaseYes;
//	}
	
	
//	public void OnPowerupCellPressed(GameObject cell) 	
//	{
//		SubPanel subPanel = cell.transform.parent.gameObject.GetComponent<SubPanel>();
//		
//		if (cell == selectedCell)		// just close it
//		{
//			subPanel.ResizeCell(selectedCell, false);
//			selectedCell = null;	
//		}
//		else
//		{
//			if (selectedCell != null)	// is there a selected cell? If so, close it.
//				subPanel.ResizeCell(selectedCell, false);
//			
//			// open the clicked cell
//			subPanel.ResizeCell(cell, true);
//			selectedCell = cell;
//		}
//	}	

	
	// gemming code below, for when we add that feature to powerups also
	
//		else if (playerStats.IsPowerGemmed(powerID) == false)		// check if already gemmed	
//		{ 
//			if (playerStats.CanAffordPowerGem(powerID) == true)
//			{
//				playerStats.GemPower(powerID);						// gem it if we can afford it
//			}
//			else
//			{
//				UIConfirmDialogOz.onNegativeResponse += invViewCont.OnNeedMoreGemsNo;
//				UIConfirmDialogOz.onPositiveResponse += invViewCont.OnNeedMoreGemsYes;
//				UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Need More Gems!","Pick up gems while running.", "OK1", "OK2");
//			}
//		}
//		else 														// it's both purchased and gemmed, so let player know that nothing more to be done here
//		{
//			UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("You already own this powerup","and you have gemmed it as well!", "OK1", "OK2"); 
//		}

	
	//-----------------------------	
//	
//	private int equippedArtifact = -1;
//	private int equippedPower = -1;
//	
//	public void SetEquippedPower(int powerID) 
//	{
//		equippedArtifact = -1;
//		equippedPower = powerID;
//		//UpdateEquippedCell("Pick a Powerup");
//	}	
//	
//	//private void UpdatePowerCellData(CellData cellData, PlayerStats player, CharacterStats activeCharacter) 
//	private void UpdatePowerCellData(GameObject cell, PlayerStats player, CharacterStats activeCharacter) 		
//	{
//		//if (cellData == null) { return; }
//		//GameObject newCell = cellData.gameObject;
//		int powerID = cell.transform.parent.GetComponent<PowerCellData>().data.PowerID;
//		
//		//BasePower power = PowerStore.Powers[cellData.Data];
//		bool purchased = player.IsPowerPurchased(powerID);	//cellData.Data);
//		bool equipped = (activeCharacter.powerID == powerID);	//cellData.Data);
//		//UpdateCellData(newCell, power.Title, power.IconName, power.Description, power.Cost.ToString(), purchased, equipped);
//	}
//	
//	private void UpdatePowerups() 
//	{
//		PlayerStats player = GameProfile.SharedInstance.Player;
//		CharacterStats activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
//		
//		//CellData[] cells = grid.GetComponentsInChildren<CellData>(true) as CellData[];
//		//foreach (CellData item in cells) 
//		//{
//		//	if (item == null) { continue; }
//		//	UpdatePowerCellData(item, player, activeCharacter);
//		//}
//	}


//	public void OnPowerItemPressed(GameObject cell) 
//	{
//		//-- Can we equip this?
//		bool closeDialog = false;
//		if (cell != null) 
//		{ 
//			//-- equip it
//			//CellData cellData = cell.GetComponent<CellData>() as CellData;
//			//if (cellData != null) 
//			//{
//				//int powerID = cellData.Data;
//				int powerID = cell.transform.parent.GetComponent<PowerCellData>().data.PowerID;
//				CharacterStats activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
//				PlayerStats playerStats = GameProfile.SharedInstance.Player;
//				//-- If locked {
//				if(playerStats.IsPowerPurchased(powerID) == false) 
//				{
//					//-- Can we afford it?
//					if(playerStats.CanAffordPower(powerID) == false) 
//					{
//						UIConfirmDialogOz.onNegativeResponse += viewController.GetComponent<UIInventoryViewControllerOz>().OnNeedMoreCoinsNo;
//						UIConfirmDialogOz.onPositiveResponse += viewController.GetComponent<UIInventoryViewControllerOz>().OnNeedMoreCoinsYes;
//						UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Need More Coins!","Would you like to get more coins?", "No", "Yes");
//						return;
//					}
//				
//					//-- Buy it if we can afford it.
//					playerStats.PurchasePower(powerID);
//					//UpdatePowerCellData(cellData, playerStats, activeCharacter);
//					UpdatePowerCellData(cell, playerStats, activeCharacter);
//					viewController.GetComponent<UIInventoryViewControllerOz>().UpdateCurrency();	//updateCurrency();
//				}
//				else if(GameProfile.SharedInstance.IsPowerEquipped(powerID, activeCharacter.characterId) == false) 
//				{
//					activeCharacter.powerID = powerID;
//					if (viewController.GetComponent<UIInventoryViewControllerOz>().characterSelectVC != null) 
//					{ 
//						viewController.GetComponent<UIInventoryViewControllerOz>().characterSelectVC.UpdateCharacterCard(activeCharacter); 
//					}
//					equippedPower = powerID;
//					//UpdateEquippedCell();
//					GameProfile.SharedInstance.Serialize();
//					//closeDialog = true;
//				}
//				else 
//				{
//					//-- Can't equip this because its already equipped.
//					//-- Show error dialog
//					UIConfirmDialogOz.onPositiveResponse += OnAlreadyEquipped;
//					UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Oops!", "That PowerUp is already in use.", "Ok", "");
//					return;
//				}	
//			//}
//		}	
//		
//		if (closeDialog == true) { viewController.GetComponent<UIInventoryViewControllerOz>().OnBackButton(); }
//	}	
//	
//	public void OnAlreadyEquipped() 
//	{
//		UIConfirmDialogOz.onPositiveResponse -= OnAlreadyEquipped;
//	}		
//}


		
		//equippedPowerupCell.GetComponent<EquippedPowerupCell>().UpdateEquippedCell(PowerStore.Powers[GameProfile.SharedInstance.GetActiveCharacter().powerID]);	// change icon next to character	