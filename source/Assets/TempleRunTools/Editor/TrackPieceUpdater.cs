using System.IO;
using UnityEditor;
using UnityEngine;

public class TrackPieceUpdater
{
	public void OnPostprocessPreFab(GameObject prefab)
    {
		TrackPieceData data = null;
		data = prefab.GetComponent<TrackPieceData>();
		if (data == null) { data = prefab.AddComponent<TrackPieceData>(); }
		data.CreateData(prefab);
    }
}
