using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveArrow : MonoBehaviour
{
    public List<Transform> objectives;
    public List<string> objectivesText;
    public bool[] objectiveEnabled;
    public bool[] objectiveKeepDisabled;
    public Bounds screenBounds;
    private GameObject[] Arrows;
    private Camera Cam;
    public GameObject arrowPrefab;
    public Sprite crossSprite;
    public Sprite arrowSprite;

    RectTransform PointerRectTransform;

    void Awake()
    {
        Cam = GetComponent<Camera>();
        objectiveEnabled = new bool[objectives.Count];
        objectiveKeepDisabled = new bool[objectives.Count];
        Arrows = new GameObject[objectives.Count];
        for(int index = 0; index < objectives.Count; index++)
        {
            objectiveEnabled[index] = false;
            objectiveKeepDisabled[index] = false;
            Arrows[index] = Instantiate(arrowPrefab,Cam.transform);
            Arrows[index].GetComponentInChildren<TMP_Text>().text = objectivesText[index];
        }
    }
    void Update()
    {
        for(int index = 0; index < objectiveEnabled.Length; index++)
        {
            if(objectiveEnabled[index] == true && objectiveKeepDisabled[index] == false)
            {
                Vector3 target = Cam.WorldToViewportPoint(objectives[index].position);

                if(target.x > 0 && target.x < 1 && target.y > 0 && target.y < 1 )//if target is in screen show cross
                {
                    Vector3 posInScreen = target;
                    Arrows[index].transform.GetChild(0).up = Vector3.zero;
                    Arrows[index].transform.position = Cam.ViewportToWorldPoint(posInScreen); 
                    Arrows[index].GetComponentInChildren<SpriteRenderer>().sprite = crossSprite;
                }
                else // else show arrow
                {
                    Vector3 posInScreen = screenBounds.ClosestPoint( target );
                    Vector3 dir = (objectives[index].position - Cam.transform.position).normalized;
                    dir.z = 0;
                    Arrows[index].transform.GetChild(0).up = dir; //sprite is a child, and will rotate, as opposed to the text with it
                    Arrows[index].transform.position = Cam.ViewportToWorldPoint(posInScreen); 
                    Arrows[index].GetComponentInChildren<SpriteRenderer>().sprite = arrowSprite;
                }
                Arrows[index].SetActive(true);
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
    }

    public void DisableObjective(int index)
    {
        objectiveEnabled[index] = false;
        objectiveKeepDisabled[index] = true;
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
}
