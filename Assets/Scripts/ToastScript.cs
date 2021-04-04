using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class ToastScript : MonoBehaviour
{
    public TMP_Text msgContent;

    struct MsgInfo {
        public string text;
        public float duration;
    }
    static List<MsgInfo> msgInfoArr;

    float passedTime;
    bool showedMsg = false;
    CanvasGroup canvasGroup;

    private void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        msgInfoArr = new List<MsgInfo>();
    }

    /**
    <summary>Add a toast message to a QUEUE</summary>
    **/
    public static void AddMsg(string text, float duration) {
        if (msgInfoArr.Count > 3) return;
        MsgInfo msgInfo;
        msgInfo.text = text; msgInfo.duration = duration;
        msgInfoArr.Add(msgInfo);
    }

    private void Update() {
        if (msgInfoArr.Count > 0) {
            if (!LeanTween.isTweening(canvasGroup.gameObject)) {
                if (!showedMsg && canvasGroup.alpha == 0f) {
                    msgContent.text = msgInfoArr[0].text;
                    LeanTween.alphaCanvas(canvasGroup, 1f, 0.3f);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
                }
                else if (!showedMsg && canvasGroup.alpha == 1f) {
                    passedTime += Time.deltaTime;
                    if (passedTime >= msgInfoArr[0].duration) {
                        showedMsg = true;
                        passedTime = 0f;
                        LeanTween.alphaCanvas(canvasGroup, 0f, 0.3f);
                    }
                }
                else if (showedMsg && canvasGroup.alpha == 0f) {
                    msgInfoArr.RemoveAt(0);
                    showedMsg = false;
                }
                
            }
        }
    }
}
