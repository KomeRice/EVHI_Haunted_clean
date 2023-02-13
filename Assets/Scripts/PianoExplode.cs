using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PianoExplode : MonoBehaviour
{
    [NonSerialized]
    public bool Exploded = false;
    public AudioClip explodeSound;
    private AudioSource _pianoAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        _pianoAudio = GetComponent<AudioSource>();
        _pianoAudio.Play();
    }

    public void ExplodePiano()
    {
        Exploded = true;
        _pianoAudio.loop = false;
        _pianoAudio.clip = explodeSound;
        _pianoAudio.Play();
        var cam = GameObject.FindWithTag("MainCamera");
        var pianoToPlayer = cam.transform.position - transform.position;
        var rng = new Random();
        for (var i = 0; i < transform.childCount; i++)
        {
            var pianoKey = transform.GetChild(i);
            var keyBody = pianoKey.GetComponent<Rigidbody>();
            var keyToPlayer = cam.transform.position - pianoKey.position;
            keyBody.useGravity = true;
            keyBody.isKinematic = false;
            var forceVector = new Vector3(keyToPlayer.x * rng.Next(20, 200) / 100,
                keyToPlayer.y * rng.Next(20, 150) / 100, keyToPlayer.z * rng.Next(20, 200) / 100) * 60;
            keyBody.AddForce(forceVector);
        }
        transform.GetComponent<Rigidbody>().useGravity = true;
        transform.GetComponent<Rigidbody>().isKinematic = false;
        transform.GetComponent<Rigidbody>().AddForce(pianoToPlayer);
    }
}
