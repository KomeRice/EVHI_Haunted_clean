using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RandomSoundEvent : GameEvent
{
    public List<AudioClip> audioClips;
    private AudioSource _playerAudio;
    
    protected override void InitEvent()
    {
        Properties = new EventProperties("RandomSoundEvent", EventClass.Ambient, new List<string>(), -1);
        _playerAudio = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
    }

    public override bool CheckPrecondition()
    {
		var rng = new System.Random();
		if (rng.NextDouble() < 0.9)
			return false;
        return !_playerAudio.isPlaying;
    }

    public override void Trigger()
    {
        if (audioClips.Count == 0)
            return;
        var rng = new Random();
        var choice = rng.Next(audioClips.Count);
        _playerAudio.clip = audioClips[choice];
        _playerAudio.Play();
    }
}
