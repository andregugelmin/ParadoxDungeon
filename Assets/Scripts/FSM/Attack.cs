using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : NPCBaseFSM
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        NPC.GetComponent<Animator>().SetBool("isAttacking", false);
        NPC.GetComponent<EnemyAnimation>().StartCoroutine("AttackPlayer");        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Vector3 direction = (NPC.GetComponent<EnemyAnimation>().player.transform.position - NPC.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        NPC.transform.rotation = Quaternion.Slerp(NPC.transform.rotation, lookRotation, Time.deltaTime * 3f);
    }
        

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NPC.GetComponent<EnemyAnimation>().StopCoroutine("AttackPlayer");
    }
    
}
