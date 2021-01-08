using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveArrow : MonoBehaviour
{
    public List<Transform> objectives;
    public List<string> objectivesText;
    public List<int> objectiveList;
    public bool[] objectiveEnabled;
    public bool[] objectiveKeepDisabled;
    public Bounds screenBounds;
    private GameObject[] Arrows;
    private Camera Cam;
    public GameObject arrowPrefab;
    public Sprite crossSprite;
    public Sprite arrowSprite;

    RectTransform PointerRectTransform;
    public bool enableArrows = true;

    void Awake()
    {
        Cam = GetComponent<Camera>();
        objectiveEnabled = new bool[objectives.Count];
        objectiveKeepDisabled = new bool[objectives.Count];
        Arrows = new GameObject[objectives.Count];

        objectiveList = new List<int>();

        for(int index = 0; index < objectives.Count; index++)
        {
            objectiveEnabled[index] = false;
            objectiveKeepDisabled[index] = false;
            Arrows[index] = Instantiate(arrowPrefab,Cam.transform);
            Arrows[index].GetComponentInChildren<TMP_Text>().text = objectivesText[index];
        }
        setObjectiveList();
    }
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.O))
        {
            enableArrows = !enableArrows;
            setObjectiveList();
        }
        for(int index = 0; index < objectiveEnabled.Length; index++)
        {
            if(objectiveEnabled[index] == true && objectiveKeepDisabled[index] == false && enableArrows == true)
            {
                Vector3 target = Cam.WorldToViewportPoint(objectives[index].position);

                if(target.x > 0 && target.x < 1 && target.y > 0 && target.y < 1 )//if target is in screen show cross
                {
                    if(crossSprite != null)
                    {
                        Vector3 posInScreen = target;
                        Arrows[index].transform.GetChild(0).up = Vector3.zero;
                        Arrows[index].transform.position = Cam.ViewportToWorldPoint(posInScreen); 
                        Arrows[index].GetComponentInChildren<SpriteRenderer>().sprite = crossSprite;
                        Arrows[index].SetActive(true);
                    }
                    else
                    {
                        Arrows[index].SetActive(false);
                    }
                }
                else // else show arrow
                {
                    Vector3 posInScreen = screenBounds.ClosestPoint( target );
                    Vector3 dir = (objectives[index].position - Cam.transform.position).normalized;
                    dir.z = 0;
                    Arrows[index].transform.GetChild(0).up = dir; //sprite is a child, and will rotate, as opposed to the text with it
                    Arrows[index].transform.position = Cam.ViewportToWorldPoint(posInScreen); 
                    Arrows[index].GetComponentInChildren<SpriteRenderer>().sprite = arrowSprite;
                    Arrows[index].SetActive(true);
                }
            }
            else
            {
                Arrows[index].SetActive(false);
            }
        }
    }
    public void EnableObjective(int index)
    {
        objectiveEnabled[index] = true;
        if(objectiveList.Contains(index) == false)
        {
            objectiveList.Add(index);
        }
        setObjectiveList();
    }

    public void DisableObjective(int index)
    {
        objectiveEnabled[index] = false;
        objectiveKeepDisabled[index] = true;
        setObjectiveList();
    }

    public bool _bIsSelected = true;
    void OnDrawGizmos()
    {
        if (_bIsSelected)
            OnDrawGizmosSelected();
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);  //center sphere
        if (GetComponent<Renderer>() != null)
        Gizmos.DrawWireCube(screenBounds.center, screenBounds.size);
    }

    public List<string> objectiveListText;
    public TMP_Text objectiveListTextObject;
    void setObjectiveList()
    {
        objectiveListTextObject.text = "Objectives:\n\n";
        foreach (int i in objectiveList)
        {
            if(objectiveListText[i] != "") // we want an objective
            {
                if(objectiveKeepDisabled[i] == true) // if it was met
                {
                    objectiveListTextObject.text += "[*] " + objectiveListText[i] + "\n";
                }
                else if(objectiveEnabled[i] == true) // if it was told, but not met
                {
                    objectiveListTextObject.text += "[  ] " + objectiveListText[i] + "\n";
                }
            }
        }
        objectiveListTextObject.text += "\n\n\n(O)bjective Arrows: " + (enableArrows==true? "on" : "off");
    }
}
