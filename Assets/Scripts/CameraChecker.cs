using Unity.VisualScripting;
using UnityEngine;

public class CameraChecker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float cameraDistance;
    [SerializeField] private GameObject MiningButton;
    [SerializeField] private Animator pickaxeAnimation;
    [SerializeField] private float extractInterval;
    private GameObject oreFronOfYou;
    private float lastExtractTime;
    void Start()
    {
        lastExtractTime = Time.time;
        InvokeRepeating(nameof(OreChecker), 0.1f,0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
            WannaExtract();
    }
    public void WannaExtract()
    {
        if (oreFronOfYou != null && Time.time - lastExtractTime > extractInterval)
        {
            lastExtractTime = Time.time;
            Extract(oreFronOfYou);
        }
    }
    private void Extract(GameObject block)
    {
        pickaxeAnimation.SetTrigger("Extract");
        block.GetComponent<BlockLogic>().RegisterHit();

    }
    
    private void OreChecker()
    {
        oreFronOfYou = GetGameObjectFromRay(transform.position, transform.forward, cameraDistance, new string[] {"Gold","Coal"} );
        if (oreFronOfYou!=null && SDKLANG.IsMobileDevice)
        {
            MiningButton.SetActive(true);

        }
        else
        {
            MiningButton.SetActive(false);
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
