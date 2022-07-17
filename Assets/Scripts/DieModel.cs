using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieModel : MonoBehaviour
{
    public int value;
    public AudioClip rollSound;

    void Start() {
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().clip = rollSound;
    }

    void OnCollisionEnter() {
        if (!GetComponent<AudioSource>().isPlaying) {}
        GetComponent<AudioSource>().Play();
    }
}
