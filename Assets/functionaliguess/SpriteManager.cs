using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager: MonoBehaviour
{

     [SerializeField] private SpriteRenderer _spriteRenderer;
     [SerializeField] private Sprite _sprite1;
     [SerializeField] private Sprite _sprite2;
    private bool _active=false;


    public void changeSprite()
    {
        if (_active)
            _spriteRenderer.sprite = _sprite2;
        else
            _spriteRenderer.sprite = _sprite1;
        _active=!_active;
    }
}
