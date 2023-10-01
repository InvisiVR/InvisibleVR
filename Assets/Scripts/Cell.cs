using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int index;

    public bool isFrontWall = true;
    public bool isBackWall = true;
    public bool isLeftWall = true;
    public bool isRightWall = true;
    public bool isFloor = true;
    public bool isCeiling = true;

    public GameObject frontWall;
    public GameObject backWall;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject floor;
    public GameObject ceiling;
    // Start is called before the first frame update
    void Start()
    {
        ShowWalls();
    }

    public void ShowWalls()
    {
        frontWall.SetActive(isFrontWall);
        backWall.SetActive(isBackWall);
        leftWall.SetActive(isLeftWall);
        rightWall.SetActive(isRightWall);
        floor.SetActive(isFloor);
        ceiling.SetActive(isCeiling);
    }
    public bool CheckAllWall()
    {
        return isFrontWall && isBackWall && isLeftWall && isRightWall && isFloor && isCeiling;
    }
}
