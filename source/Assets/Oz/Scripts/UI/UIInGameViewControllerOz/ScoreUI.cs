using UnityEngine;
using System;
using System.Collections;

public class ScoreUI : MonoBehaviour 
{
	public UILabel ScoreLabel;
	public UILabel CoinLabel;
	public UILabel GemLabel;	
	
	public Transform scoreBar;
	public Transform coinBar;
	public Transform gemBar;
	public Transform gemRoot;
	
	public Transform coinSprite;
	public Transform gemsprite;	
	
	private float lastAnimCoinVal;
	private float lastAnimCoinTime;
	
	public ParticleSystem scoreBonusElectric;
	public ParticleSystem scoreBonusSpark;
	public ParticleSystem scoreBonusSmoke;		
	
	IEnumerator Start() {
		//I know this looks crazy... but we had an issue that where the score and coins labels would flicker
		// every other time they were updated, but ONLY while they were at 8 digits. Our fix was to first set the
		// text to a nine-digit number and then wait a frame, because after that, it doesnt happen.
		
		//No, really.
		
		Vector3 start = transform.localPosition;
		int test = 101010101;
		ScoreLabel.text = test.ToString();
		CoinLabel.text = test.ToString();
		GemLabel.text = test.ToString();
		
		transform.localPosition = start + Vector3.right * 500f;
		
		yield return null;
		
		test = 0;
		ScoreLabel.text = test.ToString();
		CoinLabel.text = test.ToString();
		GemLabel.text = test.ToString();
		
		transform.localPosition = start;
		
		//Debug.Log("Complete!");
	}
	
	//void Update() { 
		//SetCoinCount(GamePlayer.SharedInstance.CoinCountTotal);
		//SetGemCount(GamePlayer.SharedInstance.GemCountTotal);
	//}
		
	private int _lastScore = 0;
	private int _score = 0;
	public void SetScore(int score) 
	{
		_score = score;
	}
	
	private bool UpdateScore()
	{
		if(_score != _lastScore)
		{
			ScoreLabel.text = _score.ToString();
			if((_score == 0)||((int)Math.Floor(Math.Log10(_score)) != (int)Math.Floor(Math.Log10(_lastScore))))
			{
				RepositionBar(scoreBar, null, _score, -30);
			}
			_lastScore = _score;
			return true;
		}
		return false;
		//ScoreLabel.transform.localPosition = new Vector3(ScoreLabel.transform.localPosition.x, ScoreLabel.transform.localPosition.y, -5.0f);
		
	}
	
	private int _lastCoins = 0;
	private int _coins = 0;
	public void SetCoinCount(int coins) 
	{
		_coins = coins;
	}
	
	private bool UpdateCoins()
	{
		if(_coins != _lastCoins)
		{
			CoinLabel.text = _coins.ToString();
			if((_coins == 0)||((int)Math.Floor(Math.Log10(_coins)) != (int)Math.Floor(Math.Log10(_lastCoins))))
			{
				RepositionBar(coinBar, coinSprite, _coins, 10);
			}
			_lastCoins = _coins;
			return true;
		}
		return false;
		//CoinLabel.transform.localPosition = new Vector3(CoinLabel.transform.localPosition.x, CoinLabel.transform.localPosition.y, -3.0f);
	}
	
	public void Update()
	{
		switch(Time.frameCount%4)
		{
		case 0:
			UpdateScore();
			break;
		case 2:
			UpdateCoins();
			break;
		default:
			break;
		}		
	}
	
	public void SetGemCount(int gems) 
	{
		GemLabel.text = gems.ToString();
		int digits = 0;
		if (gems == 0) { digits = 1; }
		else { digits = (int)Math.Floor(Math.Log10(gems) + 1); }
		Vector3 pos = new Vector3(-90f + digits * 30f, gemRoot.localPosition.y, -1.0f);
		TweenPosition tp = TweenPosition.Begin(gemRoot.gameObject,0.5f, pos);
		tp.onFinished += OnShowGem;
	}
	public void OnShowGem(UITweener tween){
		tween.onFinished -= OnShowGem;
		Vector3 pos = new Vector3(gemRoot.localPosition.x + 0.01f, gemRoot.localPosition.y, -1.0f);
		TweenPosition tp = TweenPosition.Begin(gemRoot.gameObject,2f, pos);
		tp.onFinished += OnShowGemFinished;
		
	}
	public void OnShowGemFinished(UITweener tween){
		tween.onFinished -= OnShowGemFinished;
		Vector3 pos = new Vector3(-164, gemRoot.localPosition.y, -1.0f);
		TweenPosition.Begin(gemRoot.gameObject,0.5f, pos);
	}

	private void RepositionBar(Transform bar, Transform sprite, int currencyVal, float offset = 0f)	
	{		
		int digits = 0;
		
		if (currencyVal == 0) { digits = 1; }
		else { digits = (int)Math.Floor(Math.Log10(currencyVal) + 1); }
		
		bar.localPosition = new Vector3(
			//-0.26f + (0.035f * (float)(digits - 1)),
			-620f + offset +  (30f * (float)(digits - 1)),
			bar.transform.localPosition.y,
			bar.transform.localPosition.z);
		
		if (sprite != null)
		{
			sprite.localPosition = new Vector3(
				//0.04f + (0.035f * (float)(digits - 1)),
				30f + (30f * (float)(digits - 1)),
				sprite.transform.localPosition.y,
				sprite.transform.localPosition.z);	
		}
	}

	public void ResetCurrencyBars()	
	{
		SetScore(0);
		SetCoinCount(0);
		//SetGemCount(0);
	}
	
	public void ScoreBonusEffects()
	{
		if(scoreBonusElectric) scoreBonusElectric.Play(true);		
		if(scoreBonusSpark) scoreBonusSpark.Play(true);		
		if(scoreBonusSmoke) scoreBonusSmoke.Play(true);			
	}	
}




		//double log = Math.Log10(currencyVal);
		//double logplus = log + 1;
		//double dig = Math.Floor(logplus);
		//int digits = (int)dig;
	
//	private float lastScoreXOffset = 0.0f;
//	private float lastCoinXOffset = 0.0f;		
	

//	
//	public void resetCurrencyBars()	
//	{
//		lastScoreXOffset = 0.0f;
//	}	
//	
//	private void respositionCurrencyBars()
//	{
//		float x = ScoreLabel.relativeSize.x * ScoreLabel.transform.localScale.x * 0.5f;
//		
//		if (x > lastScoreXOffset) 
//		{
//			scoreBar.transform.localPosition = new Vector3(350.0f-x, scoreBar.transform.localPosition.y, scoreBar.transform.localPosition.z);	
//			lastScoreXOffset = x;
//		}
//		
//		x = CoinLabel.relativeSize.x * CoinLabel.transform.localScale.x * 0.5f;
//		
//		if ( x > lastCoinXOffset) 
//		{
//			coinBar.transform.localPosition = new Vector3(175.0f-x, coinBar.transform.localPosition.y, coinBar.transform.localPosition.z);	
//			lastCoinXOffset = x;
//		}
//	}	
//}
