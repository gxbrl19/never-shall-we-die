using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCheck : MonoBehaviour
{
    private float waterCheckRadius = 0.1f;
    private Vector3 waterCheckDistance = new Vector3(0f, 0.7f, 0f);
    public GameObject outWaterPoint;
    public GameObject dropWater;
    public LayerMask waterLayer;

    Player player;

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    void FixedUpdate()
    {
        if (player.isDead)
            return;

        player.playerCollision.outWaterHit = !Physics2D.OverlapCircle(outWaterPoint.transform.position, waterCheckRadius, waterLayer);
        player.playerCollision.inWaterHit = Physics2D.OverlapCircle(outWaterPoint.transform.position - waterCheckDistance, waterCheckRadius, waterLayer);
        player.onWater = player.playerCollision.inWaterHit;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            if (player.onWater)
                return;

            Vector3 position = other.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(new Vector3(transform.position.x, other.transform.position.y + 5f, other.transform.position.z));
            player.playerAudio.PlayWaterSplash();
            Instantiate(dropWater, position, other.transform.rotation);

            if (player.isRolling)
            {
                player.playerMovement.FinishRoll(); //cancela o Roll ao entrar na água
                return;
            }

            if (player.playerInputs.pressParachute == true) { player.playerInputs.pressParachute = false; } //cancela o parachute

            player.EnterInWater();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            if (!player.onWater)
                return;

            Vector3 position = other.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(new Vector3(transform.position.x, other.transform.position.y, other.transform.position.z));

            Instantiate(dropWater, position, other.transform.rotation);
        }
    }

    private void OnDrawGizmos()
    {
        //water
        Gizmos.color = Color.blue; //check out water
        Gizmos.DrawWireSphere(outWaterPoint.transform.position, waterCheckRadius);
        Gizmos.DrawWireSphere(outWaterPoint.transform.position, waterCheckRadius);
        Gizmos.color = Color.blue; //check in water
        Gizmos.DrawWireSphere(outWaterPoint.transform.position - waterCheckDistance, waterCheckRadius);
        Gizmos.DrawWireSphere(outWaterPoint.transform.position - waterCheckDistance, waterCheckRadius);
    }
}
