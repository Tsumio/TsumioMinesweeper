using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;

namespace TMS.Blocks {
    /// <summary>
    /// 数字か地雷か空白が見えているブロック
    /// </summary>
    public class OpenedState : IBlockState {

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

        public OpenedState(BlockBase target) {
            InitializeFont(target);
        }

        /// <summary>
        /// フォントの初期化
        /// </summary>
        /// <param name="target">対象ブロック</param>
        private void InitializeFont(BlockBase target) {
            //文字の作成
            _textObj.Font = Engine.Graphics.CreateDynamicFont("", target.BlockSize / 2, new Color(255, 255, 255), 1, new Color(0, 0, 0));
            _textObj.Text = GetProperText(target);
            _textObj.Color = GetProperColor(target);

            //文字位置の調整
            var xCorrect      = _textObj.Font.CalcTextureSize(_textObj.Text, WritingDirection.Horizontal).X/2;
            _textObj.Position = new Vector2DF(target.XPosition + xCorrect, target.YPosition);

            //レイヤーへ追加
            target.Layer.AddObject(_textObj);
        }

        ////=============================================================================
        //// Public Method
        ////
        ////=============================================================================

        /// <summary>
        /// 通常の表示
        /// </summary>
        /// <param name="target">対象のブロック</param>
        public void Draw(BlockBase target) {
            var rect = MakeRectShape(target);
            target.Shape = rect;
            //地雷を空けたら少し赤くして、他は緑色
            if(target.HasMine) {
                target.Color = new Color(255, 100, 100);
            } else {
                target.Color = new Color(100, 255, 100);
            }
        }

        /// <summary>
        /// クリックされてもやることはない
        /// </summary>
        /// <param name="target">対象のブロック</param>
        public void OnClicked(BlockBase target) {
            //特にすることはない
        }

        /// <summary>
        /// オープンされたものは色を濃く
        /// </summary>
        /// <param name="target">対象のブロック</param>
        public void OnMouseOver(BlockBase target) {
            var rect = MakeRectShape(target);
            target.Shape = rect;
            //地雷を空けたら少し赤くして、他は緑色
            if(target.HasMine) {
                target.Color = new Color(255, 50, 50);
            } else {
                target.Color = new Color(100, 150, 100);
            }
        }

        /// <summary>
        /// オープンされているので旗をたてる意味がない
        /// </summary>
        /// <param name="target">対象のブロック</param>
        public void OnRightClicked(BlockBase target) {
            //特にすることはない
        }

        /// <summary>
        /// すでにオープンになっている
        /// </summary>
        /// <param name="target">対象ブロック</param>
        public void TryOpen(BlockBase target) {
            //特にすることはない
        }

        /// <summary>
        /// すでにオープンなので何もしない
        /// </summary>
        /// <param name="target">対象ブロック</param>
        public void ForceOpen(BlockBase target) {
            //特に処理はない
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

        /// <summary>
        /// ブロックにセットする文字列を取得する
        /// </summary>
        /// <param name="target">対象ブロック</param>
        /// <returns></returns>
        private string GetProperText(BlockBase target) {
            //地雷持ちなら即M(Mine)をリターン
            if(target.HasMine) {
                return "M";
            }

            //周りに地雷がなければ空文字を返し、それ以外は周りの地雷数を返す
            return (target.AroundMinesNum == 0) ? "" : target.AroundMinesNum.ToString();
        }

        /// <summary>
        /// 周りの地雷数によって色を変える
        /// </summary>
        /// <param name="target">対象ブロック</param>
        private Color GetProperColor(BlockBase target) {
            //地雷持ちは赤色にする
            if(target.HasMine) {
                return new Color(255, 0, 0);
            }

            //周囲の地雷数によって色を変える
            switch(target.AroundMinesNum) {
                case 1 :
                    return new Color(0, 0, 255);
                case 2 :
                    return new Color(0, 100, 0);
                case 3 :
                    return new Color(255, 0, 0);
                case 4 :
                    return new Color(0, 0, 150);
                case 5 :
                    return new Color(150, 0, 0);
                case 6 :
                    return new Color(0, 0, 150);
                case 7 :
                    return new Color(0, 0, 0);
                case 8 :
                    return new Color(150, 150, 150);
                default :
                    return new Color(255, 255, 255);
            }
        }
    }
}
