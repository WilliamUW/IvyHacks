using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro; // Include the TextMeshPro namespace

public class SceneController : MonoBehaviour
{
    public TMP_InputField inputField; // Assign in the Inspector

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public Material skyboxMaterial; // Assign a material with the Skybox shader in the editor
    public Material testMaterial;
    public Texture2D newSkyboxTexture; // Assign this in the Inspector

    public Cubemap skyboxCubemap; // Assign this in the Inspector


// Assign these textures in the Inspector
    public Texture2D frontTexture;
    public Texture2D backTexture;
    public Texture2D leftTexture;
    public Texture2D rightTexture;
    public Texture2D upTexture;
    public Texture2D downTexture;

    public int folderIndex = 0;
    public string[] folderArray = new string[] {"Manhattan", "Pyramids", "EiffelTower", "Apocalytic", "Moon", "CubeMap", "Colliding"};

    void LoadTexturesFromResources(string folder)
    {
        frontTexture = Resources.Load<Texture2D>("Skyboxes/" + folder + "/cube_front");
        backTexture = Resources.Load<Texture2D>("Skyboxes/" + folder + "/cube_back");
        leftTexture = Resources.Load<Texture2D>("Skyboxes/" + folder + "/cube_left");
        rightTexture = Resources.Load<Texture2D>("Skyboxes/" + folder + "/cube_right");
        upTexture = Resources.Load<Texture2D>("Skyboxes/" + folder + "/cube_up");
        downTexture = Resources.Load<Texture2D>("Skyboxes/" + folder + "/cube_down");
    }
    public void ChangeSkyboxTexture()
    {
        string folder = "CubeMap";
        string userInput = inputField.text.ToLower();
        if (userInput.Contains("eiffel"))
        {
            folder = "EiffelTower";
        }
        else if (userInput.Contains("colliding"))
        {
            folder = "colliding";
        }
        else if (userInput.Contains("anime"))
        {
            folder = "colliding";
        }
        else if (userInput.Contains("pyramids"))
        {
            folder = "Pyramids";
        }
        else if (userInput.Contains("apocalyptic") || userInput.Contains("post"))
        {
            folder = "Apocalytic";
        }
        else if (userInput.Contains("moon"))
        {
            folder = "moon";
        }
        else if (userInput.Contains("manhattan"))
        {
            folder = "Manhattan";
        }
        else
        {
            folder = "CubeMap";
        }
        LoadTexturesFromResources(folder);
        Cubemap cubemap = CreateCubemapFromTextures(frontTexture, backTexture, leftTexture, rightTexture, upTexture, downTexture);
        Debug.Log(cubemap);
        skyboxMaterial.SetTexture("_Tex", cubemap);
        RenderSettings.skybox = skyboxMaterial;
        DynamicGI.UpdateEnvironment();
    }

    public void ChangeSkyboxTextureRotate()
    {
        string folder = "CubeMap";
        folder = folderArray[folderIndex];
        folderIndex = (folderIndex + 1) % folderArray.Length;
        LoadTexturesFromResources(folder);
        Cubemap cubemap = CreateCubemapFromTextures(frontTexture, backTexture, leftTexture, rightTexture, upTexture, downTexture);
        Debug.Log(cubemap);
        skyboxMaterial.SetTexture("_Tex", cubemap);
        RenderSettings.skybox = skyboxMaterial;
        DynamicGI.UpdateEnvironment();
    }

    Cubemap CreateCubemapFromTextures(Texture2D front, Texture2D back, Texture2D left, Texture2D right, Texture2D up, Texture2D down)
    {
        int size = front.width; // Assuming all textures are the same size and square
        Cubemap cubemap = new Cubemap(size, TextureFormat.RGBA32, false);

        cubemap.SetPixels(FlipTextureHorizontallyAndVertically(front).GetPixels(), CubemapFace.PositiveZ);
        cubemap.SetPixels(FlipTextureHorizontallyAndVertically(back).GetPixels(), CubemapFace.NegativeZ);
        cubemap.SetPixels(FlipTextureHorizontallyAndVertically(left).GetPixels(), CubemapFace.NegativeX);
        cubemap.SetPixels(FlipTextureHorizontallyAndVertically(right).GetPixels(), CubemapFace.PositiveX);
        cubemap.SetPixels((up).GetPixels(), CubemapFace.PositiveY);
        cubemap.SetPixels((down).GetPixels(), CubemapFace.NegativeY);

        cubemap.Apply(); // Apply the changes to the cubemap

        return cubemap;
    }

    Texture2D FlipTextureHorizontallyAndVertically(Texture2D original)
{
    int width = original.width;
    int height = original.height;
    Texture2D flipped = new Texture2D(width, height);

    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            // Get the original pixel from the opposite corner
            Color originalPixel = original.GetPixel(width - 1 - j, height - 1 - i);
            flipped.SetPixel(j, i, originalPixel);
        }
    }

    flipped.Apply(); // Apply all SetPixel changes
    return flipped;
}


    Texture2D FlipTextureHorizontally(Texture2D original)
{
    Texture2D flipped = new Texture2D(original.width, original.height);
    
    for (int i = 0; i < original.width; i++)
    {
        for (int j = 0; j < original.height; j++)
        {
            // Get the original pixel from the original texture and set it in the flipped texture
            // but in a horizontally mirrored position.
            flipped.SetPixel(original.width - 1 - i, j, original.GetPixel(i, j));
        }
    }
    
    flipped.Apply(); // Apply all SetPixel changes.
    return flipped;
}

    Texture2D FlipVertically(Texture2D original)
    {
        Texture2D flipped = new Texture2D(original.width, original.height);
        int xN = original.width;
        int yN = original.height;

        for (int i = 0; i < xN; i++)
        {
            for (int j = 0; j < yN; j++)
            {
                flipped.SetPixel(i, yN - j - 1, original.GetPixel(i, j));
            }
        }
        flipped.Apply();

        return flipped;
    }
}
