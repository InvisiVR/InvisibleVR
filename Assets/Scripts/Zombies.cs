using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Zombies : MonoBehaviour
{
    NavMeshAgent agent;
    private Animator anim;
    [SerializeField] private Transform target;
    [SerializeField] HandGun handGun;
    [SerializeField] FootstepsSound footStep;

    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private GameObject jumpscareCam;
    [SerializeField] private GameObject bloodIMGobj;
    [SerializeField] private Image bloodIMG;
    [SerializeField] private GameObject FadeOutBlack;

    // Sounds
    [SerializeField] private GameObject jumpscareSound;
    [SerializeField] private GameObject HeartBeatSound;
    private AudioSource heartbeat;
    [SerializeField] private GameObject ZombieSound;

    // Raycast
    private float ray_dist = 10.0f; // Raycast Distance
    private Ray[] rays = {
        new Ray(),
        new Ray(),
        new Ray(),
        new Ray(),
        new Ray(),
        new Ray(),
        new Ray()
    };
    private RaycastHit hit1, hit2, hit3, hit4, hit5, hit6, hit7; // Racast Hits
    private Vector3 layPos;

    private float player_zombie_dist;
    private bool isZombieDie;
    private bool isPlayerCatched;

    [SerializeField] private int cur_mode = 0;
    private float hearing_dist = 7.0f;
    private float mustChase_dist = 4.0f;

    private float mode2delayMAXtime = 5.0f;
    private float mode2delayCURtime = 0.0f;

    // Zombie HP
    public float hp;

    // Weights
    public float hpWeight = 1.0f;
    public float speedWeight = 1.0f;

    private Vector3 curPatrolSpot;
    private Vector3[] patrolSpot =
    {
        // 3F Spots
        new Vector3(-18f, 9f, 26f),

        // 2F Spots
        new Vector3(-19f, 5f, 23f),
        new Vector3(1f, 5f, 24f),
        new Vector3(-8f, 5f, 30f),

        // 1F Spots
        new Vector3(-19f, 1f, 23f),
        new Vector3(-12f, 1f, 37f),
        new Vector3(28f, 1f, 31f),
        new Vector3(5f, 1f, 23f),
        new Vector3(-8f, 1f, 11f)

        /* Total 9 Spots */
    };

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        heartbeat = HeartBeatSound.GetComponent<AudioSource>();
        curPatrolSpot = patrolSpot[Random.Range(0, 9)];
        agent.speed = 1.0f * speedWeight;
        hp = 10.0f * hpWeight;
        isPlayerCatched = false;
        isZombieDie = false;
    }

    private void FixedUpdate()
    {
        // Distance Zombie-Player
        player_zombie_dist = Vector3.Distance(this.transform.position, target.position);

        // HeartBeatSound Pitch
        heartbeat.pitch = 1.0f + (2.0f / player_zombie_dist);

        // 플레이어 숙련도 가중치 업데이트 (0.0f <= weight <= 1.0f)
        float weight = GameObject.Find("GameManager").GetComponent<GameManager>().normalizedValue;
        hpWeight = 1 + weight;
        speedWeight = 1 + weight;

        // 좀비가 죽었을 때
        if (hp < 0)
        {
            cur_mode = 3;
        }
        // 플레이어를 잡았을 때
        else if (!isPlayerCatched && player_zombie_dist < 1.0f && !isZombieDie)
        {
            cur_mode = 4;
        }
        // 이미 플레이어를 잡은 상태에서, Jumpscare 효과 반복 재생
        else if (isPlayerCatched)
        {
            JumpScareCamEff();
        }

        // 현재 모드 별 행동 함수 반복 호출
        BehaviorByMode(cur_mode);
    }

    private void BehaviorByMode(int mode)
    {
        /* *** 현재 MODE별 행동 명령 ***
         * MODE 0 : 순찰 모드
         * MODE 1 : 소리난 위치 탐색
         * MODE 2 : 추격 모드
         * MODE 3 : 좀비 사망
         * MODE 4 : 플레이어를 잡음      */
        switch (mode)
        {
            /******************** 0 : Patrol Mode ********************/
            case 0:
                anim.SetInteger("mode", 0);
                agent.SetDestination(curPatrolSpot);

                // 길이 10.0f인 Ray를 통해 플레이어 발견 --> Mode 2 (Chase Mode)
                if (FindingPlayerForRay())
                {
                    cur_mode = 2;
                    agent.speed = 1.8f * speedWeight;

                    return;
                }

                // 범위 7.0f 내에서 플레이어 소리 감지 --> Mode 1 (Go to the sound location)
                if (player_zombie_dist < hearing_dist)
                {
                    if (HearingSound())
                    {
                        cur_mode = 1;

                        return;
                    }
                }

                // 현재 탐색 지점에 도달할 때까지 플레이어 발견 못하면, 다음 탐색 지점 설정.
                if (Vector3.Distance(transform.position, curPatrolSpot) < 2.0f)
                {
                    curPatrolSpot = patrolSpot[Random.Range(0, 9)];
                }

                break;
            /***********************************************************/


            /******************** 1 : Go to the sound location ********************/
            case 1:
                anim.SetInteger("mode", 0);
                agent.SetDestination(curPatrolSpot);

                // 길이 10.0f인 Ray를 통해 플레이어 발견 --> Mode 2 (Chase Mode)
                if (FindingPlayerForRay())
                {
                    cur_mode = 2;
                    agent.speed = 1.8f * speedWeight;

                    return;
                }

                // 범위 7.0f 내에서 또 다른 플레이어 소리 감지 시, 목표 지점 및 속도 변경
                if (player_zombie_dist < hearing_dist)
                {
                    if (HearingSound()) return;
                }

                // 소리 난 지점 탐색 종료 시, MODE 0으로 돌아간 후 다음 탐색 지점 설정.
                if (Vector3.Distance(transform.position, curPatrolSpot) < 2.0f)
                {
                    cur_mode = 0;
                    curPatrolSpot = patrolSpot[Random.Range(0, 9)];
                    agent.speed = 1.0f * speedWeight;
                }
                break;
            /*************************************************************************/


            /******************** 2 : Chase Mode ********************/
            case 2:
                anim.SetInteger("mode", 1);
                agent.SetDestination(target.position);

                // 플레이어 총알이 없는 상태라면, 속도 증가
                if (handGun.magazine.bulletNum == 0) agent.speed = 2.2f * speedWeight;
                else agent.speed = 1.8f * speedWeight;

                // 길이 10.0f인 Ray를 통해 플레이어 발견 시, 추격 모드 지속
                if (FindingPlayerForRay())
                {
                    mode2delayCURtime = 0.0f;
                    return;
                }
                // 발견하지 못하는 상태에서, 플레이어가 5초간 좀비 추격 범위에서 벗어남 --> MODE 0 (Patrol Mode)
                else CountToMode2Off();

                break;
            /*********************************************************/


            /******************** 3 : Zombie is dead ********************/
            case 3:
                anim.SetInteger("mode", 3);
                agent.enabled = false;
                isZombieDie = true;

                // 해당 모드 유지 위해, 변수들에 쓰레기값 할당
                cur_mode = 99999;
                hp = 99999.0f;

                StartCoroutine(ZombieDie());

                break;
            /************************************************************/


            /******************** 4 : Catched Player ********************/
            case 4:
                anim.SetInteger("mode", 4);
                agent.enabled = false;

                isPlayerCatched = true;
                StartJumpScare();
                StartCoroutine(FadeOutStart());

                break;
            /************************************************************/
        }
    }

    /* * 디버깅용 Ray Line 그리기 함수 *
    private void DebugLayCastLine()
    {
        Color cur_color;
        if (cur_mode == 2) cur_color = Color.red;
        else cur_color = Color.green;

        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * -0.3f).normalized * hit1.distance, cur_color);
        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * -0.2f).normalized * hit2.distance, cur_color);
        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * -0.1f).normalized * hit3.distance, cur_color);
        Debug.DrawLine(layPos, layPos + transform.forward * hit4.distance, cur_color);
        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * 0.1f).normalized * hit5.distance, cur_color);
        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * 0.2f).normalized * hit6.distance, cur_color);
        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * 0.3f).normalized * hit7.distance, cur_color);
    }
    */

    /******************** Ray를 기반으로, 플레이어 발견 여부를 반환하는 함수 ********************/
    private bool FindingPlayerForRay()
    {
        layPos = transform.position + new Vector3(0, 1.3f, 0);

        rays[0] = new Ray(layPos, (transform.forward + transform.right * -0.3f).normalized);
        rays[1] = new Ray(layPos, (transform.forward + transform.right * -0.2f).normalized);
        rays[2] = new Ray(layPos, (transform.forward + transform.right * -0.1f).normalized);
        rays[3] = new Ray(layPos, transform.forward);
        rays[4] = new Ray(layPos, (transform.forward + transform.right * 0.1f).normalized);
        rays[5] = new Ray(layPos, (transform.forward + transform.right * 0.2f).normalized);
        rays[6] = new Ray(layPos, (transform.forward + transform.right * 0.3f).normalized);

        if (Physics.Raycast(rays[0], out hit1, ray_dist) && hit1.collider.gameObject.tag == "Player") return true;
        if (Physics.Raycast(rays[1], out hit2, ray_dist) && hit2.collider.gameObject.tag == "Player") return true;
        if (Physics.Raycast(rays[2], out hit3, ray_dist) && hit3.collider.gameObject.tag == "Player") return true;
        if (Physics.Raycast(rays[3], out hit4, ray_dist) && hit4.collider.gameObject.tag == "Player") return true;
        if (Physics.Raycast(rays[4], out hit5, ray_dist) && hit5.collider.gameObject.tag == "Player") return true;
        if (Physics.Raycast(rays[5], out hit6, ray_dist) && hit6.collider.gameObject.tag == "Player") return true;
        if (Physics.Raycast(rays[6], out hit7, ray_dist) && hit7.collider.gameObject.tag == "Player") return true;

        return false;

        /* < 반환값 : bool >
         * true  : 플레이어를 감지함
         * false : 플레이어 감지 못함 */
    }
    /********************************************************************************/


    /******************** 플레이어 소리 감지 시, 목표 지점 및 속도 변경 함수  ********************/
    private bool HearingSound()
    {
        // if 'Gun Fire Sound' Heard -> spd = 1.6f
        if (handGun.fiered)
        {
            curPatrolSpot = target.position;
            agent.speed = 1.6f * speedWeight;
            return true;
        }

        // if 'Footstep Sound' Heard -> spd = 1.4f
        if (footStep.isFootSoundPlaying)
        {
            curPatrolSpot = target.position;
            agent.speed = 1.4f * speedWeight;
            return true;
        }

        return false;

        /* < 반환값 : bool >
         * true  : 소리를 감지함
         * false : 소리 감지 못함 */
    }
    /*********************************************************************************/


    /******************** 추격 범위에서 벗어난 채로 5초가 되면 MODE 0으로 되돌리는 함수  ********************/
    private void CountToMode2Off()
    {
        if (player_zombie_dist > mustChase_dist) mode2delayCURtime += Time.deltaTime;
        else {
            mode2delayCURtime = 0.0f;
            return;
        }

        // Ray와 추격 범위 모두 벗어난 채로 5초가 되면, MODE 0으로 되돌아감
        if (mode2delayCURtime > mode2delayMAXtime)
        {
            cur_mode = 0;
            agent.speed = 1.0f * speedWeight;
            mode2delayCURtime = 0.0f;

            return;
        }
        else return;
    }
    /*********************************************************************************************/


    /******************** 좀비가 죽었을 때의 실행 함수 ********************/
    private IEnumerator ZombieDie()
    {
        // 효과음 비활성화
        HeartBeatSound.SetActive(false);
        ZombieSound.SetActive(false);

        // 30초 대기 후, 리스폰(MODE 0인 상태로). 탐색 기능, 효과음 활성화
        yield return new WaitForSeconds(30.0f);

        isZombieDie = false;
        agent.speed = 1.0f * speedWeight;
        transform.position = patrolSpot[0];
        curPatrolSpot = patrolSpot[Random.Range(0, 9)];
        cur_mode = 0;
        hp = 10.0f * hpWeight;

        HeartBeatSound.SetActive(true);
        ZombieSound.SetActive(true);

        agent.enabled = true;
    }
    /***************************************************************/


    /******************** 점프스케어 실행 함수 ********************/
    private void StartJumpScare()
    {
        // 해당 모드 유지 위해, 변수들에 쓰레기값 할당
        cur_mode = 99999;
        hp = 99999.0f;

        // 컨트롤러 조작 및 메인캠 비활성화
        xrOrigin.SetActive(false);

        // 점프스케어 캠 활성화 및 캔버스 효과 활성화
        jumpscareCam.SetActive(true);
        bloodIMGobj.SetActive(true);
        bloodIMG.color = new Color(1, 0, 0, Random.Range(0.05f, 0.15f));

        //gameObject.GetComponent<NavMeshAgent>().enabled = false;

        // 효과음 설정
        HeartBeatSound.SetActive(false);
        ZombieSound.SetActive(false);
        jumpscareSound.SetActive(true);
    }
    /************************************************************/


    /******************** 점프스케어 카메라 효과 (프레임 마다 호출 필요) ********************/
    private void JumpScareCamEff()
    {
        // 점프스케어 카메라 위치 조정
        jumpscareCam.transform.position =
            transform.position + transform.forward * 0.85f + new Vector3(0, 0.5f, 0);
        // 점프스케어 카메라 진동 효과
        jumpscareCam.transform.eulerAngles =
            transform.eulerAngles
            + new Vector3(-65f + Random.Range(-1.5f, 1.5f), 180f
            + Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
    }
    /******************************************************************************/


    /******************** 5초 대기 후 페이드아웃 효과 ********************/
    private IEnumerator FadeOutStart()
    {
        yield return new WaitForSeconds(5.0f);
        FadeOutBlack.SetActive(true);
    }
    /***************************************************************/
}
