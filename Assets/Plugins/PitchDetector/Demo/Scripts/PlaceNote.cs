using System.Collections;
using System.Collections.Generic;
using Pitch;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(AudioSource))]
namespace FinerGames.PitchDetector.Demo
{
	public class PlaceNote : MonoBehaviour
	{
		public GameObject note;
		public GameObject timeline;
		public GameObject eighthrest;
		public static float tempo = 120f;
		float[] noteArray;
		float[] sharpsArray;
		public List<GameObject> placedNoteList = new List<GameObject>();
		static float secondsPerBeat = 60/tempo;
		static float secondsPerEighthNote=secondsPerBeat/2;
		int status;
		int octave=12;
		public int noteSelected=-1;
		public bool changeNote=false;
		public bool raisePitchValue=false;
		public bool lowerPitchValue=false;
		public bool increaseVolume=false;
		public bool decreaseVolume=false;
		float currentPitch=0;
		[SerializeField] PitchDetector detector;
		float elapsed = 0f;
		float metronome = 0f;
		AudioSource metronomeTick;
		
		void Start(){
			//Populate note array with all notes in reasonable human vocal range
			noteArray = new float[45];
			for(int i=0; i<45;i++){
				noteArray[i]=77.782f*Mathf.Exp(0.0578f*i);
			}

			metronomeTick = GetComponent<AudioSource>();
		}

		void Update()
		{
			//get timeline movement status. If recording, do recording stuff, if playing, do playing stuff
			timelinemove timelineStatus = timeline.GetComponent<timelinemove>();
			status = timelineStatus.status;
			//update status of all notes placed
			for (int i=0; i<placedNoteList.Count;i++){
				noteData nStatus = placedNoteList[i].GetComponent<noteData>();
				nStatus.status = status;
			}
			if (status==1)
			{
				//checking this live for ease, but at the end, make it so all rhythm and stuff happens after the recording is complete
				elapsed += Time.deltaTime;
				metronome += Time.deltaTime;

				//set currentPitch to non-zero pitch if a pitch in range is detected
				if (getPitch() > 0 && getPitch()<=noteArray[noteArray.Length-1])
				{
					currentPitch=getPitch();
				}
				//every frame, check for pitch. If pitch is heard, that is the set pitch for the time.
				//every eigth note, check for a pitch. If pitch is there, put a note. If not, put a rest.
				if (elapsed >= secondsPerEighthNote)
				{
					elapsed = elapsed % secondsPerEighthNote;
					//get the note index in the array and place it. If it is nothing, then eighth rest
						placeObject(getNote(currentPitch));
					currentPitch=0;
				}

				//play metronome
				if (metronome >= secondsPerBeat)
				{
					metronome = metronome % secondsPerBeat;
					metronomeTick.Play(0);
				}
			}
			else if (status==0){
				if (changeNote){
					changeNote=selectNote(noteSelected);
				}
				if (raisePitchValue){
					raisePitchValue=raisePitch(noteSelected);
				}
				if (lowerPitchValue){
					lowerPitchValue=lowerPitch(noteSelected);
				}
				if (increaseVolume){
					increaseVolume=increaseVol(noteSelected);
				}
				if (decreaseVolume){
					decreaseVolume=decreaseVol(noteSelected);
				}
			}
		}

		void placeObject(int noteIndex)
		{
			if (noteIndex<0){
				if (placedNoteList.Count!=0){
					noteData prevNote = placedNoteList[placedNoteList.Count-1].GetComponent<noteData>();
					if(!prevNote.quarterNote){
						prevNote.quarterNote=true;
						prevNote.noteChange=true;
						return;
					}
				}
			}
			var noteClone = Instantiate(note,new Vector3(transform.position.x,transform.position.y-100+noteIndex*transform.localScale.y/noteArray.Length,transform.position.z),Quaternion.identity) as GameObject;
			//check if note is sharp
			if (noteIndex%octave==0 || noteIndex%octave==3 || noteIndex%octave==5 || noteIndex%octave==7 || noteIndex%octave==10){
				noteClone.transform.GetChild(4).GetComponent<Renderer>().enabled=true;
				noteClone.transform.position= new Vector3(noteClone.transform.position.x,transform.position.y-100+(noteIndex-1)*transform.localScale.y/noteArray.Length,noteClone.transform.position.z);
			}
			noteData nP = noteClone.GetComponent<noteData>();
			nP.notePitch = noteIndex;
			placedNoteList.Add(noteClone);
		}

		int getNote(float rawPitch){
			Debug.Log(rawPitch);
			int i = 0;
			if (rawPitch!=0){
				while (!(rawPitch >= noteArray[i] && rawPitch <= noteArray[i+1])){
					i++;
					if (rawPitch >= noteArray[i] && rawPitch <= noteArray[i+1]){
						if (Mathf.Abs(rawPitch-noteArray[i]) < Mathf.Abs(rawPitch-noteArray[i+1])){
							return i;
						}
						else{
							return i+1;
						}
					}
				}
			}
			return -1;
		}

