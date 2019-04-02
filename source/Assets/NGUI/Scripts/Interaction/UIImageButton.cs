//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright � 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Sample script showing how easy it is to implement a standard button that swaps sprites.
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Image Button")]
public class UIImageButton : MonoBehaviour
{
	public UISprite target;
	public string normalSprite;
	public string hoverSprite;
	public string pressedSprite;

	void OnEnable ()
	{
		if (target != null)
		{
			target.spriteName = UICamera.IsHighlighted(gameObject) ? hoverSprite : normalSprite;
		}
	}

	void Start ()
	{
		if (target == null) target = GetComponentInChildren<UISprite>();
	}

	void OnHover (bool isOver)
	{
		if (enabled && target != null)
		{
			target.spriteName = isOver ? hoverSprite : normalSprite;
			//target.MakePixelPerfect();	//-Commented out by Bryant Cannon - Not sure why this is needed, was causing wierd things to happen
		}
	}

	void OnPress (bool pressed)
	{
		if (enabled && target != null)
		{
			target.spriteName = pressed ? pressedSprite : normalSprite;
			//target.MakePixelPerfect();	//-Commented out by Bryant Cannon - Not sure why this is needed, was causing wierd things to happen
		}
	}
}