using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public int level;
    private SpriteRenderer sprite;

    private float prevtime;

    public static Circle[,] grid = new Circle[Data.width, Data.height];
    public static List<Circle> allCircles = new List<Circle>();

    private Legend legend = null;

    // Start is called before the first frame update
    void Awake()
    {
        level = UnityEngine.Random.Range(1, Data.maxLevel+1);
        // Debug.Log("Level: " + level);
        sprite = GetComponent<SpriteRenderer>();
        UpdateColor();
    }

    public bool MoveDown()
    {
        RemoveFromGrid();
        transform.position += new Vector3(0,-1,0);
        if (!ValidMove())
        {
            transform.position -= new Vector3(0,-1,0);
            AddToGrid();
            // Debug.Log("Reached "+ transform.position);
            this.enabled = false;
            return false;
        }

        return true;
    }

    public void AddToGrid()
    {
        int rx = Xpos();
        int ry = Ypos();

        if (grid[rx,ry] == null)
        {
            grid[rx, ry] = this;
            allCircles.Add(this);
            PrintGrid();
        }

        //Makes sure this runs on load game
        if (legend == null)
            legend = (Legend) GameObject.FindObjectOfType(typeof(Legend));
        legend.UpdateLegend();
    }

    void RemoveFromGrid()
    {
        int rx = Xpos();
        int ry = Ypos();

        if (grid[rx,ry] == this)
        {
            grid[rx, ry] = null;
            allCircles.Remove(this);
            PrintGrid();
        }
    }

    void PrintGrid()
    {
        string str = "Grid: \n";
        for (int j=0; j<grid.GetLength(1); j++)
        {
            for (int i=grid.GetLength(0)-1; i>=0; i--)
            {
                if (grid[i,j] != null)
                    str += grid[i,j].level + " ";
                else
                    str += "0 ";
            }
            str += "\n";
        }
        Debug.Log(str);
    }

    public bool ValidMove()
    {
        int rx = Xpos();
        int ry = Ypos();

        Debug.Log("rx: "+rx+", ry: "+ry);

        if (rx < 0 || rx >= Data.width || ry < 0 || ry > Data.height)
            return false;

        if (grid[rx,ry] != null)
            return false;

        // Debug.Log(this);
        return true;
    }

    public List<Circle> GetMatchingNeighbors()
    {
        int rx = Xpos();
        int ry = Ypos();
        List<Circle> matchingNeighbors = new List<Circle>();
        Circle neigh;

        int[,] tests = new int[,] {{0,1}, {1,0}, {0,-1}, {-1,0}};

        for (int i=0; i<tests.GetLength(0); i++)
        {
            if (rx+tests[i,0] >= 0 && rx+tests[i,0] < Data.width
            && ry+tests[i,1] >= 0 && ry+tests[i,1] < Data.height)
            {
                neigh = grid[rx+tests[i,0], ry+tests[i,1]];
                if (neigh != null && neigh.level == level)
                    matchingNeighbors.Add(neigh);
            }
        }

        return matchingNeighbors;
    }

    public void Kill()
    {
        Debug.Log("Killing "+this);
        RemoveFromGrid();
        Debug.Log("New allCircles: "+ArrayToString(allCircles.ToArray()));
        this.enabled = false;
        Destroy(gameObject);
    }

    public void LevelUp()
    {
        if (level > Data.maxLevel)
        {
            Debug.Log("MaxLevel increased");
            Data.maxLevel = level;
            if (legend == null)
                legend = (Legend) GameObject.FindObjectOfType(typeof(Legend));
            legend.UpdateLegend();
        }

        level += 1;
        UpdateColor();
    }

    public void SetLevel(int lev)
    {
        level = lev;
        Debug.Log("Setting level for " + this);
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (Data.colors.ContainsKey(level))
        {
            sprite.color = Data.colors[level];
        }
        else
            Kill();
    }

    public void MuteColor(bool gray)
    {
        if (gray)
        {
            sprite.color = Data.gray;
        }
        else
        {
            sprite.color = Data.colors[level];
        }
    }

    public int Xpos()
    {
        return Mathf.RoundToInt(transform.position.x)-1;
    }

    public int Ypos()
    {
        // Round down the 0.5 on the y values
        return (int)(Mathf.Floor(transform.position.y));
    }

    public override string ToString()
    {
        return "C_X"+Xpos()+"_Y" +Ypos()+ "_L" + level;
    }

    public static string ArrayToString(Circle[] circles)
    {
        string str = "";
        foreach (Circle circle in circles)
        {
            str = str + ", " + circle;
        }
        return str;
    }
}
