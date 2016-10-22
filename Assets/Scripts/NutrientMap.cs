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
    public  int       nextUpdate;
    public  float     timer;

    // Initialization 
    public void Start(){
        if( nutrientMapTex == null )
            Debug.Log("You forgot your nutrientMapTexture");

        timer = 0;                      // set timer to 0
        nextUpdate = 1;                 // time between map udpates
        t = Terrain.activeTerrain;      // find the active terrain in scene
        mapDimension = 513;             // bad, bad magic numbers, issues with getting the size of the map at runtime
        nutrientMapArray = new float[mapDimension, mapDimension]; // init nutrient map array
        convertImageTo2DArray(nutrientMapTex);                    // convert the given nutrient map image to the array
        Debug.Log("nutrientMapArray dimensions are " + nutrientMapArray.GetLength(0) + "x" + nutrientMapArray.GetLength(1) );
    }

    // Unity Update
    public void Update(){
        timer += Time.deltaTime;

        if(timer > nextUpdate){
            updateMapAlphas();
            nextUpdate = 5;
            timer = 0;
        }
    }

    // Get the Nutrient Map Array
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

        // if( nutrientMapArray[xCenter, zCenter] < 0) {
        //     updateValue = 0;
        //     nutrientMapArray[xCenter, zCenter] = 0;
        // }


        // Circle with radius 20 around tree will be affected
        // We find one quater of the circle, then use symmetry because speed
        // Update value gets placed in every array index within radius around tree
        int radius = 25;
        for(int x = xCenter - radius; x <= xCenter; x++){
            for(int z = zCenter - radius; z <= zCenter; z++){
                int dist = (x-xCenter)*(x-xCenter) + (z-zCenter)*(z-zCenter); 
                if( dist <= radius*radius )
                {
                    int xSym = xCenter - (x - xCenter);
                    int zSym = zCenter - (z - zCenter);

                    // This is just a gradient, farther away gets affected less
                    // Father forgive me for this name, we grow farther from your
                    // light every day
                    float updateUpdateValue = updateValue;// - (updateValue * (1 - dist/radius));
                    
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
    // Right now, gradient between the two maps is used, 
    public void updateMapAlphas(){
        var alphaMaps = new float[t.terrainData.alphamapWidth, t.terrainData.alphamapHeight, t.terrainData.alphamapLayers];

        for(int y = 0; y < t.terrainData.alphamapWidth; y++){
            for(int x = 0; x < t.terrainData.alphamapHeight; x++){

                // alphaMaps[y, x, 0] = 2 * nutrientMapArray[x, y]; // grass texture
                // alphaMaps[y, x, 2] = 1 - nutrientMapArray[x, y]; // sand texture
                //alphaMaps[y, x, 1] = 0; // dying grass texture
                if( nutrientMapArray[x, y] > 0.4f){
                    alphaMaps[y, x, 0] = 1; // grass 0
                    alphaMaps[y, x, 1] = 0; // grass 1
                    alphaMaps[y, x, 2] = 0; // grass 2
                    alphaMaps[y, x, 3] = 0; // grass 3
                    alphaMaps[y, x, 4] = 0; // grass 4  
                    alphaMaps[y, x, 5] = 0; // grass 5
                } 
                else if( nutrientMapArray[x, y] > 0.3f){
                    alphaMaps[y, x, 0] = 0; // grass 0
                    alphaMaps[y, x, 1] = 1; // grass 1
                    alphaMaps[y, x, 2] = 0; // grass 2
                    alphaMaps[y, x, 3] = 0; // grass 3
                    alphaMaps[y, x, 4] = 0; // grass 4
                    alphaMaps[y, x, 5] = 0; // grass 5
                }
                else if( nutrientMapArray[x, y] > 0.2f){
                    alphaMaps[y, x, 0] = 0; // grass 0
                    alphaMaps[y, x, 1] = 0; // grass 1
                    alphaMaps[y, x, 2] = 1; // grass 2
                    alphaMaps[y, x, 3] = 0; // grass 3
                    alphaMaps[y, x, 4] = 0; // grass 4
                    alphaMaps[y, x, 5] = 0; // grass 5
                }
                else if( nutrientMapArray[x, y] > 0.1f){
                    alphaMaps[y, x, 0] = 0; // grass 0
                    alphaMaps[y, x, 1] = 0; // grass 1
                    alphaMaps[y, x, 2] = 0; // grass 2
                    alphaMaps[y, x, 3] = 1; // grass 3
                    alphaMaps[y, x, 4] = 0; // grass 4
                    alphaMaps[y, x, 5] = 0; // grass 5
                }
                else if( nutrientMapArray[x, y] > 0.05f){
                    alphaMaps[y, x, 0] = 0; // grass 0
                    alphaMaps[y, x, 1] = 0; // grass 1
                    alphaMaps[y, x, 2] = 0; // grass 2
                    alphaMaps[y, x, 3] = 0; // grass 3
                    alphaMaps[y, x, 4] = 1; // grass 4
                    alphaMaps[y, x, 5] = 0; // grass 5
                }
                else {
                    alphaMaps[y, x, 0] = 0; // grass 0
                    alphaMaps[y, x, 1] = 0; // grass 1
                    alphaMaps[y, x, 2] = 0; // grass 2
                    alphaMaps[y, x, 3] = 0; // grass 3
                    alphaMaps[y, x, 4] = 0; // grass 4
                    alphaMaps[y, x, 5] = 1; // grass 5
                }
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