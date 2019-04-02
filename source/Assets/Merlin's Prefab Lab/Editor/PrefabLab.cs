using System.IO;
using UnityEditor;
using UnityEngine;

/*----------------------------------
 * Description
 * ---------------------------------
 * This importer creates prefabs for all imported models.
 * This way we can preserve our prefabs AND keep them up-to-date with their models
 * The prefabs are named "modelname" + 'PrefabNameExtension'
 * You should set the Source and Destination Folder to what you want for each project!
 * Use the source and dest folders to separate 3dmax files from the prefabs 
 * ---------------------------------
 * Code Flow
 * ---------------------------------
 * When importing a model into Unity,
 * this importer does the following:
 * - Check if the imported object should be used with this importer
 * - Check if a prefab of the model already exists. If not, create the prefab
 * - Search in the prefab for the model
 * - Sync with the newly imported model
 * - Preserve the prefab (Keep all usergenerated content like ParticleSystems, Lights, Scripts etc. Basicly every Component)
 * - Update the prefab to apply the changes
 * 
 * !!! This importer uses MetaData scripts to store information about the model.
 * !!! We need this, there is no other way to check if a gameobject is new, old or user generated content
 */

public class PrefabUpdater : AssetPostprocessor  
{
    private string NewModelNameSuffix = "_new";
    private string CurrentModelNameSuffix;
    private string lostObjectsName = "Lost GameObjects";

    public void OnPostprocessModel(GameObject root)
    {
        //Read PrefabUpdater settings
        PrefabLabSettings settings = PrefabLabSettings.Defaults;
        settings.ReadSettingsFromDisk();

        CurrentModelNameSuffix = settings.ModelMetadata;

        //Skip imports outside the sourcefolder
        if (assetPath.IndexOf("Assets/" + settings.SourceFolder) == -1)
        {
            return;
        }

        //Substract subdirs from imported asset
        string relativeSubdir = assetPath.Remove(0, ("Assets" + settings.SourceFolder).Length + 1);
        //Debug.Log(relativeSubdir);
        relativeSubdir = relativeSubdir.Remove(relativeSubdir.LastIndexOf("/") + 1);
        //Debug.Log(relativeSubdir);
        string relativeSourceFolder = settings.DestinationFolder + "/" + relativeSubdir;
        //Debug.Log(relativeSourceFolder);
        string absoluteSubdir = Application.dataPath + "/" + relativeSourceFolder;
        //Debug.Log(absoluteSubdir);

        //Prepare the imported model for modification
        //Instantiate root as we don't want to break it for other importers!!!
        GameObject importedModel = (GameObject)GameObject.Instantiate(root);
        importedModel.name = root.name;
        AddMetaData(importedModel);

        GameObject lostObjects = null;

        Object originalPrefab;
        if (!FindExistingPrefab(root.name, relativeSourceFolder, settings.PrefabSuffix, out originalPrefab))
        {
            //Create subdirs
            Directory.CreateDirectory(absoluteSubdir);

            AssetDatabase.Refresh();

            //Prefab not found, create one for me please!
            string prefabFilepath = "Assets/" + relativeSourceFolder + root.name + settings.PrefabSuffix + ".prefab";
            Object emptyPrefab = PrefabUtility.CreateEmptyPrefab(prefabFilepath);
            AssetDatabase.ImportAsset(prefabFilepath);

            //Creating an empty prefab is really empty so we need a gameobject to start with, called parent
            GameObject parent = new GameObject(root.name + settings.PrefabSuffix);

            //Mark as max model
            AddNameSuffixRecursively(importedModel, CurrentModelNameSuffix);
            //Copy newModel to prefab
            importedModel.transform.parent = parent.transform;

            TrackPieceUpdater foo = new TrackPieceUpdater();
			foo.OnPostprocessPreFab(parent);
			
			originalPrefab = EditorUtility.ReplacePrefab(parent, emptyPrefab, ReplacePrefabOptions.ReplaceNameBased);
			
            //Clean up
            GameObject.DestroyImmediate(parent);
        }
        else
        {
            GameObject prefab = (GameObject)GameObject.Instantiate(originalPrefab);
            prefab.name = originalPrefab.name;

            //Find LostObjects root or create it
            //lostObjects = HierarchyUtils.GetChildByName(lostObjectsName, prefab);
			Transform lost = prefab.transform.FindChild(lostObjectsName);
			if(lost!=null)
				lostObjects = lost.gameObject;	

            if (lostObjects == null)
            {
                lostObjects = new GameObject(lostObjectsName);
                lostObjects.transform.parent = prefab.transform;
            }

            //Mark the newly imported as new to know later on if a modelpart has just been imported or its an old prefabpart
            AddNameSuffixRecursively(importedModel, NewModelNameSuffix);

            //Find newModel root in merlin's prefab and do magic
            //GameObject prefabModelRoot = HierarchyUtils.GetChildByName(importedModel.name, prefab);
			Transform tr = prefab.transform.FindChild(importedModel.name);
			if(tr==null)	{ Debug.LogError( "ERROR!!! "+importedModel.name+" was not found in the hierarchy!!!" ); return; }
            GameObject prefabModelRoot =  tr.gameObject;			
			
            if (prefabModelRoot != null)
            {
                //Does all the magic to childs
                RebuildPrefab(importedModel, prefabModelRoot);
                RescueAndRemove(importedModel, prefabModelRoot, lostObjects);
                RenameNewToCurrent(prefabModelRoot.gameObject);

                //Don't forget the parent
                CopyComponents(importedModel, prefabModelRoot);
            }

            //Clean up if no lostobjects have been found
            if (lostObjects.transform.childCount > 0)
                LogWarning(System.String.Format(PrefabLabSettings.WARNING_LOST_OBJECTS_FOUND, lostObjects.transform.childCount, lostObjectsName, prefab.name), originalPrefab);
            else
                GameObject.DestroyImmediate(lostObjects);

            TrackPieceUpdater foo = new TrackPieceUpdater();
			foo.OnPostprocessPreFab(prefab);
			
			//Save created or modified prefab
            EditorUtility.ReplacePrefab(prefab, originalPrefab, ReplacePrefabOptions.ReplaceNameBased);
			
            //Clean up
            GameObject.DestroyImmediate(importedModel);
            GameObject.DestroyImmediate(prefab);
        }
    }

