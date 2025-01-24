using UnityEngine;

public class MainKey : BaseKey, IKey
{
    public MainKey(Character character) : base(character)
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Detect Something");
        if (collision.gameObject.CompareTag("MainKey"))
        {
            Debug.Log("Main Key dah nabrak boiiiiiii");
        }
        else if (collision.gameObject.CompareTag("NormalKey"))
        {
            Debug.Log("Nabrak Normal Key");
            NormalKey normalKey = collision.gameObject.GetComponent<NormalKey>();
            if (normalKey != null)
            {
                AttachNormalKey(normalKey);
            }
            else
            {
                Debug.LogError("Collided object with tag 'NormalKey' does not have a NormalKey component.");
            }
        }
    }

    private void AttachNormalKey(NormalKey normalKey)
    {
        normalKey.IsAttached = true;
        normalKey.transform.SetParent(this.transform);
        normalKey.transform.localPosition = new Vector3(1,1,0);
        Debug.Log("Attached boi");
    }

    public override bool IsDestructable => true;
}