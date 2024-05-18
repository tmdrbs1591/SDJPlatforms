using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Potal : MonoBehaviour
{
    public string type;
    public GameObject fadein;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetKey(KeyCode.F)) // Æ÷Å» Å¸±â
        {
            StartCoroutine(FadeScene());
            Debug.Log("d");
        }
    }

    IEnumerator FadeScene()
    {
        fadein.SetActive(true);
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(type);
    }
}
