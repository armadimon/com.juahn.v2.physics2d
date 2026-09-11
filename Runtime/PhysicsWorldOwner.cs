using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.LowLevelPhysics2D;
namespace Juahn.V2.Physics2D
{
    /// <summary>기본 물리 월드와 독립적이다. 생성·변경·질의·폐기는 소유 실행 스코프의 주 스레드에서 수행한다.</summary>
    public sealed class PhysicsWorldOwner : IDisposable
    {
        private PhysicsWorld _world;
        private bool _disposed;
        public PhysicsWorld World => _world;
        public bool IsDisposed => _disposed;
        public PhysicsWorldOwner()
        {
            var definition=PhysicsWorldDefinition.defaultDefinition; definition.gravity=Vector2.zero;
            _world=PhysicsWorld.Create(definition); _world.simulationType=PhysicsWorld.SimulationType.Script;
        }
        public PhysicsBody CreateStaticBody()
        {
            ThrowIfDisposed(); var definition=PhysicsBodyDefinition.defaultDefinition; definition.type=PhysicsBody.BodyType.Static;
            return _world.CreateBody(definition);
        }
        /// <summary>후보만 반환한다. 호출자는 실제 형상 규칙을 검사하고 NativeArray를 Dispose해야 한다.</summary>
        public NativeArray<PhysicsQuery.WorldOverlapResult> OverlapAabb(Vector2 minimum,Vector2 maximum)
        { ThrowIfDisposed(); return _world.OverlapAABB(new PhysicsAABB(minimum,maximum),PhysicsQuery.QueryFilter.Everything,Allocator.Temp); }
        private void ThrowIfDisposed() { if(_disposed)throw new ObjectDisposedException(nameof(PhysicsWorldOwner)); }
        public void Dispose()
        { if(_disposed)return;_disposed=true;if(_world.isValid)_world.Destroy();_world=default; }
    }
}
