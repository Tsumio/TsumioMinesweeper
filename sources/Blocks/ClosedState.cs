using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;

namespace TMS.Blocks {
    /// <summary>
    /// ブロックが閉じられた状態を表す
    /// </summary>
    class ClosedState : IBlockState {
        ////=============================================================================
        //// Public Method
        ////
        ////=============================================================================

        /// <summary>
        /// なにもないので通常の白い矩形を描く
        /// </summary>
        /// <param name="target">対象ブロック</param>
        public void Draw(BlockBase target) {
            var rect = MakeRectShape(target);
            target.Shape = rect;
            target.Color = new Color(255, 255, 255);
        }

        /// <summary>
        /// マウスが乗ったので赤色に変える
        /// </summary>
        /// <param name="target">対象ブロック</param>
        public void OnMouseOver(BlockBase target) {
            var rect = MakeRectShape(target);
            target.Shape = rect;
            target.Color = new Color(255, 0, 0);
        }

        /// <summary>
        /// 状態をオープンにする
        /// </summary>
        /// <param name="target">対象ブロック</param>
        public void OnClicked(BlockBase target) {
            if(target.HasMine) {
                target.OnGameFailed(new EventArgs());
                return;
            }
            target.ChangeState(new OpenedState(target));
            //周囲も開けようと試みる
            TryOpen(target);
        }

        /// <summary>
        /// 旗を立てる処理
        /// </summary>
        /// <param name="target">対象ブロック</param>
        public void OnRightClicked(BlockBase target) {
            target.ChangeState(new FlagedState(target));
        }

        /// <summary>
        /// 周囲のブロックを強制オープンしようと試みる
        /// </summary>
        /// <param name="target">対象ブロック</param>
        public void TryOpen(BlockBase target) {
            /*
             * 1．最初にTryOpenが実行されたブロックをオープンにする
             * 2．TryOpenが実行されたブロックの周りが0ならば
             * 3．各ブロックを走査し
             * 4．再帰的にTryOpenを呼ぶ
             */
            target.ChangeState(new OpenedState(target));//1
            if(target.AroundMinesNum == 0) {//2
                foreach(var block in target.NeighborBlockList) {//3
                    block.CurrentState.TryOpen(block);//4
                }
            }
        }

        /// <summary>
        /// 強制的に周囲のブロックごとオープンする
        /// </summary>
        /// <param name="target">対象ブロック</param>
        public void ForceOpen(BlockBase target) {
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
