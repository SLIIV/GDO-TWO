using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
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
    public bool isPaused = false;
    public UIManager UI;

    #region sounds
    public AudioSource soundsSource;
    public AudioClip[] sounds;

    public enum Sounds
    {
        Run = 0,
        Jump = 1,
        JumpComplete = 2,
        Moving = 3,
        Death = 4,
        TakeCoin = 5
    }
    #endregion

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
       

        string playerDocsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);//путь к Документам игрока
        if (System.IO.Directory.Exists(playerDocsFolder + "\\GDO-TWO\\UserSongs") == false)
           {
                System.IO.Directory.CreateDirectory(playerDocsFolder + "\\GDO-TWO\\UserSongs"); //создание папки
           }
        DirectoryInfo musicDir = new DirectoryInfo(playerDocsFolder + "/GDO-TWO/UserSongs");
        FileInfo[] userMusic = musicDir.GetFiles("*.ogg", SearchOption.TopDirectoryOnly);

        StartCoroutine(LoadTracks(userMusic, playerDocsFolder));
        
    }

    private IEnumerator LoadTracks(FileInfo[] files, string path)
    {
        for (int i = 0; i < files.Length; i++)
        {
            WWW www = new WWW("file:///" + path + "/GDO-TWO/UserSongs/" + files[i].Name);
            yield return www;
            AudioBD newAudio = new AudioBD();
            newAudio.name = Path.GetFileNameWithoutExtension(files[i].FullName);
            newAudio.clip = www.GetAudioClip(false, true, AudioType.OGGVORBIS);
            audios.Add(newAudio);



        }
    }
    private void Start()
    {
        
    }

    private void RandomizeList()
    {
        for (int i = audios.Count - 1; i >= 0; i--)
        {

            int j = UnityEngine.Random.Range(0, i + 1);
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


    private void Update()
    {
        if(Time.timeScale == 0)
        {
            soundsSource.enabled = false;
        }
        else
        {
            soundsSource.enabled = true;
        }
    }
    private void FixedUpdate()
    {
        
        if(!musicSource.isPlaying && !isPaused && audios.Count > 0)
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
