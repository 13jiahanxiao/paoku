using UnityEngine;

/// <summary>
/// Tween the slider's value.
/// </summary>

[AddComponentMenu("NGUI/Tween/Slider")]
public class TweenSlider : UITweener
{
	public float from = 0f;
	public float to = 1f;

	UISlider mSource;

	/// <summary>
	/// Cached version of 'audio', as it's always faster to cache.
	/// </summary>

	public UISlider slideSource
	{
		get
		{
			if (mSource == null)
			{
				mSource = GetComponent<UISlider>();
				if (mSource == null)
				{
					mSource = GetComponentInChildren<UISlider>();
	
					if (mSource == null)
					{
						Debug.LogError("TweenSlider needs an AudioSource to work with", this);
						enabled = false;
					}
				}
			}
			
			return mSource;
		}
	}

	/// <summary>
	/// Slider's current value.
	/// </summary>

	public float sliderValue { get { return slideSource.sliderValue; } set { slideSource.sliderValue = value; } }

	/// <summary>
	/// Tween update function.
	/// </summary>

	override protected void OnUpdate (float factor, bool isFinished)
	{
		sliderValue = from * (1f - factor) + to * factor;
		mSource.enabled = (mSource.sliderValue > 0.01f);
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenSlider Begin (GameObject go, float duration, float targetValue)
	{
		TweenSlider comp = UITweener.Begin<TweenSlider>(go, duration);
		comp.from = comp.sliderValue;
		comp.to = targetValue;
		return comp;
	}
}