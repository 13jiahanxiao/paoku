using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class Arrow : MonoBehaviour
{
	Vector3 StartPosition = Vector3.zero;
	Vector3 Speed = Vector3.zero;
	float TimeToHitTarget = 0f;
	float TimeToLive = 0f;
	float TimeSinceShot = 0f;
	EnemyTarget EnemyTarget = null;

	public float ArrowSpeed = 500f;


	private TrailRenderer mo_TrailRenderer;
	public TrailRenderer mo_TrailRendererPrefab;

	void Awake()
	{
		//DebugTools.Assert(mo_TrailRendererPrefab != null, string.Format("Arrow {0} has no mo_TrailRendererPrefab\n", gameObject.name));
	}

	public static Arrow Instanciate()
	{
		SimplePool pool = SimplePoolManager.Instance.GetPool("ArrowsPool");

		GameObject go = pool.Spawn(Attack.SharedInstance.GetProjectilePrefab().gameObject);
		Arrow arrow = go.GetComponent<Arrow>();

		return arrow;
	}

	public void DestroySelf()
	{
		SimplePool pool = SimplePoolManager.Instance.GetPool("ArrowsPool");
		pool.Unspawn(gameObject);
		GameObject.Destroy(mo_TrailRenderer);
	}

	public void Setup(GamePlayer Player, EnemyTarget Target)
	{
		Setup(Player, Target, Target.transform.position + Target.KillCenterOffset, Vector3.zero);
	}

	public void Setup(GamePlayer Player, Vector3 TargetPosition, Vector3 TargetSpeed)
	{
		Setup(Player, null, TargetPosition, TargetSpeed);
	}

	protected void Setup(GamePlayer Player, EnemyTarget Target, Vector3 TargetPosition, Vector3 TargetSpeed)
	{
		this.EnemyTarget = Target;
		this.StartPosition = GamePlayer.SharedInstance.transform.position + new Vector3(0f, 4f, 0f);
		Vector3 impact;
		this.Speed = CalculateDirection(ref StartPosition, GamePlayer.SharedInstance.GetPlayerVelocity(), ArrowSpeed, ref TargetPosition, ref TargetSpeed, out impact) * ArrowSpeed;
		this.Speed += GamePlayer.SharedInstance.GetPlayerVelocity(); // Adding the player's speed as intertia
		this.transform.forward = this.Speed.normalized;
		this.transform.position = StartPosition + (20.0f * Speed.normalized); ;

		this.TimeToLive = 1f;
		this.TimeSinceShot = 0f;
		this.TimeToHitTarget = (impact - StartPosition).magnitude / ArrowSpeed;

		mo_TrailRenderer = GameObject.Instantiate(mo_TrailRendererPrefab) as TrailRenderer;
		Tools.AttachObjectToTarget(mo_TrailRenderer.transform, transform);

		if (this.EnemyTarget != null)
			this.EnemyTarget.IsAimedByArrow = true;
	}

	void Update()
	{
		this.transform.position = this.transform.position + Speed * Attack.TimeScale.GetDeltaTime();
		//this.transform.position = this.transform.position + Speed * Time.deltaTime;
		
		TimeSinceShot += Attack.TimeScale.GetDeltaTime();
		//TimeSinceShot += Time.deltaTime;
		
		if (TimeSinceShot >= TimeToHitTarget && EnemyTarget != null)
		{
			DestroySelf();
			EnemyTarget.Kill();
		}

		if (TimeSinceShot >= TimeToLive)
		{
			DestroySelf();

			if (this.EnemyTarget != null)
				this.EnemyTarget.IsAimedByArrow = false;
		}
	}

	Vector3 CalculateDirection(ref Vector3 ShooterPosition, Vector3 ShooterSpeed, float ProjectileSpeed, ref Vector3 TargetPosition, ref Vector3 TargetSpeed, out Vector3 Impact)
	{
	    Vector3 relativeTargetVelocity = TargetSpeed - ShooterSpeed;
	    Vector3 VectorToTarget = TargetPosition - ShooterPosition;

		Impact = Vector3.zero;

		if (relativeTargetVelocity.sqrMagnitude == 0f)
			return VectorToTarget.normalized;
		
		float a = relativeTargetVelocity.sqrMagnitude - (ProjectileSpeed * ProjectileSpeed);
	    float b = 2f * Vector3.Dot(relativeTargetVelocity, VectorToTarget);
		float c = VectorToTarget.sqrMagnitude;

	    float det = b * b - 4f * a * c;

	    if (det < 0) // There is no solution
	        return Vector3.zero;
	    else
	    {
	        float timeA = (-b + (float)Math.Sqrt(det)) / (2f * a);
	        float timeB = (-b - (float)Math.Sqrt(det)) / (2f * a);
			
			float lowestPossibleTime = timeB;
			if (timeA > 0 && timeA < timeB)
				lowestPossibleTime = timeA;

			Impact = TargetPosition + relativeTargetVelocity * lowestPossibleTime;

			return (Impact - ShooterPosition).normalized;
	    }
	}
}