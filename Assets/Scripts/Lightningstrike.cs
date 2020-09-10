using UnityEngine;
using System.Collections;

public class Lightningstrike : MonoBehaviour {

	public GameObject bolt;

	public LineRenderer line;

	public bool linePowered = false;

	bool isPlaying = false;

	float spriteSizey;

	// Use this for initialization
	void Start () {
		Sprite sprite = bolt.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
		spriteSizey = sprite.rect.height / sprite.pixelsPerUnit;
		bolt.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {	
		if(linePowered == true && line.enabled)
		{
			Vector2 point1 = line.GetPosition(0);
			Vector2 point2 = line.GetPosition(1);
			point2 += new Vector2(0.0f, 1.0f);
			
			bolt.transform.position = point1;
			Vector3 temp = bolt.transform.position;
			bolt.transform.position = temp;
					
			Vector3 scale = bolt.transform.localScale;
			scale.y = (point2-point1).magnitude / (spriteSizey * 3f);
			bolt.transform.localScale = scale;

			Vector2 _direction = (point2 - point1).normalized;  
			float rot_z = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
			bolt.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
			bolt.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
			if(isPlaying == false)
			{
				isPlaying = true;
				bolt.transform.GetChild(0).gameObject.GetComponent<Animator>().Play("spark",-1,0f);
			}
		}
		/*else
		{
			bolt.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
		}*/
	}
}
