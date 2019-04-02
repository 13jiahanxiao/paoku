using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIConsumablesList : MonoBehaviour 
{
	protected static Notify notify;
	
	public GameObject viewController;
	private GameObject grid;
	private ConsumableCellData consumableToPurchase;	// temp reference, for use when purchasing a specific item
	private List<GameObject> childCells = new List<GameObject>();	
	private List<BaseConsumable> sortedDataList = new List<BaseConsumable>();	
	//private SaleBanner saleBanner;
	//private NotificationSystem notificationSystem;
	
	void Awake() 
	{ 
		notify = new Notify(this.GetType().Name);
		
		grid = gameObject.transform.Find("consumableGrid").gameObject;			// connect to this panel's grid automatically				
		sortedDataList = SortGridItemsByPriority(ConsumableStore.consumablesList);	
		Initialize();
	}	
	
	void Start() 
	{ 
		//notificationSystem = Services.Get<NotificationSystem>();
		Refresh();
	}
	

	
	public void Refresh()
	{
		// Ensure that data list has been populated
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
	//			saleBanner.SetSaleBannerStatus(gameObject.GetComponent<UIPanel>(), 
	//				transform.parent.gameObject, DiscountItemType.Consumable, saleBanner);
			}
			*/
			
			sortedDataList = SortGridItemsByPriority( sortedDataList );
			
			int cellIndex = 0;
			
			foreach (GameObject childCell in childCells)
			{
				//int id = childCell.GetComponent<ConsumableCellData>()._data.PID;
				
				//childCell.GetComponent<ConsumableCellData>().SetData(ConsumableStore.ConsumableFromID(id));	//.Powers[i]);		
				//childCell.GetComponent<ConsumableCellData>().SetData(ConsumableStore.consumablesList[i]);
			
				childCell.GetComponent<ConsumableCellData>().SetData( sortedDataList[cellIndex] );
				childCell.name = GenerateCellLabel( sortedDataList[cellIndex] );
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

		foreach (BaseConsumable consumableData in sortedDataList)
		{
			GameObject panel = CreatePanel(consumableData, grid);
			panel.name = GenerateCellLabel(consumableData);
			newObjs.Add(panel);
		}		
		
		return newObjs;
	}	
	
	private string GenerateCellLabel(BaseConsumable consumableData)
	{
		return ("Cell_" + consumableData.SortPriority.ToString("D8") + "_" + consumableData.PID.ToString("D8"));
	}	
	
	public void OnConsumableCellPressed(GameObject cell) 	
	{
		//Services.Get<NotificationSystem>().ClearNotification(NotificationType.Consumable, cell.transform.parent.GetComponent<ConsumableCellData>()._data.PID);
	}	

	public void Reposition()
	{
		grid.GetComponent<UIGrid>().Reposition();		
	}		
	
	private List<BaseConsumable> SortGridItemsByPriority(List<BaseConsumable> list)	//unsortedList)
	{
		//List<BaseConsumable> listToSort = unsortedList.ToList();
		//listToSort = listToSort.OrderBy(x => x.SortPriority).ToList(); 
		list.Sort((a1, a2) => a1.SortPriority.CompareTo(a2.SortPriority));
		return list;	//listToSort;
	}	

	private GameObject CreatePanel(BaseConsumable _data, GameObject _grid)
	{
		GameObject obj = (GameObject)Instantiate(Resources.Load("ConsumableStoreCellOz"));	// instantiate objective from prefab	
		obj.transform.parent = _grid.transform;
		obj.transform.localScale = Vector3.one;
		obj.transform.rotation = grid.transform.rotation;
		obj.transform.localPosition = Vector3.zero;
		obj.GetComponent<ConsumableCellData>()._data = _data;					// store reference to data for this objective
		obj.GetComponent<ConsumableCellData>().viewController = viewController;	// pass on reference to view controller, for event response
		obj.GetComponent<ConsumableCellData>().scrollList = this.gameObject;
		//obj.GetComponent<SubPanel>().scrollList = this.gameObject;				// pass on reference to this script's GameObject, for triggering 'reposition' requests		
		
		// move subpanel offscreen and turn it off
		//obj.GetComponent<SubPanel>().TurnSubPanelOff(obj.transform.Find("CellContents").gameObject);		
		return obj;
	}

	private void OnPurchaseYes()
	{
		// set up shorter local identifiers, to keep code easy to read
		//UIInventoryViewControllerOz invViewCont = viewController.GetComponent<UIInventoryViewControllerOz>();		
		PlayerStats playerStats = GameProfile.SharedInstance.Player;
		//	PurchaseUtil.bIAnalysisWithParam("Purchase_Consumables","ConsumablesName|"+consumableToPurchase._data.Subtitle+",amount|1");
		playerStats.PurchaseConsumable(consumableToPurchase._data.PID);	// buy it if we can afford it
		UIManagerOz.SharedInstance.PaperVC.UpdateCurrency();
		//invViewCont.UpdateCurrency();									// will update coin and gem counts in UI				
		consumableToPurchase.Refresh();									// ask cell to update its GUI rendering to match data, in case it was updated in the transaction		
		//UIConfirmDialogOz.onNegativeResponse -= OnPurchaseNo;
		//UIConfirmDialogOz.onPositiveResponse -= OnPurchaseYes;
	}		
	
	public void CellBuyButtonPressed(GameObject cell)	//public void OnConsumableCellPressed(GameObject cell) 
	{
		// set up shorter local identifiers, to keep code easy to read
		UIInventoryViewControllerOz invViewCont = viewController.GetComponent<UIInventoryViewControllerOz>();	
		ConsumableCellData consumableCellData = cell.transform.parent.parent.parent.GetComponent<ConsumableCellData>();
		//ConsumableCellData consumableCellData = cell.transform.parent.GetComponent<ConsumableCellData>();
		int consumableID = consumableCellData._data.PID;
		//CharacterStats activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
		PlayerStats playerStats = GameProfile.SharedInstance.Player;
		
		
		if (consumableCellData._data.Type == "FastTravelConsumable" && !DownloadManager.HaveAllLocationsBeenDownloaded())
		{
			ShowDownloadPrompt();
			return;
		}
		
		Services.Get<NotificationSystem>().ClearNotification(NotificationType.Consumable, consumableID);		
		Services.Get<NotificationSystem>().SetNotificationIconsForThisPage(UiScreenName.UPGRADES);	
		
		if (playerStats.IsConsumableMaxedOut(consumableID) == false) 	// check if already purchased maximum allowed
		{
			
			
			if(consumableCellData._data.SortPriority == 1){
				consumableToPurchase = consumableCellData;
			//	purchaseSuperConsumableSuccess();
				//				PurchaseUtil.purchaseSuperConumble();
				return;
				
			}
						
			if (playerStats.CanAffordConsumable(consumableID) == true)
			{
				consumableToPurchase = consumableCellData;
				//UIConfirmDialogOz.onNegativeResponse += OnPurchaseNo;
				//UIConfirmDialogOz.onPositiveResponse += OnPurchaseYes;
				//UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog(consumableCellData.data.Title, "Purchase this consumable?", "Btn_No", "Btn_Yes");	
				//playerStats.PurchaseConsumable(consumableID);
				OnPurchaseYes();	// buy it if we can afford it
			}
			else
			{
				UIConfirmDialogOz.onNegativeResponse += invViewCont.OnNeedMoreCoinsNo;
				UIConfirmDialogOz.onPositiveResponse += invViewCont.OnNeedMoreCoinsYes;
				//UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreCoins_Prompt","Lbl_Dialogue_MoreCoins_Confirm", "Btn_No", "Btn_Yes");
				UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreCoins_Prompt", "Btn_No", "Btn_Yes");
			}
		}
//		else
//		{
//			UIOkayDialogOz.onPositiveResponse += OnAlreadyMaxedOut;
//			UIManagerOz.SharedInstance.okayDialog.ShowOkayDialog("You've maxed out this consumable","Maximum " 
//				+ ConsumableStore.maxOfEachConsumable.ToString() + " in inventory at once!", "Btn_Ok"); 
//		}
		
		UIManagerOz.SharedInstance.PaperVC.UpdateCurrency();
		//invViewCont.UpdateCurrency();									// will update coin and gem counts in UI				
		consumableCellData.Refresh();									// ask cell to update its GUI rendering to match data, in case it was updated in transaction
	}
	
	private void ShowDownloadPrompt()
	{
		UIManagerOz.SharedInstance.StartDownloadPrompts(true, false, false);
	}
	
	//void Update() { }
}


	
//	public void AnimationDone()
//	{
		//animating = false;
