using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace B站视频历史评论删除工具
{
    public partial class TaskFindComments : Form
    {
        /// <summary>
        ///  初始化。
        /// </summary>
        public TaskFindComments()
        {
            InitializeComponent();
            // 为数组赋初始值。
            _allRichTextBox[0] = richTextBox1;
            _allRichTextBox[1] = richTextBox2;
            _allRichTextBox[2] = richTextBox3;
            _allRichTextBox[3] = richTextBox4;
            _allRichTextBox[4] = richTextBox5;
            _allLabels[0] = label1;
            _allLabels[1] = label2;
            _allLabels[2] = label3;
            _allLabels[3] = label4;
            _allLabels[4] = label5;
            try
            {
                for (int i = 0; i < _allLabels.Length; i++)
                {
                    _allLabels[i].Text = _numbers[i].ToString();
                }
            }
            catch (Exception e)
            {
                _allRichTextBox[0].AppendText(e.Message);
                _allRichTextBox[0].SelectedText = e.Message;
                _allRichTextBox[0].SelectionColor = Color.Red;
            }

            // 循环获取ric列表废弃不用。
            #region 临时废弃。
            //try
            //{
            //    foreach (var item in splitContainer1.Panel2.Controls)
            //    {
            //        list.Add(((RichTextBox)item));
            //    }
            //    try
            //    {
            //        list.Reverse();
            //    }
            //    catch (Exception e)
            //    {

            //        MessageBox.Show($"ric列表反转错误\r\n{e}");
            //    }

            //}
            //catch (Exception e)
            //{

            //    MessageBox.Show($"获取ric列表错误\r\n{e}");
            //}
            #endregion


        }
        /// <summary>
        ///  各ric列表计数用。
        /// </summary>
        int[] _numbers = new int[] { 0, 0, 0, 0, 0 };
        /// <summary>
        ///  存放所有的ric。
        /// </summary>
        RichTextBox[] _allRichTextBox = new RichTextBox[5];
        /// <summary>
        ///  存放所有的lable。
        /// </summary>
        Label[] _allLabels = new Label[5];
        /// <summary>
        ///  锁，锁住send方法。
        /// </summary>
        private readonly object _lookSend = new object();
        /// <summary>
        ///  锁，锁住toRicPull方法。
        /// </summary>
        private readonly object _lookReceive = new object();
        /// <summary>
        ///  向各个ric列表填充内容。
        /// </summary>
        /// <param name="i">向哪一个填充。</param>
        /// <param name="contents">内容。</param>
        /// <param name="isNotCheck">是否开启列表重复值检查。默认false</param>
        /// <returns>成功true，失败false。</returns>
        public void ToRicPull(int i, string contents, bool isNotCheck = false)
        {
            lock (_lookReceive)
            {
                if (i < 5)
                {
                    try
                    {
                        SendToRic(i, contents);
                        #region 屏蔽。
                        // 屏蔽 暂时不判断。
                        //// 判断是否包含。
                        //if (isNotCheck)
                        //{
                        //    SendToRic(i, contents);
                        //    /* for (int j = 0; j < 5; j++)
                        //     {
                        //         if (j == i)
                        //         {
                        //             SendToRic(i, contents);
                        //         }
                        //         try
                        //         {
                        //             Task.Run(new Action(() =>
                        //             {  // 重复项置为红色。
                        //                 SendToRic(j, contents, false, true);
                        //                 SendToRic(i, contents, false, true);
                        //             }));
                        //             Thread.Sleep(100);

                        //         }
                        //         catch (Exception e)
                        //         {

                        //             SendToRic(0, $"推送错误3:{e.Message}", true);
                        //         }

                        //     }*/
                        //}
                        //else
                        //{
                        //    // 推送消息。
                        //    SendToRic(i, contents);
                        //}
                        #endregion
                       // return true;
                    }
                    catch (Exception e)
                    {
                        // 推送消息。
                        SendToRic(0, $"推送错误4:{e.Message}", true);
                       // return false;
                    }

                }
                else
                {
                    // 推送消息。
                    SendToRic(0, "给定值超出数组索引界限。", true);
                 //   return false;
                }
            }

        }
        /// <summary>
        ///  向指定Rich委托推送。
        /// </summary>
        /// <param name="i">那个列表。</param>
        /// <param name="contents">内容。</param>
        /// <param name="isNotError">是否有错误。</param>
        /// <param name="isNotCheck">是否是检查错误。 Is or not check error。 </param>
        public void SendToRic(int i, string contents, bool isNotError = false, bool isNotCheck = false)
        {
            // 任务委托推送。
            lock (_lookSend)
            {
                this.Invoke(new Action(() =>
                {
                    try
                    {
                        if (isNotCheck == false || isNotError == true)
                        {
                            _allRichTextBox[i].AppendText(contents + "\r\n");
                            // 追加oid数量。
                            if (isNotError == false && isNotCheck == false)
                            {
                                // 计数器加。
                                _allLabels[i].Text = (++_numbers[i]).ToString();
                               
                            }
                        }
                        // 是错误或检查。
                        // This is only open check or error, ok into。
                        if (isNotCheck || isNotError)
                        {
                            // 选定文本。
                            // This Ric select Text is contents.
                            if (_allRichTextBox[i].Find(contents) != -1)
                            {
                                try
                                {
                                    // 置为红色。
                                    // Will selected comments set up red because this is error or check repeat item.
                                    _allRichTextBox[i].SelectionColor = Color.Red;
                                    // 在这个列表中选中评论。
                                    // At this list in select comments.
                                    _allRichTextBox[i].SelectedText = contents;
                                    // 置为黑色。
                                    // Will selected comments set up black because this is error or check repeat item.
                                    _allRichTextBox[i].SelectionColor = Color.Black;
                                }
                                catch (Exception e)
                                {

                                    SendToRic(0, $"推送错误5:{e.Message}+\r\n", true);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {

                        SendToRic(0, $"推送错误1:{e.Message}+\r\n", true);
                    }
                }
                ));
            }
            // 延迟。
            Thread.Sleep(100);

        }
        /// <summary>
        ///  返回指定列表。
        /// </summary>
        /// <param name="i">要的列表。</param>
        /// <returns>返回指定列表。</returns>
        public RichTextBox GetRichTextBox(int i)
        {
            return _allRichTextBox[i];
        }
    }
}
