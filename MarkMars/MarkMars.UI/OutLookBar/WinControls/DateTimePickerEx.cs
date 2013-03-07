using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MarkMars.UI.OutLookBar.WinControls
{
    public partial class DateTimePickerEx : DateTimePicker
    {
        private DateTime currentInputDateTime = DateTime.Now; //当前输入的时间
        private Int32 needCancelValueChangedEventTimes = 0; //需要取消 ValueChanged 事件次数
        private Int32 length = 0; //当前输入位置要输入的长度
        private Boolean isEnd = false; //当前输入位置是否最后一个位置

        public DateTimePickerEx()
        {
            InitializeComponent();
            this.ImeMode = ImeMode.Disable;
        }

        #region 私有方法
        /// <summary>
        /// 设置要输入的长度。
        /// </summary>
        private void SetLength()
        {
            if (this.Format != DateTimePickerFormat.Custom)
            {
                return;
            }

            this.currentInputDateTime = this.Value;
            this.needCancelValueChangedEventTimes += 2;

            if (this.Value.AddYears(1) > this.MaxDate)
            {
                DateTime currentMinDateTime = this.MinDate;
                this.MinDate = this.Value.AddYears(-1);
                SendKeys.Send("{DOWN}");
                SendKeys.Send("{UP}");
                this.MinDate = currentMinDateTime;
            }
            else
            {
                SendKeys.Send("{UP}");
                SendKeys.Send("{DOWN}");
            }
        }
        #endregion

        #region 重载事件
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.SetLength();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.SetLength();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (this.Format != DateTimePickerFormat.Custom)
            {
                return;
            }

            if ((e.KeyValue >= Convert.ToInt32(Keys.D0) && e.KeyValue <= Convert.ToInt32(Keys.D9))
                || (e.KeyValue >= Convert.ToInt32(Keys.NumPad0) && e.KeyValue <= Convert.ToInt32(Keys.NumPad9)))
            {
                this.length--;
                if (this.length == 0)
                {
                    if (!this.isEnd)
                    {
                        SendKeys.Send("{RIGHT}");
                    }

                    this.SetLength();
                }
            }
        }

        protected override void OnValueChanged(EventArgs eventargs)
        {
            if (this.needCancelValueChangedEventTimes == 0)
            {
                base.OnValueChanged(eventargs);
            }

            if (this.Format != DateTimePickerFormat.Custom)
            {
                return;
            }

            Regex reg = null;
            if (this.Value.Year != this.currentInputDateTime.Year)
            {
                reg = new Regex("(y+[^yMdHhms]*)");
                length = 4;
            }
            else if (this.Value.Month != this.currentInputDateTime.Month)
            {
                reg = new Regex("(M+[^yMdHhms]*)");
                length = 2;
            }
            else if (this.Value.Day != this.currentInputDateTime.Day)
            {
                reg = new Regex("(d+[^yMdHhms]*)");
                length = 2;
            }
            else if (this.Value.Hour != this.currentInputDateTime.Hour)
            {
                reg = new Regex("([Hh]+[^yMdHhms]*)");
                length = 2;
            }
            else if (this.Value.Minute != this.currentInputDateTime.Minute)
            {
                reg = new Regex("(m+[^yMdHhms]*)");
                length = 2;
            }
            else if (this.Value.Second != this.currentInputDateTime.Second)
            {
                reg = new Regex("(s+[^yMdHhms]*)");
                length = 2;
            }

            if (!String.IsNullOrEmpty(this.CustomFormat) && reg != null && reg.IsMatch(this.CustomFormat))
            {
                String matchValue = reg.Match(this.CustomFormat).Groups[1].Value;
                this.isEnd = this.CustomFormat.IndexOf(matchValue) + matchValue.Length == this.CustomFormat.Length;
            }

            this.currentInputDateTime = this.Value;

            if (this.needCancelValueChangedEventTimes > 0)
            {
                this.needCancelValueChangedEventTimes--;
            }
        }
        #endregion
    }
}
