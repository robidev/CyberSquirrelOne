
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GlobalPlayerControl : MonoBehaviour
{
    public List<PlayerMovement> characterList;
    public List<bool> characterEnabledList;
    public List<Cinemachine.CinemachineVirtualCamera> d_camera;

    bool tabPress = false;
    int characters = 0;
    int oldSelected = -1;
    public int selected = 0;

    private Camera camera_object;
    private int current_layer = 0;
    private PlayerMovement active_character = null;


    public AudioSource audioSource;
    public AudioClip DayAudio;
    public AudioClip NightAudio;
    public AudioClip InsideAudio;
    private AudioClip OutsideAudio;

    public Transform listener;
    public TextMeshProUGUI selectedText;
    public bool GlobalControlsEnabled = true;

    // Start is called before the first frame update
    void Start()
    {       
        characters = characterList.Count;
        camera_object = gameObject.GetComponent<Camera>();
        audioSource = GetComponent<AudioSource>();
        OutsideAudio = DayAudio;
        audioSource.Play();
        //select initial character
        SelectCharacter();
    }

    // Update is called once per frame
    void Update()
    { 
      if( Time.timeScale > 0.1f)
      {
        if(Input.GetButtonDown ("Tab") && tabPress == false && GlobalControlsEnabled) {
          if(characters > selected + 1) { selected ++; }
          else { selected = 0; }

          SelectCharacter();
          tabPress = true;
        }
        
        if(Input.GetButtonDown ("Tab") && tabPress == true) {
          tabPress = false;
        }

        //handle inside/outside masks
        if(active_character != null){
          //if the camera culling mask does not match the player mask (e.g. we switch to a player that is inside, while the camera is outside)
          if(active_character.GetDisplayedLayer() != current_layer){
            current_layer = active_character.GetDisplayedLayer();

            //modify what is seen(inside/outside scene) by the camera, using culling-masks
            if(current_layer == LayerMask.NameToLayer("Player_outside"))  // 10 player_outside
            {
              camera_object.cullingMask |= 1 << LayerMask.NameToLayer("Player_outside");//1 << 10;
              camera_object.cullingMask &=  ~(1 << LayerMask.NameToLayer("Player_inside"));//~(1 << 11);
              camera_object.cullingMask |= 1 << LayerMask.NameToLayer("cables");//1 << 14;
              camera_object.cullingMask |= 1 << LayerMask.NameToLayer("outside");//1 << 15;
              camera_object.cullingMask &=  ~(1 << LayerMask.NameToLayer("inside"));//~(1 << 16);
              camera_object.cullingMask |= 1 << LayerMask.NameToLayer("camera");//1 << 18;
              camera_object.cullingMask |= 1 << LayerMask.NameToLayer("object_outside");//1 << 19;
              camera_object.cullingMask &= ~(1 << LayerMask.NameToLayer("object_inside"));//~(1 << 20);
              camera_object.cullingMask |= 1 << LayerMask.NameToLayer("enemy_outside");//1 << 21;
              camera_object.cullingMask &= ~(1 << LayerMask.NameToLayer("enemy_inside"));//~(1 << 22);
              audioSource.clip = OutsideAudio;
              audioSource.Play();
            }else {                                      // player_inside
              camera_object.cullingMask &= ~(1 << LayerMask.NameToLayer("Player_outside"));//~(1 << 10);
              camera_object.cullingMask |= 1 << LayerMask.NameToLayer("Player_inside");//1 << 11;
              camera_object.cullingMask &= ~(1 << LayerMask.NameToLayer("cables"));//~(1 << 14);
              camera_object.cullingMask &= ~(1 << LayerMask.NameToLayer("outside"));//~(1 << 15);
              camera_object.cullingMask |= 1 << LayerMask.NameToLayer("inside");//1 << 16;
              camera_object.cullingMask &= ~(1 << LayerMask.NameToLayer("camera"));//~(1 << 18);
              camera_object.cullingMask &= ~(1 << LayerMask.NameToLayer("object_outside"));//~(1 << 19);
              camera_object.cullingMask |= 1 << LayerMask.NameToLayer("object_inside");//1 << 20;
              camera_object.cullingMask &= ~(1 << LayerMask.NameToLayer("enemy_outside"));//~(1 << 21);
              camera_object.cullingMask |= 1 << LayerMask.NameToLayer("enemy_inside");//1 << 22;
              audioSource.clip = InsideAudio;
              audioSource.Play();
            }
          }
        }
      }
    }

    void SelectCharacter()
    {
      if(GlobalControlsEnabled == false)
      {
        return;
      }
      if(selected == oldSelected) // if we are selecting the same character
      {
        //Debug.Log("1 " + selected);
        characterList[selected].selected = true; 
        return;
      }

      int index = 0;
      foreach(PlayerMovement character in characterList) {
        if(character != null) {
          if(selected == index) { 
            if(characterEnabledList[index] != true) { //this clause is for when a character is disabled, to try the next available one
              if(characters > selected + 1) { selected ++; }
              else { selected = 0; }
              SelectCharacter();//retry select
              return;
            }
            character.selected = true; 
            active_character = character;
            d_camera[index].MoveToTopOfPrioritySubqueue();
            listener.parent = d_camera[index].Follow;
            listener.localPosition = Vector3.zero;
            oldSelected = selected;

            DroneControl ctl = active_character.GetComponent<DroneControl>();
            DisplaySelected(active_character.name);
          } 
          else { 
            character.selected = false; 
          }
        }
        index++;
      }
    }

    public void SelectCharacter(int index)
    {
      if(characterEnabledList[index] == true)
      {
        selected = index;
        SelectCharacter();
      }
      else
      {
        Debug.Log("cannot select disabled character:" + index);
      }
    }

    public void EnableCharacter(int index)
    {
      if(index < characterEnabledList.Count)
        characterEnabledList[index] = true;
    }

    public void DisableCharacter(int index)
    {
      if(index < characterEnabledList.Count)
        characterEnabledList[index] = false;
    }

    public void DisableAllCharacters()
    {
      GlobalControlsEnabled = false;
      for(int index = 0; index < characterEnabledList.Count; index++)
      {
        //characterEnabledList[index] = false;
        characterList[index].selected = false;
      }
    }

    public void DisplaySelected(string name)
    {
      selectedText.text = "" + name.Replace("_"," ");
    }
}
