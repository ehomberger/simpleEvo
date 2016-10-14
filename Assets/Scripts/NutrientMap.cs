using UnityEngine;
using System.Collections.Generic;

public class NutrientMap : MonoBehaviour {

    public  Texture2D nutrientMap;
    private float[,]  mapAsArray;
    private int       mapDimension;

    // 
    public void Start(){
        if( nutrientMap == null )
            Debug.Log("You forgot your NutrientMap");

        mapDimension = nutrientMap.width;
        mapAsArray   = new float[mapDimension, mapDimension];
        convertImageTo2DArray(nutrientMap);
        Debug.Log("mapAsArray dimensions are " + mapAsArray.GetLength(0) + "x" + mapAsArray.GetLength(1) );
    }

    // Get the nutrient level at given (x, z) coordinate
    public float getValue(Vector3 position){
        int x = (int)position.x;
        int z = (int)position.z;
        float mapValue = mapAsArray[x, z];

        return mapValue;
    }

    // Set the pixel in the mapAsArray, updating the value of the nutrient map
    public void setValue(Vector3 position, float updateValue){
        int x = (int)position.x;
        int z = (int)position.z;
        mapAsArray[x, z] += updateValue;
    }

    // Probably a terrible idea, but here we do a one-time scan through the map
    // and turn all of the pixel's grayscale values into a 2d array
    public void convertImageTo2DArray(Texture2D mapImage){
        Debug.Log ("Converting image to array");

        for (int i = 0; i < mapDimension; i++)
            for (int j = 0; j < mapDimension; j++)
                mapAsArray[i, j] = mapImage.GetPixel(i, j).grayscale;

    }
}