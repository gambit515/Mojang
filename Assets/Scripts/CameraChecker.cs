using Unity.VisualScripting;
using UnityEngine;

public class CameraChecker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float cameraDistance;
    [SerializeField] private GameObject MiningButton;
    [SerializeField] private GameObject MiningImage;
    [SerializeField] private GameObject UseButton;
    [SerializeField] private GameObject UseImage;
    [SerializeField] private Animator pickaxeAnimation;
    [SerializeField] private float extractInterval;
    [SerializeField] private float useInterval;
    [SerializeField] private PicaxeScript picaxe;
    private GameObject oreFrontOfYou;
    private GameObject leverFrontOfYou;
    private float lastExtractTime;
    private float lastUseTime;
    void Start()
    {
        lastExtractTime = Time.time;
        InvokeRepeating(nameof(Checker), 0.1f,0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
            WannaExtract();
        if (Input.GetKeyUp(KeyCode.E))
            WannaUse();
    }
    public void WannaExtract()
    {
        if (oreFrontOfYou != null && Time.time - lastExtractTime > extractInterval)
        {
            lastExtractTime = Time.time;
            Extract(oreFrontOfYou);
        }
    }
    public void WannaUse()
    {
        if (leverFrontOfYou != null && Time.time - lastUseTime > useInterval)
        {
            lastUseTime = Time.time;
            Use(leverFrontOfYou);
        }
    }
    private void Extract(GameObject block)
    {
        pickaxeAnimation.SetTrigger("Extract");
        //picaxe.MoveCameraToPoint(block.transform.position);
        block.GetComponent<BlockLogic>().RegisterHit();

    }
    private void Use(GameObject block)
    {
        block.GetComponent<Lever>().Switch();
    }
    private void Checker()
    {
        oreFrontOfYou = GetGameObjectFromRay(transform.position, transform.forward, cameraDistance, new string[] {"Gold","Coal"} );
        if (oreFrontOfYou!=null)
        {
            if(SDKLANG.IsMobileDevice)
                MiningButton.SetActive(true);
            else
                MiningImage.SetActive(true);

        }
        else
        {
            MiningButton.SetActive(false);
            MiningImage.SetActive(false);
        }
        leverFrontOfYou = GetGameObjectFromRay(transform.position, transform.forward, cameraDistance, new string[] { "Lever" });
        if(leverFrontOfYou != null)
        {
            if (SDKLANG.IsMobileDevice)
                UseButton.SetActive(true);
            else
                UseImage.SetActive(true);
        }
        else
        {
            UseButton.SetActive(false);
            UseImage.SetActive(false);
        }
    }
    GameObject GetGameObjectFromRay(Vector3 orgin, Vector3 moveDirection, float maxDistance,string tag)
    {
        Ray ray = new(orgin, moveDirection);

        //Отображение вектора для отладки
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.blue);

        // Проверяем, есть ли коллизии на пути луча
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if(hit.collider.gameObject.tag.Equals(tag))
                return hit.collider.gameObject;
        }
        return null;
    }
    GameObject GetGameObjectFromRay(Vector3 orgin, Vector3 moveDirection, float maxDistance, string[] tags)
    {
        Ray ray = new(orgin, moveDirection);

        //Отображение вектора для отладки
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.blue);

        // Проверяем, есть ли коллизии на пути луча
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            foreach(string tag in tags)
                if (hit.collider.gameObject.tag.Equals(tag))
                    return hit.collider.gameObject;
        }
        return null;
    }
}