//	}		
	

//	public void Refresh() 
//	{
//		//grid = gameObject.transform.Find("consumableGrid").gameObject;			// connect to this panel's grid automatically		
//		//ClearGrid(grid);														// kill all old objects under grid, prior to refresh
//		
//		List<BaseConsumable> sortedDataList = SortGridItemsByPriority(ConsumableStore.consumablesList);
//		
//		foreach (BaseConsumable consumableData in sortedDataList)
//		{
//			//CreatePanel(consumableData, grid);								//GameObject obj = CreatePanel(consumableData, grid);
//			CreatePanel(consumableData, grid).name = GenerateCellLabel(consumableData);
//		}
//		
//		grid.GetComponent<UITable>().sorted = true;
//		//grid.GetComponent<UIGrid>().Reposition();								// reset/correct positioning of all objects inside grid
//		grid.GetComponent<UITable>().Reposition();								// reset/correct positioning of all objects inside grid
//	}


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



//		if (!animating)
//		{
//			animating = true;
//			selectedCell = cell.transform.parent.gameObject.GetComponent<SubPanel>().OnCellPressed(cell, selectedCell);
//		}

	
//	private void OnPurchaseNo()
//	{
//		UIConfirmDialogOz.onNegativeResponse -= OnPurchaseNo;
//		UIConfirmDialogOz.onPositiveResponse -= OnPurchaseYes;
//	}
	

	
//	public void OnAlreadyMaxedOut() 
//	{
//		UIOkayDialogOz.onPositiveResponse -= OnAlreadyMaxedOut;
//	}			
	

		//private GameObject selectedCell = null;
	//private bool animating = false;
	
