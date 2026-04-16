using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Cell : MonoBehaviour
{
    public int index = 0;
    public int idealDistance = 0;
    public int[] neighborIndex = new int[4];
    public TMP_Text indexText;
    public TMP_Text idealDistanceText;
    public bool dirty = false;
    private MeshRenderer meshRenderer;
    public enum State
    {
        NEUTRAL,
        START,
        END,
        OBSTACLE
    }
    public State enum_state;
    public void Start()
    {
        enum_state = State.NEUTRAL;
        meshRenderer = GetComponent<MeshRenderer>();
        SetState(0);

    }
    public void SetIndex(int index)
    {
        this.index = index;
        indexText.text = index.ToString();
    }
    public void SetNeigbor(int n, int e, int s, int w)
    {

        neighborIndex[0] = n;
        neighborIndex[1] = e;
        neighborIndex[2] = s;
        neighborIndex[3] = w;
        indexText.text = index.ToString();

        dirty = true;
    }
    public Color[] colors = new Color[4]; 
    public void SetState(int stateIndex)

    {
        enum_state = (State)stateIndex;
        meshRenderer.material.color = colors[stateIndex];
        if (enum_state == State.START)
        {
            AStar.start = this;
        }
        if(enum_state == State.END)
        {
            AStar.end = this;
        }
    }
    public void SetIdealDistance(Vector3 endPosition)
    {
        Vector3 cleanCoordinates = endPosition - transform.position;
        idealDistance = (int)(cleanCoordinates.x + cleanCoordinates.z);
        idealDistanceText.text = idealDistance.ToString();
    }
}
