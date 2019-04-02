using UnityEngine;
using System.Collections;

public class animTest : MonoBehaviour {

	public int actionId=0;
	private string[] actionNameArr;
	void Start () {
		actionNameArr=new string[27];
		actionNameArr[0]="Run01";
		actionNameArr[1]="Jump01";
		actionNameArr[2]="RunStumble01";
		actionNameArr[3]="SlideEnter01";
		actionNameArr[4]="Slide01";


		actionNameArr[5]="CarriedOff01";
		actionNameArr[6]="LeftTurnSlide01";
		actionNameArr[7]="RightTurnSlide01";
		actionNameArr[8]="Fall01";
		actionNameArr[9]="WallFail01";


		actionNameArr[10]="SlideEnter02";
		actionNameArr[11]="Slide02";
		actionNameArr[12]="Jump02";
		actionNameArr[13]="EnterBalloon01";
		actionNameArr[14]="IdleBalloon01";


		actionNameArr[15]="StumbleBalloon01";
		actionNameArr[16]="SlideBalloon01";
		actionNameArr[17]="PanicBalloon01";
		actionNameArr[18]="BalloonExitFail01_needsAudio";
		actionNameArr[19]="Grab01";


		actionNameArr[20]="Flying01";
		actionNameArr[21]="Release01";
		actionNameArr[22]="Float";
		actionNameArr[23]="FloatFall";
		actionNameArr[24]="LeanRightIdle";

		actionNameArr[25]="LeanLeftIdle";

		actionNameArr[26]="BubbleJump";
	 
 
	 
 
		 
	 
 
	 
	 
	 
 
	 	/*
		AddToAnimTypeMap(AnimType.kWallFail,"WallFail01");
	  	AddToAnimTypeMap(AnimType.kBalloonExit,"ExitBalloon01");
 
	   	AddToAnimTypeMap(AnimType.kLeftLedge,"LeanLeftLedge");
		AddToAnimTypeMap(AnimType.kRightLedge,"LeanRightLedge");

		 */
	 

	 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI() {
		if (GUI.Button(new Rect(10, 10, 200, 200), "play action")){
		 
			animation.CrossFade (actionNameArr[actionId]);
		}
			 
		
	}
}
