using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Powerline : MonoBehaviour
{
    public bool _isPowered = true;
    public UnityEvent m_PowerLossEvent;
    public UnityEvent m_PowerEnableEvent;

    public Transform listener;
    public Bounds humbound;
    public Transform humSound;
    public AudioSource powerline;

    public bool isPowered 
    {
        get{ return _isPowered; }
        set
        {
            if(_isPowered == true && value == false)
            {
                m_PowerLossEvent.Invoke();
                powerline.Pause();
            }
            if(_isPowered == false && value == true)
            {
                m_PowerEnableEvent.Invoke();
                powerline.UnPause();
            }
            _isPowered = value;

        }
    }

    void Update()
    {
        humSound.position = humbound.ClosestPoint(listener.position);
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
           Gizmos.DrawWireCube(humbound.center, humbound.size);
       }
}
