using static Enums;
using UnityEngine;
public class SaveGameDataState
{
	public class LevelUnlock {
		public string unlockKey {get; private set;}
		public bool unlocked {get; private set;}

		public LevelUnlock(string unlockKey) {
			this.unlockKey = unlockKey;
			if (PlayerPrefs.HasKey(unlockKey)) {
				unlocked = PlayerPrefs.GetInt(unlockKey) == 1;
			} else {
				PlayerPrefs.SetInt(unlockKey, 0);
				unlocked = false;
			}
		}
		public void Unlock() {
			unlocked = true;
			PlayerPrefs.SetInt(unlockKey, 1);
		}
	}

	public class AdsUnlock : LevelUnlock {
		public string adsLeftKey {get; private set;}
		public int adsLeft {get; private set;}

		public AdsUnlock(string unlockKey, string adsLeftKey, int adsOnStart) : base(unlockKey) {
			this.adsLeftKey = adsLeftKey;
			if (PlayerPrefs.HasKey(adsLeftKey)) {
				adsLeft = PlayerPrefs.GetInt(adsLeftKey);
			} else {
				PlayerPrefs.SetInt(adsLeftKey, adsOnStart);
				adsLeft = adsOnStart;
			}
		}

		public void AdsLeft(int left) {
			adsLeft = left;
			PlayerPrefs.SetInt(adsLeftKey, adsLeft);
		}

		public void AdsLeftOneLess() {
			AdsLeft(adsLeft - 1);
		}
	}

	private readonly string SET_THEME_KEY = "SET_THEME";
    private Themes setTheme;
	public Themes SetTheme {
		get {return setTheme;}
		set {setTheme = value; PlayerPrefs.SetInt(SET_THEME_KEY, (int)value);}
	}


	public LevelUnlock goldTheme = new LevelUnlock("GOLD_TH");
	public LevelUnlock minecrafTheme = new LevelUnlock("MINECRAFT_TH");
	public LevelUnlock redShinyTheme = new LevelUnlock("REDSHINY_TH");
	public LevelUnlock copperTheme = new LevelUnlock("COPPER_TH");
	public AdsUnlock blueShinyTheme = new AdsUnlock("BLUESHINY_TH", "BLUESHINY_ADS", 10);
	public AdsUnlock darkElementTheme = new AdsUnlock("DARKELEMENT_TH", "DARKELEMENT_ADS", 15);
	public AdsUnlock ticTacToeTheme = new AdsUnlock("TICTACTOE_TH", "TICTACTOE_ADS", 20);

	public SaveGameDataState() {
		if (PlayerPrefs.HasKey(SET_THEME_KEY)) {
			SetTheme = (Themes)PlayerPrefs.GetInt(SET_THEME_KEY);
		} else {
			SetTheme = Themes.BasicStone;
		}
	}
}
