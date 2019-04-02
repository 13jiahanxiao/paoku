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
/// The copyright notice above does not evidence any actual or intended publication or disclosure of this source code, which includes  
/// information that is confidential and/or proprietary, and is a trade secret, of COMPANY. ANY REPRODUCTION, MODIFICATION, DISTRIBUTION, PUBLIC  PERFORMANCE, 
/// OR PUBLIC DISPLAY OF OR THROUGH USE OF THIS SOURCE CODE WITHOUT THE EXPRESS WRITTEN CONSENT OFCOMPANY IS STRICTLY PROHIBITED, AND IN VIOLATION OF APPLICABLE 
/// LAWS AND INTERNATIONAL TREATIES. THE RECEIPT OR POSSESSION OF THIS SOURCE CODE AND/OR RELATED INFORMATION DOES NOT CONVEY OR IMPLY ANY RIGHTS  
/// TO REPRODUCE, DISCLOSE OR DISTRIBUTE ITS CONTENTS, OR TO MANUFACTURE, USE, OR SELL ANYTHING THAT IT MAY DESCRIBE, IN WHOLE OR IN PART.                
///

using UnityEngine;
using System.Collections;

public class HUDFPS : MonoBehaviour
{

	// Attach this to a GUIText to make a frames/second indicator.
	//
	// It calculates frames/second over each updateInterval,
	// so the display does not keep changing wildly.
	//
	// It is also fairly accurate at very low FPS counts (<10).
	// We do this not by simply counting frames per interval, but
	// by accumulating FPS for each frame. This way we end up with
	// correct overall FPS even if the interval renders something like
	// 5.5 frames.

	public float updateInterval = 0.5F;

	private float accum = 0; // FPS accumulated over the interval
	private int frames = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval

	private UILabel Label;







	const int MAXSAMPLES = 10;
	int tickindex = 0;
	float ticksum = 0;
	float[] ticklist = new float[MAXSAMPLES];

	/* need to zero out the ticklist array before starting */
	/* average will ramp up until the buffer is full */
	/* returns average ticks per frame over the MAXSAMPPLES last frames */

	double CalcAverageTick(float newtick)
	{
		ticksum -= ticklist[tickindex];  /* subtract value falling off */
		ticksum += newtick;              /* add new value */
		ticklist[tickindex] = newtick;   /* save new value so it can be subtracted later */
		if (++tickindex == MAXSAMPLES)    /* inc buffer index */
			tickindex = 0;

		/* return average */
		return ((double)ticksum / MAXSAMPLES);
	}



	void Start()
	{
		Label = GetComponent<UILabel>();
		timeleft = updateInterval;
	}


	

	void Update()
	{
		timeleft -= Time.deltaTime;
		float frameTime = Time.timeScale / Time.deltaTime;
		accum += frameTime;
		++frames;

		CalcAverageTick(frameTime);
		if(FTPSGraph.Instance != null)
			FTPSGraph.Instance.AddPoint((float)frameTime);

		// Interval ended - update GUI text and start new interval
		if (timeleft <= 0.0) {
			// display two fractional digits (f2 format)
			float fps = accum / frames;
			string format = System.String.Format("{0:F2} FPS", fps);
			Label.text = format;

			if (fps < 30)
				Label.color = Color.yellow;
			else
				if (fps < 10)
					Label.color = Color.red;
				else
					Label.color = Color.green;
			//  DebugConsole.Log(format,level);
			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}

//		float val = GamePlayer.SharedInstance.PlayerMagnitude;
//		float xzVal = GamePlayer.SharedInstance.PlayerXZMagnitude;
//		if(FTPSGraph.Instance != null)
//		{
//			FTPSGraph.Instance.AddPoint((float)val);	
//		}
//		string format = System.String.Format("{0:F2}, {1:F2}", val, xzVal);
//		Label.text = format;
	}



}