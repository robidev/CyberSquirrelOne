
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalControl : MonoBehaviour
{
    public List<PlayerMovement> characterList;
    public List<Cinemachine.CinemachineVirtualCamera> d_camera;

    bool tabPress = false;
    int characters = 0;
    int selected = 0;

    private Camera camera_object;
    private int current_layer = 0;
    private PlayerMovement active_character = null;

    public GameObject m_GameOverUI;
    public GameObject m_GamePauseUI;

    // Start is called before the first frame update
    void Start()
    {       
        characters = characterList.Count;
        camera_object = gameObject.GetComponent<Camera>();
        //select initial character
        SelectCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && tabPress == false) {
          if(characters > selected + 1) { selected ++; }
          else { selected = 0; }

          SelectCharacter();
          tabPress = true;
        }
        
        if(Input.GetKeyUp(KeyCode.Tab) && tabPress == true) {
          tabPress = false;
        }

        if(Input.GetKeyUp(KeyCode.Escape) ) {
          GamePause();
        }

        //handle inside/outside masks
        if(active_character != null){
          //if the camera culling mask does not match the player mask (e.g. we switch to a player that is inside, while the camera is outside)
          if(active_character.gameObject.layer != current_layer){
            current_layer = active_character.gameObject.layer;

            //modify what is seen(inside/outside scene) by the camera, using culling-masks
            if(active_character.gameObject.layer == LayerMask.NameToLayer("Player_outside"))  // 10 player_outside
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
            }
          }
        }
    }

    void SelectCharacter()
    {
      int index = 0;
      foreach(PlayerMovement character in characterList) {
        if(character != null) {
          if(selected == index) { 
            character.selected = true; 
            active_character = character;
            d_camera[index].MoveToTopOfPrioritySubqueue();
          } 
          else { character.selected = false; }
        }
        index++;
      }
    }

    public void GameOver()
    {
      //showdeathscreen
      Time.timeScale = 0;
      m_GameOverUI.SetActive(true);
    }

    public void GamePause()
    {
      //showpausescreen
      Time.timeScale = 0;
      m_GamePauseUI.SetActive(true);
    }

    public void Quit()
    {
      Application.Quit();
    }

    public void Restart()
    {
      Time.timeScale = 1;
      SceneManager.LoadScene( SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void Continue()
    {
      Time.timeScale = 1;
      m_GamePauseUI.SetActive(false);
    }
}