//	public void OnConsumableCellPressed(GameObject cell) 	
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

//		else if (playerStats.IsArtifactGemmed(consumableID) == false)	// check if already gemmed	
//		{ 
//			if (playerStats.CanAffordArtifactGem(consumableID) == true)
//			{
//				playerStats.GemArtifact(consumableID);			// gem it if we can afford it
//			}
//			else
//			{
//				UIConfirmDialogOz.onNegativeResponse += invViewCont.OnNeedMoreGemsNo;
//				UIConfirmDialogOz.onPositiveResponse += invViewCont.OnNeedMoreGemsYes;
//				UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Need More Gems!","Pick up gems while running.", "OK", "OK");
//			}
//		}


//	//-----------------------------
//	
//	private int equippedArtifact = -1;
//	private int equippedPower = -1;
//	
//	public void SetEquippedArtifact(int artifactID) 
//	{
//		TR.LOG ("SetEquippedArtifact {0}", artifactID);
//		equippedArtifact = artifactID;
//		equippedPower = -1;
//		//UpdateEquippedCell("Pick an Artifact");
//	}
//	
//	private void UpdateArtifacts() 
//	{
//		//CellData[] cells = grid.GetComponentsInChildren<CellData>(true) as CellData[];
//		//foreach (CellData item in cells) 
//		//{
//		//	if (item == null) { continue; }
//		//	UpdateArtifactCellData(item, player, activeCharacter);
//		//}
//	}	
//	
//	//private void UpdateArtifactCellData(CellData cellData, PlayerStats player, CharacterStats activeCharacter) 
//	private void UpdateArtifactCellData(GameObject cell)
//	{
//		PlayerStats player = GameProfile.SharedInstance.Player;
//		CharacterStats activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
//		
//		//if (cellData == null) { return; }
//		
//		//GameObject newCell = cellData.gameObject;
//		//int artifactID = cellData.Data;
//		int artifactID = cell.transform.parent.GetComponent<ArtifactCellData>().data._id;
//		
//		ArtifactProtoData protoData = ArtifactStore.Artifacts[artifactID];
//		bool purchased = player.IsArtifactPurchased(artifactID);
//		bool equipped = activeCharacter.isArtifactEquipped(artifactID);
//		
//		//UpdateCellData(newCell, protoData._title, protoData._iconName, protoData._description, protoData._cost.ToString(), purchased, equipped);
//		UpdateCellData(cell, protoData._title, protoData._iconName, protoData._description, protoData._cost.ToString(), purchased, equipped);	
//	}	
//	
//	
//	private void UpdateCellData(GameObject newCell, string title, string iconName, string description, string coinValue, bool purchased, bool equipped)
//	{
//		GameObject go = HierarchyUtils.GetChildByName("Title", newCell);
//		if (go != null) 
//		{
//			UILabel titleLabel = go.GetComponent<UILabel>() as UILabel;
//			if (titleLabel != null) { titleLabel.text = title; }//titleLabel.color = ArtifactStore.colorForRarity(protoData._rarity);
//		}
//		
//		go = HierarchyUtils.GetChildByName("Icon", newCell);
//		if (go != null) 
//		{
//			UISprite iconSprite = go.GetComponent<UISprite>() as UISprite;
//			if (iconSprite != null) { iconSprite.spriteName = iconName; }
//		}
//		
//		go = HierarchyUtils.GetChildByName("Description", newCell);
//		if (go != null) 
//		{
//			UILabel desc = go.GetComponent<UILabel>() as UILabel;
//			if (desc != null) { desc.text = description; }
//		}
//		
//		go = HierarchyUtils.GetChildByName("Cost", newCell);
//		if (go != null) 
//		{
//			GameObject buyButton = HierarchyUtils.GetChildByName("BuyButton", newCell);
//			GameObject CoinDisplayIcon = HierarchyUtils.GetChildByName("CoinDisplayIcon", newCell);
//			UILabel cost = go.GetComponent<UILabel>() as UILabel;
//			if (cost != null)
//			{
//				if (purchased == true) 
//				{
//					if(CoinDisplayIcon != null) { NGUITools.SetActive(CoinDisplayIcon, false); }
//					
//					if (equipped == true) 
//					{
//						cost.text = "equipped";
//						if (buyButton != null) { NGUITools.SetActive(buyButton, false); }
//					}
//					else 
//					{
//						cost.text = "available";	//equip";
//						if (buyButton != null) { NGUITools.SetActive(buyButton, true); }
//					}
//				}
//				else 
//				{
//					if (buyButton != null) { NGUITools.SetActive(buyButton, true); }
//					cost.text = coinValue;
//					if (CoinDisplayIcon != null) { NGUITools.SetActive(CoinDisplayIcon, true); }
//				}
//			}
//		}
//	}
//}
//	

	//UpdateArtifactCellData(cell);	//, playerStats, activeCharacter);	//UpdateArtifactCellData(cellData, playerStats, activeCharacter);

