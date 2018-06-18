using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;
using TMS.UIObject;

namespace TMS.CompletedGame {
    /// <summary>
    /// ゲームクリア後のシーン
    /// </summary>
    public class CompletedScene : Scene{


        ////=============================================================================
        //// Local Field
        ////
        ////=============================================================================

        /// <summary>
        /// 一番下のレイヤー
        /// </summary>
        private Layer2D _mainLayer = new Layer2D();

        ////=============================================================================
        //// ASD
        ////
        ////=============================================================================

        protected override void OnRegistered() {
            base.OnRegistered();
            AddLayer(_mainLayer);

            //Note:初期化はこの順番でないといけない
            InitializeBackground();
            IntializeRetryButton();
        }


        ////=============================================================================
        //// Private Method
        ////
        ////=============================================================================

        /// <summary>
        /// 背景の初期化
        /// </summary>
        private void InitializeBackground() {
            var obj = new TextureObject2D();
            obj.Texture = Engine.Graphics.CreateTexture2D("Resources/CompletedBackground.png");
            _mainLayer.AddObject(obj);
        }

        /// <summary>
        /// リトライボタンの初期化
        /// </summary>
        private void IntializeRetryButton() {
            var obj = new UITextureObject2D();
            obj.Texture = Engine.Graphics.CreateTexture2D("Resources/Retry.png");
            obj.Position = new Vector2DF(230, 350);//Note:位置は適当。
            obj.Clicked += (sender, e) => {
                //クリックされたらメインシーンへ飛ばし、再びゲームができるようにする
                MainGameScene.MainScene scene = new MainGameScene.MainScene();
                Engine.ChangeSceneWithTransition(scene, new TransitionFade(0.2f, 0.2f));
            };
            _mainLayer.AddObject(obj);
        }
    }
}
