using UnityEngine;

            
public class FollowPlayer : MonoBehaviour
{
    
    public float speed = 8.0f;

    public GameObject player;   

    void Update()
    {

        Vector3 playerPosition = player.transform.position + new Vector3(0f, 2f, 0f);

        Vector3 position = Vector3.Lerp(transform.position, playerPosition + player.transform.right*1.5f, 1.0f - Mathf.Exp(-speed * Time.deltaTime));

        transform.position = position;
    }    
}

            