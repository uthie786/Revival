using UnityEngine;

public class TextureOffsetController : MonoBehaviour
{
    public Material material; // Reference to the material
    public float scrollSpeed = 0.1f; // Speed at which the texture scrolls

    private float offsetX = 0f;

    void Update()
    {
        offsetX += scrollSpeed * Time.deltaTime;
        material.mainTextureOffset = new Vector2(offsetX, material.mainTextureOffset.y);
    }
}