//	
//	public void OnArtifactCellPressed(GameObject cell) 
//	{
//		Debug.LogWarning("OnArtifactCellPressed called!");
//		
//		//-- Can we equip this?
//		bool closeDialog = false;
//		if(cell != null /*&& equipInSlot != ArtifactSlotType.Total*/) {
//			//-- equip it
//			
//			CellData cellData = cell.GetComponent<CellData>() as CellData;
//			if(cellData != null) {
//				int artifactID = cellData.Data;
//				CharacterStats activeCharacter = GameProfile.SharedInstance.GetActiveCharacter();
//				PlayerStats playerStats = GameProfile.SharedInstance.Player;
//				
//				//We don't care about "Equipping" in Oz. Just equip it. 
//			//	SetEquippedArtifact(activeCharacter.getArtifactForSlot(artifactID));
//				
//				if(playerStats.IsArtifactPurchased(artifactID) == false) {
//					//-- Can we afford it?
//					if(playerStats.CanAffordArtifact(artifactID) == false) {
//						UIConfirmDialogOz.onNegativeResponse += viewController.GetComponent<UIInventoryViewControllerOz>().OnNeedMoreCoinsNo;
//						UIConfirmDialogOz.onPositiveResponse += viewController.GetComponent<UIInventoryViewControllerOz>().OnNeedMoreCoinsYes;
//						UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Need More Coins!","Would you like to get more coins?", "No", "Yes");
//						return;
//					}
//					
//					//-- Buy it if we can afford it.
//					playerStats.PurchaseArtifact(artifactID);
//					UpdateArtifactCellData(cellData, playerStats, activeCharacter);
//					viewController.GetComponent<UIInventoryViewControllerOz>().UpdateCurrency();
//				}
//				
//				// code below commented out by Alex, because it's not needed for Artifacts, since we can't equip them
//				/*
//				else if(GameProfile.SharedInstance.IsArtifactEquipped(artifactID, activeCharacter.characterId) == false) {
//					activeCharacter.equipArtifactForSlot(artifactID, equipInSlot);
//					if(characterSelectVC != null) {
//						characterSelectVC.UpdateCharacterCard(activeCharacter);	
//					}
//						
//					equippedArtifact = artifactID;
//					UpdateEquippedCell();
//					UpdateArtifactCellData(cellData, playerStats, activeCharacter);
//					GameProfile.SharedInstance.Serialize();
//					//closeDialog = true;
//				}
//				else {
//					//-- Can't equip this because its already equipped.
//					//-- Show error dialog
//					UIConfirmDialogOz.onPositiveResponse += OnAlreadyEquipped;
//					UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Oops!", "That Artifact is already in use.", "Ok", "");
//					return;
//				}	
//				*/
//			}
//		}	
//		
//		if (closeDialog == true) { viewController.GetComponent<UIInventoryViewControllerOz>().OnBackButton(); }
//	}

