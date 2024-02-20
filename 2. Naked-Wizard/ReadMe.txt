Naked-Wizard 게임은 Unity Engine을 사용하여 제작한 3D Action 게임입니다.

지닌 카드를 통해 스킬을 써나아가며 몬스터를 제거하는 방식입니다.

1. Card
- Card의 정보를 통한 이미지 및 스킬 자동 초기화
- CardHanlder를 통해 카드 스킬 사용 및 획득
- 각 스킬들은 Coroutine 및 ParticleSystem을 통해 이펙트를 연출

2. Item
- 각 카드 스킬별로 획득 확률을 계산 후 생성 및 획득

3. Manager
- Singleton Pattern을 사용
- 전체 Coroutine 제어
- 각 Object(Prefab) 및 Image Resource 저장

4. BossMonster
- State Pattern을 이용한 보스의 State 제어

5. UI
- 선형 보간 및 Coroutine을 이용한 자연스러운 UI 연출

6. Utility
- MonoBehaviour를 기반으로 Singleton Pattern을 적용할 수 있는 상위 클래스 제작
- Generic ObjectPooling을 제작하여 Object 관리
- 편의성을 위한 static 형 Particle Utility 메소드 제작