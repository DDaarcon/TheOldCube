using static Enums;
using UnityEngine;
public class SaveGameDataState
{
	public class LevelUnlock {
		public string unlockKey {get; private set;}
		public bool unlocked {get; private set;}

		public LevelUnlock(string unlockKey) {
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
    // public bool goldThemeUnlocked;
	// public bool goldThemePostTry;
	public LevelUnlock minecrafTheme = new LevelUnlock("MINECRAFT_TH");
    // public bool minecraftThemeUnlocked;
	// public bool minecraftThemePostTry;
	public LevelUnlock redShinyTheme = new LevelUnlock("REDSHINY_TH");
	// public bool redShinyThemeUnlocked;
	// public bool redShinyThemePostTry;
	public LevelUnlock copperTheme = new LevelUnlock("COPPER_TH");
	// public bool copperThemeUnlocked;
	// public bool copperThemePostTry;
	public AdsUnlock blueShinyTheme = new AdsUnlock("BLUESHINY_TH", "BLUESHINY_ADS", 10);
	// public bool blueShinyThemeUnlocked;
	// public bool blueShinyThemePostTry;
	// public int blueShinyAdsLeft;
	public AdsUnlock darkElementTheme = new AdsUnlock("DARKELEMENT_TH", "DARKELEMENT_ADS", 15);
	// public bool darkElementThemeUnlocked;
	// public bool darkElementThemePostTry;
	// public int darkElementAdsLeft;
	public AdsUnlock ticTacToeTheme = new AdsUnlock("TICTACTOE_TH", "TICTACTOE_ADS", 20);
	// public bool ticTacToeThemeUnlocked;
	// public bool ticTacToeThemePostTry;
	// public int ticTacToeAdsLeft;

	public SaveGameDataState() {
		if (PlayerPrefs.HasKey(SET_THEME_KEY)) {
			SetTheme = (Themes)PlayerPrefs.GetInt(SET_THEME_KEY);
		} else {
			SetTheme = Themes.BasicStone;
		}
	}


	// public void SetPostTry(Themes themes, bool on) {
	// 	switch (themes) {
	// 		case Themes.BasicStone:
	// 		default:
	// 			break;
	// 		case Themes.Gold:
	// 			goldThemePostTry = on;
	// 			break;
	// 		case Themes.Minecraft:
	// 			minecraftThemePostTry = on;
	// 			break;
	// 		case Themes.RedShiny:
	// 			redShinyThemePostTry = on;
	// 			break;
	// 		case Themes.Copper:
	// 			copperThemePostTry = on;
	// 			break;
	// 		case Themes.BlueShiny:
	// 			blueShinyThemePostTry = on;
	// 			break;
	// 		case Themes.DarkElement:
	// 			darkElementThemePostTry = on;
	// 			break;
	// 		case Themes.TicTacToe:
	// 			ticTacToeThemePostTry = on;
	// 			break;
	// 	}
	// }
	// public void SetUnlocked(Themes themes, bool on) {
	// 	switch (themes) {
	// 		case Themes.BasicStone:
	// 		default:
	// 			break;
	// 		case Themes.Gold:
	// 			goldThemeUnlocked = on;
	// 			break;
	// 		case Themes.Minecraft:
	// 			minecraftThemeUnlocked = on;
	// 			break;
	// 		case Themes.RedShiny:
	// 			redShinyThemeUnlocked = on;
	// 			break;
	// 		case Themes.Copper:
	// 			copperThemeUnlocked = on;
	// 			break;
	// 		case Themes.BlueShiny:
	// 			blueShinyThemeUnlocked = on;
	// 			break;
	// 		case Themes.DarkElement:
	// 			darkElementThemeUnlocked = on;
	// 			break;
	// 		case Themes.TicTacToe:
	// 			ticTacToeThemeUnlocked = on;
	// 			break;
	// 	}
	// }

}
