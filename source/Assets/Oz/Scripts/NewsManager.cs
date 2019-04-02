using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class NewsManager : MonoBehaviour 
{
	protected static Notify notify;
	
	void Start()
	{
		notify = new Notify(this.GetType().Name);
		if ( ! Settings.GetBool("use-init-msg", true))
		{
			GetNews();
		}
	}
	
	/// <summary>
	/// Gets the latest in game news
	/// </summary> 
	void GetNews()
	{
		NetRequest newsReq = new NetRequest("/news", GotTheNews);
		NetAgent.Submit(newsReq);	//Services.Get<NetAgent>().Submit(newsReq);
	}
	
	/// <summary>
	/// Handler after receiving the news reply
	/// </summary>
	/// <returns>
	/// True if there were no errors
	/// </returns>
	/// <param name='www'>
	/// this is the raw www object so you can check error codes or see the error text
	/// </param>
	/// <param name='noErrors'>
	/// will be set to true if there were no transmission errors (www.isDone is true and www.error is null)
	/// </param>
	/// <param name='results'>
	/// A Json decoded object
	/// </param>
	bool GotTheNews( WWW www, bool noErrors, object results)
	{
		//return true; // temp getting a crash on ios
		notify.Debug("GotTheNews noErrors=" + noErrors + " www.error=" + www.error);
		bool result = false;
		if (noErrors)
		{
			if(results==null)
			{
				notify.Warning("No results! Must not be connected...");
//				return false;
			}
			
			notify.Debug("GotTheNews " + www.text + " " + www.error);
			try
			{	
				Dictionary<string, object> rootDict = results as Dictionary<string, object>;	
//				object query = rootDict["querySuccess"];
				
				object query = rootDict["responseCode"];
				
				if(query!=null)
				{
					int responseCode = int.Parse(rootDict["responseCode"].ToString());
					
					if (responseCode == 200)
					{
						List<object> newsItems = rootDict["newsItem"] as List<object>;
						if (newsItems.Count > 0)
						{
							string newsString = "   ";
							newsItems.Reverse();
							foreach (object oneObject in newsItems)
							{
								NewsItem oneItem = NewsItem.DecodeJsonObject(oneObject);
								if (oneItem != null)
								{
									newsString += oneItem.Title;
									newsString += "   ";
									newsString += oneItem.Body;
									newsString += "        ";
								}
								
							}
							if (UIManagerOz.SharedInstance.PaperVC != null)
							{
								//HorizontalScrollingLabel horizLabel = UIManagerOz.SharedInstance.idolMenuVC.gameObject.GetComponent<HorizontalScrollingLabel>();
								HorizontalScrollingLabel horizLabel = UIManagerOz.SharedInstance.idolMenuVC.NewsFeed.GetComponent<HorizontalScrollingLabel>();
								if (horizLabel != null)
								{
									horizLabel.FullString = newsString;
								}
								else
								{
									notify.Warning("couldn't find horizontal scrolling label in PaperVC");	
								}
							}
							result = true;
						}
					}
				}
			}
			catch (System.Exception theException)
			{
				notify.Warning("GotTheNews www.text= " + www.text + " exception " + theException.Message);
			}
		}
		
		// No news were loaded for some reason.  Display a random GameTip.
		if (!result)
		{
			ApplyGameTipForEmptyNews();
			
			/*
			string tipString = "        ";
			tipString += GameTipManager.GetRandomGameTip().tip;
			tipString += "        ";
			
			if (UIManagerOz.SharedInstance.idolMenuVC != null)
			{
				//HorizontalScrollingLabel horizLabel = UIManagerOz.SharedInstance.idolMenuVC.gameObject.GetComponent<HorizontalScrollingLabel>();
				HorizontalScrollingLabel horizLabel = UIManagerOz.SharedInstance.idolMenuVC.NewsFeed.GetComponent<HorizontalScrollingLabel>();
				if (horizLabel != null)
				{
					horizLabel.FullString = tipString;
				}
				else
				{
					notify.Warning("couldn't find horizontal scrolling label in idolMenuVC");	
				}
			}
			*/
		}
		
		return result;	
	}
	
	public void ApplyGameTipForEmptyNews()
	{
		
		int rand = 1 + (int)(Random.value * 38.9999f);
		string msg = "Lbl_Tips_" + rand;
		
		string tmpMsg = Localization.SharedInstance.Get(msg);
		
		//tmpMsg = System.Text.RegularExpressions(tmpMsg, System.Environment.NewLine, string.Empty());
		
		//System.Text.RegularExpressions regEx = new System.Text.RegularExpressions();
		
		notify.Debug("Tip: " + tmpMsg);
		
		tmpMsg = tmpMsg.Replace("\\n", " ");
		
		notify.Debug("After regex: " + tmpMsg);
		
		string tipString = "        ";
		tipString += tmpMsg;
		tipString += "        ";

		notify.Debug("tip string: " + tipString);
		
		if (UIManagerOz.SharedInstance.idolMenuVC != null)
		{
			//HorizontalScrollingLabel horizLabel = UIManagerOz.SharedInstance.idolMenuVC.gameObject.GetComponent<HorizontalScrollingLabel>();
			HorizontalScrollingLabel horizLabel = UIManagerOz.SharedInstance.idolMenuVC.NewsFeed.GetComponent<HorizontalScrollingLabel>();
			if (horizLabel != null)
			{
				horizLabel.FullString = tipString;
			}
			else
			{
				notify.Warning("couldn't find horizontal scrolling label in PaperVC");	
			}
		}
	}
	
	public bool ApplyNewsFromInit(List<object> newsList, int responseCode)
	{
		bool result = false;
		
		if (newsList != null && newsList.Count > 0)
		{
			
			notify.Debug("Applying News from init");
			
			string newsString = "    ";
			newsList.Reverse();
			foreach (object oneObject in newsList)
			{
				NewsItem oneItem = NewsItem.DecodeJsonObject(oneObject);
				if (oneItem != null)
				{
					newsString += oneItem.Title;
					newsString += "   ";
					newsString += oneItem.Body;
					newsString += "        ";
				}
				
				if (UIManagerOz.SharedInstance.PaperVC != null)
				{
					//HorizontalScrollingLabel horizLabel = UIManagerOz.SharedInstance.idolMenuVC.gameObject.GetComponent<HorizontalScrollingLabel>();
					HorizontalScrollingLabel horizLabel = UIManagerOz.SharedInstance.idolMenuVC.NewsFeed.GetComponent<HorizontalScrollingLabel>();
					if (horizLabel != null)
					{
						horizLabel.FullString = newsString;
					}
					else
					{
						notify.Warning("couldn't find horizontal scrolling label in PaperVC");	
					}
				}
				result = true;
			}
		}
		
		//No news were loaded.  Display a random GameTip
		if (!result)
		{
			ApplyGameTipForEmptyNews();
			/*
			int rand = (int)(Random.value * 39f);
			string msg = "Lbl_Tips_" + rand;	
			
			string tipString = "        ";
			tipString +=  Localization.SharedInstance.Get (msg);
			tipString += "        ";
			
			if (UIManagerOz.SharedInstance.idolMenuVC != null)
			{
				//HorizontalScrollingLabel horizLabel = UIManagerOz.SharedInstance.idolMenuVC.gameObject.GetComponent<HorizontalScrollingLabel>();
				HorizontalScrollingLabel horizLabel = UIManagerOz.SharedInstance.idolMenuVC.NewsFeed.GetComponent<HorizontalScrollingLabel>();
				if (horizLabel != null)
				{
					horizLabel.FullString = tipString;
				}
				else
				{
					notify.Warning("couldn't find horizontal scrolling label in PaperVC");	
				}
			}
			*/
		}
		
		return result;
	}
	
	
/*Replaced checking querySucces with checking if responseCode is '200' - Response is Okay, and data is present
 
					bool querySuccess = (bool) query ;
					if (querySuccess)
*/
}
