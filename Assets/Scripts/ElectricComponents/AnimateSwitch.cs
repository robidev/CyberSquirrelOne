using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.VectorGraphics;

public class AnimateSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    public bool SwitchConducting
    {
        get { return _SwitchConducting; }
        set {
             _SwitchConducting = value;
             if(oldSwitchConducting == false && _SwitchConducting == true)
             {
                CloseSwitch();
             }
             if(oldSwitchConducting == true && _SwitchConducting == false)
             {
                OpenSwitch();
             }
             oldSwitchConducting = SwitchConducting;
        }
    }
    private bool oldSwitchConducting;
    private bool _SwitchConducting;
    public enum DbPos {
        intermediate,
        open, //off
        close, //on
        bad
    }
    public DbPos position;
    public Sprite open;
    public Sprite intermediate;
    public Sprite close;
    public Sprite bad;
    //private SpriteRenderer spriteRenderer;
    private SVGImage spriteRenderer;
    public float transitionTime = 1f;
    private bool inTransition = false;
    public UnityEvent openEvent;
    public UnityEvent closeEvent;
    //public UnityEvent badEvent;
    void Start()
    {
        _SwitchConducting = true;
        spriteRenderer = GetComponent<SVGImage>();
        spriteRenderer.sprite = close;
        oldSwitchConducting = _SwitchConducting;
        position = DbPos.close;
    }

    public void OpenSwitch()
    {
        if(inTransition == false)
            StartCoroutine("Open");
    }

    IEnumerator Open()
    {
        inTransition = true;
        spriteRenderer.sprite = intermediate;
        position = DbPos.intermediate;
        SwitchConducting = false;
        openEvent.Invoke();
        yield return new WaitForSecondsRealtime(transitionTime);
        spriteRenderer.sprite = open;
        position = DbPos.open;
        inTransition = false;
    }

    public void CloseSwitch()
    {
        if(inTransition == false)
            StartCoroutine("Close");
    }

    IEnumerator Close()
    {
        inTransition = true;
        spriteRenderer.sprite = intermediate;
        position = DbPos.intermediate;
        yield return new WaitForSecondsRealtime(transitionTime);
        spriteRenderer.sprite = close;
        position = DbPos.close;
        SwitchConducting = true;
        closeEvent.Invoke();
        inTransition = false;
    }

    public void SwitchToggle()
    {
        if(inTransition == false)
            SwitchConducting = !SwitchConducting;
    }

    public void SetBadState(bool value)
    {
        if(inTransition == false && value == true)
        {
            spriteRenderer.sprite = bad;
            position = DbPos.bad;
            //badEvent.Invoke();
            inTransition = true; // switch is blocked in bad state
        }
        if(value == false)
        {
            inTransition = false;
        }
    }
}
