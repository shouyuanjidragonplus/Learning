using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Framework.Asset;
using UnityEngine;
public enum AudioType
{
    MUSIC,
    SOUND
}

public class AudioManager
{
    public static bool MusicClose
    {
        get { return Framework.Audio.AudioManager.Instance.MusicClose; }
        set { Framework.Audio.AudioManager.Instance.MusicClose = value; }
    }

    public static bool SoundClose
    {
        get { return Framework.Audio.AudioManager.Instance.SoundClose; }
        set { Framework.Audio.AudioManager.Instance.SoundClose = value; }
    }

    public static float PlayMusic(string musicName, bool loop = true)
    {
        AudioClip au = LoadAudio(AudioType.MUSIC, musicName);

        if (au != null)
        {
            Framework.Audio.AudioManager.Instance.PlayMusic(au, 1, loop, true);
            return au.length;
        }

        return -1;
    }

    public static float PlayMusic(int soundId, bool loop = false)
    {
        //string soundName = GlobalConfigManager.Instance.GetSoundName(soundId);
        string soundName = "";

        if (soundName == null || soundName == "")
            return -1;

        return PlayMusic(soundName, loop);
    }

    public static float PlaySound(int soundId, bool loop = false)
    {
        //string soundName = GlobalConfigManager.Instance.GetSoundName(soundId);
        string soundName = "";

        if (soundName == null || soundName == "")
            return -1;

        return PlaySound(soundName, loop);
    }
    public static int PlaySound(string soundName, float volume = 1)
    {
        try
        {
            AudioClip audio = LoadAudio(AudioType.SOUND, soundName);
            if (audio != null)
            {
                return Framework.Audio.AudioManager.Instance.PlaySound(audio);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"PlaySound:{soundName} error:{e.Message}");
        }

        return -1;
    }
    public static float PlaySound(string soundName, bool loop)
    {
        try
        {
            AudioClip audio = LoadAudio(AudioType.SOUND, soundName);
            if (audio != null)
            {
                if (Framework.Audio.AudioManager.Instance.PlaySound(audio, loop) > 0)
                    return audio.length;
            }

            return -1;
        }
        catch (Exception e)
        {
            Debug.LogError($"PlaySound:{soundName} error:{e.Message}");
        }

        return -1;
    }

    public void StopAllMusic()
    {
        Framework.Audio.AudioManager.Instance.StopAllMusic();
    }

    public void StopAllSound()
    {
        Framework.Audio.AudioManager.Instance.StopAllSounds();
    }

    public void PauseAllMusic()
    {
        Framework.Audio.AudioManager.Instance.PauseAllMusic();
    }

    public void ResumeAllMusic()
    {
        if (MusicClose)
            return;

        Framework.Audio.AudioManager.Instance.ResumeAllMusic();
    }

    private static AudioClip LoadAudio(AudioType type, string audioName)
    {
        string prefix = "BGM";
        switch (type)
        {
            case AudioType.MUSIC:
                prefix = "BGM";
                break;
            case AudioType.SOUND:
                prefix = "Sounds";
                break;
        }

        AudioClip _audio = ResourcesManager.Instance.LoadResource<AudioClip>(AudioPath(prefix, audioName));
        return _audio;
    }

    public static string AudioPath(string prefix, string audioName)
    {
        string[] audioArray = audioName.Split('/');
        if (audioArray.Length >= 2)
            return Path.Combine("Audios", audioArray[0], audioArray[1]);
        else
            return Path.Combine("Audios", prefix, audioName);
    }
}