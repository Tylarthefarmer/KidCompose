using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecRightNote : MonoBehaviour
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
		FinerGames.PitchDetector.Demo.PlaceNote timelineGet = timeline.GetComponent<FinerGames.PitchDetector.Demo.PlaceNote>();
		timelineGet.noteSelected=Mathf.Min(timelineGet.noteSelected+1,timelineGet.placedNoteList.Count);
		timelineGet.changeNote=true;
	}
}
