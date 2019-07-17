using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    #region 1. 계약인터페이스(클라이언트->서버)
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IGameServiceCallback))]
    public interface IGameService
    {
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        bool Join(int usernumber);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void Leave(int usernumber);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void SayAll(int usernumber, String msg);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void GameReady(int usernumber, int digits);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void BallResult(int usernumber, String msg);

    }
    #endregion

    #region 2. 콜백인터페이스(서버->클라이언트)
    public interface IGameServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveAll(int usernumber, string message);

        [OperationContract(IsOneWay = true)]
        void UserEnter(int usernumber);

        [OperationContract(IsOneWay = true)]
        void UserLeave(int usernumber);

        [OperationContract(IsOneWay = true)]
        void GameStart(int usernumber);

        [OperationContract(IsOneWay = true)]
        void UserBallResult(int usernumber, string message);
    }

    #endregion

    public delegate void GameDel(int usernum, string msg, string type);

    class MainService : IGameService
    {
        //델리게이트 선언
        public delegate void Game(int idx, string msg, string type);
        //동기화 작업을 위해서 가상의 객체 생성
        private static Object syncObj = new Object();
        //접속한 유저 이름 목록
        private static ArrayList UserList = new ArrayList();
        //준비한 유저 이름 목록
        private static ArrayList RUserList = new ArrayList();
        //델리게이트 =========================================================
        // 개인용 델리게이트
        private Game MyGame;
        //전체에게 보낼 정보를 담고 있는 델리게이트
        private static Game List;
        IGameServiceCallback callback = null; //

        public bool Join(int usernumber)
        {
            MyGame = new Game(GameHandler);
            lock (syncObj)
            {
                if (!UserList.Contains(usernumber)) //이름이 기존 채터에 있는지 검색한다.
                {
                    //2. 사용자에게 보내 줄 채널을 설정한다.
                    callback = OperationContext.Current.GetCallbackChannel<IGameServiceCallback>();
                    //현재 접속자 정보를 모두에게 전달
                    BroadcastMessage(usernumber, "", "UserEnter");
                    //델리게이터 추가(향후 데이터 수신이 가능하도록 구성)
                    UserList.Add(usernumber);
                    List += MyGame;

                    return true;
                }
                return false;
            }
        }

        public void Leave(int usernumber)
        {
            UserList.Remove(usernumber);
            List -= MyGame;
            //모든 사람에게 전송
            string msg = string.Format(usernumber + "이가 나갔습니다");
            BroadcastMessage(usernumber, msg, "UserLeave");
        }

        public void SayAll(int usernumber, string msg)
        {
            BroadcastMessage(usernumber, msg, "SayAll");
        }

        public void BallResult(int usernumber, string msg)
        {
            BroadcastMessage(usernumber, msg, "UserBallResult");
        }


        public void GameReady(int usernumber, int digits)
        {
            RUserList.Add(MyGame);  
            if (RUserList.Count == UserList.Count)
            {
                BroadcastMessage(usernumber, "게임이 시작되었습니다.", "GameStart");
                RUserList.Clear();
            }
        }




        public void BroadcastMessage(int usernumber, string msg, string type)
        {
            lock (syncObj)
            {
                if (List != null)
                {
                    //현재 이벤트들을 전달한다.
                    foreach (Game handler in List.GetInvocationList())
                    {
                        if(type == "UserBallResult" && handler == MyGame)
                        {
                            continue;
                        }
                        handler.BeginInvoke(usernumber, msg, type, new AsyncCallback(EndAsync), null);
                    }
                }
            }
        }

        private void GameHandler(int usernumber, string msg, string type)
        {
            try
            {
                //클라이언트에게 보내기
                switch (type)
                {
                    case "SayAll":
                        callback.ReceiveAll(usernumber, msg);
                        break;
                    case "UserEnter":
                        callback.UserEnter(usernumber);
                        break;
                    case "UserLeave":
                        callback.UserLeave(usernumber);
                        break;
                    case "GameStart":
                        callback.GameStart(usernumber);
                        break;
                    case "UserBallResult":
                        callback.UserBallResult(usernumber,msg);
                        break;
                }
            }
            catch//에러가 발생했을 경우
            {
                Leave(usernumber);
            }

        }

        private void EndAsync(IAsyncResult ar)
        {
            Game d = null;
            try
            {
                System.Runtime.Remoting.Messaging.AsyncResult asres =
               (System.Runtime.Remoting.Messaging.AsyncResult)ar;
                d = ((Game)asres.AsyncDelegate);
                d.EndInvoke(ar);
            }
            catch
            {
                List -= d;
            }
        }

        
    }
}
