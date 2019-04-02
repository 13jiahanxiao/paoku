using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingObject : MonoBehaviour
{
	public class MovingObjectPathNode
	{
		public float TimeStamp;
		public Vector3 Location;
		public bool Ease;

		public override string ToString()
		{
			return "PN(" + TimeStamp + " " + Location + " " + Ease + ")";
		}
	}


	public float Mass = 1;
	public bool HasForce;
	public Vector3 Force;
	public Vector3 Velocity;
	public Vector3 RotationAxis = new Vector3(0,1,0);
	public float AngularVelocity;
	public bool LimitVelocity;
	public float MaxVelocityMagnitudeTotal = -1;
	public float MaxVelocityMagnitudeX = -1;
	public float MaxVelocityMagnitudeY = -1;
	public float MaxVelocityMagnitudeZ = -1;

	public bool IsFollowingPath = false;
	public float TimeSinceFollowingPathStart = 0;																 
	public List<MovingObject.MovingObjectPathNode> FollowingPathNodes;

	// Simulation variables
	// DMW May be redudnant with Unity's time capabilities
	float TimePerFrame;
	//float LastFrameChangeTime;
	float LastUpdateTime;


	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	virtual public void Update()
	{
		Simulate();
	}


	public bool IsMoving()
	{
		if (IsFollowingPath && FollowingPathNodes != null && FollowingPathNodes.Count > 1)
			return true;
		
		if (Velocity.sqrMagnitude == 0 && AngularVelocity == 0)
			return false;
		
		return true;
	}



	public void AddFollowingPathNode(MovingObjectPathNode node)
	{
		if (FollowingPathNodes == null)
			FollowingPathNodes = new List<MovingObjectPathNode>();
		
		FollowingPathNodes.Add(node);
		
	}
	
	public void AddFollowingPathNode(float timeStamp,Vector3 location,bool ease) {
		MovingObjectPathNode node = new MovingObjectPathNode() {
			TimeStamp = timeStamp,
			Location = location,
			Ease = ease
		};
		AddFollowingPathNode(node);
	}

	public void StartFollowingPath()
	{
		TimeSinceFollowingPathStart = 0;
		IsFollowingPath = true;
		
	}

	public void StopFollowingPath()
	{
		IsFollowingPath = false;
	}

	public void ClearFollowingPath()
	{
		IsFollowingPath = false;
		if (FollowingPathNodes != null)
			FollowingPathNodes.Clear();
	}
	
	
	public void ApplyForce(Vector3 force)
	{
		Force += force;
		HasForce = true;
	}
	
	public void ResetForce()
	{
		Force = new Vector3(0, 0, 0);
		HasForce = false;
	}


	// Simulation

	public void ResetSimulation()
	{
		TimeSinceFollowingPathStart = 0;
		IsFollowingPath = false;
		HasForce = false;
		Force = new Vector3(0, 0, 0);
		Velocity = new Vector3(0, 0, 0);
		AngularVelocity = 0;
		
		LastUpdateTime = 0;
		//LastFrameChangeTime = 0;
	}

	public void Simulate()
	{
		
		LastUpdateTime += Time.deltaTime;
		
		// Impulse force
		if (HasForce) {
			Velocity += (Force * Time.deltaTime);
			Force = new Vector3(0, 0, 0);
			HasForce = false;
		}
		
		// If the object is moving simualte it
		if (IsMoving()) {
			
			// Handle path follow
			if (IsFollowingPath && FollowingPathNodes != null && FollowingPathNodes.Count > 1) {
				TimeSinceFollowingPathStart += Time.deltaTime;
				
				// Find start node
				bool isOnPath = false;
				MovingObjectPathNode startNode = null;
				MovingObjectPathNode endNode = null;
				
				for (int nodeIndex = 0; nodeIndex < FollowingPathNodes.Count-1; ++nodeIndex) {
					MovingObjectPathNode nodeA = FollowingPathNodes[nodeIndex];
					MovingObjectPathNode nodeB = FollowingPathNodes[nodeIndex + 1];
					
					
					if (nodeIndex == 0)
						startNode = nodeA;
					
					endNode = nodeB;
					
					if (TimeSinceFollowingPathStart >= nodeA.TimeStamp && TimeSinceFollowingPathStart <= nodeB.TimeStamp) {
						startNode = nodeA;
						isOnPath = true;
						break;
					}
				}

				
				if (isOnPath) {
					
					Vector3 direction = endNode.Location - startNode.Location;
					float t = 1.0f;
					
					if (startNode.TimeStamp != endNode.TimeStamp) {
						t = (TimeSinceFollowingPathStart - startNode.TimeStamp) / (endNode.TimeStamp - startNode.TimeStamp);
					}
					
					// Re-calculate the percent along if using easing
					
					if (startNode.Ease && !endNode.Ease)
						// Ease In
						t = (t * t * t); else if (!startNode.Ease && endNode.Ease) {
						// Ease Out
						float adjT = t - 1.0f;
						t = (adjT * adjT * adjT) + 1.0f;
					} else if (startNode.Ease && endNode.Ease) {
						// Ease InOut
						if (t < 0.5f) {
							float adjT = t * 2.0f;
							t = 0.5f * (adjT * adjT * adjT);
						} else {
							float adjT = t * 2.0f - 2.0f;
							t = 0.5f * ((adjT * adjT * adjT) + 2.0f);
						}
					}

					//TR.LOG("Time: " + TimeSinceFollowingPathStart + " Start: " + startNode + "   End: " + endNode + "  T: " + t);

					
					transform.position = startNode.Location + (direction * t);
				
				} else {
					
					if (startNode != null && (TimeSinceFollowingPathStart < startNode.TimeStamp || endNode == null)) {
						// Put object at beginning of path
						transform.position = startNode.Location;
						IsFollowingPath = false;
					} else if (endNode != null) {
						// Put object at end of path
						transform.position = endNode.Location;
						IsFollowingPath = false;
					} else {
						// Do nothing (no path to follow)
						IsFollowingPath = false;
					}
				}
			}
			else {
				// We are not following a path

				// Limit velocity if necessary
				if (LimitVelocity) {
					
					if (MaxVelocityMagnitudeTotal >= 0 && Velocity.magnitude > MaxVelocityMagnitudeTotal) {
						Velocity.Normalize();
						Velocity *= MaxVelocityMagnitudeTotal;
					}
					
					// Limit X velocity
					if (MaxVelocityMagnitudeX >= 0 && Mathf.Abs(Velocity.x) > MaxVelocityMagnitudeX) {
						if (Velocity.x < 0)
							Velocity.x = -MaxVelocityMagnitudeX;
						if (MaxVelocityMagnitudeX > 0)
							Velocity.x = MaxVelocityMagnitudeX;
					}
					
					// Limit Y velocity
					if (MaxVelocityMagnitudeY >= 0 && Mathf.Abs(Velocity.y) > MaxVelocityMagnitudeY) {
						if (Velocity.y < 0)
							Velocity.y = -MaxVelocityMagnitudeY;
						if (MaxVelocityMagnitudeY > 0)
							Velocity.y = MaxVelocityMagnitudeY;
					}
					
					// Limit Z velocity
					if (MaxVelocityMagnitudeZ >= 0 && Mathf.Abs(Velocity.z) > MaxVelocityMagnitudeZ) {
						if (Velocity.z < 0)
							Velocity.z = -MaxVelocityMagnitudeZ;
						if (MaxVelocityMagnitudeZ > 0)
							Velocity.z = MaxVelocityMagnitudeZ;
					}
				
				}
			
				if(Velocity != Vector3.zero)
					transform.Translate(Velocity * Time.deltaTime,Space.World);
				if (AngularVelocity != 0)
					transform.RotateAround(RotationAxis, AngularVelocity * Time.deltaTime);
			
				
			}
		}
	}
	
	
	public virtual void SetVisibility(bool visible)
	{
		Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
		foreach (Renderer r in renderers) {
			r.enabled = visible;
		}
//		UISprite[] ss = GetComponentsInChildren<UISprite>();
//		foreach (UISprite s in ss) {
//			s.enabled = visible;
//		}
	}


	public void SetX(float x)
	{
		transform.position = new Vector3(x, transform.position.y, transform.position.z);
	}

	public void SetY(float y)
	{
		transform.position = new Vector3(transform.position.x, y,transform.position.z);
	}

	public void SetZ(float z)
	{
		transform.position = new Vector3(transform.position.x, transform.position.y,z);
	}

	public void SetLocalX(float x)
	{
		transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
	}

	public void SetLocalY(float y)
	{
		transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
	}

	public void SetLocalZ(float z)
	{
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
	}

	public bool DoesSphereIntersectBoundingBox(Vector3 point, float radius)
	{
		Bounds b = GetWorldSpaceBounds();
		float d = b.SqrDistance(point);
		return (d <= (radius * radius));

	}

	public virtual Bounds GetWorldSpaceBounds()
	{
		return transform.GetComponentInChildren<Renderer>().bounds;
	}

	public Vector2 GetPosition2D()
	{
		return new Vector2(transform.position.x, transform.position.z);
	}


}





