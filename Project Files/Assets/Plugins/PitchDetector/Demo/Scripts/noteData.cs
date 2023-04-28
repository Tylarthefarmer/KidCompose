using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteData : MonoBehaviour
{
	public int notePitch;
	AudioSource playNote;
	public AudioClip[] notes; 
	public int status;
	public GameObject dynamicModel;
	public bool noteChange=false;
	public bool quarterNote=false;
	//at start, everyone gets a dynamic, it's just hidden by default. Only show landmark dynamics'
	GameObject dynamicMarking;
	//-1 is unassigned, 0=p, 1=mp, 2=mf, 3=f
	public int volume = 2;
	//denotes whether this note is assigned a dynamic specifically. All notes afterwards will follow that dynamic
	public bool landmarkDynamic = false;
    // Start is called before the first frame update
    void Start()
    {
        playNote = GetComponent<AudioSource>();
		//set to play a certain note depending on what notePitch is set to
		//select note based on the notePitch
		this.GetComponent<AudioSource>().clip = notes[notePitch+1];
		if (notePitch<0){
			//if rest, make it look like a rest
			this.transform.GetChild(0).GetComponent<Renderer>().enabled=false;
			this.transform.GetChild(1).GetComponent<Renderer>().enabled=true;
		}
		Renderer n1 = this.transform.GetChild(0).GetComponent<Renderer>();
		n1.material.color=Color.black;
		Renderer n2 = this.transform.GetChild(1).GetComponent<Renderer>();
		n2.material.color=Color.black;
		Renderer n3 = this.transform.GetChild(2).GetComponent<Renderer>();
		n3.material.color=Color.black;
		Renderer n4 = this.transform.GetChild(3).GetComponent<Renderer>();
		n4.material.color=Color.black;
		Renderer n5 = this.transform.GetChild(4).GetComponent<Renderer>();
		n5.material.color=Color.black;

		//set volume and create dynamic
		dynamicMarking = Instantiate(dynamicModel,new Vector3(transform.position.x,-150,transform.position.z),Quaternion.identity);
		dynamicMarking.transform.GetChild(0).GetComponent<Renderer>().enabled=false;
		dynamicMarking.transform.GetChild(1).GetComponent<Renderer>().enabled=false;
		dynamicMarking.transform.GetChild(2).GetComponent<Renderer>().enabled=false;
		dynamicMarking.transform.GetChild(3).GetComponent<Renderer>().enabled=false;
		if (notePitch>0){
			if (volume==0){
				playNote.volume=0.25f;
				if (landmarkDynamic){
					dynamicMarking.transform.GetChild(3).GetComponent<Renderer>().enabled=true;
				}
			}
			else if (volume==1){
				playNote.volume=0.5f;
				if (landmarkDynamic){
					dynamicMarking.transform.GetChild(2).GetComponent<Renderer>().enabled=true;
				}
			}
			else if (volume==2){
				playNote.volume=0.75f;
				if (landmarkDynamic){
					dynamicMarking.transform.GetChild(1).GetComponent<Renderer>().enabled=true;
				}
			}
			else if (volume==3){
				playNote.volume=1.0f;
				if (landmarkDynamic){
					dynamicMarking.transform.GetChild(0).GetComponent<Renderer>().enabled=true;
				}
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
		//on note update, run start script and play the note
		if (noteChange){
			updateNote(notePitch, volume);
		}
    }

	void OnTriggerEnter(Collider other){
		if (status == 2){
			if (notePitch>0){
				//play note
				playNote.mute=false;
				//Debug.Log(notePitch);
				playNote.Play(0);
			} 		
		}
	}

	void updateNote(int notePitch, int volume){
		this.GetComponent<AudioSource>().clip = notes[notePitch+1];
		dynamicMarking.transform.GetChild(0).GetComponent<Renderer>().enabled=false;
		dynamicMarking.transform.GetChild(1).GetComponent<Renderer>().enabled=false;
		dynamicMarking.transform.GetChild(2).GetComponent<Renderer>().enabled=false;
		dynamicMarking.transform.GetChild(3).GetComponent<Renderer>().enabled=false;
		if (notePitch>0){
			if (volume==0){
				playNote.volume=0.25f;
				if (landmarkDynamic){
					dynamicMarking.transform.GetChild(3).GetComponent<Renderer>().enabled=true;
				}
			}
			else if (volume==1){
				playNote.volume=0.5f;
				if (landmarkDynamic){
					dynamicMarking.transform.GetChild(2).GetComponent<Renderer>().enabled=true;
				}
			}
			else if (volume==2){
				playNote.volume=0.75f;
				if (landmarkDynamic){
					dynamicMarking.transform.GetChild(1).GetComponent<Renderer>().enabled=true;
				}
			}
			else if (volume==3){
				playNote.volume=1.0f;
				if (landmarkDynamic){
					dynamicMarking.transform.GetChild(0).GetComponent<Renderer>().enabled=true;
				}
			}
		}

		this.transform.GetChild(0).GetComponent<Renderer>().enabled=false;
		this.transform.GetChild(1).GetComponent<Renderer>().enabled=false;
		this.transform.GetChild(2).GetComponent<Renderer>().enabled=false;
		this.transform.GetChild(3).GetComponent<Renderer>().enabled=false;
		if (notePitch<0){
			//if rest, make it look like a rest
			if (quarterNote){
				this.transform.GetChild(3).GetComponent<Renderer>().enabled=true;
			}
			else if(!quarterNote){
				this.transform.GetChild(1).GetComponent<Renderer>().enabled=true;
			}
		}
		else if (notePitch>=0){
			//if note, make it look like a rest
			if (quarterNote){
				this.transform.GetChild(2).GetComponent<Renderer>().enabled=true;
			}
			else if(!quarterNote){
				this.transform.GetChild(0).GetComponent<Renderer>().enabled=true;
			}
		}
	}
}
