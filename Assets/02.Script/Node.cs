using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Line[] linkLine;
    public DoorCtrl doorCtrl;
    public Computer computer;
    public bool isNodeActivate = false;

    void Start()
    {
        
    }


    public void nodeActivate()
    {
        for (int i = 0; i < linkLine.Length; i++)
        {
            linkLine[i].activateLines();
        }
    }

    public void nodeDeactivate()
    {
        for (int i = 0; i < linkLine.Length; i++)
        {
            linkLine[i].deactivateLines();
        }
    }

    public void activateLever()
    {
        Animator Lever;
        Lever = this.GetComponent<Animator>();
        if (Lever != null)
        {
            if (Lever.GetBool("LeverDown") == false)
            {
                Lever.SetBool("LeverDown", true);
            }
            else
            {
                Lever.SetBool("LeverDown", false);
            }
        }
    }
    void OnMouseDown()
    {
        bool ispreviousdoor = (doorCtrl != null && !doorCtrl.isNext) || doorCtrl == null;
        bool isuncorrectComputer = (computer != null && !computer.iscorrect) || computer == null;
        if (ispreviousdoor && isuncorrectComputer && UIManager.isOpeningOver && !Cursor.visible)
        {
            activateLever();
            if (isNodeActivate == false)
            {
                nodeActivate();
                isNodeActivate = true;
            }
            else if (isNodeActivate == true)
            {
                nodeDeactivate();
                isNodeActivate = false;
            }
        }
    }
}