    public override int GetPostprocessOrder()
    {
        //Read PrefabUpdater settings
        PrefabLabSettings settings = PrefabLabSettings.Defaults;
        settings.ReadSettingsFromDisk();

        return settings.PostProcessOrder;
    }

    //Returns true if found and sets the foundPrefab object 
    private bool FindExistingPrefab(string modelname, string subfolder, string prefabSuffix, out Object foundPrefab)
    {
        bool result = false;

        foundPrefab = null;

        //find filepath
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/" + subfolder);
        
        string prefabFilename = "";
        if (di.Exists)
        {
            foreach (FileInfo fi in di.GetFiles())
            {
                if (fi.Name == modelname + prefabSuffix + ".prefab")
                {
                    prefabFilename = fi.Name;
                    break;
                }
            }
        }
        
        //get gameobject
        if (prefabFilename.Length > 0)
        {
            string relativePrefabPath = subfolder + prefabFilename;
            foundPrefab = AssetDatabase.LoadAssetAtPath("Assets/" + relativePrefabPath, typeof(Object));

            result = true;
        }

        return result;
    }

    //Recursive copy newly found modelparts to the prefab
    //and synch Transform, Materials etc. with current parts
    private void RebuildPrefab(GameObject importedModel, GameObject prefab)
    {
        for (int newModelIndex = importedModel.transform.childCount - 1; newModelIndex >= 0; newModelIndex--)
        {
            bool hasMatch = false;

            for (int oldPrefabIndex = prefab.transform.childCount - 1; oldPrefabIndex >= 0; oldPrefabIndex--)
            {
                GameObject importedModelChild = importedModel.transform.GetChild(newModelIndex).gameObject;
                GameObject prefabChild = prefab.transform.GetChild(oldPrefabIndex).gameObject;

                if (importedModelChild.name == prefabChild.name)
                {
                    CopyComponents(importedModelChild, prefabChild);
                    
                    hasMatch = true;

                    RebuildPrefab(importedModelChild, prefabChild);
                    break;
                }
            }

            //New gameobject in importedModel detected, copy it to the prefab
            if (!hasMatch)
            {
                ParentGOWithoutTransformChanges(prefab, importedModel.transform.GetChild(newModelIndex).gameObject);
            }
        }
    }

