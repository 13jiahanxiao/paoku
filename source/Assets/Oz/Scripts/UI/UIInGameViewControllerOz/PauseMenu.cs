using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour 
{
	//public UIInGameViewControllerOz inGameVC;	
	public GameObject UnPauseButton;
	public PauseObjectives pauseObjectives;
	public GameObject hideButton;
	public GameObject showButton;	
	public GameObject homeButton;
	public GameObject screenshotButton;	
	
	public bool ShowHomeButton { get; set; }	//Disabled when in transition tunnel
	
	public void ShowPauseMenu()		// show whole pause menu
	{
		if (GamePlayer.SharedInstance.Dying)
			return;
		
		NGUITools.SetActive(UnPauseButton, true);
		NGUITools.SetActive(pauseObjectives.gameObject, true);
		pauseObjectives.FillInObjectiveData();
		NGUITools.SetActive(hideButton, true);	
		NGUITools.SetActive(showButton, false);
		if (ShowHomeButton) NGUITools.SetActive(homeButton, true);
		NGUITools.SetActive(screenshotButton, true);
	}
	
	public void HidePauseMenu()		// hide whole pause menu
	{
		NGUITools.SetActive(UnPauseButton, false);
		NGUITools.SetActive(pauseObjectives.gameObject, false);
		NGUITools.SetActive(hideButton, false);	
		NGUITools.SetActive(showButton, false);	
		NGUITools.SetActive(homeButton, false);	
		NGUITools.SetActive(screenshotButton, false);			
	}	
	
	public void HideObjectives()	// hide only objectives pane
	{
		NGUITools.SetActive(pauseObjectives.gameObject, false);	
		NGUITools.SetActive(hideButton, false);
		NGUITools.SetActive(showButton, true);	
		if(ShowHomeButton) NGUITools.SetActive(homeButton, true);	
		NGUITools.SetActive(screenshotButton, true);	
	}
	
	public void ShowObjectives() 	// unhide only objectives pane
	{
		NGUITools.SetActive(pauseObjectives.gameObject, true);	
		NGUITools.SetActive(hideButton, true);
		NGUITools.SetActive(showButton, false);	
		if(ShowHomeButton) NGUITools.SetActive(homeButton, true);		
		NGUITools.SetActive(screenshotButton, true);		
	}		
	
//	public void OnMenuButton()
//	{
//		// pop up confirmation dialog, saying that going back to the menu will exit the current game
//		// if yes, exit the game and go to main menu
//	}	
}
