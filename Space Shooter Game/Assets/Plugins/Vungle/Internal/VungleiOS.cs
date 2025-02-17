﻿using System.Collections.Generic;
using System.Runtime.InteropServices;

#if UNITY_IOS
public enum VungleAdOrientation
{
	Portrait = 1,
	LandscapeLeft = 2,
	LandscapeRight = 3,
	PortraitUpsideDown = 4,
	Landscape = 5,
	All = 6,
	AllButUpsideDown = 7
}

public partial class VungleiOS : IVungleHelper
{
	private const long Megabyte = 1024 * 1024;
	private const long IosDefaultMinSpace = 50000000;
	private string version;

	public string SdkVersion
	{
		get
		{
			if (string.IsNullOrEmpty(version))
			{
				version = _vungleGetSdkVersion();
			}
			return version;
		}
	}

	public string VersionInfo
	{
		get
		{
			return string.Format("VungleSDKCall-iOS/{0}-{1}", Vungle.PluginVersion, SdkVersion);
		}
	}

	#region SDKSetup
	public void UpdateConsentStatus(Vungle.Consent consent, string version = "1.0")
	{
		if (Vungle.Consent.Undefined == consent)
		{
			return;
		}

		_updateConsentStatus((int)consent, version);
	}

	public Vungle.Consent GetConsentStatus()
	{
		return (Vungle.Consent)_getConsentStatus();
	}

	public void UpdateCCPAStatus(Vungle.Consent consent)
	{
		if (Vungle.Consent.Undefined == consent) { return; }
		_updateCCPAStatus((int)consent);
	}

	public Vungle.Consent GetCCPAStatus()
	{
		return (Vungle.Consent)_getCCPAStatus();
	}

	public void SetLogEnable(bool enable)
	{
		_vungleEnableLogging(enable);
	}
	#endregion

	#region SDKInitialization
	public void Init(string appId)
	{
		setupIPhoneDefaults();
		_vungleStartWithAppId(appId, Vungle.PluginVersion, false);
	}

	public void Init(string appId, bool initHeaderBiddingDelegate)
	{
		setupIPhoneDefaults();
		_vungleStartWithAppId(appId, Vungle.PluginVersion, initHeaderBiddingDelegate);
	}

	private static void setupIPhoneDefaults()
	{
		_vungleSetPublishIDFV(Vungle.enableHardwareIdPrivacy.HasValue ? !(Vungle.enableHardwareIdPrivacy.Value) : true);
		_vungleSetMinimumSpaceForInit(Vungle.minimumDiskSpaceForInitialization.HasValue ?
			(int)(Vungle.minimumDiskSpaceForInitialization.Value / Megabyte) : (int)(IosDefaultMinSpace / Megabyte));
		_vungleSetMinimumSpaceForAd(Vungle.minimumDiskSpaceForAd.HasValue ?
			(int)(Vungle.minimumDiskSpaceForAd.Value / Megabyte) : (int)(IosDefaultMinSpace / Megabyte));
	}
	#endregion

	#region AdLifeCycle
	public bool IsAdvertAvailable(string placementId)
	{
		return _vungleIsAdAvailable(placementId);
	}

	public bool IsAdvertAvailable(string placementId, Vungle.VungleBannerSize adSize)
	{
		return _vungleIsBannerAvailable(placementId, (int)adSize);
	}

	public void LoadAd(string placementId)
	{
		_vungleLoadAd(placementId);
	}

	public void PlayAd(string placementId)
	{
		PlayAd(null, placementId);
	}

	public void PlayAd(Dictionary<string, object> options, string placementId)
	{
		if (options == null)
		{
			options = new Dictionary<string, object>();
		}
		_vunglePlayAd(MiniJSONV.Json.Serialize(options), placementId);
	}

	public bool CloseAd(string placementId)
	{
		return _vungleCloseAd(placementId);
	}
	#endregion

	#region AdLifeCycle-Banner
	public void LoadBanner(string placementId, Vungle.VungleBannerSize adSize, Vungle.VungleBannerPosition adPosition)
	{
		_vungleLoadBanner(placementId, (int)adSize, (int)adPosition);
	}

	public void ShowBanner(string placementId)
	{
		_vungleShowBanner(placementId);
	}

	public void CloseBanner(string placementId)
	{
		_vungleCloseBanner(placementId);
	}
	#endregion

	#region PlaybackOptions
	public void SetSoundEnabled(bool isEnabled)
	{
		_vungleSetSoundEnabled(isEnabled);
	}
	#endregion

	#region PauseAndResumeHandling
	public void OnResume()
	{
		return;
	}

	public void OnPause()
	{
		return;
	}
	#endregion

	#region DllImports
	[DllImport("__Internal")]
	private static extern string _vungleGetSdkVersion();

	[DllImport("__Internal")]
	private static extern void _vungleStartWithAppId(string appId, string pluginVersion, bool initHeaderBiddingDelegate);

	[DllImport("__Internal")]
	private static extern void _vungleSetPublishIDFV(bool shouldEnable);

	[DllImport("__Internal")]
	private static extern void _vungleSetMinimumSpaceForInit(int minimumSize);

	[DllImport("__Internal")]
	private static extern void _vungleSetMinimumSpaceForAd(int minimumSize);

	[DllImport("__Internal")]
	private static extern void _updateConsentStatus(int consent, string version);

	[DllImport("__Internal")]
	private static extern int _getConsentStatus();

	[DllImport("__Internal")]
	private static extern void _updateCCPAStatus(int consent);

	[DllImport("__Internal")]
	private static extern int _getCCPAStatus();

	[DllImport("__Internal")]
	private static extern void _vungleSetSoundEnabled(bool enabled);

	[DllImport("__Internal")]
	private static extern void _vungleEnableLogging(bool shouldEnable);

	[DllImport("__Internal")]
	private static extern bool _vungleIsAdAvailable(string placementID);

	[DllImport("__Internal")]
	private static extern bool _vungleIsBannerAvailable(string placementID, int bannerSize);

	[DllImport("__Internal")]
	private static extern void _vungleLoadAd(string placementID);

	[DllImport("__Internal")]
	private static extern void _vunglePlayAd(string options, string placementID);

	[DllImport("__Internal")]
	private static extern bool _vungleCloseAd(string placementID);

	[DllImport("__Internal")]
	private static extern void _vungleLoadBanner(string placementID, int bannerSize, int bannerPosition);

	[DllImport("__Internal")]
	private static extern void _vungleShowBanner(string placementID);

	[DllImport("__Internal")]
	private static extern void _vungleCloseBanner(string placementID);
	#endregion
}
#endif
