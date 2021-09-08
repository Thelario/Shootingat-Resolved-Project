using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private float volume; // Volume of SFX

    private AudioSource source; // Private reference of the audioSource where we are going to play our SFX

    protected override void Awake()
    {
        base.Awake();

        source = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundType st)
    {
        source.PlayOneShot(SearchSound(st), volume * volume);
    }

    public void PlaySound(SoundType st, float newVolume)
    {
        source.PlayOneShot(SearchSound(st), volume * newVolume);
    }

    /// <summary>
    /// Looks through all the sounds in the array and returns the audioClip associated to the sound
    /// </summary>
    /// <param name="st"> It is the sound type of the sound we are looking for </param>
    /// <returns> Returns the AudioClip corresponding to the soundType passed as parameter </returns>
    private AudioClip SearchSound(SoundType st)
    {
        foreach (SoundAudioClip sac in Assets.Instance.soundAudioClipArray)
        {
            if (sac.sound == st)
                return sac.audioClip;
        }

        Debug.LogError("Sound Not Found");
        return null;
    }
}
