# LowLevelPhysics2D ownership

Unity 6000.3 LowLevelPhysics2D의 독립 월드 수명과 실제 사용하는 정적 공간 질의를 제공한다. Collections 2.6.2가 필요하다. ECS, Rigidbody2D 기본 월드, 자동 고정 업데이트를 만들지 않는다.

`PhysicsWorldOwner`는 gravity=0인 독립 월드를 생성하고 `SimulationType.Script`로 설정한다. `CreateStaticBody()`로 등록한 정적 형상은 Unity 6000.3.11f1에서 Simulate 호출 없이 즉시 AABB 질의에 반영된다. 생성·변경·질의·폐기는 소유 스코프의 주 스레드에서 실행한다.

`OverlapAabb(minimum, maximum)`은 후보를 `Allocator.Temp` NativeArray로 반환한다. 호출자가 `try/finally`에서 Dispose하고 실제 게임 형상·종류 규칙을 검사해야 한다. 월드와 body/shape 핸들은 소유 스코프를 넘겨 보관하지 않는다. Dispose는 반복 호출 가능하며 하위 핸들도 무효화한다.

게임의 격자/청크 매핑은 이 패키지에 포함하지 않는다. IdleMine.Physics2D의 LowLevelExpeditionContactQuery가 16행 청크와 셀 shape를 관리하고 이동 전체 구간의 후보를 Core 외곽 규칙으로 필터링한다. UI나 보이는 행 범위에 의존하지 않는다.

검증 소비자: IdleMine ExpeditionPhysicsTests. 새 형상 즉시 질의, 격자 기준과 80개 무작위 이동 및 파괴 셀 갱신 비교, 전 스테이지 이동, 세션 교체, 12회 월드 생성/폐기, 실제 RunScope 무화면 원정이 포함된다.
