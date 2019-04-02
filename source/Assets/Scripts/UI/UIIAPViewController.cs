/*

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//public struct IAP_DATA 
//{
//	public string 		iconname;
//	public CostType 	costType;
//	public int			costValueOrID;
//	
//	public string		title;
//	public string		description;
//	public string		price;
//}

public class UIIAPViewController : UIViewController
{
	public static UIIAPViewController Instance;
	
	public override void Awake() {
		Instance = this;
		base.Awake();
	}
	
	public enum AndroidIAPMechanism
	{
		None,
		GooglePlay,
		Amazon
	};
	
#if UNITY_ANDROID
	public enum BillingAvaialbleStatus
	{
		Checking,
		NotAllowed,
		Allowed
	}
	
	BillingAvaialbleStatus _BillingStatus;

	public BillingAvaialbleStatus BillingStatus
	{
		get
		{
			return _BillingStatus;
		}
	}
#endif	
	
	
	public Transform 					BackButtonRoot = null;
	public UIInGameViewController 		inGameVC = null;
	public UIViewController				freeStuffVC = null;
	public UILabel 						errorLabel = null;
	public UIGrid						gridRoot = null;
	
	private Dictionary<string, IAP_DATA> iapTable = null;
	
	private ArtifactProtoData		coinDoublerData = null;
	public override void OnBackButton() {
		if(previousViewController != null) {
			if(previousViewController == inGameVC) {
				OnReturnFromIAPStoreGem();
				return;
			}
		}
		
		base.OnBackButton();
	}
	
	public void OnReturnFromIAPStoreGem() {
		notify.Debug ("OnReturnFromIAPStore_PostRes1");
		base.OnBackButton();
		
		//-- Do logic based on if we can afford the res.
		if(GameProfile.SharedInstance.Player.CanAffordResurrect() == false) {
			notify.Debug ("OnReturnFromIAPStore_PostRes2a");
			inGameVC.chooseToResurrect = false;
			inGameVC.OnDiePostGame();
		}
		else {
			notify.Debug ("OnReturnFromIAPStore_PostRes2b");
			inGameVC.OnResurrect();
		}
	}
	
	static List<string> ProductIdentifiers = null;
	
	static string IAP_COINS_MIN = "com.imangi.templerun2.coinpacka";
	static string IAP_COINS_SMALL = "com.imangi.templerun2.coinpackb";
	static string IAP_COINS_MEDIUM = "com.imangi.templerun2.coinpackc";
	static string IAP_COINS_LARGE = "com.imangi.templerun2.coinpackd";
	static string IAP_GEMS_MIN = "com.imangi.templerun2.gempacka";
	static string IAP_GEMS_SMALL = "com.imangi.templerun2.gempackb";
	static string IAP_GEMS_MEDIUM = "com.imangi.templerun2.gempackc";
	static string IAP_GEMS_LARGE = "com.imangi.templerun2.gempackd";
	static string IAP_COINDOUBLER = "com.imangi.templerun2.coindoubler"; //<-- non consumable
	
	void populateProducts() {
		if(ProductIdentifiers == null) {
			ProductIdentifiers = new List<string>();
			ProductIdentifiers.Add (IAP_COINS_MIN);
			ProductIdentifiers.Add (IAP_COINS_SMALL);
			ProductIdentifiers.Add (IAP_COINS_MEDIUM);
			ProductIdentifiers.Add (IAP_COINS_LARGE);
			ProductIdentifiers.Add (IAP_GEMS_MIN);
			ProductIdentifiers.Add (IAP_GEMS_SMALL);
			ProductIdentifiers.Add (IAP_GEMS_MEDIUM);
			ProductIdentifiers.Add (IAP_GEMS_LARGE);
			ProductIdentifiers.Add (IAP_COINDOUBLER);
		}
		
		if(iapTable == null) {
			iapTable = new Dictionary<string, IAP_DATA>();
			
			IAP_DATA cd = new IAP_DATA();
			cd.costType = CostType.Coin;
			cd.costValueOrID = 2500;
			cd.iconname = "powerup_coin_bonus";
			cd.title = string.Format("{0:#,###0}", cd.costValueOrID);
			cd.description = "";
			cd.price = "Buy Now!";
			iapTable.Add(UIIAPViewController.IAP_COINS_MIN, cd);
			
			cd = new IAP_DATA();
			cd.costType = CostType.Coin;
			cd.costValueOrID = 25000;
			cd.iconname = "powerup_coin_bonus";
			cd.title = string.Format("{0:#,###0}", cd.costValueOrID);
			cd.description = "";
			cd.price = "Buy Now!";
			iapTable.Add(UIIAPViewController.IAP_COINS_SMALL, cd);
			
			cd = new IAP_DATA();
			cd.costType = CostType.Coin;
			cd.costValueOrID = 75000;
			cd.iconname = "powerup_coin_bonus";
			cd.title = string.Format("{0:#,###0}", cd.costValueOrID);
			cd.description = "";
			cd.price = "Buy Now!";
			iapTable.Add(UIIAPViewController.IAP_COINS_MEDIUM, cd);
			
			cd = new IAP_DATA();
			cd.costType = CostType.Coin;
			cd.costValueOrID = 200000;
			cd.iconname = "powerup_coin_bonus";
			cd.title = string.Format("{0:#,###0}", cd.costValueOrID);
			cd.description = "";
			cd.price = "Buy Now!";
			iapTable.Add(UIIAPViewController.IAP_COINS_LARGE, cd);
			
			cd = new IAP_DATA();
			cd.costType = CostType.Special;
			cd.costValueOrID = 10;
			cd.iconname = "powerup_gem_bonus";
			cd.title = string.Format("{0:#,###0}", cd.costValueOrID);
			cd.description = "";
			cd.price = "Buy Now!";
			iapTable.Add(UIIAPViewController.IAP_GEMS_MIN, cd);
			
			cd = new IAP_DATA();
			cd.costType = CostType.Special;
			cd.costValueOrID = 100;
			cd.iconname = "powerup_gem_bonus";
			cd.title = string.Format("{0:#,###0}", cd.costValueOrID);
			cd.description = "";
			cd.price = "Buy Now!";
			iapTable.Add(UIIAPViewController.IAP_GEMS_SMALL, cd);
			
			cd = new IAP_DATA();
			cd.costType = CostType.Special;
			cd.costValueOrID = 1000;
			cd.iconname = "powerup_gem_bonus";
			cd.title = string.Format("{0:#,###0}", cd.costValueOrID);
			cd.description = "";
			cd.price = "Buy Now!";
			iapTable.Add(UIIAPViewController.IAP_GEMS_MEDIUM, cd);
			
			cd = new IAP_DATA();
			cd.costType = CostType.Special;
			cd.costValueOrID = 10000;
			cd.iconname = "powerup_gem_bonus";
			cd.title = string.Format("{0:#,###0}", cd.costValueOrID);
			cd.description = "";
			cd.price = "Buy Now!";
			iapTable.Add(UIIAPViewController.IAP_GEMS_LARGE, cd);
			
			cd = new IAP_DATA();
			cd.costType = CostType.RealMoney;
			cd.costValueOrID = coinDoublerData._id;
			cd.iconname = "coin_doubler";
			cd.title = "Coin Doubler";
			cd.description = "";
			cd.price = "Buy Now!";
			iapTable.Add(UIIAPViewController.IAP_COINDOUBLER, cd);
		}
	}
	
	public override void disappear (bool hidePaper)
	{
		base.disappear (hidePaper);
		UnHookEvents();
	}
	
	public override void appear ()
	{
		base.appear ();
		//-- If we open the store, we have already checked for canmakepayments
		//StoreKitBinding.canMakePayments() == true
		HookEvents();
		
		if(errorLabel != null) {
			NGUITools.SetActive(errorLabel.gameObject, true);
			errorLabel.text = "Retrieving the Store...";
		}
		
		if(gridRoot != null) {
			NGUITools.SetActive(gridRoot.gameObject, false);
		}
		
		//-- find coindoubler
		if(coinDoublerData == null) {
			int max = ArtifactStore.GetNumberOfArtifacts();
			ArtifactProtoData data = null;
			for (int i = 0; i < max; i++) {
				data = ArtifactStore.GetArtifactProtoData(i);
				if(data == null)
					continue;
				if(data._costType != CostType.RealMoney )
					continue;
				if(data._statType != StatType.CoinMultiplier)
					continue;
				coinDoublerData = data;
				break;
			}
		}
		
		populateProducts();
		
		
		//-- Do we always want to pull products?
		
#if UNITY_IPHONE
		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			StoreKitBinding.requestProductData(ProductIdentifiers.ToArray());
		}
#elif UNITY_ANDROID
		notify.Debug ("mechanism is "+GameController.SharedInstance.Mechanism);
		if(GameController.SharedInstance.Mechanism == AndroidIAPMechanism.Amazon) {
			AmazonIAP.initiateItemDataRequest(ProductIdentifiers.ToArray());
		}
		else if(GameController.SharedInstance.Mechanism == AndroidIAPMechanism.GooglePlay) {
			IABAndroid.init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAn1u2YeH2E6+usI47Me1ZpAfoEFmv4FJ4aAfz3iNVNlRAOInvVA1LNVVw4bU2UAvdGpJgMABtxmyeAUgsoKkRkGm6yCM4ZDpDbAlYucTeigVKQD+WUuYoU9eJm6FCnp0/piORYJ6M0m0SB560Xvc2XZilCODRXXN7dUmsTKPrQUc7wmevA/bppf0nqcFd0oQ2gRaviqJ9ec3sHl7DJi7RM5vdlVgrKq3f5BIfTrfydEn8pK9ACVMQApYgv6Uo1ph1utYl8yKTl6GYENdIncD3ENaon1rNytCiUg71Ync41fqKaMGTW/B+qVzux4AYDENnjoZoUtnx0f7AVnfk8rKpZwIDAQAB");		
		}
#endif
		
#if UNITY_EDITOR			
		createStoreCells();
		if(errorLabel != null) {
			NGUITools.SetActive(errorLabel.gameObject, false);
		}
#endif
	}
	
	void HookEvents()
	{
#if UNITY_IPHONE
		// Listens to all the StoreKit events.  All event listeners MUST be removed before this object is disposed!
//		StoreKitManager.productPurchaseAwaitingConfirmationEvent += productPurchaseAwaitingConfirmationEvent;
		StoreKitManager.purchaseSuccessfulEvent += purchaseSuccessful;
		StoreKitManager.purchaseCancelledEvent += purchaseCancelled;
		StoreKitManager.purchaseFailedEvent += purchaseFailed;
//		StoreKitManager.receiptValidationFailedEvent += receiptValidationFailed;
//		StoreKitManager.receiptValidationRawResponseReceivedEvent += receiptValidationRawResponseReceived;
//		StoreKitManager.receiptValidationSuccessfulEvent += receiptValidationSuccessful;
		StoreKitManager.productListReceivedEvent += productListReceived;
		StoreKitManager.productListRequestFailedEvent += productListRequestFailed;
//		StoreKitManager.restoreTransactionsFailedEvent += restoreTransactionsFailed;
//		StoreKitManager.restoreTransactionsFinishedEvent += restoreTransactionsFinished;
//		StoreKitManager.paymentQueueUpdatedDownloadsEvent += paymentQueueUpdatedDownloadsEvent;
#elif UNITY_ANDROID
		if (GameController.SharedInstance.Mechanism == AndroidIAPMechanism.Amazon) {

			notify.Debug("IAP: AMAZON");
			AmazonIAPManager.onSdkAvailableEvent += onSdkAvailableEvent;
			AmazonIAPManager.itemDataRequestFailedEvent -= itemDataRequestFailedEvent;
			AmazonIAPManager.itemDataRequestFinishedEvent += itemDataRequestFinishedEvent;
			AmazonIAPManager.purchaseFailedEvent += purchaseFailed;
			AmazonIAPManager.purchaseSuccessfulEvent += purchaseSuccessfulEvent;
		}
		else if(GameController.SharedInstance.Mechanism == AndroidIAPMechanism.GooglePlay){

			notify.Debug("IAP: GOOGLE PLAY");
			IABAndroidManager.billingSupportedEvent += billingSupportedEvent;
			IABAndroidManager.purchaseSucceededEvent += purchaseProduct;
			IABAndroidManager.purchaseCancelledEvent += purchaseCancelledEvent;
			IABAndroidManager.purchaseFailedEvent += purchaseFailedEvent;
		}
#endif
	}
	
	void UnHookEvents() {
#if UNITY_IPHONE		
		StoreKitManager.productListReceivedEvent -= productListReceived;
		StoreKitManager.productListRequestFailedEvent -= productListRequestFailed;
		StoreKitManager.purchaseSuccessfulEvent -= purchaseSuccessful;
		StoreKitManager.purchaseCancelledEvent -= purchaseCancelled;
		StoreKitManager.purchaseFailedEvent -= purchaseFailed;
#elif UNITY_ANDROID
		if (GameController.SharedInstance.Mechanism == AndroidIAPMechanism.Amazon) {

			AmazonIAPManager.onSdkAvailableEvent -= onSdkAvailableEvent;
			AmazonIAPManager.itemDataRequestFailedEvent -= itemDataRequestFailedEvent;
			AmazonIAPManager.itemDataRequestFinishedEvent -= itemDataRequestFinishedEvent;
			AmazonIAPManager.purchaseFailedEvent -= purchaseFailed;
			AmazonIAPManager.purchaseSuccessfulEvent -= purchaseSuccessfulEvent;
		}
		else if(GameController.SharedInstance.Mechanism == AndroidIAPMechanism.GooglePlay){

			IABAndroidManager.billingSupportedEvent -= billingSupportedEvent;
			IABAndroidManager.purchaseSucceededEvent -= purchaseProduct;
			IABAndroidManager.purchaseCancelledEvent -= purchaseCancelledEvent;
			IABAndroidManager.purchaseFailedEvent -= purchaseFailedEvent;
		}
#endif		
	}
	
	void purchaseSuccessfulWithProductID( string productID, string developerPayload = "" )
	{
//#if UNITY_ANDROID
//		FlurryAndroid.endTimedEvent("STORE");
//		Dictionary<string,string> d = new Dictionary<string, string>();
//		d.Add("Product", productId);
//		d.Add("Result", "Success");
//		FlurryAndroid.logEvent("PURCHASE", d);
//		EtceteraAndroid.showToast(string.Format("Added {0} coins!", amount), true);
//#endif

		bool needSave = false;
		if(DoesProductIdentiferGiveCoins(productID) == true) {
			GameProfile.SharedInstance.Player.coinCount += getCurrencyValueFromProductIdentifier(productID);
			needSave = true;
		}
		else if(DoesProductIdentiferGiveGems(productID) == true) {
			GameProfile.SharedInstance.Player.specialCurrencyCount += getCurrencyValueFromProductIdentifier(productID);
			needSave = true;
		}
		else if(productID == UIIAPViewController.IAP_COINDOUBLER && coinDoublerData != null) {
			GameProfile.SharedInstance.Player.PurchaseArtifact(coinDoublerData._id, coinDoublerData);
		}
		
		if(needSave == true) {
			GameProfile.SharedInstance.Serialize();
		}
		AudioManager.SharedInstance.PlayFX(AudioManager.Effects.angelWings);
		OnBackButton();
	}
	
	void productListRequestFailed( string error )
	{
		notify.Warning( "productListRequestFailed: " + error );
		UIConfirmDialog.onPositiveResponse += OnBadProductRequest;
		UIManager.SharedInstance.confirmDialog.ShowInfoDialog("Ooops!", "Failed to get Products", "Btn_Ok");
		
		if(errorLabel != null) {
			NGUITools.SetActive(errorLabel.gameObject, true);
			errorLabel.text = "The Store is offline.";
		}
	}
	
	void createStoreCells() {
		if(gridRoot == null)
			return;
		
		NGUITools.SetActive(gridRoot.gameObject, true);
		if(errorLabel != null) {
			NGUITools.SetActive(errorLabel.gameObject, false);
		}
		
		//-- Sort the list?
		
		int max = iapTable.Count;
		int totalItems = max;
		if(freeStuffVC != null) {
			totalItems+=1;
		}
		
		GameObject protoCell = (GameObject)Resources.Load ("interface/IAPStoreCell", typeof(GameObject));
		
		int i = 0;
		foreach(KeyValuePair<string, IAP_DATA> data in iapTable) {
			IAP_DATA item = data.Value;
//			if(i < max) {
				GameObject newCell = null;
				if(i < gridRoot.transform.GetChildCount()) {
					newCell = gridRoot.transform.GetChild(i).gameObject;
					if(newCell == null)
						continue;
				}
				else {
					newCell = Instantiate(protoCell) as GameObject;	
					if(newCell == null)
						continue;
					Destroy(newCell.GetComponent<UIPanel>());
					newCell.transform.parent = gridRoot.transform;
					newCell.transform.localScale = Vector3.one;
					newCell.transform.rotation = gridRoot.transform.rotation;
					newCell.transform.localPosition = Vector3.zero;
				}
				
				GameObject go = GetChildByName("Title", newCell);
				if(go != null) {
					NGUITools.SetActive(go, true);
					UILabel titleLabel = go.GetComponent<UILabel>() as UILabel;
					if(titleLabel != null) {
						titleLabel.text = item.title;	
						titleLabel.MakePixelPerfect();
					}
				}
				
				go = GetChildByName("Icon", newCell);
				if(go != null) {
					NGUITools.SetActive(go, true);
					UISprite iconSprite = go.GetComponent<UISprite>() as UISprite;
					if(iconSprite != null) {
						iconSprite.spriteName = GetIconNameFromProductIdentifer(data.Key);
					}
				}
				
				
				
				go = GetChildByName("Cost", newCell);
				if(go != null) {
					NGUITools.SetActive(go, true);
					UILabel cost = go.GetComponent<UILabel>() as UILabel;
					if(cost != null) {
						cost.text = item.price;
						cost.MakePixelPerfect();
						notify.Debug("Setting cost item={0}, cost={1}", item.price, cost.text);
					}
				}
				
				bool purchased = false;
				go = GetChildByName("BuyButton", newCell);
				if(go != null) {
					if(data.Key == UIIAPViewController.IAP_COINDOUBLER) {
						if(GameProfile.SharedInstance.Player.IsArtifactPurchased(coinDoublerData._id) == true) {
							purchased = true;
							NGUITools.SetActive(go, !purchased);
						}
					}	
				}
				
				//-- Turn of description for now.
				go = GetChildByName("Description", newCell);
				if(go != null) {
					NGUITools.SetActive(go, false);
//					UILabel desc = go.GetComponent<UILabel>() as UILabel;
//					if(desc != null) {
//						if(purchased == false) {
//							desc.text = item.description;	
//						}
//						else {
//							desc.text = "Purchased!";
//						}
//						
//					}
				}
				
				go = GetChildByName("CellContents", newCell);
				if(go != null) {
					UIButtonMessage message = go.GetComponent<UIButtonMessage>() as UIButtonMessage;
					if(message != null) {
						if(purchased == false) {
							message.target = this.gameObject;
							message.functionName = "OnBuyIAP";	
						}
						else {
							message.target = null;
						}
					}
					
					CellData cellData = go.GetComponent<CellData>() as CellData;
					if(cellData == null) {
						//-- Add the script.
						cellData = go.AddComponent<CellData>() as CellData;
					}
					cellData.DataString = data.Key;
					cellData.cellParent = newCell.transform;
				}
				notify.Debug ("Adding Product"+data.Key);	
//			}
			i++;
			if(i == max) {
				//GameObject newCell = null;
				if(i < gridRoot.transform.GetChildCount()) {
					newCell = gridRoot.transform.GetChild(i).gameObject;
					
					if(newCell == null)
						continue;
				}
				else {
					newCell = Instantiate(protoCell) as GameObject;	
					if(newCell == null)
						continue;
					Destroy(newCell.GetComponent<UIPanel>());
					newCell.transform.parent = gridRoot.transform;
					newCell.transform.localScale = Vector3.one;
					newCell.transform.rotation = gridRoot.transform.rotation;
					newCell.transform.localPosition = Vector3.zero;
				}
				
				go = GetChildByName("Title", newCell);
				if(go != null) {
					UILabel titleLabel = go.GetComponent<UILabel>() as UILabel;
					if(titleLabel != null) {
						titleLabel.text = "Get FREE Stuff!";	
					}
				}
				
				go = GetChildByName("Icon", newCell);
				if(go != null) {
					UISprite iconSprite = go.GetComponent<UISprite>() as UISprite;
					if(iconSprite != null) {
						iconSprite.spriteName = "store";
					}
				}
				
				go = GetChildByName("Description", newCell);
				if(go != null) {
					NGUITools.SetActive(go, false);
				}
				
				go = GetChildByName("BuyButton", newCell);
				if(go != null) {
					NGUITools.SetActive(go, false);
				}
				
				go = GetChildByName("CellContents", newCell);
				if(go != null) {
					UIButtonMessage message = go.GetComponent<UIButtonMessage>() as UIButtonMessage;
					if(message != null) {
						message.target = this.gameObject;
						message.functionName = "OnFreeStuff";
					}
				}
			}
			
		}
		gridRoot.Reposition();
	}
	
#if UNITY_IPHONE
	void productListReceived( List<StoreKitProduct> productList )
	{
		if(productList != null && productList.Count > 0 && gridRoot != null) {
			if(gridRoot != null) {
				NGUITools.SetActive(gridRoot.gameObject, true);
			}
			createStoreCellsWithStoreKitProducts(productList);
		}
	}

	void purchaseSuccessful( StoreKitTransaction transaction )
	{
		purchaseSuccessfulWithProductID(transaction.productIdentifier);	
	}
	
	void createStoreCellsWithStoreKitProducts(List<StoreKitProduct> productList) {
		foreach (StoreKitProduct ai in productList) {
			IAP_DATA data = iapTable[ai.productIdentifier];
			data.price = ai.formattedPrice;
			data.title = ai.title;
			data.description = ai.description;
			iapTable[ai.productIdentifier] = data;
		}
		createStoreCells();
	}
	
#endif
	
	void purchaseFailed( string error )
	{
	}
	
	void purchaseCancelled( string error )
	{
		
	}
	
	
#if UNITY_ANDROID
	//=======================================================================
	// Amazon IAP
	void onSdkAvailableEvent(bool isTestMode)
	{
		notify.Debug("AMAZON: onSdkAvailableEvent. isTestMode: " + isTestMode);
		_BillingStatus = BillingAvaialbleStatus.Allowed;
	}

	void itemDataRequestFinishedEvent(List<string> unavailableSkus, List<AmazonItem> availableItems)
	{
		notify.Debug("AMAZON: itemDataRequestFinishedEvent. unavailable skus: " + unavailableSkus.Count + ", avaiable items: " + availableItems.Count);
		foreach (AmazonItem ai in availableItems) {
			IAP_DATA data = iapTable[ai.sku];
			data.price = ai.price;
			data.title = ai.title;
			data.description = ai.description;
			iapTable[ai.sku] = data;
		}
		createStoreCells();
	}

	void itemDataRequestFailedEvent()
	{
		productListRequestFailed("AMAZON: itemDataRequestFailedEvent");
	}

	void purchaseSuccessfulEvent(AmazonReceipt receipt)
	{
		notify.Debug("AMAZON: purchaseSuccessfulEvent: " + receipt);
		purchaseSuccessfulWithProductID(receipt.sku);
	}
	
	//=======================================================================
	// Google IAP
	void billingSupportedEvent(bool isSupported)
	{
		notify.Debug("Google billingSupportedEvent: " + isSupported);
		_BillingStatus = (isSupported) ? BillingAvaialbleStatus.Allowed : BillingAvaialbleStatus.NotAllowed;
		if(BillingStatus == BillingAvaialbleStatus.NotAllowed) {
			productListRequestFailed("Not supported.");
		}
		else if(BillingStatus == BillingAvaialbleStatus.Allowed) {
			createStoreCells();
		}
	}
	
	void purchaseProduct( string productId, string developerPayload ) {
		purchaseSuccessfulWithProductID(productId, developerPayload);
	}
	void purchaseCancelledEvent( string productId, string developerPayload) {
		notify.Debug ("purchaseCancelledEvent " + productId + " " + developerPayload);
	}
	void purchaseFailedEvent( string productId, string developerPayload) {
		notify.Debug ("purchaseFailedEvent " + productId + " " + developerPayload);
	}
#endif

	void OnBadProductRequest() {
		UIConfirmDialog.onPositiveResponse -= OnBadProductRequest;
		base.OnBackButton();
	}
	
	void OnFreeStuff() {
		if(freeStuffVC != null) {
			disappear(false);	
			freeStuffVC.previousViewController = this;
			freeStuffVC.appear();
		}
	}
	
	void OnBuyIAP(GameObject cell) {
		if(cell == null)
			return;
		
		CellData cellData = cell.GetComponent<CellData>() as CellData;
		if(cellData == null || cellData.DataString == null)
			return;
		
		
#if UNITY_IPHONE			
		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			StoreKitBinding.purchaseProduct(cellData.DataString, 1);
		}
#elif UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android) {
			if(GameController.SharedInstance.Mechanism == AndroidIAPMechanism.Amazon) {
 
			} 
			else if(GameController.SharedInstance.Mechanism == AndroidIAPMechanism.GooglePlay) {
 
			}
		}
#elif UNITY_EDITOR
		purchaseSuccessfulWithProductID(cellData.DataString);
#endif
	}
	
	public void SizeForScreen() {
		//-- ONLY CALL THIS FOR 1136 for now.
		UIPanel artP = gridRoot.transform.parent.GetComponent<UIPanel>() as UIPanel;
		Vector4 temp = Vector4.zero;
		if(artP != null) {
			temp = artP.clipRange;
			temp.w = 930;
			artP.clipRange = temp;
		}
		
		UIDraggablePanel dp = gridRoot.transform.parent.GetComponent<UIDraggablePanel>() as UIDraggablePanel;
		if(dp) {
			dp.repositionClipping = true;
		}
	}
	
	//Find a child in go by its name
    private GameObject GetChildByName(string name, GameObject go)
    {
        GameObject result = null;
        Transform t = go.transform.FindChild(name);

        if (t != null)
            return t.gameObject;

        for(int i=0; i<go.transform.childCount; i++)
		{
			t = go.transform.GetChild(i);
			result = GetChildByName(name, t.gameObject);
			if(result != null)
				return result;
		}
		return null;
    }
	
	private int getCurrencyValueFromProductIdentifier(string pid) {
		if(pid == null)
			return 0;
		IAP_DATA data;
		if(iapTable.TryGetValue(pid, out data)) {
			return data.costValueOrID;
		}
		return 0;
	}
	
	private string GetIconNameFromProductIdentifer(string pid) {
		IAP_DATA data;
		if(iapTable.TryGetValue(pid, out data)) {
			return data.iconname;
		}
		return " ";
	}
	
	private bool DoesProductIdentiferGiveGems(string pid) {
		if(pid == null)
			return false;
		IAP_DATA data;
		if(iapTable.TryGetValue(pid, out data)) {
			return (data.costType == CostType.Special);
		}
		return false;
	}
	private bool DoesProductIdentiferGiveCoins(string pid) {
		if(pid == null)
			return false;
		IAP_DATA data;
		if(iapTable.TryGetValue(pid, out data)) {
			return (data.costType == CostType.Coin);
		}
		return false;
	}
}

 */
