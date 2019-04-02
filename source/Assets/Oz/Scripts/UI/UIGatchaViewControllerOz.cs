using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIGatchaViewControllerOz : UIViewControllerOz
{
	
	public Transform cardPanel;
	public GatchaCard[] cards;
	public Transform bannerPanel;
	public UILabel bannerLabel;
	public int availableCards = 0;
	public Transform noCardsPanel;
	public int startingPrice = 200;
	private int currentPrice = 200;
	public Transform nextPanel;
	public UILabel scoreLabel;
	public GameObject instructionLabel;
	public UILabel instructionText;
	public UISprite instructionFade;
	public UILabel playerBank;
	
	public UISprite[] keys;
	private Vector3 key1Pos;
	private Vector3 key2Pos;
	private Vector3 key3Pos;
	private int currentKey = 0;
	private bool cardFromRun = false;
	
	
	private Vector3 origCardSize;
	public Vector3 openCardSize;
	
	public Material[] fxMaterials;
	
//	private float timer = 0f;
	
	
	[HideInInspector]
	public bool visible = false;
	private bool noCardsDisplayed = true; // this needs to be saved in gameProfile
	private bool showNoMoreCards = false;
	private GatchaData data;
	private int totalCardsFlipped = 0;
	private bool canSwitchCard = false;
	private bool canSwitchCard2 = false;
	
	public GameObject coinPrefab;
	private List<Transform> coins;
	//private int coinsAnimatedCount = 0;
	//private int localScore;
	//private int localCoins;
	private int gatchaCounter = 0;
	
	private int maxCoinScoreThisRun = 0;
	
	protected override void Awake(){
		//notify = new Notify(this.GetType().Name);	
		base.Awake();
		notify.Debug("Gatcha Awake");
		coins = new List<Transform>();
		data = GetComponent<GatchaData>();
		int noCardsDisplayedInt = PlayerPrefs.GetInt("noCardsDisplayed",0); 
		if(noCardsDisplayedInt == 0) noCardsDisplayed = false;
		else noCardsDisplayed = true;
		notify.Debug("gatcha Start() noCardsDisplayed " + noCardsDisplayed + " noCardsDisplayedInt " + noCardsDisplayedInt);
		//spawnAnimItems();
		origCardSize = cards[0].musicBox.transform.localScale;
		key1Pos = keys[0].transform.localPosition;
		key2Pos = keys[1].transform.localPosition;
		key3Pos = keys[2].transform.localPosition;
		SetGatchCounterFromPrefs();
		
	}
	
	public void SetGatchCounterFromPrefs()
	{
		gatchaCounter = PlayerPrefs.GetInt("gatchaCounter", 0);		
	}
	
	protected override void Start(){
			
		//renderqueue
				
		Material mat = cards[0].icon.material;
		Material iconMat = new Material(mat.shader);
		iconMat.renderQueue = 4010;
		iconMat.mainTexture = mat.mainTexture;
		
		mat = cards[0].musicBox.material;
		Material musicBoxMat = new Material(mat.shader);
		musicBoxMat.renderQueue = 2100;
		musicBoxMat.mainTexture = mat.mainTexture;
		

		
		foreach( Material fxMatNew in fxMaterials){
			fxMatNew.renderQueue = 3500;
		}
		
		
					
		foreach(GatchaCard card in cards){
			//card.icon.material.renderQueue = 4001;
			card.icon.material = iconMat;
			//card.musicBox.material.renderQueue = 2100;
			card.musicBox.material = musicBoxMat;
		}
		
		mat = scoreLabel.material;
		Material scoreLabelMat = new Material(mat.shader);
		scoreLabelMat.renderQueue = 4100;
		scoreLabelMat.mainTexture = mat.mainTexture;
		scoreLabel.material = scoreLabelMat;
		//scoreLabel.material.renderQueue = 4100;
		
	
		
		mat = keys[0].material;
		Material keyMat = new Material(mat.shader);
		keyMat.renderQueue = 4800;
		keyMat.mainTexture = mat.mainTexture;
		
		foreach(UISprite key in keys){
			key.material = keyMat;
		}
	}
	
	
	/*
	public override void Start (){
		notify.Debug("Gatcha Start");
		base.Start();
		//disappear();	//false);
	}
	*/
	
	public void spawnAnimItems(){
		if(coinPrefab){
			for(int i=0; i<10; i++){
				GameObject coin = GameObject.Instantiate(coinPrefab,Vector3.right * 1200f, Quaternion.identity) as GameObject;
				coin.transform.parent = myCamera.transform;
				coin.transform.localScale *= 0.5f;
				coin.layer = LayerMask.NameToLayer("gatchaUI");
				//coin.active = false;
				coins.Add(coin.transform);
			}
		}
	}
	
	public override void appear()
	{
		SetupNotify();
		
		notify.Debug("gatcha appear");
		
		base.appear();
		UIManagerOz.SharedInstance.MainGameCamera.enabled = false;
		
		if(availableCards > 2 ){
			availableCards = 2;
		}
		
		//gameObject.SetActiveRecursively(true);
		visible = true;
		Reset();
		//localCoins = GameProfile.SharedInstance.Player.coinCount;
		//localScore = GamePlayer.SharedInstance.Score;
		
		if(availableCards == 1){
			keys[0].gameObject.active = true;
			keys[0].transform.localPosition = key1Pos;
			cardFromRun = true;
		}		
		if(availableCards == 2){
			keys[0].gameObject.active = true;
			keys[1].gameObject.active = true;
			keys[0].transform.localPosition = key1Pos;
			keys[1].transform.localPosition = key2Pos;
			cardFromRun = true;
		}
				
		playerBank.text = GameProfile.SharedInstance.Player.coinCount.ToString();
		
		
		if(gatchaCounter == 0){
			instructionLabel.transform.localPosition = new Vector3(0f,-30f,instructionLabel.transform.localPosition.z);	
			RemoveInstructions();
		}	
		else{
			instructionLabel.transform.localPosition = new Vector3(0f,2000f,instructionLabel.transform.localPosition.z);	
			DealCards();
		}
		
		maxCoinScoreThisRun = GamePlayer.SharedInstance.CoinCountTotal;
	
	}
	
	
	private void RemoveInstructions(){
		iTween.MoveTo(instructionLabel.gameObject, iTween.Hash(
			"y", 2000f,
			"name", "RemoveInstructions",
			"time", 0.5f,
			"delay", 3.0f,
			"islocal", true,
			"oncomplete", "DealCards",
			"oncompletetarget", gameObject
			));
	}
	
	private void RemoveInstructionsNow(){
		iTween.StopByName("RemoveInstructions");
		iTween.MoveTo(instructionLabel.gameObject, iTween.Hash(
			"y", 2000f,
			"time", 0.5f,
			"islocal", true,
			"oncomplete", "DealCards",
			"oncompletetarget", gameObject
			));
	}
	

	private void DealCards(){
		notify.Debug("DealCards");
		
		List<int> ints = new List<int>();
		for(int j=0; j<6; j++){
			ints.Add(j);
		}
		
		for(int i=0; i<6; i++){
			int rand = (int)(Random.value * 0.99999f * ints.Count);
			int id = ints[rand];
			ints.RemoveAt(rand);
			
			GatchaCard card = cards[id];
			card.isFlipped = false;
			card.musicBox.transform.localScale = Vector3.zero;
			card.gameObject.active = true;
			card.musicBox.alpha = 0f;
			iTween.ScaleTo(card.musicBox.gameObject, iTween.Hash(
				"scale", card.origScale * 0.01f,
				"isLocal", true,
				"time", 0.05f,
				"delay", i * 0.4f - 0.3f * i/6,
				"easetype", iTween.EaseType.easeInOutBack,
				"onstart", "OnDealCardsStart",
				"onstarttarget", gameObject,
				"onstartparams", card
				));

		}
		
		
		iTween.MoveTo(bannerPanel.gameObject, iTween.Hash(
			"position", Vector3.up * 48f,
			"isLocal", true,
			"time", 0.3f,
			"delay", 2.5f,
			"easetype", iTween.EaseType.easeOutExpo,
			"oncomplete", "OnAppearComplete",
			"oncompletetarget", gameObject
		));
	}
	
	public void OnDealCardsStart(GatchaCard card){
		card.fx.Play(true);
		AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_Gatcha_Deal);
	
		card.musicBox.alpha = 1f;
		iTween.ScaleTo(card.musicBox.gameObject, iTween.Hash(
			"scale", card.origScale,
			"isLocal", true,
			"time", 0.3f,
			"delay", 0.2f,
			"easetype", iTween.EaseType.easeInOutBack
			));
	}
	

	private void Reset(){
		

			
		foreach(GatchaCard card in cards){
			card.transform.localPosition = card.origLocalPos;
			card.label.alpha = 0f;
			card.label.enabled = false;
			card.icon.enabled = false;
			card.icon.transform.localPosition = new Vector3(-10f, 20f, -200f);	//-100f);
			card.icon.transform.localScale = card.iconOrigScale;
			card.musicBox.spriteName = "musicbox_closed";
			card.musicBox.transform.localScale = origCardSize;
			card.gameObject.active = false;
			

			card.fx.Stop(true);
			card.fx.Clear();
			card.fxEmpty.Stop(true);
			card.fxEmpty.Clear();
			card.fxOpenBox.Stop(true);
			card.fxOpenBox.Clear();
		}
		

		
		foreach(UISprite key in keys){
			key.transform.localPosition = key3Pos;
			key.gameObject.active = false;
		}
		
		bannerPanel.localPosition = Vector3.up * -400f;
		noCardsPanel.localPosition = new Vector3(0f,2000f,-20f);
		nextPanel.localPosition = new Vector3(0f,-1000f,-20f);
		showNoMoreCards = false;
		canSwitchCard = false;
		canSwitchCard2 = false;
		totalCardsFlipped = 0;
		currentKey = 0;
		PopulateCards();
		UpdateLabel();
	}
	
	
	public void OnAppearComplete(){
		notify.Debug("OnAppearComplete");
		canSwitchCard = true;
		canSwitchCard2 = true;
	}
	
	
	public void OnCardPressed(GameObject card){
		Debug.Log("OnCardPressed canSwitchCard " + canSwitchCard);
		if(!canSwitchCard || !canSwitchCard2) return;
		
		notify.Debug("OnCardPressed " + availableCards + " noCardsDisplayed " + noCardsDisplayed);
		
		GatchaCard gc = card.GetComponent<GatchaCard>();
		if(gc.isFlipped) return;
		
		if (availableCards == 0 && GameProfile.SharedInstance.Player.coinCount < currentPrice)		{
			UIConfirmDialogOz.onNegativeResponse += OnNeedMoreCoinsNoInGame;
			UIConfirmDialogOz.onPositiveResponse += OnNeedMoreCoinsYesInGame;
			UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreCoins_Prompt", "Btn_No", "Btn_Yes");	
			return;
		}
	
		
		canSwitchCard = false; // this is comment out since we only use it for noMoreCards
		Invoke("CanSwitchCard", 1f);
		
		
		notify.Debug(gc.name + " " + gc.data.type);
		
		if(gatchaCounter == 0){ // rig the machine to give us something good the first time
			notify.Debug ("Rigging gatcha " + gatchaCounter);
			gatchaCounter = 1;
			PlayerPrefs.SetInt("gatchaCounter", 1);
			PlayerPrefs.Save();
			gc.data.type  = GatchaType.HeadStart;
			gc.data.amount = 1;
			gc.label.text = GameProfile.SharedInstance.Player.GetConsumableLocalizeString(UIManagerOz.SharedInstance.inGameVC.bonusButtons.HeadStartID);
			gc.icon.spriteName = UIManagerOz.SharedInstance.inGameVC.bonusButtons.HeadStartButton.GetComponent<UIImageButton>().normalSprite;
			gc.icon.transform.localScale = gc.iconOrigScale * 0.2f;
		}
		
		notify.Debug("after rigging " + gc.name + " " + gc.data.type);
		
		Hashtable ht = new Hashtable();
		ht.Add("card", gc);
		ht.Add("key", keys[currentKey].gameObject);
		
		ParticleSystem trail = keys[currentKey].GetComponentInChildren<ParticleSystem>();
		if(trail && trail.gameObject.active)
			trail.Play ();
		
		iTween.MoveTo(keys[currentKey].gameObject, iTween.Hash(
			"position", card.transform.position + new Vector3(0.02f,-0.08f,-0.1f),
			"time", 0.4f,
			"oncomplete", "OnKeyArrived",
			"oncompletetarget", gameObject,
			"oncompleteparams", ht
			));
		currentKey++;
		
		if(currentKey < 6){
			keys[currentKey].gameObject.active = true;
			keys[currentKey].enabled = true;
		}
		
		

	}
	
	public void OnKeyArrived(Hashtable ht){
		GatchaCard gc =  ht["card"] as GatchaCard;
		FlipCard(gc);
		GameObject key =  ht["key"] as GameObject;

		ParticleSystem trail = key.GetComponentInChildren<ParticleSystem>();
		if(trail && trail.gameObject.active) {
			trail.Stop();
			trail.Clear ();
		}
		
		key.transform.localPosition = new Vector3(-1600, 0f, 0f);
	}
	
	private void FlipCard(GatchaCard gc){
		if(availableCards > 0){ // use a chance token
			availableCards--;
			GameProfile.SharedInstance.Player.PopChanceToken();
			
			ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.UsedDestinyCard,1);
			if(availableCards == 0){
				notify.Debug("OnCardPressedPost " + availableCards);
				if(!noCardsDisplayed) { // first time only
					showNoMoreCards = true; // next time show the msg
					//ShowNextPanel();
				}
				else{// we just ran out of cards
					currentPrice = startingPrice; // set default price
					UpdateLabel(); // update the label
					ShowNextPanel(); // show the next button
					//cardFromRun = false;
				}
			}		
		}
		else{ 	// spend some coins
			cardFromRun = false;
			if (GameProfile.SharedInstance.Player.coinCount < currentPrice)
			{
				UIConfirmDialogOz.onNegativeResponse += OnNeedMoreCoinsNoInGame;
				UIConfirmDialogOz.onPositiveResponse += OnNeedMoreCoinsYesInGame;
				//UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreCoins_Prompt", "Lbl_Dialogue_MoreCoins_Confirm", "Btn_No", "Btn_Yes");	
				UIManagerOz.SharedInstance.confirmDialog.ShowConfirmDialog("Lbl_Dialogue_MoreCoins_Prompt", "Btn_No", "Btn_Yes");	
				return;
			}
			else
			{
				//int coins = GamePlayer.SharedInstance.CoinCountTotal - currentPrice;
				//UIManagerOz.SharedInstance.inGameVC.scoreUI.AddCoinCountAnim(-currentPrice);
				//GamePlayer.SharedInstance.AddCoinsInstantly(-currentPrice);
				notify.Debug ("coins before purchase " + GameProfile.SharedInstance.Player.coinCount);
				//GameProfile.SharedInstance.Player.coinCount -= currentPrice;
				GameProfile.SharedInstance.UpdateCoinsPostSession(-currentPrice,true);
				notify.Debug ("coins after gatcha " + GameProfile.SharedInstance.Player.coinCount);
				//localCoins = GameProfile.SharedInstance.Player.coinCount;
				playerBank.text = GameProfile.SharedInstance.Player.coinCount.ToString();
				//totalCoinsLabel.text = GameProfile.SharedInstance.Player.coinCount.ToString(); - HAVE BEEN REQUESTED TO REMOVE THESE N.N.
				
				//AnalyticsInterface.LogInAppCurrencyActionEvent( CostType.Coin, 0 - currentPrice, "gatcha", currentKey.ToString(), 0, "store", GameProfile.GetAreaCharacterString() );
				
				currentPrice *= 2;
				
				ObjectivesDataUpdater.AddToGenericStat(ObjectiveType.PaidDestinyCard,1);
			}
		}
		//canSwitchCard = false; // this is comment out since we only use it for noMoreCards
		gc.isFlipped = true;
		totalCardsFlipped++;
		UpdateLabel();
		if(gc.data.type == GatchaType.EMPTY){
			AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_Poof_activate);
			gc.fxEmpty.Play(true);
		}
		else{
			AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_UI_Menu_levelUp);
			gc.fxOpenBox.Play(true);
		}
		gc.musicBox.spriteName = "musicbox_opened";
		gc.musicBox.transform.localScale = openCardSize;
		FlipCardComplete(gc);
	}
	
	private void CanSwitchCard(){
		canSwitchCard = true;
	}
	
	public void FlipCardComplete(GatchaCard gc){
		//GatchaCard gc = card.GetComponent<GatchaCard>();
		
		// give player the stuff she won.
		if(gc.data != null){
			notify.Debug("FlipCardComplete " + gc.name + " reward " + gc.label.text + " type " + gc.data.type + " showNoMoreCards " + showNoMoreCards);
			switch(gc.data.type){
				case GatchaType.EMPTY:
					if(showNoMoreCards){ // if we need to show the msg
						NoMoreCards(); 
					}
					else{ // keep playing
						//canSwitchCard = true;  //  commented since we want to be able to flip multiple cards
						//Invoke("CanSwitchCard", 0.5f);
					}
					if(totalCardsFlipped == 6){
						Invoke("RemoveGatchaIfDone", 1.5f);
					}
					break;
				case GatchaType.COINS:
					//notify.Debug("These are coins " + gc.data.amount);
					//AnimateCoins(gc); - HAVE BEEN REQUESTED TO REMOVE THESE N.N.
					WobbleScore(gc.label, gc.data.amount, true);
					gc.icon.enabled = true;
					gc.icon.transform.localPosition = gc.label.transform.localPosition + new Vector3(-55f, 0f, -20f);
					iTween.ScaleTo(gc.icon.gameObject, iTween.Hash(
						"scale",  gc.icon.transform.localScale * 4f,
						"time", 0.4f,
						"delay", 0.3f,
						"easetype", iTween.EaseType.easeOutBack,
						"oncomplete", "AnimateIconComplete",
						"oncompletetarget", gameObject
						));
					notify.Debug ("adding coins " + gc.data.amount + " to currently " + GameProfile.SharedInstance.Player.coinCount);
					//GameProfile.SharedInstance.Player.coinCount += gc.data.amount;
					int currentCount = GameProfile.SharedInstance.UpdateCoinsPostSession(gc.data.amount,true);
					if(currentCount>maxCoinScoreThisRun)
						maxCoinScoreThisRun = currentCount;
					playerBank.text = GameProfile.SharedInstance.Player.coinCount.ToString();
					notify.Debug ("new coins count " + GameProfile.SharedInstance.Player.coinCount);
					AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_Apprentice_01);
					break;
				case GatchaType.SCORE_BONUS:
					//notify.Debug("These are SCORE_BONUS " + gc.data.amount);
					WobbleScore(gc.label, gc.data.amount);
					AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_Apprentice_01);
					// add score icon next to numbers/label						
					gc.icon.enabled = true;
					gc.icon.transform.localPosition = gc.label.transform.localPosition + new Vector3(-55f, 0f, -20f);
					iTween.ScaleTo(gc.icon.gameObject, iTween.Hash(
						"scale",  gc.icon.transform.localScale * 4f,
						"time", 0.4f,
						"delay", 0.3f,
						"easetype", iTween.EaseType.easeOutBack
						));
					notify.Debug ("add score " + gc.data.amount + " to current score " + GamePlayer.SharedInstance.Score);
					GamePlayer.SharedInstance.AddScore( gc.data.amount, true);
					notify.Debug (" new score " + GamePlayer.SharedInstance.Score);
					break;
				case GatchaType.HeadStart:
					GameProfile.SharedInstance.Player.EarnConsumable(UIManagerOz.SharedInstance.inGameVC.bonusButtons.HeadStartID, gc.data.amount);	//.BigHeadStartID, gc.data.amount);
					AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_Apprentice_01);
					AnimateIcon(gc);
					break;
				case GatchaType.BigHeadStart:
					GameProfile.SharedInstance.Player.EarnConsumable(UIManagerOz.SharedInstance.inGameVC.bonusButtons.BigHeadStartID, gc.data.amount);
					AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_Apprentice_01);
					AnimateIcon(gc);
					break;
				case GatchaType.StumbleProof:
					GameProfile.SharedInstance.Player.EarnConsumable(UIManagerOz.SharedInstance.inGameVC.bonusButtons.StumbleProofID, gc.data.amount);
					AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_Apprentice_01);
					AnimateIcon(gc);
					break;
				case GatchaType.ThirdEye:
					GameProfile.SharedInstance.Player.EarnConsumable(UIManagerOz.SharedInstance.inGameVC.bonusButtons.ThirdEyeID, gc.data.amount);
					AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_Apprentice_01);
					AnimateIcon(gc);
					break;
			}
			if (cardFromRun) {
				//				AnalyticsInterface.LogGameAction("gatcha","received","free",GameProfile.GetAreaCharacterString(),0);
			}
			//AnalyticsInterface.LogGameAction("gatcha", "achieved", gc.data.type.ToString(), GameProfile.GetAreaCharacterString(), 0);
		}	
	
	
		
	}
	
	private void UpdateLabel(){
		string text = "";
		if(availableCards > 0){
			//text = "You have " + availableCards +" destiny card\nChoose your card";
			string format = "";
			if(availableCards == 1){
				format = Localization.SharedInstance.Get ("Gat_AvailableCard");
			}
			else{
				format = Localization.SharedInstance.Get ("Gat_AvailableCards");
			}
			//text = string.Format(format, availableCards.ToString());
			text = format.Replace("{0}", availableCards.ToString());
		}
		else{
			//text = "Pay " + currentPrice + " to\nChoose your card";
			string format = Localization.SharedInstance.Get("Gat_PayforCard");
			//text = string.Format(format, currentPrice.ToString());
			text = format.Replace("{0}", currentPrice.ToString());
		}
		if(totalCardsFlipped == 6){
			//text = Localization.SharedInstance.Get ("Gat_NextContinue");
			text = "";
			iTween.MoveTo(bannerPanel.gameObject, iTween.Hash(
				"position", Vector3.up * -400f,
				"isLocal", true,
				"time", 0.4f,
				"easetype", iTween.EaseType.easeInCirc
			));
		}
		bannerLabel.text = text;
	}
	
	private void NoMoreCards(){
		notify.Debug("NoMoreCards()");
		showNoMoreCards = false; // and don't show it again
		noCardsDisplayed = true;
		PlayerPrefs.SetInt("noCardsDisplayed", 1);
		PlayerPrefs.Save();
		canSwitchCard = false;
		canSwitchCard2 = false;
		currentPrice = startingPrice;
		UpdateLabel();
		iTween.MoveTo(noCardsPanel.gameObject, iTween.Hash(
			"position", new Vector3(0f, 0f, -20f),
			"isLocal", true,
			"time", 0.3f,
			"easetype", iTween.EaseType.easeInOutCirc
		));
	}
	
	public void OnNoCardsPressed(){
		iTween.MoveTo(noCardsPanel.gameObject, iTween.Hash(
			"position", new Vector3(0f, 2000f, -20f),
			"isLocal", true,
			"time", 0.3f,
			"easetype", iTween.EaseType.easeInCirc
		));
		ShowNextPanel();
		canSwitchCard = true;
		canSwitchCard2 = true;
	}
	
	public void ShowNextPanel(){
		iTween.MoveTo(nextPanel.gameObject, iTween.Hash(
			"y", -463f,
			"isLocal", true,
			"time", 0.3f,
			"easetype", iTween.EaseType.easeOutCubic
		));
	}
	
	public void OnNextPressed(){
		if(!canSwitchCard2) return;
		StartCoroutine(OnNextPressedLoop());
	}
	
	public void OnEscapeButtonClickedModel()
	{
		if( noCardsPanel.gameObject.active == false ) return ;
		
		if( UIManagerOz.escapeHandled ) return;
		UIManagerOz.escapeHandled = true;
		
		OnNoCardsPressed();
	}		
	
	public void OnSkipPressed(){
		OnRemoveGatcha();
	}
	
	public IEnumerator OnNextPressedLoop(){
		canSwitchCard2 = false;
		yield return StartCoroutine( FlipRemainingCards());
		OnRemoveGatcha();
	}
	
	public void OnRemoveGatcha(){
		notify.Debug("OnRemoveGatcha");

		//Update the max coin count during gatcha, simply for "score this run" displays
		GamePlayer.SharedInstance.CoinCountTotal = maxCoinScoreThisRun;
		
	//	GameController.SharedInstance.UpdateChallengeAndEndGame();
		
		visible = false;
		GameProfile.SharedInstance.Serialize();

		NGUITools.SetActive(UIManagerOz.SharedInstance.inGameVC.gameObject, false);
		//UIManagerOz.SharedInstance.inGameVC.disappear();	//true);
		UIManagerOz.SharedInstance.postGameVC.appear();
		
		bool testWeeklyDisplay = Settings.GetBool( "always-display-weekly-postrun", false );
		
		if ( testWeeklyDisplay || ObjectivesDataUpdater.AreAnyWeeklyChallengesCompleted() )
		{
			UIManagerOz.SharedInstance.postGameVC.ShowWeeklyChallengesPage();	
		}
		else
		{
			UIManagerOz.SharedInstance.postGameVC.ShowObjectivesPage();	
		}
		
		//	UIManagerOz.SharedInstance.PaperVC.BringInBottomPanel();

		// clear background with color similar to menu UI background, eliminate flashing between screens
		UIManagerOz.SharedInstance.SetUICameraClearFlagToSolidColorBG(true);		
		
		disappear();
	}
	
	private void PopulateCards(){
		for(int i=0; i<6; i++){
			GatchaCard card = cards[i];
			card.label.text = "";
			card.label.alpha = 1f;
			card.label.transform.parent = card.transform;
			card.label.transform.localScale = new Vector3(1f,1f,1f);
			card.label.transform.localPosition = new Vector3(0f,25f,-100f); // middle of musicbox
			GatchaDataSet gds = GetRandomGatcha();
			if(gds != null){
				card.data = gds;
				switch (gds.type){
					case GatchaType.EMPTY:
						card.label.text = "";
						break;
					case GatchaType.COINS:
						card.label.text = gds.amount.ToString();
						card.icon.spriteName = "currency_coin";
						card.icon.transform.localScale = card.iconOrigScale * 0.5f;
						card.label.transform.localScale = new Vector3(30f,30f,3f);
						break;
					case GatchaType.SCORE_BONUS:
						card.icon.spriteName = "icon_stats_score";
						card.icon.transform.localScale = card.iconOrigScale * 0.5f;
						card.label.text = (gds.amount * GameProfile.SharedInstance.GetScoreMultiplier()).ToString();
						card.data.amount = gds.amount * GameProfile.SharedInstance.GetScoreMultiplier();
						card.label.transform.localScale = new Vector3(30f,30f,3f);
						break;
					case GatchaType.HeadStart:
						card.label.text = GameProfile.SharedInstance.Player.GetConsumableLocalizeString(UIManagerOz.SharedInstance.inGameVC.bonusButtons.HeadStartID);
						card.icon.spriteName = UIManagerOz.SharedInstance.inGameVC.bonusButtons.HeadStartButton.GetComponent<UIImageButton>().normalSprite;
						break;
					case GatchaType.BigHeadStart:
						card.label.text = GameProfile.SharedInstance.Player.GetConsumableLocalizeString(UIManagerOz.SharedInstance.inGameVC.bonusButtons.BigHeadStartID);
						card.icon.spriteName = UIManagerOz.SharedInstance.inGameVC.bonusButtons.BigHeadStartButton.GetComponent<UIImageButton>().normalSprite;
						break;
					case GatchaType.ThirdEye:
						card.label.text = GameProfile.SharedInstance.Player.GetConsumableLocalizeString(UIManagerOz.SharedInstance.inGameVC.bonusButtons.ThirdEyeID);
						card.icon.spriteName = UIManagerOz.SharedInstance.inGameVC.bonusButtons.ThirdEyeButton.GetComponent<UIImageButton>().normalSprite;
						break;
					case GatchaType.StumbleProof:
						card.label.text = GameProfile.SharedInstance.Player.GetConsumableLocalizeString(UIManagerOz.SharedInstance.inGameVC.bonusButtons.StumbleProofID);;
						card.icon.spriteName = UIManagerOz.SharedInstance.inGameVC.bonusButtons.StumbleProofButton.GetComponent<UIImageButton>().normalSprite;
						break;
				}
		
				card.icon.transform.localScale *= 0.2f;
				card.icon.transform.localPosition = new Vector3(0f,30f,-200f);
			}
			else{
				card.label.text = "";
				notify.Debug("card is Empty");
			}
		}
	}

	
	private GatchaDataSet GetRandomGatcha(){
		int totalWeight = 0;
		foreach(GatchaDataSet gds in data.gatchaList){
			if(gds.active){
				totalWeight += gds.randomWeight;
			}
		}
		int rand = (int)(Random.value * (float)totalWeight);
		notify.Debug(totalWeight + " rand " + rand);
		
		
		GatchaDataSet g = new GatchaDataSet();
		g.type = GatchaType.EMPTY;
		g.amount = 0;
		
		
		foreach(GatchaDataSet gds in data.gatchaList){
			if(gds.active){
				rand -= gds.randomWeight;
				if(rand < 0){
					notify.Debug(" the winner is " + gds.type + " amount " + gds.amount);
					g.type = gds.type;
					g.amount = gds.amount;
					return g;
				}
			}
		}

		return g;
		
		
	}
	
	//- HAVE BEEN REQUESTED TO REMOVE THESE N.N.
