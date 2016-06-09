using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WorkManage
{
    public class FormDragMover
    {
        // 移動の対象となるパネル・フォーム
        private Panel movePanel;
        private Form moveForm;

        // 移動中を表す状態
        private bool moveStatus;

        // ドラッグを無効とする幅（フォームの端をサイズ変更に使うときなど）
        private int noDragAreaWidth;

        // 標準のカーソル
        private Cursor defaultCursor;

        // マウスをクリックした位置
        private Point lastMouseDownPoint;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="moveForm">移動の対象となるフォーム</param>
        /// <param name="noDragAreaWidth">ドラッグを無効とする幅</param>
        public FormDragMover(Panel movePanel, Form moveForm, int noDragAreaWidth)
        {
            this.moveForm = moveForm;
            this.movePanel = movePanel;
            this.noDragAreaWidth = noDragAreaWidth;

            // 現時点でのカーソルを保存しておく
            defaultCursor = movePanel.Cursor;

            // イベントハンドラを追加
            movePanel.MouseDown += new MouseEventHandler(moveForm_MouseDown);
            movePanel.MouseMove += new MouseEventHandler(moveForm_MouseMove);
            movePanel.MouseUp += new MouseEventHandler(moveForm_MouseUp);
        }

        /// <summary>
        /// マウスボタン押下イベントハンドラ
        /// </summary>
        void moveForm_MouseDown(object sender, MouseEventArgs e)
        {
            // 左クリック時のみ処理する。左クリックでなければ何もしない
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left) return;

            // 移動が有効になる範囲
            // 例えばフォームの端から何ドットかをサイズ変更用の領域として使用する場合、
            // そこを避けるために使う。
            Rectangle moveArea = new Rectangle(
                noDragAreaWidth, noDragAreaWidth,
                movePanel.Width - (noDragAreaWidth * 2), movePanel.Height - (noDragAreaWidth * 2));

            // クリックした位置が移動が有効になる範囲であれば、移動中にする
            if (moveArea.Contains(e.Location))
            {
                // 移動中にする
                moveStatus = true;

                // クリックしたポイントを保存する
                lastMouseDownPoint = e.Location;

                // マウスキャプチャー
                movePanel.Capture = true;
            }
            else
            {
                moveStatus = false;
            }
        }

        /// <summary>
        /// マウス移動イベントハンドラ
        /// </summary>
        void moveForm_MouseMove(object sender, MouseEventArgs e)
        {
            // 移動中の場合のみ処理。移動中でなければ何もせず終わる
            if (moveStatus == false) return;

            // 左ボタン押下中のみ処理する。押下中ではないときは何もしない。
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left) return;

            // フォームの移動
            //*//通常の場合
            moveForm.Left += e.X - lastMouseDownPoint.X;
            moveForm.Top += e.Y - lastMouseDownPoint.Y;
            //*/

            // 吸着の処理は後回し
        }

        /// <summary>
        /// マウスボタン押上イベントハンドラ
        /// </summary>
        void moveForm_MouseUp(object sender, MouseEventArgs e)
        {
            // 左ボタンのみ処理する。左ボタンではないときは何もしない。
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left) return;

            // 移動を終了する
            moveStatus = false;

            // マウスキャプチャーを終了する
            movePanel.Capture = false;

            // マウスカーソルを戻す
            movePanel.Cursor = defaultCursor;
        }

    }
}
