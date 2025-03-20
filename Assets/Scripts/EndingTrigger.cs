using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingTrigger : MonoBehaviour
{
    public float fadeOutTime = 3;
    private SpriteRenderer spriteRenderer;
    public GameObject blackScreen;
    public GameObject candleFlame;
    

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = blackScreen.GetComponent<SpriteRenderer>();
        //StartCoroutine(FadeOutScene(spriteRenderer));
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            candleFlame.SetActive(false);
            StartCoroutine(FadeOutScene(spriteRenderer));
        }
    }

    // set alpha of white screen slowly higher to mimic a fade to white
    IEnumerator FadeOutScene(SpriteRenderer sprite)
    {
        Color tempColor = sprite.color;

        while (tempColor.a < 1)
        {
            tempColor.a += Time.deltaTime / fadeOutTime;
            sprite.color = tempColor;
            yield return null;
        }
        sprite.color = tempColor;
        
        SceneManager.LoadSceneAsync("End Screen", LoadSceneMode.Single);
       
    }
}
