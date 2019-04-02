using UnityEngine;
using System.Collections;

public class PlayerFx : MonoBehaviour {
	public ParticleSystem bubble;
	public ParticleSystem bubblePop;
	public ParticleSystem bubbleInside;
	public ParticleSystem poofSmoke;
	public ParticleSystem poofTrail;	
	public ParticleSystem poofPop;
	public ParticleSystem shieldRings;
	public ParticleSystem shieldInside;	
	public ParticleSystem shieldBreak;	
	public ParticleSystem boostTrail;		
	public ParticleSystem magicMagnet;	
	public ParticleSystem stumbleProof;
	public ParticleSystem stumbleProofStatic;
	public ParticleSystem scoreBonusElectric;
	public ParticleSystem scoreBonusSmoke;
	public ParticleSystem scoreBonusSpark;
	public ParticleSystem dustSlide;
	public ParticleSystem dustLand;	
	public ParticleSystem objectGrab;	
	public ParticleSystem magicWand;
	public ParticleSystem ozDisappear;
	
	public Vector3 shieldEffectsChinaGirlPosition;
	public Vector3 glindasBubbleChinaGirlPosition;
	public Vector3 scoreBonusElectricChinaGirlPosition;	
	public Vector3 magicMagnetChinaGirlPosition;		
	//public float scoreBonusElectricChinaGirlHeight;	
	private Vector3 shieldEffectsOriginalPosition;		
	private Vector3 glindasBubbleOriginalPosition;	
	private Vector3 scoreBonusElectricOriginalPosition;	
	private Vector3 magicMagnetOriginalPosition;	
	
	// Use this for initialization
	void Start () {
		if(scoreBonusElectric != null) {
			Vector3 sbTempVec3 = scoreBonusElectric.transform.localPosition;
			scoreBonusElectricOriginalPosition = sbTempVec3;
		}
		if(magicMagnet != null) {
			Vector3 mmTempVec3 = magicMagnet.transform.localPosition;
			magicMagnetOriginalPosition = mmTempVec3;			
		}	
		if(shieldRings) {
			Vector3 shieldTempVec3 = shieldRings.transform.localPosition;
			shieldEffectsOriginalPosition = shieldTempVec3;
		}
		if(bubble) {
			Vector3 bubbleTempVec3 = bubble.transform.localPosition;
			glindasBubbleOriginalPosition = bubbleTempVec3;
		}		
	}
	
	public void StartBubble(){
		if(bubble && bubble.gameObject.active){
			bubble.Play(true);
			if(GameProfile.SharedInstance.GetActiveCharacter().characterId == 3)
			{
				Vector3 tempVector = glindasBubbleChinaGirlPosition;
				bubble.transform.localPosition = tempVector;
				bubble.startSize = 1.2f;
			}
			else
			{				
				Vector3 tempVector = glindasBubbleOriginalPosition;
				bubble.transform.localPosition = tempVector;
				bubble.startSize = 1.5f;				
			}			
		}
	}
	
	public void StopBubble(){
		if(bubble){
			bubble.Stop(true);
			bubble.Clear(true);
		}
	}

	public void StartBubblePop(){
		if(bubblePop && bubblePop.gameObject.active){
			bubblePop.Play(true);
			if(GameProfile.SharedInstance.GetActiveCharacter().characterId == 3)
			{
				Vector3 tempVector = glindasBubbleChinaGirlPosition;
				bubblePop.transform.localPosition = tempVector;	
			}
			else
			{				
				Vector3 tempVector = glindasBubbleOriginalPosition;
				bubblePop.transform.localPosition = tempVector;
			}
		}
	}	
	
	public void StartBubbleInside(){
		if(bubbleInside && bubbleInside.gameObject.active){
			bubbleInside.Play(true);
			if(GameProfile.SharedInstance.GetActiveCharacter().characterId == 3)
			{
				Vector3 tempVector = glindasBubbleChinaGirlPosition;
				bubbleInside.transform.localPosition = tempVector;	
				bubbleInside.startLifetime = 0.3f;
			}
			else
			{				
				Vector3 tempVector = glindasBubbleOriginalPosition;
				bubbleInside.transform.localPosition = tempVector;
				bubbleInside.startLifetime = 1.0f;
			}			
		}
	}
	
	public void StopBubbleInside(){
		if(bubbleInside){
			bubbleInside.Stop(true);
			bubbleInside.Clear(true);
		}
	}	
	
	public void StartPoofSmoke(){
		if(poofSmoke && poofSmoke.gameObject.active){
			poofSmoke.Play(true);
		}
	}

