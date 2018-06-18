using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;

namespace TMS.Blocks {
    /// <summary>
    /// 旗を立てているブロックの状態
    /// </summary>
    class FlagedState : IBlockState {

        ////=============================================================================
        //// Local Field
        ////
        ////=============================================================================

        /// <summary>
        /// 使用するテキストオブジェクト
        /// </summary>
        private TextObject2D _textObj = new TextObject2D();

        ////=============================================================================
        //// コンストラクタ
        ////
        ////=============================================================================

        public FlagedState(BlockBase target) {
            InitializeFont(target);
        }

        /// <summary>
        /// フォトの初期化
        /// </summary>
        /// <param name="target">対象ブロック</param>
        private void InitializeFont(BlockBase target) {
            //文字の作成
            _textObj.Font = Engine.Graphics.CreateDynamicFont("", target.BlockSize/2, new Color(255, 0, 0, 255), 1, new Color(255, 255, 255, 255));
            _textObj.Text = "▲";

            //文字位置の調整
            var xCorrect = _textObj.Font.CalcTextureSize("S", WritingDirection.Horizontal).X / 2;
            _textObj.Position = new Vector2DF(target.XPosition + xCorrect, target.YPosition);

            //レイヤーへ追加
            target.Layer.AddObject(_textObj);
        }
        ////=============================================================================
        //// Public Method
        ////
        ////=============================================================================

        /// <summary>
        /// 旗マークをつけて描画
        /// </summary>
        /// <param name="target">対象ブロック</param>
        public void Draw(BlockBase target) {
            var rect = MakeRectShape(target);
            target.Shape = rect;
            target.Color = new Color(0, 0, 255);
        }

        /// <summary>
        /// 旗を立ててガードしているので何もしない
        /// </summary>
        /// <param name="target">対象ブロック</param>
        public void OnClicked(BlockBase target) {
            //特に何もしない
        }

        /// <summary>
        /// 色を濃くする
        /// </summary>
        /// <param name="target">対象ブロック</param>
        public void OnMouseOver(BlockBase target) {
            var rect = MakeRectShape(target);
            target.Shape = rect;
            target.Color = new Color(0, 0, 150);
        }

        /// <summary>
        /// 旗マークを解除する
        /// </summary>
        /// <param name="target">対象ブロック</param>
        public void OnRightClicked(BlockBase target) {
            _textObj.Dispose();
            target.ChangeState(new ClosedState());
        }

        /// <summary>
        /// 旗マークが間違っていた場合、強制オープン
        /// </summary>
        /// <param name="target">対象ブロック</param>
        public void TryOpen(BlockBase target) {
            //最初に文字を解放
            _textObj.Dispose();
            //その後で強制オープンの処理
            target.ChangeState(new OpenedState(target));
            if(target.AroundMinesNum == 0) {
                foreach(var block in target.NeighborBlockList) {
                    block.CurrentState.TryOpen(block);
                }
            }
        }

        /// <summary>
        /// 強制的に周囲のブロックごとオープンする
        /// </summary>
        /// <param name="target">対象ブロック</param>
        public void ForceOpen(BlockBase target) {
            //最初に文字を解放
            _textObj.Dispose();
            //その後で強制オープンの処理
            target.ChangeState(new OpenedState(target));
        }

        ////=============================================================================
        //// Private Method
        ////
        ////=============================================================================

        /// <summary>
        /// 矩形の基本的な形を作る
        /// </summary>
        /// <param name="target">対象ブロック</param>
        /// <returns></returns>
        private RectangleShape MakeRectShape(BlockBase target) {
            var rect = new RectangleShape();
            rect.DrawingArea = new RectF(target.XPosition, target.YPosition, target.BlockSize, target.BlockSize);

            return rect;
        }
    }
}
