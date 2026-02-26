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
    [SerializeField] GOAPAgent script_GOADAgent;

    [Header("Canvas References")]
    public GameObject happinessImg;
    public GameObject sadnessImg;
    public GameObject busyImg;

    public void SetModeDesactivation()
    {
        happinessImg.SetActive(false);
        sadnessImg.SetActive(false);
        busyImg.SetActive(false);
    }
    public void SetMood(EmotionReferenceInAgent mood)
    {
        if (mood == EmotionReferenceInAgent.HAPPYNESS)
        {
            SetModeDesactivation();
            happinessImg.SetActive(true);
        }
        else if (mood == EmotionReferenceInAgent.SADNESS)
        {
            SetModeDesactivation();
            sadnessImg.SetActive(true);
        }
        else if (mood == EmotionReferenceInAgent.BUSY)
        {
            SetModeDesactivation();
            busyImg.SetActive(true);
        }
    }
}
