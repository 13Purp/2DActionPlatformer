using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactables;

public class Lever : MonoBehaviour,IInteractable
{
    [SerializeField] SpriteManager sm;


    public void Interact()
    {
        Debug.Log("Changing Sprite");
        sm.changeSprite();
    }

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
