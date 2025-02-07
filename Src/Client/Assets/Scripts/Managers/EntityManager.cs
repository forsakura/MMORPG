using Entities;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    interface IEntityNotify
    {
        void OnEntityRemoved();
    }
    class EntityManager : Singleton<EntityManager>
    {

        Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        Dictionary<int, IEntityNotify> notifiers = new Dictionary<int, IEntityNotify>();

        public void RegisterEntityChangeNotify(int notifyId, IEntityNotify notify)
        {
            notifiers[notifyId] = notify;
        }

        public void AddEntity(Entity entity)
        {
            entities[entity.entityId] = entity;
        }

        public void RemoveEntity(NEntity entity)
        {
            entities.Remove(entity.Id);
            if (notifiers.ContainsKey(entity.Id))
            {
                notifiers[entity.Id].OnEntityRemoved();
                notifiers.Remove(entity.Id);
            }
        }
    }
}