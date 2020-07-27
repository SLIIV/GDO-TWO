using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    /// <summary>
    /// База данных всей музыки
    /// </summary>
    public List<AudioBD> audios = new List<AudioBD>();

    /// <summary>
    /// Источник музыки
    /// </summary>
    private AudioSource musicSource;

    /// <summary>
    /// Текущая композиция
    /// </summary>
    public int currentSong = 0;
    
    /// <summary>
    /// Текущий клип
    /// </summary>
    private AudioClip currentClip;

    /// <summary>
    /// Изображение кнопки паузы
    /// </summary>
    public Sprite pauseImage;
    
    /// <summary>
    /// Изображение кнопки проигрывания
    /// </summary>
    public Sprite playImage;

    /// <summary>
    /// Текущее изображение
    /// </summary>
    public Button curImage;

    /// <summary>
    /// Поставлена ли на паузу песня
    /// </summary>
    public bool isPaused = false;

    #region sounds

    /// <summary>
    /// Источник звуков
    /// </summary>
    public AudioSource soundsSource;

    /// <summary>
    /// Массив со звуками
    /// </summary>
    public AudioClip[] sounds;
    #endregion

    private void Awake()
    {

        //устанавливаем названия песен, если они отсутствуею
        for(int i = 0; i < audios.Count; i++)
        {
            if(audios[i].name == "")
            {
                SetTheName(i, audios[i].clip.name);
            }
        }

        //Получаем компонент с источником звука
        musicSource = GetComponent<AudioSource>();
        RandomizeList();
       

        string playerDocsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);//путь к Документам игрока
        if (System.IO.Directory.Exists(playerDocsFolder + "\\GDO-TWO\\UserSongs") == false)
           {
                System.IO.Directory.CreateDirectory(playerDocsFolder + "\\GDO-TWO\\UserSongs"); //создание папки
           }
        DirectoryInfo musicDir = new DirectoryInfo(playerDocsFolder + "/GDO-TWO/UserSongs"); // получение информации о файлах внутрти папки
        FileInfo[] userMusic = musicDir.GetFiles("*.ogg", SearchOption.TopDirectoryOnly);

        StartCoroutine(LoadTracks(userMusic, playerDocsFolder)); //загрузка треков
        
    }

    /// <summary>
    /// Загружает треки
    /// </summary>
    /// <param name="files">Массив с информацией о файлах</param>
    /// <param name="path">Путь к папке с музыкой</param>
    /// <returns></returns>
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


    /// <summary>
    /// Случайно заполняет музыкальный плейлист
    /// </summary>
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


    /// <summary>
    /// Переключение на следующую песню
    /// </summary>
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

    /// <summary>
    /// Предыдущая песня
    /// </summary>
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


    /// <summary>
    /// Проигрывание песни через её ID
    /// </summary>
    /// <param name="songId"></param>
    private void Play(int songId)
    {
        currentClip = audios[songId].clip;
        musicSource.clip = currentClip;
        musicSource.Play();
    }

    
    private void Update()
    {
        //Выключаем звуки, если игра на паузе
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
        //Если музыка закончилась, включаем следующую
        if(!musicSource.isPlaying && !isPaused && audios.Count > 0)
        {
            NextSong();
        }
    }

    /// <summary>
    /// Устанавливаем имя музыке
    /// </summary>
    /// <param name="songId">Айди музыки</param>
    /// <param name="name">Имя</param>
    private void SetTheName(int songId, string name)
    {
        audios[songId].name = name;
    }
    
    /// <summary>
    /// Ставит музыку на паузу
    /// </summary>
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
