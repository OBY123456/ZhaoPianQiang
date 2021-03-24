using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OBYPool
{
    public abstract class BasePool : MonoBehaviour
    {
        /// <summary>
        /// 实例化的对象
        /// </summary>
        public GameObject Prefab;

        /// <summary>
        /// 对象池保存的对象最大值
        /// </summary>
        public int MaxCount = 50;

        /// <summary>
        /// 对象池
        /// </summary>
        public List<GameObject> Pool_Idle = new List<GameObject>();

        /// <summary>
        /// 当前使用的
        /// </summary>
        public List<GameObject> Pool_Use = new List<GameObject>();

        /// <summary>
        /// 从对象池中取出对象
        /// </summary>
        /// <returns></returns>
        public virtual GameObject GetForPool()
        {
            if (Pool_Idle.Count > 0)
            {
                GameObject go = Pool_Idle[0];
                Pool_Idle.RemoveAt(0);
                return go;
            }
            return Instantiate(Prefab);
        }

        /// <summary>
        /// 将物体放回对象池
        /// </summary>
        /// <param name="go"></param>
        public virtual void AddToPool(GameObject go)
        {
            if (Pool_Idle.Count < MaxCount)
            {
                Pool_Idle.Add(go);
            }
            else
            {
                Destroy(go);
            }
        }

        /// <summary>
        /// 创建物体
        /// </summary>
        public virtual void CreatObject()
        {
            GameObject obj = GetForPool();
            Pool_Use.Add(obj);
            obj.SetActive(true);
        }

        /// <summary>
        /// 销毁物体
        /// </summary>
        public virtual void DestroyObject(GameObject obj)
        {
            if (Pool_Use.Count > 0)
            {
                AddToPool(obj);
                obj.SetActive(false);
                Pool_Use.Remove(obj);
            }
        }
        public virtual void Clear()
        {
            Pool_Idle.Clear();
            Pool_Use.Clear();
        }
    }
}