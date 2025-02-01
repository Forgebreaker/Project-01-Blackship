using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicDDOL : MonoBehaviour
{
    private static MusicDDOL instance;

    [SerializeField] private AudioClip lobbyMusic;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SetupAudioSource();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Easy Challenge [1]" || scene.name == "Easy Challenge [2]" ||
            scene.name == "Medium Challenge [1]" || scene.name == "Medium Challenge [2]" ||
            scene.name == "Hard Challenge [1]" || scene.name == "Hard Challenge [2]")
        {
            Destroy(gameObject);
        }
        else if (scene.name == "Main Lobby" && instance == null)
        {
            RecreateMusicManager();
        }
    }

    private void RecreateMusicManager()
    {
        GameObject newManager = new GameObject("MusicDDOL");
        var newManagerScript = newManager.AddComponent<MusicDDOL>();

        newManagerScript.lobbyMusic = lobbyMusic;

        newManagerScript.SetupAudioSource();

        DontDestroyOnLoad(newManager);
    }

    private void SetupAudioSource()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = lobbyMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = 0.5f; 
        audioSource.Play();
    }
}
