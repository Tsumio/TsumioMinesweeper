using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;
using TMS.Field;
using TMS.Blocks;

namespace TMS.MainGameScene {
    /// <summary>
    /// 実際にマインスイーパーが行われるシーン。
    /// クリア処理もここでおこなう。
    /// </summary>
    public class MainScene : Scene {

        ////=============================================================================
        //// Local Field
        ////
        ////=============================================================================

        /// <summary>
        /// 現在のフィールド
        /// </summary>
        private IField _field;

        /// <summary>
        /// フレームのカウンター。これを使って、定期的にクリア状態をチェックする。
        /// </summary>
        private int _frameCount;

        ////=============================================================================
        //// Propetries
        ////
        ////=============================================================================

        /// <summary>
        /// クリアしているかどうかをチェックすべきかどうか
        /// </summary>
        private bool ShouldCheck => _frameCount % 60 == 0;

        ////=============================================================================
        //// コンストラクタ
        ////
        ////=============================================================================

        protected override void OnRegistered() {
            _field = new FieldObject();
            ILevelSelecter selecter = new LevelSelecter(_field);
            AddLayer(_field.FieldLayer);
            AddLayer(selecter.LevelLayer);
        }

        ////=============================================================================
        //// Protected Method
        ////
        ////=============================================================================

        protected override void OnUpdated() {
            base.OnUpdated();

            CheckCompleteGame();
            UpdateCounter();
        }

        ////=============================================================================
        //// Private Method
        ////
        ////============================================================================

        /// <summary>
        /// フレームカウンターを更新しているだけ。
        /// </summary>
        private void UpdateCounter() {
            _frameCount++;
        }

        /// <summary>
        /// クリア状態であるかどうかを返す。
        /// ブロックリストを走査して、地雷以外の全てのブロックが開いていてかつ、全ての地雷が開いていないことを満たせばクリアとみなす。
        /// </summary>
        private bool IsCompleted() {
            bool isAllBlockOpened = _field.BlockList
                .Where(b => !b.HasMine)
                .All(b => b.CurrentState.GetType() == typeof(OpenedState));
            bool isAllMineClosed = _field.BlockList
                .Where(b => b.HasMine)
                .All(b => b.CurrentState.GetType() != typeof(OpenedState));
            return isAllBlockOpened && isAllMineClosed;
        }

        /// <summary>
        /// ゲームをクリアしたかどうかをチェックする。
        /// クリアしていた場合、リトライボタンとクリア画像を表示する。
        /// Note:全体的に汚いし、特定のフレーム毎にチェックするのも無駄な処理。できれば改善したい。
        /// </summary>
        private void CheckCompleteGame() {
            //ブロックリストがNullならば、そもそもゲームが始まっていないので即リターン
            //Note:この処理をしないと、Nullオブジェクトにアクセスしようとして例外エラーが出るので注意。
            if(_field.BlockList == null) {
                return;
            }

            //チェックすべきでない状態ならチェックしない
            if(!ShouldCheck) {
                return;
            }

            //実際のクリア処理
            if(IsCompleted()) {
                CompletedGame.CompletedScene completedScene = new CompletedGame.CompletedScene();
                Engine.ChangeSceneWithTransition(completedScene, new TransitionFade(1.0f, 1.0f));
                Dispose();
            }
        }
    }
}
