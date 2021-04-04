using System.IO;
using System.Xml.Serialization;
using UnityEngine;

[RequireComponent(typeof(GameScript))]
public class SaveScript : MonoBehaviour
{
    public const string LEVEL4_PREF = "levels", LEVEL3_PREF = "levels3";
    public const string INFO4_PREF = "info";
    public const string INFO3_PREF = "info3";
    public const string GAME_DATA_PREF = "game_data";
    public const string LEVEL4_FILENAME = "Levels4";
    public const string LEVEL3_FILENAME = "Levels3";
    public SaveInfoState saveInfo4State {get; set;}
    public SaveInfoState saveInfo3State {get; set;}
    public SaveLevelState saveLevel4State {get; set;}
    public SaveLevelSetting saveLevel4Setting {get; set;}
    public SaveLevelState saveLevel3State {get; set;}
    public SaveLevelSetting saveLevel3Setting {get; set;}
    public SaveGameDataState saveGameDataState {get; set;}


    private T DeserializeStateFromFile<T>(string filename) {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        TextAsset textAsset = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
        StringReader reader = new StringReader(textAsset.text);
        T deserialized = (T)xml.Deserialize(reader);
        reader.Close();
        return deserialized;
    }
    private string SerializeStateToStr<T>(T toSerialize) {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        StringWriter writer = new StringWriter();
        xml.Serialize(writer, toSerialize);
        string serialized = writer.ToString();
        writer.Close();
        return serialized;
    }
    private T DeserializeStateFromStr<T>(string toDeserialize) {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        StringReader reader = new StringReader(toDeserialize);
        T deserialized = (T)xml.Deserialize(reader);
        reader.Close();
        return deserialized;
    }



    public void LoadLevelData() {
        saveLevel4Setting = DeserializeStateFromFile<SaveLevelSetting>(LEVEL4_FILENAME);
        for (int i = 0; i < saveLevel4Setting.levelList.Count; i++) {
            saveLevel4Setting.levelList[i].level = i;
        }
        saveLevel3Setting = DeserializeStateFromFile<SaveLevelSetting>(LEVEL3_FILENAME);
        for (int i = 0; i < saveLevel3Setting.levelList.Count; i++) {
            saveLevel3Setting.levelList[i].level = i;
        }
        if (PlayerPrefs.HasKey(LEVEL4_PREF)) {
            saveLevel4State = DeserializeStateFromStr<SaveLevelState>(PlayerPrefs.GetString(LEVEL4_PREF));
            // in case of adding more levels
            if (saveLevel4Setting.levelList.Count > saveLevel4State.levelList.Count) {
                for (int i = saveLevel4State.levelList.Count; i < saveLevel4Setting.levelList.Count; i++) {
                    LevelStates ls = new LevelStates();
                    ls.level = saveLevel4Setting.levelList[i].level;  ls.finished = false;
                    saveLevel4State.levelList.Add(ls);
                }
            }
        } else {
            saveLevel4State = new SaveLevelState();
            foreach (var item in saveLevel4Setting.levelList) {
                LevelStates ls = new LevelStates();
                ls.level = item.level;  ls.finished = false;
                saveLevel4State.levelList.Add(ls);
            }
        }
        if (PlayerPrefs.HasKey(LEVEL3_PREF)) {
            saveLevel3State = DeserializeStateFromStr<SaveLevelState>(PlayerPrefs.GetString(LEVEL3_PREF));
            // in case of adding more levels
            if (saveLevel3Setting.levelList.Count > saveLevel3State.levelList.Count) {
                for (int i = saveLevel3State.levelList.Count; i < saveLevel3Setting.levelList.Count; i++) {
                    LevelStates ls = new LevelStates();
                    ls.level = saveLevel3Setting.levelList[i].level;  ls.finished = false;
                    saveLevel3State.levelList.Add(ls);
                }
            }
        } else {
            saveLevel3State = new SaveLevelState();
            foreach (var item in saveLevel3Setting.levelList) {
                LevelStates ls = new LevelStates();
                ls.level = item.level;  ls.finished = false;
                saveLevel3State.levelList.Add(ls);
            }
        }
        SaveLevelData();
    }
    public void SaveLevelData() {
        PlayerPrefs.SetString(LEVEL4_PREF, SerializeStateToStr<SaveLevelState>(saveLevel4State));
        PlayerPrefs.SetString(LEVEL3_PREF, SerializeStateToStr<SaveLevelState>(saveLevel3State));
    }
    


    public void LoadInfoData() {
        if (PlayerPrefs.HasKey(INFO4_PREF)) {
            saveInfo4State = DeserializeStateFromStr<SaveInfoState>(PlayerPrefs.GetString(INFO4_PREF));
        } else {
            saveInfo4State = new SaveInfoState();
            saveInfo4State.randomAverageTime = 0f;
            saveInfo4State.randomGamesWon = 0;
            saveInfo4State.randomShortestTime = 0f;
        }
        if (PlayerPrefs.HasKey(INFO3_PREF)) {
            saveInfo3State = DeserializeStateFromStr<SaveInfoState>(PlayerPrefs.GetString(INFO3_PREF));
        } else {
            saveInfo3State = new SaveInfoState();
            saveInfo3State.randomAverageTime = 0f;
            saveInfo3State.randomGamesWon = 0;
            saveInfo3State.randomShortestTime = 0f;
        }
    }
    public void SaveInfoData() {
        PlayerPrefs.SetString(INFO4_PREF, SerializeStateToStr<SaveInfoState>(saveInfo4State));
        PlayerPrefs.SetString(INFO3_PREF, SerializeStateToStr<SaveInfoState>(saveInfo3State));
    }



    public void LoadGameData() {
        // if (PlayerPrefs.HasKey(GAME_DATA_PREF)) {
        //     saveGameDataState = DeserializeStateFromStr<SaveGameDataState>(PlayerPrefs.GetString(GAME_DATA_PREF));
        // } else {
        //     saveGameDataState = new SaveGameDataState();
        //     saveGameDataState.SetTheme = Enums.Themes.BasicStone;
        // }
        saveGameDataState = new SaveGameDataState();
        GetComponent<GameScript>().ChangeTheme(saveGameDataState.SetTheme);
    }
    // public void SaveGameData() {
    //     PlayerPrefs.SetString(GAME_DATA_PREF, SerializeStateToStr<SaveGameDataState>(saveGameDataState));
    // }


}
