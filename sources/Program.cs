using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.MainGameScene;

namespace TMS {
    class Program {

        ////=============================================================================
        //// Main
        ////
        ////=============================================================================

        [STAThread]
        static void Main(string[] args) {
            // Altseedを初期化する。
            asd.Engine.Initialize("ツミオマインスイーパー", 640, 480, new asd.EngineOption());

            /*ここからメインの処理*/

            StartGame();

            /*ここまでメインの処理*/

            // Altseedのウインドウが閉じられていないか確認する。
            while(asd.Engine.DoEvents()) {
                // Altseedを更新する。
                asd.Engine.Update();
            }

            // Altseedの終了処理をする。
            asd.Engine.Terminate();
        }


        ////=============================================================================
        //// Initializer
        ////
        ////=============================================================================

        private static void StartGame() {
            MainScene scene = new MainScene();
            asd.Engine.ChangeSceneWithTransition(scene, new asd.TransitionFade(0, 1.0f));
        }
    }
}