    //Recursive remove old prefab parts and move the userdata on it to lostObjects
    private void RescueAndRemove(GameObject importedModel, GameObject prefab, GameObject lostObjects)
    {
        //Check prefab for removed parts
        for (int oldPrefabIndex = prefab.transform.childCount - 1; oldPrefabIndex >= 0; oldPrefabIndex--)
        {
            bool hasMatch = false;

            for (int newModelIndex = importedModel.transform.childCount - 1; newModelIndex >= 0; newModelIndex--)
            {
                GameObject newModelChild = importedModel.transform.GetChild(newModelIndex).gameObject;
                GameObject oldPrefabChild = prefab.transform.GetChild(oldPrefabIndex).gameObject;

                if (newModelChild.name == oldPrefabChild.name)
                {
                    hasMatch = true;

                    RescueAndRemove(newModelChild, oldPrefabChild, lostObjects);
                    break;
                }
            }

            //Removed gameobject in the prefab detected. Gather userdata and add it to lost gameobjects
            if (!hasMatch)
            {
                PrefabLabData metadata = prefab.transform.GetChild(oldPrefabIndex).gameObject.GetComponent<PrefabLabData>();
                if (metadata != null)
                {
                    if (metadata.Data != NewModelNameSuffix)    //Check whether the modelpart has just been added to the prefab by Rebuild() or not
                    {
                        //Gather userdata before removing
                        FindAndSaveLostObjects(prefab.transform.GetChild(oldPrefabIndex).gameObject, lostObjects);

                        GameObject.DestroyImmediate(prefab.transform.GetChild(oldPrefabIndex).gameObject);
                    }
                }
            }
        }
    }

    private void CopyTransform(GameObject source, GameObject destination)
    {
        destination.transform.localPosition = source.transform.localPosition;
        destination.transform.localRotation = source.transform.localRotation;
        destination.transform.localScale = source.transform.localScale;
    }

    private void CopyAnimations(GameObject source, GameObject destination)
    {
        if (source.animation == null && destination.animation != null)
        {
            //Remove animation component
            Component.DestroyImmediate(destination.animation);
        }
        else if (source.animation != null && destination.animation == null)
        {
            //Add & sync animation component
            Animation anim = destination.AddComponent<Animation>();
            anim.clip = source.animation.clip;
            foreach (AnimationState animState in source.animation)
            {
                anim.AddClip(animState.clip, animState.name);
            }
        }
        else if (source.animation != null && destination.animation != null)
        {
            //Sync animation component
            if (ModelImporter.generateAnimations != ModelImporterGenerateAnimations.None)
            {
                Animation anim = destination.GetComponent<Animation>();
                
                //Remove all clips from dest
                foreach (AnimationState animState in destination.animation)
                {
                    anim.RemoveClip(animState.clip);
                }

                //Add all clips from source to dest
                foreach (AnimationState animState in source.animation)
                {
                    anim.AddClip(animState.clip, animState.name);
                }

                anim.clip = source.animation.clip;
            }
        }
        else
        {
            //source.anmation && destination.animation are null;
            //Do nothing
        }
    }

    private void CopyColliders(GameObject source, GameObject destination)
    {
        if (ModelImporter.addCollider)
        {
            if (destination.GetComponent<Collider>() == null)
            {
                MeshCollider srcCollider = source.GetComponent<MeshCollider>();
                if (srcCollider != null)
                {
                    MeshCollider dstCollider = destination.AddComponent<MeshCollider>();
                    dstCollider.sharedMesh = srcCollider.sharedMesh;
                }
            }
        }
    }

    private void CopyComponents(GameObject source, GameObject destination)
    {
        CopyTransform(source, destination);
        CopyMeshFilter(source, destination);
        CopyMeshRenderer(source, destination);
        CopyAnimations(source, destination);
        CopyColliders(source, destination);
    }