//	private void AnimateCoins(GatchaCard card){
//		//StartCoroutine(EnableSwitchCardOnTimer(2f));
//		coinsAnimatedCount = 0;
//		Vector3 wp = totalCoinsIcon.transform.position;
//		wp -= Vector3.forward * 0.1f;
//		foreach(Transform coin in coins){
//			coin.renderer.enabled = false;
//			coin.position = card.transform.position - Vector3.forward * 0.01f;
//			float time = 0.3f;
//			iTween.MoveTo(coin.gameObject, iTween.Hash(
//				"position", wp,
//				"time", time,
//				"delay", coinsAnimatedCount * 0.15f ,
//				//"easetype", iTween.EaseType.easeInCubic,
//				"onstart", "AnimateCoinsStart",
//				"onstarttarget", gameObject,
//				"onstartparams", coin,
//				"oncomplete", "AnimateCoinsComplete",
//				"oncompletetarget", gameObject,
//				"oncompleteparams", coin
//				));
//			
//			coinsAnimatedCount++;
//		}
//		
//		iTween.ValueTo(totalCoinsLabel.gameObject, iTween.Hash(
//			"name", "coinsUpdate",
//			"from", 0f,
//			"to", 1f,
//			"time", 1.6f,
//			"onupdate", "onAnimateCoinsUpdate",
//			"onupdatetarget", gameObject
//			));
//	
//		card.icon.enabled = true;
//		card.icon.transform.localPosition = card.label.transform.localPosition + new Vector3(-55f, 0f, -20f);
//		iTween.ScaleTo(card.icon.gameObject, iTween.Hash(
//			"scale",  card.icon.transform.localScale * 4f,
//			"time", 0.4f,
//			"delay", 0.3f,
//			"easetype", iTween.EaseType.easeOutBack,
//			"oncomplete", "AnimateIconComplete",
//			"oncompletetarget", gameObject
//			));
//		
//	}
	public void onAnimateCoinsUpdate(float val){
		//notify.Debug(val + " " + localCoins + " " + GameProfile.SharedInstance.Player.coinCount);
		//int diff = GameProfile.SharedInstance.Player.coinCount - localCoins;
		//totalCoinsLabel.text = ((int)(localCoins +  diff * val)).ToString(); - HAVE BEEN REQUESTED TO REMOVE THESE N.N.
		if(val >= 1){
			//localCoins = GameProfile.SharedInstance.Player.coinCount;
			//totalCoinsLabel.text = localCoins.ToString(); - HAVE BEEN REQUESTED TO REMOVE THESE N.N.
		}
	}
	public void AnimateCoinsStart(Transform coin){
		//coin.renderer.enabled = true;
	}	
	public void AnimateCoinsComplete(Transform coin){
		coin.renderer.enabled = false;
	}

	private void AnimateCoinIcon(GatchaCard card){
		
		//card.label.transform.parent = card.transform.parent;
		card.label.enabled = true;
		card.icon.enabled = true;
		//Hashtable param = new Hashtable();
		//param.Add("card", card);
		//param.Add("label", card.label);
		iTween.ScaleTo(card.label.gameObject, iTween.Hash(
			"scale",  Vector3.one * 64f,
			"time", 0.5f,
			"easetype", iTween.EaseType.easeOutBack
			));
		/*
		iTween.MoveTo(card.label.gameObject, iTween.Hash(
			"position", new Vector3(0f,85f,card.label.transform.localPosition.z - 12f),
			"islocal", true,
			"time", 0.5f,
			"oncomplete", "OnAnimateLabelComplete",
			"oncompletetarget", gameObject,
			"oncompleteparams", card
			));
		*/	
		
		iTween.ScaleTo(card.icon.gameObject, iTween.Hash(
			"scale",  card.icon.transform.localScale * 4f,
			"time", 0.4f,
			"delay", 0.3f,
			"easetype", iTween.EaseType.easeOutBack,
			"oncomplete", "AnimateIconComplete",
			"oncompletetarget", gameObject
			));
	}
	
	
	private void WobbleScore(UILabel score, int points, bool isCoins = false){
		
		iTween.StopByName("scoreScale");
		iTween.StopByName("scoreMove");
		iTween.StopByName("scoreMove2");
		iTween.StopByName("scoreUpdate");
	
		score.enabled = true;
		
		scoreLabel.transform.position = score.transform.position ;
		scoreLabel.transform.localPosition -= Vector3.forward * 3f;
		scoreLabel.transform.localScale = score.transform.localScale;
		scoreLabel.text = score.text;
		if(isCoins){
			scoreLabel.text += " " + Localization.SharedInstance.Get ("Ttl_Sub_Coins");
		}
		scoreLabel.alpha = 1f;
		
		iTween.ScaleTo(scoreLabel.gameObject, iTween.Hash(
			"name", "scoreScale",
			"scale",  scoreLabel.transform.localScale * 3f,
			"time", 0.3f,
			"easetype", iTween.EaseType.easeOutBack
			));
		
		
		
		iTween.MoveTo(scoreLabel.gameObject, iTween.Hash(
			"name", "scoreMove",
			"position",   new Vector3(0f,85f,scoreLabel.transform.localPosition.z - 12f),
			"islocal", true,
			"time", 0.3f,
			"easetype", iTween.EaseType.easeOutBack,
			"oncomplete", "AnimateScore",
			"oncompletetarget", gameObject,
			"oncompleteparams", isCoins
			));	
		

	}

	private void AnimateScore(bool isCoins){	
		/*
		Vector3 wp = totalScoreIcon.transform.position;
		wp -= Vector3.forward * 0.1f;
		if(isCoins){
			wp = scoreLabel.transform.position + Vector3.up * 0.1f;
		}
		iTween.MoveTo(scoreLabel.gameObject, iTween.Hash(
			"name", "scoreMove2",
			"position", wp,
			"time", 0.6f,
			"easetype", iTween.EaseType.easeInCubic,
			"oncomplete", "onAnimateScoreComplete",
			"oncompletetarget", gameObject
			));
		
		*/
		iTween.ValueTo(scoreLabel.gameObject, iTween.Hash(
			"name", "scoreUpdate",
			"from", 0f,
			"to", 1f,
			"time", 0.6f,
			"onupdate", "onAnimateScoreUpdate",
			"onupdatetarget", gameObject,
			"oncomplete", "onAnimateScoreComplete",
			"oncompletetarget", gameObject
			));
		
	}
	
	public void onAnimateScoreUpdate(float val){
		scoreLabel.alpha = 1f - val;
		//int diff = GamePlayer.SharedInstance.Score - localScore;
		//totalScoreLabel.text = ((int)(localScore +  diff * val)).ToString(); - HAVE BEEN REQUESTED TO REMOVE THESE N.N.
	}
	
	public void onAnimateScoreComplete(){
		//localScore = GamePlayer.SharedInstance.Score;
		if(showNoMoreCards){ // if we need to show the msg
			NoMoreCards(); 
		}
		else{ // keep playing
			//canSwitchCard = true;
		}
		if(totalCardsFlipped == 6){
			Invoke("RemoveGatchaIfDone", 1f);
		}
	}
	
	private void AnimateIcon(GatchaCard card){
		
		card.label.transform.parent = card.transform.parent;
		card.label.enabled = true;
		card.icon.enabled = true;
		//Hashtable param = new Hashtable();
		//param.Add("card", card);
		//param.Add("label", card.label);
		iTween.ScaleTo(card.label.gameObject, iTween.Hash(
			"scale",  Vector3.one * 64f,
			"time", 0.5f,
			"easetype", iTween.EaseType.easeOutBack
			));
		iTween.MoveTo(card.label.gameObject, iTween.Hash(
			"position", new Vector3(0f,85f,card.label.transform.localPosition.z - 12f),
			"islocal", true,
			"time", 0.5f,
			"oncomplete", "OnAnimateLabelComplete",
			"oncompletetarget", gameObject,
			"oncompleteparams", card
			));
			
		
		iTween.ScaleTo(card.icon.gameObject, iTween.Hash(
			"scale",  card.icon.transform.localScale * 4f,
			"time", 0.4f,
			"delay", 0.3f,
			"easetype", iTween.EaseType.easeOutBack,
			"oncomplete", "AnimateIconComplete",
			"oncompletetarget", gameObject
			));
	}
	
	public void OnAnimateLabelComplete(GatchaCard card){
		TweenAlpha.Begin(card.label.gameObject, 0.5f,0f);
	}
	
	public void AnimateIconComplete(){
		notify.Debug("AnimateIconComplete " + showNoMoreCards);
		if(showNoMoreCards){ // if we need to show the msg
			NoMoreCards(); 
		}
		else{ // keep playing
			//canSwitchCard = true;
		}
		if(totalCardsFlipped == 6){
			Invoke("RemoveGatchaIfDone", 1f);
		}
	}
	
	private IEnumerator FlipRemainingCards(){
		int i = 0;
		//AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_Gatcha_PickupDeck);
		bool delayGame = false;
		foreach(GatchaCard gc in cards){
			if(!gc.isFlipped){
				delayGame = true;
				yield return new WaitForSeconds(0.2f);
				
				gc.musicBox.spriteName = "musicbox_opened";
				gc.musicBox.transform.localScale = openCardSize;
				AudioManager.SharedInstance.PlayFX(AudioManager.Effects.oz_UI_Menu_back);	
				if(gc.data.type != GatchaType.EMPTY){
					gc.label.enabled = true;
					iTween.ScaleTo(gc.label.gameObject, iTween.Hash(
						"scale",  Vector3.one * 40f,
						"time", 0.5f,
						"easetype", iTween.EaseType.easeOutBack
						));
						
					if(gc.data.type == GatchaType.COINS || gc.data.type == GatchaType.SCORE_BONUS){
						gc.icon.transform.localPosition = gc.label.transform.localPosition + new Vector3(-55f, 0f, -20f);
					}
					gc.icon.enabled = true;
					iTween.ScaleTo(gc.icon.gameObject, iTween.Hash(
						"scale",  gc.icon.transform.localScale * 4f,
						"time", 0.4f,
						"delay", 0.3f,
						"easetype", iTween.EaseType.easeOutBack
						));	
				}
				i++;
			}
		}
		if(delayGame){
			yield return new WaitForSeconds(2f);
		}
	}
	
	private void RemoveGatchaIfDone(){
		OnRemoveGatcha();
	}

	
	IEnumerator EnableSwitchCardOnTimer(float time){
		yield return new WaitForSeconds(time);
		if(showNoMoreCards){ // if we need to show the msg
			NoMoreCards(); 
		}
		else{ // keep playing
			//canSwitchCard = true;
		}
	}
	
	
	public void OnNeedMoreCoinsNoInGame() 	// use in-game only, goes to mini store.  Used on gatcha screen.
	{
		//UIConfirmDialogOz.ClearEventHandlers();
		UIConfirmDialogOz.onNegativeResponse -= OnNeedMoreCoinsNoInGame;
		UIConfirmDialogOz.onPositiveResponse -= OnNeedMoreCoinsYesInGame;
	}
	
	public void OnNeedMoreCoinsYesInGame()  // use in-game only, goes to mini store.  Used on gatcha screen.
	{
		//UIConfirmDialogOz.ClearEventHandlers();
		UIConfirmDialogOz.onNegativeResponse -= OnNeedMoreCoinsNoInGame;
		UIConfirmDialogOz.onPositiveResponse -= OnNeedMoreCoinsYesInGame;
		
		UIManagerOz.SharedInstance.GoToMiniStore(ShopScreenName.Coins, false);	//"coins");	// send player to in-game mini store, coins page
	}		
	
		
//	public override void disappear(){	//(bool hidePaper = true){
//		notify.Debug("disappear");
		//GameProfile.SharedInstance.Serialize();
//		visible = false;
//		base.disappear();
		//gameObject.SetActiveRecursively(false);
		// now lets continue to objectives
		//if(hidePaper){
		//	UIManagerOz.SharedInstance.inGameVC.disappear();	//true);
		//	UIManagerOz.SharedInstance.postGameVC.appear();
		//	UIManagerOz.SharedInstance.PaperVC.BringInBottomPanel();
		//}
//	}
	
	
	/*
	void Update(){
		if(UIManagerOz.SharedInstance) return;
		if (Input.GetKeyDown(KeyCode.G)) {
			if(visible){
				OnNextPressed();
			}
			else{
				appear();
			}
		}
	}
	*/
	
}


