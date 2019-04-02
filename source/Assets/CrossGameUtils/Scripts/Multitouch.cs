using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Multitouch : MonoBehaviour {

	public enum TouchState { Unclaimed, Claimed, Locked }
	
	[System.Serializable]	//Serializable for debug purposes
	public class TouchTracker
	{
		public TouchState State = TouchState.Unclaimed;
		public int TouchIndex = 0;
		
		public TouchTracker(int ind)
		{
			TouchIndex = ind;
		}
		
		public Vector2 Position
		{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
			get { return Input.mousePosition; }
#else
			get { return TouchIndex<Input.touches.Length ? Input.touches[TouchIndex].position : Vector2.zero; }
#endif
		}
		public Vector2 Delta
		{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
			get { return (Vector2)Input.mousePosition - Multitouch.main.LastMousePos; }
#else
			get { return TouchIndex<Input.touches.Length ? Input.touches[TouchIndex].deltaPosition : Vector2.zero; }
#endif
		}
	};
	
	static Multitouch()
	{
		if(_main==null && Application.isPlaying) {
			GameObject go = new GameObject("Multitouch_Singleton");
			_main = go.AddComponent<Multitouch>();
		}
	}
	
	
	
	private static Multitouch _main;
	public static Multitouch main
	{
		get {
			if(_main==null && Application.isPlaying) {
				GameObject go = new GameObject("Multitouch_Singleton");
				_main = go.AddComponent<Multitouch>();
			}
			return _main;
		}
	}
	
	
	
	private List<TouchTracker> touches = new List<TouchTracker>();
	/*public List<TouchTracker> Touches
	{
		get { return new List<TouchTracker>(touches); }
	}*/
		
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
	private Vector2 lastMousePos = Vector2.zero;
	public Vector2 LastMousePos
	{
		get { return lastMousePos; }
	}
#endif
	
	
	protected virtual void Awake()
	{
		_main = this;
		
		touches = new List<TouchTracker>();
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		if(Input.GetMouseButtonDown(0))
			touches.Add(new TouchTracker(0));
#else
		for(int i=0;i<Input.touches.Length;i++)
		{
			touches.Add(new TouchTracker(i));
		}
#endif
	}
	
	protected virtual void Update()
	{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		Mouse_HandleInput();
#else
		Multitouch_HandleInput();
#endif
	}
	
	
	
	void Mouse_HandleInput()
	{
		//Clear the TouchStates array if we have no touches
		//    (because if this function doesnt run when a touch comes up, its not removed from this array!)
		if(!Input.GetMouseButton(0))
			touches.Clear();
		
        if(Input.GetMouseButtonDown(0))
		{
			//Add the new "touch"
			touches.Add(new TouchTracker(0));
		
			StartCoroutine(TryTouch(0));
			StartCoroutine(TryTouchHold(0));
			StartCoroutine(TrySwipe(0));
			StartCoroutine(TryDrag(0));
		//	for(int j=0;j<touches.Count-1;j++)
		//		StartCoroutine(TryPinch(j,0));
			StartCoroutine(TryTap(0));
			StartCoroutine(TryDoubleTap(0));
		}
		
		if(Input.GetMouseButtonUp(0))
		{
			//NOTE: This could be simplified for mouse input, but I'm deciding to keep it equal to the multitouch segment, so we have less discrepancy.
			//Set ended touch states' indices to 
			for(int k=0;k<touches.Count;k++) {
				if(touches[k].TouchIndex>0)		touches[k].TouchIndex--;
				else if(touches[k].TouchIndex==0)	{ touches[k].TouchIndex=-1; touches[k].State = TouchState.Locked; }
			}
			//Remove all old touch states
			while(touches.Count>0 && touches[touches.Count-1].TouchIndex==-1)
				touches.RemoveAt(touches.Count-1);
		}
	}
	
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
	void LateUpdate()
	{
		lastMousePos = Input.mousePosition;
	}
#endif

    void Multitouch_HandleInput()
    {
		//Clear the TouchStates array if we have no touches
		//    (because if this function doesnt run when a touch comes up, its not removed from this array!)
		if(Input.touches.Length==0)
			touches.Clear();

        for (int i = Input.touchCount-1; i >= 0; --i)	//Need to count from top, because taking touches of the list needs to happen top-first; otherwise, numbers get out of order
        {
            switch (Input.touches[i].phase)
            {
                case TouchPhase.Began:
				
					//Add the new touch
					touches.Add(new TouchTracker(i));
				
					StartCoroutine(TryTouch(touches.Count-1));
					StartCoroutine(TryTouchHold(touches.Count-1));
					StartCoroutine(TrySwipe(touches.Count-1));
					StartCoroutine(TryDrag(touches.Count-1));
					for(int j=0;j<touches.Count-1;j++)
						StartCoroutine(TryPinch(j,touches.Count-1));
					StartCoroutine(TryTap(touches.Count-1));
					StartCoroutine(TryDoubleTap(touches.Count-1));

                    break;
				
                case TouchPhase.Canceled:
                case TouchPhase.Ended:
					//Set ended touch states' indices to 
					for(int k=0;k<touches.Count;k++) {
						if(touches[k].TouchIndex>i)		touches[k].TouchIndex--;
						else if(touches[k].TouchIndex==i)	{ touches[k].TouchIndex=-1; touches[k].State = TouchState.Locked; }
					}
					//Remove all old touch states
					while(touches.Count>0 && touches[touches.Count-1].TouchIndex==-1)
						touches.RemoveAt(touches.Count-1);
				
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    break;
                default:
                    break;
            }
        }
			
    }
	
	
	
	// INPUT DELEGATES //
	public delegate void positionDelegate(Vector2 pos);
	public delegate void positionDeltaDelegate(Vector2 pos, Vector2 delta);
	public delegate void floatDelegate(float f);
	public delegate void endGestureDelegate();
	
	
	// INPUT EVENTS //
	
	public static event floatDelegate OnPinch;
	public static event floatDelegate OnPinchStart;
	public static event floatDelegate OnPinchEnd;
	
	public static event positionDelegate OnTouch;
	public static event positionDelegate OnTouchStart;
	public static event positionDelegate OnTouchEnd;
	
	public static event positionDelegate OnTouchHold;
	public static event positionDelegate OnTouchHoldStart;
	public static event positionDelegate OnTouchHoldEnd;
	public static event positionDelegate OnTouchHoldAttempt;
	public static event endGestureDelegate OnTouchHoldAbandonded;
	
	public static event positionDelegate OnTap;
	public static event positionDelegate OnDoubleTap;
	
	public static event positionDelegate OnDoubleTapHold;
	public static event positionDelegate OnDoubleTapHoldStart;
	public static event positionDelegate OnDoubleTapHoldEnd;
	
	public static event positionDelegate OnSwipe;
	public static event positionDeltaDelegate OnSwipeStart;
	public static event positionDelegate OnSwipeEnd;
	
	public static event positionDelegate OnDrag;
	public static event positionDelegate OnDragStart;
	public static event positionDelegate OnDragEnd;
	
	
	
	
	// INPUT TRACKER FUNCTIONS //
	private IEnumerator TryTouch(int touchIndex)
	{
		Vector2 vCurPos = touches[touchIndex].Position;
		
		if(OnTouchStart!=null)	OnTouchStart(vCurPos);
		
		while(!IsTouchOver(touchIndex))
		{
			vCurPos = touches[touchIndex].Position;
			
			if(!IsTouchUnclaimed(touchIndex))
				break;
			
			if(OnTouch!=null)	OnTouch(vCurPos);
			
			yield return null;
		}
		
		if(OnTouchEnd!=null)	OnTouchEnd(vCurPos);
	}
	
	private IEnumerator TryTap(int touchIndex)
	{
		const float TAP_TIME_THRESHOLD = 0.25f;
		float TAP_DIST_THRESHOLD = Screen.height/50f;
		
		float fTouchStartTime = Time.time;
		Vector2 vTouchStartPos = touches[touchIndex].Position;
		
		while(!IsTouchOver(touchIndex))
		{
			//If the touch moved or was held down too long, it's not a tap, so quit the ENTIRE FUNCTION.
			if((vTouchStartPos-touches[touchIndex].Position).sqrMagnitude > TAP_DIST_THRESHOLD*TAP_DIST_THRESHOLD)
				yield break;
			if(Time.time-fTouchStartTime > TAP_TIME_THRESHOLD)
				yield break;
			
			yield return null;
		}
		
		// -- OnTap
		if(OnTap!=null) OnTap(vTouchStartPos);
	}
	
	private IEnumerator TryTouchHold(int touchIndex)
	{
		const float DEAD_TIME = 0.075f;
		float TAPHOLD_DIST_THRESHOLD = Screen.height/40f;
		
		float fTouchStartTime = Time.time;
		
		Vector2 vInitPos = touches[touchIndex].Position;
		Vector2 vCurPos = vInitPos;
		
		bool bActivated = false;
		
		while(!IsTouchOver(touchIndex))
		{
			vCurPos = touches[touchIndex].Position;
	
			//If the touch is either locked or consumed before we got going, quit trying to touch-hold
			if(!IsTouchUnclaimed(touchIndex))
			{
				if(OnTouchHoldAbandonded!=null)	OnTouchHoldAbandonded();
				break;
			}
			
			//If the touch has moved too far, it's not longer a tap-hold, so exit the loop
			if((vCurPos-vInitPos).sqrMagnitude > TAPHOLD_DIST_THRESHOLD*TAPHOLD_DIST_THRESHOLD)
			{
				if(OnTouchHoldAbandonded!=null)	OnTouchHoldAbandonded();
				break;
			}
			
			//After the "Dead time", test for drag
			if(Time.time-fTouchStartTime >= DEAD_TIME)
			{
				if(!bActivated) {
					bActivated = true;
					// -- OnTouchHoldStart
					if(OnTouchHoldStart!=null)	OnTouchHoldStart(vCurPos);
				}
				
				// -- OnTouchHold
				if(OnTouchHold!=null)	OnTouchHold(vCurPos);
			}
			else
			{
				if(OnTouchHoldAttempt!=null)	OnTouchHoldAttempt(vCurPos);
			}
			
			yield return null;
		}
		
		if(bActivated) {
			// -- OnTouchHoldEnd
			if(OnTouchHoldEnd!=null)	OnTouchHoldEnd(vCurPos);
		}
	}
	
	private IEnumerator TryDrag(int touchIndex)
	{
		const float DEAD_TIME = 0.125f;
		float DRAG_DIST_THRESHOLD = Screen.height/40f;
		
		float fTouchStartTime = Time.time;
		
		Vector2 vInitPos = touches[touchIndex].Position;
		Vector2 vCurPos = vInitPos;
		
		bool bActivated = false;
		
		while(!IsTouchOver(touchIndex))
		{
			vCurPos = touches[touchIndex].Position;
			
			//If the touch is either locked or consumed before we got going, quit trying to touch-hold
			if(!IsTouchUnclaimed(touchIndex))
				break;
			
			//After the "Dead time", test for drag
			if(Time.time-fTouchStartTime >= DEAD_TIME)
			{
				if(!bActivated && (vCurPos-vInitPos).sqrMagnitude > DRAG_DIST_THRESHOLD*DRAG_DIST_THRESHOLD) {
					bActivated = true;
					// -- OnDragStart
					if(OnDragStart!=null)	OnDragStart(vCurPos);
				}
				
				// -- OnDrag
				if(OnDrag!=null)	OnDrag(vCurPos);
			}
			
			yield return null;
		}
		
		if(bActivated) {
			// -- OnDragEnd
			if(OnDragEnd!=null)	OnDragEnd(vCurPos);
		}
	}
	
	
	private IEnumerator TrySwipe(int touchIndex)
	{
		Vector2 vInitPos = touches[touchIndex].Position;
		Vector2 vSwipeStartDecayed = vInitPos;	//The start of a swipe, which decays TOWARD the current position each frame
		
		float SWIPE_DECAY = Screen.height/3f;			//On a screen with height 1080, 360 px/sec
		float SWIPE_MIN_THRESHOLD = Screen.height/50f;	//On a screen with height 1080, 22 px
		float REQ_SWIPE_DIST = Screen.height/20f;		//On a screen with height 1080, 54 px
		
		Vector2 vCurPos = Vector2.zero;
		
		bool bActivated = false;
		
		while(!IsTouchOver(touchIndex))
		{
			if(!IsTouchUnclaimed(touchIndex))
				break;
			
			vCurPos = touches[touchIndex].Position;
			
			if(!bActivated &&
			   (vCurPos-vInitPos).sqrMagnitude > SWIPE_MIN_THRESHOLD*SWIPE_MIN_THRESHOLD &&
			   (vCurPos-vSwipeStartDecayed).sqrMagnitude > REQ_SWIPE_DIST*REQ_SWIPE_DIST)
			{
				bActivated = true;
				// -- OnSwipeStart
				if(OnSwipeStart!=null)	OnSwipeStart(vCurPos, vCurPos-vInitPos);
			}
			
			if(bActivated) {
				// -- OnSwipe
				if(OnSwipe!=null)	OnSwipe(vCurPos);	//TODO: Determine when a swipe starts, stays, and ends (needs balancing)
			}
			
			//Swipe Decay
			vSwipeStartDecayed = Vector2.MoveTowards(vSwipeStartDecayed,vCurPos,SWIPE_DECAY*Time.deltaTime);
			
			yield return null;
		}
			
		if(bActivated) {
			// -- OnSwipeEnd
			if(OnSwipeEnd!=null)	OnSwipeEnd(vCurPos);
		}
	}
	
	private IEnumerator TryDoubleTap(int touchIndex)
	{
		const float TAP_DOWN_TIME_MAX = 0.25f;
		const float TAP_UP_TIME_MAX = 0.5f;
		
		float fStart = Time.time;
		
		Vector2 vStartPos = touches[touchIndex].Position;
		Vector2 vCurPos = vStartPos;
		
		//Wait for the touch to be up OR for a timeout
		while(!IsTouchOver(touchIndex)) {
			vCurPos = touches[touchIndex].Position;
			
			if(!IsTouchUnclaimed(touchIndex))		yield break;
			
			if(Time.time-fStart<TAP_DOWN_TIME_MAX)	yield return null;	//wait
			else 									yield break;		//quit entire function
		}
		
		fStart = Time.time;
		
		//Wait for the touch to be down again OR for a timeout
		while(IsTouchOver(touchIndex)) {
			if(Time.time-fStart<TAP_UP_TIME_MAX)	yield return null;	//wait
			else 									yield break;		//quit entire function
		}
		
		//Double tap complete!
		bool bActivated = false;
		fStart = Time.time;
		vCurPos = touches[touchIndex].Position;
		Claim(touchIndex);
		
		//Wait and see if this seconds tap turns into a hold, or just ends in a double tap
		while(!IsTouchOver(touchIndex))
		{
			vCurPos = touches[touchIndex].Position;
			
			if(!bActivated && Time.time-fStart >= TAP_DOWN_TIME_MAX) {
				bActivated = true;
				// -- OnDoubleTapHoldStart
				if(OnDoubleTapHoldStart!=null)	OnDoubleTapHoldStart(vCurPos);
			}
			if(bActivated) {
				// -- OnDoubleTapHold
				if(OnDoubleTapHold!=null)	OnDoubleTapHold(vCurPos);
			}
			yield return null;
		}
		
		//End in a double-tap or a double-tap-hold-ending
		if(!bActivated) {		//If the second touch wasnt held down very long, log a double tap
			// -- OnDoubleTap
			if(OnDoubleTap!=null)	OnDoubleTap(vStartPos);
		}
		else {
			//Otherwise, it's the end of a double-tap-hold.
			// -- OnDoubleTapHoldEvent
			if(OnDoubleTapHoldEnd!=null)	OnDoubleTapHoldEnd(vCurPos);
		}
	}
	
	
	//TODO: Look this over; may not be functioning
	private IEnumerator TryPinch(int index1, int index2)
	{
		if(!IsTouchUnclaimed(index2))	//Only the new touch needs to be unclaimed
			yield break;
		
		float fInitDist = (touches[index1].Position-touches[index2].Position).magnitude;
		bool bDeadZonePassed = false;
		float PinchPixelThreshold = 5f;
		
		//Claim both touches, so that most other mechanics cant use them
		Claim(index1);
		Claim(index2);
		Vector2 t1 = Vector2.zero;
		Vector2 t2 = Vector2.zero;
		//Two touches have been consumed; now wait for the two to pass the distance difference threshold
		while(!IsTouchOver(index1) && !IsTouchOver(index2))
		{
			t1 = touches[index1].Position;
			t2 = touches[index2].Position;
			if(!bDeadZonePassed && Mathf.Abs((t1-t2).magnitude-fInitDist) > PinchPixelThreshold)
			{
				bDeadZonePassed = true;
				if(OnPinchStart!=null)	OnPinchStart((t1-t2).magnitude-fInitDist);
			}
			
			if(bDeadZonePassed)
			{
				if(OnPinch!=null)	OnPinch((t1-t2).magnitude-fInitDist);
				fInitDist = (t1-t2).magnitude;
			}
				
			yield return null;
		}
		
		if(bDeadZonePassed) {
			if(OnPinchStart!=null)	OnPinchEnd((t1-t2).magnitude-fInitDist);
		}
		
		//This was for megadillo... do we still need it?
		if(!IsTouchOver(index2))
			RenewTouch(index2);
		if(!IsTouchOver(index1))
			RenewTouch(index1);
		
	}
	
	
	
	
	// TOUCH CLAIMING FUNCTIONS //
	
	protected void Claim(int index)
	{
		if(index>=0 && index<=touches.Count && touches[index].State!=TouchState.Locked)
		{
			touches[index].State = TouchState.Claimed;
		}
	}
	protected void Unclaim(int index)
	{
		if(index>=0 && index<=touches.Count && touches[index].State!=TouchState.Locked)
		{
			touches[index].State = TouchState.Unclaimed;
		}
	}
	
	protected bool IsTouchOver(int index)
	{
		return IsTouchLocked(index);
	}
	
	protected bool IsTouchUnclaimed(int index)
	{
		return index>=0 && index<touches.Count && touches[index].State==TouchState.Unclaimed;
	}
	
	protected bool IsTouchLocked(int index)
	{
		return index<0 || index>=touches.Count || touches[index].State==TouchState.Locked;
	}
	
	
	protected void RenewTouch(int index)
	{
		//Reinstate touch as if it was just started (almost)
		touches[index].State = TouchState.Unclaimed;
		
		//NOTE: Be careful with these, they could lead to INFINITE LOOPS if this function is called in them!
		StartCoroutine(TryTouchHold(index));
		StartCoroutine(TrySwipe(index));
	}
	
}
