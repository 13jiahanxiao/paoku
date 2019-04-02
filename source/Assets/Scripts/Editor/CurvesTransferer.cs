using UnityEditor;
using UnityEngine;

public class CurvesTransferer
{
const string duplicatePostfix = "_copy";
	protected static Notify notify = new Notify("CurvesTransferer");

static void CopyClip(string importedPath, string copyPath)
{
    AnimationClip src = AssetDatabase.LoadAssetAtPath(importedPath, typeof(AnimationClip)) as AnimationClip;
    AnimationClip newClip = new AnimationClip();
    newClip.name = src.name + duplicatePostfix;
    AssetDatabase.CreateAsset(newClip, copyPath);
    AssetDatabase.Refresh();
}

[MenuItem("Assets/Transfer Clip Curves to Copy")]
static void CopyCurvesToDuplicate()
{
    // Get selected AnimationClip
    AnimationClip imported = Selection.activeObject as AnimationClip;
    if (imported == null)
    {
        notify.Debug("Selected object is not an AnimationClip");
        return;
    }

    // Find path of copy
    string importedPath = AssetDatabase.GetAssetPath(imported);
    string copyPath = importedPath.Substring(0, importedPath.LastIndexOf("/"));
    copyPath += "/" + imported.name + duplicatePostfix + ".anim";

    CopyClip(importedPath, copyPath);

    AnimationClip copy = AssetDatabase.LoadAssetAtPath(copyPath, typeof(AnimationClip)) as AnimationClip;
    if (copy == null)
    {
        notify.Debug("No copy found at " + copyPath);
        return;
    }
    // Copy curves from imported to copy
    AnimationClipCurveData[] curveDatas = AnimationUtility.GetAllCurves(imported, true);
    for (int i = 0; i < curveDatas.Length; i++)
    {
        AnimationUtility.SetEditorCurve(
            copy,
            curveDatas[i].path,
            curveDatas[i].type,
            curveDatas[i].propertyName,
            curveDatas[i].curve
        );
    }

    notify.Debug("Copying curves into " + copy.name + " is done");
}
	
//[MenuItem("Assets/Transfer Clip Curves to UIWidget")]
//	static void Foo() {
//		GameObject selectedObject = Selection.activeGameObject as GameObject;
//		if(selectedObject == null) {
//			notify.Debug("selected object is not a GameObject.");
//			return;
//		}
//		
//		UIWidgetAnimation widgetAnim = selectedObject.GetComponent<UIWidgetAnimation>()  as UIWidgetAnimation;
//		if(widgetAnim == null) {
//			notify.Debug("Gameobject needs to have a UIWidgetAnimation");
//			return;
//		}
//		
//		Animation anim = selectedObject.animation;
//		if(anim == null) {
//			notify.Debug("GameObject is missing an animation");
//			return;
//		}
//		
//		AnimationClipCurveData[] curves = AnimationUtility.GetAllCurves(anim.clip, true);
//		for(int i=0; i<curves.Length; i++)
//		{
//			notify.Debug(curves[i].path +" "+curves[i].propertyName+ " "+curves[i].type.ToString());
//			
//		}
//		AnimationCurve localPositionx = AnimationUtility.GetEditorCurve(anim.clip, "Transform", typeof(float), "m_LocalPosition.x");
//		if(localPositionx != null) {
//			anim.clip.SetCurve(null,typeof(UIWidgetAnimation), "Position.x", localPositionx);
//			//AnimationUtility.SetEditorCurve(anim.clip, "UIWidget Animation", typeof(float), "Position.x", localPositionx);
//		}
//	}
}
