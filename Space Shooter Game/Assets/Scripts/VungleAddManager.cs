using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VungleAddManager : MonoBehaviour
{
    //App ID
    string appID = " ";
    [SerializeField] string iOSAppID = "5912326f0e96c1a540000014";
    [SerializeField] string androidAppID = "591236625b2480ac40000028";

//PlcaementIDS point
#if UNITY_IPHONE
    Dictionary<string, bool> placements = new Dictionary<string, bool>
    {
        { "DEFAULT63997", false },
        { "PLMT02I58266", false },
        { "PLMT03R65406", false }
    };
#elif UNITY_ANDROID
	Dictionary<string, bool> placements = new Dictionary<string, bool>
	{
		{ "DEFAULT18080", false },
		{ "PLMT02I58745", false },
		{ "PLMT03R02739", false }
	};
#endif

    List<string> placementIdList;

	bool adInited = false;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_IPHONE
        appID = iOSAppID;
#elif UNITY_ANDROID
        appID = androidAppID;
#endif
        placementIdList = new List<string>(placements.Keys);

        //Vungle SDK Initilization
        Vungle.init(appID);
		initializeEventHandlers();
    }

	// Setup EventHandlers for all available Vungle events
	void initializeEventHandlers()
	{

		// Event triggered during when an ad is about to be played
		Vungle.onAdStartedEvent += (placementID) =>
		{
			DebugLog("Ad " + placementID + " is starting!  Pause your game  animation or sound here.");
#if UNITY_ANDROID
				placements[placementID] = false;
#endif
		};

		// Event is triggered when a Vungle ad finished and provides the entire information about this event
		// These can be used to determine how much of the video the user viewed, if they skipped the ad early, etc.
		Vungle.onAdFinishedEvent += (placementID, args) =>
		{
			DebugLog("Ad finished - placementID " + placementID + ", was call to action clicked:" + args.WasCallToActionClicked + ", is completed view:"
				+ args.IsCompletedView);
		};

		// Event is triggered when the ad's playable state has been changed
		// It can be used to enable certain functionality only accessible when ad plays are available
		Vungle.adPlayableEvent += (placementID, adPlayable) =>
		{
			DebugLog("Ad's playable state has been changed! placementID " + placementID + ". Now: " + adPlayable);
			placements[placementID] = adPlayable;
		};

		//Fired initialize event from sdk
		Vungle.onInitializeEvent += () =>
		{
			adInited = true;
			DebugLog("SDK initialized");
		};
	}

	void DebugLog(string message)
	{
		Debug.Log("VungleUnitySample " + System.DateTime.Today + ": " + message);
	}

	public void PlayPlacement1Add()
	{
		// option to change orientation
		Dictionary<string, object> options = new Dictionary<string, object>();
#if UNITY_IPHONE
		options["orientation"] = 5;
#else
		options ["orientation"] = true;
#endif

		Vungle.playAd(options, placementIdList[0]);
	}
}
