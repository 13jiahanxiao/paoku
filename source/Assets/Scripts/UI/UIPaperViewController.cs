using UnityEngine;
using System.Collections;

public class UIPaperViewController : MonoBehaviour
{
	//-- TODO Support the special currency.	
	
	
	public UILabel TitleLabel = null;
	public UILabel CurrencyLabel = null;
	public UISprite CurrencyIcon = null;
	public UILabel SpecialCurrencyLabel = null;
	public UISprite SpecialCurrencyIcon = null;
	public Transform PlayButtonRoot = null;
	public Transform BackButtonRoot = null;
	
	public void SetTitle(string titleText) {
		if(TitleLabel == null)
			return;
		
		TitleLabel.text = titleText;
	}
	
	public void SetCurrencyLabel(string currencyText, bool show = true, CostType costType = CostType.Coin) {
		UILabel label = null;
		
		if(costType == CostType.Coin) {
			label = CurrencyLabel;
			if(CurrencyLabel == null)
				return;
			if(show) {
				CurrencyLabel.text = currencyText;
				CurrencyLabel.enabled = true;
			} else {
				CurrencyLabel.enabled = false;
			}
		}
		else if(costType == CostType.Special) {
			label = SpecialCurrencyLabel;
		}
		
		if(label == null)
			return;
		if(show) {
			label.text = currencyText;
			label.enabled = true;
		} else {
			label.enabled = false;
		}
		
		//respositionCurrencyWidgets();
	}
	
	public void ShowPlayButton(bool show = true) {
		if(PlayButtonRoot == null || PlayButtonRoot.gameObject == null)
			return;
		
		PlayButtonRoot.gameObject.SetActiveRecursively(show);
	}
	
	public void ShowBackButton(bool show = true) {
		if(BackButtonRoot == null || BackButtonRoot.gameObject == null)
			return;
		
		BackButtonRoot.gameObject.SetActiveRecursively(show);
	}
	
	public void SetPlayButtonCallback(GameObject target, string functionName) {
		if(BackButtonRoot == null)
			return;
		
		UIButtonMessage message = PlayButtonRoot.GetComponent<UIButtonMessage>() as UIButtonMessage;
		if(message == null)
			return;
		message.target = target;
		message.functionName = functionName;
	}
	
	public void SetBackButtonCallback(GameObject target, string functionName) {
		if(BackButtonRoot == null)
			return;
		
		UIButtonMessage message = BackButtonRoot.GetComponent<UIButtonMessage>() as UIButtonMessage;
		if(message == null)
			return;
		message.target = target;
		message.functionName = functionName;
	}
	
	void respositionCurrencyWidgets()
	{
		//-- These controls are children of a TopRight Anchor, so we lay out going negative X, starting at 0.
		float currencySpacer = SpecialCurrencyIcon.transform.localScale.x*0.25f;
		float currentX = 0;
		if(SpecialCurrencyIcon.enabled) {
			SpecialCurrencyIcon.transform.localPosition = new Vector3(currentX, 0, 0);
			currentX -= SpecialCurrencyIcon.transform.localScale.x;
			
			SpecialCurrencyLabel.transform.localPosition = new Vector3(currentX, SpecialCurrencyLabel.transform.localPosition.y, SpecialCurrencyLabel.transform.localPosition.z);
			currentX -= (SpecialCurrencyLabel.relativeSize.x*SpecialCurrencyLabel.transform.localScale.x);
			currentX -= currencySpacer;
		}
		
		if(CurrencyIcon.enabled) {
			CurrencyIcon.transform.localPosition = new Vector3(currentX, 0, 0);
			currentX -= CurrencyIcon.transform.localScale.x;
			CurrencyLabel.transform.localPosition = new Vector3(currentX, CurrencyLabel.transform.localPosition.y, CurrencyLabel.transform.localPosition.z);
		}
	}
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
//	void Update ()
//	{
//		//-- Reposition the currency labels.
//	}
}

