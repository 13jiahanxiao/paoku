using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour {
	
	public Transform Target;
	
	public Vector3 offset = Vector3.down;
	
	private bool isSet = false;
	
	void OnEnable()
	{
		GamePlayer p = GameController.SharedInstance.Player;

		if(p!=null)
		{
			transform.position = p.CurrentPosition + p.transform.TransformDirection(offset);
			transform.rotation = p.transform.rotation;
		}
		stopped = false;
	}
	
	private bool stopped = false;
	private float internal_vel = 0f;
	public void Stop()
	{
		stopped = true;
		internal_vel = GamePlayer.SharedInstance.GetPlayerVelocity().magnitude;
	}
	
	public void Unstop()
	{
		stopped = false;
	}
	
	void LateUpdate()
	{
		GamePlayer p = GameController.SharedInstance.Player;
		
		if(stopped)
		{
			internal_vel = Mathf.MoveTowards(internal_vel,0f,10f*Time.deltaTime);
			Vector3 frw = transform.forward;
			frw.y = 0f;
			frw.Normalize();
			transform.position += frw * internal_vel*Time.deltaTime;
		}

		else if(p!=null)
		{
			if(p.OnTrackPiece!=null)
			{
				if(!isSet)
				{
					transform.position = p.CurrentPosition + p.transform.TransformDirection(offset);//p.transform.position - p.distanceAlongRight*p.transform.right+p.transform.TransformDirection(offset);
					transform.rotation = p.transform.rotation;
					isSet = true;
				}
				else
				{
					transform.position = Vector3.Lerp(transform.position, p.CurrentPosition + p.transform.TransformDirection(offset), Time.deltaTime*50f);
					transform.rotation = Quaternion.Slerp(transform.rotation,p.transform.rotation,Time.deltaTime*10f);
				}
			}
		}
	}
}
