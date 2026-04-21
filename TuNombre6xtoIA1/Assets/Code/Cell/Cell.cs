using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private int g = 0;
    private int h = 0;
    private int f => g + h;

    public Cell parent = null;

    public int[] neighborIndex = new int[4];
    public TMP_Text indexText;
    public TMP_Text hText;
    public TMP_Text gText;
    public TMP_Text fText;
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
        this.g = index;
        indexText.text = index.ToString();
    }
    public void SetNeigbor(int n, int e, int s, int w)
    {

        neighborIndex[0] = n;
        neighborIndex[1] = e;
        neighborIndex[2] = s;
        neighborIndex[3] = w;
        indexText.text = g.ToString();

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
        if (enum_state == State.END)
        {
            AStar.end = this;
        }
    }
    public void SetH(Vector3 endPosition)
    {
        Vector3 cleanCoordinates = endPosition - transform.position;
        h = Mathf.Abs((int)(cleanCoordinates.x) + Mathf.Abs((int)cleanCoordinates.z));
        RefreshText();
    }
    public void SetG(int value)
    {
        g = value;
        RefreshText();

    }
    public void ResetPathData()
    {
        g = 0;
        h = 0;
        parent = null;
        RefreshText();
    }
    void RefreshText()
    {
        hText.text = h.ToString();
        gText.text = g.ToString();
        fText.text = f.ToString();
    }
    public int F() { return f; }
    public int H() { return h; }
    public int G() { return g; }
}
