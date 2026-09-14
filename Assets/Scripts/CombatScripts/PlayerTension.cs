using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerTension : MonoBehaviour
{
    public Image tentionBar;
    private GameObject player;

    private bool isTensionActive;
    private float speed = 0.1f;

    private Coroutine tentionCoroutine;

    public void Start()
    {
        tentionBar.fillAmount = 0f;
        isTensionActive = true; 
        
        tentionCoroutine = StartCoroutine(Tention());
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (tentionCoroutine != null)
            {
                StopCoroutine(tentionCoroutine);
                player = GameObject.FindGameObjectWithTag("Player");
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                playerMovement.ArrowSpawn(tentionBar.fillAmount);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator Tention()
    {
        while (true)
        {
            if (isTensionActive)
            {
                tentionBar.fillAmount += Time.fixedDeltaTime * speed;
                if (tentionBar.fillAmount >= 1f)
                {
                    isTensionActive = false;
                }
            }
            else
            {
                tentionBar.fillAmount -= Time.fixedDeltaTime * speed;
                if (tentionBar.fillAmount <= 0f)
                {
                    isTensionActive = true;
                }
            }
            yield return null;
        }
    }
}
