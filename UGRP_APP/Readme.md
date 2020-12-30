1. 경로
    
  script녹음: UGRP/data
  유저가 원하는 텍스트: UGRP/sync_system/computer/upload
  서버에서 만든 보이스: UGRP/sync_system/computer/download
  유저가 보이스 저장: UGRP/data
  피드백 점수: UGRP/feedback_system/computer/upload
  
2. 경로수정방법 (본인 UGRP 폴더 저장한 경로로 수정하면 됩니다.)
     TextManager.cs
       22, 25줄

       SendRoutineManager.cs
       15줄

       SendTxtSceneUIManager.cs
       59줄

       AudioSerializer.cs
       41줄

3. IP주소 설정
     UNetUIManager.cs
       16줄

4. 본인 local IP 확인하는법
     cmd창 -> 명령어 ipconfig > IPv4 주소