using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject soundSourcePrefab;
    public static AudioManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void PlaySound(AudioClip sound, Vector3 position)
    {
        GameObject soundObj = Instantiate(soundSourcePrefab, position, Quaternion.identity);
        AudioSource audio = soundObj.GetComponent<AudioSource>();
        audio.clip = sound;
        audio.Play();
        Destroy(soundObj, sound.length);
    }

}
