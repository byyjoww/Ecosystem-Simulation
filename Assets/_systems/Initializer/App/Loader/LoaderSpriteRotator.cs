using UnityEngine;
using UnityEngine.UI;

public class LoaderSpriteRotator : MonoBehaviour
{
    public float timeInterval;
    public Image loaderSprite;

    [Range(-360,360)]
    public int rotationStep;

    float timer = 0;
    
    // Start is called before the first frame update
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeInterval)
        {
            loaderSprite.transform.Rotate(new Vector3(0, 0, rotationStep));
            timer -= timeInterval;
        }
    }
}
