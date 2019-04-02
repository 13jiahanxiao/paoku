using UnityEngine;
using System;
using System.Collections;

public class TimeLeftCountdown : MonoBehaviour 
{
	private UILabel timeLabel;
	private DateTime endDateTime = DateTime.MinValue;
	private int previousSeconds = -1;
	// wxj, flag, activity or weekly
	public string type;
	
	void Start() 
	{
		timeLabel = gameObject.GetComponent<UILabel>();		
	}
	
	void Update()
	{
		// wxj, if type=activity and endDate is 00:00, day+1 automatic
		if(type != null && type.Equals("activity") && previousSeconds != DateTime.Now.Second)
		{
			TimeSpan span = endDateTime - DateTime.Now;
			if(span <= TimeSpan.Zero)
			{
				endDateTime = ObjectivesManager.updateActiForNextDay();
			}
			
			// wxj, china use DateTime.Now
			timeLabel.text = FormatTimeSpan(endDateTime - DateTime.Now);
			previousSeconds = DateTime.Now.Second;
			return;
		}
		
		if (previousSeconds != DateTime.UtcNow.Second)	// check if a second has passed
		{
			timeLabel.text = FormatTimeSpan(endDateTime - DateTime.UtcNow);
			previousSeconds = DateTime.UtcNow.Second;

		}
	}
	
	public void SetExpirationDateTime(DateTime endDT)
	{
		endDateTime = endDT;
	}
	
   	private string FormatTimeSpan(TimeSpan span)
   	{
		if (span <= TimeSpan.Zero)
			return "00:00:00:00";
		else
		  	return span.Days.ToString("00") + ":" + 
				span.Hours.ToString("00") + ":" + 
				span.Minutes.ToString("00") + ":" + 
				span.Seconds.ToString("00");
   }	
}

	//if (endDateTime != DateTime.MinValue && endDateTime > DateTime.UtcNow)
		//{


			//timeLabel.text = string.Format("{0:dd:hh:mm:ss}", new DateTime((endDateTime - DateTime.UtcNow).Ticks));
			
		//}
		//else
		//	timeLabel.text = "00:00:00:00";