using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.VectorGraphics;

public class AnimateSwitch : MonoBehaviour
{
    public Switch Switch;
    public bool SwitchConducting
    {
        get { return _SwitchConducting; }
        set {
             if(oldSwitchConducting == false && value == true)
             {
                closeEvent.Invoke();
             }
             if(oldSwitchConducting == true && value == false)
             {
                openEvent.Invoke();
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
    private SVGImage spriteRenderer;
    public float transitionTime = 1f;
    private bool inTransition = false;
    public UnityEvent openEvent;
    public UnityEvent closeEvent;
    void Start()
    {
        spriteRenderer = GetComponent<SVGImage>();
        if(Switch != null)
        {
            _SwitchConducting = Switch.SwitchConducting;//true;
            if(_SwitchConducting == true)
            {
                spriteRenderer.sprite = close;
                position = DbPos.close;
            }
            else
            {
                spriteRenderer.sprite = open;
                position = DbPos.open;
            }
        }
        else
        {
            _SwitchConducting = false;
            spriteRenderer.sprite = bad;
            position = DbPos.bad;
        }
        oldSwitchConducting = _SwitchConducting;

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
        _SwitchConducting = false;
        Switch.SwitchConducting = _SwitchConducting;
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
        _SwitchConducting = true;
        Switch.SwitchConducting = _SwitchConducting;
        inTransition = false;
    }

    public void SwitchToggle()
    {
        if(inTransition == false)
            SwitchConducting = !SwitchConducting;
    }

    public void SetBadState(bool value)
    {
        if(/*inTransition == false &&*/ value == true)
        {
            StopAllCoroutines();
            spriteRenderer.sprite = bad;
            position = DbPos.bad;
            inTransition = true; // switch is blocked in bad state
        }
        if(value == false)
        {
            inTransition = false;
        }
    }
}
