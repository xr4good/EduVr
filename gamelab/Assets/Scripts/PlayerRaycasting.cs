using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerRaycasting : MonoBehaviour
{
    public float distanceToSee;
    RaycastHit whatIHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

         if ( Input.GetMouseButtonDown (0)){ 
              if(Physics.Raycast(this.transform.position, this.transform.forward, out whatIHit,distanceToSee)){
                  IPointerClickHandler clickHandler=whatIHit.collider.gameObject.GetComponent<IPointerClickHandler>();
                  if(clickHandler!=null){
                      PointerEventData pointerEventData=new PointerEventData(EventSystem.current);
                        clickHandler.OnPointerClick(pointerEventData);
                  }
            Debug.Log("touched "+ whatIHit.collider.gameObject.name);
        }
        }
        //Debug.DrawRay(this.transform.position, this.transform.forward*distanceToSee, Color.magenta);
       
    }
}
