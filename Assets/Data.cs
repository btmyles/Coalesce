using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    // These variables change throughout the game
    public static int score = 0;
    public static int maxLevel = 2;

    // These variables are set and don't change over the course of a game.
    public static int width = 7;
    public static int height = 9;
    public static float fallspeed = 0.07f;
    public static int coalesceCount = 3;
    public static Color gray = new Color(0.6886f, 0.6886f, 0.6886f, 1);

    public static Dictionary<int, Color> colors = new Dictionary<int, Color>(){
                                                                {1, new Color(0.2705f, 0.9137f, 0.2588f, 1)},
                                                                {2, new Color(1,       0.7843f, 0.2156f, 1)},
                                                                {3, new Color(0.8901f, 0.4588f, 0.1607f, 1)},
                                                                {4, new Color(0.8980f, 0.0274f, 0.1647f, 1)},
                                                                {5, new Color(0.9725f, 0.2549f, 0.5803f, 1)},
                                                                {6, new Color(0.7098f, 0.2196f, 0.9999f, 1)},
                                                                {7, new Color(0,       0.3882f, 0.9725f, 1)},
                                                                {8, new Color(0,       0.7921f, 0.9176f, 1)},
                                                                {9, new Color(0,       0,       0,       1)},
                                                                {10,new Color(1,       1,       1,       1)}
                                                            };

    public static Dictionary<int, int> scores = new Dictionary<int, int>(){
                                                                {1, 5},
                                                                {2, 10},
                                                                {3, 15},
                                                                {4, 35},
                                                                {5, 295},
                                                                {6, 305},
                                                                {7, 315},
                                                                {8, 325},
                                                                {9, 335},
                                                                {10, 345}
                                                            };
}
