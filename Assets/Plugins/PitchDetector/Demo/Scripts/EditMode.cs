using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditMode : MonoBehaviour
{

	public GameObject timeline;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

	void OnMouseDown(){
		timelinemove timelineGet = timeline.GetComponent<timelinemove>();
		timelineGet.status = 0;
		timelineGet.transform.position = timelineGet.spawnPos;	
	}
}
