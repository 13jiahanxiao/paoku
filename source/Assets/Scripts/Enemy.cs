///
/// Imangi Studios LLC ("COMPANY") CONFIDENTIAL
/// Copyright (c) 2011-2012 Imangi Studios LLC, All Rights Reserved.
///
/// NOTICE:  All information contained herein is, and remains the property of COMPANY. The intellectual and technical concepts contained
/// herein are proprietary to COMPANY and may be covered by U.S. and Foreign Patents, patents in process, and are protected by trade secret or copyright law.
/// Dissemination of this information or reproduction of this material is strictly forbidden unless prior written permission is obtained
/// from COMPANY.  Access to the source code contained herein is hereby forbidden to anyone except current COMPANY employees, managers or contractors who have executed 
/// Confidentiality and Non-disclosure agreements explicitly covering such access.
///
/// The copyright notice above does not evidence any actual or intended publication or disclosure of this source code, which includes  
/// information that is confidential and/or proprietary, and is a trade secret, of COMPANY. ANY REPRODUCTION, MODIFICATION, DISTRIBUTION, PUBLIC  PERFORMANCE, 
/// OR PUBLIC DISPLAY OF OR THROUGH USE OF THIS SOURCE CODE WITHOUT THE EXPRESS WRITTEN CONSENT OFCOMPANY IS STRICTLY PROHIBITED, AND IN VIOLATION OF APPLICABLE 
/// LAWS AND INTERNATIONAL TREATIES. THE RECEIPT OR POSSESSION OF THIS SOURCE CODE AND/OR RELATED INFORMATION DOES NOT CONVEY OR IMPLY ANY RIGHTS  
/// TO REPRODUCE, DISCLOSE OR DISTRIBUTE ITS CONTENTS, OR TO MANUFACTURE, USE, OR SELL ANYTHING THAT IT MAY DESCRIBE, IN WHOLE OR IN PART.                
///

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {
	protected static Notify notify;
	
	public static Enemy main;

	public bool IsJumping;
	public bool JumpAfterDelay;
	public float JumpDelay;
	public float JumpVelocity;
	public bool IsOverGround;
	public float GroundHeight;
	public bool IsTaunting = false;
	public float baboonMusicVolume = 0.6f;
//	private bool isBaboonMusic = false;
	
	public Vector3 Velocity;
	public Vector3 PlayerForce;
	public bool HasForce = false;
	public float AngularVelocity;
	public bool LimitVelocity;
	public float MaxVelocityMagnitudeTotal = -1;
	public float MaxVelocityMagnitudeX = -1;
	public float MaxVelocityMagnitudeY = -1;
	public float MaxVelocityMagnitudeZ = -1;
	public Vector3 RotationAxis = new Vector3(0,1,0);
	public Material mat = null;
	private Animation[] CachedAnimations = null;
	
	public Animation baboonAnim;
	public Transform rootXform = null;
	public bool isAttacking = false;
	public bool isGrabbing = false;
	public bool isKilling = false;
//	private Vector3 attackPos;
	
	
	// sfx music test for baboons
	//public AudioClip baboonMusic;
	public AudioClip baboonChatter;
	public AudioClip baboonIntro;
	//public AudioClip baboonWings;
	//public AudioSource audioMusic;
	public AudioSource audioChatter;
	//public AudioSource audioWings;
	
	public Animation baboon_L;
	public Animation baboon_R;
	public Animation baboon_M;
	public Vector3 offset_L;
	public Vector3 offset_R;
	public Vector3 offset_M;
	
	private Renderer[] baboonRenderer = new Renderer[3];

	
	// Use this for initialization
	void Awake() {
		notify = new Notify(this.GetType().Name);
		mat = this.gameObject.GetComponentInChildren<Renderer>().material;
		// in oz we have three baboons chasing him, not one big one, so deliberately keeping this null
		main = this; 
	}
	public void Setup () {
		Reset();
		
		baboonRenderer[0] = baboon_L.GetComponentInChildren<Renderer>();
		baboonRenderer[1] = baboon_M.GetComponentInChildren<Renderer>();
		baboonRenderer[2] = baboon_R.GetComponentInChildren<Renderer>();
		
		if(!AudioManager.SharedInstance.DisableAudio){
			if(!audioChatter && baboonChatter){
				audioChatter = gameObject.AddComponent<AudioSource>();
				audioChatter.volume = 0.5f;
				audioChatter.clip = baboonChatter;
				audioChatter.playOnAwake = false;
				audioChatter.loop = true;
			}
		}
		
		baboon_L.Play("IntroLeft");
		baboon_M.Play("IntroMiddle");
		baboon_R.Play("IntroRight");
	
		//Play the idle animations after their current one
		if(!hasFlownIn)
		{
			baboon_L.CrossFadeQueued("IdleLeft");
			baboon_M.CrossFadeQueued("IdleMiddle");
			baboon_R.CrossFadeQueued("IdleRight");
		}
	}
	
	public void PlayIdleBaboonAnimations()	// for main menu
	{
		baboon_L.Play("IdleLeft");
		baboon_M.Play("IdleMiddle");
		baboon_R.Play("IdleRight");
	}

	public void Jump(float delay=0)
	{
		if (!IsJumping) {
			if (delay > 0.0f) {
				JumpAfterDelay = true;
				JumpDelay = delay;
			} else {
				JumpAfterDelay = false;
				IsJumping = true;			
				Velocity.y = JumpVelocity;
			//	if (this.animation != null)
				//{
				//	this.animation.Play("Jump01", PlayMode.StopAll);
				//	this.animation.PlayQueued("Run01");
				//}
			}
		}
	}
	
	
	public void doPauseAnimation()
	{
		foreach(AnimationState ans in this.animation) 
		{
			ans.speed = 0;
		}
	}
	
	public void doUnPauseAnimation()
	{
		doAdjustAnimationSpeed();
	}
	
	public void doAdjustAnimationSpeed()
	{
		foreach(AnimationState state in this.animation) 
		{
			state.speed = 1.0f;
		}
	}
	
	public void doApplyForce(Vector3 deltaForce)
	{
		PlayerForce += deltaForce;
		HasForce = true;
	}
	
	public void doApplyForcef(float deltaForce)
	{
		PlayerForce.x += deltaForce;
		PlayerForce.y += deltaForce;
		PlayerForce.z += deltaForce;
		HasForce = true;
	}
	
	public void doResetForce()
	{
		PlayerForce = Vector3.zero;
		HasForce = false;
	}
	
	public bool IsMoving()
	{
		if (Velocity.sqrMagnitude == 0)
			return false;
		
		return true;
	}
	
	public bool ShouldFollowPlayer() {
		if(GamePlayer.SharedInstance.Dying || isKilling)	return false;
		if(GamePlayer.SharedInstance.IsDead == true && 
			(GamePlayer.SharedInstance.DeathType == DeathTypes.Mine ||
			GamePlayer.SharedInstance.DeathType == DeathTypes.MineLedge ||
			GamePlayer.SharedInstance.DeathType == DeathTypes.Wheel))
			return false;
		
		return true;
	}
	
	public void SetY(float y)
	{
		transform.position = new Vector3(transform.position.x, y,transform.position.z);
	}
	
	private bool firstUpdate = false;
	private Vector3 runToTargetLocation = Vector3.zero;
	private Vector3 currentForward = Vector3.zero;
	private float timeInAir = 0f;
	
	private bool hasFlownIn = false;
	public bool HasFlownIn { get { return hasFlownIn; } }
	
	public void FlyIn()
	{
		if(!hasFlownIn)
		{
			hasFlownIn = true;
			baboon_M.CrossFade("FlyInMiddle");
			baboon_L.CrossFade("FlyInLeft");
			baboon_R.CrossFade("FlyInRight");
			baboon_M.CrossFadeQueued("chase");
			baboon_L.CrossFadeQueued("chase");
			baboon_R.CrossFadeQueued("chase");
			StartCoroutine(MoveMonkeyTowards(baboon_L.transform,offset_L));
			StartCoroutine(MoveMonkeyTowards(baboon_R.transform,offset_R));
			StartCoroutine(MoveMonkeyTowards(baboon_M.transform,offset_M));
			
			//audio.PlayOneShot(baboonIntro);
			AudioManager.SharedInstance.PlayBaboonsFlyIn();
		}
		
	}
	
	public void StartChase()
	{
		SetY(0);
		IsJumping = false;
		JumpAfterDelay = false;
		JumpVelocity = 8.5f;
		IsOverGround = false;
		GroundHeight = 0.0f;
		IsTaunting = false;
		isAttacking = false;
		isGrabbing = false;
		isKilling = false;
		
		hasFlownIn = true;
		baboon_M.Play("chase");
		baboon_L.Play("chase");
		baboon_R.Play("chase");
		baboon_R.transform.localPosition = offset_R;
		baboon_M.transform.localPosition = offset_M;
		baboon_L.transform.localPosition = offset_L;
//		StartCoroutine(MoveMonkeyTowards(baboon_L.transform,offset_L,0f));
	//	StartCoroutine(MoveMonkeyTowards(baboon_R.transform,offset_R,0f));
	//	StartCoroutine(MoveMonkeyTowards(baboon_M.transform,offset_M,0f));
	}
	
	public IEnumerator MoveMonkeyTowards(Transform tr, Vector3 target, float wait = 1.5f)
	{
		//First, wait a bit for the animation to start
		yield return new WaitForSeconds(wait);
		
		while(tr.localPosition!=target)
		{
			//If we pass in zero, we are calling this from "StartChase" and thus want it to happen immediately
			float actualWait = wait==0f ? 10f : Time.deltaTime;
			
			tr.localPosition = Vector3.MoveTowards(tr.localPosition,target,actualWait);
			yield return null;
		}
	}
	
	private bool[] lastVisible = new bool[3] {false,false,false};
	
	public void doUpdate()
	{
		if(!enabled || Time.timeScale == 0f)
		{
			return;
		}
		if (GamePlayer.SharedInstance.Hold) {
			return;
		}
		if(firstUpdate == false) {
			firstUpdate = true;
		}
		//bool flyInDone = (baboon_L.animation["chase"].speed > 0.0f)&&(baboon_R.animation["chase"].speed > 0.0f);
		if(hasFlownIn && (!(isAttacking||isGrabbing||isKilling)))
		{
			for(int i = 0; i < 3; ++i)
			{
				if(lastVisible[i] != baboonRenderer[i].isVisible)
				{
					lastVisible[i] = baboonRenderer[i].isVisible;
					if(!lastVisible[i])
					{
						if(i==0)
						{
							baboon_L.gameObject.SetActiveRecursively(false);
						}
						else
						if(i==2)
						{
							baboon_R.gameObject.SetActiveRecursively(false);
						}
						else
						if(i==1)
						{
							baboon_M.animation.Stop();
						}
					}
					else
					if(i==1)
					{
						if(!baboon_M.animation.isPlaying)
							baboon_M.animation.Play();
					}
				}
			}
		}
		else
		{
			if(!baboon_L.gameObject.active)
			{
				baboon_L.gameObject.SetActiveRecursively(true);
				baboon_L.animation.Play("chase");
				baboon_L.transform.localPosition = offset_L;
				lastVisible[0] = false;
			}
			if(!baboon_R.gameObject.active)
			{
				baboon_R.gameObject.SetActiveRecursively(true);
				baboon_R.animation.Play("chase");
				baboon_R.transform.localPosition = offset_R;
				lastVisible[2] = false;
			}
		}
		
		
		if(rootXform && audioChatter){
			Vector3 diff = GamePlayer.SharedInstance.transform.position - rootXform.position;
			float mag = diff.magnitude;
			
			audioChatter.volume = baboonMusicVolume * Mathf.Clamp(1f - mag * 0.12f, 0f, 1f) * AudioManager.SharedInstance.SoundVolume; // 9 * 0.12
				
			if(mag > 9f){
			//if(isBaboonMusic){
			//	if(audioChatter){
					//	if(mag > 9f){
					if(audioChatter.isPlaying)
					{
						audioChatter.Stop();
				//		isBaboonMusic = false;
					}
				//	}
			//	}
			}
			else if(mag < 9f) {
				//notify.Debug("turn on baboon music");
			//	if(audioChatter){
				//	audioChatter.volume = 0f;
					if(!audioChatter.isPlaying && !GamePlayer.SharedInstance.IsDead)
				{
						audioChatter.Play();
				}
			//	}
			//	isBaboonMusic = true;
			}
			
			if(GamePlayer.SharedInstance.IsDead){
				if(audio!=null)	audio.Stop();
			}
		}
			
		if(isAttacking) {
			Attack();
			//if(!isGrabbing) transform.position += diff.normalized * Time.deltaTime * GamePlayer.SharedInstance.PlayerVelocity.magnitude * 1.3f;
			//else transform.position += (transform.forward + Vector3.up) * Time.deltaTime * 2f;
			//notify.Debug ("isAttacking " + isAttacking + " isGrabbing " + isGrabbing + " mag " + diff.magnitude);
			//if(mag < 1.7f) Attack();
		//	if(isGrabbing){
		//		if((attackPos - rootXform.position).magnitude > 6f) Kill();
		//	}
			return;
		}
		
		if (JumpAfterDelay) {
			JumpDelay -= Time.smoothDeltaTime;
			if (JumpDelay <= 0)
				Jump();
		}
		
		float yAdd = 0f;
		
		if(IsJumping == true) {
			// Gravity
			float jumpGravityForce = GamePlayer.SharedInstance.PlayerGravity;
			
//			if (transform.position.y > GamePlayer.SharedInstance.currentPosition.y) {
//				jumpGravityForce *= 2.0f;
//			}
				
			// Gravity force on jump
			Velocity.y += (jumpGravityForce * Time.deltaTime);	
			
			timeInAir += Time.deltaTime;
			yAdd = (-Mathf.Cos(Mathf.Clamp01(timeInAir)*Mathf.PI*2f) + 1f);
			
			if(timeInAir>=1f)
				IsJumping = false;
		}
		
	//	else if(Velocity != Vector3.zero)
	//		transform.Translate(Velocity * Time.deltaTime, Space.World);
		//
	//	if(transform.position.y < GamePlayer.SharedInstance.CurrentPosition.y) {
	//		IsJumping = false;
	//	}
		
		if(ShouldFollowPlayer() == true) {
			transform.eulerAngles = GamePlayer.SharedInstance.CachedTransform.eulerAngles;	
			runToTargetLocation = GamePlayer.SharedInstance.CurrentPosition;
			currentForward = GamePlayer.SharedInstance.CachedTransform.forward;
		}
		
	//	float newYLocation = transform.position.y;
		if(IsJumping == false) {
			//newYLocation = runToTargetLocation.y; 
		}
		else
			runToTargetLocation.y += yAdd;
		
		transform.position = runToTargetLocation + (currentForward * -GameController.SharedInstance.EnemyFollowDistance);
		//SetY(newYLocation);
		IsOverGround = true;
		


	}
	
	public void StopAudio(){
		if(audio!=null)	audio.Stop();
	}
	
	
	public void Attack(){
		if(isGrabbing) return;
		baboon_M.transform.localPosition = Vector3.zero;
		isGrabbing = true;
		//notify.Debug ("Enemy Attack()");
	//	attackPos = rootXform.position;
		//((OzGameCamera)OzGameCamera.SharedInstance).Stop();
		GamePlayer.SharedInstance.Dying = true;
	//	GamePlayer.SharedInstance.enabled = false;	//This was causing problems (first tile piece missing next run), and it turns out we didnt need it!
		
		//enabled = false;
		
		//StartCoroutine(LerpOzToClutches());
		StartCoroutine(LerpToOz());
	}
	
	private IEnumerator LerpToOz()
	{
		Vector3 pos = baboon_M.transform.position;
		Vector3 target = GamePlayer.SharedInstance.transform.position;
		GameController.SharedInstance.Player.AnimateObject.CrossFade("CarriedOff01",0.15f);
		baboon_M.CrossFade("grab",0.15f);
		while(pos!=target)
		{
			pos = Vector3.MoveTowards(pos,target,Time.deltaTime*15f);
			baboon_M.transform.position = pos;
			
		//	Debug.Log((pos-target).magnitude);
			
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		Kill();
	}
	
	/*private IEnumerator LerpOzToClutches()
	{
		yield return new WaitForSeconds(0.35f);	//Approx. the amount of time through the baboon animation that it reaches Oz
		
		Transform animTransform = GameController.SharedInstance.Player.CharacterModel.transform;
		
		GameController.SharedInstance.Player.AnimateObject.CrossFade("CarriedOff01");
		
		animTransform.parent = rootXform;
		
		Vector3 targetLocalPos = Vector3.down*0.95f + Vector3.forward*0.3f;
		Vector3 startLocPos = animTransform.localPosition;
		Quaternion startLocRot = animTransform.localRotation;
		float t=0f;
		while(t<1f)
		{
			t = Mathf.MoveTowards(t,1f,Time.deltaTime*5f);
			animTransform.localRotation = Quaternion.Slerp(startLocRot,Quaternion.identity,t);
			animTransform.localPosition = Vector3.Lerp(startLocPos,targetLocalPos,t);
			yield return null;
		}
	}*/
	
	public void Kill() {
		//notify.Debug("Enemy Kill");
		isAttacking = false;
		isGrabbing = false;
		isKilling = true;
		
		//((OzGameCamera)OzGameCamera.SharedInstance).Unstop();
	//	GamePlayer.SharedInstance.enabled = true;
		GamePlayer.SharedInstance.transform.parent = null;
		GamePlayer.SharedInstance.CharacterModel.transform.parent = null;
		//baboonAnim.CrossFade("chase");
		GamePlayer.SharedInstance.Kill(DeathTypes.Baboon);
	//	enabled = true;
	}

	public void Reset()
	{
		//Debug.Log("Enemy Reset");
		StopAllCoroutines();
		
		SetY(0);
		IsJumping = false;
		JumpAfterDelay = false;
		JumpVelocity = 8.5f;
		IsOverGround = false;
		GroundHeight = 0.0f;
		IsTaunting = false;
		isAttacking = false;
		isGrabbing = false;
		isKilling = false;

		if(!baboon_L.gameObject.active)
		{
			baboon_L.gameObject.SetActiveRecursively(true);
			lastVisible[0] = false;
		}
		if(!baboon_R.gameObject.active)
		{
			baboon_R.gameObject.SetActiveRecursively(true);
			lastVisible[2] = false;
		}
		if(hasFlownIn)
		{
			baboon_L.transform.localPosition = offset_L;
			baboon_L.Play("IdleLeft");
			baboon_M.Play("IdleMiddle");
			baboon_R.transform.localPosition = offset_R;
			baboon_R.Play("IdleRight");
		}
		
		hasFlownIn = false;

		CachedAnimations = GetComponentsInChildren<Animation>();
		foreach(Animation a in CachedAnimations)
		{
			if(a!=null && a["chase"]!=null)
			{
				a["chase"].normalizedTime = Random.Range(0.0f,1.0f);
				a["chase"].speed = Random.Range(0.9f,1.1f);
			}
		}
		
		if(mat == null) {
			mat = this.gameObject.GetComponentInChildren<Renderer>().material;
		}
		
		transform.position = Vector3.zero;
		transform.LookAt(-Vector3.forward,Vector3.up);
		
		foreach(Animation a in CachedAnimations)
		{
			a.transform.position = Vector3.zero;
		}
		
	}

	public bool _bIsSelected = true;

	void OnDrawGizmos()
	{
		if (_bIsSelected)
			OnDrawGizmosSelected();
	}


	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position, 0.1f);  //center sphere
		if (transform.renderer != null)
			Gizmos.DrawWireCube(transform.position, transform.renderer.bounds.size);
	}

}
