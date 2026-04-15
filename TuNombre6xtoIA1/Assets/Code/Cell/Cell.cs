using UnityEngine;
using TMPro;
public class Cell : MonoBehaviour
{
    public int index = 0;
    public int[] neighborIndex = new int[4];
    public TMP_Text indexText;
    public bool dirty = false;
    public void SetIndex(int index)
    {
        this.index = index;
        indexText.text=index.ToString();
    }
    public void SetNeigbor(int n, int e, int s, int w)
    {
        
        neighborIndex[0] = n;
        neighborIndex[1] = e;
        neighborIndex[2] = s;
        neighborIndex[3] = w;
        indexText.text =index.ToString();

        dirty = true;
    }

}
