using Assets.Scripts.Managers;
using Entities;
using SkillBridge.Message;
using System;
using UnityEngine;

namespace Assets.Scripts.GameObject
{
    public class RideController : MonoBehaviour
    {
        public Transform mountPoint;
        public EntityController rider;
        public Vector3 offset;
        Animator anim;

        private void Start()
        {
            this.anim = GetComponent<Animator>();
        }

        private void Update()
        {
            if (this.mountPoint == null || this.rider == null) return;

            this.rider.SetRidePotision(this.mountPoint.position + this.mountPoint.TransformDirection(this.offset));
        }

        public void SetRider(EntityController rider)
        {
            this.rider = rider;
        }

        public void OnEntityEvent(EntityEvent @event, int param)
        {
            switch (@event)
            {
                case EntityEvent.Idle:
                    anim.SetBool("Move", false);
                    anim.SetTrigger("Idle");
                    break;
                case EntityEvent.MoveFwd:
                    anim.SetBool("Move", true);
                    break;
                case EntityEvent.MoveBack:
                    anim.SetBool("Move", true);
                    break;
                case EntityEvent.Jump:
                    anim.SetTrigger("Jump");
                    break;
            }
        }
    }
}
