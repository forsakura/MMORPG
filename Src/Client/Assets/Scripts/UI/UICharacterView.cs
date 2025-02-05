using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour {
	public GameObject[] characters;
	private int currentCharacterIndex = -1;
	public int CurrentCharacterIndex
	{
		set
		{
			currentCharacterIndex = value;
			UpdateCharacterView();
		}
		get 
		{
			return currentCharacterIndex;
		}
	}
	// Use this for initialization
	void Start () {
		UpdateCharacterView ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void UpdateCharacterView()
	{
        for (int i = 0; i < characters.Length; i++)
        {
			characters[i].SetActive(i == currentCharacterIndex);
        }
    }
}
