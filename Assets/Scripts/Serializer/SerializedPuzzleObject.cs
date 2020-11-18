using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedPuzzleObject : SerializedObject
{
    PuzzleObjectData data;
    Puzzle1 puzzle;
    // Start is called before the first frame update
    void Start()
    {
        puzzle = GetComponent<Puzzle1>();
        data = new PuzzleObjectData{ isSolved = puzzle.IsPuzzleSolved() }; //get/set
    }

    public override object getSaveData()
    {   
        if(puzzle == null || data == null)
            Start();
            
        data.isSolved = puzzle.IsPuzzleSolved();
        return data;
    }

    public override void setLoadData(object obj)
    {
        if(puzzle == null )
            puzzle = GetComponent<Puzzle1>();
            
        data = (PuzzleObjectData) obj;
        puzzle.PuzzleSolved = data.isSolved;
        Debug.Log("data.isSolved:" + data.isSolved);
    }

    [System.Serializable]
    private class PuzzleObjectData
    {
        public bool isSolved;
    }
}
