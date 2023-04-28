using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;

public class VoiceRecognition : MonoBehaviour
{
	public GameObject timeline;
    private KeywordRecognizer keywordRecognizer;
	private Dictionary<string, UnityAction> actions = new Dictionary<string, UnityAction>();

	void Start(){
		actions.Add("record", Record);
		actions.Add("play", Play);
		actions.Add("louder",Louder);
		actions.Add("quieter", Quieter);
		actions.Add("up", Up);
		actions.Add("down", Down);
		actions.Add("left", Left);
		actions.Add("right", Right);
		actions.Add("stop", Edit);

		keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
		keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
		keywordRecognizer.Start();
	}

	private void RecognizedSpeech(PhraseRecognizedEventArgs speech){
		Debug.Log(speech.text);
		actions[speech.text].Invoke();
	}

	private void Record(){
		timelinemove timelineGet = timeline.GetComponent<timelinemove>();
		timelineGet.status = 1;
		timelineGet.transform.position = timelineGet.spawnPos;	

		FinerGames.PitchDetector.Demo.PlaceNote timelineClear = timeline.GetComponent<FinerGames.PitchDetector.Demo.PlaceNote>();
		timelineClear.resetNotes = true;	
	}
	private void Play(){
		timelinemove timelineGet = timeline.GetComponent<timelinemove>();
		timelineGet.status = 2;
		timelineGet.transform.position = timelineGet.spawnPos;	
	}
	private void Louder(){
		FinerGames.PitchDetector.Demo.PlaceNote timelineGet = timeline.GetComponent<FinerGames.PitchDetector.Demo.PlaceNote>();
		timelineGet.increaseVolume=true;
	}
	private void Quieter(){
		FinerGames.PitchDetector.Demo.PlaceNote timelineGet = timeline.GetComponent<FinerGames.PitchDetector.Demo.PlaceNote>();
		timelineGet.decreaseVolume=true;
	}
	private void Up(){
		FinerGames.PitchDetector.Demo.PlaceNote timelineGet = timeline.GetComponent<FinerGames.PitchDetector.Demo.PlaceNote>();
		timelineGet.raisePitchValue=true;
	}
	private void Down(){
		FinerGames.PitchDetector.Demo.PlaceNote timelineGet = timeline.GetComponent<FinerGames.PitchDetector.Demo.PlaceNote>();
		timelineGet.lowerPitchValue=true;
	}
	private void Left(){
		FinerGames.PitchDetector.Demo.PlaceNote timelineGet = timeline.GetComponent<FinerGames.PitchDetector.Demo.PlaceNote>();
		timelineGet.noteSelected=Mathf.Max(timelineGet.noteSelected-1,0);
		timelineGet.changeNote=true;
	}
	private void Right(){
		FinerGames.PitchDetector.Demo.PlaceNote timelineGet = timeline.GetComponent<FinerGames.PitchDetector.Demo.PlaceNote>();
		timelineGet.noteSelected=Mathf.Min(timelineGet.noteSelected+1,timelineGet.placedNoteList.Count);
		timelineGet.changeNote=true;
	}
	private void Edit(){
		timelinemove timelineGet = timeline.GetComponent<timelinemove>();
		timelineGet.status = 0;
		timelineGet.transform.position = timelineGet.spawnPos;	
	}
}
