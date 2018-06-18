using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Blocks;
using asd;
using TMS.UIObject;

namespace TMS.Field {
    /// <summary>
    /// フィールドを描画する機能を持つ
    /// </summary>
    public class FieldObject : IField {

        ////=============================================================================
        //// Local Field
        ////
        ////=============================================================================

        /// <summary>
        /// 一つ一つのブロックを表す
        /// </summary>
        private BlockBase[,] _blocks;

        /// <summary>
        /// 地雷を持つべき場所であるかどうか
        /// </summary>
        private bool[,] _mines;

        /// <summary>
        /// その場所の周りに地雷がいくつあるか
        /// </summary>
        private int[,] _aroundMineNum;

        /// <summary>
        /// ブロック一片の大きさ
        /// </summary>
        private int _blockSize;

        /// <summary>
        /// 地雷の数
        /// </summary>
        private int _mineNum;


        ////=============================================================================
        //// Properties
        ////
        ////=============================================================================

        /// <summary>
        /// フィールドのためのレイヤー
        /// </summary>
        public Layer2D FieldLayer { get; } = new Layer2D();

        /// <summary>
        /// ブロック全体のインスタンス
        /// </summary>
        public IReadOnlyList<BlockBase> BlockList { get; private set; }

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
        public void ConstructField(int maxX, int maxY, int blockSize, int mineNum) {
            //地雷の数とブロック1片の大きさを設定
            _blockSize = blockSize;
            _mineNum = mineNum;

            //必要なマス目分だけインスタンスを作成する
            //Note:この順番でなければエラーが出るので注意
            DecideMinePosition(maxX, maxY);
            CalcEachBlockAroundMineNum(maxX, maxY);
            MakeBlocks(maxX, maxY);
            SetNeighborBlocks(maxX, maxY);

            //作成されたブロック全体をコピーする(参照はそのまま)
            BlockList = _blocks.Cast<BlockBase>().ToList();

            //各ブロックを画面に描画する
            DrawAllBlocks();
        }

        ////=============================================================================
        //// Private Method
        ////
        ////=============================================================================

        /// <summary>
        /// 全てのブロックを開く
        /// </summary>
        private void OpenAllBlocks() {
            foreach(var block in BlockList) {
                block.CurrentState.ForceOpen(block);
            }
        }

        /// <summary>
        /// 各ブロックの、周りに存在する地雷の数を計算する
        /// </summary>
        /// <param name="x">Xマスの最大の大きさ</param>
        /// <param name="y">Yマスの最大の大きさ</param>
        private void CalcEachBlockAroundMineNum(int x, int y) {
            //周りの地雷を知るための情報を初期化
            _aroundMineNum = new int[x, y];

            //周りの地雷の数を計算
            for(int i = 0; i < x; i++) {
                for(int j = 0; j < y; j++) {
                    //周りの地雷の数を計算し、保存する
                    _aroundMineNum[i, j] = CountAroundMines(i, j, x, y);
                }
            }
        }

