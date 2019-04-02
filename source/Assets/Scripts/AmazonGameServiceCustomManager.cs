using UnityEngine;
using System.Collections;

public class AmazonGameServiceCustomManager : MonoBehaviour {
	protected static Notify notify;
	public bool DoAmazonGamerServices = true;
	public bool GameServicesAvailable = false;

	public static AmazonGameServiceCustomManager Instance;

	void Awake()
	{
		Instance = this;
		notify = new Notify(this.GetType().Name);
	}

#if UNITY_ANDROID

	// Use this for initialization
	void Start () {
		if (DoAmazonGamerServices) {

			GameCircleManager.serviceReadyEvent += serviceReadyEvent;
			GameCircleManager.serviceNotReadyEvent += serviceNotReadyEvent;

			GameCircle.init(false);
		}
	}


	void serviceReadyEvent()
	{
		notify.Debug("serviceReadyEvent");
		GameServicesAvailable = true;
	}


	void serviceNotReadyEvent(string param)
	{
		notify.Debug("serviceNotReadyEvent: " + param);
		GameServicesAvailable = false;
	}

#endif	

	public void UpdateStats(int score, int coin, int distance)
	{
#if UNITY_ANDROID
		if (DoAmazonGamerServices && GameServicesAvailable) {
			if(score > 0)
				GameCircle.submitScore(GameController.Leaderboard_HighScores, score);
			if(distance > 0)
				GameCircle.submitScore(GameController.Leaderboard_DistanceRun, coin);
		}
#endif
	}

	public void UpdateAchievementProgress( string achievementId, float progress)
	{
#if UNITY_ANDROID
		if (DoAmazonGamerServices && GameServicesAvailable) {
			if (progress > 0) {
				//notify.Debug("*** UPDATE PROGRESS: " + achievementId + " : " + progress);
				GameCircle.updateAchievementProgress(achievementId, progress);
			}
		}
#endif
	}
}
