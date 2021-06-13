using UnityEngine;


// From Brackeys
public class Rope : MonoBehaviour {

    // First Rope Segment
	public Rigidbody2D hook;

    // Rope Prefab
	public GameObject linkPrefab;

    // End Weight
	public Weight weigth;

    // num of links
	public int links = 7;

	void Start () {
		GenerateRope();
	}

	void GenerateRope ()
	{
		Rigidbody2D previousRB = hook;
		for (int i = 0; i < links; i++)
		{
			GameObject link = Instantiate(linkPrefab, transform);
			HingeJoint2D joint = link.GetComponent<HingeJoint2D>();
			joint.connectedBody = previousRB;

			if (i < links - 1)
			{
				previousRB = link.GetComponent<Rigidbody2D>();
			} else
			{
				weigth.ConnectRopeEnd(link.GetComponent<Rigidbody2D>());
			}

			
		}
	}

}