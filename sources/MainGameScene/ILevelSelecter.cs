using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;

namespace TMS.MainGameScene {
    /// <summary>
    /// レベルを選択するクラスのためのインターフェイス。
    /// 今回はLevelSelecter.cs以外で実装しないかも。
    /// </summary>
    interface ILevelSelecter {

        ////=============================================================================
        //// Properties
        ////
        ////=============================================================================

        /// <summary>
        /// レベル選択機能のための画像を登録するレイヤー
        /// </summary>
        Layer2D LevelLayer { get; }
    }
}
