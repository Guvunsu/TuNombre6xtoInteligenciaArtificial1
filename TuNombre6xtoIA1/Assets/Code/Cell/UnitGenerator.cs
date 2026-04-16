using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    public int size = 4;
    public GameObject[] grid;
    public GameObject cellprefb;
    void Start()
    {
        grid = new GameObject[size * size];
        GenerateGrid();
        CreteNeighboor();
    }
    void GenerateGrid()
    {
        int index = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject instance = Instantiate(cellprefb);
                instance.transform.position = new Vector3(j, 0, i);

                instance.GetComponent<Cell>().SetIndex(index);
                grid[index] = instance;
                index++;
            }
        }
    }
    void CreteNeighboor()
    {
        int temp_index;

        //esquina izq up 
        grid[0].GetComponent<Cell>().SetNeigbor(size, 1, -1, -1);
        temp_index = size - 1;
        //  esquina derecha abajo
        grid[temp_index].GetComponent<Cell>().SetNeigbor(temp_index + size, -1, -1, temp_index - 1);
        temp_index = (size * size) - size;
        //esquina izquierdo arriba
        grid[temp_index].GetComponent<Cell>().SetNeigbor(-1, temp_index + 1, temp_index - size, -1);
        //esquina  derecho arriba
        temp_index = (size * size) - 1;
        grid[temp_index].GetComponent<Cell>().SetNeigbor(-1, -1, temp_index - size, temp_index - 1);

        //bottom
        for (int i = 1; i < size - 1; i++)
        {
            grid[0].GetComponent<Cell>().SetNeigbor(i + size, i + 1, -1, i - 1);
        }
        //top
        temp_index = (size * size) - size + 1;
        for (int i = temp_index; i < (size * size) - 1; i++)
        {
            grid[i].GetComponent<Cell>().SetNeigbor(i + size, i + 1, i - size, -1);
        }
        //left 
        temp_index = (size * size) - size;
        for (int i = temp_index; i < (size * size); i += size)
        {
            grid[i].GetComponent<Cell>().SetNeigbor(i + size, i + 1, i - size, -1);
        }
        //right
        temp_index = (size * size) - 1;
        for (int i = temp_index; i < (size * size); i += size)
        {
            grid[i].GetComponent<Cell>().SetNeigbor(i + size, -1, i - size, i - 1);
        }

        //center

        for (int i = 0; i < grid.Length; i++)
        {
            //east border =(i =size)-1


            Cell temp_cell = grid[i].GetComponent<Cell>();
            if (temp_cell.dirty) break;
            int n_index = i - size;
            int e_index = i + 1;
            int s_index = i + size;
            int w_index = i - 1;


            temp_cell.SetNeigbor(n_index, e_index, s_index, w_index);
        }
    }
}


