using UnityEngine;
using System.Collections;

public class DirectSwipeDetector : MonoBehaviour {

	public TouchInput IManager;

	// Use this for initialization
	void Start () {
	
	}
	
	bool isPressed = false;
	
	void Update()
	{

		if (Input.touchCount > 0) {

			Touch t = Input.GetTouch(0);
			switch (t.phase) {
			case TouchPhase.Began:
				IManager.TouchBegan(t.position);
				//CharacterPlayer.Instance.debugColor = new Color(1,0,0,1);
				break;
			case TouchPhase.Moved:
				IManager.TouchMoved(t.position);
				//CharacterPlayer.Instance.debugColor = new Color(0,0,1,1);
				break;
			case TouchPhase.Ended:
				IManager.TouchEnded(t.position);
				//CharacterPlayer.Instance.debugColor = new Color(0,1,0,1);
				break;
			case TouchPhase.Canceled:
				IManager.TouchEnded(t.position);
				//CharacterPlayer.Instance.debugColor = new Color(1,1,1,1);
				break;
			}
		}

		if (Input.GetMouseButtonDown(0) == true) {
			IManager.TouchBegan(Input.mousePosition);
			isPressed = true;
		}
		else
		if (Input.GetMouseButtonUp(0) == true) {
			IManager.TouchEnded(Input.mousePosition);
			isPressed = false;
		}
		else
		if (isPressed) {
			IManager.TouchMoved(Input.mousePosition);
		}


	}

}
