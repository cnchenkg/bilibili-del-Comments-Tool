using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace B站视频历史评论删除工具
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        ///  存放带评论的oid。
        /// </summary>
        Dictionary<string, Dictionary<string, string>> findContainCommentsIsOid = new Dictionary<string, Dictionary<string, string>>();
        /// <summary>
        ///  存放所有获取到的oid账号。
        /// </summary>
        List<string> tempAllOid = new List<string>();
        /// <summary>
        ///  tfc 查找任务窗口。
        /// </summary>
        TaskFindComments tfc = new TaskFindComments();
        /// <summary>
        ///  tfc 窗口ric富文本数量。
        /// </summary>
        RichTextBox[] richTexts = new RichTextBox[5];
        /// <summary>
        ///  为propertiesName设置键值对。
        /// </summary>
        Dictionary<TextBox, string> propertiesName = new Dictionary<TextBox, string>();
        /// <summary>
        ///  oid选择。
        /// </summary>
        /// <param name="sender">触发者。</param>
        /// <param name="e">事件。</param>
        private void SelectOidGetComments(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                if (findContainCommentsIsOid.ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    rpidList.DataSource = findContainCommentsIsOid[listBox1.SelectedItem.ToString()].Keys.ToArray();
                }
            }
        }
        /// <summary>
        ///  根据评论号获取评论。
        /// </summary>
        /// <param name="sender">触发者。</param>
        /// <param name="e">事件。</param>
        private void SelectRpid(object sender, EventArgs e)
        {
            if (rpidList.SelectedItem != null)
            {
                textComments.Clear();
                if (findContainCommentsIsOid.ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    if (findContainCommentsIsOid[listBox1.SelectedItem.ToString()].ContainsKey(rpidList.SelectedItem.ToString()))
                    {
                        textComments.Text = findContainCommentsIsOid[listBox1.SelectedItem.ToString()][rpidList.SelectedItem.ToString()];
                    }
                }
            }
        }
        /// <summary>
        ///  显示删除键。
        /// </summary>
        /// <param name="sender">触发者。</param>
        /// <param name="e">事件。</param>
        private void TextComment(object sender, EventArgs e)
        {
            if (textComments.Text.Length > 0)
            {
                buttonDel.Enabled = true;

            }
            else
            {
                buttonDel.Enabled = false;

            }
        }
        /// <summary>
        ///  数据发生变化。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataSoureChange(object sender, EventArgs e)
        {
            textComments.Clear();
        }
        /// <summary>
        ///  更新list列表。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpListOid(object sender, EventArgs e)
        {
            listBox1.DataSource = findContainCommentsIsOid.Keys.ToArray();
        }
        /// <summary>
        ///  加载评论。
        /// </summary>
        private void GetComments()
        {
            // 任务列表。
            List<Task> fiveTask = new List<Task>();
            // 声明list大小为5。
            fiveTask.Capacity = 5;
            //// 定义循环数组。
            //int[] arrayFor = new int[5];
            // 所有获取到的oid与5取余数。
            int tempindex = tempAllOid.Count % 5;
            //// oid数组。
            //List<string[]> oneOidArray = new List<string[]>();
            //// 声明容量大小。
            //oneOidArray.Capacity = 5;
            // 进行判断 5减去余数后的商加上tempAllOid的数量除5。
            if (tempindex != 0)
            {
                tempindex = ((5 - tempindex) + tempAllOid.Count) / 5;
            }
            else
            {
                tempindex = tempAllOid.Count / 5;
            }
            int tempAdd = 0;
            for (int i = 0; i < fiveTask.Capacity; i++)
            {
                fiveTask.Add(Task.Run(async () =>
                {
                    await Task.Run(new Action(() => { GetCommentJudgment(i, tempAdd, tempindex); }));
                }));
                Thread.Sleep(100);
                tempAdd += tempindex;
            }
            #region 暂时废弃。
            //for (int i = 0; i < arrayFor.Length; i++)
            //{

            //    fiveTask.Add(Task.Run(new Action(() =>
            //    {
            //        GetCommentJudgment(i, oneOidArray[i]);

            //        //if (i == 0)
            //        //{
            //        //    for (int k = 0; k < arrayFor[0]; k++)
            //        //    {
            //        //        if (k > tempAllOid.Count)
            //        //        {
            //        //            break;
            //        //        }
            //        //        if (ThereAreComments(tempAllOid[k]))
            //        //        {
            //        //            strings.Add(tempAllOid[k]);
            //        //        }
            //        //        lock (this) { AddPush(0, tempAllOid[k]); }

            //        //    }
            //        //} 

            //        //else
            //        //{
            //        //    int temp = 0;
            //        //    if (i < 5)
            //        //    {
            //        //        temp = i;
            //        //    }

            //        //    for (int k = arrayFor[temp - 1]; k < arrayFor[temp]; k++)
            //        //    {
            //        //        if (k > tempAllOid.Count)
            //        //        {
            //        //            break;
            //        //        }
            //        //        if (ThereAreComments(tempAllOid[k]))
            //        //        {
            //        //            strings.Add(tempAllOid[k]);
            //        //        }
            //        //        lock (fiveTask) { AddPush(temp, tempAllOid[k]); }

            //        //    }
            //        //}

            //    })));
            //    Thread.Sleep(100);
            //}
            #endregion
            Task.WaitAll(fiveTask.ToArray());
        }
        /// <summary>
        ///  调用方法进行判断。
        /// </summary>
        /// <param name="i">线程标识。</param>
        /// <param name="beginindex">开始数。</param>
        /// <param name="stopNumber">判断长度。</param>
        private void GetCommentJudgment(int i, int beginindex, int stopNumber)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int k = beginindex; k < beginindex + stopNumber; k++)
            {
                if (k > tempAllOid.Count - 1)
                {
                    break;
                }
                else
                {
                    ThereAreComments(tempAllOid[k]);
                    tfc.ToRicPull(i, tempAllOid[k], true);
                }
            }
            sw.Stop();
            tfc.ToRicPull(i, $"该线程耗时：{sw.Elapsed}");
            tfc.Invoke(new Action(() => Writelog(tfc.GetRichTextBox(i), $"oid第{i}列表-")));
        }
        /// <summary>
        /// 　获取历史记录。
        /// </summary>
        /// <param name="sender">触发者。</param>
        /// <param name="e">事件。</param>
        private async void GetHistoryRecord(object sender, EventArgs e)
        {
            button2.Enabled = false;
            DialogResult result = DialogResult.None;
            if (tempAllOid.Count != 0)
            {
                result = MessageBox.Show("是否清空上次收索记录。", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    tempAllOid.Clear();
                }
                else if (result == DialogResult.Cancel)
                {
                    button2.Enabled = true;
                }
            }
            if (button2.Enabled == false)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                /// <summary>
                ///  下一次请求历史记录时起始位置。
                /// </summary>
                string historyMax = "0";
                /// <summary>
                ///  向上获取还是向下获取。
                /// </summary>
                string historyView_at = "0";
                int findAllNumber = 0;
                string tempps = null;
                if (result != DialogResult.No)
                {
                    await Task.Run(new Action(() =>
                   {
                       MatchCollection resluteMatch = null;
                       string resultToBackWeb;
                       Task[] tasks = new Task[4];
                       Stopwatch getStopWatch = new Stopwatch();
                       stopwatch.Start();
                       do
                       {
                           if (resluteMatch != null)
                           {
                               resluteMatch = null;
                               for (int i = 0; i < tasks.Length; i++)
                               {
                                   tasks[i] = null;
                               }
                           }
                           // 计时开始。
                           getStopWatch.Start();
                           // resultToBackWeb = GetHttpContent($"https://api.bilibili.com/x/web-interface/history/cursor?max={historyMax}&view_at={historyView_at}&business=").Result;
                           // 获取最近30天的观看记录。
                           resultToBackWeb = HttpRequest.Get($"https://api.bilibili.com/x/web-interface/history/cursor?max={historyMax}&view_at={historyView_at}&business=", Properties.Settings.Default.userCookie);
                           // 停止计时。
                           getStopWatch.Stop();
                           // 截取下一次的起始max值。
                           historyMax = Regex.Match(resultToBackWeb, "(?<=\"max\":)\\d{3,10}(?=,\"view_at\":)").Value.ToString();
                           // 截取下一次的起始view值。
                           historyView_at = Regex.Match(resultToBackWeb, "(?<=\"view_at\":)\\d{5,10}(?=,\"business\")").Value.ToString();
                           // 截取返回页数量。
                           tempps = Regex.Match(resultToBackWeb, "(?<=\"ps\":)\\d{1,2}(?=\\},\"tab\":)").Value.ToString();
                           // 截取视频oid。
                           resluteMatch = Regex.Matches(resultToBackWeb, "(?<=\"oid\":)\\d{3,12}(?=,\"epid)");
                           // 获取截取到的数量。
                           findAllNumber += resluteMatch.Count;
                           // 富文本推送。
                           RicTextActionPut($"---{tempAllOid.Count}---\r\n返回历史数量：{findAllNumber}\r\nMax:{historyMax}\r\nView_at:{historyView_at}\r\n耗时:{getStopWatch.Elapsed.TotalSeconds}\r\n\r\n");
                           getStopWatch.Restart();
                           // task任务循环标志。
                           int tempAdd = 0;

                           // 委托，临时。
                           for (int i = 0; i < tasks.Length; i++)
                           {
                               // 为任务列表添加任务。
                               tasks[i] = Task.Run(new Action(() =>
                               {
                                   // new 新的委托。
                                   // 匿名函数，for循环判断。
                                   for (int j = tempAdd; j < tempAdd + 5; j++)
                                   {
                                       // 如果循环值大于截取的数量，跳出。
                                       if (j >= resluteMatch.Count)
                                       {
                                           break;
                                       }
                                       // 判断是否包含了，防止重复。
                                       if (tempAllOid.Contains(resluteMatch[j].Value))
                                       {
                                           // 推送，红色警告。
                                           RicTextActionPut($"已包含该oid：\r\n{resluteMatch[j].Value}", true);
                                       }
                                       else
                                       {
                                           tempAllOid.Add(resluteMatch[j].ToString());
                                       }
                                   }
                               }
                               ));
                               // 延迟。
                               Thread.Sleep(100);
                               // 延迟加。
                               tempAdd += 5;
                           }
                           try
                           {
                               // 等待所有任务完成。
                               Task.WaitAll(tasks);
                           }
                           catch (Exception)
                           {
                               Thread.Sleep(50);
                           }
                       } while (tempps != "0");
                       // 计时停止。
                       stopwatch.Stop();
                       RicTextActionPut($"查询历史记录数量:{findAllNumber}\r\n最终Ps:{tempps}\r\n 运行时间：{stopwatch.Elapsed}\r\n");
                       richTextBox1.Invoke(new Action(() => Writelog(richTextBox1)));
                   }));
                }
                // oid判断刷新界面。
                tfc.Show();
                // 计时停止。
                stopwatch.Start();
                // 判断oid。
                await Task.Run(new Action(GetComments));
                // 按钮启用。
                button2.Enabled = true;
                // 更新listoid列表。
                UpListOid(sender, e);
                // 计时停止。
                stopwatch.Stop();
                // 返回判断是否包含评论时的耗时。
                richTextBox1.AppendText($"获取评论耗时：{stopwatch.Elapsed}\r\n");
            }
        }
        private static readonly object _lock = new object();
        /// <summary>
        ///  写入log。
        /// </summary>
        /// <param name="ric">要将那个地方的ric写入log。</param>
        /// <returns></returns>
        private bool Writelog(RichTextBox ric, string expMessage = null)
        {

            using (StreamWriter write = new StreamWriter(Properties.Settings.Default.logSavePath + $"{expMessage}{ric.Name}-{DateTime.Now.ToString("yyyy-MM-dd-mm-ss")}s.txt", true, Encoding.UTF8))
            {
                try
                {
                    foreach (var item in ric.Lines)
                    {
                        write.WriteLine(item);
                    }
                    ric.SelectionColor = Color.LightSeaGreen;
                    ric.SelectedText = "++++++++\r\n写入成功\r\n++++++++\r\n";
                    ric.SelectionColor = Color.Black;
                    return true;
                }
                catch (Exception e)
                {
                    RicTextActionPut($"写入log错误：\r\n{e.Message}\r\n", true);
                    MessageBox.Show($"{e}");

                }

                return false;
            }
        }
        /// <summary>
        ///  委托推送到文本框。
        /// </summary>
        /// <param name="comment">内容。</param>
        /// <param name="convent">是否改变颜色为红色。</param>
        private void RicTextActionPut(string comment, bool convent = false)
        {
            richTextBox1.Invoke(new Action(() =>
            {
                if (convent == false)
                {
                    richTextBox1.AppendText(comment);
                }
                if (convent)
                {

                    // Select error text color.
                    // 选择错误文本的颜色。
                    richTextBox1.SelectionColor = Color.Red;
                    // 错误信息。
                    richTextBox1.SelectedText = "*-*-*-*-*-*错误*-*-*-*-*-*\r\n" + comment + "\r\n*-*-*-*-*-*结束*-*-*-*-*-*\r\n";
                }
            }
            ));

        }
        private static readonly object _okk = new object();
        /// <summary>
        ///  判断是否存在评论。
        /// </summary>
        /// <param name="oid">视频oid号。</param>
        /// <returns>包含返回true。其他则为false。</returns>
        private bool OldThereAreComments(string oid)
        {
            // 接受返回结果。
            string result = GetMd5.NewResultGetMd5(oid, "1315875");
            // 判断是否包含。
            string resultToBackWeb = HttpRequest.Get(GetMd5.NewResultGetMd5(oid, "1315875"), cookieText.Text);
            // 结束标志，设置为true。
            string is_end = "true";
            string is_name = "最新评论";
            // 正则表达式提取文字。
            string next = null;
            int all_count = 0;
            do
            { // 接受评论区信息，并判断是否包含指定mid。
                if (resultToBackWeb.Contains("\"mid\":" + Properties.Settings.Default.userMid))
                {
                    if (!findContainCommentsIsOid.ContainsKey(oid))
                    {
                        try
                        {
                            MatchCollection match = Regex.Matches(resultToBackWeb, $"(?<=\"rpid\":)\\d{{4,18}}(?=,\"oid\":{oid},\"type\":\\d{{1,2}},\"mid\":{Properties.Settings.Default.userMid},\"root\")");
                            MatchCollection CommentMatch = Regex.Matches(resultToBackWeb, $"(?<=\"mid\":\"{Properties.Settings.Default.userMid}\"}}}},\"content\":{{\"message\":\").*?(?=\",\"members\":)");

                            if (match.Count == 0 || CommentMatch.Count != match.Count)
                            {
                                RicTextActionPut($"错误：rpid与评论不相等。\r\nOid：{oid}\r\nrpid数：{match.Count}\r\n评论数：{CommentMatch.Count}", true);
                                break;
                            }
                            try
                            {
                                findContainCommentsIsOid.Add(oid, new Dictionary<string, string>());
                                try
                                {
                                    for (int i = 0; i < match.Count; i++)
                                    {
                                        try
                                        {
                                            findContainCommentsIsOid[oid].Add(match[i].ToString(), CommentMatch[i].ToString());
                                        }
                                        catch (Exception message)
                                        {

                                            RicTextActionPut($"错误代码：500\r\n内容：{message.Message}", true);
                                        }
                                    }
                                }
                                catch (Exception message)
                                {

                                    RicTextActionPut($"错误代码：527\r\n内容：{message.Message}", true);
                                }

                            }
                            catch (Exception message)
                            {

                                RicTextActionPut($"错误代码：534\r\n内容：{message.Message}", true);

                            }
                        }
                        catch (Exception message)
                        {
                            RicTextActionPut($"错误代码：542\r\n内容：{message.Message}", true);
                        }
                    }
                    // 返回。
                    return true;
                }
                try
                {
                    // 数字转换异常。
                    try
                    {

                        // 结束标志，设置为true。
                        is_end = Regex.Match(resultToBackWeb, "(?<=,\"is_end\":)\\w{4,5}(?=,\"mode\":)").Value;
                        is_name = Regex.Match(resultToBackWeb, "(?<=,\"name\":\")[\u4e00-\u9fa5]{4}(?=\",\"pagination_reply\")").Value;
                        // 是否结束，是否不等于最新评论。
                        if (is_end == "true" || is_name != "最新评论")
                        {
                            break;
                        }
                        if (all_count == 0)
                        {
                            // 获取当前全部评论数。
                            all_count = Convert.ToInt32(Regex.Match(resultToBackWeb, "(?<=\"all_count\":)\\d{0,20}(?=,\"support_mode\":)").Value);

                        }

                    }
                    catch (Exception errorMessage)
                    {
                        RicTextActionPut($"错误：594  :{oid}数字转换失败,可能未开启评论区。\r\n" + errorMessage.Message, true);
                        break;
                    }
                    // 判断是否大于200。
                    if (checkBox1.Checked && (all_count > Properties.Settings.Default.userMax))
                    {
                        break;
                    }
                    try
                    {
                        // 正则表达式提取文字。
                        next = Regex.Match(resultToBackWeb, "(?<=,\"next\":)\\d{1,10}(?=,\"is_end\":)").Value;
                        if (next == "0")
                        {
                            break;
                        }
                        // 获取网页返回值。
                        resultToBackWeb = HttpRequest.Get(GetMd5.AllResultGetMd5(oid, "1315875", next), Properties.Settings.Default.userCookie);
                    }
                    catch (Exception errorMessage)
                    {
                        RicTextActionPut("错误：615" + errorMessage.Source + "\r\n" + errorMessage.Message, true);

                    }

                }
                catch (Exception errorMessage)
                {
                    RicTextActionPut("错误：622  " + errorMessage.Source + "\r\n" + errorMessage.Message, true);

                }
            } while (is_end == "false");
            return false;

        }
        private bool ThereAreComments(string oid)
        {
            lock (_lock)
            {
                // 接受返回结果。
                string result = GetMd5.NewResultGetMd5(oid, "1315875");
                // 判断是否包含。
                string resultToBackWeb = HttpRequest.Get(GetMd5.NewResultGetMd5(oid, "1315875"), cookieText.Text);
                string is_name = "最新评论";
                // 正则表达式提取文字。
                string next = Regex.Match(resultToBackWeb, "(?<=,\"next\":)\\d{1,10}(?=,\"is_end\":)").Value;
                string prev = Regex.Match(resultToBackWeb, "(?<=,\"prev\":)\\d{0,10}(?=,\"next\":)").Value;
                int all_count = 0;
                try
                {
                    if (Regex.Match(resultToBackWeb, "(?<=\"input_disable\":)\\w{0,4}(?=,\"root_input_text\":)").Value == "true" || prev == "0"|| prev == "")
                    {
                        if (prev != "0")
                        {
                            RicTextActionPut($"oid-{oid}-错误：当前页面评论功能已关闭或up开启限制。", true);
                        }
                        return false;
                    }
                    // 获取当前全部评论数。
                    all_count = Convert.ToInt32(Regex.Match(resultToBackWeb, "(?<=\"all_count\":)\\d{0,20}(?=,\"support_mode\":)").Value);

                }
                catch (Exception error)
                {
                    RicTextActionPut($"oid-{oid}-错误：当前oid未包含任何评论。\r\n{error}\r\n", true);
                    return false;
                }
                do
                {
                    try
                    {
                        // 接受评论区信息，并判断是否包含指定mid。
                        if (resultToBackWeb.Contains("\"mid\":" + Properties.Settings.Default.userMid))
                        {
                            if (!findContainCommentsIsOid.ContainsKey(oid))
                            {
                                try
                                {
                                    MatchCollection match = Regex.Matches(resultToBackWeb, $"(?<=\"rpid\":)\\d{{4,18}}(?=,\"oid\":{oid},\"type\":\\d{{1,2}},\"mid\":{Properties.Settings.Default.userMid},\"root\")");
                                    MatchCollection CommentMatch = Regex.Matches(resultToBackWeb, $"(?<=\"mid\":\"{Properties.Settings.Default.userMid}\"}}}},\"content\":{{\"message\":\").*?(?=\",\"members\":)");

                                    if (match.Count == 0 || CommentMatch.Count != match.Count)
                                    {
                                        RicTextActionPut($"错误：rpid与评论不相等。\r\nOid：{oid}\r\nrpid数：{match.Count}\r\n评论数：{CommentMatch.Count}", true);
                                        break;
                                    }
                                    try
                                    {
                                        findContainCommentsIsOid.Add(oid, new Dictionary<string, string>());
                                        try
                                        {
                                            for (int i = 0; i < match.Count; i++)
                                            {
                                                try
                                                {
                                                    findContainCommentsIsOid[oid].Add(match[i].ToString(), CommentMatch[i].ToString());
                                                }
                                                catch (Exception message)
                                                {

                                                    RicTextActionPut($"错误代码：500\r\n内容：{message.Message}", true);
                                                }
                                            }
                                        }
                                        catch (Exception message)
                                        {

                                            RicTextActionPut($"错误代码：527\r\n内容：{message.Message}", true);
                                        }

                                    }
                                    catch (Exception message)
                                    {

                                        RicTextActionPut($"错误代码：534\r\n内容：{message.Message}", true);

                                    }
                                }
                                catch (Exception message)
                                {
                                    RicTextActionPut($"错误代码：542\r\n内容：{message.Message}", true);
                                }
                            }
                            // 返回。
                            return true;
                        }
                        else
                        {
                            if (checkBox1.Checked && all_count > Properties.Settings.Default.userMax)
                            {
                                break;
                            }
                            is_name = Regex.Match(resultToBackWeb, "(?<=,\"name\":\")[\u4e00-\u9fa5]{4}(?=\",\"pagination_reply\")").Value;
                            if (is_name == "最新评论")
                            {
                                // 正则表达式提取文字。
                                next = Regex.Match(resultToBackWeb, "(?<=,\"next\":)\\d{1,10}(?=,\"is_end\":)").Value;
                                if (next == "0" || next == "")
                                {
                                    break;
                                }
                                try
                                {
                                    // 获取网页返回值。
                                    resultToBackWeb = HttpRequest.Get(GetMd5.AllResultGetMd5(oid, "1315875", next), Properties.Settings.Default.userCookie);
                                }
                                catch (Exception error)
                                {

                                    RicTextActionPut($"获取评论内容错误:\r\n{error.Message}", true);
                                }
                            }
                            else
                            {
                                RicTextActionPut($"oid{oid}-错误：\r\n不能判断以{is_name}方式显示评论的视频。", true);
                                break;
                            }
                        }
                    }
                    catch (Exception error)
                    {

                        RicTextActionPut($"oid-{oid}-值判断错误：{error.Message}。\r\n{error}\r\n", true);

                    }

                } while (true);
            }
            return false;
        }
        /// <summary>
        ///  删除按钮。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (textComments.TextLength != 0)
            {


                DialogResult result = MessageBox.Show($"是否删除该评论：\r\n{textComments.Text}", "警告！！！", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    string crsf = Regex.Match(Properties.Settings.Default.userCookie, "(?<=bili_jct=)[A-Za-z0-9]*(?=;)").Value;
                    string webSiteResult = HttpRequest.Post("https://api.bilibili.com/x/v2/reply/del", $"oid={listBox1.SelectedItem.ToString()}&type=1&rpid={rpidList.SelectedItem.ToString()}&csrf={crsf}", Properties.Settings.Default.userCookie);
                    if (webSiteResult.Contains("message\":\"0\""))
                    {
                        MessageBox.Show($"成功：\r\n返回结果：\r\n{webSiteResult}");
                        try
                        {
                            findContainCommentsIsOid[listBox1.SelectedItem.ToString()].Remove(rpidList.SelectedItem.ToString());
                            SelectOidGetComments(sender, e);
                            if (findContainCommentsIsOid[listBox1.SelectedItem.ToString()].Count == 0)
                            {
                                try
                                {
                                    findContainCommentsIsOid.Remove(listBox1.SelectedItem.ToString());
                                    UpListOid(sender, e);
                                }
                                catch (Exception message)
                                {

                                    RicTextActionPut($"Oid删除错误：{message.Message}", true);

                                }
                            }
                        }
                        catch (Exception message)
                        {

                            RicTextActionPut($"rpid删除错误：{message.Message}", true);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"失败：\r\n返回结果：\r\n{webSiteResult}");
                    }

                }
                else
                {
                    MessageBox.Show("操作取消。");
                }
            }
            else
            {
                MessageBox.Show("请先加载评论。");
                RicTextActionPut("请先加载评论。", true);
            }
        }
        /// <summary>
        ///  窗体加载。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            propertiesName.Add(midText, "userMid");
            propertiesName.Add(maxTextbox, "userMax");
            propertiesName.Add(cookieText, "userCookie");
            foreach (var item in propertiesName)
            {
                try
                {
                    item.Key.Text = Properties.Settings.Default[item.Value].ToString();
                }
                catch (Exception error)
                {

                    RicTextActionPut($"{item.Value}值加载失败：\r\n{error}\r\n", true);

                }

            }
        }
        /// <summary>
        ///  log写入按钮。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                // 设置初始化路径。
                dialog.SelectedPath = Properties.Settings.Default.logSavePath;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (dialog.SelectedPath + "\\" != Properties.Settings.Default.logSavePath)
                        {
                            Properties.Settings.Default.logSavePath = dialog.SelectedPath.ToString() + "\\";
                            Properties.Settings.Default.Save();
                        }
                    }
                    catch (Exception message)
                    {
                        RicTextActionPut($"设置log路径错误：\r\n{message.Message}\r\n", true);
                    }

                }
            }
        }
        /// <summary>
        ///  鼠标离开控件。
        /// </summary>
        /// <param name="sender">触发者。</param>
        /// <param name="e">事件。</param>
        private void MouseFocusLeave(object sender, EventArgs e)
        {
            // 转换为 textbox控件。
            TextBox ss = (TextBox)sender;
            // 如果不想等。
            if (ss.Text != Properties.Settings.Default[propertiesName[ss]].ToString())
            {
                try
                {
                    // 是否是cookietext控件。
                    // This is not cookieText control.
                    if (ss.Name == "cookieText")
                    {
                        // 赋值。
                        Properties.Settings.Default[propertiesName[ss]] = ss.Text.ToString();
                    }
                    else
                    {
                        // 赋值。
                        Properties.Settings.Default[propertiesName[ss]] = Convert.ToInt32(ss.Text);
                    }
                    // 保存。
                    Properties.Settings.Default.Save();
                }
                catch (Exception error)
                {
                    RicTextActionPut($"******{propertiesName[ss]}值保存失败：\r\n******输入值：{ss.Text}\r\n{error}\r\n", true);
                }
            }

        }
        /// <summary>
        ///  只允许输入数字。
        /// </summary>
        /// <param name="sender">触发者。</param>
        /// <param name="e">事件。</param>
        private void OnlyInputNumeral(object sender, KeyPressEventArgs e)
        {
            // 是否是back键。
            if (e.KeyChar != '\b')
            {
                // 是否是0-9的数字。
                if (e.KeyChar < '0' || e.KeyChar > '9')
                {
                    // 让系统人为已经处理过该事件了，不在填充文字。
                    e.Handled = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            ThereAreComments("450117081");
            button1.Enabled = true;

        }
    }

}
