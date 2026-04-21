using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    public int size = 4;
    public GameObject cellprefb;
    public GameObject[] grid;

    public AStar aStar;
    void Start()
    {
        grid = new GameObject[size * size];
        GenerateGrid();
        CreateNeighborhood();

        Cell[] cells = new Cell[grid.Length];
        for (int i = 0; i < grid.Length; i++)
            cells[i] = grid[i].GetComponent<Cell>();

        aStar.allCells = cells;
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
    void CreateNeighborhood()
    {
        int temp_index;

        // LD
        grid[0].GetComponent<Cell>().SetNeigbor(size, 1, -1, -1);
        // RD
        temp_index = size - 1;
        grid[temp_index].GetComponent<Cell>().SetNeigbor(temp_index + size, -1, -1, temp_index - 1);
        // LU
        temp_index = (size * size) - size;
        grid[temp_index].GetComponent<Cell>().SetNeigbor(-1, temp_index + 1, temp_index - size, -1);
        // RU
        temp_index = (size * size) - 1;
        grid[temp_index].GetComponent<Cell>().SetNeigbor(-1, -1, temp_index - size, temp_index - 1);

        // Botton
        for (int i = 1; i < size - 1; i++)
            grid[i].GetComponent<Cell>().SetNeigbor(i + size, i + 1, -1, i - 1);
        // Top
        temp_index = (size * size) - size + 1;
        for (int i = temp_index; i < (size * size) - 1; i++)
            grid[i].GetComponent<Cell>().SetNeigbor(-1, i + 1, i - size, i - 1);

        // L
        int temp_limit = (size * size) - size;
        for (int i = size; i < temp_limit; i += size)
            grid[i].GetComponent<Cell>().SetNeigbor(i + size, i + 1, i - size, -1);
        // R
        temp_limit = (size * size) - 1;
        temp_index = (2 * size) - 1;
        for (int i = temp_index; i < temp_limit; i += size)
            grid[i].GetComponent<Cell>().SetNeigbor(i + size, -1, i - size, i - 1);


        for (int i = 0; i < grid.Length; ++i)
        {
            Cell temp_cell = grid[i].GetComponent<Cell>();

            if (!temp_cell.dirty)
            {
                int n = i + size;
                int e = i + 1;
                int s = i - size;
                int w = i - 1;

                temp_cell.SetNeigbor(n, e, s, w);
            }
        }
    }
}


