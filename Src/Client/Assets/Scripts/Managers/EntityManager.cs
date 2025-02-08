using Entities;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    interface IEntityNotify
    {
        void OnEntityRemoved();
        void OnEntityChanged(Entity entity);
        void OnEntityEvent(EntityEvent @event);
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

        internal void OnEntitySync(NEntitySync item)
        {
            Entity entity = null;
            entities.TryGetValue(item.Id, out entity);
            if (entity != null)
            {
                if(item.Entity != null)
                {
                    entity.EntityData = item.Entity;
                }
                if (notifiers.ContainsKey(item.Id))
                {
                    notifiers[entity.entityId].OnEntityChanged(entity);
                    notifiers[entity.entityId].OnEntityEvent(item.Event);
                }
            }
        }

        /*internal void OnEntitySync(NEntitySync item)
        {
            Entity entity = null;
            entities.TryGetValue(item.Id, out entity);
            if (entity != null)
            {
                if(item.Entity != null)
                {
                    entity.EntityData = item.Entity;
                }
                if(notifiers.ContainsKey(item.Id))
                {
                    notifiers[entity.entityId].OnEntityChanged(entity);
                    notifiers[entity.entityId].OnEntityEvent(item.Event);

                }
            }
        }*/
    }
}