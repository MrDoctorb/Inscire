using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Manager : MonoBehaviour
{
    AudioSource musicPlayer;
    [SerializeField] List<Song> songList = new List<Song>();
    Song currentSong;
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
        currentSong = songList[0];
        SetSongData();
    }

    void Update()
    {
        /*if(!musicPlayer.isPlaying)
        {
            SetSongData();
            musicPlayer.Play();
        }*/
    }

    void SetSongData()
    {
        musicPlayer.clip = currentSong.clip;
        musicPlayer.volume = currentSong.volume;
        musicPlayer.loop = currentSong.loop;
        musicPlayer.Play();
    }

    IEnumerator SongFade()
    {
        while (musicPlayer.volume > 0)
        {
            musicPlayer.volume -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        musicPlayer.Stop();
        SetSongData();
        musicPlayer.volume = 0;
        while(musicPlayer.volume < currentSong.volume)
        {
            musicPlayer.volume += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void ChangeSong(string songName)
    {
        bool found = false;
        if (songName != currentSong.name)
        {
            foreach (Song song in songList)
            {
                if (song.name == songName)
                {
                    found = true;
                    currentSong = song;
                    StartCoroutine(SongFade());
                    break;
                }
            }
            if (!found)
            {
                print("No song of name " + songName + " was found. Skipping song change.");
            }
        }
        else
        {
            print(songName + " is already playing.");
        }
    }
}

[System.Serializable]
public class Song
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1;
    public bool loop = true;
}
