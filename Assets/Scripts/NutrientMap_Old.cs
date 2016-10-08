using UnityEngine;
using System.Collections.Generic;

public class NutrientMap_Old : MonoBehaviour {

    public Texture2D nutrientMap;
    private int mapDimension;
    private float[][] mapAsArray;

    // 
    public void Start(){
        if( nutrientMap == null )
            Debug.Log("You forgot your NutrientMap");
        // This doesn't work because getPixels() will only retrieve a square of pixels
        // at the position and size you tell it to
        mapAsArray = convertImageTo2DArray(nutrientMap);
    }

    // Get the nutrient level at given (x, z) coordinate
    public float getValue(Vector3 position){
        int x = (int)position.x;
        int z = (int)position.z;
        float mapValue = mapAsArray[x][z];

        return mapValue;
    }

    // Set the pixel in the mapAsArray, updating the value of the nutrient map
    public void setValue(Vector3 position, float updateValue){
        int x = (int)position.x;
        int z = (int)position.z;
        mapAsArray[x][z] += updateValue;
    }

    // Probably a terrible idea, but here we do a one-time scan through the map
    // and turn all of the pixel's grayscale values into a 2d array
    public float[][] convertImageTo2DArray(Texture2D mapImage){
        int mapWidth = mapImage.width;
        float[][] imageArray = new float[mapWidth][];

        for (int i = 0; i < mapWidth; i++){
            for (int j = 0; j < mapWidth; j++)
                imageArray[i][j] = mapImage.GetPixel(i, j).grayscale;
        }

        return imageArray;
    }

    public bool startupComplete(){
        return mapAsArray[0][0] != null;
    }
}