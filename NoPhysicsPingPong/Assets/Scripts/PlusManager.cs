using UnityEngine;

//Manager para features que deixam o jogo mais "juicy"
//Features controladas:
//##Particulas de explosão
//##Screenshake

//Essas features são controladas separadamente pois o projeto original
//era um clone bem próximo do pong original e eu queria que essas
//features extras fossem independentes e que pudessem ser desligadas
//sem precisar alterar scripts nos gameobjects;


public class PlusManager : MonoBehaviour
{
    public Animator cameraAnim;
    public ParticleSystem ballExplosion;

    public GameObject mobileTutorial;
    public GameObject consoleTutorial;

    void Awake()
    {
        #if UNITY_EDITOR
            mobileTutorial.SetActive(false);
            consoleTutorial.SetActive(true);
        #endif

        #if UNITY_STANDALONE
            mobileTutorial.SetActive(false);
            consoleTutorial.SetActive(true);
        #endif

        #if UNITY_IOS
            mobileTutorial.SetActive(true);
            consoleTutorial.SetActive(false);
        #endif

        #if UNITY_ANDROID
            mobileTutorial.SetActive(true);
            consoleTutorial.SetActive(false);
        #endif
    }

    void Start()
    {
        cameraAnim = Camera.main.GetComponent<Animator>();
        ballExplosion = transform.GetChild(0).transform.GetComponent<ParticleSystem>();
    }

    public void PlayerExplosion()
    {
        ballExplosion.Play();
    }

    public void PlayScreenshake()
    {
        cameraAnim.SetTrigger("Shake");
    }

    public void PhoneShake()
    {
        #if UNITY_ANDROID
                    Handheld.Vibrate();
        #endif

    }
}