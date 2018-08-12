using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AudioManager : Singleton<AudioManager> {
    
    

	public class SE
    {
        
        public string name = "New SE";
        public AudioClip se;
        [Range(0f, 3f)]
        public float pitch = 1f;
        [Range(0f, 1f)]
        public float volume = 1f;

        [HideInInspector]
        public AudioSource source;
    }

    public class BGM
    {
        
        public string name = "New BGM";
        public AudioClip bgm;
        [Range(0f,3f)]
        public float pitch = 1f;
        [Range(0f, 1f)]
        public float volume = 1f;

        [HideInInspector]
        public AudioSource source;
    }

    public List<SE> SEs = new List<SE>();
    public List<BGM> BGMs = new List<BGM>();
    

    private void Start()
    {
        foreach (var se in SEs)
        {
            se.source = gameObject.AddComponent<AudioSource>();
            se.source.clip = se.se;
            se.source.pitch = se.pitch;
            se.source.volume = se.volume;
        }
        foreach (var bgm in BGMs)
        { 
            bgm.source = gameObject.AddComponent<AudioSource>();
            bgm.source.clip = bgm.bgm;
            bgm.source.pitch = bgm.pitch;
            bgm.source.volume = bgm.volume;
        }
    }

    public void PlaySE(string name)
    {
        bool find = false;
        foreach (var s in SEs)
        {
            if (s.name == name)
            {
                s.source.Play();
            }
        }
        if (!find)
        {
            Debug.LogWarning("SE " + name + " was no found!!");
        }
    }
    public void PlayBGM(string name ,bool loop=true)
    {
        bool find = false;
        foreach (var bgm in BGMs)
        {
            if (bgm.name == name)
            {
                bgm.source.loop = loop;
                bgm.source.Play();
            }
        }
        if (!find)
        {
            Debug.LogWarning("BGM " + name + " was no found!!");
        }
    }
    public void StopBGM(string name)
    {
        bool find = false;
        foreach (var s in SEs)
        {
            if (s.name == name)
            {
                s.source.Stop();
                s.source.loop = false;
            }
        }
        if (!find)
        {
            Debug.LogWarning("BGM " + name + " was no found!!");
        }
    }

    public void FadeInBGM(string name,int duration,bool loop = true)
    {
        bool find = false;
        foreach (var s in BGMs)
        {
            if (s.name == name)
            {
                StartCoroutine(FadeIn(s.source, duration));
                s.source.loop = loop;
                s.source.Play();
                find = true;
            }
        }
        if (!find)
        {
            Debug.LogWarning("BGM " + name + " was no found!!");
        }
    }

    public void FadeOutBGM(string name, int duration)
    {
        bool find = false;
        foreach (var bgm in BGMs)
        {
            if (bgm.name == name)
            {
                StartCoroutine(FadeOut(bgm.source, duration));
            }
        }
        if (!find)
        {
            Debug.LogWarning("BGM " + name + " was no found!!");
        }
    }

    IEnumerator FadeIn(AudioSource source,int duration)
    {
        //StopAllCoroutines();
        float tmpVolume = 0f;
        float fullVolume = source.volume;

        for (float i = 0; i <= duration; i++)
        {
            tmpVolume = (i / duration) * fullVolume;
            source.volume = tmpVolume;
            yield return new WaitForEndOfFrame();
        }

    }

    IEnumerator FadeOut(AudioSource source, int duration)
    {
        //StopAllCoroutines();
        float tmpVolume = 0f;
        float fullVolume = source.volume;
        for (float i = duration; i >=0; i--)
        {
            tmpVolume = (i / duration) * fullVolume;
            source.volume = tmpVolume;
            yield return new WaitForEndOfFrame();
        }
        source.Stop();
        source.loop = false;
    }


}
