using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NpcController : MonoBehaviour {
	public int ID;
	public NpcDefine npcDefine;
    Animator animator;
    SkinnedMeshRenderer render;
    Color originColor;
    bool inInteracive;
	// Use this for initialization
	void Start () {
		npcDefine = NpcManager.Instance.GetDefine(ID);
        animator = GetComponent<Animator>();
        render = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = render.sharedMaterial.color;
        StartCoroutine(Actions());
	}

    IEnumerator  Actions()
    {
        while (true) {
            if (inInteracive) yield return new WaitForSeconds(2f);
            else yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
            this.Relax();
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
        if(!inInteracive)
        {
            inInteracive = true;
            StartCoroutine(DoInteractive());
        }
    }
    IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if (NpcManager.Instance.Interactive(npcDefine))
        {
            animator.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteracive = false;
    }

    private IEnumerator FaceToPlayer()
    {
        Vector3 faceto = (User.Instance.currentCharacterObject.transform.position-transform.position).normalized;
        while (Mathf.Abs(Vector3.Angle(gameObject.transform.forward, faceto)) > 5)
        {
            transform.forward = Vector3.Lerp(transform.forward, faceto, Time.deltaTime * 5f);
            yield return null;
        }
    }

    private void OnMouseExit()
    {
        HighLight(false);
    }

    private void OnMouseEnter()
    {
        HighLight(true);
    }
    private void OnMouseOver()
    {
        HighLight(true);
    }
    private void HighLight(bool v)
    {
        if(v)
        {
            if(render.sharedMaterial.color != Color.white) render.sharedMaterial.color = Color.white;
        }
        else
        {
            if(render.sharedMaterial.color != originColor) render.sharedMaterial.color = originColor;
        }
    }
}
