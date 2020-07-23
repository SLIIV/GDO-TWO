using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public List<AudioBD> audios = new List<AudioBD>();
    private AudioSource musicSource;
    public int currentSong = 0;
    private AudioClip currentClip;
    public Sprite pauseImage;
    public Sprite playImage;
    public Button curImage;
    private bool isPaused = false;


    private void Awake()
    {
        for(int i = 0; i < audios.Count; i++)
        {
            if(audios[i].name == "")
            {
                SetTheName(i, audios[i].clip.name);
            }
        }
        musicSource = GetComponent<AudioSource>();
        RandomizeList();
        Play(currentSong);

    }
    private void Start()
    {
       
    }

    private void RandomizeList()
    {
        for (int i = audios.Count - 1; i >= 0; i--)
        {

            int j = Random.Range(0, i + 1);
            var temp = audios[j];
            audios[j] = audios[i];
            audios[i] = temp;
        }
    }

    public void NextSong()
    {
        if(currentSong + 1 < audios.Count)
        {
            currentSong++;
            Play(currentSong);
        }
        else
        {
            RandomizeList();
            currentSong = 0;
            Play(currentSong);
        }
    }
    public void PrevSong()
    {
        if(currentSong > 0)
        {
            currentSong--;
            Play(currentSong);
        }
        else
        {
            RandomizeList();
            currentSong = 0;
            Play(currentSong);
        }
    }

    private void Play(int songId)
    {
        currentClip = audios[songId].clip;
        musicSource.clip = currentClip;
        musicSource.Play();
    }

    private void FixedUpdate()
    {
        if(!musicSource.isPlaying && !isPaused)
        {
            NextSong();
        }
    }

    private void SetTheName(int songId, string name)
    {
        audios[songId].name = name;
    }
    

    public void Pause()
    {
        
        isPaused = !isPaused;
        if (isPaused)
        {
            curImage.image.sprite = pauseImage;
            musicSource.Pause();
        }
        else
        {
            curImage.image.sprite = playImage;
            musicSource.Play();
        }
    }


}
