

0. 경로

   script 녹음: UGRP/data
   유저가 원하는 텍스트: UGRP/sync_system/computer/upload
   서버에서 만든 보이스: UGRP/sync_system/computer/download
   피드백 점수: UGRP/feedback_system/computer/upload

   유저가 보이스 저장: persistantdatapath/data

1. 경로수정 (본인 UGRP 폴더 저장한 경로로 수정하면 됩니다.)
     TextManager.cs
       22, 25줄

       SendRoutineManager.cs
       15줄

       AudioSerializer.cs
       41줄, 114줄

2. IP주소 설정
     UNetUIManager.cs
       16줄

3. 본인 local IP 확인하는법
     cmd창 -> 명령어 ipconfig > IPv4 주소

4. 빌드

     file > Build Settings 클릭

     ​	Scenes In Build

     ​		Client:  startScene, ScriptScene, SendTxtScene (위에 3개) 클릭

     ​		Host: UNetHostScene(가장 아래거) 클릭

     ​	Platform: PC, Mac & Linux Standalone

     ​	오른쪽에 뜨는 Server Build 어쩌구저쩌구는 다 체크 해제

     file > BuildSettings > Player Settings > ProductName 이름 client, host로 변경

     Build 버튼 클릭

5. 실행 (저장한 폴더에서 찾아서 실행시키면 됩니다.)

   1) Host 실행, LAN Server Only 클릭

   2) Client 실행, LAN client 클릭

