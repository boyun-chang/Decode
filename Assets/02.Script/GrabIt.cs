using UnityEngine;

namespace Lightbug.GrabIt
{
    [System.Serializable]
    public class GrabObjectProperties
    {
        
        public bool m_useGravity = false;
        public float m_drag = 10;
        public float m_angularDrag = 10;
        public RigidbodyConstraints m_constraints = RigidbodyConstraints.FreezeRotation;

    }

    public class GrabIt : MonoBehaviour
    {
        [Header("Grab properties")]

        [SerializeField]
        [Range(4, 50)]
        float m_grabSpeed = 7;

        [SerializeField]
        [Range(0.1f, 5)]
        float m_grabMinDistance = 1;

        [SerializeField]
        [Range(4, 25)]
        float m_grabMaxDistance = 10;

        [SerializeField]
        [Range(1, 10)]
        float m_scrollWheelSpeed = 5;
        
        [SerializeField]
        [Range(10, 50)]
        float m_impulseMagnitude = 25;


        [Header("Affected Rigidbody Properties")]
        [SerializeField] GrabObjectProperties m_grabProperties = new GrabObjectProperties();

        GrabObjectProperties m_defaultProperties = new GrabObjectProperties();

        [Header("Layers")]
        [SerializeField]
        LayerMask m_collisionMask;



        Rigidbody m_targetRB = null;
        Transform m_transform;

        Vector3 m_targetPos;
        GameObject m_hitPointObject;
        float m_targetDistance;

        bool m_grabbing = false;
        bool m_applyImpulse = false;
        bool m_isHingeJoint = false;

        //Debug
        LineRenderer m_lineRenderer;
        Item item;
        Item previous;

        void Awake()
        {
            m_transform = transform;
            m_hitPointObject = new GameObject("Point");

            m_lineRenderer = GetComponent<LineRenderer>();
        }


        void Update()
        {
            if (m_grabbing)
            {
                m_targetDistance += Input.GetAxisRaw("Mouse ScrollWheel") * m_scrollWheelSpeed;
                m_targetDistance = Mathf.Clamp(m_targetDistance, m_grabMinDistance, m_grabMaxDistance);

                m_targetPos = m_transform.position + m_transform.forward * m_targetDistance;

                if (!m_isHingeJoint)
                {
                    m_targetRB.constraints = m_grabProperties.m_constraints;
                }


                if (Input.GetMouseButtonUp(0))
                {
                    Reset();
                    m_grabbing = false;
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    m_applyImpulse = true;
                }
            }
            else
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(m_transform.position, m_transform.forward, out hitInfo, m_grabMaxDistance /*, m_collisionMask */))
                {
                    if (hitInfo.collider.gameObject.GetComponent<Item>() != null) // 들어온 item 있는 경우
                    {
                        item = hitInfo.transform.GetComponent<Item>(); // 아이템 스크립트 가져오기
                        if (previous != null && item != previous) // 이전 아이템이 있고, 같지 않으면
                        {
                            previous.gameObject.GetComponent<MeshRenderer>().material = previous.preMat;
                            previous = null;
                        }
                        else if (previous != null) // 이전아이템이 있으면
                        {
                            previous.gameObject.GetComponent<MeshRenderer>().material = previous.preMat;
                        }
                        else // 이전 아이템이 없으면
                        {
                            previous = item;
                        }
                        // 그냥 new item
                        item.gameObject.GetComponent<MeshRenderer>().material = item.Mat;
                        
                    }
                    else if (item != null && hitInfo.collider.gameObject.GetComponent<Item>() == null) // item 없는 경우
                    {
                        item.gameObject.GetComponent<MeshRenderer>().material = item.preMat;
                        previous = null;
                    }
                    
                    if (Input.GetMouseButtonDown(0))
                    {
                        Rigidbody rb = hitInfo.collider.GetComponent<Rigidbody>();
                        if (rb != null && !rb.gameObject.CompareTag("Player")) // 일반 뮬체
                        {
                            Set(rb, hitInfo.distance);
                            m_grabbing = true;
                        }
                    }
                }// 05.11 yb lever 움직이는 애니메이션 실행 코드 this -> nodeClick으로 가져감.
            }
        }
        
        void Set(Rigidbody target, float distance)
        {
            m_targetRB = target;
            m_isHingeJoint = target.GetComponent<HingeJoint>() != null;

            //Rigidbody default properties	
            m_defaultProperties.m_useGravity = m_targetRB.useGravity;
            m_defaultProperties.m_drag = m_targetRB.drag;
            m_defaultProperties.m_angularDrag = m_targetRB.angularDrag;
            m_defaultProperties.m_constraints = m_targetRB.constraints;

            //Grab Properties	
            m_targetRB.useGravity = m_grabProperties.m_useGravity;
            m_targetRB.drag = m_grabProperties.m_drag;
            m_targetRB.angularDrag = m_grabProperties.m_angularDrag;
            m_targetRB.constraints = m_isHingeJoint ? RigidbodyConstraints.None : m_grabProperties.m_constraints;


            m_hitPointObject.transform.SetParent(target.transform);

            m_targetDistance = distance;
            m_targetPos = m_transform.position + m_transform.forward * m_targetDistance;

            m_hitPointObject.transform.position = m_targetPos;
            m_hitPointObject.transform.LookAt(m_transform);

        }

        void Reset()
        {
            //Grab Properties	
            m_targetRB.useGravity = m_defaultProperties.m_useGravity;
            m_targetRB.drag = m_defaultProperties.m_drag;
            m_targetRB.angularDrag = m_defaultProperties.m_angularDrag;
            m_targetRB.constraints = m_defaultProperties.m_constraints;

            m_targetRB = null;

            m_hitPointObject.transform.SetParent(null);

            if (m_lineRenderer != null)
                m_lineRenderer.enabled = false;
        }

        void Grab()
        {
            Vector3 hitPointPos = m_hitPointObject.transform.position;
            Vector3 dif = m_targetPos - hitPointPos;

            if (m_isHingeJoint)
                m_targetRB.AddForceAtPosition(m_grabSpeed * dif * 100, hitPointPos, ForceMode.Force);
            else
                m_targetRB.velocity = m_grabSpeed * dif;


            if (m_lineRenderer != null)
            {
                m_lineRenderer.enabled = true;
                m_lineRenderer.SetPositions(new Vector3[] { m_targetPos, hitPointPos });
            }
        }
        
        void FixedUpdate()
        {
            if (item != null && item.isItem && m_targetRB != null) // 아이템 스크립트가 있는지 확인하고 잡고있는 물체가 아이템일 경우
            {
                m_targetRB.isKinematic = true; // 아이템 고정
                m_lineRenderer.enabled = false;
                return;
            }

            if (!m_grabbing)
                return;

            Grab();

            if (m_applyImpulse)
            {
                m_targetRB.velocity = m_transform.forward * m_impulseMagnitude;
                Reset();
                m_grabbing = false;
                m_applyImpulse = false;
            }

        }

    }

}