    private void CopyMeshFilter(GameObject source, GameObject destination)
    {
        MeshFilter smf = source.GetComponent<MeshFilter>();
        MeshFilter dmf = destination.GetComponent<MeshFilter>();
        if (smf == null && dmf != null)
        {
            //Remove meshfilter component
            Component.DestroyImmediate(dmf);
        }
        else if (smf != null && dmf== null)
        {
            //Add & sync meshfilter component
            dmf = destination.AddComponent<MeshFilter>();
            dmf.sharedMesh = smf.sharedMesh;
        }
        else if (smf != null && dmf != null)
        {
            //Sync meshfilter component
            dmf.sharedMesh = smf.sharedMesh;
        }
        else
        {
            //source.meshfilter && destination.meshfilter are null;
            //Do nothing
        }
    }

    private void CopyMeshRenderer(GameObject source, GameObject destination)
    {
        MeshRenderer smr = source.GetComponent<MeshRenderer>();
        MeshRenderer dmr = destination.GetComponent<MeshRenderer>();
        if (smr == null && dmr != null)
        {
            //Remove MeshRenderer component
            Component.DestroyImmediate(dmr);
        }
        else if (smr != null && dmr == null)
        {
            //Add & sync MeshRenderer component
            dmr = destination.AddComponent<MeshRenderer>();
            dmr.sharedMaterials = smr.sharedMaterials;
        }
        else if (smr != null && dmr != null)
        {
            //Sync MeshRenderer component
            if (ModelImporter.importMaterials != false)
                dmr.sharedMaterials = smr.sharedMaterials;
        }
        else
        {
            //source.MeshRenderer && destination.MeshRenderer are null;
            //Do nothing
        }
    }

    //Recursively add metadata to gameobjects
    private void AddMetaData(GameObject root)
    {
        root.AddComponent<PrefabLabData>();

        for (int i = 0; i < root.transform.childCount; i++)
        {
            GameObject child = root.transform.GetChild(i).gameObject;
            AddMetaData(child);
        }
    }

    //Recursively find all gameobjects in root and add suffix to name
    private void AddNameSuffixRecursively(GameObject root, string suffix)
    {
        PrefabLabData metadata = root.GetComponent<PrefabLabData>();
        metadata.Data = suffix;

        for (int i = 0; i < root.transform.childCount; i++)
        {
            GameObject child = root.transform.GetChild(i).gameObject;
            AddNameSuffixRecursively(child, suffix);
        }
    }

    //Rename MetaData.Data from NewModelNameSuffix to CurrentModelNameSuffix
    //(New and Current exist so we can detect whether an gameobject has just been imported or its an older part of the prefab)
    private void RenameNewToCurrent(GameObject root)
    {
        PrefabLabData metadata = root.GetComponent<PrefabLabData>();

        if (metadata != null)
        {
            if (metadata.Data == NewModelNameSuffix)
            {
                metadata.Data = CurrentModelNameSuffix;
            }
        }

        for (int i = 0; i < root.transform.childCount; i++)
        {
            RenameNewToCurrent(root.transform.GetChild(i).gameObject);
        }
    }

    //Find and move userdata from the source to lostObjects
    private void FindAndSaveLostObjects(GameObject source, GameObject lostObjectsStorage)
    {
        for (int i = source.transform.childCount - 1; i >= 0; i--)
        {
            if (source.transform.GetChild(i).gameObject.GetComponent<PrefabLabData>() == null)
                ParentGOWithoutTransformChanges(lostObjectsStorage, source.transform.GetChild(i).gameObject);
            else
                FindAndSaveLostObjects(source.transform.GetChild(i).gameObject, lostObjectsStorage);
        }
    }

    //Parent one GameObject to another without modifying the transform
    private void ParentGOWithoutTransformChanges(GameObject parent, GameObject childToBe)
    {
        Vector3 position = childToBe.transform.localPosition;
        Quaternion rotation = childToBe.transform.localRotation;
        Vector3 scale = childToBe.transform.localScale;

        childToBe.transform.parent = parent.transform;

        childToBe.transform.localScale = scale;
        childToBe.transform.localPosition = position;
        childToBe.transform.localRotation = rotation;

    }

    private ModelImporter ModelImporter
    {
        get { return (ModelImporter)base.assetImporter; }
    }
}


//    private GameObject GetChildByName(string name, GameObject go)
//    {
//        GameObject result = null;
//        Transform t = go.transform.FindChild(name);
//
//        if (t != null)
//            result = t.gameObject;
//
//        return result;
//    }
