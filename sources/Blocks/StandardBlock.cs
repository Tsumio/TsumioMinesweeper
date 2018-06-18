using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;

namespace TMS.Blocks {
    /// <summary>
    /// 通常のブロック。今回はこれを使っとけばOK。
    /// </summary>
    public class StandardBlock : BlockBase {

        ////=============================================================================
        //// Local Field
        ////
        ////=============================================================================

        /// <summary>
        /// すでにブロックはマウスに乗られているかどうか
        /// </summary>
        private bool _isAlreadyOnMouseOver = false;

        ////=============================================================================
        //// コンストラクタ
        ////
        ////=============================================================================

        /// <summary>
        /// 基本コンストラクタ
        /// </summary>
        /// <param name="state">初期状態</param>
        public StandardBlock(IBlockState state, int x, int y, int blockSize, int aroundMineNum, bool hasMine = false) 
            : base(state, x, y, blockSize, aroundMineNum, hasMine) {
            
        }


        ////=============================================================================
        //// ADS
        ////
        ////=============================================================================

        /// <summary>
        /// 更新処理
        /// </summary>
        protected override void OnUpdate() {
            base.OnUpdate();

            UpdateDrawing();
            UpdateClick();
            UpdateRightClick();
        }


        ////=============================================================================
        //// Private Method
        ////
        ////=============================================================================

        /// <summary>
        /// マウスの左クリック状態をチェックする
        /// </summary>
        private void UpdateClick() {
            //マウスをクリックしたのでなければ即リターン
            if(Engine.Mouse.LeftButton.ButtonState != MouseButtonState.Push) {
                return;
            }

            //マウスがヒットしたのでなければ即リターン
            if(!IsMouseHit()) {
                return;
            }

            //状態に応じてクリックイベント発火
            CurrentState.OnClicked(this);
        }

        /// <summary>
        /// マウスの右クリック状態をチェックする
        /// </summary>
        private void UpdateRightClick() {
            //マウスをクリックしたのでなければ即リターン
            if(Engine.Mouse.RightButton.ButtonState != MouseButtonState.Push) {
                return;
            }

            //マウスがヒットしたのでなければ即リターン
            if(!IsMouseHit()) {
                return;
            }

            //状態に応じて右クリックイベント発火
            CurrentState.OnRightClicked(this);
        }

        /// <summary>
        /// 描画状態を更新する
        /// </summary>
        private void UpdateDrawing() {
            //HACK:条件分岐が複雑なので、なんとかしたい……。
            //Note:要はマウスがブロックの上に乗ったらOnMOuseOverを実行して、乗っていなかったら通常のDrawを実行
            //Note:複雑に見えるのは、無駄な重複処理をなくすための_isAlreadyOnMouseOverフィールドを使っているため。これ使わなかったら見た目が綺麗になる。
            if(IsMouseHit()) {
                //すでにマウスがブロックの上に乗っていたらリターン
                if(_isAlreadyOnMouseOver) {
                    return;
                }
                //描画の更新＋フラグONに
                CurrentState.OnMouseOver(this);
                _isAlreadyOnMouseOver = true;
            } else {
                //すでにマウスがブロックの上から離れていたらリターン
                if(!_isAlreadyOnMouseOver) {
                    return;
                }
                //描画の更新＋フラグOFFに
                CurrentState.Draw(this);
                _isAlreadyOnMouseOver = false;
            }
        }

        /// <summary>
        /// マウスが矩形にヒットしているかどうか
        /// </summary>
        private bool IsMouseHit() {
            //各矩形の大きさを取得
            var top    = YPosition;
            var bottom = YPosition + BlockSize;
            var left   = XPosition;
            var right  = XPosition + BlockSize;

            //当たり判定の計算
            if(top > Engine.Mouse.Position.Y || bottom < Engine.Mouse.Position.Y || left > Engine.Mouse.Position.X || right <　Engine.Mouse.Position.X) {
                return false;
            } else {
                return true;
            }
        }
    }
}
