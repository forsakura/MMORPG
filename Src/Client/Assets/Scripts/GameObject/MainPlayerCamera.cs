﻿using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerCamera : MonoSingleton<MainPlayerCamera> {
    public Camera Camera;
    public Transform viewPoint;
    public GameObject player;
    private void LateUpdate()
    {
        if(player == null  && User.Instance.currentCharacterObject != null)
             player = User.Instance.currentCharacterObject.gameObject;
        if (player == null) return;
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;
    }
}
