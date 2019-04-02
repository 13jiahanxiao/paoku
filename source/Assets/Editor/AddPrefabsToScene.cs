using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class AddPrefabsToScene : MonoBehaviour 
{
	private const string kTempDirectoryPath = "Assets/Resources/temp";
	
	[MenuItem ("TR2/Optimization/Add WW Prefabs")]
	public static void AddWWPrefabsToScene()
	{
		AddPrefabsByDirectoryPath( "Assets/AssetBundleSource/whimsywoods" );	
	}
	
	[MenuItem ("TR2/Optimization/Add DF Prefabs")]
	public static void AddDFPrefabsToScene()
	{
		AddPrefabsByDirectoryPath( "Assets/AssetBundleSource/darkforest" );
	}
	
	[MenuItem ("TR2/Optimization/Add YBR Prefabs")]
	public static void AddYBRPrefabsToScene()
	{
		AddPrefabsByDirectoryPath( "Assets/AssetBundleSource/yellowbrickroad" );
	}
	
	[MenuItem ("TR2/Optimization/Add EC Prefabs")]
	public static void AddECPrefabsToScene()
	{
		AddPrefabsByDirectoryPath( "Assets/AssetBundleSource/emeraldcity" );
	}

	private static void AddPrefabsByDirectoryPath( string path )
	{
		// Iterate on all of the "*.prefab" files at "path"
		string[] filePaths = Directory.GetFiles( path, "*.prefab" );
		
        foreach ( string filePath in filePaths )
		{
			// Load the prefab and add it to the scene
			var asset = AssetDatabase.LoadAssetAtPath( filePath, typeof( GameObject ) );
			if ( asset != null )
			{
			//	Debug.Log( "Added " + filePath + " to the scene" );
				GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab( asset );
				obj.active = true;
			}
			else
			{
				Debug.Log( "ERROR: Couldn't load " + filePath );
			}
		}
	}
	
	/*[MenuItem ("TR2/Optimization/Animation Report")]
	private static void DoAnimationReport()
	{
		int verts = 0;
		int tris = 0;
		int uv = 0;
		int uv1 = 0;
		int uv2 = 0;
		int normals = 0;
		int tangents = 0;
		int colors = 0;
		int colors32 = 0;
		int boneweights = 0;
		int bindposes = 0;
		
		AnimationClip[] clips = (AnimationClip[])Resources.FindObjectsOfTypeAll(typeof(AnimationClip));
		
		foreach(AnimationState c in clips)
		{
			verts += m.vertices.Length;
			tris += m.triangles.Length;
			uv += m.uv.Length;
			uv1 += m.uv1.Length;
			uv2 += m.uv2.Length;
			normals += m.normals.Length;
			tangents += m.tangents.Length;
			colors += m.colors.Length;
			colors32 += m.colors32.Length;
			boneweights += m.boneWeights.Length;
			bindposes += m.bindposes.Length;
		}
		
		int memUsed = 	12 * verts +
						4 * tris +
						8 * uv +
						8 * uv1 +
						8 * uv2 +
						12 * normals +
						12 * tangents +
						16 * colors + 
						16 * colors32 + 
						32 * boneweights +
						64 * bindposes;
		
		string output = "";
		
		output += ("MESH REPORT - "+meshes.Length + " meshes found") + '\n';
		output += ("Verts: "+verts + " ("+(12*verts)+" bytes)") + '\n';
		output += ("Tris: "+tris + " ("+(4*tris)+" bytes)") + '\n';
		output += ("UV: "+uv + " ("+(8*uv)+" bytes)") + '\n';
		output += ("UV1: "+uv1 + " ("+(8*uv1)+" bytes)") + '\n';
		output += ("UV2: "+uv2 + " ("+(8*uv2)+" bytes)") + '\n';
		output += ("Normals: "+normals + " ("+(12*normals)+" bytes)") + '\n';
		output += ("Tangents: "+tangents + " ("+(12*tangents)+" bytes)") + '\n';
		output += ("Colors: "+colors + " ("+(16*colors)+" bytes)") + '\n';
		output += ("Colors32: "+colors32 + " ("+(16*colors32)+" bytes)") + '\n';
		output += ("Bone Weights: "+boneweights + " ("+(32*boneweights)+" bytes)") + '\n';
		output += ("Bind Poses: "+bindposes + " ("+(64*bindposes)+" bytes)") + '\n';
		output += ("------------------------") + '\n';
		output += ("Memory used: " + memUsed + " bytes (" + (((float)memUsed)/1000000f) + " MB)" ) + '\n';
		output += ("------------------------") + '\n';
		
		Debug.Log(output);
	}*/
	
	[MenuItem ("TR2/Optimization/Full Memory Report")]
	private static void DoMemoryReport()
	{
		int bytes = DoMeshReport() + DoTextureReport() + DoAnimationReport() + DoAudioReport();
		Debug.Log("TOTAL MEM USAGE FOR ART ASSETS: "+bytes+ " bytes (" + (((float)bytes)/1000000f) + " MB)" );
	}
	
	[MenuItem ("TR2/Optimization/Audio Report")]
	private static int DoAudioReport()
	{
		int totalsize = 0;
		
		
		string output = "";
		
		
		output += ("AUDIO REPORT") + '\n';
		
		foreach(AudioClip a in (AudioClip[])Resources.FindObjectsOfTypeAll(typeof(AudioClip)))
		{
			string clippath = AssetDatabase.GetAssetPath(a);
			
        	string guid = AssetDatabase.AssetPathToGUID(clippath);
			string p = Path.GetFullPath(Application.dataPath + "../../Library/cache/" + guid.Substring(0, 2) + "/" + guid);
	            
			if (File.Exists(p))
	        {
				FileInfo file = new FileInfo(p);
				long size = file.Length;
				output += a.name + " - " +size+ " bytes" + '\n';
				totalsize += (int)size;
			}
		}
		
		output += ("------------------------") + '\n';
		output += ("Memory used: " + totalsize + " bytes (" + (((float)totalsize)/1000000f) + " MB)" ) + '\n';
		output += ("------------------------") + '\n';
		
		Debug.Log(output);
		
		return totalsize;
	}
	
	[MenuItem ("TR2/Optimization/Animation Report")]
	private static int DoAnimationReport()
	{
		
		List<AnimationClip> animations = new List<AnimationClip>();
		
		int totalsize = 0;
		
		string output = "";
		
		output += ("ANIMATION REPORT") + '\n';
		
		foreach(Animation a in (Animation[])Object.FindSceneObjectsOfType(typeof(Animation)))
		{
			foreach (AnimationClip clip in AnimationUtility.GetAnimationClips(a))
			{
				if(clip!=null && !animations.Contains(clip))
				{
					int bytes = 0;
					foreach(AnimationClipCurveData data in AnimationUtility.GetAllCurves(clip))
					{
						bytes += data.curve.keys.Length * 16;
					}
					
					output += a.gameObject.name + " - " + clip.name + " - " + bytes + " bytes" + '\n';
					
					totalsize += bytes;
					
					animations.Add(clip);
				}
			}
		}
		
		SpawnEnemyFromPiece[] spawners = (SpawnEnemyFromPiece[])Object.FindSceneObjectsOfType(typeof(SpawnEnemyFromPiece));
		foreach(SpawnEnemyFromPiece spawn in spawners)
		{
			foreach(SpawnEnemyFromPiece.SpList list in spawn.SpawnPointLists)
			{
				foreach(GameObject prefab in list.prefabs)
				{
					foreach(Animation a in prefab.GetComponentsInChildren<Animation>(true))
					{
						foreach (AnimationClip clip in AnimationUtility.GetAnimationClips(a))
						{
							if(clip!=null && !animations.Contains(clip))
							{
								int bytes = 0;
								foreach(AnimationClipCurveData data in AnimationUtility.GetAllCurves(clip))
								{
									bytes += data.curve.keys.Length * 16;
								}
								
								output += a.gameObject.name + " - " + clip.name + " - " + bytes + " bytes" + '\n';
								
								totalsize += bytes;
								animations.Add(clip);
							}
						}
					}
				}
			}
		}
		
		
		output += ("------------------------") + '\n';
		output += ("Memory used: " + totalsize + " bytes (" + (((float)totalsize)/1000000f) + " MB)" ) + '\n';
		output += ("------------------------") + '\n';
		
		Debug.Log(output);
		
		return totalsize;
	}
	
	[MenuItem ("TR2/Optimization/Texture Report")]
	private static int DoTextureReport()
	{
		
		List<Texture2D> textures = new List<Texture2D>();
		
		int texturebytes = 0;
		
		string output = "";
		
		output += ("TEXTURE REPORT") + '\n';
		
		foreach(Renderer r in (Renderer[])Object.FindSceneObjectsOfType(typeof(Renderer)))
		{
			if(r.sharedMaterial!=null)
			{
				foreach (Object obj in EditorUtility.CollectDependencies(new UnityEngine.Object[] {r.sharedMaterial}))
				{
					Texture2D t = obj as Texture2D;
					if(t!=null)
					{
						if(t.name.Contains("_hi"))
							Debug.Log("Using hi texture!!! "+r.gameObject.name,r.gameObject);
						
						if(textures.Contains(t))
							continue;
						
						textures.Add(t);
						
						int tWidth=t.width;
						int tHeight=t.height;
						int bpp = GetBitsPerPixel(t.format);
						int mipMapCount = t.mipmapCount;
						int mipLevel = 1;
						int tSize = 0;
						while (mipLevel<=mipMapCount)
						{
							tSize+=tWidth*tHeight*bpp/8;
							tWidth=tWidth/2;
							tHeight=tHeight/2;
							mipLevel++;
						}
						texturebytes += tSize;
						output += t.name + " - " + t.width + "x" + t.height + " Size: "  + tSize + " bytes" + '\n';
					}
				}
			}
		}
		
		
		SpawnEnemyFromPiece[] spawners = (SpawnEnemyFromPiece[])Object.FindSceneObjectsOfType(typeof(SpawnEnemyFromPiece));
		foreach(SpawnEnemyFromPiece spawn in spawners)
		{
			foreach(SpawnEnemyFromPiece.SpList list in spawn.SpawnPointLists)
			{
				foreach(GameObject prefab in list.prefabs)
				{
					foreach(Renderer r in prefab.GetComponentsInChildren<Renderer>(true))
					{
						if(r.sharedMaterial!=null)
						{
							foreach (Object obj in EditorUtility.CollectDependencies(new UnityEngine.Object[] {r.sharedMaterial}))
							{
								Texture2D t = obj as Texture2D;
								if(t!=null)
								{
									if(t.name.Contains("_hi"))
										Debug.Log("Using hi texture!!! "+r.gameObject.name,r.gameObject);
									
									if(textures.Contains(t))
										continue;
									
									textures.Add(t);
									
									int tWidth=t.width;
									int tHeight=t.height;
									int bpp = GetBitsPerPixel(t.format);
									int mipMapCount = t.mipmapCount;
									int mipLevel = 1;
									int tSize = 0;
									while (mipLevel<=mipMapCount)
									{
										tSize+=tWidth*tHeight*bpp/8;
										tWidth=tWidth/2;
										tHeight=tHeight/2;
										mipLevel++;
									}
									texturebytes += tSize;
									output += t.name + " - " + t.width + "x" + t.height + " Size: "  + tSize + " bytes" + '\n';
								}
							}
						}
					}
				}
			}
		}
		
		
		output += ("------------------------") + '\n';
		output += ("Memory used: " + texturebytes + " bytes (" + (((float)texturebytes)/1000000f) + " MB)" ) + '\n';
		output += ("------------------------") + '\n';
		
		Debug.Log(output);
		
		return texturebytes;
	}
	
	[MenuItem ("TR2/Optimization/Mesh Report")]
	private static int DoMeshReport()
	{
		int verts = 0;
		int tris = 0;
		int uv = 0;
		int uv1 = 0;
		int uv2 = 0;
		int normals = 0;
		int tangents = 0;
		int colors = 0;
		int colors32 = 0;
		int boneweights = 0;
		int bindposes = 0;
		
		
		string output = "";
		
		output += ("MESH REPORT") + '\n';
		
		//Mesh[] meshes = (Mesh[])Object.FindSceneObjectsOfType(typeof(Mesh));
		List<Mesh> meshes = new List<Mesh>();
		
		foreach(MeshFilter filter in (MeshFilter[])Object.FindSceneObjectsOfType(typeof(MeshFilter)))
		{
			if(filter.sharedMesh!=null && !meshes.Contains(filter.sharedMesh))
			{
				meshes.Add(filter.sharedMesh);
			}
		}
		foreach(SkinnedMeshRenderer skinned in (SkinnedMeshRenderer[])Object.FindSceneObjectsOfType(typeof(SkinnedMeshRenderer)))
		{
			if(skinned.sharedMesh!=null && !meshes.Contains(skinned.sharedMesh))
				meshes.Add(skinned.sharedMesh);
		}
		
		
		List<Mesh> counted = new List<Mesh>();
		
		foreach(Mesh m in meshes)
		{
			if(!counted.Contains(m))
			{
				verts += m.vertices.Length;
				tris += m.triangles.Length;
				uv += m.uv.Length;
				uv1 += m.uv1.Length;
				uv2 += m.uv2.Length;
				normals += m.normals.Length;
				tangents += m.tangents.Length;
				colors += m.colors.Length;
				colors32 += m.colors32.Length;
				boneweights += m.boneWeights.Length;
				bindposes += m.bindposes.Length;
				counted.Add(m);
				
		
		int mem = 	12 * m.vertices.Length +
						4 * m.triangles.Length +
						8 * m.uv.Length +
						8 * m.uv1.Length +
						8 * m.uv2.Length +
						12 * m.normals.Length +
						12 * m.tangents.Length +
						16 * m.colors.Length + 
						16 * m.colors32.Length + 
						32 * m.boneWeights.Length +
						64 * m.bindposes.Length;
				//+ counted.Count * 24;	//for bounds
				output += m + " - " + mem  + " bytes" + '\n';
				
			//	if(m.normals.Length>0)
			//		Debug.Log(m.name,m);
			}
		}
		
		
		SpawnEnemyFromPiece[] spawners = (SpawnEnemyFromPiece[])Object.FindSceneObjectsOfType(typeof(SpawnEnemyFromPiece));
		
		foreach(SpawnEnemyFromPiece spawn in spawners)
		{
			foreach(SpawnEnemyFromPiece.SpList list in spawn.SpawnPointLists)
			{
				foreach(GameObject prefab in list.prefabs)
				{
					foreach(MeshFilter filter in prefab.GetComponentsInChildren<MeshFilter>(true))
					{
						Mesh m = filter.sharedMesh;
						if(m!=null && !counted.Contains(m))
						{
							verts += m.vertices.Length;
							tris += m.triangles.Length;
							uv += m.uv.Length;
							uv1 += m.uv1.Length;
							uv2 += m.uv2.Length;
							normals += m.normals.Length;
							tangents += m.tangents.Length;
							colors += m.colors.Length;
							colors32 += m.colors32.Length;
							boneweights += m.boneWeights.Length;
							bindposes += m.bindposes.Length;
							counted.Add(m);
		
		int mem = 	12 * m.vertices.Length +
						4 * m.triangles.Length +
						8 * m.uv.Length +
						8 * m.uv1.Length +
						8 * m.uv2.Length +
						12 * m.normals.Length +
						12 * m.tangents.Length +
						16 * m.colors.Length + 
						16 * m.colors32.Length + 
						32 * m.boneWeights.Length +
						64 * m.bindposes.Length;
				//+ counted.Count * 24;	//for bounds
				output += m + " - " + mem  + " bytes"+ '\n';
						}
					}
					foreach(SkinnedMeshRenderer skinned in prefab.GetComponentsInChildren<SkinnedMeshRenderer>(true))
					{
						Mesh m = skinned.sharedMesh;
						if(m!=null && !counted.Contains(m))
						{
							verts += m.vertices.Length;
							tris += m.triangles.Length;
							uv += m.uv.Length;
							uv1 += m.uv1.Length;
							uv2 += m.uv2.Length;
							normals += m.normals.Length;
							tangents += m.tangents.Length;
							colors += m.colors.Length;
							colors32 += m.colors32.Length;
							boneweights += m.boneWeights.Length;
							bindposes += m.bindposes.Length;
							counted.Add(m);
		
		int mem = 	12 * m.vertices.Length +
						4 * m.triangles.Length +
						8 * m.uv.Length +
						8 * m.uv1.Length +
						8 * m.uv2.Length +
						12 * m.normals.Length +
						12 * m.tangents.Length +
						16 * m.colors.Length + 
						16 * m.colors32.Length + 
						32 * m.boneWeights.Length +
						64 * m.bindposes.Length;
				//+ counted.Count * 24;	//for bounds
				output += m + " - " + mem  + " bytes" + '\n';
						}
					}
				}
			}
		}
		
		int memUsed = 	12 * verts +
						4 * tris +
						8 * uv +
						8 * uv1 +
						8 * uv2 +
						12 * normals +
						12 * tangents +
						16 * colors + 
						16 * colors32 + 
						32 * boneweights +
						64 * bindposes
				+ counted.Count * 24;	//for bounds
		
		output += ("MESH REPORT - "+counted.Count + " meshes found") + '\n';
		output += ("Verts: "+verts + " ("+(12*verts)+" bytes)") + '\n';
		output += ("Tris: "+tris + " ("+(4*tris)+" bytes)") + '\n';
		output += ("UV: "+uv + " ("+(8*uv)+" bytes)") + '\n';
		output += ("UV1: "+uv1 + " ("+(8*uv1)+" bytes)") + '\n';
		output += ("UV2: "+uv2 + " ("+(8*uv2)+" bytes)") + '\n';
		output += ("Normals: "+normals + " ("+(12*normals)+" bytes)") + '\n';
		output += ("Tangents: "+tangents + " ("+(12*tangents)+" bytes)") + '\n';
		output += ("Colors: "+colors + " ("+(16*colors)+" bytes)") + '\n';
		output += ("Colors32: "+colors32 + " ("+(4*colors32)+" bytes)") + '\n';
		output += ("Bone Weights: "+boneweights + " ("+(32*boneweights)+" bytes)") + '\n';
		output += ("Bind Poses: "+bindposes + " ("+(64*bindposes)+" bytes)") + '\n';
		output += ("------------------------") + '\n';
		output += ("Memory used: " + memUsed + " bytes (" + (((float)memUsed)/1000000f) + " MB)" ) + '\n';
		output += ("------------------------") + '\n';
		
		Debug.Log(output);
		
		return memUsed;
	}
	
	static int GetBitsPerPixel(TextureFormat format)
	{
		switch (format)
		{
			case TextureFormat.Alpha8: //	 Alpha-only texture format.
				return 8;
			case TextureFormat.ARGB4444: //	 A 16 bits/pixel texture format. Texture stores color with an alpha channel.
				return 16;
			case TextureFormat.RGB24:	// A color texture format.
				return 24;
			case TextureFormat.RGBA32:	//Color with an alpha channel texture format.
				return 32;
			case TextureFormat.ARGB32:	//Color with an alpha channel texture format.
				return 32;
			case TextureFormat.RGB565:	//	 A 16 bit color texture format.
				return 16;
			case TextureFormat.DXT1:	// Compressed color texture format.
				return 4;
			case TextureFormat.DXT5:	// Compressed color with alpha channel texture format.
				return 8;
			/*
			case TextureFormat.WiiI4:	// Wii texture format.
			case TextureFormat.WiiI8:	// Wii texture format. Intensity 8 bit.
			case TextureFormat.WiiIA4:	// Wii texture format. Intensity + Alpha 8 bit (4 + 4).
			case TextureFormat.WiiIA8:	// Wii texture format. Intensity + Alpha 16 bit (8 + 8).
			case TextureFormat.WiiRGB565:	// Wii texture format. RGB 16 bit (565).
			case TextureFormat.WiiRGB5A3:	// Wii texture format. RGBA 16 bit (4443).
			case TextureFormat.WiiRGBA8:	// Wii texture format. RGBA 32 bit (8888).
			case TextureFormat.WiiCMPR:	//	 Compressed Wii texture format. 4 bits/texel, ~RGB8A1 (Outline alpha is not currently supported).
				return 0;  //Not supported yet
			*/
			case TextureFormat.PVRTC_RGB2://	 PowerVR (iOS) 2 bits/pixel compressed color texture format.
				return 2;
			case TextureFormat.PVRTC_RGBA2://	 PowerVR (iOS) 2 bits/pixel compressed with alpha channel texture format
				return 2;
			case TextureFormat.PVRTC_RGB4://	 PowerVR (iOS) 4 bits/pixel compressed color texture format.
				return 4;
			case TextureFormat.PVRTC_RGBA4://	 PowerVR (iOS) 4 bits/pixel compressed with alpha channel texture format
				return 4;
			case TextureFormat.ETC_RGB4://	 ETC (GLES2.0) 4 bits/pixel compressed RGB texture format.
				return 4;
			case TextureFormat.ATC_RGB4://	 ATC (ATITC) 4 bits/pixel compressed RGB texture format.
				return 4;
			case TextureFormat.ATC_RGBA8://	 ATC (ATITC) 8 bits/pixel compressed RGB texture format.
				return 8;
			case TextureFormat.BGRA32://	 Format returned by iPhone camera
				return 32;
			case TextureFormat.ATF_RGB_DXT1://	 Flash-specific RGB DXT1 compressed color texture format.
			case TextureFormat.ATF_RGBA_JPG://	 Flash-specific RGBA JPG-compressed color texture format.
			case TextureFormat.ATF_RGB_JPG://	 Flash-specific RGB JPG-compressed color texture format.
				return 0; //Not supported yet
		}
		return 0;
	}
}