		bool selectNote(int noteSelected){
			var noteGet = placedNoteList[noteSelected].transform.GetChild(0).GetComponent<Renderer>();
			for(int i = 0; i<placedNoteList.Count; i++){
				noteGet = placedNoteList[i].transform.GetChild(0).GetComponent<Renderer>();
				noteGet.material.color=Color.black;
				noteGet = placedNoteList[i].transform.GetChild(1).GetComponent<Renderer>();
				noteGet.material.color=Color.black;
				noteGet = placedNoteList[i].transform.GetChild(2).GetComponent<Renderer>();
				noteGet.material.color=Color.black;
				noteGet = placedNoteList[i].transform.GetChild(3).GetComponent<Renderer>();
				noteGet.material.color=Color.black;
				noteGet = placedNoteList[i].transform.GetChild(4).GetComponent<Renderer>();
				noteGet.material.color=Color.black;

			}
			noteGet = placedNoteList[noteSelected].transform.GetChild(0).GetComponent<Renderer>();
			noteGet.material.color=Color.blue;
			noteGet = placedNoteList[noteSelected].transform.GetChild(1).GetComponent<Renderer>();
			noteGet.material.color=Color.blue;
			noteGet = placedNoteList[noteSelected].transform.GetChild(2).GetComponent<Renderer>();
			noteGet.material.color=Color.blue;
			noteGet = placedNoteList[noteSelected].transform.GetChild(3).GetComponent<Renderer>();
			noteGet.material.color=Color.blue;
			noteGet = placedNoteList[noteSelected].transform.GetChild(4).GetComponent<Renderer>();
			noteGet.material.color=Color.blue;
			return false;
		}

		bool raisePitch(int noteSelected){
			noteData nPitch = placedNoteList[noteSelected].GetComponent<noteData>();
			nPitch.notePitch = Mathf.Min(nPitch.notes.Length-1,nPitch.notePitch+1);
			nPitch.transform.position = new Vector3(nPitch.transform.position.x,transform.position.y-100+nPitch.notePitch*transform.localScale.y/noteArray.Length,nPitch.transform.position.z);
			if (nPitch.notePitch%octave==0 || nPitch.notePitch%octave==3 || nPitch.notePitch%octave==5 || nPitch.notePitch%octave==7 || nPitch.notePitch%octave==10){
				nPitch.transform.GetChild(4).GetComponent<Renderer>().enabled=true;
				nPitch.transform.position = new Vector3(nPitch.transform.position.x,transform.position.y-100+(nPitch.notePitch-1)*transform.localScale.y/noteArray.Length,nPitch.transform.position.z);
			}
			else{
				nPitch.transform.GetChild(4).GetComponent<Renderer>().enabled=false;
			}
			nPitch.noteChange=true;
			return false;
		}

		bool lowerPitch(int noteSelected){
			noteData nPitch = placedNoteList[noteSelected].GetComponent<noteData>();
			nPitch.notePitch = Mathf.Max(-1,nPitch.notePitch-1);
			nPitch.transform.position = new Vector3(nPitch.transform.position.x,transform.position.y-100+nPitch.notePitch*transform.localScale.y/noteArray.Length,nPitch.transform.position.z);
			if (nPitch.notePitch%octave==0 || nPitch.notePitch%octave==3 || nPitch.notePitch%octave==5 || nPitch.notePitch%octave==7 || nPitch.notePitch%octave==10){
				nPitch.transform.GetChild(4).GetComponent<Renderer>().enabled=true;
				nPitch.transform.position = new Vector3(nPitch.transform.position.x,transform.position.y-100+(nPitch.notePitch-1)*transform.localScale.y/noteArray.Length,nPitch.transform.position.z);
			}
			else{
				nPitch.transform.GetChild(4).GetComponent<Renderer>().enabled=false;
			}
			nPitch.noteChange=true;
			return false;
		}

		bool increaseVol(int noteSelected){
		//denote this note as a landmark note, then set every note afterwards as the specificied dynamic.
			noteData nPitch = placedNoteList[noteSelected].GetComponent<noteData>();
			nPitch.volume = Mathf.Min(3,nPitch.volume+1);
			var volumeSet = nPitch.volume;
			nPitch.landmarkDynamic=true;
			nPitch.noteChange=true;
			for (int i=noteSelected+1; i<placedNoteList.Count;i++){
				noteData nStatus = placedNoteList[i].GetComponent<noteData>();
				if (!nStatus.landmarkDynamic){
					nStatus.volume=volumeSet;
					nStatus.noteChange=true;
				}
				else{
					return false;
				}
			}
			return false;
		}

		bool decreaseVol(int noteSelected){
		//denote this note as a landmark note, then set every note afterwards as the specificied dynamic.
			noteData nPitch = placedNoteList[noteSelected].GetComponent<noteData>();

			nPitch.volume = Mathf.Max(0,nPitch.volume-1);
			var volumeSet = nPitch.volume;
			nPitch.landmarkDynamic=true;
			nPitch.noteChange=true;
			for (int i=noteSelected+1; i<placedNoteList.Count;i++){
				noteData nStatus = placedNoteList[i].GetComponent<noteData>();
				if (!nStatus.landmarkDynamic){
					nStatus.volume=volumeSet;
					nStatus.noteChange=true;
				}
				else{
					return false;
				}
			}
			return false;
		}

		float getPitch()
		{
			if (detector.Pitch > 1.0f){
				return detector.Pitch;
			}
			else{
				return 0f;
			}
		}
	}
}
