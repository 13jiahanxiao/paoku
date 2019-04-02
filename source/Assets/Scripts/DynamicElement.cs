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
/// The copyright notice above does NotEditable evidence any actual or intended publication or disclosure of this source code, which includes  
/// information that is confidential and/or proprietary, and is a trade secret, of COMPANY. ANY REPRODUCTION, MODIFICATION, DISTRIBUTION, PUBLIC  PERFORMANCE, 
/// OR PUBLIC DISPLAY OF OR THROUGH USE OF THIS SOURCE CODE WITHOUT THE EXPRESS WRITTEN CONSENT OFCOMPANY IS STRICTLY PROHIBITED, AND IN VIOLATION OF APPLICABLE 
/// LAWS AND INTERNATIONAL TREATIES. THE RECEIPT OR POSSESSION OF THIS SOURCE CODE AND/OR RELATED INFORMATION DOES NOT CONVEY OR IMPLY ANY RIGHTS  
/// TO REPRODUCE, DISCLOSE OR DISTRIBUTE ITS CONTENTS, OR TO MANUFACTURE, USE, OR SELL ANYTHING THAT IT MAY DESCRIBE, IN WHOLE OR IN PART.                
///

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicElement : MonoBehaviour {

	public SpawnPool Pool = null;
	public PrefabPool prefabPool = null;
	
	static public Dictionary<string, PrefabPool> loaded_prefabs = new Dictionary<string, PrefabPool>();
	
//		public static GameObject Instantiate(PathType type, PathLevel level = PathLevel.kPathLevelTemple)
//	{
//		if (PathElementPrefab == null)
//			PathElementPrefab = ((GameObject)Resources.Load("Prefabs/Temple/Path/PathElement")).transform;
//
//		SpawnPool pool = PoolManager.Pools["PathElements"];
//		GameObject go = pool.Spawn(PathElementPrefab).gameObject;
//		PathElement element = go.GetComponent<PathElement>();
//		element.Pool = pool;
//		element.SetPathType(type, level);
//		element.TileIndex = ++PathElement.sCount;
//		string newName = "PathElement: " + element.TileIndex;
//		go.name = newName;
//		return go;
//	}
	
	public GameObject SetTrackMesh(string fullPath)
	{
//		notify.Debug ("SetTrackMesh " + fullPath);
		ClearAllChildren();

		PrefabPool premadePool = null;
		GameObject go = null;
		if (loaded_prefabs.ContainsKey(fullPath))
			premadePool = loaded_prefabs[fullPath];
		
		if(premadePool == null) {
			return null;	
		}
		
		SpawnPool p = PoolManager.Pools["TrackMesh"];
		go = p.Spawn(premadePool.prefab).gameObject;//TODO: maybe defer the activate?
		DynamicElement de = go.GetComponent<DynamicElement>();
		if (de == null)
			de = go.AddComponent<DynamicElement>();
		de.Pool = p;
		de.prefabPool = premadePool;
		
		go.transform.parent = transform;
		go.transform.localPosition = new Vector3(0, 0, 0);

		return go;
	}

	public void ClearAllChildren()
	{
		if (transform.childCount == 0)
			return;
		
		//TR.LOG ("ClearAllChildren {0}", name);
		
		int max = transform.GetChildCount();
		DynamicElement de = null;
		for (int i = 0; i < max; i++) {
			de = transform.GetChild(i).GetComponent<DynamicElement>();
			if(de == null)
				continue;
			de.DestroySelf();
		}
	}

	public void DestroySelf()
	{
		if(Pool != null) {
			//TR.LOG ("DestroySelf {0}", name);
			transform.parent = Pool.transform;
			Pool.Despawn(transform, prefabPool);	
		}
	}

}
