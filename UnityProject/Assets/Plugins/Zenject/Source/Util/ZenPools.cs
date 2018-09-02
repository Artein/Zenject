#define ZEN_INTERNAL_USE_POOLS

using System;
using System.Collections.Generic;

namespace Zenject.Internal
{
    public static class ZenPools
    {
#if ZEN_INTERNAL_USE_POOLS
        static readonly StaticMemoryPool<InjectContext> _contextPool = new StaticMemoryPool<InjectContext>();
        static readonly StaticMemoryPool<LookupId> _lookupIdPool = new StaticMemoryPool<LookupId>();
        static readonly StaticMemoryPool<BindInfo> _bindInfoPool = new StaticMemoryPool<BindInfo>();
        static readonly StaticMemoryPool<BindStatement> _bindStatementPool = new StaticMemoryPool<BindStatement>();

        public static Dictionary<TKey, TValue> SpawnDictionary<TKey, TValue>()
        {
            return DictionaryPool<TKey, TValue>.Instance.Spawn();
        }

        public static BindStatement SpawnStatement()
        {
            return _bindStatementPool.Spawn();
        }

        public static void DespawnStatement(BindStatement statement)
        {
            statement.Reset();
            _bindStatementPool.Despawn(statement);
        }

        public static BindInfo SpawnBindInfo()
        {
            return _bindInfoPool.Spawn();
        }

        public static void DespawnBindInfo(BindInfo bindInfo)
        {
            bindInfo.Reset();
            _bindInfoPool.Despawn(bindInfo);
        }

        public static void DespawnDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            DictionaryPool<TKey, TValue>.Instance.Despawn(dictionary);
        }

        public static LookupId SpawnLookupId(IProvider provider, BindingId bindingId)
        {
            var lookupId = _lookupIdPool.Spawn();

            lookupId.Provider = provider;
            lookupId.BindingId = bindingId;

            return lookupId;
        }

        public static void DespawnLookupId(LookupId lookupId)
        {
            _lookupIdPool.Despawn(lookupId);
        }

        public static List<T> SpawnList<T>()
        {
            return ListPool<T>.Instance.Spawn();
        }

        public static void DespawnList<T>(List<T> list)
        {
            ListPool<T>.Instance.Despawn(list);
        }

        public static InjectContext SpawnInjectContext(DiContainer container, Type memberType)
        {
            var context = _contextPool.Spawn();

            context.Container = container;
            context.MemberType = memberType;

            return context;
        }

        public static void DespawnInjectContext(InjectContext context)
        {
            context.Reset();
        }
#else
        public static InjectContext SpawnInjectContext(DiContainer container, Type memberType)
        {
            return new InjectContext(container, memberType);
        }

        public static void DespawnInjectContext(InjectContext context)
        {
        }

        public static List<T> SpawnList<T>()
        {
            return new List<T>();
        }

        public static void DespawnList<T>(List<T> list)
        {
        }

        public static Dictionary<TKey, TValue> SpawnDictionary<TKey, TValue>()
        {
            return new Dictionary<TKey, TValue>();
        }

        public static void DespawnDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
        }

        public static LookupId SpawnLookupId(IProvider provider, BindingId bindingId)
        {
            return new LookupId(provider, bindingId);
        }

        public static void DespawnLookupId(LookupId lookupId)
        {
        }

        public static BindInfo SpawnBindInfo()
        {
            return new BindInfo();
        }

        public static void DespawnBindInfo(BindInfo bindInfo)
        {
        }

        public static BindStatement SpawnStatement()
        {
            return new BindStatement();
        }

        public static void DespawnStatement(BindStatement statement)
        {
        }
#endif
    }
}