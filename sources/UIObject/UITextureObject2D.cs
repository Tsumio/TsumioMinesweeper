using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;

namespace TMS.UIObject {
    /// <summary>
    /// タッチに反応する機能を持っているTextureObject2D。
    /// ClickedイベントとRightClickedイベントに、実行したい処理を登録していく。
    /// Note:必要があれば、マウスポインターがオーバーしたときや、離れたときの処理も作っていいかも。
    /// </summary>
    public class UITextureObject2D : TextureObject2D {

        ////=============================================================================
        //// Events
        ////
        ////=============================================================================

        /// <summary>
        /// 左クリックされたときに発火するイベント
        /// </summary>
        public event EventHandler<EventArgs> Clicked;

        /// <summary>
        /// 右クリックされたときに発火するイベント
        /// </summary>
        public event EventHandler<EventArgs> RightClicked;

        ////=============================================================================
        //// ASD
        ////
        ////=============================================================================

        protected override void OnUpdate() {
            base.OnUpdate();
            UpdateClick();
            UpdateRightClick();
        }

        /// <summary>
        /// マウスの左クリック状態をチェックする
        /// </summary>
        private void UpdateClick() {
            //マウスをクリックしたのでなければ即リターン
            if(Engine.Mouse.LeftButton.ButtonState != MouseButtonState.Push) {
                return;
            }

            //オブジェクトにヒットしていなければ即リターン
            if(!IsHit()) {
                return;
            }

            OnClicked(new EventArgs());
        }

        /// <summary>
        /// マウスの右クリック状態をチェックする
        /// </summary>
        private void UpdateRightClick() {
            //マウスを右クリックしたのでなければ即リターン
            if(Engine.Mouse.RightButton.ButtonState != MouseButtonState.Push) {
                return;
            }

            //オブジェクトにヒットしていなければ即リターン
            if(!IsHit()) {
                return;
            }

            OnRightClicked(new EventArgs());
        }


        ////=============================================================================
        //// Protected Method
        ////
        ////=============================================================================

        /// <summary>
        /// 左クリックされたときに発火
        /// </summary>
        protected virtual void OnClicked(EventArgs e) {
            Clicked?.Invoke(this, e);
        }

        /// <summary>
        /// 右クリックされたときに発火
        /// </summary>
        protected virtual void OnRightClicked(EventArgs e) {
            RightClicked?.Invoke(this, e);
        }

        ////=============================================================================
        //// Private Method
        ////
        ////=============================================================================

        /// <summary>
        /// 対象オブジェクトがクリックされたかどうかをチェック
        /// </summary>
        private bool IsHit() {
            //当たり判定の計算
            if(Position.Y > Engine.Mouse.Position.Y || Position.Y +  Src.Height < Engine.Mouse.Position.Y ||
                Position.X > Engine.Mouse.Position.X || Position.X + Src.Width < Engine.Mouse.Position.X) {
                return false;
            }

            return true;
        }
    }
}
