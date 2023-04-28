using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordButton : MonoBehaviour
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
		timelineGet.status = 1;
		timelineGet.transform.position = timelineGet.spawnPos;	

		FinerGames.PitchDetector.Demo.PlaceNote timelineClear = timeline.GetComponent<FinerGames.PitchDetector.Demo.PlaceNote>();
		timelineClear.resetNotes = true;	
	}
}
