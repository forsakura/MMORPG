using Assets.Scripts.Managers;
using Assets.Scripts.Services;
using Entities;
using GameServer.Managers;
using SkillBridge.Message;
using UnityEngine;

public class PlayerInputController : MonoBehaviour {

	public Rigidbody rb;
	CharacterState state;
	public Character character;
	public float rotateSpeed = 2f;
	public float turnAngle = 10;
	public int speed;
	public EntityController entityController;
	public bool onAir = false;
	// Use this for initialization
	void Start () {
		state = CharacterState.Idle;
		if(character == null)
		{
			DataManager.Instance.Load();
			NCharacterInfo info = new NCharacterInfo()
			{
				Id = 1,
				Name = "Test",
				ConfigId = 1,
				Entity = new NEntity
				{
					Position = new NVector3(),
					Direction = new NVector3() { X = 0, Y = 100, Z = 0 },
				}				
			};
			character = new Character(info);
			if(entityController != null) entityController.entity = character;
		}
	}

    private void FixedUpdate()
    {
		if (character == null) return;
		if (InputManager.Instance != null && InputManager.Instance.isInputMode) return;
		float v = Input.GetAxis("Vertical");
		if(v > 0.1f)
		{
			if(state !=  CharacterState.Move)
			{
                state = CharacterState.Move;
                character.MoveForward();
                SendEntityEvent(EntityEvent.MoveFwd);
            }
            rb.velocity = rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (character.speed + 9.81f) / 100f;
        }

        else if (v < -0.1f)
        {
            if (state != CharacterState.Move)
            {
                state = CharacterState.Move;
                character.MoveBack();
                SendEntityEvent(EntityEvent.MoveBack);
            }
            rb.velocity = rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (character.speed + 9.81f) / 100f;
        }

		else
		{
			if(state != CharacterState.Idle)
			{
				state = CharacterState.Idle;
				this.rb.velocity = Vector3.zero;
				character.Stop();
				SendEntityEvent(EntityEvent.Idle);
			}
        }

		if(Input.GetButtonDown("Jump"))
		{
			SendEntityEvent(EntityEvent.Jump);
		}

		float h = Input.GetAxis("Horizontal");
		if(h < -0.1f || h > 0.1f)
		{
			transform.Rotate(0, h * rotateSpeed, 0);
			Vector3 dir = GameObjectTool.LogicToWorld(character.direction);
			Quaternion rot = new Quaternion();
			rot.SetFromToRotation(dir, transform.forward);

			if(rot.eulerAngles.y > this.turnAngle && rot.eulerAngles.y < (360 - turnAngle))
			{
				character.SetDirection(GameObjectTool.WorldToLogic(transform.forward));
				rb.transform.forward = transform.forward;
				SendEntityEvent(EntityEvent.None);
			}
		}
		//Debug.LogFormat("velocity {0}", rb.velocity.magnitude);
    }
    Vector3 lastPos;
    float lastSync = 0;
    private void LateUpdate()
    {
        if (this.character == null) return;

        Vector3 offset = this.rb.transform.position - lastPos;
        this.speed = (int)(offset.magnitude * 100f / Time.deltaTime);
        //Debug.LogFormat("LateUpdate velocity {0} : {1}", this.rb.velocity.magnitude, this.speed);
        this.lastPos = this.rb.transform.position;

        Vector3Int goLogicPos = GameObjectTool.WorldToLogic(this.rb.transform.position);
        float logicOffset = (goLogicPos - this.character.position).magnitude;
        if (logicOffset > 100)
        {
            this.character.SetPosition(GameObjectTool.WorldToLogic(this.rb.transform.position));
            this.SendEntityEvent(EntityEvent.None);
        }
        this.transform.position = this.rb.transform.position;
    }

    public void SendEntityEvent(EntityEvent entityEvent, int param = 0)
    {
        if(entityController != null)
		{
			entityController.OnEntityEvent(entityEvent, param);
		}
		MapService.Instance.SendMapEntitySync(entityEvent, character.EntityData, param);
    }
}
