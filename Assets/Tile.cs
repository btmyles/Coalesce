using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public VirtualDPad dpad;
    private string prevdir;

    public Circle[] circles;
    private float prevtime;

    private int step;
    List<HashSet<Circle>> sets;

    private static ScoreKeeper scoreKeeper;

    // Start is called before the first frame update
    void Start()
    {
        scoreKeeper = GameObject.Find("ScoreKeeper").GetComponent<ScoreKeeper>();
        dpad = Instantiate(dpad, new Vector3(0,0,0), Quaternion.identity);
        circles[0] = Instantiate(circles[0], transform.position - new Vector3( 0.5f,0,0), Quaternion.identity, transform);
        circles[1] = Instantiate(circles[1], transform.position - new Vector3(-0.5f,0,0), Quaternion.identity, transform);
        step = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (step == 0) // Accept user input
        {
            if (dpad.action != prevdir)
            {
                if (dpad.action == "R")
                {
                    transform.position += new Vector3(1,0,0);
                    if (!ValidMove())
                        transform.position -= new Vector3(1,0,0);

                }
                else if (dpad.action == "L")
                {
                    transform.position += new Vector3(-1,0,0);
                    if (!ValidMove())
                        transform.position -= new Vector3(-1,0,0);
                }
                else if (dpad.action == "U")
                {
                    // Rotate
                    // transform.RotateAround(circles[0].transform.position, new Vector3(0,0,1), 90);
                    // if (!ValidMove())
                    //     transform.RotateAround(circles[0].transform.position, new Vector3(0,0,1), -90);
                    // else
                    //     foreach (Circle circle in circles)
                    //     {
                    //         circle.transform.Rotate(0,0,-90,Space.Self);
                    //     }
                    //
                    // if (circles[1].Ypos() < Data.height-2)
                    //     transform.position += new Vector3(0,1,0);
                    // if (circles[0].Ypos() == Data.height-1 && circles[1].Ypos() == Data.height-1)
                    //     transform.position += new Vector3(0,-1,0);

                    if (circles[0].Xpos() == circles[1].Xpos())
                    {
                        // Circles are stacked. circles[0] is always on top.
                        transform.RotateAround(circles[1].transform.position, new Vector3(0,0,1), -90);
                        transform.position = transform.position + new Vector3(-1,0,0);
                        if (!ValidMove())
                        {
                            transform.position = transform.position - new Vector3(-1,0,0);
                        }
                        foreach (Circle circle in circles)
                        {
                            circle.transform.Rotate(0,0,90,Space.Self);
                        }
                    }
                    else if (circles[0].Ypos() == circles[1].Ypos())
                    {
                        // Circles are side-by-side. circles[0] is always on left.
                        if (circles[0].Xpos() == 0)
                        {
                            transform.position = transform.position + new Vector3(-1,0,0);
                        }
                        transform.RotateAround(circles[1].transform.position, new Vector3(0,0,1), -90);
                        foreach (Circle circle in circles)
                        {
                            circle.transform.Rotate(0,0,90,Space.Self);
                        }
                    }

                    if (circles[1].Xpos() < circles[0].Xpos())
                    {
                        // Swap order so that the left circle is our first one
                        Circle tmp = circles[0];
                        circles[0] = circles[1];
                        circles[1] = tmp;
                    }


                }
                else if (dpad.action == "D")
                {
                        transform.DetachChildren();
                        step = 1;
                }

                prevdir = dpad.action;
            }
        }
        else if (step == 1) // Drop the tile
        {
            if (Time.time - prevtime > Data.fallspeed)
            {
                Debug.Log("Step 1");
                if (circles[1].transform.position.y < circles[0].transform.position.y)
                {
                    // Swap order so that the lower circle is evaluated first.
                    Circle tmp = circles[0];
                    circles[0] = circles[1];
                    circles[1] = tmp;
                }

                if (!circles[0].MoveDown() & !circles[1].MoveDown())
                {
                    // Both circles have reached their final point
                    step = 2;
                }

                prevtime = Time.time;
            }
        }
        // Optimize: Add a step that only checks the new circles.
        else if (step == 2) // Check for ANY sets and clear them. Skip to step 5 if none to clear.
        {
            Debug.Log("Step 2");
            sets = CheckSets();
            if (sets.Count > 0)
                step = 3;
            else
                step = 6;
        }
        else if (step == 3)
        {
            Debug.Log("Step 3");
            foreach (HashSet<Circle> set in sets)
                CombineSet(set);
            step = 4;
        }
        else if (step == 4) // Drop floating circles.
        {
            Debug.Log("Step 4");
            DropCircles();
            step = 2;
        }
        else if (step == 6) // Spawn new tile.
        {
            Debug.Log("Step 6");
            Destroy(dpad);
            this.enabled = false;
            scoreKeeper.SaveGame();

            foreach (Circle c in circles)
                if (c != null && c.Ypos() >= Data.height-2)
                {
                    scoreKeeper.LoseGame();
                    return;
                }

            FindObjectOfType<SpawnTile>().NewTile();
        }
    }

    bool ValidMove()
    {
        foreach (Circle child in circles)
            if (!child.ValidMove())
                return false;

        return true;
    }

    List<HashSet<Circle>> CheckSets(Circle[] circles)
    {
        // DEBUG variable
        Circle[] arr;

        bool alreadyDone;
        List<HashSet<Circle>> sets = new List<HashSet<Circle>>();

        Debug.Log("Checking objects: " + Circle.ArrayToString(circles));

        foreach (Circle circle in circles)
        {
            // if circle is not already in a set
            alreadyDone = false;
            foreach (HashSet<Circle> testset in sets)
            {
                arr = new Circle[testset.Count];
                testset.CopyTo(arr);
                Debug.Log(circle + "in set?: " + Circle.ArrayToString(arr));
                if (testset.Contains(circle) || circle == null)
                {
                    Debug.Log("Yes. Continuing.");
                    alreadyDone = true;
                }
            }
            if (alreadyDone)
                continue;


            // Build a set of connected circles of the same color
            HashSet<Circle> set = new HashSet<Circle>() {circle};
            Queue<Circle> q = new Queue<Circle>();
            q.Enqueue(circle);
            Circle next;

            while (q.Count > 0)
            {
                next = q.Dequeue();
                Debug.Log("Getting Neighbors of " + next);
                foreach (Circle neighbor in next.GetMatchingNeighbors())
                {
                    if (!set.Contains(neighbor))
                    {
                        q.Enqueue(neighbor);
                        set.Add(neighbor);
                    }
                }
            }

            // Debug.Log("Elements in set " + circle);
            // foreach (Circle member in set)
            // {
            //     Debug.Log(member);
            // }

            if (set.Count >= Data.coalesceCount)
            {
                Debug.Log("Set to be combined: "+ set);
                sets.Add(set);
            }
        }

        return sets;
    }

    List<HashSet<Circle>> CheckSets()
    {
        Debug.Log("Checking all circles: "+Circle.ArrayToString(Circle.allCircles.ToArray()));
        return CheckSets(Circle.allCircles.ToArray());
    }

    // Possible issue: What if 2 identical hash sets are passed in one after another?
    void CombineSet(HashSet<Circle> set)
    {
        // Find circle with the lowest y coord, using x coord to break ties
        Circle low = null;
        int xlow = Data.width+1;
        int ylow = Data.height+1;
        foreach (Circle circle in set)
        {
            // if (circle == null)
            // {
            //     set.Remove(circle);
            //     continue;
            // }
            if (circle.Ypos() < ylow || (circle.Ypos() == ylow && circle.Xpos() < xlow))
            {
                xlow = circle.Xpos();
                ylow = circle.Ypos();
                low = circle;
            }
        }

        if (low != null)
        {

            Data.score += (set.Count * Data.scores[low.level]);

            // Remove others from grid and delete the circle objects
            set.Remove(low);
            foreach (Circle circle in set)
            {
                circle.Kill();
            }

            // Level up the low circle
            low.LevelUp();
        }
    }

    void DropCircles()
    {
        Circle circle;
        for (int i=0; i<Circle.grid.GetLength(0); i++) // Skip first row because it's already on the bottom
        {
            for (int j=1; j<Circle.grid.GetLength(1); j++)
            {
                circle = Circle.grid[i,j];
                if (circle != null)
                {
                    circle.enabled = true;
                    Debug.Log("Dropping "+circle);
                    while(circle.MoveDown());
                    circle.enabled = false;
                }
            }
        }
    }
}
