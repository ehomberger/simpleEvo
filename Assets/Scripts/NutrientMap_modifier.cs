using UnityEngine;
using System.Collections.Generic;

public class NutrientMap_modifier : MonoBehaviour {

    public  Texture2D nutrientMap;
    public  float[,]  modifiedMap;
    private int       mapWidth;

    // 
    public void Start(){
        if( nutrientMap == null )
            Debug.Log("You forgot your NutrientMap");
        
        mapWidth    = nutrientMap.width;
        modifiedMap = new float[mapWidth, mapWidth];
        Debug.Log("modifiedMap[10][10] = " + modifiedMap[10, 10]);
    }

    // Get the nutrient level at given (x, z) coordinate
    public float getValue(Vector3 position){
        int   x = (int)position.x;
        int   z = (int)position.z;
        float mapValue = nutrientMap.GetPixel(x, z).grayscale;

        return mapValue;
    }

    public float getModifier(Vector3 position){
        int   x = (int)position.x;
        int   z = (int)position.z;
        float mapValue = modifiedMap[x, z];

        return mapValue;
    }

    // Set the pixel in the mapAsArray, updating the value of the nutrient map
    public void setValue(Vector3 position, float updateValue){
        int x = (int)position.x;
        int z = (int)position.z;
        modifiedMap[x, z] += updateValue;
    }
}