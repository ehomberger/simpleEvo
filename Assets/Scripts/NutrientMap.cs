using UnityEngine;
using System.Collections.Generic;

// !!
// This script is set to run first in script execution order. I'm like 90% it 
// needs to in order to set up convertImageTo2DArray()
public class NutrientMap : MonoBehaviour {

    public  Texture2D nutrientMapTex; 
    private float[,]  nutrientMapArray; // Name other arrays and textures like this
    private int       mapDimension;
    private Terrain   t;
    private int       nextUpdate;
    private float     timer;

    // Initialization 
    public void Start(){
        if( nutrientMapTex == null )
            Debug.Log("You forgot your nutrientMapTexture");

        timer = 0; 
        nextUpdate = 1;
        t = Terrain.activeTerrain;
        mapDimension = 513;
        nutrientMapArray   = new float[mapDimension, mapDimension];
        convertImageTo2DArray(nutrientMapTex);
        Debug.Log("nutrientMapArray dimensions are " + nutrientMapArray.GetLength(0) + "x" + nutrientMapArray.GetLength(1) );
    }

    public void Update(){
        timer += Time.deltaTime;

        if(timer > nextUpdate){
            updateMapAlphas();
            nextUpdate = 5;
            timer = 0;
        }
    }
    // Get the NutrientMapArray
    public float[,] getNutrientMapArray(){
        return nutrientMapArray;
    }

    // Get the nutrient level at given (x, z) coordinate
    public float getValue(Vector3 position){
        int x = (int)position.x;
        int z = (int)position.z;
        float mapValue = nutrientMapArray[x, z];

        return mapValue;
    }

    // Set the pixel in the nutrientMapArray, updating the value of the nutrient map
    // Erase this when you change it, because it's bad and just for testing
    public void setValue(Vector3 position, float updateValue){
        int xCenter = (int)position.x;
        int zCenter = (int)position.z;
        nutrientMapArray[xCenter, zCenter] = updateValue;

        if( nutrientMapArray[xCenter, zCenter] < 0) {
            updateValue = 0;
            nutrientMapArray[xCenter, zCenter] = 0;
        }

        int radius = 10;
        for(int x = xCenter - radius; x <= xCenter; x++){
            for(int z = zCenter - radius; z <= zCenter; z++){
                int dist = (x-xCenter)*(x-xCenter) + (z-zCenter)*(z-zCenter); 
                if( dist <= radius*radius )
                {
                    int xSym = xCenter - (x - xCenter);
                    int zSym = zCenter - (z - zCenter);

                    float updateUpdateValue = updateValue * (1 - (radius - dist)/(radius));
                    
                    nutrientMapArray[x   , z   ] = updateUpdateValue;
                    nutrientMapArray[x   , zSym] = updateUpdateValue;
                    nutrientMapArray[xSym, z   ] = updateUpdateValue;
                    nutrientMapArray[xSym, zSym] = updateUpdateValue;
                }
            }
        }
    }

    // Probably a terrible idea, but here we do a one-time scan through the map
    // and turn all of the pixel's grayscale values into a 2d array
    public void convertImageTo2DArray(Texture2D mapImage){
        Debug.Log ("Converting image to array");

        for (int i = 0; i < mapDimension; i++)
            for (int j = 0; j < mapDimension; j++)
                nutrientMapArray[i, j] = mapImage.GetPixel(i, j).grayscale;

    }

    // Update the alpha maps of the terrain based on nutrientMapArray values
    public void updateMapAlphas(){
        var alphaMaps = new float[t.terrainData.alphamapWidth, t.terrainData.alphamapHeight, t.terrainData.alphamapLayers];

        for(int y = 0; y < t.terrainData.alphamapWidth; y++){
            for(int x = 0; x < t.terrainData.alphamapHeight; x++){

                alphaMaps[y, x, 0] =     nutrientMapArray[x, y]; // grass texture
                alphaMaps[y, x, 2] = 1 - nutrientMapArray[x, y]; // sand texture
                //alphaMaps[y, x, 1] = 0; // dying grass texture
                // if( nutrientMapArray[x, y] > 0.3f){
                //     alphaMaps[y, x, 0] = 1; // grass texture
                //     alphaMaps[y, x, 1] = 0; // dying grass texture
                //     alphaMaps[y, x, 2] = 0; // sand texture
                // } 
                // else if( nutrientMapArray[x, y] > 0.2f){
                //     alphaMaps[y, x, 0] = 0; // grass texture
                //     alphaMaps[y, x, 1] = 1; // dying grass texture
                //     alphaMaps[y, x, 2] = 0; // sand texture
                // }
                // else {
                //     alphaMaps[y, x, 0] = 0; // grass texture
                //     alphaMaps[y, x, 1] = 0; // dying grass texture
                //     alphaMaps[y, x, 2] = 1; // sand texture
                // }
            }
        }

        t.terrainData.SetAlphamaps(0, 0, alphaMaps);
    }

    // public float[,] circleAround(int xCenter, int yCenter, int radius){
    //     for(int x = xCenter - radius; x < xCenter; x++){
    //         for(int y = yCenter - radius; y < yCenter; y++){
    //             if( (x-xCenter)*(x-xCenter) + (y-yCenter)*(y-yCenter) <= radius*radius ){
    //                 int xSym = xCenter - (x - xCenter);
    //                 int ySym = yCenter - (y - yCenter);
    //                 nutrientMapArray[x, y] = 
    //                 nutrientMapArray[x, ySym] = 
    //                 nutrientMapArray[xSym, y] = 
    //                 nutrientMapArray[xSym, ySym] = 
    //             }
    //         }
    //     }
    // }
}