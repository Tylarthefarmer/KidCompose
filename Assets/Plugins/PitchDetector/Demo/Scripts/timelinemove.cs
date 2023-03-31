
    using System.Collections;  
    using System.Collections.Generic;  
    using UnityEngine;  
      
    public class timelinemove : MonoBehaviour  
    {  
        Vector3 Vec;  
		//0=nothing, 1=recording, 2=playing
		public int status=1;
		public Vector3 spawnPos;
        // Start is called before the first frame update  
        void Start()  
        {  
              spawnPos = transform.position;
        }  
      
        // Update is called once per frame  
        void Update()  
        {  
			if (status != 0){
				Vec = transform.localPosition;  
				Vec.x += Time.deltaTime * 120;  
				transform.localPosition = Vec;  
			}
        }  
    }  
