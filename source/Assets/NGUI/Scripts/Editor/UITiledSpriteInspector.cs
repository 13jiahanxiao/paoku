//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

/// <summary>
/// Inspector class used to edit UITiledSprites.
/// </summary>

[CustomEditor(typeof(UITiledSprite))]
public class UITiledSpriteInspector : UISlicedSpriteInspector
{
	/// <summary>
	/// Draw the tiling vector fields.
	/// </summary>

	override protected bool OnDrawProperties ()
	{
		if (base.OnDrawProperties())
		{
			UITiledSprite sp = mSprite as UITiledSprite;
			Vector2 tiling = EditorGUILayout.Vector2Field("Tiling", sp.tiling);
			if ((sp.tiling.x != tiling.x)||(sp.tiling.y != tiling.y))
			{
				Debug.Log("Setting dirty");
				NGUIEditorTools.RegisterUndo("Sprite Change", sp);
				sp.tiling = tiling;
				EditorUtility.SetDirty(sp.gameObject);
			}
			return true;
		}
		return false;
	}
			
}