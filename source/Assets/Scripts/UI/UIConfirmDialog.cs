using UnityEngine;
using System.Collections;

public class UIConfirmDialog : MonoBehaviour
{
	public delegate void voidClickedHandler();
	public static event voidClickedHandler	onNegativeResponse = null;
	public static event voidClickedHandler	onPositiveResponse = null;
	
	
	public Transform LeftButton = null;
	public Transform RightButton = null;
	public Transform LeftButtonText = null;
	public Transform RightButtonText = null;
	public Transform DescriptionText = null;
	public Transform TitleText = null;
	public Transform BackgroundSprite = null;
	public Transform AlphaBackground = null;
	
	
	public UILabel rewardLabel = null;
	public UILabel levelLabel = null;
	public UISprite itemIcon = null;
	
	private Vector3 rightButtonCachedPosition = new Vector3(160, -76, 0);
	
	void Awake() {
		if(RightButton != null) {
			rightButtonCachedPosition = RightButton.transform.localPosition;
		}
	}
	void Start() {
		if(AlphaBackground != null) {
			UITexture  sprite = AlphaBackground.GetComponent<UITexture>();
			if(sprite != null) {
			}
		}
	}
	
	public void ShowConfirmDialog(string title, string description, string negativeButtonText, string positiveButtonText) {
		if(LeftButtonText) {
			UILabel label = LeftButtonText.GetComponent<UILabel>() as UILabel;
			if(label) {
				label.text = negativeButtonText;
			}
		}
		
		if(RightButton != null) {
			RightButton.transform.localPosition = rightButtonCachedPosition;
		}
		
		if(RightButtonText) {
			UILabel label = RightButtonText.GetComponent<UILabel>() as UILabel;
			if(label) {
				label.text = positiveButtonText;
			}
		}
		if(DescriptionText) {
			UILabel label = DescriptionText.GetComponent<UILabel>() as UILabel;
			if(label) {
				label.text = description;
			}
		}
		if(TitleText) {
			UILabel label = TitleText.GetComponent<UILabel>() as UILabel;
			if(label) {
				label.text = title;
			}
		}
		NGUITools.SetActive(this.gameObject, true);
	}
	
	public void ShowInfoDialog(string title, string description, string positiveButtonText) {
		NGUITools.SetActive(this.gameObject, true);
		NGUITools.SetActive(LeftButton.gameObject, false);
		
		if(RightButton != null) {
			RightButton.transform.position = new Vector3(0, RightButton.transform.position.y, RightButton.transform.position.z);
		}
		
		if(RightButtonText) {
			UILabel label = RightButtonText.GetComponent<UILabel>() as UILabel;
			if(label) {
				label.text = positiveButtonText;
			}
		}
		if(DescriptionText) {
			UILabel label = DescriptionText.GetComponent<UILabel>() as UILabel;
			if(label) {
				label.text = description;
			}
		}
		if(TitleText) {
			UILabel label = TitleText.GetComponent<UILabel>() as UILabel;
			if(label) {
				label.text = title;
			}
		}
		
	}
	
	public void ShowRewardDialog(string rewardText, string levelText, string itemIconName) {
		NGUITools.SetActive(this.gameObject, true);
		
		if(RightButton != null) {
			RightButton.transform.position = new Vector3(0, RightButton.transform.position.y, RightButton.transform.position.z);
		}
		
		if(rewardLabel != null && rewardText!= null) {
			rewardLabel.text = rewardText;
		}
		
		if(levelLabel != null && levelText!= null) {
			levelLabel.text = levelText;
		}
		if(itemIcon != null && itemIconName!= null) {
			itemIcon.spriteName = itemIconName;
			itemIcon.MakePixelPerfect();
		}
	}
	
	public void OnLeftButtonPress() {
		if(onNegativeResponse != null)
		{
			onNegativeResponse();
		}
		NGUITools.SetActive(this.gameObject, false);
	}
	public void OnRightButtonPress() {
		if(onPositiveResponse != null)
		{
			onPositiveResponse();
		}
		NGUITools.SetActive(this.gameObject, false);
	}
}

