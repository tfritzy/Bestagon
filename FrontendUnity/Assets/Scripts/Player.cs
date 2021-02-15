using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Ball;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Try create projectile");
            Vector2 position = Input.mousePosition;
            RequestProjectileCreation(position);
        }
    }

    private void RequestProjectileCreation(Vector2 inputPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);
        Debug.Log($"Requesting projectile be created at {worldPosition}");
        Schema.ProjectileCreated projectileCreated = new Schema.ProjectileCreated
        {
            Position = worldPosition.ToContract(),
            Velocity = Random.insideUnitCircle.ToContract(),
        };

        Managers.Client.SendMessage(Any.Pack(projectileCreated));
    }

    public void InstantiateProjectile(Schema.ProjectileCreated projectileCreated)
    {
        Debug.Log($"Projectile created at {projectileCreated.Position}");
        GameObject ball = Instantiate(Ball, projectileCreated.Position.ToInternal(), new Quaternion());
        ball.GetComponent<Rigidbody2D>().velocity = projectileCreated.Velocity.ToInternal();
    }
}
