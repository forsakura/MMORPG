using Assets.Scripts.Models;
using Common.Data;
using GameServer.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// NPCController的功能包括点击交互以及高亮显示
/// </summary>
public class NpcController : MonoBehaviour {
    public int npcID;
    NpcDefine npcDefine;
    public bool inInteractive;
    Animator animator;
    SkinnedMeshRenderer skinnedMeshRenderer;
    Color originColor;
    private void Start()
    {
        npcDefine = NpcManager.Instance.GetNPCDefine(npcID);
        animator = GetComponent<Animator>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor =skinnedMeshRenderer.sharedMaterial.color;
        StartCoroutine(Actions());
    }

    IEnumerator Actions()
    {
        while(true)
        {
            if (inInteractive) yield return new WaitForSeconds(2f);
            else yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 5f));
            Relax(); 
        }
    }

    private void Relax()
    {
        animator.SetTrigger("Relax");
    }

    private void OnMouseDown()
    {
        Interactive();
    }

    private void Interactive()
    {
        if(!inInteractive)
        {
            inInteractive = true;
            StartCoroutine(DoInteractive());
        }
    }

    IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if(NpcManager.Instance.Interactive(npcID))
        {
            animator.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }

    IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (User.Instance.currentCharacterObject.transform.position - transform.position).normalized;
        while(Mathf.Abs(Vector3.Angle(transform.forward, faceTo)) > 5f)
        {
            transform.forward =  Vector3.Lerp(transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }

    private void OnMouseEnter()
    { 
        HighLight(true);
    }

    private void OnMouseExit()
    {
        HighLight(false);
    }

    private void OnMouseOver()
    {
        HighLight(true);
    }

    private void HighLight(bool v)
    {
        if(v)
        {
            if(skinnedMeshRenderer.sharedMaterial.color !=  Color.white) skinnedMeshRenderer.sharedMaterial.color = Color.white;
        }
        else 
            if(skinnedMeshRenderer.sharedMaterial.color != originColor) skinnedMeshRenderer.sharedMaterial.color= originColor;
    }
}
