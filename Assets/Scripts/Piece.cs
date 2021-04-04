using UnityEngine;
using static Enums;

public class Piece : MonoBehaviour
{
    [SerializeField]
    protected Variant variant = Variant.x4;
    protected int variantInt;
    public int index {get; set;}
    public Material materialTemplate;
    protected Material material;
    protected Material materialForMistakes;
    public Color color {get; protected set;}
    protected Color colorForMistakes;

    // below for color change in time 
    protected Color destinedColor;
    protected Color previousColor;
    protected float timeForColorUpdate;
    protected float timerColor;

    private float transparency = 1;

    // below for transparency change in time
    private float destinedTransparency;
    private float timeForTransparencyUpdate;
    private float previousTransparency;
    private bool destroyOnFinishTransparencyUpdate;
    private float timerTrans;

    protected bool[,] setting;
    protected byte[,] shiftedCubes;

    protected GameObject[,] children;

    public Variant GetVariant() {
        return variant;
    }

    public void ChangeSetting(bool[,] setting_) {
        if (children == null)
            FindChildren();

        if (setting_ == null) return;
        if (setting_.GetLength(0) != variantInt) {
            Debug.Log("Incorrect setting size\nGot: " + setting_.GetLength(0) + ", expected: " + variantInt);
            return;
        }
        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                if (setting[i, j] != setting_[i, j]) {
                    setting = (bool[,])setting_.Clone();
                    ChangeSetting();
                    return;
                }
            }
        }
    }
    protected void ChangeSetting() {
        if (children[0, 0] == null)
            FindChildren();

        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                if (setting[i, j]){
                    children[i, j].SetActive(true);
                }else{
                    children[i, j].SetActive(false);
                }
            }
        }
        PrepareSidesVisibility();
    }
    public virtual void ChangeSettingForShift(bool [,] arrang = null) {
        ChangeSetting(arrang);
    }

    public virtual void ChangeLayer(int layer) {
        gameObject.layer = layer;
        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                children[i, j].layer = layer;
                for (int k = 0; k < children[i, j].transform.childCount; k++) {
                    children[i, j].transform.GetChild(k).gameObject.layer = layer;
                }
            }
        }
    }

    protected virtual void PrepareSidesVisibility() {
        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                // ustaw wszystko na false, wymagane w przypadku zmian w setting
                children[i, j].transform.Find("Top").gameObject.SetActive(false);
                children[i, j].transform.Find("Bottom").gameObject.SetActive(false);
                children[i, j].transform.Find("Left").gameObject.SetActive(false);
                children[i, j].transform.Find("Right").gameObject.SetActive(false);

                if (children[i, j].activeSelf) {
                    GameObject top = null, left = null, right = null, bottom = null;
                    if (i > 0) bottom = children[i - 1, j];
                    if (i < variantInt - 1) top = children[i + 1, j];
                    if (j > 0) left = children[i, j - 1];
                    if (j < variantInt - 1) right = children[i, j + 1];

                    if (top == null || !top.activeSelf) {
                        children[i, j].transform.Find("Top").gameObject.SetActive(true);
                    }
                    if (bottom == null || !bottom.activeSelf) {
                        children[i, j].transform.Find("Bottom").gameObject.SetActive(true);
                    }
                    if (left == null || !left.activeSelf) {
                        children[i, j].transform.Find("Left").gameObject.SetActive(true);
                    }
                    if (right == null || !right.activeSelf) {
                        children[i, j].transform.Find("Right").gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public virtual void RotateClockwise(int times) {
        times %= 4;
        int l = variantInt - 1;
        if (times % 2 == 1) {
            for (int b = 0; b < l; b++) {
                bool temp = setting[b, 0];
                // x increasing, y static min
                setting[b, 0] = setting[l, b];
                // x static max, y increasing
                setting[l, b] = setting[l - b, l];
                // x decreasing, y static max
                setting[l - b, l] = setting[0, l - b];
                // x static min, y decreasing
                setting[0, l - b] = temp;
            }
            times--;
        }
        if (times == 2) {
            for (int b = 0; b < l; b++) {
                bool temp = setting[b, 0];
                // x increasing, y static min
                setting[b, 0] = setting[l - b, l];
                // x decreasing, y static max
                setting[l - b, l] = temp;
                temp = setting[l, b];
                // x static max, y increasing
                setting[l, b] = setting[0, l - b];
                // x static min, y decreasing
                setting[0, l - b] = temp;
            }
        }
        ChangeSetting();
    }

    public bool[,] GetSetting() {
        return setting;
    }

    protected virtual void FindChildren() {
        if (variant == Variant.x3) {
            variantInt = 3;
            setting = new bool[3, 3]{
                {I, I, I}, 
                {I, I, I},
                {I, I, I}};
            children = new GameObject[3, 3];
        } else if (variant == Variant.x4) {
            variantInt = 4;
            setting = new bool[4, 4]{
                {I, I, I ,I}, 
                {I, I, I, I},
                {I, I, I, I},
                {I, I, I, I}};
            children = new GameObject[4, 4];
        }

        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                children[i, j] = transform.GetChild(i * variantInt + j).gameObject;
            }
        }
    }

    protected virtual void AssignStandardMaterial() {
        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                for (int k = 0; k < 6; k++) {
                    children[i, j].transform.GetChild(k).GetComponent<MeshRenderer>().material = material;
                }
            }
        }
    }
    protected virtual void ChangeMaterialFor(bool[,] arrang) {
        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                if (arrang[i, j])
                for (int k = 0; k < 6; k++) {
                    children[i, j].transform.GetChild(k).GetComponent<MeshRenderer>().material = materialForMistakes;
                }
            }
        }
    }

    public Material GetMaterial() {
        return material;
    }

    public virtual void ChangeColor(Color color_) {
        color = new Color(color_.r, color_.g, color_.b, transparency);
        material.SetColor("_Color", color);
        AssignStandardMaterial(); //??
    }
    public virtual void RetrieveColor(){
        AssignStandardMaterial();
    }
    public virtual void ChangeColorFor(Color color_, float trans, bool[,] arrang) {
        colorForMistakes = new Color(color_.r, color_.g, color_.b, trans);
        materialForMistakes.SetColor("_Color", colorForMistakes);
        ChangeMaterialFor(arrang);
    }
    public void ChangeColorFor(Color color_, bool[,] arrang) {
        ChangeColorFor(color_, transparency, arrang);
    }
    public virtual void ChangeTransparency(float trans, bool abandonChangeInTime = false) {
        if (abandonChangeInTime) duringTransparencyUpdate = false;
        transparency = trans;
        ChangeColor(new Color(color.r, color.g, color.b, trans));
    }
    private bool duringTransparencyUpdate = false;
    public virtual void ChangeTransparencyInTime(float destinedTransparency, float time, bool destroyOnFinish = false) {
        duringTransparencyUpdate = true;
        this.destinedTransparency = destinedTransparency;
        previousTransparency = transparency;
        timeForTransparencyUpdate = time;
        timerTrans = 0f;
        destroyOnFinishTransparencyUpdate = destroyOnFinish;
    }
    protected bool duringColorUpdate = false;
    public void ChangeColorInTime(Color destinedColor, float time) {
        duringColorUpdate = true;
        this.destinedColor = destinedColor;
        previousColor = color;  // might not work
        timeForColorUpdate = time;
        timerColor = 0f;
    }
    public float GetTransparency() {
        return transparency;
    }

    protected void ColorUpdateContinous() {
        float rate = Time.deltaTime / timeForColorUpdate;
        Color newColor = new Color();
        newColor.r = color.r + (destinedColor.r - previousColor.r) * rate;
        newColor.g = color.g + (destinedColor.g - previousColor.g) * rate;
        newColor.b = color.b + (destinedColor.b - previousColor.b) * rate;
        newColor.a = previousColor.a;
        ChangeColor(newColor);
        timerColor += Time.deltaTime;
        if (timerColor >= timeForColorUpdate) duringColorUpdate = false;
    }

    protected virtual void Awake() {
        material = new Material(materialTemplate);
        material.name = "Standard Material";
        materialForMistakes = new Material(materialTemplate);
        materialForMistakes.name = "Material For Mistakes";
        materialForMistakes.SetFloat("_Mode", 0); // set to opaque mode
        color = material.GetColor("_Color");
        
        FindChildren();
        AssignStandardMaterial();
    }

    protected virtual void Update()
    {
        if (duringTransparencyUpdate) {
            float rate = Time.deltaTime / timeForTransparencyUpdate;
            float entireChange = destinedTransparency - previousTransparency;
            float changeForFrame = entireChange * rate;
            ChangeTransparency(transparency + changeForFrame);
            timerTrans += Time.deltaTime;
            if (timerTrans >= timeForTransparencyUpdate) duringTransparencyUpdate = false;
        }
        if (!duringTransparencyUpdate && destroyOnFinishTransparencyUpdate) {
            Destroy(this.gameObject);
        }
        if (duringColorUpdate) {
            ColorUpdateContinous();
        }

        if (duringDecay) {
            DecayContinous();
        }
    }

    // Methods and fields for piece decay
    [Header("Decay:")]
    public float rangeForThrowDirections = 10f;
    public float gravityStrength = 0.5f;
    public float throwStrength = 10f;
    public float throwStrengthScope = 5f;
    public float timeToDisappear;
    public float timeToDisappearScope;
    protected Vector3[] directionsForThrow;
    protected bool duringDecay = false;

    public virtual void InitializeDecay(float time) {
        directionsForThrow = new Vector3[variantInt * variantInt];
        timeToDisappear = time;

        for (int i = 0; i < variantInt * variantInt; i++) {
            int x = i / variantInt, y = i % variantInt;
            directionsForThrow[i] = transform.up * (throwStrength + Random.Range(-throwStrengthScope, throwStrengthScope));
            Quaternion rotation = Quaternion.AngleAxis(Random.Range(-rangeForThrowDirections, rangeForThrowDirections), transform.up) *
                                    Quaternion.AngleAxis(Random.Range(-rangeForThrowDirections, rangeForThrowDirections), transform.right) *
                                    Quaternion.AngleAxis(Random.Range(-rangeForThrowDirections, rangeForThrowDirections), transform.forward);
            directionsForThrow[i] = rotation * directionsForThrow[i];

            children[x, y].LeanScale(Vector3.zero, 1f).delay = timeToDisappear + Random.Range(-timeToDisappearScope, timeToDisappearScope);

            children[x, y].transform.Find("Bottom").gameObject.SetActive(true);
            children[x, y].transform.Find("Back").gameObject.SetActive(true);
            children[x, y].transform.Find("Left").gameObject.SetActive(true);
            children[x, y].transform.Find("Right").gameObject.SetActive(true);
            children[x, y].transform.Find("Front").gameObject.SetActive(true);
            children[x, y].transform.Find("Top").gameObject.SetActive(true);
        }
        LeanTween.delayedCall(timeToDisappear + timeToDisappearScope + 1f, () => {/* ChangeTransparencyInTime(1f, 1f, true); */ Destroy(this.gameObject);});

        duringDecay = true;
    }

    protected virtual void DecayContinous() {
        for (int i = 0; i < variantInt * variantInt; i++) {
            int x = i / variantInt, y = i % variantInt;
            // only if active
            if (setting[x, y]) {
                children[x, y].transform.Translate(directionsForThrow[i] * Time.deltaTime, Space.World);
                directionsForThrow[i] += Vector3.down * Time.deltaTime * gravityStrength;
            }
        }
    }
}
