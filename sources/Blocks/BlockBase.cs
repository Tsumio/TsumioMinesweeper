using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;
using TMS.UIObject;

namespace TMS.Blocks {
    /// <summary>
    /// ブロックそのもの
    /// </summary>
    public abstract class BlockBase : GeometryObject2D {

        ////=============================================================================
        //// Local Field
        ////
        ////=============================================================================

        /// <summary>
        /// 周囲のブロックを保持するリスト
        /// </summary>
        private List<BlockBase> _neighborBlockList;


        ////=============================================================================
        //// Events
        ////
        ////=============================================================================

        /// <summary>
        /// 地雷をクリックしたときに発火するイベント
        /// </summary>
        public event EventHandler<EventArgs> GameFailed;

        ////=============================================================================
        //// Properties
        ////
        ////=============================================================================

        /// <summary>
        /// 地雷を保持しているかどうか
        /// </summary>
        public bool HasMine { get; }

        /// <summary>
        /// 周りに存在する地雷数
        /// </summary>
        public int AroundMinesNum { get; }

        /// <summary>
        /// 現在の状態を表す
        /// </summary>
        public IBlockState CurrentState { get; private set; }

        /// <summary>
        /// ブロックのX座標
        /// </summary>
        public int XPosition { get; }

        /// <summary>
        /// ブロックのY座標
        /// </summary>
        public int YPosition { get; }

        /// <summary>
        /// ブロックのサイズ
        /// </summary>
        public int BlockSize { get; }

        /// <summary>
        /// 周囲のブロックを保持しているインスタンス。
        /// HACK:もっとうまい感じに隣接するブロックを取得できそうなら、その方法に変えたい。
        /// </summary>
        public IReadOnlyList<BlockBase> NeighborBlockList {
            get {
                return _neighborBlockList;
            }
        }

        ////=============================================================================
        //// コンストラクタ
        ////
        ////=============================================================================

        /// <summary>
        /// 基本的なコンストラクタ
        /// </summary>
        /// <param name="state">初期ステート</param>
        /// <param name="parentLayer">親レイヤーへの参照</param>
        /// <param name="x">ブロックのX座標</param>
        /// <param name="y">ブロックのY座標</param>
        /// <param name="blockSize">ブロックのサイズ</param>
        /// <param name="aroundMineNum">周りの地雷の数</param>
        /// <param name="hasMine">地雷を保持すべきかどうか</param>
        public BlockBase(IBlockState state, int x, int y, int blockSize, int aroundMineNum, bool hasMine = false) {
            XPosition = x;
            YPosition = y;
            BlockSize = blockSize;
            HasMine = hasMine;
            AroundMinesNum = aroundMineNum;
            ChangeState(state);
        }

        ////=============================================================================
        //// Publc Method
        ////
        ////=============================================================================

        /// <summary>
        /// 状態を変更する
        /// </summary>
        /// <param name="nextState">次の状態</param>
        public void ChangeState(IBlockState nextState) {
            CurrentState = nextState;
            Refresh();
        }

        /// <summary>
        /// 隣接するブロックを登録する。
        /// HACK:このメソッド自体は安全ではなく、使い方によってはバグの温床となる。メソッドの使用には気をつけたい。
        /// HACK:必要がなければFieldObject.cs以外では使わない。
        /// </summary>
        /// <param name="blockList"></param>
        public void RegisterNeighbor(List<BlockBase> blockList) {
            _neighborBlockList = blockList;
        }

        /// <summary>
        /// 地雷を踏んだときに発火
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnGameFailed(EventArgs e) {
            GameFailed?.Invoke(this, e);
        }

        ////=============================================================================
        //// Private Method
        ////
        ////=============================================================================

        /// <summary>
        /// 再描画する
        /// </summary>
        private void Refresh() {
            CurrentState.Draw(this);
        }
    }
}
