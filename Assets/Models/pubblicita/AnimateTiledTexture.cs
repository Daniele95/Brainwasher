using UnityEngine;
using System.Collections;
 
class AnimateTiledTexture : MonoBehaviour
{
    public int columns;
    public int rows;
    public float framesPerSecond;
	
	public string myTex;
 
    //the current frame to display
    private int index = 0;
 
    void Start()
    {
        StartCoroutine(updateTiling());
 
        //set the tile size of the texture (in UV units), based on the rows and columns
        Vector2 size = new Vector2(1f / columns, 1f / rows);
        gameObject.GetComponent<Renderer>().material.SetTextureScale(myTex, size);
    }
 
    private IEnumerator updateTiling()
    {
        while (true)
        {
            //move to the next index
            index++;
            if (index >= rows * columns)
                index = 0;
 
            //split into x and y indexes
            Vector2 offset = new Vector2((float)index / columns - (index / columns), //x index
                                          (index / columns) / (float)rows);          //y index
 
            gameObject.GetComponent<Renderer>().material.SetTextureOffset(myTex, offset);
			
            yield return new WaitForSeconds(1f / framesPerSecond);
        }
 
    }
}