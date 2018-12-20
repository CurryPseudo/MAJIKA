﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MoonSharp.Interpreter;

namespace LuaHost.Proxy
{
    class ProxyBase<T> where T:class
    {
        protected T target;
        protected LuaScriptHost host;
        public ProxyBase(T target):this(target,null)
        {
        }
        public ProxyBase(T target,LuaScriptHost host)
        {
            this.target = target;
            this.host = host;
        }
    }
    class GameObjectProxy:ProxyBase<GameObject>
    {

        [MoonSharpHidden]
        public GameObjectProxy(GameObject gameObject):base(gameObject)
        {
        }
        public string name => target.name;
    }

    

    class Vector2Proxy
    {
        Vector2 vector;

        [MoonSharpHidden]
        public Vector2Proxy(Vector2 v)
        {
            this.vector = v;
        }

        public float x
        {
            get { return vector.x; }
            set { vector.x = value; }
        }
        public float y
        {
            get { return vector.y; }
            set { vector.y = value; }
        }
        public float magnitude => vector.magnitude;
        public Vector2 normalized => vector.normalized;
    }

    class CoroutineProxy : ProxyBase<UnityEngine.Coroutine>
    {
        public CoroutineProxy(UnityEngine.Coroutine target) : base(target)
        {
        }
    }
}