//	public void OnGemItemPressed() 
//	{
//		int itemID = -1;
//		Buff newBuff = null;
//		if (equippedArtifact != -1) { return; }
//		
//		itemID = equippedPower;
//		if (itemID == -1) { return; }
//		
//		BasePower basePower = PowerStore.Powers[itemID];
//		if (basePower == null) { return; }
//		newBuff = new Buff(basePower.ProtoBuff.ToDict());
//		newBuff.itemID = itemID;
//		
//		int gemcost = GameProfile.SharedInstance.Player.GetBuffCost(BuffType.Powerup, itemID, newBuff);
//		if (gemcost < 0) { gemcost = 1; }
//		
//		if(GameProfile.SharedInstance.Player.specialCurrencyCount < gemcost)
//		{
//			UIConfirmDialogOz.onNegativeResponse += OnNeedMoreGemsNo;
//			UIConfirmDialogOz.onPositiveResponse += OnNeedMoreGemsYes;
//			UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Need More Gems!","Would you like to get more Gems?", "No", "Yes");
//			return;
//		}
//		
//		//-- Augment the power.
//		GameProfile.SharedInstance.Player.specialCurrencyCount -= gemcost;
//		if (GameProfile.SharedInstance.Player.specialCurrencyCount < 0) { GameProfile.SharedInstance.Player.specialCurrencyCount = 0; }
//		GameProfile.SharedInstance.Player.CreateBuff(BuffType.Powerup, itemID, basePower.ProtoBuff);
//		GameProfile.SharedInstance.Serialize();
//		UpdateEquippedCell(null);
//		updateCurrency();
//	}	
