using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;
using TMS.Field;

namespace TMS.MainGameScene {
    /// <summary>
    /// 難易度選択の機能を持つクラス
    /// Note:クラスの責務が大きいし、全体的にコードも汚い。
    /// Note:例えば、汎用的なWindowクラスを作ると改善されそう。
    /// Note:あるいは、UITextureObject2Dを作ったので、せめてそれと差し替えるとか。
    /// </summary>
    class LevelSelecter : TextureObject2D, ILevelSelecter {

        ////=============================================================================
        //// Local Field
        ////
        ////=============================================================================

        /// <summary>
        /// 今回使用するフィールド
        /// </summary>
        private IField _field;

        /// <summary>
        /// 各レベルのテクスチャを保持しているディクショナリ
        /// </summary>
        private IDictionary<string, TextureObject2D> _levelTextureDict;

        /// <summary>
        /// EASYモードを表す文字列
        /// </summary>
        private readonly string EASY_MODE = "Easy";

        /// <summary>
        /// Normalモードを表す文字列
        /// </summary>
        private readonly string NORMAL_MODE = "Normal";

        /// <summary>
        /// Hardモードを表す文字列
        /// </summary>
        private readonly string HARD_MODE   = "Hard";

        /// <summary>
        /// VeryHardモードを表す文字列
        /// </summary>
        private readonly string VERY_HARD_MODE = "VeryHard";

        ////=============================================================================
        //// Properties
        ////
        ////=============================================================================

        /// <summary>
        /// 難易度選択のためのレイヤー
        /// </summary>
        public Layer2D LevelLayer { get; } = new Layer2D();

        ////=============================================================================
        //// コンストラクタ
        ////
        ////=============================================================================

        /// <summary>
        /// 基本コンストラクタ
        /// </summary>
        /// <param name="field">構築したいフィールドのインスタンス</param>
        public LevelSelecter(IField field) {
            _field = field;
            LevelLayer.AddObject(this);//Note:自分自身を登録しないと、OnUpdateメソッドが実行されない

            InitializeWindowObject(180, 70);//Note:位置は適当
        }

        /// <summary>
        /// ウィンドウの外側と中身を初期化して描画する
        /// </summary>
        private void InitializeWindowObject(int x, int y) {
            //Note:この順番に実行する必要がある
            DrawSelectWindow(x, y);
            RegisterEachLevelTexture();
            DrawEachLevel();
            SetPositionEachLeve(x, y);
        }


        ////=============================================================================
        //// ASD
        ////
        ////=============================================================================

        protected override void OnUpdate() {
            base.OnUpdate();
            UpdateClick();
        }

        /// <summary>
        /// このオブジェクトを解放するとき、レイヤーごと解放
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();
            LevelLayer.Dispose();
        }

        /// <summary>
        /// マウスの左クリック状態をチェックする
        /// </summary>
        private void UpdateClick() {
            //マウスをクリックしたのでなければ即リターン
            if(Engine.Mouse.LeftButton.ButtonState != MouseButtonState.Push) {
                return;
            }

            //HACK:汚い
            if(IsHit(_levelTextureDict[EASY_MODE])) {
                SelectEasyMode();
            } else if(IsHit(_levelTextureDict[NORMAL_MODE])) {
                SelectNormalMode();
            } else if(IsHit(_levelTextureDict[HARD_MODE])) {
                SelectHardMode();
            } else if(IsHit(_levelTextureDict[VERY_HARD_MODE])) {
                SelectVeryHardMode();
            }
        }

        ////=============================================================================
        //// Private Method
        ////
        ////=============================================================================

