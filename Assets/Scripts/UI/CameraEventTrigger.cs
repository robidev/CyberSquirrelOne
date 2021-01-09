using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEventTrigger : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //change the text
        GameObject text_online = animator.transform.parent.Find("dashboard").Find("cam2_online").gameObject;
        GameObject text_offline = animator.transform.parent.Find("dashboard").Find("cam2_offline").gameObject;
        text_online.SetActive(false);
        text_offline.SetActive(true);

        //stop the screen from being unclickable
        animator.transform.parent.GetComponent<ScreenOverlay>().DestroyOverlay();

        //set the camera image to a static animation
        animator.runtimeAnimatorController = animator.transform.parent.Find("CameraStatic").GetComponent<Animator>().runtimeAnimatorController;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
