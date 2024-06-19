using UnityEngine;

public class AudioManager1 : MonoBehaviour
{
    //ComponentReferences
    //Params
    //Temps
    //Public
    public static AudioManager1 Instance {get; private set; }
     
    private void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }
    
    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}