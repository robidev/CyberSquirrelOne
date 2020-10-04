using UnityEngine;

/// <summary>
/// Used to move a transform relative to the main camera position with a scale factor applied.
/// This is used to implement parallax scrolling effects on different branches of gameobjects.
/// </summary>

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour 
{
   public float referenceSize = 2;

   public float xParallax = 0.2f;
   public float yParallax = 0.2f;
   public float sizeParallax = 0.2f;
 
   void LateUpdate () 
   {
      float sizeRatio = (Camera.main.orthographicSize/referenceSize);
      float newSize = (sizeRatio - 1) * sizeParallax + 1;
  
      Vector3 tempScale = transform.localScale;
      tempScale.x = newSize;
      tempScale.y = newSize;
      transform.localScale = tempScale;

      Vector3 tempPos = transform.position;

      tempPos.x = Camera.main.transform.position.x * (1 -newSize + newSize*xParallax );
      tempPos.y = Camera.main.transform.position.y * (1 -newSize + newSize*yParallax );
        
      transform.position = tempPos;
   }
}