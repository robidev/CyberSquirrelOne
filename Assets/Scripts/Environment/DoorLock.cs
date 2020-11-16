using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    public bool _isOpen = true;
    public bool isOpen {
        set 
        {
            if(puzzle == null || puzzle.GetComponent<Puzzle1>().IsPuzzleSolved())
            {
                _isOpen = value;
            }
            else
            {
                puzzle.SetActive(true);
            }
        }
        get 
        {
            if(puzzle == null || puzzle.GetComponent<Puzzle1>().IsPuzzleSolved())
            {
                return _isOpen;
            }
            else
            {
                return false;
            }
        }
    }
    public GameObject puzzle;
    // Start is called before the first frame update
    public void UnlockDoor()
    {
        _isOpen = true;
    }
}