        /// <summary>
        /// 各レベルの画像のテクスチャをディクショナリに登録する
        /// </summary>
        private void RegisterEachLevelTexture() {
            //Nullでなければ、すでに登録済みのはず
            if(_levelTextureDict != null) {
                return;
            }
            _levelTextureDict = new Dictionary<string, TextureObject2D>();

            //HACK:インスタンスを登録していっているが、もっとうまい追加方法ないか？変更に弱い。
            _levelTextureDict.Add(EASY_MODE     , new TextureObject2D());
            _levelTextureDict.Add(NORMAL_MODE   , new TextureObject2D());
            _levelTextureDict.Add(HARD_MODE     , new TextureObject2D());
            _levelTextureDict.Add(VERY_HARD_MODE, new TextureObject2D());

            //各インスタンスに画像をいったんロード
            foreach(var texture in _levelTextureDict) {
                texture.Value.Texture = Engine.Graphics.CreateTexture2D("Resources/Level.png");
            }
        }
        
        /// <summary>
        /// 選択ウィンドウを描画
        /// </summary>
        private void DrawSelectWindow(int x, int y) {
            //Window用のインスタンスを作成
            TextureObject2D window = new TextureObject2D();
            window.Texture = Engine.Graphics.CreateTexture2D("Resources/Window.png");
            //位置調整
            window.Position = new Vector2DF(x, y);

            //レイヤーに登録
            LevelLayer.AddObject(window);
        }

        /// <summary>
        /// 各レベルを描画
        /// </summary>
        private void DrawEachLevel() {
            //各レベルにそって、適切に画像を切り取る。
            //HACK:これも変更に弱い。例えば画像のサイズが変わったら、それだけでアウト。実際のゲーム制作では画像の規格を決めておいたほうがいい？
            int width  = 160;
            int height = 40;
            _levelTextureDict[EASY_MODE].Src      = new RectF(0, 0, width, height);
            _levelTextureDict[NORMAL_MODE].Src    = new RectF(0, height*1, width, height);
            _levelTextureDict[HARD_MODE].Src      = new RectF(0, height*2, width, height);
            _levelTextureDict[VERY_HARD_MODE].Src = new RectF(0, height*3, width, height);

            //各インスタンスをレイヤーに登録
            foreach(var texture in _levelTextureDict) {
                LevelLayer.AddObject(texture.Value);
            }
        }

        /// <summary>
        /// 各レベルの位置を設定する
        /// </summary>
        private void SetPositionEachLeve(int x, int y) {
            int basePosX = 35;
            int basePosY = 50;
            _levelTextureDict[EASY_MODE].Position      = new Vector2DF(x + basePosX, y + basePosY*1);
            _levelTextureDict[NORMAL_MODE].Position    = new Vector2DF(x + basePosX, y + basePosY*2);
            _levelTextureDict[HARD_MODE].Position      = new Vector2DF(x + basePosX, y + basePosY*3);
            _levelTextureDict[VERY_HARD_MODE].Position = new Vector2DF(x + basePosX, y + basePosY*4);
        }

        /// <summary>
        /// 対象オブジェクトがクリックされたかどうかをチェック
        /// </summary>
        /// <param name="obj">対象オブジェクト</param>
        /// <returns></returns>
        private bool IsHit(TextureObject2D obj) {
            //当たり判定の計算
            if(obj.Position.Y > Engine.Mouse.Position.Y || obj.Position.Y + obj.Src.Height < Engine.Mouse.Position.Y ||
                obj.Position.X > Engine.Mouse.Position.X || obj.Position.X + obj.Src.Width < Engine.Mouse.Position.X) {
                return false;
            }

            return true;
        }


        ////=============================================================================
        //// Private Method
        ////  モード選択用のメソッド
        ////=============================================================================

        /// <summary>
        /// 簡単モードを選択する
        /// </summary>
        private void SelectEasyMode() {
            _field.ConstructField(6, 6, 50, 4);
            Dispose();
        }

        /// <summary>
        /// 普通のモードを選択する
        /// </summary>
        private void SelectNormalMode() {
            _field.ConstructField(10, 10, 40, 20);
            Dispose();
        }

        /// <summary>
        /// 難しいモードを選択する
        /// </summary>
        private void SelectHardMode() {
            _field.ConstructField(16, 16, 25, 40);
            Dispose();
        }

        /// <summary>
        /// 激むずモードを選択する
        /// </summary>
        private void SelectVeryHardMode() {
            _field.ConstructField(30, 20, 18, 130);
            Dispose();
        }
    }
}
