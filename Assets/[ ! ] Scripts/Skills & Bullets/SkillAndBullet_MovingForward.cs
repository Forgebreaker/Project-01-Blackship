using UnityEngine;

public class SkillAndBullet_MovingForward : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 12.5f;
    void Update()
    {
        MovingForward();
        DestroyWhenOutOfScreen();
    }

    private void MovingForward() 
    {
        this.gameObject.transform.Translate(new Vector2(0, MoveSpeed * Time.deltaTime), Space.Self);
    }

    private void DestroyWhenOutOfScreen() 
    {
        if (   this.gameObject.transform.position.y >= 20f 
            || this.gameObject.transform.position.y <= -20f
            || this.gameObject.transform.position.x <= -20f
            || this.gameObject.transform.position.x >= 20f
           ) 
        {
            Destroy(this.gameObject);
        }
    }

}
