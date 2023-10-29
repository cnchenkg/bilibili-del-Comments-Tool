
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

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
        readonly TaskFindComments tfc = new TaskFindComments();
        /// <summary>
        ///  为propertiesName设置键值对。
        /// </summary>
        Dictionary<TextBox, string> propertiesName = new Dictionary<TextBox, string>();
        /// <summary>
        ///  判断是否包含评论的锁。
        /// </summary>
        private static readonly object _lock = new object();

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
            Stopwatch sw = Stopwatch.StartNew();
            sw.Restart();
            //GetCommentJudgment(0, 0, tempAllOid.Count);
            #region MyRegion
            // 任务列表。
            List<Task> fiveTask = new List<Task>();
            // 声明list大小为5。
            fiveTask.Capacity = 5;
            //// 定义循环数组。
            //int[] arrayFor = new int[5];
            // 所有获取到的oid与5取余数。
            int tempindex = JudgeNum(tempAllOid.Count, 5);
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
            Task.WaitAll(fiveTask.ToArray());
            #endregion
            sw.Stop();
            RicTextActionPut($"------处理耗时:{sw.Elapsed.TotalSeconds.ToString("N6")}秒\r\n");
        }
        /// <summary>
        ///  对传入数字进行均分。
        /// </summary>
        /// <param name="num">要均分的数。</param>
        /// <param name="division">均分多少份。</param>
        /// <returns>返回均分后的结果。</returns>
        private int JudgeNum(int num, int division)
        {
            // 所有获取到的oid与5取余数。
            int tempindex = num % division;
            // 进行判断 5减去余数后的商加上tempAllOid的数量除5。
            if (tempindex != 0)
            {
                return tempindex = ((division - tempindex) + num) / division;
            }
            else
            {
                return tempindex = num / division;
            }
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
            if (midText.Text != "0" && midText.Text != "")
            {
                if (cookieText.Text != "")
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
                        if (result != DialogResult.No)
                        {
                            await Task.Run(() =>
                           {
                               // 开始计时。
                               stopwatch.Restart();
                               GetHistoryVideoOid();
                               stopwatch.Stop();
                               RicTextActionPut($"获取历史记录数量:{tempAllOid.Count}\r\n 运行时间：{stopwatch.Elapsed}\r\n");
                               richTextBox1.Invoke(new Action(() => Writelog(richTextBox1)));
                           });
                        }
                        // 计时停止。
                        stopwatch.Restart();
                        // oid判断刷新界面。
                        tfc.Show();
                        // 判断oid。
                        await Task.Run(new Action(GetComments));
                        // 计时停止。
                        stopwatch.Stop();
                        // 返回判断是否包含评论时的耗时。
                        richTextBox1.AppendText($"获取评论耗时：{stopwatch.Elapsed}\r\n");
                        // 按钮启用。
                        button2.Enabled = true;
                        // 更新listoid列表。
                        UpListOid(sender, e);
                    }
                }
                else
                {
                    MessageBox.Show("请输入cookie号。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("请输入mid号。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        ///  获取评论结果。
        /// </summary>
        private void GetRemarkResult()
        {



        }
        /// <summary>
        ///  获取历史记录视频oid号
        /// </summary>
        private void GetHistoryVideoOid()
        {
            /// <summary>
            ///  下一次请求历史记录时起始位置。
            /// </summary>
            string historyMax = "0";
            /// <summary>
            ///  向上获取还是向下获取。
            /// </summary>
            string historyView_at = "0";
            // 获取视频oid结果。
            MatchCollection resultMatch = null;
            // 计时器。
            Stopwatch getStopWatch = new Stopwatch();
            // 浏览器返回结果。
            string resultToBackWeb;
            // 找到的oid总数量。
            int findAllNumber = 0;
            do
            {
                if (resultMatch != null)
                {
                    resultMatch = null;
                }
                // 计时开始。
                getStopWatch.Restart();
                // 获取最近30天的观看记录。
                resultToBackWeb = HttpRequest.Get($"https://api.bilibili.com/x/web-interface/history/cursor?max={historyMax}&view_at={historyView_at}&business=", Properties.Settings.Default.userCookie);
                // 停止计时。
                getStopWatch.Stop();
                // 截取下一次的起始max值。
                historyMax = Regex.Match(resultToBackWeb, "(?<=\"max\":)\\d{3,10}(?=,\"view_at\":)").Value.ToString();
                // 截取下一次的起始view值。
                historyView_at = Regex.Match(resultToBackWeb, "(?<=\"view_at\":)\\d{5,10}(?=,\"business\")").Value.ToString();
                // 截取视频oid。
                resultMatch = Regex.Matches(resultToBackWeb, "(?<=\"oid\":)\\d{3,12}(?=,\"epid)");
                // 获取截取到的数量。
                findAllNumber += resultMatch.Count;
                // 富文本推送。
                RicTextActionPut($"---{tempAllOid.Count}---\r\n返回历史数量：{findAllNumber}\r\nMax:{historyMax}\r\nView_at:{historyView_at}\r\n耗时:{getStopWatch.Elapsed.TotalSeconds}秒\r\n");
                // 计时开始。
                getStopWatch.Restart();
                foreach (object item in resultMatch)
                {
                    //判断是否包含了，防止重复。
                    if (tempAllOid.Contains(item.ToString()))
                    {
                        // 推送，红色警告。
                        RicTextActionPut($"已包含该oid：\r\n{item}", true);
                    }
                    else
                    {
                        tempAllOid.Add(item.ToString());
                    }
                }
                getStopWatch.Stop();
                // 富文本推送。
                RicTextActionPut($"------处理耗时:{getStopWatch.Elapsed.TotalMilliseconds.ToString("N6")}毫秒\r\n");
            } while (resultMatch.Count != 0);
        }
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
        /// <summary>
        ///  判断是否包含评论。
        /// </summary>
        /// <param name="oid">视频oid号。</param>
        /// <returns>返回结果。</returns>
        private bool ThereAreComments(string oid)
        {

            string next;
            string temp = null;
            string is_name = "最新评论";
            int all_count = 0;
            int prev = 0;
            List<string> stringList = new List<string>();
            List<Task> tasksList = new List<Task>();
            try
            {
                stringList.Add(HttpRequest.Get(GetMd5.RequestMd5(0, oid, "1315875"), cookieText.Text));
            }
            catch (Exception error)
            {
                RicTextActionPut($"oid-{oid}-错误：获取评论失败。\r\n{error}\r\n", true);
            }
            try
            {
                // 获得当前评论类型。
                is_name = Regex.Match(stringList[stringList.Count - 1], "(?<=,\"name\":\")[\u4e00-\u9fa5]{4}(?=\",\"pagination_reply\")").Value;
            }
            catch (Exception error)
            {
                RicTextActionPut($"oid-{oid}-错误：评论内容不能为空\r\n{error.Message}。", true);
            }
            if (is_name != "最新评论")
            {
                RicTextActionPut($"oid-{oid}-错误：不能以{is_name}方式获取评论。", true);
                return false;
            }
            try
            {
                // 获取当前全部评论数。
                all_count = Convert.ToInt32(Regex.Match(stringList[stringList.Count - 1], "(?<=\"all_count\":)\\d{0,20}(?=,\"support_mode\":)").Value);
            }
            catch (Exception error)
            {
                RicTextActionPut($"oid-{oid}-错误：当前oid未包含任何评论。\r\n{error}\r\n", true);
                return false;
            }
            if ((checkBox1.Checked && all_count > Properties.Settings.Default.userMax) || all_count == 0)
            {
                if (all_count == 0)
                {
                    try
                    {
                        RicTextActionPut($"oid-{oid}-错误：当前oid评论数为{all_count}。\r\n", true);

                    }
                    catch (Exception error)
                    {
                        RicTextActionPut($"oid-{oid}-错误：{error}。\r\n", true);
                    }
                }
                else
                {
                    try
                    {
                        RicTextActionPut($"oid-{oid}-错误：当前oid评论超出设定值，返回评论数{all_count}。\r\n", true);
                    }
                    catch (Exception error)
                    {

                        RicTextActionPut($"oid-{oid}-错误：{error}。\r\n", true);
                    }
                }
                return false;
            }
            try
            {
                prev = Convert.ToInt32(Regex.Match(stringList[stringList.Count - 1], "(?<=,\"prev\":)\\d{1,10}(?=,\"next\":)").Value);

            }
            catch (Exception error)
            {
                RicTextActionPut($"错误：不能将string类型转为int。\r\n{error}\r\n", true);
                return false;
            }
            int tempindex = JudgeNum(prev, 4);
            Stopwatch ss = new Stopwatch();
            ss.Start();
           
                    do
                    {
                        next = Regex.Match(stringList[stringList.Count - 1], "(?<=,\"next\":)\\d{1,10}(?=,\"is_end\":)").Value;
                        stringList.Add(HttpRequest.Get(GetMd5.RequestMd5(1, oid, "1315875", next), cookieText.Text));
                    } while (next!="0");
                   
            //do
            //{

            //    // 正则表达式提取文字。
            //    next = Regex.Match(stringList[stringList.Count - 1], "(?<=,\"next\":)\\d{1,10}(?=,\"is_end\":)").Value;
            //    stringList.Add(HttpRequest.Get(GetMd5.RequestMd5(1, oid, "1315875", next), cookieText.Text));
            //} while (next != "0");
            ss.Stop();
            RicTextActionPut($"评论耗时：{ss.Elapsed.Seconds}秒", true);
            ss.Restart();
            JudgeRemark(stringList, oid);
            ss.Stop();
            RicTextActionPut($"耗时：{ss.Elapsed.Milliseconds}毫秒", true);
            // MessageBox.Show("sdss");
            #region 临时屏蔽
            //try
            //{
            //    if (Regex.Match(resultToBackWeb, "(?<=\"input_disable\":)\\w{0,4}(?=,\"root_input_text\":)").Value == "true" || prev == "0" || prev == "")
            //    {
            //        if (prev != "0")
            //        {
            //            RicTextActionPut($"oid-{oid}-错误：当前页面评论功能已关闭或up开启限制。", true);
            //        }
            //        return false;
            //    }
            //    // 获取当前全部评论数。
            //    all_count = Convert.ToInt32(Regex.Match(resultToBackWeb, "(?<=\"all_count\":)\\d{0,20}(?=,\"support_mode\":)").Value);

            //}
            //catch (Exception error)
            //{
            //    RicTextActionPut($"oid-{oid}-错误：当前oid未包含任何评论。\r\n{error}\r\n", true);
            //    return false;
            //}
            //do
            //{
            //    try
            //    {
            //        // 接受评论区信息，并判断是否包含指定mid。
            //        if (resultToBackWeb.Contains("\"mid\":" + Properties.Settings.Default.userMid))
            //        {
            //            if (!findContainCommentsIsOid.ContainsKey(oid))
            //            {
            //                try
            //                {
            //                    // 获取rpid。
            //                    MatchCollection match = Regex.Matches(resultToBackWeb, $"(?<=\"rpid\":)\\d{{4,18}}(?=,\"oid\":{oid},\"type\":\\d{{1,2}},\"mid\":{Properties.Settings.Default.userMid},\"root\")");
            //                    // 获取评论。
            //                    MatchCollection CommentMatch = Regex.Matches(resultToBackWeb, $"(?<=\"mid\":\"{Properties.Settings.Default.userMid}\"}}}},\"content\":{{\"message\":\").*?(?=\",\"members\":)");
            //                    // 判断评论id与评论是否相等。
            //                    if (match.Count == 0 || CommentMatch.Count != match.Count)
            //                    {
            //                        RicTextActionPut($"错误：rpid与评论不相等。\r\nOid：{oid}\r\nrpid数：{match.Count}\r\n评论数：{CommentMatch.Count}", true);
            //                        break;
            //                    }
            //                    try
            //                    {
            //                        findContainCommentsIsOid.Add(oid, new Dictionary<string, string>());
            //                        try
            //                        {
            //                            for (int i = 0; i < match.Count; i++)
            //                            {
            //                                try
            //                                {
            //                                    findContainCommentsIsOid[oid].Add(match[i].ToString(), CommentMatch[i].ToString());
            //                                }
            //                                catch (Exception message)
            //                                {

            //                                    RicTextActionPut($"错误代码：500\r\n内容：{message.Message}", true);
            //                                }
            //                            }
            //                        }
            //                        catch (Exception message)
            //                        {

            //                            RicTextActionPut($"错误：获取评论时出错。\r\n内容：{message.Message}", true);
            //                        }

            //                    }
            //                    catch (Exception message)
            //                    {

            //                        RicTextActionPut($"错误代码：534\r\n内容：{message.Message}", true);

            //                    }
            //                }
            //                catch (Exception message)
            //                {
            //                    RicTextActionPut($"错误代码：542\r\n内容：{message.Message}", true);
            //                }
            //            }
            //            // 返回。
            //            return true;
            //        }
            //        else
            //        {
            //            if (checkBox1.Checked && all_count > Properties.Settings.Default.userMax)
            //            {
            //                break;
            //            }
            //            is_name = Regex.Match(resultToBackWeb, "(?<=,\"name\":\")[\u4e00-\u9fa5]{4}(?=\",\"pagination_reply\")").Value;
            //            if (is_name == "最新评论")
            //            {
            //                // 正则表达式提取文字。
            //                next = Regex.Match(resultToBackWeb, "(?<=,\"next\":)\\d{1,10}(?=,\"is_end\":)").Value;
            //                if (next == "0" || next == "")
            //                {
            //                    break;
            //                }
            //                try
            //                {
            //                    // 获取网页返回值。
            //                    resultToBackWeb = HttpRequest.Get(GetMd5.RequestMd5(1, oid, "1315875", next), Properties.Settings.Default.userCookie);
            //                }
            //                catch (Exception error)
            //                {

            //                    RicTextActionPut($"获取评论内容错误:\r\n{error.Message}", true);
            //                }
            //            }
            //            else
            //            {
            //                RicTextActionPut($"oid{oid}-错误：\r\n不能判断以{is_name}方式显示评论的视频。", true);
            //                break;
            //            }
            //        }
            //    }
            //    catch (Exception error)
            //    {
            //        RicTextActionPut($"oid-{oid}-值判断错误：{error.Message}。\r\n{error}\r\n", true);
            //    }

            //} while (true);
            #endregion
            return false;
        }
        /// <summary>
        ///  判断评论。
        /// </summary>
        /// <param name="remark">评论。</param>
        /// <param name="oid">视频oid。</param>
        private void JudgeRemark(List<string> remark, string oid)
        {
            try
            {
                if (!findContainCommentsIsOid.ContainsKey(oid))
                {
                    MatchCollection match;
                    MatchCollection CommentMatch;
                    foreach (var item in remark)
                    {
                        // 接受评论区信息，并判断是否包含指定mid。
                        if (item.Contains("\"mid\":" + Properties.Settings.Default.userMid))
                        {

                            try
                            {
                                // 获取rpid。
                                match = Regex.Matches(item, $"(?<=\"rpid\":)\\d{{4,18}}(?=,\"oid\":{oid},\"type\":\\d{{1,2}},\"mid\":{Properties.Settings.Default.userMid},\"root\")");
                                // 获取评论。
                                CommentMatch = Regex.Matches(item, $"(?<=\"mid\":\"{Properties.Settings.Default.userMid}\"}}}},\"content\":{{\"message\":\").*?(?=\",\"members\":)");
                                // 判断评论id与评论是否相等。
                                if (match.Count == 0 || CommentMatch.Count != match.Count)
                                {
                                    RicTextActionPut($"错误：rpid与评论不相等。\r\nOid：{oid}\r\nrpid数：{match.Count}\r\n评论数：{CommentMatch.Count}", true);
                                }
                                try
                                {
                                    findContainCommentsIsOid.Add(oid, new Dictionary<string, string>());
                                    try
                                    {
                                        // 循环取值。
                                        for (int i = 0; i < match.Count; i++)
                                        {
                                            try
                                            {
                                                // 添加评论。
                                                findContainCommentsIsOid[oid].Add(match[i].ToString(), CommentMatch[i].ToString());
                                            }
                                            catch (Exception message)
                                            {

                                                RicTextActionPut($"oid{oid}错误：未能成功保存值。\r\n内容：{message.Message}", true);
                                            }
                                        }
                                    }
                                    catch (Exception message)
                                    {

                                        RicTextActionPut($"oid{oid}错误：取出评论时出错。\r\n内容：{message.Message}", true);
                                    }
                                }
                                catch (Exception message)
                                {
                                    RicTextActionPut($"oid{oid}错误：未能成功创建字典。\r\n内容：{message.Message}", true);
                                }
                            }
                            catch (Exception message)
                            {
                                RicTextActionPut($"{oid}错误：未能正确获取评论与rpid。\r\n内容：{message.Message}", true);
                            }
                        }
                    }
                }
            }
            catch (Exception error)
            {
                RicTextActionPut($"oid-{oid}-值判断错误：{error.Message}。\r\n{error}\r\n", true);
            }
            #region MyRegion
            //try
            //{
            //    // 接受评论区信息，并判断是否包含指定mid。
            //    if (remark.Contains("\"mid\":" + Properties.Settings.Default.userMid))
            //    {
            //        if (!findContainCommentsIsOid.ContainsKey(oid))
            //        {
            //            try
            //            {
            //                // 获取rpid。
            //                MatchCollection match = Regex.Matches(remark, $"(?<=\"rpid\":)\\d{{4,18}}(?=,\"oid\":{oid},\"type\":\\d{{1,2}},\"mid\":{Properties.Settings.Default.userMid},\"root\")");
            //                // 获取评论。
            //                MatchCollection CommentMatch = Regex.Matches(remark, $"(?<=\"mid\":\"{Properties.Settings.Default.userMid}\"}}}},\"content\":{{\"message\":\").*?(?=\",\"members\":)");
            //                // 判断评论id与评论是否相等。
            //                if (match.Count == 0 || CommentMatch.Count != match.Count)
            //                {
            //                    RicTextActionPut($"错误：rpid与评论不相等。\r\nOid：{oid}\r\nrpid数：{match.Count}\r\n评论数：{CommentMatch.Count}", true);
            //                }
            //                try
            //                {
            //                    findContainCommentsIsOid.Add(oid, new Dictionary<string, string>());
            //                    try
            //                    {
            //                        // 循环取值。
            //                        for (int i = 0; i < match.Count; i++)
            //                        {
            //                            try
            //                            {
            //                                // 添加评论。
            //                                findContainCommentsIsOid[oid].Add(match[i].ToString(), CommentMatch[i].ToString());
            //                            }
            //                            catch (Exception message)
            //                            {

            //                                RicTextActionPut($"oid{oid}错误：未能成功保存值。\r\n内容：{message.Message}", true);
            //                            }
            //                        }
            //                    }
            //                    catch (Exception message)
            //                    {

            //                        RicTextActionPut($"oid{oid}错误：取出评论时出错。\r\n内容：{message.Message}", true);
            //                    }
            //                }
            //                catch (Exception message)
            //                {
            //                    RicTextActionPut($"oid{oid}错误：未能成功创建字典。\r\n内容：{message.Message}", true);
            //                }
            //            }
            //            catch (Exception message)
            //            {
            //                RicTextActionPut($"{oid}错误：未能正确获取评论与rpid。\r\n内容：{message.Message}", true);
            //            }
            //        }
            //    }
            //}
            //catch (Exception error)
            //{
            //    RicTextActionPut($"oid-{oid}-值判断错误：{error.Message}。\r\n{error}\r\n", true);
            //}
            #endregion

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
            // 添加资源信息。
            propertiesName.Add(midText, "userMid");
            propertiesName.Add(maxTextbox, "userMax");
            propertiesName.Add(cookieText, "userCookie");
            // 循环获取。
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
            // creat Dicrectory。
            // 创建目录。
            if (!Directory.Exists(Properties.Settings.Default.logSavePath))
            {
                Directory.CreateDirectory(Properties.Settings.Default.logSavePath);
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
            // 如果不相等。
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
                    // 让系统认为已经处理过该事件了，不在填充文字。
                    e.Handled = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ThereAreComments("317775239");
            sw.Stop();
            RicTextActionPut($"耗时：{sw.Elapsed.Seconds}", true);
            button1.Enabled = true;

        }
    }

}

