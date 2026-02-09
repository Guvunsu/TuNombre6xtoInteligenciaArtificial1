using UnityEngine;
using UnityEngine.UI;
public class UICanvasEmotions : MonoBehaviour
{
    public enum EmotionReferenceInAgent
    {
        HAPPYNESS,
        SADNESS,
        BUSY
    }
    [Header("Referencia GOAPAction")]
    [SerializeField] GOAPAgent script_GOAPAgent;

    [Header("Canvas References")]
    public GameObject happinessImg;
    public GameObject sadnessImg;
    public GameObject busyImg;

    public void SetMood(EmotionReferenceInAgent mood)
    {
        happinessImg.SetActive(false);
        sadnessImg.SetActive(false);
        busyImg.SetActive(false);

        if (mood == EmotionReferenceInAgent.HAPPYNESS) happinessImg.SetActive(true);
        else if (mood == EmotionReferenceInAgent.SADNESS) sadnessImg.SetActive(true);
        else if (mood == EmotionReferenceInAgent.BUSY) busyImg.SetActive(true);
    }
}
