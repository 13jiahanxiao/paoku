using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// will return true if everything went well, 
/// noErrors will be true if www.iSDone is true and www.error is null
/// results will be a json decoded object if noErrors is true,  may be null otherwise
/// </summary>
public delegate bool NetRequestDelegate( WWW www, bool noErrors, object results);

/// <summary>
/// Net request.
/// </summary>
public class NetRequest
{
	/// <summary>
	/// Initializes a new instance of the <see cref="NetRequest"/> class. Used for WWW Gets
	/// </summary>
	/// <param name='url'>
	/// URL is meant to be relative to game server, if the full url is www.disney.com/news/about
	/// this parameter would just be "/news/about"
	/// </param>
	public NetRequest(string url)
	{
		this.url = url;	
		this.postData = null;
		this.requestId = requestCounter ++;
		this.resultHandler = null; // implies we don't care if we fail or succeed
	}
	
	public NetRequest(string url, NetRequestDelegate resultHandler)
	{
		this.url = url;	
		this.postData = null;
		this.requestId = requestCounter ++;
		this.resultHandler = resultHandler;
	}
	
	public NetRequest(string url, bool overrideUrl, NetRequestDelegate resultHandler) : this ( url, resultHandler )
	{
		this.overrideUrl = overrideUrl;
	}
	
	public bool overrideUrl = false;
	
	public NetRequest(string url, WWWForm postData, NetRequestDelegate resultHandler) : this (url,resultHandler)
	{
		this.postData = postData;		
	}
	
	public NetRequest(string url, WWWForm postData) : this (url)
	{
		this.postData = postData;		
	}
	
	public NetRequest(string url, WWWForm postData, NetRequestDelegate resultHandler, Hashtable headers) :
		this(url, postData, resultHandler)
	{
		this.headers = headers;
	}
	
	public Hashtable headers;
	public string url;
	public WWWForm postData;
	public NetRequestDelegate	resultHandler;
	public int requestId;
	
	private static int requestCounter = 0;
}

/// <summary>
/// Meant to be the only class that sends out url requests from the game.
/// 3rd party libraries might do their own network handling, but this is meant for in game use
/// </summary>
public class NetAgent : MonoBehaviour 
{
	protected static Notify notify;

	// in order to use coroutines we must have an instance of an active game object	
	private static NetAgent instance;
	
	/// <summary>
	/// fundamental design decision to send stuff to the server only one at a time
	/// </summary>
	private static Queue<NetRequest> netRequests = new Queue<NetRequest>();
	
	/// <summary>
	/// would become true if we are handling any net requests
	/// </summary>
	private static bool busy;
	public static bool Busy
	{
		get { 
			return busy;
		}
	}
	
	private static string serverUrl; 
	
	/// <summary>
	/// Sets up our needed static fields, 
	/// </summary>
	void Awake () {
		notify = new Notify(this.GetType().Name);
		if (instance != null)
		{
			notify.Warning("we have potentially two instances of NetAgent running");	
		}
		serverUrl = Settings.GetString("server-url","");
 
		if (serverUrl == "")
		{
			notify.Error("please add a server-url setting");	
		}
		instance = this;
	}
	
	/// <summary>
	/// Submit the specified netReq, sends it immediately if not busy, otherwise will send it when preceding
	/// net requests have finished
	/// </summary>
	/// <param name='netReq'>
	/// Net req.
	/// </param>
	public static void Submit( NetRequest netReq)
	{
		if (instance == null)
		{
			notify.Error("Net Agent instance is null");
			return;
		}
		if (busy)
		{
			netRequests.Enqueue(netReq);
		}	
		else
		{
			instance.StartCoroutine(instance.RunRequest(netReq));
		}
	}
	
	/// <summary>
	/// Runs the request. in a coroutine, will call the delegate when done and start the next one if needed
	/// </summary>
	/// <param name='netReq'>
	/// Netrequest with needed info
	/// </param>
	IEnumerator RunRequest(NetRequest netReq)
	{
		busy = true;
		
		string fullUrl = "";
		
		if ( !netReq.overrideUrl )
		{
			fullUrl = serverUrl + netReq.url;
		}
		else
		{
			fullUrl = netReq.url;
		}
		
		WWW www;
		if (netReq.postData == null)
		{
			www = new WWW(fullUrl);
		}
		else if (netReq.postData != null && netReq.headers != null)
		{
			www = new WWW(fullUrl, netReq.postData.data, netReq.headers);
		}
		else
		{
			www = new WWW(fullUrl, netReq.postData);	
		}
		
		bool noErrors;

		notify.Debug("NetAgent starting yield return www, url=" + fullUrl);
		yield return www;
		notify.Debug("NetAgent done with yield return www, fullUrl = " + fullUrl);
		noErrors = ( www.isDone == true && www.error == null && www.text != null);

		if (www.error != null)
		{
			notify.Warning("NetAgent.RunRequest  www.error=" + www.error);
		}
		
		if (netReq.resultHandler != null)
		{
			object result = null;
			if (  noErrors)
			{
				notify.Debug("calling deserialize  ");
				notify.Debug ("www.text=" + www.text);
				try
				{
					result= MiniJSON.Json.Deserialize(www.text);
					if (result == null)
					{
						notify.Warning("deserialization failed " + fullUrl);
						noErrors = false;
					}
				}
				catch (System.Exception e)
				{
					notify.Warning("deserialization exception " + fullUrl + " exception=" + e);
					noErrors = false;
				}
			}
			netReq.resultHandler(www, noErrors, result);
		}
		
		// now start up the next request if needed
		if ( netRequests.Count > 0)
		{
			NetRequest next = netRequests.Dequeue();
			instance.StartCoroutine(instance.RunRequest(next));	
		}
		else
		{
			busy = false;
		}
	}
}
