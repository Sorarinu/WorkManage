using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WorkManage
{
    public class FormDragResizer
    {
        // サイズ変更の対象となるフォーム
        private Form resizeForm;
        private Panel panel;

        /// <summary>
        /// サイズ変更の対象となる枠の位置
        /// </summary>
        public enum ResizeDirection
        {
            None = 0,
            Top = 1,
            Left = 2,
            Bottom = 4,
            Right = 8,
            All = 15
        }

        // サイズ変更が有効になる枠
        private ResizeDirection resizeDirection;

        // サイズ変更中を表す状態
        private ResizeDirection resizeStatus;

        // サイズ変更が有効になる範囲の幅
        private int resizeAreaWidth;

        // 標準のカーソル
        private Cursor defaultCursor;

        // マウスをクリックした位置
        private Point lastMouseDownPoint;

        // マウスをクリックした時点のサイズ
        private Size lastMouseDownSize;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="resizeForm">サイズ変更の対象となるフォーム</param>
        /// <param name="resizeDirection">サイズ変更が有効になる枠</param>
        /// <param name="resizeAreaWidth">サイズ変更が有効になる範囲の幅</param>
        public FormDragResizer(Form resizeForm, Panel panel, ResizeDirection resizeDirection, int resizeAreaWidth)
        {
            this.resizeForm = resizeForm;
            this.panel = panel;
            this.resizeDirection = resizeDirection;
            this.resizeAreaWidth = resizeAreaWidth;

            // 現時点でのカーソルを保存しておく
            defaultCursor = resizeForm.Cursor;

            // イベントハンドラを追加
            panel.MouseDown += new MouseEventHandler(resizeForm_MouseDown);
            panel.MouseMove += new MouseEventHandler(resizeForm_MouseMove);
            panel.MouseUp += new MouseEventHandler(resizeForm_MouseUp);
            panel.MouseLeave += new EventHandler(resizeForm_MouseLeave);
        }

        /// <summary>
        /// マウスボタン押下イベントハンドラ
        /// </summary>
        void resizeForm_MouseDown(object sender, MouseEventArgs e)
        {
            // クリックしたポイントを保存する
            lastMouseDownPoint = e.Location;

            // クリックした時点でのフォームのサイズを保存する
            lastMouseDownSize = panel.Size;

            // クリックした位置から、サイズ変更する方向を決める
            resizeStatus = ResizeDirection.None;

            // 上の判定
            if ((resizeDirection & ResizeDirection.Top) == ResizeDirection.Top)
            {
                Rectangle topRect = new Rectangle(0, 0, panel.Width, resizeAreaWidth);
                if (topRect.Contains(e.Location))
                {
                    resizeStatus |= ResizeDirection.Top;
                }
            }

            // 左の判定
            if ((resizeDirection & ResizeDirection.Left) == ResizeDirection.Left)
            {
                Rectangle leftRect = new Rectangle(0, 0, resizeAreaWidth, panel.Height);
                if (leftRect.Contains(e.Location))
                {
                    resizeStatus |= ResizeDirection.Left;
                }
            }

            // 下の判定
            if ((resizeDirection & ResizeDirection.Bottom) == ResizeDirection.Bottom)
            {
                Rectangle bottomRect = new Rectangle(0, panel.Height - resizeAreaWidth, panel.Width, resizeAreaWidth);
                if (bottomRect.Contains(e.Location))
                {
                    resizeStatus |= ResizeDirection.Bottom;
                }
            }

            // 右の判定
            if ((resizeDirection & ResizeDirection.Right) == ResizeDirection.Right)
            {
                Rectangle rightRect = new Rectangle(panel.Width - resizeAreaWidth, 0, resizeAreaWidth, panel.Height);
                if (rightRect.Contains(e.Location))
                {
                    resizeStatus |= ResizeDirection.Right;
                }
            }

            // サイズ変更の対象だったら、マウスキャプチャー
            if (resizeStatus != ResizeDirection.None)
            {
                panel.Capture = true;
            }
        }

        /// <summary>
        /// マウス移動イベントハンドラ
        /// </summary>
        void resizeForm_MouseMove(object sender, MouseEventArgs e)
        {
            // サイズ変更が有効になる枠の上にカーソルが乗ったら
            // マウスカーソルをサイズ変更用のものに変更する

            // どの枠の上にカーソルが乗っているか
            ResizeDirection cursorPos = ResizeDirection.None;

            // 上の判定
            if ((resizeDirection & ResizeDirection.Top) == ResizeDirection.Top)
            {
                Rectangle topRect = new Rectangle(0, 0, panel.Width, resizeAreaWidth);
                if (topRect.Contains(e.Location))
                {
                    cursorPos |= ResizeDirection.Top;
                }
            }

            // 左の判定
            if ((resizeDirection & ResizeDirection.Left) == ResizeDirection.Left)
            {
                Rectangle leftRect = new Rectangle(0, 0, resizeAreaWidth, panel.Height);
                if (leftRect.Contains(e.Location))
                {
                    cursorPos |= ResizeDirection.Left;
                }
            }

            // 下の判定
            if ((resizeDirection & ResizeDirection.Bottom) == ResizeDirection.Bottom)
            {
                Rectangle bottomRect = new Rectangle(0, panel.Height - resizeAreaWidth, panel.Width, resizeAreaWidth);
                if (bottomRect.Contains(e.Location))
                {
                    cursorPos |= ResizeDirection.Bottom;
                }
            }

            // 右の判定
            if ((resizeDirection & ResizeDirection.Right) == ResizeDirection.Right)
            {
                Rectangle rightRect = new Rectangle(panel.Width - resizeAreaWidth, 0, resizeAreaWidth, panel.Height);
                if (rightRect.Contains(e.Location))
                {
                    cursorPos |= ResizeDirection.Right;
                }
            }

            // カーソルを変更

            // 左上（左上から右下への斜め矢印）
            if (((cursorPos & ResizeDirection.Left) == ResizeDirection.Left)
                && ((cursorPos & ResizeDirection.Top) == ResizeDirection.Top))
            {
                panel.Cursor = Cursors.SizeNWSE;
            }
            // 右下（左上から右下への斜め矢印）
            else if (((cursorPos & ResizeDirection.Right) == ResizeDirection.Right)
                && ((cursorPos & ResizeDirection.Bottom) == ResizeDirection.Bottom))
            {
                panel.Cursor = Cursors.SizeNWSE;
            }
            // 右上（右上から左下への斜め矢印）
            else if (((cursorPos & ResizeDirection.Right) == ResizeDirection.Right)
                && ((cursorPos & ResizeDirection.Top) == ResizeDirection.Top))
            {
                panel.Cursor = Cursors.SizeNESW;
            }
            // 左下（右上から左下への斜め矢印）
            else if (((cursorPos & ResizeDirection.Left) == ResizeDirection.Left)
                && ((cursorPos & ResizeDirection.Bottom) == ResizeDirection.Bottom))
            {
                panel.Cursor = Cursors.SizeNESW;
            }
            // 上（上下矢印）
            else if ((cursorPos & ResizeDirection.Top) == ResizeDirection.Top)
            {
                panel.Cursor = Cursors.SizeNS;
            }
            // 左（左右矢印）
            else if ((cursorPos & ResizeDirection.Left) == ResizeDirection.Left)
            {
                panel.Cursor = Cursors.SizeWE;
            }
            // 下（上下矢印）
            else if ((cursorPos & ResizeDirection.Bottom) == ResizeDirection.Bottom)
            {
                panel.Cursor = Cursors.SizeNS;
            }
            // 右（左右矢印）
            else if ((cursorPos & ResizeDirection.Right) == ResizeDirection.Right)
            {
                panel.Cursor = Cursors.SizeWE;
            }
            // どこにも属していない（デフォルト）
            else
            {
                panel.Cursor = defaultCursor;
            }

            // ボタンを押していた場合は、サイズ変更を行う
            if (e.Button == MouseButtons.Left)
            {
                // ドラッグにより移動した距離を計算
                int diffX = e.X - lastMouseDownPoint.X;
                int diffY = e.Y - lastMouseDownPoint.Y;

                // 上
                if ((resizeStatus & ResizeDirection.Top) == ResizeDirection.Top)
                {
                    // まず、ドラッグした距離分だけサイズを変更する
                    // その後、フォームの位置を調整する
                    // （順番を逆にすると、ちょっとちらつく？）
                    int h = resizeForm.Height;
                    resizeForm.Height -= diffY;
                    resizeForm.Top += h - resizeForm.Height;
                }
                // 左
                if ((resizeStatus & ResizeDirection.Left) == ResizeDirection.Left)
                {
                    // 上と同じ
                    int w = resizeForm.Width;
                    resizeForm.Width -= diffX;
                    resizeForm.Left += w - resizeForm.Width;
                }
                // 下
                if ((resizeStatus & ResizeDirection.Bottom) == ResizeDirection.Bottom)
                {
                    // マウスクリックした時点のサイズを基点として、
                    // ドラッグした距離分だけサイズを変更する
                    resizeForm.Height = lastMouseDownSize.Height + diffY;
                }
                // 右
                if ((resizeStatus & ResizeDirection.Right) == ResizeDirection.Right)
                {
                    // マウスクリックした時点のサイズを基点として、
                    // ドラッグした距離分だけサイズを変更する
                    resizeForm.Width = lastMouseDownSize.Width + diffX;
                }
            }
        }

        /// <summary>
        /// マウスボタン押上イベントハンドラ
        /// </summary>
        void resizeForm_MouseUp(object sender, MouseEventArgs e)
        {
            // マウスキャプチャーを終了する
            resizeForm.Capture = false;
        }

        /// <summary>
        /// マウスどかしたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void resizeForm_MouseLeave(object sender, EventArgs e)
        {
            panel.Cursor = defaultCursor;
        }
    }
}
