using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Blocks {
    /// <summary>
    /// ブロックの状態ためのインターフェイス
    /// </summary>
    public interface IBlockState {

        ////=============================================================================
        //// Public Method
        ////
        ////=============================================================================

        /// <summary>
        /// ブロックを描画する
        /// </summary>
        /// <param name="target">該当ブロック</param>
        void Draw(BlockBase target);

        /// <summary>
        /// クリックされたときに呼び出す
        /// </summary>
        /// <param name="target">該当ブロック</param>
        void OnClicked(BlockBase target);

        /// <summary>
        /// 右クリックされたときに呼び出す
        /// </summary>
        /// <param name="target">該当ブロック</param>
        void OnRightClicked(BlockBase target);

        /// <summary>
        /// マウスが上に乗ったときに呼び出す
        /// </summary>
        /// <param name="target">該当ブロック</param>
        void OnMouseOver(BlockBase target);

        /// <summary>
        /// 該当ブロックをオープンしようと試みる。主にOpenedBlockが空だったときで、隣も連鎖的に開けたいときに使う
        /// </summary>
        /// <param name="target">該当ブロック</param>
        void TryOpen(BlockBase target);

        /// <summary>
        /// 該当ブロックを強制的にオープンする。主にゲームオーバー時、強制的に開ける目的で使う
        /// </summary>
        /// <param name="target">該当ブロック</param>
        void ForceOpen(BlockBase target);
    }
}
