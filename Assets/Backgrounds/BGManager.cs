using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BGManager : MonoBehaviour {

    public GameSetup gm;

    public void setBackground(int backgroundIndex) {
        SpriteRenderer backgroundSprite = this.GetComponent<SpriteRenderer>();
        string backgroundName = "BG" + backgroundIndex;
        Debug.Log(backgroundSprite);
        backgroundSprite.sprite = Resources.Load(backgroundName, typeof(Sprite)) as Sprite;
        transform.position = new Vector3(-gm.menuBarSize/2f,0,0);
        setSpriteScale(backgroundSprite.sprite);
    }

    private void setSpriteScale(Sprite sp) {
        SpriteRenderer backgroundSprite = this.GetComponent<SpriteRenderer>();
        Vector3 pos = transform.position;
        Vector3[] array = new Vector3[2];
        //top left
        array[0] = pos + sp.bounds.min;
        // Bottom right
        array[1] = pos + sp.bounds.max;

        float width = Math.Abs(array[1].x - array[0].x);
        float height = Math.Abs(array[0].y - array[1].y);

        float gridWidth = gm.getGridDimensions().x * gm.spriteSize;
        float gridHeight = gm.getGridDimensions().y * gm.spriteSize;

        float scaleX = gridWidth / width;
        float scaleY = gridHeight / height;

        backgroundSprite.transform.localScale = new Vector2(scaleX, scaleY);

        Debug.Log("width: " + width + " height: " + height);
        Debug.Log("grid  -- width: " + gridWidth + " height: " + gridHeight);
        Debug.Log(scaleX + " " + scaleY);
    }

}