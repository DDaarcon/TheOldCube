using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class LevelSettings {
    [XmlIgnore]
    public int level {get; set;} = 0;
    [XmlAttribute("seed")]
    public int seed {get; set;} = 1;
    // [XmlArray("Placed_Sides")]
    // [XmlArrayItem("Side")]
    [XmlIgnore]
    public bool[] placedSides {get; set;} = new bool[6];
    [XmlElement("Placed_Sides")]
    public string placedSiedsStr {get; set;}
    [XmlIgnore]
    public bool finished {get; set;} = false;

    [XmlElement("Diff")]
    public int difficulty {get; set;} = 0;

    public LevelSettings(int number, int seed, bool[] placedSides, bool finished, string content, int diff) {
        level = number;
        this.seed = seed;
        this.placedSides = placedSides.Clone() as bool[];
        this.finished = finished;
        // this.color = color;
        this.difficulty = diff;
    }
    public LevelSettings() {
    }
}

public class LevelStates {
    [XmlAttribute("level")]
    public int level {get; set;} = 0;
    [XmlAttribute("isFinished")]
    public bool finished {get; set;} = false;
}

public class SaveLevelState
{
    public List<LevelStates> levelList = new List<LevelStates>();

}

public class SaveLevelSetting {
    public List<LevelSettings> levelList = new List<LevelSettings>();
}
