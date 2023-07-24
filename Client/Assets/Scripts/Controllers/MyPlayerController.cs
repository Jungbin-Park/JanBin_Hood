using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class MyPlayerController : PlayerController
{
    // 마이룸
    private bool _isMyRoom = true;
    GameObject map;

    bool _moveKeyPressed = false;

    AudioSource audio;

    protected override void Init()
    {
        base.Init();
        audio = GetComponent<AudioSource>();
        audio.loop = false;
    }

    protected override void UpdateController()
    {
        switch (State)
        {
            case CreatureState.Idle:
                GetDirInput();
                break;
            case CreatureState.Moving:
                GetDirInput();
                break;
        }

        base.UpdateController();
    }

    
    protected override void UpdateIdle()
    {
        // 이동 상태로 갈지 확인
        if (_moveKeyPressed)
        {
            State = CreatureState.Moving;
            return;
        }
        
        // 스킬 상태로 갈지 확인
        if (_coSkillCooltime == null && Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Skill");

            C_Skill skill = new C_Skill() { Info = new SkillInfo() };
            skill.Info.SkillId = 1;
            Managers.Network.Send(skill);

            _coSkillCooltime = StartCoroutine("CoInputCooltime", 0.2f);

            //if (audio.isPlaying) return;
            audio.Play();
        }
        
    }

    Coroutine _coSkillCooltime;
    IEnumerator CoInputCooltime(float time)
    {
        yield return new WaitForSeconds(time);
        _coSkillCooltime = null;
    }

    void LateUpdate()
    {
        if (_isMyRoom)
        {
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            Camera.main.orthographicSize = 5;
        }
            
        else
        {
            Camera.main.transform.position = new Vector3(map.transform.position.x, map.transform.position.y, -20);
            Camera.main.orthographicSize = 7;
        }
            
    }

    // 키보드 입력
    void GetDirInput()
    {
        _moveKeyPressed = true;

        if (Input.GetKey(KeyCode.W))
        {
            Dir = MoveDir.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Dir = MoveDir.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Dir = MoveDir.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Dir = MoveDir.Right;
        }
        else if (Input.GetKey(KeyCode.Return))
        {
            UIFieldManager.Instance.panelChat.SetActive(true);
            UIFieldManager.Instance.ChatMsg.ActivateInputField();
        }
        else
        {
            _moveKeyPressed = false;
        }
    }

    protected override void MoveToNextPos()
    {
        if (_moveKeyPressed == false)
        {
            State = CreatureState.Idle;
            CheckUpdatedFlag();
            return;
        }

        Vector3Int destPos = CellPos;

        // 좌표 변화가 일어나는 시점 -> 패킷을 서버에 보내는 시점
        switch (Dir)
        {
            case MoveDir.Up:
                destPos += Vector3Int.up;
                break;
            case MoveDir.Down:
                destPos += Vector3Int.down;
                break;
            case MoveDir.Left:
                destPos += Vector3Int.left;
                break;
            case MoveDir.Right:
                destPos += Vector3Int.right;
                break;
        }

        if (Managers.Map.CanGo(destPos))
        {
            if (Managers.Object.FindCreature(destPos) == null)
            {
                CellPos = destPos;
            }
        }

        CheckUpdatedFlag();
    }

    protected override void CheckUpdatedFlag()
    {
        if (_updated)
        {
            // C_Move로 서버에 보내줌
            C_Move movePacket = new C_Move();
            movePacket.PosInfo = PosInfo;
            Managers.Network.Send(movePacket);
            _updated = false;
        }
    }


    #region 마이룸

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MyRoom"))
        {
            map = GameObject.Find("Map/MyRoom");

            C_MyroomLoad c_MyroomLoad = new C_MyroomLoad();
            Managers.Network.Send(c_MyroomLoad);

            _isMyRoom = false;

            UIFieldManager.Instance.MyRoomUI.SetActive(true);
        }

        if (collision.CompareTag("NPC"))
        {
            UIFieldManager.Instance.panelNPC.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MyRoom"))
        {
            map = GameObject.Find("Map/MyRoom");
            _isMyRoom = true;

            UIFieldManager.Instance.MyRoomUI.SetActive(false);
        }

        if (collision.CompareTag("NPC"))
        {
            UIFieldManager.Instance.panelNPC.SetActive(false);
        }
    }

    #endregion
}