	public void StartPoofTrail(){
		if(poofTrail && poofTrail.gameObject.active){
			poofTrail.Play(true);
		}
	}
	
	public void StopPoofTrail(){
		if(poofTrail){
			poofTrail.Stop(true);
		}
	}	
	
	public void StartPoofPop(){
		if(poofPop && poofPop.gameObject.active){
			poofPop.Play(true);
		}
	}		
	
	public void StartShieldEffects(){	
		if(shieldRings && shieldRings.gameObject.active){
			shieldRings.Play(true);
		}
		if(shieldInside && shieldInside.gameObject.active){
			shieldInside.Play(true);
		}		
		ResetShieldEffectsPosition();
	}
	
	public void StopShieldEffects(){
		if(shieldRings){
			shieldRings.Stop(true);
			shieldRings.Clear(true);			
		}
		if(shieldInside){
			shieldInside.Stop(true);
			shieldInside.Clear(true);
		}		
	}	
	
	public void ResetShieldEffectsPosition()
	{
		if(shieldRings && shieldRings.isPlaying){	
			if(GameProfile.SharedInstance.GetActiveCharacter().characterId == 3)
			{
				Vector3 ringsVector = shieldEffectsChinaGirlPosition;
				shieldRings.transform.localPosition = ringsVector;	
			}
			else
			{
				Vector3 ringsVector = shieldEffectsOriginalPosition;
				shieldRings.transform.localPosition = ringsVector;
			}
		}
		if(shieldInside && shieldInside.isPlaying){	
			if(GameProfile.SharedInstance.GetActiveCharacter().characterId == 3)
			{
				Vector3 insideVector = shieldEffectsChinaGirlPosition;
				shieldInside.transform.localPosition = insideVector;	
			}
			else
			{				
				Vector3 insideVector = shieldEffectsOriginalPosition;
				shieldInside.transform.localPosition = insideVector;
			}
		}	
	}

	public void SetShieldEffectsSlidePosition(float x, float y, float z)
	{
		if(shieldRings && shieldRings.isPlaying){
			Vector3 ringsVector = shieldRings.transform.localPosition;
			
			ringsVector.x = x;
			ringsVector.y = y;
			ringsVector.z = z;					
			
			shieldRings.transform.localPosition = ringsVector;		
		}
		if(shieldInside && shieldInside.isPlaying){
			Vector3 insideVector = shieldInside.transform.localPosition;
			
			insideVector.x = x;
			insideVector.y = y;
			insideVector.z = z;			
			
			shieldInside.transform.localPosition = insideVector;
		}
	}	
	
	public void StartShieldBreak(){
		if(shieldBreak && shieldBreak.gameObject.active){
			if(GameProfile.SharedInstance.GetActiveCharacter().characterId == 3)
			{
				Vector3 tempVector = shieldEffectsChinaGirlPosition;
				shieldBreak.transform.localPosition = tempVector;	
			}			
			else
			{
				Vector3 tempVector = shieldEffectsOriginalPosition;
				shieldBreak.transform.localPosition = tempVector;
			}			
			shieldBreak.Play(true);
		}
	}		
	
	public void StartBoostTrail(){
		if(boostTrail && boostTrail.gameObject.active){
			boostTrail.Play(true);
		}
	}		

	public void StopBoostTrail(){
		if(boostTrail){
			boostTrail.Stop(true);
		}
	}	
	
	public void StartStumbleProof(){
		if(stumbleProof && stumbleProof.gameObject.active){
			stumbleProof.Play(true);
		}
	}
	
	public void StopStumbleProof(){
		if(stumbleProof){
			stumbleProof.Stop(true);
			stumbleProof.Clear(true);
		}
	}

	public void StartStumbleProofStatic(){
		if(stumbleProofStatic && stumbleProofStatic.gameObject.active){
			stumbleProofStatic.Play(true);
		}
	}	

	public void StartDustSlide(){
		if(dustSlide && dustSlide.gameObject.active){
			if(GameProfile.SharedInstance.GetActiveCharacter().characterId == 3)
			{
				dustSlide.startSize = 0.4f;
			}
			else
			{
				dustSlide.startSize = 0.7f;
			}
			dustSlide.Play(true);
		}
	}		

	public void StopDustSlide(){
		if(dustSlide){
			dustSlide.Stop(true);
		}
	}		
	
	public void StartDustLand(){
		if(dustLand && dustLand.gameObject.active){
			if(GameProfile.SharedInstance.GetActiveCharacter().characterId == 3)
			{
				dustLand.startSize = 0.2f;
			}
			else
			{
				dustLand.startSize = 0.4f;
			}
			dustLand.Play(true);
		}
	}	
	
