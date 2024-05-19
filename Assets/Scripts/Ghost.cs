using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float ghostDelay;
    [SerializeField] private GameObject ghost;

    private float ghostDelaySeconds;
    public bool makeGhost = false;
    // Start is called before the first frame update
    void Start()
    {
        ghostDelaySeconds = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (makeGhost)
        {

            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                //Generate a ghost
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                Sprite currentSprite = GetComponentInChildren<SpriteRenderer>().sprite;

                currentGhost.transform.localScale = this.transform.localScale;

                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                ghostDelaySeconds = ghostDelay;
                Destroy(currentGhost,1f);

            }
        }
    }
}
