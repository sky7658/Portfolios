PangPang 게임은 Unity Engine을 사용하여 제작한 2D Puzzle Game 입니다.

Block을 Drag하여 3 * 3 ~ 으로 조합하는 방식입니다.

1. Action
- Coroutine을 이용하여 Swap, Pang(Match가 되어 제거) 등 Block의 기본적인 연출을 표현

2. Board
- 각 블럭들은 ObjectPooling을 이용한 생성
- 경우의 수를 체크하여 게임이 원활히 진행되도록 도움
- Action의 기본적인 연출을 바탕으로
BoardController를 이용해 Block 제어하며, 게임 규칙에 따른 플레이를 도움(조합 및 비어 있는 Board에 Block 생성)
- BFS 알고리즘을 이용하여 Special Block(4 * 4 ~의 Match) 생성을 판단
- Special Block 사용 판단

3. Button
- UI에 표현되는 Button을 제어할 수 있는 스크립트

4. Input
- 플랫폼에 따른 InputBase(Mouse Or Touch)를 초기화

5. Particle
- Pang Block들의 연출을 위한 Particle을 ObjectPooling을 이용하여 생성

6. Sound
- Singleton Pattern을 이용하여 SoundManager 제작
- SFX 및 BGM Volume을 조절할 수 있는 Button 제작

7. Stage
- 게임의 전반적인 플레이를 도와주는 StageManager 제작