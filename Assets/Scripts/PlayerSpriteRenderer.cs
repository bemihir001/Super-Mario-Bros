using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PlayerMoment moment;

    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public AnimatedSprite run;

    private void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        moment = GetComponentInParent<PlayerMoment>();
    }

    private void OnEnable(){
        spriteRenderer.enabled = true;
    }

    private void OnDisable(){
        spriteRenderer.enabled = false;
        run.enabled = false;
    }

    private void LateUpdate(){
        run.enabled = moment.running;
        if(moment.jumping){
            spriteRenderer.sprite = jump;
        }
        else if(moment.sliding){
            spriteRenderer.sprite = slide;
        }
        else if(!moment.running){
            spriteRenderer.sprite = idle;
        }
    }
}
