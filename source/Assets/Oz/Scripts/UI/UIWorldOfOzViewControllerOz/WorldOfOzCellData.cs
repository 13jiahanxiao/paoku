using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldOfOzData 
{
	public string Title;
	public string Description;
	public string IconName;
	//public int SortPriority;
	public bool installed;
	public bool available;
	public int ID;

	public WorldOfOzData(int id, string title, string description, string iconName, bool inst, bool avail)
	{
		ID = id;
		Title = title;
		Description = description;
		IconName = iconName;
		installed = inst;
		available = avail;
	}
}

public class WorldOfOzCellData : MonoBehaviour 
{
	public GameObject viewController;
	public GameObject scrollList;									// reference to the scroll list that this cell is parented under the grid/table of		
	public UISprite frame;
	public List<GameObject> buttons = new List<GameObject>();
	public WorldOfOzData _data;										// reference to data	
	
	private NotificationSystem notificationSystem;
	private NotificationIcons notificationIcons;	
	
	protected static Notify notify;
	
	void Awake()
	{
		if (notify != null)
			notify = new Notify("WorldOfOzCellData");
		
		notificationIcons = gameObject.GetComponent<NotificationIcons>();		
	}
	
	void Start()
	{
		Destroy(gameObject.GetComponent<UIPanel>());				// kill auto-attached UIPanel component
		
		notificationSystem = Services.Get<NotificationSystem>();
		
		//if (_data != null)	// && viewController != null)
		//	Refresh();												// populate fields
	}

	public void SetData(WorldOfOzData data)
	{
		_data = data;
		Refresh();													// populate fields	
	}
	
	private void Refresh()
	{
		if (_data != null)	// && viewController != null)
		{	
			// populate fields from data
			//gameObject.transform.Find("CellContents").GetComponent<UIButtonMessage>().target = viewController;			
			//gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton").GetComponent<UIButtonMessage>().target = scrollList;	// link up 'buy' button
			foreach (GameObject go in buttons)
				go.GetComponent<UIButtonMessage>().target = scrollList;	// link up 'buy' button			
			
			gameObject.transform.Find("CellContents/FontAnchor/LabelTitle").GetComponent<UILocalize>().SetKey(_data.Title);
			gameObject.transform.Find("CellContents/FontAnchor/LabelDescription").GetComponent<UILocalize>().SetKey(_data.Description);
			gameObject.transform.Find("CellContents/IconAnchor/SpriteIcon").GetComponent<UISprite>().spriteName = _data.IconName;
			
			SetNotificationIcon();	// show notification icon if still can download this item
			
			if (_data.installed)
			{
				gameObject.transform.Find("CellContents/FontAnchor/LabelBuy").GetComponent<UILocalize>().SetKey("Lbl_Installed");		// set localized 'installed' button text
				EnableButton(1);
			}
			else if (_data.available)
			{
				gameObject.transform.Find("CellContents/FontAnchor/LabelBuy").GetComponent<UILocalize>().SetKey("Lbl_Download_Cap");	// set localized 'download' button text
				EnableButton(0);
			}
			else
			{
				gameObject.transform.Find("CellContents/FontAnchor/LabelBuy").GetComponent<UILocalize>().SetKey("");					// hide text
				EnableButton(-1);	// disable button			
			}
		}
	}
	
	private void EnableButton(int id)
	{
		foreach (GameObject go in buttons)
			NGUITools.SetActive(go, false);
		
		if (id >= 0 && id < buttons.Count)
		{
			NGUITools.SetActive(buttons[id], true);		// enable active button, if within range
			frame.enabled = true;
		}
		else
			frame.enabled = false;
	}	
	
	public void SetNotificationIcon()
	{
		if (notificationSystem == null)
			notificationSystem = Services.Get<NotificationSystem>();
		
		bool enable = notificationSystem.GetNotificationStatusForThisCell(NotificationType.Land, _data.ID);
//		if (_data.ID == 0) - commented out for Emerald City "Coming Soon" 1.4 ONLY N.N.
//		{
//			enable = false;	// don't show notification for HD assets
//		}
//		notificationIcons.SetNotification(0, (enable) ? 0 : -1);
		if (_data.ID != 1) //Use for 1.4 ONLY N.N.
		{
			enable = false;	// don't show notification for HD assets and Emerald City "Coming Soon"
		}
		notificationIcons.SetNotification(0, (enable) ? 0 : -1);
	}	
}


				//gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton").GetComponent<UIButtonMessage>().enabled = true;	// enable button message
				//gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton/Background").GetComponent<UISprite>().color = new Color(56f/255f, 165f/255f, 0f, 1f); // green	
				

				//gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton").GetComponent<UIButtonMessage>().enabled = false;	// disable button message
				//gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton/Background").GetComponent<UISprite>().color = new Color(50f/255f, 50f/255f, 50f/255f, 1f); // gray	
				


//	public void OnWorldOfOzCellPressed(GameObject cell)
//	{
//		switch (cell.name)
//		{
//			case "cell0":	// download HD upgrade
//				break;
//			case "cell1":	// download Dark Forest
//				break;
//		}
//	}


	
//	private void SetButtonStatus(bool active)
//	{
//		gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton/Background").GetComponent<UISprite>().enabled = active;
//		gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton/Highlight").GetComponent<UISprite>().enabled = active;		
//		gameObject.transform.Find("CellContents/GraphicsAnchor/Frame").GetComponent<UISprite>().enabled = active;
//		gameObject.transform.Find("CellContents/FontAnchor/LabelBuy").GetComponent<UILabel>().enabled = active;
//		gameObject.transform.Find("CellContents/GraphicsAnchor/ImageButton").GetComponent<BoxCollider>().enabled = active;
//	}
	