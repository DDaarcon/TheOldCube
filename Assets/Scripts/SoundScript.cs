using UnityEngine;
using UnityEngine.UI;
using static Enums;

[System.Serializable]
public class SoundScript : MonoBehaviour
{
    public AudioSource workspaceAudio;
    public AudioClip themeSong;

    [SerializeField]
    public AudioClip[] diceSounds;
    public float diceSoundVolume = 0.5f;
    [Space(5)]

    [SerializeField]
    public AudioClip cubeVibrateSound;
    public float cubeVibrateVolume = 0.5f;
    [Space(5)]

    [SerializeField]
    public AudioClip placeSideSound;
    public float placeSideVolume = 0.5f;
    [Space(5)]

    [SerializeField]
    public AudioClip destroySideSound;
    public float destroySideVolume = 0.5f;
    [Space(5)]

    [SerializeField]
    public AudioClip choosedSideSound;
    public float choosedSideVolume = 0.5f;
    [Space(5)]

    [SerializeField]
    public AudioClip wrongPlaceSound;
    public float wrongPlaceVolume = 1f;
    [Space(5)]

    [SerializeField]
    public AudioClip cancelPlacingSound;
    public float cancelPlacingVolume = 0.5f;
    [Space(5)]

    [SerializeField]
    public AudioClip cubeDecaySound;
    public float cubeDecayVolume = 0.5f;
    [Space(5)]

    [SerializeField]
    public AudioClip pieceRotationSound;
    public float pieceRotationVolume = 0.5f;
    private float delayTimer;
    private float delayTime = 0f;
    [Space(5)]

    [SerializeField]
    public AudioClip menuMoveSound;
    public float menuMoveVolume = 0.5f;
    [Space(5)]

    [SerializeField]
    public AudioClip rightMenuMoveSound;
    public float rightMenuMoveVolume = 0.5f;
    [Space(5)]

    private AudioSource audioSource;
    private const string MUTE_INFO = "muteInfo";

    public bool mute = false;
    public Image icon_P;
    public Image icon_L;
    public Sprite muteIcon;
    public Sprite unmuteIcon;

    public void PlayRandomDiceSound() {
        if (!mute) audioSource.PlayOneShot(diceSounds[Random.Range(0, diceSounds.Length)], diceSoundVolume);
    }
    public void PlayCubeVibrateSound() {
        if (!mute) workspaceAudio.PlayOneShot(cubeVibrateSound, cubeVibrateVolume);
    }
    public void PlayPlaceSideSound() {
        if (!mute) workspaceAudio.PlayOneShot(placeSideSound, placeSideVolume);
    }
    public void PlayDestroySideSound() {
        if (!mute) workspaceAudio.PlayOneShot(destroySideSound, destroySideVolume);
    }
    public void PlayChoosedSideSound() {
        if (!mute) workspaceAudio.PlayOneShot(choosedSideSound, choosedSideVolume);
    }
    public void PlayWrongPlaceSound() {
        if (!mute) audioSource.PlayOneShot(wrongPlaceSound, wrongPlaceVolume);
    }
    public void PlayCancelPlacingSound() {
        if (!mute) audioSource.PlayOneShot(cancelPlacingSound, cancelPlacingVolume);
    }
    public void PlayCubeDecaySound() {
        if (!mute) audioSource.PlayOneShot(cubeDecaySound, cubeDecayVolume);
    }
    public void PlayPieceRotationSound(float delayTime = 0f) {
        if (delayTime <= 0f) {
            if (!mute) workspaceAudio.PlayOneShot(pieceRotationSound, pieceRotationVolume);
        } else {
            this.delayTimer = 0f;
            this.delayTime = delayTime;
        }
    }
    public void PlayMenuMoveSound() {
        if (!mute) audioSource.PlayOneShot(menuMoveSound, menuMoveVolume);
    }
    public void PlayRightMenuMoveSound() {
        if (!mute) audioSource.PlayOneShot(rightMenuMoveSound, rightMenuMoveVolume);
    }

    public void ToggleMute() {
        mute = !mute;
        if (mute) {
            icon_L.sprite = muteIcon;
            icon_P.sprite = muteIcon;
            audioSource.volume = 0f;
        } else {
            icon_L.sprite = unmuteIcon;
            icon_P.sprite = unmuteIcon;
            audioSource.volume = 0.6f;
        }
        MuteInPlayerPrefs(get: false);
    }

    private void Awake() {
        mute = MuteInPlayerPrefs(get: true);
        audioSource = GetComponent<AudioSource>();
        if (mute) {
            icon_L.sprite = muteIcon;
            icon_P.sprite = muteIcon;
        } else {
            icon_L.sprite = unmuteIcon;
            icon_P.sprite = unmuteIcon;
        }
        audioSource.clip = themeSong;
        audioSource.volume = mute ? 0f : 0.6f;
        audioSource.Play();
        audioSource.loop = true;

    }

    private bool MuteInPlayerPrefs(bool get) {
        if (get) {
            if (PlayerPrefs.HasKey(MUTE_INFO)) {
                return IntToBool(PlayerPrefs.GetInt(MUTE_INFO));
            } else {
                return mute;
            }
        } else {
            PlayerPrefs.SetInt(MUTE_INFO, BoolToInt(mute));
            return mute;
        }
    }


    public void Update() {
        if (delayTime != 0f) {
            if (delayTimer >= delayTime) {
                delayTime = 0f;
                PlayPieceRotationSound();
            } else {
                delayTimer += Time.deltaTime;
            }
        }
    }
}
