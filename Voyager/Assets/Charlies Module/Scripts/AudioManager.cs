using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;                                                        

public class AudioManager : MonoBehaviour 
{
    public static AudioManager instance = null;                                 

    public GameObject soundObjectPrefab;                                        

    public AudioClip clipOne;                                                   
    public AudioClip clipTwo;
    public AudioClip clipThree;
    public AudioClip clipFour;
    public AudioClip clipFive;
    public AudioClip clipSix;
    public AudioClip clipSeven;

    public AudioMixerGroup groupOne; // music                                           
    public AudioMixerGroup groupTwo; // SFX
    public AudioMixerGroup subgroupCollision;
    public AudioMixerGroup subgroupEngine;

    public AudioMixerSnapshot snapshotOne;                                      
    public AudioMixerSnapshot snapshotTwo;

    private void Awake()                                                        
    {
        if (instance == null)  
            instance = this;
        else
            Destroy(gameObject);                                                

        DontDestroyOnLoad(gameObject);                                          
    }

    void Start ()                                                               
    {
        Play("ambientMusic1", gameObject); //play ambient music from start
        Play("idleEngine1", gameObject);
        return;
	}
	
	void Update () 
    {
        return;
	}

    public void Play(string audioEvent, GameObject obj)                         
    {
        GameObject soundObject = (GameObject)Instantiate(soundObjectPrefab);    
        soundObject.transform.position = obj.transform.position;                
        AudioSource source = soundObject.GetComponent<AudioSource>();           

        switch (audioEvent)                                                     
        {
            case "asteroidCollision":
                source.clip = clipOne;                                          
                source.outputAudioMixerGroup = subgroupCollision;                        
                //Other stuff                                                    
                break;

            case "powerupCollision":
                source.clip = clipTwo;
                source.outputAudioMixerGroup = groupTwo;                        
                //Other stuff
                break;
            case "laserCollision":
                source.clip = clipThree;
                source.outputAudioMixerGroup = groupTwo;
                //Other stuff                                                    
                break;

            case "laserFire":
                source.clip = clipFour;
                source.outputAudioMixerGroup = groupTwo;
                //Other stuff
                break;
            case "ambientMusic1":
                source.clip = clipFive;
                source.outputAudioMixerGroup = groupOne;
                source.loop = true;
                //Other stuff
                break;
            case "ambientMusic2":
                source.clip = clipSix;
                source.outputAudioMixerGroup = groupOne;
                source.loop = true;
                //Other stuff
                break;
            case "idleEngine1":
                source.clip = clipSeven;
                source.outputAudioMixerGroup = subgroupEngine;
                //Other stuff
                break;
        }

        source.Play();                                                          
        Destroy(soundObject, source.clip.length);                               
    }
}

//Add this code to other scripts to call the play function:
//AudioManager.instance.Play("audioEventName", gameObject);