	public void StartScoreBonusEffects()
	{
		if(scoreBonusElectric && scoreBonusElectric.gameObject.active) {
			//Reset the height in case it was effected by sliding
			ResetScoreBonusElectricHeight();
				
			scoreBonusElectric.Play(true);
		}
		if(scoreBonusSpark && scoreBonusSpark.gameObject.active)
			scoreBonusSpark.Play(true);
		if(scoreBonusSmoke && scoreBonusSmoke.gameObject.active)
			scoreBonusSmoke.Play(true);
	}	
	
	public void StopScoreBonusEffects()
	{
		if(scoreBonusElectric && scoreBonusElectric.gameObject.active) {	
			//Reset the height in case it was effected by sliding
			ResetScoreBonusElectricHeight();
			
			scoreBonusElectric.Stop(true);
			scoreBonusElectric.Clear(true);
		}
		if(scoreBonusSpark && scoreBonusSpark.gameObject.active) {
			scoreBonusSpark.Stop(true);
			scoreBonusSpark.Clear(true);
		}
		if(scoreBonusSmoke && scoreBonusSmoke.gameObject.active) {
			scoreBonusSmoke.Stop(true);	
			scoreBonusSmoke.Clear(true);	
		}
	}
	
	public void ResetScoreBonusElectricHeight()
	{			
		if(GameProfile.SharedInstance.GetActiveCharacter().characterId == 3)
		{
			Vector3 tempVector = scoreBonusElectricChinaGirlPosition;
			scoreBonusElectric.transform.localPosition = tempVector;	
		}
		else
		{
			Vector3 tempVector = scoreBonusElectricOriginalPosition;
			scoreBonusElectric.transform.localPosition = tempVector;					
		}		
	}

	public void SetScoreBonusElectricSlideHeight()
	{
		Vector3 tempVec3 = scoreBonusElectric.transform.localPosition;
		tempVec3.y = 0.0f;
		scoreBonusElectric.transform.localPosition = tempVec3;		
	}	
	
	public void StartMagicMagnet(){
		if(magicMagnet && magicMagnet.gameObject.active){
			ResetMagicMagnetHeight();
			magicMagnet.Play(true);
			ResetMagicMagnetHeight();		
		}
	}		

	public void StopMagicMagnet(){
		if(magicMagnet){
			//ResetMagicMagnetHeight();
			magicMagnet.Stop(true);
			magicMagnet.Clear(true);
		}
	}		
	
	public void ResetMagicMagnetHeight()
	{			
		if(GameProfile.SharedInstance.GetActiveCharacter().characterId == 3)
		{
			Vector3 tempVector = magicMagnetChinaGirlPosition;
			magicMagnet.transform.localPosition = tempVector;	
		}
		else	
		{
			Vector3 tempVector = magicMagnetOriginalPosition;
			magicMagnet.transform.localPosition = tempVector;	
		}
	}

	public void SetMagicMagnetSlideHeight()
	{
		Vector3 tempVec3 = magicMagnet.transform.localPosition;
		tempVec3.y = 0.3f;
		magicMagnet.transform.localPosition = tempVec3;		
	}	
	
	public void StartObjectGrab(){
		if(objectGrab && objectGrab.gameObject.active){
			objectGrab.Play(true);
		}
	}	
	
	int cur_shoot_id = 0;
	public IEnumerator ShootWandParticle(Transform target)
	{
		int my_id = ++cur_shoot_id;
		
		magicWand.Play();
		magicWand.transform.position = transform.position;
		
		Vector3 curPos = magicWand.transform.position;
		while(target!=null && curPos!=target.position && my_id==cur_shoot_id)
		{
			curPos = Vector3.MoveTowards(curPos,target.position,Time.deltaTime*50f);
			magicWand.transform.position = curPos;
			yield return null;
		}
		if(my_id==cur_shoot_id)
			magicWand.Stop();
	}
	
	public IEnumerator ShootWandParticle(Vector3 target)
	{
		int my_id = ++cur_shoot_id;
		
		magicWand.Play();
		magicWand.transform.position = transform.position;
		
		Vector3 curPos = magicWand.transform.position;
		while(curPos!=target && my_id==cur_shoot_id)
		{
			curPos = Vector3.MoveTowards(curPos,target,Time.deltaTime*50f);
			magicWand.transform.position = curPos;
			yield return null;
		}
		if(my_id==cur_shoot_id)
			magicWand.Stop();
	}
	
	public void DoOzDisappear()
	{
		if(ozDisappear!=null)
		{
			ozDisappear.Play(true);
		}
	}
	
}
