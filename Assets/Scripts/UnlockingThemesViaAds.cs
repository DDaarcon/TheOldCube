using UnityEngine;
using UnityEngine.Advertisements;
using static Enums;

[RequireComponent(typeof(GameScript))]
[RequireComponent(typeof(SaveScript))]
public class UnlockingThemesViaAds : MonoBehaviour, IUnityAdsListener
{

    public SaveScript saveScript;
    private readonly string myPlacementID = "rewardedVideo";
    private ThemeButton themeButton;
    private SaveGameDataState.AdsUnlock themeData;

    public void ShowAd(ThemeButton themeButton) {
        Advertisement.AddListener(this);
        this.themeButton = themeButton;
        switch (themeButton.themeToApply) {
            case Themes.BlueShiny:
                themeData = saveScript.saveGameDataState.blueShinyTheme;
                break;
            case Themes.DarkElement:
                themeData = saveScript.saveGameDataState.darkElementTheme;
                break;
            case Themes.TicTacToe:
                themeData = saveScript.saveGameDataState.ticTacToeTheme;
                break;
            default:
                this.themeButton = null;
                return;
        }

        if (themeData.adsLeft > 0) {
            if (Advertisement.IsReady(myPlacementID)) {
                Advertisement.Show(myPlacementID);
            } else {
                ToastScript.AddMsg("Ad not available", 2f);
                Finish(false);
            }
        } else {
            Finish(false);
        }

    }
    private void Finish(bool completed) {
        if (completed) {
            themeData.AdsLeftOneLess();
            themeButton.SetProperNote();
        }
        themeButton = null;
        themeData = null;
        Advertisement.RemoveListener(this);
    }

    private void Start() {
    }

    public void OnUnityAdsReady(string placementId) {
        // if (placementId == myPlacementID) {

        // }
    }
    public void OnUnityAdsDidError(string placementId) {
        Finish(false);
    }
    public void OnUnityAdsDidStart(string placementId) {
    }
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
        if (showResult == ShowResult.Finished) {
            Finish(true);
        } else if (showResult == ShowResult.Skipped) {
            Finish(false);
        } else if (showResult == ShowResult.Failed) {
            Finish(false);
        }
    }

    private void OnDestroy() {
    }

}
