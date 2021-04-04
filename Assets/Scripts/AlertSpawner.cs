using UnityEngine;
using TMPro;

public class AlertSpawner : MonoBehaviour
{
    public TMP_Text textPrefab;
    public float time;

    public void SpawnText(int level) {
        SpawnText("Level " + level + " passed!");
    }
    public void SpawnText(string content) {
        Vector3[] corners = new Vector3[4];
        this.GetComponent<RectTransform>().GetWorldCorners(corners);

        GameObject alertInstance = Instantiate(textPrefab.gameObject, this.transform);
        alertInstance.transform.position = corners[0];
        alertInstance.GetComponent<TMP_Text>().text = content;

        LeanTween.move(alertInstance, corners[1], time).destroyOnComplete = true;
        LeanTween.value(alertInstance, (float val) => {
            Color color = alertInstance.GetComponent<TMP_Text>().color;
            color.a = val;
            alertInstance.GetComponent<TMP_Text>().color = color;
            }, 0f, 0.7f, time * 0.5f);
        LeanTween.value(alertInstance, (float val) => {
            Color color = alertInstance.GetComponent<TMP_Text>().color;
            color.a = val;
            alertInstance.GetComponent<TMP_Text>().color = color;
            }, 0.7f, 0f, time * 0.3f).delay = time * 0.7f;
        LeanTween.scale(alertInstance, Vector3.one * 1.3f, time);

    }

    private void Update() {
    }
}
