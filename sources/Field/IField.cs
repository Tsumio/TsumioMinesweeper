using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;
using TMS.Blocks;

namespace TMS.Field {
    /// <summary>
    /// フィールドのインターフェイス
    /// </summary>
    public interface IField {

        ////=============================================================================
        //// Properties
        ////
        ////=============================================================================

        /// <summary>
        /// フィールド用のレイヤーを返す
        /// </summary>
        Layer2D FieldLayer { get; }

        /// <summary>
        /// ブロック全体のインスタンス
        /// </summary>
        IReadOnlyList<BlockBase> BlockList { get; }

        ////=============================================================================
        //// Public Method
        ////
        ////=============================================================================

        /// <summary>
        /// フィールドを組み立てる
        /// </summary>
        /// <param name="maxX">Xマスの最大の大きさ</param>
        /// <param name="maxY">Yマスの最大の大きさ</param>
        /// <param name="blockSize">ブロックの1片の大きさ</param>
        /// <param name="mineNum">地雷の数</param>
        void ConstructField(int x, int y, int blockSize, int mineNum);
    }
}
