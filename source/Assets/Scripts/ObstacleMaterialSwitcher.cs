using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleMaterialSwitcher : MonoBehaviour {
	
	public Material opaque;
	public Material opaque_fade;
	public Material decal;
	public Material decal_fade;
	
	private bool bUsingOpaque;
	private Transform CachedTransform;
	private Vector3 CachedPosition;
	public void CacheData()
	{
		CachedPosition = CachedTransform.position;
		UpdateRenderers();
		dataCached = true;
	}
	private bool dataCached = false;
	
	private List<Renderer>		OpaqueRenderers = null;
	private List<Renderer>		DecalRenderers = null;

	// Use this for initialization
	void Start () {

		CachedTransform = transform;
//		ResetRendererLists();
		//UpdateRenderers();
	}
	

	void OnEnable()
	{
		dataCached = false;
	}
	
	void OnDisable()
	{
		dataCached = false;
	}
	
	private void ResetRendererLists()
	{
		//Set all track piece renderers to opaque OR transparent
		if(OpaqueRenderers!=null)	
		{
			OpaqueRenderers.Clear();
		}
		else
		{
			OpaqueRenderers = new List<Renderer>();
		}
		if(DecalRenderers!=null)	
		{
			DecalRenderers.Clear();
		}
		else
		{
			DecalRenderers = new List<Renderer>();
		}
		
	}	
	
	private void AddRendererToList(Renderer r)
	{
		bool isDecal = r.gameObject.name.Contains("decals");
		
		//Ensure that any decals are on the "decal" layer (ignores shadow)
		if(isDecal)	
		{
			r.gameObject.layer = LayerMask.NameToLayer("decals");
		}
		//if(r.gameObject.tag != "DontChangeMat")
		{
			if(isDecal)
			{
				DecalRenderers.Add(r);
			}
			else
			{
				OpaqueRenderers.Add(r);
			}				
		}
	}
	
	public void UpdateRenderers()
	{
		ResetRendererLists();
		bUsingOpaque = false;
		
		GameObject rendererParent = gameObject;
		
		foreach(Renderer r in rendererParent.GetComponentsInChildren<SkinnedMeshRenderer>(true))
		{
			AddRendererToList (r);
		}
		foreach(Renderer r in rendererParent.GetComponentsInChildren<MeshRenderer>(true))
		{
			AddRendererToList (r);
		}
		//now we have the lists of renderers set them to the appropriate state
		UpdateRendererMaterials(bUsingOpaque);
	}
	
	private void UpdateRendererMaterials(bool useOpaque)
	{	

		Material opaqueMat = null;
		Material decalMat = null;
		if(useOpaque)
		{
			opaqueMat = (opaque!=null)?opaque:GameController.SharedInstance.TrackMaterials.Opaque;
			decalMat = (decal!=null)?decal:GameController.SharedInstance.TrackMaterials.Decal;
		}
		else
		{
			opaqueMat = (opaque_fade!=null)?opaque_fade:GameController.SharedInstance.TrackFadeMaterials.Opaque;
			decalMat = (decal_fade!=null)?decal_fade:GameController.SharedInstance.TrackFadeMaterials.Decal;
		}
		if((opaqueMat != null)&&(OpaqueRenderers != null))
		{
			for(int i=0;i<OpaqueRenderers.Count;i++)
			{
				Renderer r = OpaqueRenderers[i];
				
				if(r == null)
					continue;
				
				r.material = opaqueMat;
			}
		}
		if((decalMat != null) && (DecalRenderers != null))
		{
			for(int i=0;i<DecalRenderers.Count;i++)
			{
				Renderer r = DecalRenderers[i];
				
				if(r == null)
					continue;

				r.material = decalMat;
			}
		}
		bUsingOpaque = useOpaque;
	}
	
	
	// Update is called once per frame
//	void Update () {
//	
//	}
	
	public void LateUpdate()
	{
		//-- Get distance from Camera. change materials if need to be blended.
		if((GameController.SharedInstance == null) || (GameController.SharedInstance.Player == null) || (GameCamera.SharedInstance == null) || (GameCamera.SharedInstance.CachedTransform == null))
			return;
		
		if(dataCached == false)
		{
			CacheData ();
		}
		
		bool useOpaque = false;
		
		float boundingRadius = 5f;
		
		if(GameController.SharedInstance.Player.OnTrackPiece != this)
		{
			if(boundingRadius < GameController.SharedInstance.AlphaCullDistance)
			{
				float distSqr = Vector3.SqrMagnitude(GameCamera.SharedInstance.CachedTransform.position-CachedPosition);
				float testDistSqr = GameController.SharedInstance.AlphaCullDistance - boundingRadius;
				testDistSqr *= testDistSqr;
				useOpaque = (distSqr < testDistSqr);
			}
			else
			{
				useOpaque = false;
			}
		}
		else
		{
			useOpaque = true;
		}
		
		if(bUsingOpaque!=useOpaque)
		{
			UpdateRendererMaterials(useOpaque);
		}
	}
}
