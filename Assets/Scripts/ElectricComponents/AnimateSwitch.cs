using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.VectorGraphics;

public class AnimateSwitch : MonoBehaviour
{
    OperateDialog operateDialog;
    public Switch Switch;
    public int CtlNum = 1;
    public bool SwitchConducting
    {
        get { return _SwitchConducting; }
        set 
        {
            //Debug.Log("old:" + oldSwitchConducting.ToString() + " cur:" + value.ToString());
            if(oldSwitchConducting == false && value == true)
            {
                closeEvent.Invoke();
                CtlNum++;
            }
            if(oldSwitchConducting == true && value == false)
            {
                openEvent.Invoke();
                CtlNum++;
            }
            //oldSwitchConducting = SwitchConducting;
        }
    }

    public void OperateOverrideChecks(bool value)
    {
        if(oldSwitchConducting == false && value == true)
        {
            CloseSwitch();
            CtlNum++;
        }
        if(oldSwitchConducting == true && value == false)
        {
            OpenSwitch();
            CtlNum++;
        }
        //oldSwitchConducting = SwitchConducting;
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

        if(Resources.FindObjectsOfTypeAll<OperateDialog>().Length > 0)
            operateDialog = Resources.FindObjectsOfTypeAll<OperateDialog>()[0];
        else
            Debug.Log("cannot find operateDialog");
    }

    public void OpenSwitch()
    {
        if(inTransition == false)
        {
            if(operateDialog != null)
                operateDialog.SetOperateResult(1);
            StartCoroutine("Open");
        }
        else
        {
            Debug.Log("cannot open moving switch");
            if(operateDialog != null)
                operateDialog.SetOperateResult(-3);
        }
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
        oldSwitchConducting = _SwitchConducting;
        if(operateDialog != null)
            operateDialog.SetOperateResult(0);
    }

    public void CloseSwitch()
    {
        if(inTransition == false)
        {
            if(operateDialog != null)
                operateDialog.SetOperateResult(1);
            StartCoroutine("Close");
        }
        else
        {
            Debug.Log("cannot close moving switch");
            if(operateDialog != null)
                operateDialog.SetOperateResult(-3);
        }
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
        oldSwitchConducting = _SwitchConducting;
        if(operateDialog != null)
            operateDialog.SetOperateResult(0);
    }

    public void SwitchToggle()
    {
        if(operateDialog != null)
        {
            if(inTransition == false)
            {
                operateDialog.SetOperateResult(1);
                operateDialog.ShowDialog(gameObject);
                //SwitchConducting = !SwitchConducting;
            }
            else
            {
                Debug.Log("cannot operate moving switch");
                operateDialog.SetOperateResult(-3);
            }            
        }
    }

    public void SetBadState(bool value)
    {
        if(/*inTransition == false &&*/ value == true)
        {
            StopAllCoroutines();
            spriteRenderer.sprite = bad;
            position = DbPos.bad;
            inTransition = true; // switch is blocked in bad state
            if(operateDialog != null)
                operateDialog.SetOperateResult(-2);
        }
        if(value == false)//re-enable switch
        {
            inTransition = false;
            if(operateDialog != null)
                operateDialog.SetOperateResult(2);
        }
    }
}
