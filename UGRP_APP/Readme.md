

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
       44줄, 125줄

2. 빌드

     file > Build Settings 클릭

     ​	Scenes In Build

     ​		Client:  ConnectScene, startScene, ScriptScene, SendTxtScene, LoadingScene, startScene2 (위에 6개) 클릭

     ​		Host: UNetHostScene(가장 아래거) 클릭

     ​	Platform: PC, Mac & Linux Standalone

     ​	오른쪽에 뜨는 Server Build 어쩌구저쩌구는 다 체크 해제

     file > BuildSettings > Player Settings > ProductName 이름 client, host로 변경

     Build 버튼 클릭

3. 실행 (저장한 폴더에서 찾아서 실행시키면 됩니다.)

   1) Host 실행, LAN Server Only 클릭

   2) Client 실행, 본인 IP주소 입력

   ​	(IP 확인: cmd창 -> 명령어 ipconfig > IPv4 주소)

   3) 순서

   목소리 추가 버튼 > Script 녹음화면 > 로딩화면 > 메인화면 > CD버튼 > Text 보내는 화면

   

   