        /// <summary>
        /// 周りの地雷の数を計算する
        /// </summary>
        /// <param name="targetX">対象ブロックのX座標</param>
        /// <param name="targetY">対象ブロックのY座標</param>
        /// <param name="maxX">配列のX座標最大値</param>
        /// <param name="maxY">配列のY座標最大値</param>
        private int CountAroundMines(int targetX, int targetY, int maxX, int maxY) {
            //HACK:うんこコード。わかりにくすぎる。
            //Note:しかしこれ以外の書き方が思い浮かばないのでこうしている。
            int count = 0;
            if(TestArrayExistence(targetX - 1, targetY - 1, maxX, maxY)) {
                if(_mines[targetX - 1, targetY - 1]) {
                    count++;
                }
            }
            if(TestArrayExistence(targetX - 1, targetY, maxX, maxY)) {
                if(_mines[targetX - 1, targetY]) {
                    count++;
                }
            }
            if(TestArrayExistence(targetX - 1, targetY + 1, maxX, maxY)) {
                if(_mines[targetX - 1, targetY + 1]) {
                    count++;
                }
            }
            if(TestArrayExistence(targetX, targetY - 1, maxX, maxY)) {
                if(_mines[targetX, targetY - 1]) {
                    count++;
                }
            }
            if(TestArrayExistence(targetX, targetY + 1, maxX, maxY)) {
                if(_mines[targetX, targetY + 1]) {
                    count++;
                }
            }
            if(TestArrayExistence(targetX + 1, targetY - 1, maxX, maxY)) {
                if(_mines[targetX + 1, targetY - 1]) {
                    count++;
                }
            }
            if(TestArrayExistence(targetX + 1, targetY, maxX, maxY)) {
                if(_mines[targetX + 1, targetY]) {
                    count++;
                }
            }
            if(TestArrayExistence(targetX + 1, targetY + 1, maxX, maxY)) {
                if(_mines[targetX + 1, targetY + 1]) {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// 対象のブロックの周りにブロックが存在するかどうかチェック（配列の境界チェック）
        /// 主に周囲の地雷数チェックに使う
        /// </summary>
        /// <param name="targetX">対象配列のX座標</param>
        /// <param name="targetY">対象配列のY座標</param>
        /// <param name="maxX">配列のX座標最大値</param>
        /// <param name="maxY">配列のY座標最大値</param>
        private bool TestArrayExistence(int targetX, int targetY, int maxX, int maxY) {
            //値がマイナスなら即リターン
            if(targetX < 0 || targetY < 0) {
                return false;
            }

            //対象が範囲内に収まっているかどうかをチェック
            return (targetX < maxX && targetY < maxY);
        }

        /// <summary>
        /// 地雷の位置を確定させる
        /// <param name="x">Xマスの最大の大きさ</param>
        /// <param name="y">Yマスの最大の大きさ</param>
        /// </summary>
        private void DecideMinePosition(int x, int y) {
            //決定された地雷の数を初期化
            int decidedNum = 0;
            _mines = new bool[x, y];
            //乱数作成の準備
            Random rand = new Random();

            //決定された地雷の数が、設置しなければならない地雷の数に達するまでループ
            while(decidedNum != _mineNum) {
                //乱数を使って地雷の位置を決定
                var posX = rand.Next(x);
                var posY = rand.Next(y);

                //まだ地雷が設置されていない場合に限り、地雷を設置する
                if(!_mines[posX, posY]) {
                    _mines[posX, posY] = true;
                    decidedNum++;
                }
            }
        }

        /// <summary>
        /// 2次元配列のブロック要素を作成する
        /// </summary>
        /// <param name="x">横の数</param>
        /// <param name="y">縦の数</param>
        private void MakeBlocks(int x, int y) {
            //余白の設定
            int padding = 3;
            //ブロック全体の大きさを取得
            int allBlockWidth   = (_blockSize + padding) * x;
            int allBlockHeight  = (_blockSize + padding) * y;

            //画面の中心にブロックが来るようになる位置
            int baseX = (Engine.WindowSize.X / 2) - (allBlockWidth / 2);
            int baseY = (Engine.WindowSize.Y / 2) - (allBlockHeight / 2);

            //各ブロックを描画
            _blocks = new BlockBase[x, y];
            for(int i = 0; i < x; i++) {
                for(int j = 0; j < y; j++) {
                    //地雷持ちなら
                    if(_mines[i, j]) {
                        _blocks[i, j] = new StandardBlock(new ClosedState(), i * (_blockSize + padding) + baseX, j * (_blockSize + padding) + baseY, _blockSize, _aroundMineNum[i, j], true);
                    }else {
                        _blocks[i, j] = new StandardBlock(new ClosedState(), i * (_blockSize + padding) + baseX, j * (_blockSize + padding) + baseY, _blockSize, _aroundMineNum[i, j]);
                    }
                    _blocks[i, j].GameFailed += (sender, e) => {
                        OpenAllBlocks();
                        CreateRetryButton();
                    };
                }
            }
        }

        /// <summary>
        /// 隣接するブロックのリストを各ブロックに登録していく
        /// </summary>
        /// <param name="x">横の数</param>
        /// <param name="y">縦の数</param>
        private void SetNeighborBlocks(int x, int y) {
            for(int i = 0; i < x; i++) {
                for(int j = 0; j < y; j++) {
                    _blocks[i, j].RegisterNeighbor(GetNeighborList(i, j, x, y));
                }
            }
        }

        /// <summary>
        /// 隣接するブロックのリストを返す。
        /// HACK:綺麗じゃないので、使い方には十分注意。
        /// HACK:なるべくSetNeighborBlocksメソッド以外では使わない。
        /// </summary>
        /// <param name="targetX">対象ブロックのX座標</param>
        /// <param name="targetY">対象ブロックのY座標</param>
        /// <param name="maxX">配列のX座標最大値</param>
        /// <param name="maxY">配列のY座標最大値</param>
        private List<BlockBase> GetNeighborList(int targetX, int targetY, int maxX, int maxY) {
            //HACK:うんこコード。わかりにくすぎる。
            //Note:しかしこれ以外の書き方が思い浮かばないのでこうしている。
            List<BlockBase> tempList = new List<BlockBase>();
            if(TestArrayExistence(targetX - 1, targetY - 1, maxX, maxY)) {
                tempList.Add(_blocks[targetX - 1, targetY - 1]);
            }
            if(TestArrayExistence(targetX - 1, targetY, maxX, maxY)) {
                tempList.Add(_blocks[targetX - 1, targetY]);
            }
            if(TestArrayExistence(targetX - 1, targetY + 1, maxX, maxY)) {
                tempList.Add(_blocks[targetX - 1, targetY + 1]);
            }
            if(TestArrayExistence(targetX, targetY - 1, maxX, maxY)) {
                tempList.Add(_blocks[targetX, targetY - 1]);
            }
            if(TestArrayExistence(targetX, targetY + 1, maxX, maxY)) {
                tempList.Add(_blocks[targetX, targetY + 1]);
            }
            if(TestArrayExistence(targetX + 1, targetY - 1, maxX, maxY)) {
                tempList.Add(_blocks[targetX + 1, targetY - 1]);
            }
            if(TestArrayExistence(targetX + 1, targetY, maxX, maxY)) {
                tempList.Add(_blocks[targetX + 1, targetY]);
            }
            if(TestArrayExistence(targetX + 1, targetY + 1, maxX, maxY)) {
                tempList.Add(_blocks[targetX + 1, targetY + 1]);
            }

            return tempList;
        }

        /// <summary>
        /// 画面に全てのブロックを描画する
        /// </summary>
        private void DrawAllBlocks() {
            foreach(var block in _blocks) {
                FieldLayer.AddObject(block);
            }
        }

        /// <summary>
        /// リトライボタンを作成する。
        /// </summary>
        private void CreateRetryButton() {
            var textObj = new UITextureObject2D();
            textObj.Texture = Engine.Graphics.CreateTexture2D("Resources/Retry.png");
            textObj.Position = new Vector2DF(230, 30);//Note:位置は適当。
            textObj.Color = new Color(255, 255, 255, 200);//Note:色を不透明にすると、後ろのブロックが見えなくなるので少し透明にしている。
            textObj.Clicked += (sender, e) => {
                //クリックされたらメインシーンへ飛ばし、再びゲームができるようにする
                MainGameScene.MainScene scene = new MainGameScene.MainScene();
                Engine.ChangeSceneWithTransition(scene, new TransitionFade(0.2f, 0.2f));
            };
            FieldLayer.AddObject(textObj);
        }
    }
}
