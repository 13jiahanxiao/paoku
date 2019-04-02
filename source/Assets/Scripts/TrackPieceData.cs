using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TrackPieceData : MonoBehaviour {
	
	protected static Notify notify;
	
	//public List<Vector3>		GeneratedPath = null;
	public List<Transform>		PathLocations = null;
	public List<Transform>		AltPathLocations = null;
	//public List<Renderer>		TrackPieceRenderers = null;
	public List<Transform>		InvincibleObjects = null;
	public List<Transform>		InvincibleEffects = null;
	public List<Vector3>		CameraDirectionVectors = new List<Vector3>();
	public List<Vector3>		CameraPositionOffsets = new List<Vector3>();
	public float				EstimatedPathLength = 0f;
	public float				BoundingRadius = 0f;
	public int					VertCount = 0;
	public int					TriCount = 0;
	
	public float arcStartDist = 0f;
	
	public SplineNode splineStart;
	
	public int preloadAmount = 1;
	
	[System.NonSerialized]
	public PrefabPool			preFabPool = null;
	
	// Use this for initialization
	void Awake () {
		if ( notify == null)
		{
			notify = new Notify (this.GetType().Name);	
		}
	}
//	
//	void Start () {
//		TR.LOG("Start");
//	}
	
	
	private void FindChildrenWithName(string partialName, GameObject go, ref List<Transform> objects)
    {
       	// FindObjectsOfType
		if((go.name.ToLower().IndexOf(partialName.ToLower()) > -1) && go.GetComponent<MeshRenderer>() != null)
		{
			objects.Add(go.transform);
		}
		
        int count = go.transform.GetChildCount();
		Transform child = null;
		for(int i=0; i<count; i++) 
		{
			child = go.transform.GetChild(i);
			if(child == null)
				continue;
			
			FindChildrenWithName(partialName, child.gameObject, ref objects);
		}
    }
	
	private void FindChildrenWithName_ParticleRenderer(string partialName, GameObject go, ref List<Transform> objects)
    {
        if(go.name.Contains(partialName) == true && go.GetComponent<UnityEngine.ParticleSystem>() != null)
		{
			objects.Add(go.transform);
		}
		
		
        int count = go.transform.GetChildCount();
		Transform child = null;
		for(int i=0; i<count; i++) 
		{
			child = go.transform.GetChild(i);
			if(child == null)
				continue;
			
			FindChildrenWithName_ParticleRenderer(partialName, child.gameObject, ref objects);
		}
    }
	
	public float GetTrackPieceBounds()
	{
		return BoundingRadius;
	}
	
	public void GenerateTrackPieceBounds(GameObject importedModel)
	{
		Renderer[] renderers = importedModel.GetComponentsInChildren<Renderer>(true);
		MeshFilter[] meshes = importedModel.GetComponentsInChildren<MeshFilter>(true);
		Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
		bool boundsStarted = false;
		int triCount = 0;
		int vertCount = 0;
		if(meshes != null)
		{
			foreach(MeshFilter mesh in meshes)
			{
				if(mesh == null)
				{
					continue;
				}
				if(mesh.name.Contains("decals_lo"))
				{
					continue;
				}
				if(mesh.sharedMesh != null)
				{
					triCount += (mesh.sharedMesh.triangles.Length/3);
					vertCount += mesh.sharedMesh.vertexCount;
				}
			}
		}
		TriCount = triCount;
		VertCount = vertCount;
		if(renderers != null)
		{
			foreach(Renderer renderer in renderers)
			{
				if(renderer == null)
				{
					continue;
				}
				if(!renderer.name.Contains("force"))
				{
					if(boundsStarted == false)
					{
						boundsStarted = true;
						bounds.center = renderer.bounds.center;
						bounds.extents = renderer.bounds.extents;
					}
					else
					{
						bounds.Encapsulate(renderer.bounds.min);
						bounds.Encapsulate(renderer.bounds.center);
						bounds.Encapsulate(renderer.bounds.max);
					}
				}
			}			
		}
		
		BoundingRadius = bounds.max.magnitude;
		float minRadiusFrom0 = bounds.min.magnitude;
		if(minRadiusFrom0 > BoundingRadius)
		{
			BoundingRadius = minRadiusFrom0;
		}
		if(BoundingRadius > 35.0f)
			Debug.LogWarning("EXCESSIVE TRACKPIECE SIZE: " + importedModel.name + " radius " + BoundingRadius);
	}
	
	public void CreateData(GameObject importedModel)
	{
		//note deliberately using Debug.Log, as CreateData is called from Merlin's prefab lab, while the game is not running
		Debug.Log ("Track Data Create Data " + importedModel.name);
		
		if(PathLocations == null)
		{
			PathLocations = new List<Transform>();
		}
		
		PathLocations.Clear();
		
		GameObject pathObj = HierarchyUtils.GetChildByName("path_default", importedModel);
		if(pathObj != null)
		{
			FindPathLocations(pathObj, "dummy_{0}", ref PathLocations);
		}
		
		pathObj = HierarchyUtils.GetChildByName("path_left", importedModel);
		if(pathObj != null)
		{
			FindPathLocations(pathObj, "dummy_{0}_left", ref PathLocations);
		}
		
		pathObj = HierarchyUtils.GetChildByName("path_right", importedModel);
		if(pathObj != null)
		{
			if(AltPathLocations == null)
			{
				AltPathLocations = new List<Transform>();
			}
		
			AltPathLocations.Clear();
			FindPathLocations(pathObj, "dummy_{0}_right", ref AltPathLocations);
		}
		GenerateTrackPieceBounds (importedModel);
/*		
		if(TrackPieceRenderers == null)
		{
			TrackPieceRenderers = new List<Renderer>();
		}
		
		Renderer[] childRenderers = importedModel.GetComponentsInChildren<Renderer>();
		if(childRenderers != null)
		{
			TrackPieceRenderers.Clear();
			notify.Debug("Building TrackPieceRenderers for {0}", importedModel);
			foreach(Renderer r in childRenderers)
			{
				if(r == null)
					continue;
				r.castShadows = false;
				r.receiveShadows = false;

				if(r.sharedMaterial.name == "machu_master_opaque" || r.sharedMaterial.name == "machu_master_alpha" || r.sharedMaterial.name.StartsWith("oz_"))
				{
					if(TrackPieceRenderers == null)
					{
						TrackPieceRenderers = new List<Renderer>();
					}
					
					TrackPieceRenderers.Add(r);
				}
			}
		}
		else{
			notify.Warning ("Trackpiece has no renderers? {0}", importedModel);
		}
		*/
		// Estimate the path length
		EstimatedPathLength = 0.0f;
		
		//TR.LOG ("Estimating Path Length with {0} dummies.", PathLocations.Count);
		if (PathLocations != null && PathLocations.Count > 1) {
			Vector3 startPoint;
			Vector3 endPoint;
			
			for(int i = 1; i < PathLocations.Count; i++) {
				startPoint = PathLocations[i-1].position;
				endPoint = PathLocations[i].position;
				

				if(startPoint==endPoint)
				{
#if UNITY_EDITOR
					EditorUtility.DisplayDialog("Path Location Error","ERROR! Two path locations on '"+gameObject.name+"' are in the same place! This will cause a runtime crash, please fix!","Yes, Sir!");
#endif
					Debug.LogError("ERROR! Two path locations on '"+gameObject.name+"' are in the same place! This will cause a runtime crash, please fix!");
					
				}


				
				
				EstimatedPathLength += Vector3.Distance(startPoint, endPoint);
			}
		}
		
		InvincibleObjects = new List<Transform>();
		InvincibleEffects = new List<Transform>();
		FindChildrenWithName("inv_fill", this.gameObject, ref InvincibleObjects);
		FindChildrenWithName_ParticleRenderer("inv", this.gameObject, ref InvincibleEffects );
		
		Animation[] anims = gameObject.GetComponentsInChildren<Animation>();
		if(anims != null) {
			for (int i = 0; i < anims.Length; i++) {
				if(anims[i] == null)
					continue;
				
				if(anims[i].GetClipCount() == 0) {
					DestroyImmediate(anims[i]);
				}	
			}
		}
		Animation anim = gameObject.GetComponent<Animation>();
		if(anim != null && anim.GetClipCount() == 0) {
			DestroyImmediate(anim);
		}
	}
	
	private void FindPathLocations(GameObject go, string formatString, ref List<Transform> locations)
	{
		//-- Get PathLocations from Node
		int indexChar = 97; // "a"
		string dummyName = string.Format(formatString, (char)indexChar);
		Transform foundPathDummy = go.transform.Find(dummyName) as Transform;
		while(foundPathDummy != null)
		{
			locations.Add(foundPathDummy);
			
			indexChar++;
			dummyName = string.Format(formatString, (char)indexChar);
			foundPathDummy = go.transform.Find(dummyName) as Transform;
		}
	}
	
#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Vector3 pos = transform.position - transform.forward * arcStartDist;
		Gizmos.DrawLine(pos+transform.forward,pos-transform.forward);
		Gizmos.DrawLine(pos+transform.right,pos-transform.right);
	}
#endif
	
	
}
